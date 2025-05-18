using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCamera : MonoBehaviour {

	public Transform characters;
	public GameObject floor;
	float offsetFloor = 19.2f;
	void Update () {
		
		transform.RotateAround (characters.transform.position, Vector3.up, 50 * Time.deltaTime);
		Vector3 pos = characters.transform.localPosition;
		pos.z += 30 * Time.deltaTime;
		characters.transform.localPosition = pos;
		transform.localPosition = pos;
		if (pos.z > floor.transform.localPosition.z + offsetFloor) {
			floor.transform.localPosition = new Vector3 (0,0, floor.transform.localPosition.z + offsetFloor);
		}
	}
}
