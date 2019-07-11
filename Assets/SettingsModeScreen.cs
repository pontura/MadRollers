using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsModeScreen : MonoBehaviour {

	void Start()
	{
		//InitStoryMode ();
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
}
