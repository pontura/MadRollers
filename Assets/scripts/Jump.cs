using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {

    public float delayToJump;
	Animation anim;

    void OnEnable()
    {
		Invoke ("Hack_wait", 0.1f);       
    }
	void Hack_wait()
	{
		anim = GetComponentInChildren<Animation> ();
		anim.Stop ();
		Invoke ("Delayed", delayToJump);       
	}
    void OnDisable()
    {
		if(anim != null)
			anim.Stop ();
    }
	void Delayed()
	{
		if (anim != null) {
			anim.Rewind ();
			anim.Play ();
		}
	}
}
