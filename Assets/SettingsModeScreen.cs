using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsModeScreen : MonoBehaviour {

    public Toggle toggleControls;
    public Toggle togglePlayers;

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
        ContinueMode();
        // CreditsMode();
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
		Data.Instance.LoadLevel("MainMenuMobile");
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
