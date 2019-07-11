using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiscoresMobile : MonoBehaviour
{
    public Transform container;
    public ScoreLine scoreLine;
    System.Action<int> MyScoreLoaded;

    public void Init(int videoGameID, int missionID, System.Action<int> MyScoreLoaded)
    {
        this.MyScoreLoaded = MyScoreLoaded;
        Utils.RemoveAllChildsIn(container);
        HiscoresByMissions hiscoresByMission = UserData.Instance.hiscoresByMissions;
        hiscoresByMission.ResetAllHiscores();
        hiscoresByMission.LoadHiscore(videoGameID, missionID, OnDone);       
    }
    public void OnDone(HiscoresByMissions.MissionHiscoreData data)
    {
        int id = 0;
        if (data == null || data.all.Count == 0)
            return;
        foreach (HiscoresByMissions.MissionHiscoreUserData m in data.all)
        {
            ScoreLine newLine = Instantiate(scoreLine);
            newLine.transform.SetParent(container);
            newLine.transform.localPosition = Vector3.zero;
            newLine.transform.localScale = Vector3.one;
            newLine.Init(id + 1, m.username, m.score);
            newLine.SetImage(m.userID);
            if (m.userID == UserData.Instance.userID)
                MyScoreLoaded(m.score);
            id++;
        }
    }
}

