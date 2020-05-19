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
    int missionID;
    int videoGameID;
    int score;

    void Start()
    {
        panel.SetActive(false);
    }
    
    public void Init()
    {
        hiscoreOtherPanel.SetActive(false);
        Data.Instance.events.RalentaTo(0, 0.025f);
        panel.SetActive(true);
        missionID = Data.Instance.missions.MissionActiveID-1;
        titleField.text = "MISION " + (missionID+1);
        score = Data.Instance.multiplayerData.GetTotalScore();
        scoreField.text = Utils.FormatNumbers(score);
        videoGameID = Data.Instance.videogamesData.actualID;
        UserData.Instance.hiscoresByMissions.LoadHiscore(videoGameID, missionID, HiscoreLoaded);        
    }
    void HiscoreLoaded(HiscoresByMissions.MissionHiscoreData hiscoreData)
    {
        hiscores.Init(videoGameID, missionID, MyScoreLoaded);
        avatarImage.Init(UserData.Instance.userID);
        usernameField.text = UserData.Instance.username;
        if (hiscoreData == null || hiscoreData.all.Count < 1)
        {
            Debug.Log("No ranking yet for videoGameID " + videoGameID + ", mission " + missionID);
        }
        else
        {
            hiscoreOtherPanel.SetActive(true);
            float p = (float)score / (float)hiscoreData.all[0].score;
            progressBar.gameObject.SetActive(true);
            progressBar.SetProgression(p);
            hiscoreAvatarThumb.Init(hiscoreData.all[0].userID);
            hiscoreScoreField.text = Utils.FormatNumbers(hiscoreData.all[0].score);
            hiscoreNameField.text = hiscoreData.all[0].username.ToUpper();
        }
    }
    void MyScoreLoaded(int a) { }
    public void Next()
    {
        Data.Instance.events.OnResetScores();
        Data.Instance.events.FreezeCharacters(true);
        Data.Instance.GetComponent<MusicManager>().stopAllSounds();
        Data.Instance.isReplay = false;
        Game.Instance.ResetLevel();
        Data.Instance.events.ForceFrameRate(1);
    }
    public void Retry()
    {
        Data.Instance.events.OnResetScores();
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
