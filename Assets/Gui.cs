using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Gui : MonoBehaviour {
    
	public LevelComplete levelComplete;
    public GameObject[] scaleInAndroidGO;

    private Data data;   

	private int barWidth = 200;
    private bool MainMenuOpened = false;

	public Text genericField;
	public GameObject centerPanel;

    public ScoreLine hiscorePanel;

	void Start()
	{
        if (Data.Instance.isAndroid)
        {
           // GetComponent<CanvasScaler>().referenceResolution = new Vector2(260, 593);
            hiscorePanel.gameObject.SetActive(true);
            Data.Instance.events.OnMissionStart += OnMissionStart;
            foreach (GameObject go in scaleInAndroidGO)
                go.transform.localScale *= 1.75f;
        }
        else
        {
            GetComponent<CanvasScaler>().referenceResolution = new Vector2(381, 593);
            hiscorePanel.gameObject.SetActive(false);
        }
		centerPanel.SetActive (false);
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarCrash;

        if (Data.Instance.playMode != Data.PlayModes.SURVIVAL)
            Data.Instance.events.OnBossActive += OnBossActive;

		Data.Instance.events.OnGenericUIText += OnGenericUIText;
        Data.Instance.events.OnGameOver += OnGameOver;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnMissionStart -= OnMissionStart;
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnAvatarFall -= OnAvatarCrash;
		Data.Instance.events.OnBossActive -= OnBossActive;
		Data.Instance.events.OnGenericUIText -= OnGenericUIText;
        Data.Instance.events.OnGameOver -= OnGameOver;

        levelComplete = null;
    }
    void OnGameOver(bool isOver)
    {
        CancelInvoke();
        Reset();
    }
    void OnBossActive(bool isOn)
	{
		CancelInvoke ();
		Reset ();
		if (isOn) {
			OnGenericUIText( "Kill 'em all");
		} else {
            if (Data.Instance.isAndroid)
            {
                GetComponent<SummaryMobile>().Init();
                return;
            }
            else
            {
                levelComplete.gameObject.SetActive(true);
                levelComplete.Init(Data.Instance.missions.MissionActiveID);
            }
		}
		Invoke ("Reset", 2);
	}
	void OnGenericUIText(string text)
	{
		centerPanel.SetActive (true);
		Data.Instance.handWriting.WriteTo(genericField, text, null);
		CancelInvoke ();
		Invoke ("Reset", 2);
	}
	void Reset()
	{
		levelComplete.gameObject.SetActive(false); 
		centerPanel.SetActive (false);
	}
    void OnAvatarCrash(CharacterBehavior cb)
    {
        levelComplete.gameObject.SetActive(false); 
    }
    public void Settings()
    {
        //Data.Instance.GetComponent<GameMenu>().Init();
    }
    void OnMissionStart(int missionID)
    {
        if (Data.Instance.isAndroid)
        {
            int videoGameID = Data.Instance.videogamesData.actualID;
            HiscoresByMissions.MissionHiscoreUserData hiscoreData = UserData.Instance.hiscoresByMissions.GetHiscore(videoGameID, missionID);
            if (hiscoreData == null)
            {
                print("no hay hiscore de videoGameID: " + videoGameID + " mission " + missionID);
                hiscorePanel.gameObject.SetActive(false);
            }
            else
            {
                hiscorePanel.Init(0, hiscoreData.username, hiscoreData.score);
                hiscorePanel.SetImage(hiscoreData.userID);
            }
        }
    }
}
