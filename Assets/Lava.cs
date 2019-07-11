using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : SceneObject {

	public Animation anim;

	void OnEnable()
	{
		Vector3 r = anim.transform.localEulerAngles;
		r.y += 90 * Random.Range (1,10);
		anim.transform.localEulerAngles = r;
		anim.Play ();
	}
	void OnDisable()
	{
		anim.Stop ();
	}
}
