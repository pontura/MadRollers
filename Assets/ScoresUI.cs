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
            print("get hiscore missionID: " + missionID);
            UserData.Instance.hiscoresByMissions.LoadHiscore(videoGameID, missionID, HiscoreLoaded);           
        }
    }
    void HiscoreLoaded(HiscoresByMissions.MissionHiscoreData hiscoreData)
    {
        if (hiscoreData == null || hiscoreData.all.Count <1 )
        {
            otherAvatarThumb.gameObject.SetActive(false);
        }
        else
        {
            otherAvatarThumb.Init(hiscoreData.all[0].userID);
            otherScore.text = Utils.FormatNumbers(hiscoreData.all[0].score);
            otherName.text = hiscoreData.all[0].username.ToUpper();
        }
    }
}
