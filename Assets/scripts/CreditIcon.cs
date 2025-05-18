using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditIcon : MonoBehaviour {

	public Animation anim;

	public void SetOff()
	{
		anim.Play ("creditsLose");
		Invoke ("RemoveThis", 1.5f);
	}
	void RemoveThis()
	{
		Destroy (gameObject);
	}
}
