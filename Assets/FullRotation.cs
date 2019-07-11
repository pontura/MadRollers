using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class FullRotation : MonoBehaviour {

	public bool rotateX;
	public bool rotateY;
	public bool rotateZ;
	public bool inverse;

    public bool randomRotation = true;
    public float startRotation = 0;
	
	public float speed = 50f;
    public bool inverseRotation;
	
	private float rotationX;
	private float rotationY;
	private float rotationZ;

	public bool frameByFrame;
    

	void OnEnable () {

		if (randomRotation) {
			if (rotateX)
				rotationX = UnityEngine.Random.Range (-180, 180);
			if (rotateY)
				rotationY = UnityEngine.Random.Range (-180, 180);
			if (rotateZ)
				rotationZ = UnityEngine.Random.Range (-45, 45);

			if (UnityEngine.Random.Range (0, 10) < 5)
				speed *= -1;
			
		} else if (startRotation != 0) {
			if (rotateX)
				rotationX = startRotation;
			else if (rotateY)
				rotationY = startRotation;
			else if (rotateZ)
				rotationZ = startRotation;
		} else {
			rotationX = transform.localEulerAngles.x;
			rotationY = transform.localEulerAngles.y;
			rotationZ = transform.localEulerAngles.z;
		}

		if(inverse)
			speed *=-1;

//		if (frameByFrame) {
//			Loop ();
//		}
	}
//	int rotID = 0;
//
//	void Loop()
//	{
//		Vector3 rot = transform.localEulerAngles;
//		rot.y += 90;
//		transform.localEulerAngles = rot;
//		Invoke ("Loop", 0.1f);
//	}
	void Update () {
		if (frameByFrame) {
			return;
		}
		if (rotateX) 
			rotationX += speed * Time.deltaTime;
		else 
			rotationX = transform.localRotation.x;

		if (rotateY) 
			rotationY += speed * Time.deltaTime;
        else 
			rotationY = transform.localRotation.y;

        if (rotateZ && inverseRotation)
			rotationZ += speed*Time.deltaTime;
        else if (rotateZ && !inverseRotation)
            rotationZ -= speed * Time.deltaTime;
        else rotationZ = transform.localRotation.z;

			
		transform.localRotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
		
	}
	public void OnComponentDisposed()
	{
		Destroy (this);
	}
}