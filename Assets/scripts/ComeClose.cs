using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComeClose : MonoBehaviour {

	public float speed = 20;
	public float howMuchClose;
	public states state;

	public enum states
	{
		PREPARE_TO_ATTACK,
		WAITING,
		GO_FRONT,
		STAY,
	 	GO_BACK
	}
	public void Init(float howMuchClose)
	{
		offset = 0;
		state = states.PREPARE_TO_ATTACK;
		this.howMuchClose = howMuchClose;
	}
	public float offset;
	public void OnUpdate(float _z)
	{
		Vector3 pos = transform.localPosition;
		pos.x = 0;
		pos.z = _z + offset;

		if (state == states.PREPARE_TO_ATTACK) {
			if (Mathf.Abs (pos.x) < 0.05f) {
				state = states.WAITING;
				Invoke ("ComeFront", 2);
			}
			else if(pos.x>0) pos.x -= Time.deltaTime * (speed/4);
			else if(pos.x<0) pos.x += Time.deltaTime * (speed/4);
		} else if (state == states.GO_FRONT) {
			if (offset > -howMuchClose)
				offset -= Time.deltaTime * speed;
			else
				state = states.GO_BACK;
		}
		else if (state == states.GO_BACK) {
			if (offset < 0)
				offset += Time.deltaTime * (speed/4);
			else {				
				GetComponent<BossCruzader> ().ConvertToMove ();
			}
		}
		transform.localPosition = pos;
	}
	void ComeFront()
	{
		state = states.GO_FRONT;
	}
}
