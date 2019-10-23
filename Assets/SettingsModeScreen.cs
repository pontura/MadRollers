using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsModeScreen : MonoBehaviour {

    public Toggle toggleControls;

    void Start()
	{
        Cursor.visible = true;
        Data.Instance.events.OnJoystickClick += OnJoystickClick;
        ToogleControls();
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
    public void ContinueMode () {
		Data.Instance.playMode = Data.PlayModes.CONTINUEMODE;
		Go ();
	}

	public void CreditsMode() {
		//Data.Instance.totalCredits = 4;
		Data.Instance.playMode = Data.PlayModes.PARTYMODE;
		Go ();
	}

    public void InsaneMode()
    {
        Data.Instance.playMode = Data.PlayModes.SURVIVAL;
        Go();
    }
    void Go()
	{
        Data.Instance.missions.Init();
        Cursor.visible = false;
		Data.Instance.LoadLevel("MainMenu");
	}
    public void Controls()
    {
        Data.Instance.events.OnJoystickClick -= OnJoystickClick;
        Data.Instance.controlMapper.Open();
    }
    public void ToogleControls()
    {

        Rewired.UI.ControlMapper.ControlMapper controlMapper = Data.Instance.controlMapper;

        if (toggleControls.isOn)
        {
            foreach (Rewired.Player player in Rewired.ReInput.players.AllPlayers)
            {
                player.controllers.maps.SetMapsEnabled(true, "Default");
            }
        }
        else
        {
            foreach (Rewired.Player player in Rewired.ReInput.players.AllPlayers)
            {
                player.controllers.maps.SetMapsEnabled(false, "Default");
            }
        }

    }
    public void Exit()
    {
        Application.Quit();
    }
}
