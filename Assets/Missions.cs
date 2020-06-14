using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Missions : MonoBehaviour {

    public bool hasReachedBoss;
    public ExtraAreasManager extraAreasManager;
    public int times_trying_same_mission;
	public int MissionActiveID = 0;
	public MissionData MissionActive;    

    private float missionCompletedPercent = 0;

	private Level level;
	private bool showStartArea;
	private Data data;
	float distance;

	public AreaData areaDataActive;
	float areasLength;
	int offset = 100;
	int areaSetId = 0;
	int areaNum = 0;
	int areaID = 0;
    float totalDistance = 0;

    VideogamesData videogamesData;
    
    public void Init()
    {
        
        if (Data.Instance.playMode == Data.PlayModes.STORYMODE && Data.Instance.isReplay)
            offset -= 40;

        videogamesData = GetComponent<VideogamesData> ();
		data = Data.Instance;

        if (Data.Instance.playMode == Data.PlayModes.SURVIVAL)
        {
            offset -= 40;
            MissionsManager.Instance.all = null;
            //MissionActive = MissionsManager.Instance.LoadDataFromMission("survival", "boyland").data[0];
            MissionActive = MissionsManager.Instance.videogames[3].missions[0].data[0];
           // extraAreasManager.Init();              
        }
        else
        {
            data.events.ResetMissionsBlocked += ResetMissionsBlocked;
            data.events.OnMissionComplete += OnMissionComplete;
        }
    }
    void OnDestroy()
    {
        if (data != null)
        {
            data.events.ResetMissionsBlocked -= ResetMissionsBlocked;
            data.events.OnMissionComplete -= OnMissionComplete;
        }
    }
    void ResetMissionsBlocked()
    {
        //foreach(MissionsManager.MissionsByVideoGame mbv in MissionsManager.Instance.videogames)
        //    mbv.missionUnblockedID = 0;
    }
	public MissionData GetMissionsDataByJsonName(string jsonName)
	{
		Debug.Log (jsonName);
		foreach (MissionsManager.MissionsByVideoGame mvv in MissionsManager.Instance.videogames) {
			foreach (MissionsManager.MissionsData mmData in mvv.missions)  {
				foreach (MissionData mData in mmData.data)  {
					if (mData.jsonName == jsonName)
						return mData;
				}
			}
		}
		return null;
	}
	public void Init (Level level) {

        if (Data.Instance.playMode == Data.PlayModes.SURVIVAL)
        {
            MissionActive = MissionsManager.Instance.videogames[3].missions[0].data[0];
            extraAreasManager.Init();              
        }


        totalDistance = 0;
        this.level = level;
		areasLength = -4;
		StartNewMission ();

        if (Data.Instance.isReplay && Data.Instance.playMode == Data.PlayModes.STORYMODE)
        {
            AddAreaByName("continue_Multiplayer");
        }
        else {
          //if (!Data.Instance.DEBUG && Data.Instance.playMode == Data.PlayModes.PARTYMODE)
		//	ShuffleMissions ();
		    AddAreaByName ("start_Multiplayer");
		} 
	}
	void ShuffleMissions()
	{		
		foreach (MissionsManager.MissionsByVideoGame mbv in MissionsManager.Instance.videogames) {		
			for (int a = 0; a < 50; a++) {	
				int rand = UnityEngine.Random.Range (3, mbv.missions.Count);
                MissionsManager.MissionsData randomMission1 = mbv.missions [2];
                MissionsManager.MissionsData randomMission2 = mbv.missions [rand];

				mbv.missions [rand] = randomMission1;
				mbv.missions [2] = randomMission2;
			}
		}
	}
	void OnMissionComplete(int id)
	{
        hasReachedBoss = false;
        times_trying_same_mission = 0;
		//if (Data.Instance.playMode == Data.PlayModes.PARTYMODE) {
		//	AddAreaByName ("areaChangeLevel");
		//	return;
		//} else 
        if (MissionActiveID >= MissionsManager.Instance.videogames [videogamesData.actualID].missions.Count - 1) {
			Game.Instance.GotoVideogameComplete ();
		} else {
			NextMission ();
			int videogameID = videogamesData.actualID+1;
            UserData.Instance.SetMissionReady(videogameID, MissionActiveID);
		}
    }
	public int GetTotalMissionsInVideoGame(int videogameID)
	{
		return MissionsManager.Instance.videogames [videogameID].missions.Count;
	}
	public MissionsManager.MissionsByVideoGame GetMissionsByVideoGame(int videogameID)
	{
		return MissionsManager.Instance.videogames [videogameID];
	}
	void NextMission()
	{
		MissionActiveID++;
  //      AddAreaByName("newLevel_playing");
  //      StartNewMission ();
		//Data.Instance.events.OnChangeBackgroundSide (MissionActive.fondo);
	}
	void StartNewMission()
	{
		areaSetId = 0;
		ResetAreaSet ();
        if (Data.Instance.playMode != Data.PlayModes.SURVIVAL)
			MissionActive = MissionsManager.Instance.videogames[videogamesData.actualID].missions[MissionActiveID].data[0];
		this.missionCompletedPercent = 0;
	}
	public MissionData GetActualMissionData()
	{
        if (Data.Instance.playMode == Data.PlayModes.SURVIVAL)
            return MissionActive;
        else
            return MissionsManager.Instance.videogames[videogamesData.actualID].missions[MissionActiveID].data[0];
	}
	public MissionData GetMission(int videoGameID, int missionID)
	{
		return MissionsManager.Instance.videogames[videoGameID].missions[missionID].data[0];
	}
	public int GetActualMissionByVideogame()
	{
		int viedogameActive = videogamesData.actualID;
		int id = 0;
		foreach (MissionData mission in MissionsManager.Instance.videogames[viedogameActive].missions[0].data) {
			if (mission.id == MissionActive.id)
				return id;
			id++;
		}
		return 0;
	}
	public void OnUpdateDistance(float distance)
	{
		if (distance > areasLength-offset) {
			SetNextArea ();
		}
        if (Data.Instance.playMode == Data.PlayModes.SURVIVAL)
            return;
		if (MissionActiveID == 0 && videogamesData.actualID == 0)
			CheckTutorial (distance);
	}
    
    int total_areas = 1;
	void SetNextArea()
	{
        MissionData.AreaSetData data = MissionActive.areaSetData[areaSetId];
        if (Data.Instance.playMode != Data.PlayModes.SURVIVAL && (Data.Instance.playOnlyBosses || hasReachedBoss) && !data.boss && areaSetId < MissionActive.areaSetData.Count - 2)
        {
           // print("_________________area set id ++");
            areaSetId++;
            ResetAreaSet();
            SetNextArea();
            return;
        }

        if (data.boss && Data.Instance.playMode != Data.PlayModes.STORYMODE)
            hasReachedBoss = true;

        CreateCurrentArea ();

       // Debug.Log("areaSetId: " + areaSetId + "   data.cameraOrientation: " + data.cameraOrientation + " bending: " + data.bending);

        Game.Instance.gameCamera.SetOrientation (data.cameraOrientation);
		total_areas = data.total_areas;
		float bending = data.bending;
		
		if(bending != 0)
			Data.Instance.events.ChangeCurvedWorldX(bending);
        areaNum++;

      //  print("___________areaNum: " + areaNum + "  areaSetId " + areaSetId + "     total_areas: " + total_areas);

        if (areaNum >= total_areas) {
			if (areaSetId < MissionActive.areaSetData.Count - 1) {
				areaSetId++;
				ResetAreaSet ();
			} else {
				areaNum--;
			}
		}
		
	}
	void ResetAreaSet()
	{
		areaNum = 0;
		areaID = 0;
	}
	private void CreateCurrentArea()
	{
		MissionData.AreaSetData areaSetData = MissionActive.areaSetData[areaSetId];
        string areaName = GetArea(areaSetData);
        CreateCurrentArea(areaName);
    }
    public void CreateCurrentArea(string areaName, bool isXtra = false)
    {
        //DEBUG:::::
        if (Data.Instance.testAreaName != "")
            AddAreaByName(Data.Instance.testAreaName);
        else
            AddAreaByName(areaName, isXtra);

    }
    void AddAreaByName(string areaName, bool isXtra = false)
    {
        TextAsset asset = MissionsManager.Instance.areasManager.GetArea(areaName);
       // TextAsset asset = Resources.Load ("areas/" + areaName ) as TextAsset;
		if (asset != null) {					
			areaDataActive = JsonUtility.FromJson<AreaData> (asset.text);
			areasLength += areaDataActive.z_length/2;
			level.sceneObjects.AddSceneObjects (areaDataActive, areasLength);
			//print ("AREA: " + areaName + " km: " + areasLength + " mission: " + MissionActiveID +  " areaSetId: " + areaSetId + " areaID: " + areaID + " z_length: " + areaDataActive.z_length + " en: areas/" + areaName +  " totalAreas" + total_areas );
			areasLength += areaDataActive.z_length/2;

            //HACK : no ocupe lugar el area extra:
            if (isXtra)
                areasLength -= areaDataActive.z_length;

        } else {
			Debug.LogError ("Loco, no existe esta area: " + areaName + " en Respurces/areas/");
		}

	}
	List<MissionData.AreaSetData> GetActualAreaSetData()
	{
		return MissionActive.areaSetData;
	}
	string GetArea(MissionData.AreaSetData areaSetData)
	{
		if (areaSetData.randomize) {
			areaID++;
			return areaSetData.areas [UnityEngine.Random.Range(0,areaSetData.areas.Count)];
		} else if (areaID < areaSetData.areas.Count - 1) {
			areaID++;
			return areaSetData.areas [areaID-1];
		} else {
			return areaSetData.areas [areaSetData.areas.Count-1];
		}
	}

	int tutorialID = 0;
	void CheckTutorial(float distance)
	{
        if (hasReachedBoss)
            return;

		if(tutorialID >= 3 || Data.Instance.playMode == Data.PlayModes.VERSUS )
			return;

		if (distance>148 && tutorialID < 1)
		{
			Data.Instance.voicesManager.PlayClip (Data.Instance.voicesManager.tutorials [0].audioClip);
			tutorialID = 1;
		} else if(distance>200 && tutorialID < 2)
		{
			Data.Instance.voicesManager.PlayClip (Data.Instance.voicesManager.tutorials [1].audioClip);
			tutorialID = 2;
		} else if(distance>305 && tutorialID < 3 && Data.Instance.playMode != Data.PlayModes.STORYMODE)
		{
			Data.Instance.voicesManager.PlayClip (Data.Instance.voicesManager.tutorials [2].audioClip);
			tutorialID = 3;
		}
	}
    public float GetTotalRoutDistance()
    {
        totalDistance = 100;
        foreach (MissionData.AreaSetData d in MissionActive.areaSetData)
        {           
            int id = 0;
            int totalAreas = d.total_areas;
            foreach (string areaName in d.areas)
            {
                if (id < totalAreas)
                {
                    TextAsset asset = MissionsManager.Instance.areasManager.GetArea(areaName);
                   // TextAsset asset = Resources.Load("areas/" + areaName) as TextAsset;
                    if (asset != null)
                    {
                           
                        AreaData areaData = JsonUtility.FromJson<AreaData>(asset.text);
                        totalDistance += areaData.z_length;
                        //print(":::::::::::::  area: " + areaName + "  distance: " + areaData.z_length + "  totalDistance: " + totalDistance);
                    }
                }
                id++;
            }
            if (d.boss)
                return totalDistance;     
        }
        return totalDistance;
    }
}
