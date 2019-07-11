using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionSelector : MonoBehaviour {
	
	public Image puntero;
	public Image bar;
	public Text titleField;
	public Text titlePartyField;

	public Text missionField;
	public Text percentField;
	public int totalMissions;
	public int actualMission;
	public int videogameID;
	int missionUnblockedID;

	public void LoadVideoGameData(int _videogameID)
	{
		this.videogameID = _videogameID;

		if (Data.Instance.playMode == Data.PlayModes.STORYMODE || Data.Instance.playMode == Data.PlayModes.SURVIVAL)
			titleField.text = Data.Instance.videogamesData.all [videogameID].name;
		else if(titlePartyField != null){
			titlePartyField.text = Data.Instance.videogamesData.all [videogameID].name;
			return;
		}
		
		missionUnblockedID = Data.Instance.missions.GetMissionsByVideoGame (videogameID).missionUnblockedID;
		actualMission = missionUnblockedID;
		totalMissions = Data.Instance.missions.GetTotalMissionsInVideoGame (videogameID);
		SetTexts ();
	}
	public void ChangeMission(int _actualMission)
	{
		actualMission = _actualMission;
		SetTexts ();
	}
	void SetTexts()
	{		
		missionField.text = "MISION " + (actualMission+1) + "/" + totalMissions;
		Data.Instance.missions.MissionActiveID = actualMission;
	}
	void Update()
	{
		
		if (Data.Instance.playMode != Data.PlayModes.STORYMODE)
			return;
		
		float fillAmount = (float)missionUnblockedID / (float)totalMissions;
		float goTo = (float)actualMission / (float)totalMissions;
		bar.fillAmount = Mathf.Lerp (bar.fillAmount, fillAmount, 0.05f);
		percentField.text = ((int)(bar.fillAmount * 100)).ToString() + "%";
		Vector3 pos = puntero.transform.localPosition;
		pos.x = Mathf.Lerp (pos.x, goTo*200, 0.05f);
		puntero.transform.localPosition = pos;
	}
}
