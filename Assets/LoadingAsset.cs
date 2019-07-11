using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingAsset : MonoBehaviour {

	public Image logo;
	public Text field;
	public string[] texts;
	public GameObject loadingPanel;
	//public GameObject videoPanel;

	public GameObject panel;
	public Sprite[] spritesToBG;
	public GameObject bg;
	public Image[] backgroundImages;
	float speed;
	bool isOn;

	void Start () {
		ChangeBG ();
	}
	public void SetOn(bool _isOn)
	{
		logo.sprite = Data.Instance.videogamesData.GetActualVideogameData ().loadingSplash;

		this.isOn = _isOn;
		panel.SetActive (_isOn);
		loadingPanel.SetActive (_isOn);
        if (isOn)
        {
            if(Data.Instance.isAndroid)
                StartCoroutine(LoadingRoutineAndroid()); 
            else
                StartCoroutine(LoadingRoutine()); 
        }
	}
	IEnumerator LoadingRoutine()
	{
        Data.Instance.voicesManager.PlaySpecificClipFromList (Data.Instance.voicesManager.UIItems, 1);
		Data.Instance.GetComponent<MusicManager>().OnLoadingMusic();
		field.text = "";		
		AddText("*** MAD ROLLERS ***");
		yield return new WaitForSeconds (0.2f);
		AddText("Buenos Aires USER ALLOWING ACCESS!");
		yield return new WaitForSeconds (0.35f);
        AddText("tumba-gallardo<location_pin>");
		yield return new WaitForSeconds (0.4f);
        AddText("-> GOTO 1985 ");
		yield return new WaitForSeconds (0.5f);
		AddText("Hacking the system ...");
        UnityEngine.SceneManagement.SceneManager.LoadScene ("Game");
		yield return new WaitForSeconds (0.5f);

		int i = texts.Length;
		while (i > 0) {
			yield return new WaitForSeconds ((float)Random.Range (6, 10) / 10f);
			AddText(texts[i-1]);
			i--;
		}
		AddText("COMPLETE!");
		yield return new WaitForSeconds (0.5f);
		SetOn (false);
		if (!Data.Instance.isReplay) {
			Data.Instance.GetComponent<MusicManager>().stopAllSounds();
		}
		yield return null;
	}

    IEnumerator LoadingRoutineAndroid()
    {
        VideogameData videogameData = Data.Instance.videogamesData.GetActualVideogameData();
        HiscoresByMissions.MissionHiscoreUserData missionHiscoreUserData = UserData.Instance.hiscoresByMissions.GetHiscore(videogameData.id, Data.Instance.missions.MissionActiveID);
        string username = UserData.Instance.username;

        Data.Instance.voicesManager.PlaySpecificClipFromList(Data.Instance.voicesManager.UIItems, 1);
        Data.Instance.GetComponent<MusicManager>().OnLoadingMusic();
        field.text = "";
        AddText("*** MAD ROLLERS ***");
        yield return new WaitForSeconds(0.5f);
        AddText("Loading " + videogameData.name + "...");
        yield return new WaitForSeconds(0.2f);
        if (missionHiscoreUserData != null)
        {
            AddText("*****************");
            yield return new WaitForSeconds(0.12f);
            AddText("Hiscore by:");
            yield return new WaitForSeconds(0.1f);
            AddText("HACKER: " + missionHiscoreUserData.username + " [" + Utils.FormatNumbers( missionHiscoreUserData.score) + "]");
            yield return new WaitForSeconds(0.15f);
            AddText("in mission_id: [" + Data.Instance.missions.MissionActiveID + "]");
            AddText("*****************");
            AddText(" ");
            yield return new WaitForSeconds(5f);
        }
        AddText("Buenos Aires <" + username + "> USER ALLOWING ACCESS!");        
        yield return new WaitForSeconds(0.35f);
        AddText(username + " -> GOTO 1985 ");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        AddText("COMPLETE!");
        yield return new WaitForSeconds(0.35f);
        SetOn(false);
    }
    void AddText(string text)
	{
		field.text += text +'\n';
	}

	void Update () {
		
		if (!isOn)
			return;
		
		Vector2 pos = bg.transform.localPosition;
		pos.y += speed * Time.deltaTime;
		if (pos.y > 0) {
			ChangeBG ();
			pos.y = -90;
		}
		bg.transform.localPosition = pos;
		Invoke ("ChangeSpeed", (float)Random.Range (5, 30) / 10);
	}
	void ChangeSpeed()
	{
		speed = (float)Random.Range (100, 400);
	}
	void ChangeBG()
	{
		Sprite s = spritesToBG [Random.Range (0, spritesToBG.Length)];
		foreach (Image image in backgroundImages)
			image.sprite = s;
	}
}
