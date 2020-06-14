using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MissionsManager : MonoBehaviour
{
    public int VideogameIDForTorneo = 100;
    public TextAsset _all;
    public MissionsListInVideoGame all;
    public List<MissionsByVideoGame> videogames;
    public AreasManager areasManager;
    [Serializable]
    public class MissionsListInVideoGame
    {
        public string[] missionsVideoGame1;
        public string[] missionsVideoGame2;
        public string[] missionsVideoGame3;
        public string[] torneo;
    }
    [Serializable]
    public class MissionsByVideoGame
    {
        public List<MissionsData> missions;
    }
    [Serializable]
    public class MissionsData
    {
        public string title;
        public List<MissionData> data;
    }


    static MissionsManager mInstance = null;
    public static MissionsManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                Debug.LogError("Algo llama a MissionsData antes de inicializarse");
            }
            return mInstance;
        }
    }
    private void Awake()
    {
        if (!mInstance)
            mInstance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }
        areasManager = GetComponent<AreasManager>();
        DontDestroyOnLoad(this);       
    }
    public void LoadAll()
    {
        Debug.Log("Load all missions from Resources");
        areasManager = GetComponent<AreasManager>();
        areasManager.Init();
        all = JsonUtility.FromJson<MissionsListInVideoGame>(_all.text);

        LoadByVideogame(all.missionsVideoGame1, 0);
        LoadByVideogame(all.missionsVideoGame2, 1);
        LoadByVideogame(all.missionsVideoGame3, 2);
        LoadByVideogame(all.torneo, 3);
    }
    public void LoadByVideogame(string[] missionsInVideogame, int videogameID)
    {
        MissionsByVideoGame videogame = videogames[videogameID];
        videogame.missions = new List<MissionsData>();
        foreach (string missionName in missionsInVideogame)
        {
            videogame.missions.Add(LoadDataFromMission("missions", missionName));
        }
    }
    public MissionsData LoadDataFromMission(string folder, string missionName)
    {
        string dataAsJson = LoadResourceTextfile(folder, missionName);
        MissionsData missionData = JsonUtility.FromJson<MissionsData>(dataAsJson);
        missionData.data[0].jsonName = missionName;
        foreach (MissionData.AreaSetData areasSetData in missionData.data[0].areaSetData)
        {
            foreach (string areaName in areasSetData.areas)
                areasManager.Add(areaName); 
        }
        return missionData;
    }
    public string LoadResourceTextfile(string folder, string path)
    {
        string filePath = folder + "/" + path.Replace(".json", "");
        
        TextAsset targetFile = Resources.Load<TextAsset>(filePath);
        return targetFile.text;
    }

}
