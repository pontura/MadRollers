using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelayed : MonoBehaviour {

	Animation anim;
	public float delay;

	void Start () {
		anim = GetComponent<Animation> ();
		anim[anim.clip.name].time = delay;
	}
}
