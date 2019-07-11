using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour {

	public int totalThrowsInActivation = 30;
	public float delayInBetweenThrows = 0.1f;
	public float speed = 100;
	public float range = 5;
	IEnumerator Looper;
	public Color color;


	void OnTriggerEnter(Collider other)
	{
		if (other.name == "BossActivator")
			Init ();
	}
	public void Init()
	{
		Looper = Loop();
		StartCoroutine (Looper);
	}
	void OnDisable(){
		if(Looper != null)
		StopCoroutine (Looper);
	}
	IEnumerator Loop()
	{
		int num = totalThrowsInActivation;
		while(num>0)
		{
			yield return new WaitForSeconds(delayInBetweenThrows);
			SceneObject newSceneObject;
			Vector3 f;
			newSceneObject = Data.Instance.sceneObjectsPool.GetObjectForType("ThrowableSceneObject_real", false);  
			Game.Instance.sceneObjectsManager.AddSceneObjectAndInitIt(newSceneObject, transform.position);
			newSceneObject.ChangeColor (color);
			newSceneObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			f = transform.forward * speed * -1;
			f.y = Random.Range (-range, range);
			newSceneObject.GetComponent<Rigidbody> ().AddForce (f,ForceMode.Impulse);
			num--;
		}
		yield return null;
	}
}
