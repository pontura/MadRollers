using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour {
	
	void Start () {
		Invoke("Next", 1);
	}
	void Next () {
        if(Data.Instance.isAndroid)
        {
            Data.Instance.missions.Init();
            Data.Instance.LoadLevel("MainMenuMobile");
            return;
        }
		Data.Instance.LoadLevel("Settings");
	}
}
