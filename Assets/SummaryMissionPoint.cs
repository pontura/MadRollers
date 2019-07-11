using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummaryMissionPoint : MonoBehaviour {

	public GameObject activeAsset;

	public void Init(bool isActive)
	{
		activeAsset.SetActive (isActive);
	}
}
