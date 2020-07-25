using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoresUI : MonoBehaviour
{

    [SerializeField] private Image hiscoreBar;
   // [SerializeField] private GameObject panel;

   // [SerializeField] private AvatarThumb myAvatarThumb;

    //[SerializeField] private AvatarThumb otherAvatarThumb;
    [SerializeField] private Text otherScore;
    [SerializeField] private Text otherName;

    float hiscore;

    void Start()
    {
        if (Data.Instance.playMode != Data.PlayModes.STORYMODE || Data.Instance.playMode == Data.PlayModes.SURVIVAL)
        {
           /// panel.SetActive(false);
            Destroy(this);
        }
       // panel.SetActive(true);
      //  Data.Instance.events.OnMissionStart += OnMissionStart;
      //  myAvatarThumb.Init(UserData.Instance.userID);
        Loop();
    }
    private void Loop()
    {
        int score = Data.Instance.multiplayerData.score;
        if (hiscore > 0 && score > 0)
        {
            float t = (float)score / (float)hiscore;
            hiscoreBar.fillAmount = t;
        }
        Invoke("Loop", 0.1f);
    }
    void OnDestroy()
    {
        Data.Instance.events.OnMissionStart -= OnMissionStart;
    }

    void OnMissionStart(int missionID)
    {
        if (Data.Instance.playMode == Data.PlayModes.STORYMODE)
        {
            int videoGameID = Data.Instance.videogamesData.actualID;
            UserData.Instance.hiscoresByMissions.LoadHiscore(videoGameID, missionID, HiscoreLoaded);           
        }
        else  if( Data.Instance.playMode == Data.PlayModes.SURVIVAL)
            UserData.Instance.hiscoresByMissions.LoadHiscore(MissionsManager.Instance.VideogameIDForTorneo, missionID, HiscoreLoaded);
    }
    void HiscoreLoaded(HiscoresByMissions.MissionHiscoreData hiscoreData)
    {
        if (hiscoreData == null || hiscoreData.all.Count <1 )
        {
           // otherAvatarThumb.gameObject.SetActive(false);
        }
        else
        {
          //  otherAvatarThumb.Init(hiscoreData.all[0].userID);
          //  hiscore = hiscoreData.all[0].score;
          //  otherScore.text = Utils.FormatNumbers((int)hiscore);
         //   otherName.text = hiscoreData.all[0].username.ToUpper();
        }
    }
}
