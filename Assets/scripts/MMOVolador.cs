using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMOVolador : SceneObject {

	public float Distance_to_react;
	public Animation anim;
	bool isOn;
	bool isPreparing;
	float initial_speed=2.5f;
	float speed;
	float acceleration = 20f;

	public override void OnRestart(Vector3 pos)
	{
		anim.Play ("gargola_idle");
		speed = initial_speed;
		isOn = false;
		isPreparing = false;
		base.OnRestart (pos);
		GetComponent<Rigidbody> ().useGravity = true;
	}
	void Update()
	{
		if (!isActive)
			return;
		if (!isPreparing && distanceFromCharacter < Distance_to_react + 15) {
			anim.Play ("gargola_prepare");
			isPreparing = true;
		} else if ( distanceFromCharacter < Distance_to_react) {
			
			if(!isOn)
			{
				GetComponent<Rigidbody> ().useGravity = false;
				isOn = true;
				anim.Play ("gargola_fly");
			}
			speed += Time.deltaTime*acceleration;

			Vector3 pos = transform.localPosition;
			pos += -transform.forward * Time.deltaTime * speed;
			pos.y +=  Time.deltaTime * (speed/2);
			transform.localPosition = pos;
		}
	}
}
