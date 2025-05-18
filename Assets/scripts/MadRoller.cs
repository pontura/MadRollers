using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadRoller : MonoBehaviour {

	public PlayerAsset[] assets;
	public int forceInitOnMenues;
	public GameObject[] gameFxOffOnStart;

	void Start()
	{
		if (forceInitOnMenues > 0) {
			Init (forceInitOnMenues - 1);
		}
	}
	public void SetFxOff()
	{
		foreach (GameObject go in gameFxOffOnStart)
			go.SetActive (false);
	}
	public void Init(int id)
	{
		foreach (PlayerAsset asset in assets) {
			if (asset.player1 && id == 0)
				asset.gameObject.SetActive (true);
			else if (asset.player2 && id == 1)
				asset.gameObject.SetActive (true);
			else if (asset.player3 && id == 2)
				asset.gameObject.SetActive (true);
			else if (asset.player4 && id == 3)
				asset.gameObject.SetActive (true);
			else
				asset.gameObject.SetActive (false);
		}
	}
}
