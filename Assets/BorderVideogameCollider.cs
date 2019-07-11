using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderVideogameCollider : MonoBehaviour {

	public GameObject border;
	public Transform[] smallPlatformsTransform;
	bool setInactive;
	Collider colliders;

	public void Init()
	{
		colliders = GetComponent<Collider> ();
		colliders.enabled = true;
		setInactive = false;
		Invoke("CheckCollisionDone", 0.5f);		
	}
	void CheckCollisionDone()
	{
		if (!setInactive) {
			SceneObjectsManager manager = Game.Instance.sceneObjectsManager;

			foreach (Transform so in smallPlatformsTransform) {
				SceneObject newSceneObject = Data.Instance.sceneObjectsPool.GetObjectForType("smallBlock1_real", true);  

				if (newSceneObject != null) 
					manager.AddSceneObject (newSceneObject, so.position);
			}
		}
		colliders.enabled = false;

	}
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "floor") {
			setInactive = true;
			colliders.enabled = false;
		}
	}
	void OnDisable()
	{
		CancelInvoke();
	}
	
}
