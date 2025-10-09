using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static HiscoresByMissions;

public class SummaryMobile : MonoBehaviour
{
    public GameObject panel;
    public TMPro.TMP_Text titleField;

    public Stars stars;
    public HiscoresMobile hiscores;
   // public AvatarThumb avatarImage;
   // public AvatarThumb hiscoreAvatarThumb;

    public TMPro.TMP_Text scoreField;
    public TMPro.TMP_Text usernameField;

    public TMPro.TMP_Text hiscoreScoreField;
    public TMPro.TMP_Text hiscoreNameField;
    public TMPro.TMP_Text puestoField;
    public TMPro.TMP_Text initialSignalTitleField;

    public GameObject hiscoreOtherPanel;

    public ProgressBar progressBar;

    bool canClick;
    int missionID;
    int videoGameID;
    int score;

    void Start()
    {
        if (Data.Instance.playMode == Data.PlayModes.PARTYMODE)
            return;
        puestoField.text = "";
        panel.SetActive(false);
    }
    
    public void Init()
    {
        if (Data.Instance.playMode == Data.PlayModes.STORYMODE || Data.Instance.playMode == Data.PlayModes.SURVIVAL)
        {
            Events.OnMadRollersSFXStatus(false);
            hiscoreOtherPanel.SetActive(false);
            Events.RalentaTo(0, 0.005f);
            panel.SetActive(true);
            StartCoroutine(InitCoroutine());
        } 
    }
    void SetStars()
    {
        string result = "MISSION COMPLETE!";
        int starsNum = 0;

        if (score > 12000)
        {
            result = "A glorious victory!";
            starsNum = 3;
        }
        else if (score > 8000)
        {
            result = "A solid success!";
            starsNum = 2;
        }
        else if (score > 4000)
        {
            result = "Job done, no heroics.";
            starsNum = 1;
        }

        Data.Instance.handWriting.WriteTo(initialSignalTitleField, result.ToUpper(), NextScreen);

        stars.Init(starsNum);
    }
    private void OnDestroy()
    {
        Events.OnJoystickClick -= OnJoystickClick;
    }
    void OnJoystickClick()
    {
        Next();
    }
    void NextScreen()   { }
    bool canShowHiscores;
    IEnumerator InitCoroutine()
    {
        missionID = Data.Instance.missions.MissionActiveID - 1;
        titleField.text = TextsManager.Instance.GetText("DISKETTE") + " " + (missionID + 1);
        score = Data.Instance.multiplayerData.GetTotalScore();
        scoreField.text = Utils.FormatNumbers(score);
        SetStars();

        if (Data.Instance.playMode == Data.PlayModes.SURVIVAL)
        {
            UserData.Instance.hiscoresByMissions.SaveSurvivalScore();
            videoGameID = MissionsManager.Instance.VideogameIDForTorneo;
        }
        else
            videoGameID = Data.Instance.videogamesData.actualID;

        Debug.Log("____OnSaveScore: " + score);
        Events.OnSaveScore();

        yield return new WaitForSecondsRealtime(4);
        //Events.RalentaTo(0, 0.025f);


        //HiscoreLoaded(null);//To-DO
        UserData.Instance.hiscoresByMissions.LoadHiscore(missionID, HiscoreLoaded);
        //if (!Data.Instance.isAndroid)
        //    Events.OnJoystickClick += OnJoystickClick;
    }
    void HiscoreLoaded(HiscoresByMissions.MissionHiscoreData hiscoreData)
    {
        if (Data.Instance.playMode == Data.PlayModes.SURVIVAL)
        {
            videoGameID = MissionsManager.Instance.VideogameIDForTorneo;
            missionID = 0;
        }

        UserData.Instance.hiscoresByMissions.CheckToAddNewHiscore(UserData.Instance.userID, score, videoGameID, missionID);
        hiscores.InitLoaded(hiscoreData);

     //   avatarImage.Init(UserData.Instance.userID);
        usernameField.text = UserData.Instance.username.ToUpper();
        
        if (hiscoreData == null || hiscoreData.all.Count < 1)
        {
            Debug.Log("No ranking yet for videoGameID " + videoGameID + ", mission " + missionID);
        }
        else
        {
            int puesto = 1;
            foreach (HiscoresByMissions.MissionHiscoreUserData data in hiscoreData.all)
            {
                if (data.userID == UserData.Instance.userID)
                    puestoField.text = TextsManager.Instance.GetText("RANK") + " " + puesto;
                puesto ++;
            }
            hiscoreOtherPanel.SetActive(true);
            float p = (float)score / (float)hiscoreData.all[0].score;
            progressBar.gameObject.SetActive(true);
            progressBar.SetProgression(p);
            //hiscoreAvatarThumb.Init(hiscoreData.all[0].userID);
            hiscoreScoreField.text = Utils.FormatNumbers(hiscoreData.all[0].score);
            hiscoreNameField.text = hiscoreData.all[0].username.ToUpper();
        }
    }
    void MyScoreLoaded(int a) { }
    public void Next()
    {
        Data.Instance.videogamesData.UpdateVideogame();
        Events.OnResetScores();
        Events.ForceFrameRate(1);
        Game.Instance.PlayAgain();
    }
    public void ChangeVideoGame()
    {
        Events.OnResetScores();
        Events.FreezeCharacters(true);
        MusicManager.Instance.stopAllSounds();
        Data.Instance.isReplay = false;
        // Game.Instance.ResetLevel();
        Events.OnResetLevel();
        Data.Instance.LoadLevel("LevelSelectorMobile");
        Events.ForceFrameRate(1);
    }
    public void Retry()
    {
        Events.OnResetScores();
        Events.ForceFrameRate(1);
        Data.Instance.missions.MissionActiveID--;
        if (Data.Instance.missions.MissionActiveID < 0)
            Data.Instance.missions.MissionActiveID = 0;
        Game.Instance.PlayAgain();
    }
    //public void Exit()
    //{
    //    Events.OnResetScores();
    //    Events.ForceFrameRate(1);
    //    Game.Instance.GotoMainMobile();
    //}
}
