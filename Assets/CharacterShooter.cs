using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooter : MonoBehaviour {
	
	public GameObject myProjectile;
	public CharacterBehavior characterBehavior;
	float lastShot = 0;
	float timePressing;
	public Weapon.types weawponType;
	public Missil weapon;
//	bool isLoadingGun;

	void Start()
	{
		ResetWeapons ();
		Data.Instance.events.OnChangeWeapon += OnChangeWeapon;
	}
	void OnDestroy()
	{
		Data.Instance.events.OnChangeWeapon -= OnChangeWeapon;
	}
	public void ResetWeapons()
	{
		weapon.ResetAll ();
		weawponType = Weapon.types.SIMPLE;
	}
	void OnChangeWeapon(int playerID, Weapon.types type)
	{     
		if (playerID != characterBehavior.player.id) 
			return; 

		this.weawponType = type;
		
		weapon.OnChangeWeapon(type);
	}
	public void ChangeNextWeapon()
	{
		if (!Data.Instance.isArcadeMultiplayer)
			SetFire (Weapon.types.TRIPLE, 0.7f);
		else {
			Weapon.types nextWeapon;
			if (weawponType == Weapon.types.SIMPLE)
				nextWeapon = Weapon.types.TRIPLE;
			else
				nextWeapon = Weapon.types.SIMPLE;
		
			Data.Instance.events.OnChangeWeapon (characterBehavior.player.id, nextWeapon);
		}
	}
	public void StartPressingFire(){
		//isLoadingGun = true;
		timePressing = Time.time;
		weapon.OnChangeWeapon (Weapon.types.SIMPLE);
	}
	public void CheckFireDouble()
	{
		//SetFire (Weapon.types.DOUBLE, 0.45f);
		SetFire (Weapon.types.SIMPLE, 0.45f);
	}
	public void CheckFire()
	{
		if(Data.Instance.isArcadeMultiplayer)
			SetFire (weawponType, 0.3f);
		else
			SetFire (Weapon.types.SIMPLE, 0.3f);
	}
	public void SetFire(Weapon.types weawponType, float delay)
	{

		if(!characterBehavior.controls.isAutomata)
			Data.Instance.events.OnAvatarShoot (characterBehavior.player.id);

		if (Game.Instance.state !=  Game.states.PLAYING)
			return;
		
		//isLoadingGun = false;

		if(lastShot+delay > Time.time) return;

	//	ResetWeapons ();


		if (characterBehavior.state != CharacterBehavior.states.RUN && characterBehavior.state != CharacterBehavior.states.SHOOT && transform.localPosition.y<6)
			GetComponent<Rigidbody>().AddForce(new Vector3(0, 400, 0), ForceMode.Impulse);

		characterBehavior.state = CharacterBehavior.states.SHOOT;

		if (characterBehavior.madRoller)
			characterBehavior.madRoller.Play("shoot");

		characterBehavior.shooter.weapon.Shoot();
		Data.Instance.events.OnSoundFX("fire", characterBehavior.player.id);

		lastShot = Time.time;

		Vector3 pos = new Vector3(transform.position.x, transform.position.y+2.5f, transform.position.z+4f);

		OnShoot (pos, weawponType);

		Invoke("ResetShoot", delay);
	}
	void OnShoot(Vector3 pos, Weapon.types type)
	{
		

		float offsetY = characterBehavior.transform.localEulerAngles.y;
		switch (type)
		{
		case Weapon.types.SIMPLE:
			Shoot(pos, offsetY);
			break;
		case Weapon.types.DOUBLE:
			Shoot(new Vector3(pos.x+1, pos.y, pos.z),-20 + offsetY);
			Shoot(new Vector3(pos.x-1, pos.y, pos.z), 20 + offsetY);
			break;
		case Weapon.types.TRIPLE:
			Shoot(pos, 0);
			Shoot(new Vector3(pos.x + 1, pos.y, pos.z), -20 + offsetY);
			Shoot(new Vector3(pos.x - 1, pos.y, pos.z), 20 + offsetY);
			break;
		}

	}
	void Shoot(Vector3 pos, float RotationY)
	{
		Projectil projectil = ObjectPool.instance.GetObjectForType(myProjectile.name, true) as Projectil;

		if (projectil)
		{
			projectil.playerID = characterBehavior.player.id;
			projectil.SetColor(characterBehavior.player.color);

			Game.Instance.sceneObjectsManager.AddSceneObjectAndInitIt(projectil, pos);
			projectil.team_for_versus = characterBehavior.team_for_versus;
			Vector3 rot = transform.localEulerAngles;
			rot.x = characterBehavior.madRoller.transform.eulerAngles.x;

//			if (characterBehavior.team_for_versus > 1) {
//				rot.y += 180;
//			}
//			else
				rot.y = RotationY;

			projectil.transform.localEulerAngles = rot;
		}
		else
		{
			print("no hay projectil");
		}
	}
	void ResetShoot()
	{
		if (characterBehavior.state == CharacterBehavior.states.DEAD)
			return;
		if (characterBehavior.grounded)
			characterBehavior.Run();
//		else if(characterBehavior.jumpsNumber<2)
//			characterBehavior.state = CharacterBehavior.states.JUMP;
		//else
		//	characterBehavior.state = CharacterBehavior.states.DOUBLEJUMP;
	}
}
