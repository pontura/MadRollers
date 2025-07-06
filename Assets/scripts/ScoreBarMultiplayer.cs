using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBarMultiplayer : MonoBehaviour {

    public GameObject panel;

    Animation scoreSignalAnimation;
    public GameObject scoreSignal;
    public Text scoreSignalField;

	public Text myScoreFields;
	//public Image bar;
	//public RawImage hiscoreImage;
	public int hiscore;
    float newVictoryAreaScore;

    bool hiscoreWinned;
    bool isAndroid;
    int totalAdded;

    void Start () {

        if (Data.Instance.isAndroid)
            isAndroid = true;

        panel.SetActive(true);

        scoreSignalAnimation = scoreSignal.GetComponent<Animation>();
        scoreSignal.SetActive(false);

        RefreshScore ();
		Data.Instance.events.OnDrawScore += OnDrawScore;
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnPayPixeles += OnPayPixeles;
        Data.Instance.events.OnContinue += OnContinue;

        Data.Instance.multiplayerData.score = 0;
        RefreshScore();
    }
    void OnContinue()
    {
        RefreshScore();
    }
    void OnMissionComplete(int id)
    {
        panel.SetActive(false);
    }
    void OnPayPixeles(int price)
    {
        Data.Instance.multiplayerData.score -= price;
        if(Data.Instance.multiplayerData.score < 0) Data.Instance.multiplayerData.score = 0;
        myScoreFields.text = Utils.FormatNumbers(Data.Instance.multiplayerData.score);
    }
    void OnDestroy()
	{
		Data.Instance.events.OnDrawScore -= OnDrawScore;
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
        Data.Instance.events.OnPayPixeles -= OnPayPixeles;
        Data.Instance.events.OnContinue -= OnContinue;
    }
	float delayToReset = 1;
	float ResetFieldsTimer;

	void OnDrawScore(int score, string desc)
	{	
		RefreshScore ();

        if (Data.Instance.playMode == Data.PlayModes.STORYMODE || Data.Instance.playMode == Data.PlayModes.SURVIVAL)
        {
            if (score < 0)
                return;
            scoreSignalAnimation[scoreSignalAnimation.clip.name].normalizedTime = 0;
            scoreSignalAnimation.Play();
            scoreSignal.SetActive(true);
            scoreSignalField.text = "+" + score.ToString();
            return;
        }
            

		ResetFieldsTimer = Time.time + delayToReset;
		totalAdded += score;
		SetDesc(desc);
        

    }
	void RefreshScore(){
		myScoreFields.text = Utils.FormatNumbers(  Data.Instance.multiplayerData.score);
	}
	//void Update()
	//{
 //       if (Data.Instance.playMode == Data.PlayModes.STORYMODE || Data.Instance.playMode == Data.PlayModes.SURVIVAL || totalAdded == 0)
	//		return;
	//	if (Time.time > ResetFieldsTimer) {
	//		totalAdded = 0;
	//		lastDesc = "";
	//	}
	//}
	string lastDesc = "";
	void SetDesc(string text)
	{
		if (text == lastDesc)
			return;
		lastDesc = text;
	}
}
