using UnityEngine;
using System.Collections;

public class Parachute : MonoBehaviour {

	public float speed = 7f;
	private Vector3 pos;
	private bool isActive;

	// Use this for initialization
	public void Init () {
        isActive = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isActive)
		{
			pos.y += speed*Time.deltaTime;
			transform.position = pos;
		}
	}

	public void Die() {
		Destroy(gameObject);
	}

	public void activate()
	{
		if(!transform) return;
		pos = transform.position;
        isActive = true;
	}
}
