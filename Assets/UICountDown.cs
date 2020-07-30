﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountDown : MonoBehaviour {

	public GameObject panel;

	public Text countDownField;
	int countDown = 3;
	bool isOn;

	void Start () {	
		panel.SetActive (false);
        //if (Data.Instance.isAndroid)
        //    Invoke("Done", 0.6f);
		if (Data.Instance.isReplay)
			return;
		
		Data.Instance.events.OnAddNewPlayer += OnAddNewPlayer;
	}
	void OnDestroy()
	{
		Data.Instance.events.OnAddNewPlayer -= OnAddNewPlayer;
	}
	void OnAddNewPlayer(int id)
	{		
		if (isOn)
			return;

        Data.Instance.musicManager.OnGamePaused(true);

        isOn = true;
		panel.SetActive (true);
		Data.Instance.events.OnGameStart ();
		SetNextCountDown ();
	}
	void SetNextCountDown()
	{
		
		panel.GetComponent<Animation>().Play("logo");

        Data.Instance.voicesManager.PlayCountDown(countDown);
        if (countDown == 0)
        {
            Data.Instance.events.StartMultiplayerRace();
            countDownField.text = "GO!";
            Invoke("Done", 1f);
            Data.Instance.events.OnSoundFX("FX upgrade003", -1);
            return;
        }
        else
        {            
            countDownField.text = countDown.ToString();
            Data.Instance.events.OnSoundFX("FX upgrade002", -1);
        }

        countDown--;
		Invoke ("SetNextCountDown", 1.25f);
	}
    void Done()
    {        
        panel.SetActive(false);
        Data.Instance.events.OnGenericUIText("ROMPAN TODO!");
    }
}
