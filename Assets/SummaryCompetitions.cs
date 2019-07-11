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

	void Start()
	{
		panel.SetActive (false);
	}
	public void Init()
	{
		return;

//		panel.SetActive(false);
//		if (Data.Instance.playMode == Data.PlayModes.STORYMODE)
//			return;
//		if (isOn) return;
//		Invoke("SetOn", 2F);
	}
//	float fillAmount;
//	public void SetOn()
//	{
//		Data.Instance.events.RalentaTo (1, 0.05f);
//		isOn = true;
//		panel.SetActive(true);
//		SetSelected ();
//		int missionActive = Data.Instance.missions.MissionActiveID;
//		int id = 0;
////		foreach (Missions.MissionsData m in Data.Instance.missions.videogames[Data.Instance.videogamesData.actualID].missions) {
////			SummaryMissionPoint smp = Instantiate (missionPoint);
////			smp.transform.SetParent (container);
////			smp.transform.localScale = Vector3.one;
////			if (id <= missionActive) 
////				smp.Init (true);
////			else
////				smp.Init (false);
////			id++;
////		}
////		Missions.MissionsByVideogame missionsInThisVideogame = Data.Instance.missions.videogames [Data.Instance.videogamesData.actualID];
////		MissionData mission;
////		int num = 0;
////		int numMission = -1;
////		foreach (MissionData m in missionsInThisVideogame.data) {
////			if (m.id == missionActive) {
////				numMission = num;
////			}
////			num++;
////		}
//		scoreField.text = "SCORE " + Data.Instance.multiplayerData.score;
//		Invoke ("TimeOver", 35);
//		missionsField.text = "";
//		if (missionActive < 0)
//			return;
//		int totalMissions = Data.Instance.missions.videogames[Data.Instance.videogamesData.actualID].missions.Count;
//		fillAmount = (float)missionActive / (float)totalMissions;
//		missionsField.text = "MISSION " + (missionActive+1).ToString() + "/" + totalMissions.ToString();
//	}
//	void TimeOver()
//	{
//		Data.Instance.inputSavedAutomaticPlay.RemoveAllData ();
//		Game.Instance.GotoMainMenu ();	
//	}
//	void Update()
//	{
//		if (!isOn)
//			return;
//
//		if(progressImage.fillAmount < fillAmount)
//			progressImage.fillAmount += 0.005f;
//		else
//			progressImage.fillAmount = fillAmount;
//		
//		lastClickedTime += Time.deltaTime;
//
//		if (lastClickedTime > delayToReact)
//			processAxis = true;
//		
//		for (int a = 0; a < 4; a++) {
//			if (InputManager.getJump (a)) 
//				OnJoystickClick ();
//			if (InputManager.getFireDown (a)) 
//				OnJoystickClick ();
//			if (processAxis) {				
//				float v = InputManager.getVertical (a);
//				if (v < -0.5f)
//					OnJoystickDown ();
//				else if (v > 0.5f)
//					OnJoystickUp ();
//
//				float h = InputManager.getHorizontal (a);
//				if (h < -0.5f)
//					OnJoystickUp ();
//				else if (h > 0.5f)
//					OnJoystickDown ();
//			}
//		}
//	}
//
//
//	float lastClickedTime = 0;
//	bool processAxis;
//
//	void OnJoystickUp () {
//		if (optionSelected >= buttons.Count - 1)
//			return;
//		optionSelected++;
//		SetSelected ();
//	}
//	void OnJoystickDown () {
//		if (optionSelected <= 0)
//			return;
//		optionSelected--;
//		SetSelected ();
//	}
//	void SetSelected()
//	{
//		foreach(MainMenuButton b in buttons)
//			b.SetOn (false);
//		buttons [optionSelected].SetOn (true);
//	}
//
//	void OnJoystickClick () {
//		if (optionSelected == 0) {
//			Data.Instance.events.OnResetScores ();
//			Data.Instance.missions.MissionActiveID = 0;
//			Game.Instance.LoadGame ();	
//		} else if (optionSelected == 1) {
//			Data.Instance.events.OnResetScores ();
//			Data.Instance.inputSavedAutomaticPlay.RemoveAllData ();
//			Data.Instance.videogamesData.SetOtherGameActive ();
//			Game.Instance.GotoLevelSelector ();	
//		}
//		isOn = false;
//	}
//	void OnJoystickBack () {
//		//Data.Instance.events.OnJoystickBack ();
//	}
//	void ResetMove()
//	{
//		processAxis = false;
//		lastClickedTime = 0;
//	} 
}
