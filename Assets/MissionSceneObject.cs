using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSceneObject : MonoBehaviour {

	public string missionName;

	public void Die()
	{
		Data.Instance.events.OnDestroySceneObject (missionName);
	}
}
