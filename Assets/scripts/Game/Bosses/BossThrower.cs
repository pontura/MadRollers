using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThrower : Boss {

	bool canAddEnemies;
	public int totalArms;

	public override void OnRestart(Vector3 pos)
	{
		base.OnRestart (pos);
		Data.Instance.events.OnBossSetNewAsset ("helicopter");
		Data.Instance.events.OnBossSetTimer (40);
		SetTotal (8);
	}

	void Update()
	{
		if (!isActive)
			return;
		float avatarsDistance = Game.Instance.level.charactersManager.getDistance ();
		if (avatarsDistance + distance_from_avatars < transform.localPosition.z)
			return;
		if (!canAddEnemies)
			canAddEnemies = true;
		float _z = avatarsDistance + distance_from_avatars;

		Vector3 pos = transform.localPosition;
		pos.z = _z;
		transform.localPosition = pos;
	} 
	public void AddEnemy(Vector3 pos)	
	{
		if (!canAddEnemies)
			return;
		pos.x = 0;
		pos.z -= 0;
		pos.y += 0.35f;
		SceneObject sceneObject = Data.Instance.sceneObjectsPool.GetObjectForType( "flyer_real", false);  

		print ("AddEnemy " + pos + " flyer");
		if (sceneObject) {
			Game.Instance.sceneObjectsManager.AddSceneObjectAndInitIt(sceneObject, pos);
		}
	}
	public override void OnPartBroken(BossPart part)
	{
		breakOut ();
	}

}
