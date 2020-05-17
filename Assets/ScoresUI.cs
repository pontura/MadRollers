using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoresUI : MonoBehaviour
{
    
    [SerializeField] private GameObject panel;

    [SerializeField] private AvatarThumb myAvatarThumb;

    [SerializeField] private AvatarThumb otherAvatarThumb;
    [SerializeField] private Text otherScore;
    [SerializeField] private Text otherName;

    void Start()
    {
        if (!Data.Instance.isAndroid)
        {
            panel.SetActive(false);
            Destroy(this);
        }
        else
        {
            panel.SetActive(true);
            Data.Instance.events.OnMissionStart += OnMissionStart;
            myAvatarThumb.Init(UserData.Instance.userID);
        }
    }
    void OnDestroy()
    {
        Data.Instance.events.OnMissionStart -= OnMissionStart;
    }

    void OnMissionStart(int missionID)
    {
        if (Data.Instance.isAndroid)
        {
            int videoGameID = Data.Instance.videogamesData.actualID;
            HiscoresByMissions.MissionHiscoreUserData hiscoreData = UserData.Instance.hiscoresByMissions.GetHiscore(videoGameID, missionID);
            if (hiscoreData == null)
            {
                print("no hay hiscore de videoGameID: " + videoGameID + " mission " + missionID);
                otherAvatarThumb.gameObject.SetActive(false);
            }
            else
            {
                otherAvatarThumb.Init(hiscoreData.userID);
                otherScore.text = Utils.FormatNumbers(hiscoreData.score);
                otherName.text = hiscoreData.username;
            }
        }
    }
}
