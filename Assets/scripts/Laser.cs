using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : SceneObject {

	public Transform container;
	public GameObject laserObject;
	public GameObject[] parts;
	public List<GameObject> laserParts;
	public List<Vector3> pos;
	public GameObject laser;
	public float distanceToBeActive = 10f;
	bool isActivatedByAvatar;
	SceneObjectData data;

	public override void OnRestart(Vector3 pos)
	{
		data = GetComponent<SceneObjectData> ();
		base.OnRestart (pos);
		laser.SetActive (false);
		parts [1].transform.localPosition = new Vector3 (0, data.size.y, 0);
	}
	bool isBeenBroken()
	{
		if (parts [0] == null || parts [1] == null) {
			if (laserParts.Count > 0) {
				Utils.RemoveAllChildsIn (container);
				laserParts.Clear ();
				if(laser != null)
					laser.SetActive (false);
			}
			return true;
		}
		return false;
	}
	void InitLaser()
	{
		if (isBeenBroken ()) return;
		laser.SetActive (true);

		Vector3 initPos = parts [0].transform.localPosition;
		Vector3 lastPos = parts [1].transform.localPosition;
		Vector3 newPos = initPos;
		while (newPos.y < lastPos.y - 0.4f) {
			GameObject o = Instantiate (laserObject);
			o.transform.SetParent (container);
			newPos.y += 0.4f;
			o.transform.localPosition = newPos;
			laserParts.Add (o);
		}
		Repositionate ();
		laser.transform.localPosition = initPos + lastPos / 2;
		laser.transform.localScale = new Vector3 (2f,(laser.transform.localPosition.y/0.4f),2f);
	}
	float delayedLastFrame;
	void Update()
	{
		if (!isActive)
			return;
		if (isBeenBroken ()) return;
		if (distanceFromCharacter < distanceToBeActive) {
			if (!isActivatedByAvatar) {
				isActivatedByAvatar = true;
				InitLaser ();
			} else {
				if (Time.time > delayedLastFrame) {
					delayedLastFrame = Time.time + 0.1f;
					Repositionate ();
				}
			}
		}
	}

	void Repositionate()
	{
		Vector3 lastPos = Vector3.zero;
		int id = 0;
		int mirroredID = 0;
		int halfId = (laserParts.Count) / 2;
		foreach (GameObject go in laserParts) {
			if (id < halfId) {
				float randX = (float)Random.Range (-1, 2) * 0.4f;
				float randZ = (float)Random.Range (-1, 2) * 0.4f;
				lastPos.x += randX;
				lastPos.z += randZ;
			} else {
				mirroredID++;
				int newID = halfId - mirroredID;
				if(newID >=0)
					lastPos = laserParts [newID].transform.localPosition;				
			}
			lastPos.y = go.transform.localPosition.y;
			go.transform.localPosition = lastPos;
			id++;
		}
	}

}
