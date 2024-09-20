using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideogameBossPanelsManager : MonoBehaviour {

    public Transform container;
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
        actualBossPanel.transform.SetParent(container);
        actualBossPanel.transform.localPosition = Vector3.zero;
        actualBossPanel.transform.localEulerAngles = Vector3.zero;
        actualBossPanel.transform.localScale = Vector3.one;

        boosPanel2 = null;
        boosPanel1 = null;
        boosPanel3 = null;

    }
}
