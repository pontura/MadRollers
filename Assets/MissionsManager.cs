using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MissionsManager : MonoBehaviour
{
    public TextAsset _all;
    public MissionsListInVideoGame all;
    public List<MissionsByVideoGame> videogames;
    [Serializable]
    public class MissionsListInVideoGame
    {
        public string[] missionsVideoGame1;
        public string[] missionsVideoGame2;
        public string[] missionsVideoGame3;
    }
    [Serializable]
    public class MissionsByVideoGame
    {
        public List<MissionsData> missions;
        public int missionUnblockedID;
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
        DontDestroyOnLoad(this);
        LoadAll();
    }
    public void LoadAll()
    {
        all = JsonUtility.FromJson<MissionsListInVideoGame>(_all.text);

        LoadByVideogame(all.missionsVideoGame1, 0);
        LoadByVideogame(all.missionsVideoGame2, 1);
        LoadByVideogame(all.missionsVideoGame3, 2);
    }
    public void LoadByVideogame(string[] missionsInVideogame, int videogameID)
    {
        MissionsByVideoGame videogame = videogames[videogameID];
        videogame.missions = new List<MissionsData>();
        foreach (string missionName in missionsInVideogame)
        {
            videogame.missions.Add(LoadDataFromMission("missions", missionName));
            videogame.missionUnblockedID = PlayerPrefs.GetInt("missionUnblockedID_" + (videogameID + 1), 0);
        }
    }
    public MissionsData LoadDataFromMission(string folder, string missionName)
    {
        string dataAsJson = LoadResourceTextfile(folder, missionName);
        MissionsData missionData = JsonUtility.FromJson<MissionsData>(dataAsJson);
        missionData.data[0].jsonName = missionName;
        return missionData;
    }
    public string LoadResourceTextfile(string folder, string path)
    {
        string filePath = folder + "/" + path.Replace(".json", "");
        TextAsset targetFile = Resources.Load<TextAsset>(filePath);
        return targetFile.text;
    }

}
