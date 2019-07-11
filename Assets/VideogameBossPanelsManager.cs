using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideogameBossPanelsManager : MonoBehaviour {

	public VideogameBossPanel boosPanel1;
	public VideogameBossPanel boosPanel2;
	public VideogameBossPanel boosPanel3;

	public VideogameBossPanel actualBossPanel;

	void Awake () {
		switch (Data.Instance.videogamesData.actualID) {
		case 0:
			actualBossPanel = Instantiate (boosPanel1) ;
			break;
		case 1:
			actualBossPanel = Instantiate (boosPanel2) ;
			break;
		default:
			actualBossPanel = Instantiate (boosPanel3) ;
			break;
		}


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
