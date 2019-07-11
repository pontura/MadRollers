using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryMobile : MonoBehaviour
{
    public GameObject panel;
    public Text titleField;
    public Text scoreField;
    public HiscoresMobile hiscores;
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
        titleField.text = "Misión " + (missionID + 1) + " COMPLETA!";
        int score = Data.Instance.multiplayerData.GetTotalScore();
        scoreField.text = "Score: " + Utils.FormatNumbers(score);
        hiscores.Init(Data.Instance.videogamesData.actualID, missionID, OnMyScoreLoaded);
    }
    void OnMyScoreLoaded(int myscore)
    {
        //
    }
    public void Next()
    {
        Game.Instance.gameCamera.ResetSnapping(0.1f);
        Data.Instance.events.FreezeCharacters(false);
        panel.SetActive(false);
    }
    public void Retry()
    {
        Data.Instance.events.ForceFrameRate(1);
        Data.Instance.missions.MissionActiveID--;
        Game.Instance.Continue();
    }
    public void Exit()
    {
        Data.Instance.events.ForceFrameRate(1);
        Game.Instance.GotoMainMobile();
    }
}
