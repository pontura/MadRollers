using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class SummaryCompetitions : MonoBehaviour {

	public GameObject panel;
	public List<MainMenuButton> buttons;
	public int optionSelected = 0;
	private bool isOn;
	public SummaryMissionPoint missionPoint;
	public Transform container;
	public Image progressImage;
	public Text scoreField;
	public Text missionsField;
	float delayToReact = 0.3f;
    public Image videogameLogo;

	void Start()
	{
		panel.SetActive (false);
	}
	public void Init()
	{
		if (isOn)
            return;
        
        Invoke("SetOn", 1F);
    }
    private void OnDestroy()
    {
        Data.Instance.events.OnJoystickClick -= OnJoystickClick;
        Data.Instance.events.OnJoystickUp -= OnJoystickUp;
        Data.Instance.events.OnJoystickDown -= OnJoystickDown;
    }
    float fillAmount;
	public void SetOn()
	{
        Data.Instance.events.OnJoystickClick += OnJoystickClick;
        Data.Instance.events.OnJoystickUp += OnJoystickUp;
        Data.Instance.events.OnJoystickDown += OnJoystickDown;

        videogameLogo.sprite = Data.Instance.videogamesData.GetActualVideogameData().logo;
        Data.Instance.events.RalentaTo (1, 0.05f);
		isOn = true;
		panel.SetActive(true);
		SetSelected ();
		int missionActive = Data.Instance.missions.MissionActiveID;
		int id = 0;
		scoreField.text = TextsManager.Instance.GetText("GAME OVER");
        Invoke ("TimeOver", 35);
		int totalMissions = MissionsManager.Instance.videogames[Data.Instance.videogamesData.actualID].missions.Count;
		fillAmount = (float)missionActive / (float)totalMissions;
		missionsField.text = TextsManager.Instance.GetText("DISKETTE") + " " + (missionActive+1).ToString() + "/" + totalMissions.ToString();
	}
	void TimeOver()
	{
		Data.Instance.inputSavedAutomaticPlay.RemoveAllData ();
		Game.Instance.GotoMainMenu ();	
	}
	void Update()
	{
		if (!isOn)
			return;

		if(progressImage.fillAmount < fillAmount)
			progressImage.fillAmount += 0.005f;
		else
			progressImage.fillAmount = fillAmount;
	}


	float lastClickedTime = 0;
	bool processAxis;

	void OnJoystickUp () {
		optionSelected++;
		SetSelected ();
	}
	void OnJoystickDown () {
		optionSelected--;
		SetSelected ();
	}
	void SetSelected()
	{
        if (optionSelected < 0)
            optionSelected = buttons.Count - 1;
        else if (optionSelected >= buttons.Count)
            optionSelected = 0;

        foreach (MainMenuButton b in buttons)
			b.SetOn (false);
		buttons [optionSelected].SetOn (true);
	}

	void OnJoystickClick () {
		if (optionSelected == 0) {
            Data.Instance.isReplay = true;
            Game.Instance.ResetLevel();
        } else if (optionSelected == 1) {
			Data.Instance.events.OnResetScores ();
			Data.Instance.inputSavedAutomaticPlay.RemoveAllData ();

            if(Data.Instance.playMode != Data.PlayModes.STORYMODE)
			    Data.Instance.videogamesData.SetOtherGameActive ();

			Game.Instance.GotoLevelSelector ();	
		}
		isOn = false;
	}
	void ResetMove()
	{
		processAxis = false;
		lastClickedTime = 0;
	} 
}
