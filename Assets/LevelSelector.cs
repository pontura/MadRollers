using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelSelector : MonoBehaviour {

	public Animation camAnimnation;

	public GameObject storyMode;
	public GameObject partyMode;

	public Text title;

	public Text credits;
	public Text creditsParty;

	public MissionButton diskette;
    public MissionButton diskette2;
    VideogameData videogameData;
	public int videgameID;
	//VideogamesUIManager videogameUI;
	bool canInteract;
	float timePassed;
	MissionSelector missionSelector;

	void Start()
	{
        Data.Instance.missions.hasReachedBoss = false;
        if (Data.Instance.playMode == Data.PlayModes.SURVIVAL || Data.Instance.playMode == Data.PlayModes.PARTYMODE || Data.Instance.playMode == Data.PlayModes.CONTINUEMODE) {
			Data.Instance.missions.MissionActiveID = 0;
			storyMode.SetActive (false);
			partyMode.SetActive (true);
			timePassed = 0;
			Invoke ("TimeOver", 90);
			Invoke ("SetCanInteract", 1);
		} else {			
			Data.Instance.events.OnJoystickUp += OnJoystickUp;
			Data.Instance.events.OnJoystickDown += OnJoystickDown;
			partyMode.SetActive (false);
			storyMode.SetActive (true);
			Invoke ("SetCanInteract", 0.2f);
		}
		Data.Instance.isReplay = false;
		missionSelector = GetComponent<MissionSelector> ();
        

        Data.Instance.multiplayerData.ResetAll ();
		Data.Instance.events.OnResetScores ();

        title.text = "CAMBIAR JUEGO";
       // title.text = "SELECT GAME";

		videgameID = Data.Instance.videogamesData.actualID;
        missionSelector.LoadVideoGameData(videgameID);
        //videogameUI = GetComponent<VideogamesUIManager> ();
        //videogameUI.Init ();
        SetSelected ();

		Data.Instance.voicesManager.PlaySpecificClipFromList (Data.Instance.voicesManager.UIItems, 0);

		Data.Instance.events.OnJoystickLeft += OnJoystickLeft;
		Data.Instance.events.OnJoystickRight += OnJoystickRight;

		Data.Instance.events.OnJoystickClick += OnJoystickClick;


	}
	void SetCanInteract()
	{
		canInteract = true;
	}
	void TimeOver()
	{
		Data.Instance.LoadLevel("MainMenu");
	}
	void OnDestroy()
	{
		Data.Instance.events.OnJoystickClick -= OnJoystickClick;
		Data.Instance.events.OnJoystickDown -= OnJoystickDown;
		Data.Instance.events.OnJoystickUp -= OnJoystickUp;
		Data.Instance.events.OnJoystickLeft -= OnJoystickLeft;
		Data.Instance.events.OnJoystickRight -= OnJoystickRight;
	}
	void OnJoystickClick()
	{
		if (!canInteract)
			return;
		
		canInteract = false;
		//computerUI.SetActive (false);
		diskette.SetOn ();
		Invoke ("Delayed", 4f);
		camAnimnation.Play ("levelSelectorCamera");
	}
	void Delayed()
	{
		Data.Instance.videogamesData.actualID = videgameID;
		Data.Instance.LoadLevel ("Game");
	}
	void OnJoystickUp()
	{
		print ("up");
		if (!canInteract)
			return;
		
		int MissionActiveID = Data.Instance.missions.MissionActiveID;
		if (MissionActiveID < Data.Instance.missions.GetMissionsByVideoGame (videgameID).missionUnblockedID) {
			Data.Instance.missions.MissionActiveID++;
			missionSelector.ChangeMission (Data.Instance.missions.MissionActiveID);
		}
	}
	void OnJoystickDown()
	{
		print ("down");

		if (!canInteract)
			return;

		int MissionActiveID = Data.Instance.missions.MissionActiveID;
		if (MissionActiveID > 0) {
			Data.Instance.missions.MissionActiveID--;
			missionSelector.ChangeMission (Data.Instance.missions.MissionActiveID);
		}
	}
	void OnJoystickLeft()
	{		
		if (!canInteract)
			return;

		int total =  Data.Instance.videogamesData.all.Length-1;
		if (videgameID < total)
			videgameID++;
		else
			return;
		SetSelected ();
	}
	void OnJoystickRight()
	{
		if (!canInteract)
			return;
		if(videgameID>0)
			videgameID--;
		else
			return;

		SetSelected ();	
	}
	void SetSelected()
	{
		List<VoicesManager.VoiceData> list = Data.Instance.voicesManager.videogames_names;
		Data.Instance.voicesManager.PlaySpecificClipFromList (list, videgameID);
		videogameData = Data.Instance.videogamesData.all [videgameID];
		missionSelector.LoadVideoGameData (videgameID);
		diskette.Init (videogameData);

        int _videgameID = videgameID + 1;
        if (_videgameID > 2)
            _videgameID = 0;

        VideogameData _videogameData = Data.Instance.videogamesData.all[_videgameID];
        diskette2.Init(_videogameData, false);

        //videogameUI.Change ();
        if (Data.Instance.playMode == Data.PlayModes.STORYMODE)
			Data.Instance.handWriting.WriteTo (credits, videogameData.credits, null);
		else
			Data.Instance.handWriting.WriteTo (creditsParty, videogameData.credits, null);
	}
	public void OnJoystickBack()
	{
		Data.Instance.LoadLevel("MainMenu");
	}
}
