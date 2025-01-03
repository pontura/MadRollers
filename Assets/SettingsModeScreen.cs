﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsModeScreen : MonoBehaviour {

    public Toggle toggleControls;
    public Toggle togglePlayers;
    bool controls;
    void Start()
	{
        int controlsID = PlayerPrefs.GetInt("toggleControls", 1);

        if (controlsID == 0)
            toggleControls.isOn = false;

        Cursor.visible = true;
        Data.Instance.events.OnJoystickClick += OnJoystickClick;
        SetControls();
    }
    void OnDestroy()
    {
        Data.Instance.events.OnJoystickClick -= OnJoystickClick;
    }
    void OnJoystickClick()
    {
        //ContinueMode();
         CreditsMode();
    }
    public void Storymode()
    {
        Data.Instance.playMode = Data.PlayModes.STORYMODE;
        Go();   
    }
    public void ContinueMode () {
		Data.Instance.playMode = Data.PlayModes.CONTINUEMODE;
		Go ();
	}

	public void CreditsMode() {
		//Data.Instance.totalCredits = 4;
		Data.Instance.playMode = Data.PlayModes.PARTYMODE;
        Data.Instance.LoadLevel("MainMenu");
    }

    public void InsaneMode()
    {
        Data.Instance.playMode = Data.PlayModes.SURVIVAL;
        Go();
    }
    void Go()
	{
        Data.Instance.missions.Init();
		Data.Instance.LoadLevel("MainMenu");
	}
    public void Controls()
    {
        Data.Instance.events.OnJoystickClick -= OnJoystickClick;
        Data.Instance.controlMapper.Open();
    }
    public void TooglePlayers()
    {
        Data.Instance.singlePlayer = !Data.Instance.singlePlayer;
    }
    public void ToogleControls()
    {
        if(toggleControls.isOn)
            PlayerPrefs.SetInt("toggleControls", 1);
        else
            PlayerPrefs.SetInt("toggleControls", 0);
        SetControls();
    }   
    void SetControls()
    {
        Rewired.UI.ControlMapper.ControlMapper controlMapper = Data.Instance.controlMapper;

        foreach (Rewired.Player player in Rewired.ReInput.players.AllPlayers)
        {
            player.controllers.maps.SetMapsEnabled(toggleControls.isOn, "Default");
        }

    }
    public void Exit()
    {
        Application.Quit();
    }
}
