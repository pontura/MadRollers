using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaySceneObject : SceneObject {
	
	public GameObject rayObject;
	bool hasBrokenFloor;

	public override void OnRestart(Vector3 pos)
	{
		base.OnRestart(pos);
		rayObject.SetActive (false); 
		StartCoroutine (ActionsToBeDone ());
		hasBrokenFloor = false;
	}
	IEnumerator ActionsToBeDone()
	{
		yield return new WaitForSeconds (1.5f);
		Data.Instance.events.OnBossDropRay ((int)transform.localPosition.x);
		yield return new WaitForSeconds (0.5f);
		float randomValue = 20;
		rayObject.transform.localEulerAngles = new Vector3 (Random.Range (-randomValue, randomValue), 0, Random.Range (-randomValue, randomValue));
		rayObject.SetActive (true);      
	}
	public override void OnPool()
	{
		StopAllCoroutines ();
		rayObject.SetActive (false);
	}
	public override void onDie()
	{
		addExplotion(0.2f);   
	}
	void addExplotion(float _y)
	{
		if (hasBrokenFloor) return;
		hasBrokenFloor = true;
		Game.Instance.level.OnAddExplotion(transform.position, 6, Color.red);
	}
}
