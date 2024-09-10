using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HiscoresByMissions : MonoBehaviour
{
    private string secretKey = "pontura";
    string saveNewHiscore = "saveHiscore.php";
    string getHiscore = "getHiscore.php";

    public bool loaded;

    public List<MissionHiscoreData> all;

    [Serializable]
    public class MissionHiscoreData
    {
        public int mission;
        public int videogame;
        public List<MissionHiscoreUserData> all;
    }
    [Serializable]
    public class MissionHiscoreUserData
    {
        [HideInInspector]
        public int mission;
        [HideInInspector]
        public int videogame;
        public string userID;
        public string username;
        public int score;
    }
    public void Init()
    {
        if(Data.Instance.playMode == Data.PlayModes.STORYMODE)
            Data.Instance.events.OnMissionComplete += OnMissionComplete;
    }
    public void ResetAllHiscores()
    {
        all.Clear();
    }
    private void OnDestroy()
    {
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
    }    
    public void SaveSurvivalScore()
    {
        Save(0, Data.Instance.multiplayerData.score);
    }
    void OnMissionComplete(int missionID)
    {
        Save(missionID, Data.Instance.multiplayerData.score);
        Invoke("Delayed", 1);
    }
    void Delayed()
    {
        Data.Instance.multiplayerData.score = 0;
    }
    public void CheckToAddNewHiscore(string userID, int score, int videogame, int mission)
    {
        MissionHiscoreData md = IfAlreadyLoaded(videogame, mission);
        if (md == null) return;
        int id = 0;
        int addNewHiscoreID = 0;
        foreach (MissionHiscoreUserData mhd in md.all)
        {
            if (mhd.userID == userID)
                return; //ya tenias un hiscore mayor
            if (mhd.score < score && addNewHiscoreID == 0)
                addNewHiscoreID = id;
            id++;
        }
        if (addNewHiscoreID != 0)
        {
            foreach (MissionHiscoreData md2 in all)
            {
                if (md2.videogame == videogame && md2.mission == mission)
                {
                    MissionHiscoreUserData mhd = new MissionHiscoreUserData();
                    mhd.userID = userID;
                    mhd.score = score;
                    mhd.videogame = videogame;
                    mhd.mission = mission;
                    md.all.Insert(addNewHiscoreID, mhd);
                }
            }
        }
    }
    public void LoadHiscore(int videogame, int mission, System.Action<MissionHiscoreData> OnDone)
    {
       // OnDone(null);

        if (Data.Instance.playMode == Data.PlayModes.SURVIVAL)
        {
            mission = 0;
            videogame = MissionsManager.Instance.VideogameIDForTorneo;
        }

        MissionHiscoreData md = IfAlreadyLoaded(videogame, mission);

        if (md != null)
        {
            OnDone(md);
            return;
        }

        string post_url = UserData.Instance.URL + getHiscore;
        post_url += "?videogame=" + (videogame);
        post_url += "&mission=" + mission;
        post_url += "&limit=50";
        StartCoroutine(Send(post_url, OnDone));
    }
    MissionHiscoreData IfAlreadyLoaded(int videogame, int mission)
    {
        foreach(MissionHiscoreData md in all)
        {
            if (md.videogame == videogame && md.mission == mission)
                return md;
        }
        return null;
    }
    public void Save(int mission, int score)
    {
        int videogame;

        if (Data.Instance.playMode == Data.PlayModes.SURVIVAL)
            videogame = MissionsManager.Instance.VideogameIDForTorneo;
        else
            videogame = Data.Instance.videogamesData.actualID;

        string hash = Utils.Md5Sum(UserData.Instance.userID + videogame + mission + score + secretKey);
        string post_url = UserData.Instance.URL + saveNewHiscore + "?userID=" + WWW.EscapeURL(UserData.Instance.userID);
        post_url += "&username=" + UserData.Instance.username;
        post_url += "&videogame=" + videogame;
        post_url += "&mission=" + mission;
        post_url += "&score=" + score;
        post_url += "&hash=" + hash;

        print("SAVE: " + post_url);

        StartCoroutine( Send(post_url, null) );
    }
    IEnumerator Send(string post_url, System.Action<MissionHiscoreData> OnDone)
    {
        print("Hiscores SEND: " +  post_url);
        WWW www = new WWW(post_url);
        yield return www;

        if (www.error != null)
        {
            //UsersEvents.OnPopup("Internet Error: " + www.error);
           if (OnDone != null)
                OnDone(null);
        }
        else
        {
            if (OnDone != null)
                OnDataSended(www.text, OnDone);
        }
    }
    void OnDataSended(string result, System.Action<MissionHiscoreData> OnDone)
    {       
        MissionHiscoreData missionHiscoreData = JsonUtility.FromJson<MissionHiscoreData>(result);

        print("Hiscores OnDataSended: " + missionHiscoreData.all.Count);
        if (missionHiscoreData.all.Count == 0)
        {
            OnDone(null);
            return;
        }
     

        MissionHiscoreData md = IfAlreadyLoaded(missionHiscoreData.videogame, missionHiscoreData.mission);

        if (md == null)
            all.Add(missionHiscoreData);

        OnDone(missionHiscoreData);
        print("Hiscores missionHiscoreData: " + missionHiscoreData + " md: " + md);
    }
    public MissionHiscoreUserData GetHiscore(int videogame, int mission)
    {
        foreach (MissionHiscoreData md in all)
        {
            if (md.videogame == videogame && md.mission == mission)
            {
                return md.all[0];
            }
        }
        return null;
    }

}
