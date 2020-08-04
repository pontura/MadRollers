using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
public class LevelCreator : MonoBehaviour {

    public MissionsManager missionsManager;
    public Data.PlayModes playMode;
    public bool playOnlyBosses;
    public bool isArcadeMultiplayer;
	public bool Debbug;

	//[HideInInspector]
	public int videoGameID;
	//[HideInInspector]
	public int missionID;

	public TextAsset mission;
	public Missions missions;
	public AreaCreator areaCreator;
    public TextAsset missionAsset;
    public TextAsset area;
    

    float totalDistance;

	void Start () {
		if (Debbug) {
            LevelDataDebug.Instance.playOnlyBosses = playOnlyBosses;
            LevelDataDebug.Instance.isArcadeMultiplayer = isArcadeMultiplayer;
			LevelDataDebug.Instance.isDebbug = true;
			LevelDataDebug.Instance.videogameID = videoGameID-1;
			LevelDataDebug.Instance.missionID = missionID;
            LevelDataDebug.Instance.playMode = playMode;
            if (area != null && LevelDataDebug.Instance.playMode != Data.PlayModes.SURVIVAL)
				LevelDataDebug.Instance.testArea = area.name;
		}
		Application.LoadLevel("00_Loading");        
    }
	public void LoadArea()
	{		
		Clear ();
		totalDistance = 0;
		AddAreaByName (area.name);
	}
	List<string> allNames = new List<string>();
	public void LoadMissions()
	{		
		Clear ();
		totalDistance = 0;

		allNames = new List<string>();
		if (mission != null) {
			MissionData missionData = missions.GetMissionsDataByJsonName (mission.name);
			foreach (MissionData.AreaSetData data in missionData.areaSetData) {
				LoadMissionData (data);
			}
		} else {
			foreach (MissionData.AreaSetData data in missionsManager.videogames[videoGameID-1].missions[missionID].data[0].areaSetData) {
				LoadMissionData (data);
			}
		}
	}
	void LoadMissionData(MissionData.AreaSetData data)
	{
		foreach (string areaName in data.areas) {
			bool exists = false;
			foreach (string savedAreaName in allNames) {
				if(savedAreaName == areaName)
					exists = true;
			}
			if (!exists) {
				AddAreaByName (areaName);
				allNames.Add (areaName);
			}
		}
	}
	void AddAreaByName(string areaName)
	{
		TextAsset asset = Resources.Load ("areas/" + areaName ) as TextAsset;
		if (asset != null) {					
			AreaData areaDataActive = JsonUtility.FromJson<AreaData> (asset.text);
			totalDistance += areaDataActive.z_length / 2;
			areaCreator.AddSceneObjectsToNewArea(areaName, areaDataActive, totalDistance);
			totalDistance += areaDataActive.z_length/2;
		} else {
			Debug.LogError ("Loco, no existe esta area: " + areaName + " en Respurces/areas/");
		}

	}
//	public MissionData GetMission()
//	{
//		//missionData = missions.allMissionsByVideogame;
//		//return allMissionsByVideogame [videoGameID].missions[missionID];
//		//return missions.GetMission(videoGameID, missionID);
//	}
	public void ResetAreas()
	{
		totalDistance = 0;
		Utils.RemoveAllChildsIn (transform);
	}
	public void AddArea(GameObject area, float distance)
	{		
		totalDistance += distance/2;
		GameObject a = Instantiate (area);
		a.name = a.name.Substring (0, a.name.Length - 7);
		a.transform.SetParent (transform);
		a.transform.localPosition = new Vector3 (0, 0, totalDistance);
		Area areaReal = a.GetComponent<Area>();
		totalDistance += distance/2;
	}
	public void UpdateMissions()
	{
        missionsManager.LoadAll ();
	}
	public void Clear()
	{
		int num = transform.childCount;
		for (int i = 0; i < num; i++) UnityEngine.Object.DestroyImmediate(transform.GetChild(0).gameObject);
	}

	public void SaveArea()
	{
		foreach (Area area in transform.gameObject.GetComponentsInChildren<Area>()) {
			area.transform.localPosition = Vector3.zero;
			if (area.name != area.name) {
				area.gameObject.SetActive (false);
			} else {
				area.gameObject.SetActive (true);
			}
			GetComponent<AreaCreator> ().CreateData (area);
		}
		Utils.RemoveAllChildsIn (transform);
	}
}
