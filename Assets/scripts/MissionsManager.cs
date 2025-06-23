using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class MissionsManager : MonoBehaviour
{
    [SerializeField] GameObject thisPrefab;
    public int VideogameIDForTorneo = 100;
    public TextAsset _all;
    public TextAsset _all_partymode;
    // public MissionsListInVideoGame all;
    public MissionsList all;
    public List<MissionsData> missions;
    public List<MissionsData> missionsSurvival;
    public AreasManager areasManager;
    [Serializable]
    public class MissionsList
    {
        public string[] missions;
        //public string[] missionsVideoGame1;
        //public string[] missionsVideoGame2;
        //public string[] missionsVideoGame3;
        public string[] torneo;
    }
    [Serializable]
    public class MissionsData
    {
        public List<MissionData> data;
    }
    public MissionData GetMission(int id)
    {
        return missions[id].data[0];
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
    public void LoadAll(Data.PlayModes playmode)
    {
        Debug.Log("Load all missions from Resources playmode: " + playmode);
        areasManager = GetComponent<AreasManager>();
        areasManager.Init();

        if(playmode == Data.PlayModes.PARTYMODE)
            all = JsonUtility.FromJson<MissionsList>(_all_partymode.text);
        else
            all = JsonUtility.FromJson<MissionsList>(_all.text);

        Load(all.missions);

#if UNITY_EDITOR
        PrefabUtility.ApplyPrefabInstance(thisPrefab, InteractionMode.UserAction);
#endif
    }
    public void Load(string[] m)
    {
        missions = new List<MissionsData>();
        foreach (string missionName in m)
        {
            missions.Add(LoadDataFromMission("missions", missionName));
        }
    }
    public MissionsData LoadDataFromMission(string folder, string missionName)
    {
        string dataAsJson = LoadResourceTextfile(folder, missionName);
        MissionsData missionData = JsonUtility.FromJson<MissionsData>(dataAsJson);
        missionData.data[0].jsonName = missionName;
        print("______videogameID: " + missionData.data[0].videoGameID);
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
