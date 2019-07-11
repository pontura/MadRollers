using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	void Start () {
		panel.SetActive (false);
		arcadeRanking = Data.Instance.GetComponent<ArcadeRanking> ();
	}

	public void Init() {

        Data.Instance.videogamesData.SetOtherGameActive();

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

		iTween.MoveTo(mySignal.gameObject, iTween.Hash(
			"x", gotoX,
			"islocal", true,
			"time", 2
		));
        StartCoroutine(DrawHiscores());
	}
	IEnumerator DrawHiscores()
	{
		puesto = 0;
		int num = 1;
		bool isCompleted = false;
		foreach(ArcadeRanking.Hiscore data in arcadeRanking.all)
		{		
			if (isCompleted) { }	
			else if (num > 10) {
				isCompleted = true;
				SetPuesto ();
				yield return new WaitForSeconds (3f);
				if (puesto != 0) {
					GotoNewHiscore ();
					Reset ();
				} else {
					yield return new WaitForSeconds (3f);
					Reset ();
				}
			} else {
				if (puesto == 0 && data.hiscore < score)
					puesto = num;
				yield return new WaitForSeconds (0.17f);
				AddSignal (data, num);
				num++;
			}
		}
	}
	void SetPuesto()
	{
		GetComponent<GameOverPartyMode> ().Init ();
		if (puesto == 0)
			Data.Instance.voicesManager.PlaySpecificClipFromList (Data.Instance.voicesManager.UIItems, 5);
		else
			Data.Instance.voicesManager.PlaySpecificClipFromList (Data.Instance.voicesManager.UIItems, 4);
		
		mySignal.SetPuesto (puesto);
	}
	HiscoresComparisonSignal AddSignal(ArcadeRanking.Hiscore data, int puesto)
	{
		float normalizedPos = GetNormalizedPosition (data.hiscore);
		HiscoresComparisonSignal newSignal = Instantiate (signal);
		newSignal.transform.SetParent (container);
		newSignal.transform.localScale = Vector3.one;

		if(puesto ==1 || puesto >10)
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
