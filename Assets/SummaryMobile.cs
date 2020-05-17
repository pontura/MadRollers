using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryMobile : MonoBehaviour
{
    public GameObject panel;
    public Text titleField;

    public HiscoresMobile hiscores;
    public AvatarThumb avatarImage;
    public AvatarThumb hiscoreAvatarThumb;

    public Text scoreField;
    public Text usernameField;

    public Text hiscoreScoreField;
    public Text hiscoreNameField;

    public GameObject hiscoreOtherPanel;

    public ProgressBar progressBar;

    bool canClick;

    void Start()
    {
        panel.SetActive(false);
        
    }
    public void Init()
    {
        Data.Instance.events.RalentaTo(0, 0.025f);
        panel.SetActive(true);
        int missionID = Data.Instance.missions.GetActualMissionData().id;
        titleField.text = "MISION " + (missionID + 1);
        int score = Data.Instance.multiplayerData.GetTotalScore();
        scoreField.text = Utils.FormatNumbers(score);
        int videoGameID = Data.Instance.videogamesData.actualID;
        HiscoresByMissions.MissionHiscoreUserData hiscoreData = UserData.Instance.hiscoresByMissions.GetHiscore(videoGameID, missionID);
        hiscores.Init(videoGameID, missionID, MyScoreLoaded);
        avatarImage.Init(UserData.Instance.userID);
        usernameField.text = UserData.Instance.username;
        if (hiscoreData == null)
        {
            hiscoreOtherPanel.SetActive(false);
        }
        else
        {
            float p = (float)score / (float)hiscoreData.score;
            progressBar.gameObject.SetActive(true);
            progressBar.SetProgression(p);
            hiscoreAvatarThumb.Init(hiscoreData.userID);
            hiscoreScoreField.text = Utils.FormatNumbers(hiscoreData.score);
            hiscoreNameField.text = hiscoreData.username;
        }
    }
    void MyScoreLoaded(int a) { }
    public void Next()
    {
        Data.Instance.isReplay = true;
        Game.Instance.ResetLevel();

        Data.Instance.events.ForceFrameRate(1);
        //Data.Instance.events.FreezeCharacters(false);
        //panel.SetActive(false);
    }
    public void Retry()
    {
        Data.Instance.events.ForceFrameRate(1);
      //  Data.Instance.missions.MissionActiveID--;
        Game.Instance.Continue();
    }
    public void Exit()
    {
        Data.Instance.events.ForceFrameRate(1);
        Game.Instance.GotoMainMobile();
    }
}
