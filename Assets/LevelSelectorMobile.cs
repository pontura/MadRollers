﻿using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectorMobile : MonoBehaviour
{
    VideogameData videogameData;
    public GameObject torneoButton;
    public MissionSelectorMobile missionSelectorMobile;

    public AvatarThumb avatarThumb;
    public Text scoreField;
    public Text avatarName;

    int score;
    int scoreTo;

    void LoopForScore()
    {
        if (score == scoreTo)
            return;
        score += (int)(((float)scoreTo - (float)score )/ 4f);
        if (score > scoreTo)
            score = scoreTo;
        scoreField.text = Utils.FormatNumbers(score);
        Invoke("LoopForScore", Time.deltaTime*5);
    }
    void Start()
    {
        avatarThumb.Init(UserData.Instance.userID);
        avatarName.text = UserData.Instance.username.ToUpper();

        scoreTo =  UserData.Instance.Score();
        score = UserData.Instance.GetLastScoreWon();

        if (score == scoreTo || score == 0)
            scoreField.text = Utils.FormatNumbers(scoreTo);
        else
        {
           
            LoopForScore();
        }


        Data.Instance.events.SetHamburguerButton(true);
        Data.Instance.events.OnMadRollersSFXStatus(false);
        missionSelectorMobile.Init();
        Data.Instance.multiplayerData.ResetAll();
        Data.Instance.events.OnResetMultiplayerData();
        Data.Instance.isReplay = false;
        Data.Instance.voicesManager.PlaySpecificClipFromList(Data.Instance.voicesManager.UIItems, 0);

        switch (UserData.Instance.playerID)
        {
            case 0:
                Data.Instance.multiplayerData.player1 = true;
                break;
            case 1:
                Data.Instance.multiplayerData.player2 = true;
                break;
            case 2:
                Data.Instance.multiplayerData.player3 = true;
                break;
            default:
                Data.Instance.multiplayerData.player4 = true;
                break;
        }
    }
    public void Go()
    {
        Data.Instance.playMode = Data.PlayModes.STORYMODE;
        Data.Instance.LoadLevel("Game");
    }
    public void Torneo()
    {
        if(UserData.Instance.missionUnblockedID_3<=0)
        {
            Data.Instance.events.OnAlertSignal("TORNEO: Solo para Avanzados (desbloqueá los 3 juegos)");
            return;
        }
        Data.Instance.videogamesData.actualID = UnityEngine.Random.Range(0, 3);
        Data.Instance.missions.MissionActiveID = 0;
        Data.Instance.playMode = Data.PlayModes.SURVIVAL;
        Data.Instance.LoadLevel("Game");
    }
    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        Start();
    }
    public void EditUser()
    {
        Data.Instance.LoadLevel("Registration");
    }
    public void Plugins()
    {
        Data.Instance.events.OnAlertSignal("Todavía no puedes gastar tus pixeles para construir plugins! (Próximamente)");
    }
    
}
