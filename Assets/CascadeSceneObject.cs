using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadeSceneObject : SceneObject {

	int num = 0;
	Coroutine Looper;
	public override void OnRestart(Vector3 pos)
	{
		base.OnRestart (pos);
		num = 0;
		Looper = StartCoroutine (Loop());
	}
	public override void OnPool()
	{
		if (Looper == null)
			return;
		StopCoroutine (Looper);
	}
	IEnumerator Loop()
	{
		int num = 100;
		while(num>0)
		{
			yield return new WaitForSeconds(0.07f);
			SceneObject newSceneObject;
			Vector3 f;
		//		if (Data.Instance.playMode == Data.PlayModes.VERSUS && num%4==0) {
		//			 newSceneObject = Data.Instance.sceneObjectsPool.GetObjectForType("ThrowableSceneObject_real", false);  
		//			//newSceneObject.OnRestart (transform.position);
		//			Game.Instance.sceneObjectsManager.AddSceneObjectAndInitIt(newSceneObject, transform.position);
		//			newSceneObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		//			f = transform.forward * 100;
		//			f.y = Random.Range (-5, 5);
		//			newSceneObject.GetComponent<Rigidbody> ().AddForce (f,ForceMode.Impulse);
		//			return;
		//		}
			 newSceneObject = Data.Instance.sceneObjectsPool.GetObjectForType("ThrowableSceneObject_real", false);  
			Game.Instance.sceneObjectsManager.AddSceneObjectAndInitIt(newSceneObject, transform.position);
			newSceneObject.ChangeColor (Color.red);
			newSceneObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			f = transform.forward * 100;
			f.y = Random.Range (-5, 5);
			newSceneObject.GetComponent<Rigidbody> ().AddForce (f,ForceMode.Impulse);
			num--;
		}
		yield return null;
	}
}
