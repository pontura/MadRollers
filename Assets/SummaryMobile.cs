﻿using System.Collections;
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
    public Text puestoField;
    public Text initialSignalTitleField;

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
            Data.Instance.events.OnMadRollersSFXStatus(false);
            hiscoreOtherPanel.SetActive(false);
            Data.Instance.events.RalentaTo(0, 0.005f);
            panel.SetActive(true);
            if (Data.Instance.playMode == Data.PlayModes.SURVIVAL)
                Data.Instance.handWriting.WriteTo(initialSignalTitleField, "YOU ARE DONE!", NextScreen);
            else
                Data.Instance.handWriting.WriteTo(initialSignalTitleField, "DISKETTE DESTROYED!", NextScreen);
            StartCoroutine(InitCoroutine());
        } 
    }
    private void OnDestroy()
    {
        Data.Instance.events.OnJoystickClick -= OnJoystickClick;
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

        if (Data.Instance.playMode == Data.PlayModes.SURVIVAL)
        {
            UserData.Instance.hiscoresByMissions.SaveSurvivalScore();
            videoGameID = MissionsManager.Instance.VideogameIDForTorneo;
        }
        else
            videoGameID = Data.Instance.videogamesData.actualID;

        Debug.Log("____OnSaveScore");
        Data.Instance.events.OnSaveScore();

        yield return new WaitForSecondsRealtime(4);
        //Data.Instance.events.RalentaTo(0, 0.025f);



        UserData.Instance.hiscoresByMissions.LoadHiscore(videoGameID, missionID, HiscoreLoaded);
        if (!Data.Instance.isAndroid)
            Data.Instance.events.OnJoystickClick += OnJoystickClick;
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

        avatarImage.Init(UserData.Instance.userID);
        usernameField.text = UserData.Instance.username;
        
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
        Data.Instance.musicManager.stopAllSounds();
        Data.Instance.isReplay = false;
        // Game.Instance.ResetLevel();
        Data.Instance.events.OnResetLevel();
        Data.Instance.LoadLevel("LevelSelectorMobile");
        Data.Instance.events.ForceFrameRate(1);
    }
    public void Retry()
    {
        Data.Instance.events.OnResetScores();
        Data.Instance.events.ForceFrameRate(1);
        Data.Instance.missions.MissionActiveID--;
        if (Data.Instance.missions.MissionActiveID < 0)
            Data.Instance.missions.MissionActiveID = 0;
        Game.Instance.Continue();
    }
    //public void Exit()
    //{
    //    Data.Instance.events.OnResetScores();
    //    Data.Instance.events.ForceFrameRate(1);
    //    Game.Instance.GotoMainMobile();
    //}
}
