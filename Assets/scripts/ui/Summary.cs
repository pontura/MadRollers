using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Summary : MonoBehaviour {
    
    private int countDown;
    [SerializeField] Animation anim;

    [SerializeField] GameObject mobilePanel;
    [SerializeField] ProgressBar routeProgressBar;

    [SerializeField] int optionSelected = 0;
    private bool isOn;

    [SerializeField] Text percentfield;
    [SerializeField] Text missionField;

    [SerializeField] ContinuePanel continuePanel;

	float delayToReact = 0.3f;

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
    void OnGameOver(bool isTimeOver)
    {
        print("on game over");
        /// se hace cargo el continue:
        if (Data.Instance.playMode != Data.PlayModes.PARTYMODE && !Data.Instance.isAndroid)
            return;

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
    float distance;
    float totalDistance;

    void SetOn()
    {
        distance = Game.Instance.level.charactersManager.getDistance();
        totalDistance = Data.Instance.missions.GetTotalRoutDistance();
        Data.Instance.events.RalentaTo(1, 0.05f);
        mobilePanel.SetActive(true);
        int continuePrice = Data.Instance.missions.MissionActive.GetContinuePrice();
        continuePanel.Init(continuePrice);
        StartCoroutine(SetProgress());

        if(distance >= totalDistance)
            missionField.text = TextsManager.Instance.GetText("BOSS") + "! M." + (Data.Instance.missions.MissionActiveID + 1);
        else
            missionField.text = TextsManager.Instance.GetText("DISKETTE") + " " + (Data.Instance.missions.MissionActiveID + 1);
    }
    IEnumerator SetProgress()
    {
        if (distance > totalDistance)
            distance = totalDistance;
        float i = 0;
        while (i < distance)
        {
            i += Time.deltaTime * 300;
            float v = i / totalDistance;
            routeProgressBar.SetProgression(v);
            int value = (int)(v * 100);
            if (value > 99) value = 99;
            percentfield.text = value.ToString() + "%";
            yield return null;
        }
    }
    public void Restart()
	{
		Data.Instance.isReplay = true;
		Game.Instance.ResetLevel();        
	}
}
