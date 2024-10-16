﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HiscoresComparison : MonoBehaviour {

	public HiscoresComparisonSignal signal;
	public GameObject panel;
	public Transform container;
	ArcadeRanking arcadeRanking;
	public float hiscore;
	public float topTenPercent;
	public Image topTenImage;
	HiscoresComparisonSignal mySignal;
	int score;
	int puesto;
    int rankingNum = 5;

    void Start () {
        if (Data.Instance.playMode == Data.PlayModes.STORYMODE || Data.Instance.playMode == Data.PlayModes.SURVIVAL)
            this.enabled = false;

        panel.SetActive (false);
		arcadeRanking = Data.Instance.GetComponent<ArcadeRanking> ();
	}

	public void Init() {

        if (Data.Instance.playMode != Data.PlayModes.SURVIVAL && Data.Instance.playMode != Data.PlayModes.PARTYMODE) {
			GetComponent<GameOverPartyMode> ().Init ();
			return;
		}
		if (arcadeRanking.all.Count == 0)
			return;

		hiscore = (float)arcadeRanking.all [0].hiscore;

		SetTopTenImage ();

		panel.SetActive (true);
		score = Data.Instance.multiplayerData.score;
		ArcadeRanking.Hiscore myTempHiscore = new ArcadeRanking.Hiscore ();
		myTempHiscore.hiscore = score;
		myTempHiscore.username = "TU SCORE";
		mySignal = AddSignal (myTempHiscore, 120);
		float gotoX = mySignal.transform.localPosition.x;

        if (gotoX > 100)
            gotoX = 100;

        Vector3 pos = mySignal.transform.localPosition;
		pos.x = 0;
		mySignal.transform.localPosition = pos;

        mySignal.gameObject.transform.DOLocalMoveX(gotoX, 2);

        StartCoroutine(DrawHiscores());
	}
    IEnumerator DrawHiscores()
    {
        puesto = 0;
        int num = 1;
        bool isCompleted = false;


        foreach (ArcadeRanking.Hiscore data in arcadeRanking.all)
		{
            if (isCompleted) { }	
			else if (num > rankingNum) {
				isCompleted = true;
				SetPuesto ();				
			} else {
				if (data.hiscore < score && puesto == 0)
					puesto = num;
				yield return new WaitForSeconds (0.12f);
				AddSignal (data, num);
				num++;
			}
		}
        yield return new WaitForSeconds(3f);
        print("puesto: " + puesto);
        if (puesto != 0 && puesto < rankingNum)
        {
            GotoNewHiscore();
            Reset();
        }
        else
        {
            yield return new WaitForSeconds(3f);
            GetComponent<GameOverPartyMode>().Init();
            yield return new WaitForSeconds(5f);
            Reset();
        }
    }
	void SetPuesto()
	{
		GetComponent<GameOverPartyMode> ().Init ();
		if (puesto == 0)
			VoicesManager.Instance.PlaySpecificClipFromList (VoicesManager.Instance.UIItems, 5);
		else
			VoicesManager.Instance.PlaySpecificClipFromList (VoicesManager.Instance.UIItems, 4);
		
		mySignal.SetPuesto (puesto);
	}
	HiscoresComparisonSignal AddSignal(ArcadeRanking.Hiscore data, int puesto)
	{
		float normalizedPos = GetNormalizedPosition (data.hiscore);
		HiscoresComparisonSignal newSignal = Instantiate (signal);
		newSignal.transform.SetParent (container);
		newSignal.transform.localScale = Vector3.one;

		if(puesto ==1 || puesto > 10)
			newSignal.Init (data.username, data.hiscore, puesto);
		else
			newSignal.Init ("",0, 0);

		float _x = GetNormalizedPosition(data.hiscore);
		newSignal.transform.localPosition = new Vector3 (_x ,0,0);
		return newSignal;
	}
	float GetNormalizedPosition(int score)
	{
		return ((float)score * 100) / hiscore;
	}
	public void Reset()
	{
		StopAllCoroutines ();
		panel.SetActive (false);
	}
	void SetTopTenImage()
	{
		int lastScore;
		if (arcadeRanking.all.Count >= 10)
			lastScore = arcadeRanking.all [9].hiscore;
		else
			lastScore = arcadeRanking.all [arcadeRanking.all.Count-1].hiscore;
		
		float f = GetNormalizedPosition (lastScore) / 100;
		topTenImage.fillAmount = f;
	}
	void GotoNewHiscore()
	{
		Data.Instance.multiplayerData.OnRefreshPlayersByActiveOnes ();
		Data.Instance.inputSavedAutomaticPlay.RemoveAllData ();
		Data.Instance.isReplay = false;
		CancelInvoke ();
		Data.Instance.events.OnResetLevel();
		Data.Instance.LoadLevel ("Hiscores");
		Reset ();
	}
	void OnDisable()
	{
		StopAllCoroutines ();
	}
}
