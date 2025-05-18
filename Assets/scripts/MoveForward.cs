using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour {

	public float speed = 2;
	public float randomSpeedDiff  = 0;

	bool isOn;
	public float moveBackIn = 0;

	private float realSpeed;

	void Start()
	{
		if (randomSpeedDiff != 0)
			realSpeed = speed + Random.Range(0, randomSpeedDiff);
		else
			realSpeed = speed;

		if (moveBackIn > 0)
			Invoke("LoopRotates", moveBackIn);
	}
	void LoopRotates()
	{
		Vector3 rot = transform.localEulerAngles;
		rot.y += 180;
		transform.localEulerAngles = rot;
		Invoke("LoopRotates", moveBackIn);
	}
	void OnDisable()
	{
		CancelInvoke ();
		isOn = false;
	}
	void OnEnable()
	{
		isOn = true;
	}
	void Update()
	{
		if (!isOn)
			return;
		transform.Translate(-Vector3.forward * Mathf.Abs(realSpeed) * Time.deltaTime);
	}
}
