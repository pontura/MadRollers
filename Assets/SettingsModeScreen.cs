using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsModeScreen : MonoBehaviour {

    public Toggle toggleControls;

    void Start()
	{
        Data.Instance.events.OnJoystickClick += OnJoystickClick;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnJoystickClick -= OnJoystickClick;
    }
    void OnJoystickClick()
    {
        ContinueMode();
    }
    public void ContinueMode () {
		Data.Instance.playMode = Data.PlayModes.CONTINUEMODE;
		Go ();
	}

	public void CreditsMode() {
		Data.Instance.totalCredits = 4;
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
        //Data.Instance.events.OnMusicStatus (true);
        //Data.Instance.events.OnSFXStatus (true);
        //Data.Instance.events.OnMadRollersSFXStatus (true);
        //Data.Instance.events.OnVoicesStatus (true);

        //Data.Instance.canContinue = true;
        //Data.Instance.musicOn = true;
        //Data.Instance.soundsFXOn = true;
        //Data.Instance.madRollersSoundsOn = true;
        //Data.Instance.voicesOn = true;

        //Data.Instance.switchPlayerInputs = false;

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
}
