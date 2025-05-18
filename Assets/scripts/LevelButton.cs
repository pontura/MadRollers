using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour {

	public GameObject missionFields;
	public GameObject missionNames;

	public TextMesh[] overs;

	public GameObject[] stars;

	public void Init (int missionID, string missionName) {
		foreach (TextMesh m in missionFields.GetComponentsInChildren<TextMesh>())
			m.text = missionName;
	}
	public void SetOn(bool isOn)
	{
		Color color = Color.white;
		if (isOn)
			color = Color.yellow;
		//foreach (TextMesh m in overs)
			//m.text = color;
	}

}
