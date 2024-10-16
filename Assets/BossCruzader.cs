using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCruzader : Boss {

	float timeToCreateEnemy = 1.5f;
	float actualTime = 0;
	public float timeToConvert;
	public float timer;
	public Animator anim;
	public states state;
	public ComeClose comeClose;
	//public Move move;
	public enum states
	{
		MOVEING,
		COME_CLOSE,
		DEATH
	}
	public override void OnRestart(Vector3 pos)
	{
		Data.Instance.events.OnBossSetNewAsset ("cruzaderhead");
		Data.Instance.events.OnBossSetTimer (45);
		base.OnRestart (pos);
		SetTotal (10);
	}
	void Update()
	{
		if (!isActive)
			return;
		if (state == states.DEATH)
			return;
		float avatarsDistance = Game.Instance.level.charactersManager.getDistance ();
		if (avatarsDistance + distance_from_avatars < transform.localPosition.z)
			return;
		float _z = avatarsDistance + distance_from_avatars;

		timer += Time.deltaTime;
		if (timer >= timeToConvert) {
			if (state == states.MOVEING) {
				ConvertToComeFront ();
			} 
			timer = 0;
		}
		if (state == states.MOVEING) {
			Vector3 pos = transform.localPosition;
			pos.z = _z;
			transform.localPosition = pos;
			actualTime += Time.deltaTime;
			if (actualTime > timeToCreateEnemy) {
				actualTime = 0;
				AddEnemy ();
			}
		} else {
			comeClose.OnUpdate (_z);
		}

	} 
	void ConvertToComeFront()
	{
		actualTime = 0;
		//move.enabled = false;
		comeClose.enabled = true;
		comeClose.Init (distanceFromCharacter);
		state = states.COME_CLOSE;
		timer = 0;
		RunAttack ();
	}
	public void ConvertToMove()
	{
		comeClose.enabled = false;
		//move.enabled = true;
		state = states.MOVEING;
		timer = 0;
		Run ();
	}
	void AddEnemy()
	{
		SceneObject sceneObject = Data.Instance.sceneObjectsPool.GetObjectForType( "enemyFrontal_real", false);  
		if (sceneObject) {
			sceneObject.isActive = false;
			Vector3 pos = transform.position;
			pos.z -= 4;
			pos.y += 2;
			Game.Instance.sceneObjectsManager.AddSceneObject(sceneObject, pos);
			//sceneObject.Restart(pos);
			EnemyRunnerBehavior comp = sceneObject.GetComponent<EnemyRunnerBehavior> ();
			if (comp != null)
				comp.speed = 2;
			else {
				EnemyRunnerBehavior newComp = sceneObject.gameObject.AddComponent <EnemyRunnerBehavior>() as EnemyRunnerBehavior;
				newComp.speed = 2;
			}
		}
	}
	void RunAttack()
	{
		gameObject.tag = "firewall";
		anim.Play ("run_attack");
	}
	void Run()
	{
		gameObject.tag = "boss";
		anim.Play ("run");
	}
	void Continue()
	{
		if (state == states.MOVEING)
			Run ();
		else if (state == states.COME_CLOSE)
			RunAttack ();
	}
	public override void Hit()
	{
		anim.Play ("hit");
		Invoke ("Continue", 1);
	}
	public override void Death()
	{
		//Death ();
		anim.Play ("death");
	}

}
