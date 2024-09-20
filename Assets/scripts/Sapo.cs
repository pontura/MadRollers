using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sapo : SceneObject {

	public Animator anim;
	Collider collider;
	Rigidbody rb;

	public override void OnRestart(Vector3 pos)
	{
		base.OnRestart( pos);
		collider = GetComponent<Collider> ();
		rb = GetComponent<Rigidbody> ();
		anim.Play ("sapo");		
		collider.enabled = true;
		rb.isKinematic = true;
		rb.useGravity = false;
	}
	void OnEnable()
	{
		anim.gameObject.layer = 17;
		if(anim != null)
			anim.Play ("sapo");
		CancelInvoke ();
	}
	void OnTriggerEnter(Collider other) 
	{
		switch (other.tag)
		{
		case "Player":
			CharacterCollisions ch = other.transform.GetComponent<CharacterCollisions> ();
			if (ch != null) {
				Jump ();
			}
			break;
		case "projectil":
			anim.gameObject.layer = 18;
			Jump ();
				
			break;
		}
	}
	void Jump()
	{
		Invoke ("AddGravity", 1);

		collider.enabled = false;
		if(anim != null)
			anim.Play ("sapoJump");		
	}
	void AddGravity()
	{
		if (rb == null)
			return;
		rb.isKinematic = false;
		rb.useGravity = true;
	}
}
