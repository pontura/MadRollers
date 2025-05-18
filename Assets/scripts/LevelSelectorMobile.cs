using System;
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

    public Text tournamentField;
    public Text hiscoresField;
    public Text pluginsField;

    int score;
    int scoreTo;
    
    void Start()
    {
        if (Data.Instance.playMode == Data.PlayModes.PARTYMODE)
            InitParty();
        else
            InitStoryMode();
    }
    void InitParty()
    {
        Data.Instance.events.OnMadRollersSFXStatus(false);
        //missionSelectorMobile.Init();
       // Data.Instance.multiplayerData.ResetAll();
       // Data.Instance.events.OnResetMultiplayerData();
       // Data.Instance.isReplay = false;
        missionSelectorMobile.Clicked(Data.Instance.videogamesData.actualID, Data.Instance.missions.MissionActiveID);
    }
    void InitStoryMode()
    { 
        tournamentField.text = TextsManager.Instance.GetText("TOURNAMENT");
        hiscoresField.text = TextsManager.Instance.GetText("HI-SCORES");
        pluginsField.text = TextsManager.Instance.GetText("PLUG-INS");

        avatarThumb.Init(UserData.Instance.userID);
        avatarName.text = UserData.Instance.Username;

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
        VoicesManager.Instance.PlaySpecificClipFromList(VoicesManager.Instance.UIItems, 0);

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
    void LoopForScore()
    {
        if (score == scoreTo)
            return;
        score += (int)(((float)scoreTo - (float)score) / 4f);
        if (score > scoreTo)
            score = scoreTo;
        scoreField.text = Utils.FormatNumbers(score);
        Invoke("LoopForScore", Time.deltaTime * 5);
    }
    public void Go()
    {
       // Data.Instance.playMode = Data.PlayModes.STORYMODE;
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
