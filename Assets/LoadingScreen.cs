using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour {
	
	void Start () {
        if (LevelDataDebug.Instance)
        {
            Data.Instance.missions.Init();
            // Data.Instance.isReplay = true;
            Data.Instance.LoadLevel("LevelSelector");
        } else if (Data.Instance.playMode == Data.PlayModes.STORYMODE)
        {
            Data.Instance.missions.Init();
            Data.Instance.LoadLevel("LevelSelector");
        } else
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
