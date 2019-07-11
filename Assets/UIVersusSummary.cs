using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVersusSummary : MonoBehaviour {

	public GameObject panel;
	public GameObject win1;
	public GameObject win2;

	void Start () {
		panel.SetActive (false);
		Data.Instance.events.OnVersusTeamWon += OnVersusTeamWon;
	}
	void OnDestroy()
	{
		Data.Instance.events.OnVersusTeamWon -= OnVersusTeamWon;
	}
	
	public void OnVersusTeamWon(int TeamID) {
		
		panel.SetActive (true);
		switch (TeamID) {
		case 1:
			win1.SetActive (true);
			win2.SetActive (false);
			break;
		default:
			win2.SetActive (true);
			win1.SetActive (false);
			break;
		}
		GetComponent<UIVersus> ().OnGameOver ();
	}
}
