using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HiscoresByMissions : MonoBehaviour
{
    private string secretKey = "pontura";
    string saveNewHiscore = "http://madrollers.com/game/saveHiscore.php";
    string getHiscore = "http://madrollers.com/game/getHiscore.php";

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
    void OnMissionComplete(int missionID)
    {
        Save(missionID, Data.Instance.multiplayerData.score);
        Invoke("Delayed", 1);
    }
    void Delayed()
    {
        Data.Instance.multiplayerData.score = 0;
    }
    //retorna List<HiscoresByMissions.MissionHiscoreData>
    public void LoadHiscore(int videogame, int mission, System.Action<MissionHiscoreData> OnDone)
    {
        MissionHiscoreData md = IfAlreadyLoaded(videogame, mission);

        if (md != null)
        {
            OnDone(md);
            return;
        }

        string post_url = getHiscore;
        post_url += "?videogame=" + (videogame);
        post_url += "&mission=" + mission;
        post_url += "&limit=10";

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
        int videogame = Data.Instance.videogamesData.actualID;
        string hash = Utils.Md5Sum(UserData.Instance.userID + videogame + mission + score + secretKey);
        string post_url = saveNewHiscore + "?userID=" + WWW.EscapeURL(UserData.Instance.userID);
        post_url += "&username=" + UserData.Instance.username;
        post_url += "&videogame=" + videogame;
        post_url += "&mission=" + mission;
        post_url += "&score=" + score;
        post_url += "&hash=" + hash;

        StartCoroutine( Send(post_url, null) );
    }
    IEnumerator Send(string post_url, System.Action<MissionHiscoreData> OnDone)
    {
        print(post_url);
        WWW www = new WWW(post_url);
        yield return www;

        if (www.error != null)
            UsersEvents.OnPopup("Error sending al server: " + www.error);
        else
        {
            if(OnDone != null)
                OnDataSended( www.text , OnDone);
        }
    }
    void OnDataSended(string result, System.Action<MissionHiscoreData> OnDone)
    {       
        MissionHiscoreData missionHiscoreData = JsonUtility.FromJson<MissionHiscoreData>(result);

        if (missionHiscoreData.all.Count == 0)
        {
            OnDone(null);
            return;
        }
     

        MissionHiscoreData md = IfAlreadyLoaded(missionHiscoreData.videogame, missionHiscoreData.mission);

        if (md == null)
            all.Add(missionHiscoreData);

        OnDone(missionHiscoreData);
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
