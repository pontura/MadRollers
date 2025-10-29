using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Missions : MonoBehaviour
{

    //areasetData que vas guardando para cambiar el angulo de la camara:
    public List<MissionData.AreaSetData> areasetDataLoaded;

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
    int offset = 150;
    int areaSetId = 0;
    int areaNum = 0;
    int areaID = 0;
    float totalDistance = 0;

    VideogamesData videogamesData;

    public void Init()
    {
        MissionsManager.Instance.areasManager.LoadData();
        areasetDataLoaded.Clear();
        if (Data.Instance.playMode == Data.PlayModes.STORYMODE && Data.Instance.isReplay)
            offset -= 40;

        videogamesData = GetComponent<VideogamesData>();
        data = Data.Instance;

        if (Data.Instance.playMode == Data.PlayModes.SURVIVAL)
        {
            offset -= 40;
            MissionsManager.Instance.all = null;
            //MissionActive = MissionsManager.Instance.LoadDataFromMission("survival", "boyland").data[0];
              MissionActive = MissionsManager.Instance.missionsSurvival[0].data[0];
            // extraAreasManager.Init();              
        }
        else
        {
            Events.StartMultiplayerRace += StartMultiplayerRace;
            Events.ResetMissionsBlocked += ResetMissionsBlocked;
            Events.OnMissionComplete += OnMissionComplete;
            Events.OnBossActive += OnBossActive;
        }

    }
    public void Reset()
    {
        MissionActiveID = 0;
    }
    void OnDestroy()
    {
        if (data != null)
        {
            Events.StartMultiplayerRace -= StartMultiplayerRace;
            Events.ResetMissionsBlocked -= ResetMissionsBlocked;
            Events.OnMissionComplete -= OnMissionComplete;
            Events.OnBossActive -= OnBossActive;
        }
    }
    bool bossResetedOnce;
    void StartMultiplayerRace()
    {
        bossResetedOnce = false;
    }
    void ResetMissionsBlocked()
    {
        //foreach(MissionsManager.MissionsByVideoGame mbv in MissionsManager.Instance.videogames)
        //    mbv.missionUnblockedID = 0;
    }
    public MissionData GetMissionsDataByJsonName(string jsonName)
    {
        Debug.Log(jsonName);
        foreach (MissionsManager.MissionsData mvv in MissionsManager.Instance.missions)
        {
            foreach (MissionData mData in mvv.data)
            {
                if (mData.jsonName == jsonName)
                    return mData;
            }
        }
        return null;
    }
    public void Init(Level level)
    {

        if (Data.Instance.playMode == Data.PlayModes.SURVIVAL)
        {
            MissionActive = MissionsManager.Instance.missionsSurvival[0].data[0];
           // MissionActive = MissionsManager.Instance.missionsSurvival[0].data[0];
            extraAreasManager.Init();
        }

        totalDistance = 0;
        this.level = level;
        areasLength = -4;
        StartNewMission();

        if (Data.Instance.isReplay && Data.Instance.playMode == Data.PlayModes.STORYMODE)
        {
            AddAreaByName("continue_Multiplayer");
        }
        else
        {
            //if (!Data.Instance.DEBUG && Data.Instance.playMode == Data.PlayModes.PARTYMODE)
            //	ShuffleMissions ();
            AddAreaByName("start_Multiplayer");
        }
    }
   
    void OnComplete()
    {
        bossResetedOnce = true;
        times_trying_same_mission = 0;
        hasReachedBoss = false;
    }
    //si terminaste la mision matando a un boss:
    void OnBossActive(bool isOn)
    {
        if (!isOn)
            OnComplete();
    }
    //si no...
    void OnMissionComplete(int id)
    {
        hasReachedBoss = false;
        times_trying_same_mission = 0;

        if (MissionActiveID >= MissionsManager.Instance.missions.Count - 1)
        {
            Game.Instance.GotoVideogameComplete();
        }
        else
        {
            MissionActiveID++;
            print("StartNewMission " + MissionActiveID);
            UserData.Instance.SetMissionReady(MissionActiveID);
        }
    }
    public int GetTotalMissions()
    {
        return MissionsManager.Instance.missions.Count;
    }
    public List<MissionsManager.MissionsData> GetMissionsByVideoGame(int videogameID)
	{
		return MissionsManager.Instance.missions;
	}
	void NextMission()
	{
      //  print("________________mjission active " + MissionActiveID + "    videoga,e: " + Data.Instance.videogamesData.actualID);
      //  if(Data.Instance.videogamesData.actualID == 2) MissionActiveID++;
  //      AddAreaByName("newLevel_playing");
  //      StartNewMission ();
		//Events.OnChangeBackgroundSide (MissionActive.fondo);
	}
	void StartNewMission()
	{
		areaSetId = 0;
		ResetAreaSet ();
        if (Data.Instance.playMode != Data.PlayModes.SURVIVAL)
			MissionActive = MissionsManager.Instance.missions[MissionActiveID].data[0];
		this.missionCompletedPercent = 0;
    }
	public MissionData GetActualMissionData()
	{
        if (Data.Instance.playMode == Data.PlayModes.SURVIVAL)
            return MissionActive;
        else
            return MissionsManager.Instance.missions[MissionActiveID].data[0];
	}
	public MissionData GetMission(int videoGameID, int missionID)
	{
		return MissionsManager.Instance.missions[missionID].data[0];
	}
	public int GetActualMissionByVideogame()
	{
		int viedogameActive = videogamesData.actualID;
		int id = 0;
		foreach (MissionData mission in MissionsManager.Instance.missions[0].data) {
			if (mission.id == MissionActive.id)
				return id;
			id++;
		}
		return 0;
	}
	public void OnUpdateDistance(float distance)
	{
        if (areasetDataLoaded.Count > 0)
        {
            MissionData.AreaSetData m = areasetDataLoaded[0];
            if (distance > m.totalDistanceToCamFX)
                OnAvatarReachedNextArea(m);
        }
		
        // if (areasetDataLoaded.Count>0 && distance > areasetDataLoaded[0].totalDistanceToCamFX)
            // OnAvatarReachedNextArea(areasetDataLoaded[0]);
        if (distance > areasLength-offset) {
			SetNextArea ();
		}
	}
    
    int total_areas = 1;
    float areasetIDLastAdded = -1;
	void SetNextArea()
	{
        MissionData.AreaSetData data = MissionActive.areaSetData[areaSetId];

       
        if (areasetIDLastAdded != areaSetId)
        {
            areasetIDLastAdded = areaSetId;
            data.totalDistanceToCamFX = (int)areasLength;
            areasetDataLoaded.Add(data);
        }

        total_areas = data.total_areas;

        if (Data.Instance.playMode != Data.PlayModes.SURVIVAL && (Data.Instance.playOnlyBosses || hasReachedBoss) && !data.boss && areaSetId < MissionActive.areaSetData.Count - 2)
        {
            areaSetId++;
            ResetAreaSet();
            SetNextArea();
            print("SetNextArea " + areaSetId);
            return;
        }

        if (data.boss && Data.Instance.playMode != Data.PlayModes.STORYMODE && !bossResetedOnce)
            hasReachedBoss = true;

        CreateCurrentArea ();

       // Debug.Log("areaSetId: " + areaSetId + "   data.cameraOrientation: " + data.cameraOrientation + " bending: " + data.bending);
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
    void OnAvatarReachedNextArea(MissionData.AreaSetData data)
    {
      //  print("__________ distance: "  + data.totalDistanceToCamFX +  " areaName: " + data.areas[0] +   " cam: "  + data.cameraOrientation + " bending: " + data.bending);
        Game.Instance.gameCamera.SetOrientation(data.cameraOrientation);
        if (data.bending != 0)
            Events.ChangeCurvedWorldX(data.bending);
        areasetDataLoaded.RemoveAt(0);
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
    AreasManager areasManager;
    void AddAreaByName(string areaName, bool isXtra = false)
    {
        if (areasManager == null)
            areasManager = MissionsManager.Instance.areasManager;
        areaDataActive = areasManager.GetArea(areaName);
       // TextAsset asset = Resources.Load ("areas/" + areaName ) as TextAsset;
		if (areaDataActive != null) {					
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
                    AreaData areaData = MissionsManager.Instance.areasManager.GetArea(areaName);
                   // TextAsset asset = Resources.Load("areas/" + areaName) as TextAsset;
                    if (areaData != null)
                    {
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
