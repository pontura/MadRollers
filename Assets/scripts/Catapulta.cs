using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapulta : SceneObject {
	
	public GameObject throwItem;

	public Animation anim;
	CharacterBehavior characterBehavior; 
	bool attacked;
	public override void OnRestart(Vector3 pos)
	{
		base.OnRestart(pos);
	}

	void OnTriggerEnter(Collider other) 
	{
		if (!isActive)
			return;
		if (attacked)
			return;
		switch (other.tag)
		{
		case "Player":
			characterBehavior = other.transform.GetComponent<CharacterBehavior> ();
			if (characterBehavior != null) {
				if (throwItem == null)
					return;

				throwItem.SetActive (false);

				anim.Play ("catapulta");
				attacked = true;
				characterBehavior.SuperJumpByBumped ((int)2 * 100, 0.5f, false);
				Vector3 pos = transform.position;
				pos.y += 6;
				pos.z += 2;
				Shoot (pos, transform.localEulerAngles.y);
			}
			break;
		}
	}
	void Shoot(Vector3 pos, float RotationY)
	{
		Projectil projectil = ObjectPool.instance.GetObjectForType("BombThrowed", false) as Projectil;

		if (projectil)
		{
			projectil.playerID = -1;
			projectil.SetColor(Color.black);

			Game.Instance.sceneObjectsManager.AddSceneObject(projectil, pos);

			Vector3 rot = transform.localEulerAngles;
			rot.x = -20;
			rot.y = RotationY;

			projectil.transform.localEulerAngles = rot;
		}
		else
		{
			print("no hay projectil");
		}
	}
}
