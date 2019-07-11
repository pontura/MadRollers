using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBarMultiplayer : MonoBehaviour {


	public GameObject panel;
	public GameObject hiscoreFields;
	public Text myScoreFields;
	public Text scoreAdviseNum;
	public Text scoreAdviseDesc;
	//public Image bar;
	//public RawImage hiscoreImage;
	public int hiscore;
    float newVictoryAreaScore;

    bool hiscoreWinned;
    bool isAndroid;

    void Start () {
        if (Data.Instance.isAndroid)
            isAndroid = true;
        if (Data.Instance.playMode != Data.PlayModes.PARTYMODE) {
			Vector2 pos = panel.transform.localPosition;
			pos.y += 20;
			panel.transform.localPosition = pos;
		}
		RefreshScore ();
		Data.Instance.events.OnDrawScore += OnDrawScore;

//		bar.fillAmount = 0;
//
//		ArcadeRanking arcadeRanking = Data.Instance.GetComponent<ArcadeRanking> ();
//		if (arcadeRanking.all.Count > 0) {
//			hiscore = arcadeRanking.all [0].score;
//			foreach (Text textfield in hiscoreFields.GetComponentsInChildren<Text>())
//				textfield.text = hiscore.ToString ();
//
//			hiscoreImage.material.mainTexture = Data.Instance.GetComponent<ArcadeRanking>().all[0].texture;
//		}
		scoreAdviseNum.text = "";
		scoreAdviseDesc.text = "";
	}

	void OnDestroy()
	{
		Data.Instance.events.OnDrawScore -= OnDrawScore;
	}
	float delayToReset = 1;
	float ResetFieldsTimer;
	int totalAdded;

	void OnDrawScore(int score, string desc)
	{	
		RefreshScore ();

        if (isAndroid)
            return;

		ResetFieldsTimer = Time.time + delayToReset;
		totalAdded += score;
		scoreAdviseNum.text = "+" + totalAdded.ToString ();
		SetDesc(desc);
	}
	void RefreshScore(){
		myScoreFields.text = Utils.FormatNumbers(  Data.Instance.multiplayerData.score);
	}
	void Update()
	{
        if (isAndroid || totalAdded == 0)
			return;
		if (Time.time > ResetFieldsTimer) {
			totalAdded = 0;
			scoreAdviseNum.text = "";
			scoreAdviseDesc.text = "";
			lastDesc = "";
		}
	}
	string lastDesc = "";
	void SetDesc(string text)
	{
		if (text == lastDesc)
			return;
		lastDesc = text;
		string lastChars = scoreAdviseDesc.text;
		scoreAdviseDesc.text = "\n" + text + lastChars;
	}
}
