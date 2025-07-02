using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Summary : MonoBehaviour {
    
    [SerializeField] Animation anim;

    [SerializeField] GameObject mobilePanel;
    [SerializeField] ProgressBar routeProgressBar;
    [SerializeField] MissionBar missionBarBoss;

    private bool isOn;

    [SerializeField] TMPro.TMP_Text percentfield;
    [SerializeField] TMPro.TMP_Text missionField;

    [SerializeField] ContinuePanel continuePanel;


    void Start()
    {
        SetOff();
            Data.Instance.events.OnGameOver += OnGameOver;
       if (Data.Instance.playMode == Data.PlayModes.PARTYMODE  )
            Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnContinue += OnContinue;
    }
    public void SetOff()
    {
        mobilePanel.SetActive(false);
    }
    void OnDestroy()
    {
        Data.Instance.events.OnGameOver -= OnGameOver;
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
        Data.Instance.events.OnContinue -= OnContinue;
    }
    void OnContinue()
    {
        isOn = false;
        anim.Play("summaryMobileClose");
        Invoke("SetOffDelayed", 0.5f);
    }
    void SetOffDelayed()
    {
        SetOff();
    }
    void OnMissionComplete(int missionID)
    {
        Invoke("SetOnPartyMode", 2F);
    }
    bool isTimeOver;
    void OnGameOver(bool isTimeOver)
    {
        this.isTimeOver = isTimeOver;
        print("on game over " + isTimeOver);
        if(isTimeOver)
            Game.Instance.GameOver();


        if (isOn) return;
        isOn = true;
        
        if(Data.Instance.playMode == Data.PlayModes.SURVIVAL)
            Invoke("GotoDirectToSummary", 2F);
        else if (Data.Instance.playMode != Data.PlayModes.PARTYMODE)
            Invoke("SetOn", 1.5F);
    }
    
    void GotoDirectToSummary()
    {
        GetComponent<SummaryMobile>().Init();
    }
    void SetOnPartyMode()
    {
        print("SetOnPartyMode");
        Data.Instance.isReplay = true;
        Game.Instance.GotoNextGame();
    }
    void SetOn()
    {
        float value;
        float total;
        Data.Instance.events.RalentaTo(1, 0.05f);
        mobilePanel.SetActive(true);
        int continuePrice = Data.Instance.missions.MissionActive.GetContinuePrice();

        if (isTimeOver)
            continuePanel.SetOff();
        else
            continuePanel.Init(continuePrice);

        if(missionBarBoss.isOn)
        {
            missionField.text = TextsManager.Instance.GetText("BOSS") + "! M." + (Data.Instance.missions.MissionActiveID + 1);
            value = 1-missionBarBoss.progress;
            if (value == 1) value = 0.01f;
            total = 1;
        }
        else
        {
            missionField.text = TextsManager.Instance.GetText("DISKETTE") + " " + (Data.Instance.missions.MissionActiveID + 1);
            value = Game.Instance.level.charactersManager.getDistance();
            total = Data.Instance.missions.GetTotalRoutDistance();
        }
        StartCoroutine(SetProgress(value, total));
    }
            
    IEnumerator SetProgress(float value, float total)
    {
        routeProgressBar.SetProgression(0);
        percentfield.text = "0%";
        yield return new WaitForSeconds(0.5f);
        if (value > total)
            value = total;
        float i = 0;
        while (i < value)
        {
            i += Time.deltaTime * total;
            float v1 = i / total;
            routeProgressBar.SetProgression(v1);
            int v = (int)(v1 * 100);
            if (v > 99) v = 99;
            percentfield.text = v.ToString() + "%";
            yield return null;
        }
    }
    public void Restart()
	{
		Data.Instance.isReplay = true;
		Game.Instance.ResetLevel();        
	}
    public void Replay()
    {
        Data.Instance.events.OnResetScores();
        Data.Instance.isReplay = true;
        Game.Instance.ResetLevel();
    }
    public void Exit()
    {
         Game.Instance.GotoLevelSelector();
    }
}
