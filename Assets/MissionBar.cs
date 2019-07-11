using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionBar : MonoBehaviour {

	public Animator panel;
	public ProgressBar progressBar;
	public int totalHits;
	//public float value;
	public Text field;
	public int sec;
	//public Transform itemContainer;
	public GameObject bossTimer;
	public Text videogameField;
	public Text missionField;

	void Start () {
		videogameField.text = Data.Instance.videogamesData.GetActualVideogameData ().name;
		missionField.text = Data.Instance.texts.genericTexts.mission + " " + (Data.Instance.missions.MissionActiveID+1);
		bossTimer.SetActive (false);
		panel.gameObject.SetActive (false);
		Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;

        if (Data.Instance.playMode != Data.PlayModes.SURVIVAL)
        {
            Data.Instance.events.OnBossInit += OnBossInit;        
            Data.Instance.events.OnBossActive += OnBossActive;
		    Data.Instance.events.OnBossHitsUpdate += OnBossHitsUpdate;
		    Data.Instance.events.OnBossSetNewAsset += OnBossSetNewAsset;
		    Data.Instance.events.OnBossSetTimer += OnBossSetTimer;
        }

		Data.Instance.events.OnGameOver += OnGameOver;
	}
	void OnDestroy () {
		Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
		Data.Instance.events.OnBossInit -= OnBossInit;
		Data.Instance.events.OnBossActive -= OnBossActive;
		Data.Instance.events.OnBossHitsUpdate -= OnBossHitsUpdate;
		Data.Instance.events.OnBossSetNewAsset -= OnBossSetNewAsset;
		Data.Instance.events.OnBossSetTimer -= OnBossSetTimer;
		Data.Instance.events.OnGameOver -= OnGameOver;
	}
	void StartMultiplayerRace()
	{
		panel.gameObject.SetActive (true);
	}
	void OnGameOver(bool isTimeOut)
	{
		if (isTimeOut)
			return;
		panel.gameObject.SetActive (false);
		CancelInvoke ();
	}
	void OnBossSetNewAsset(string assetName)
	{
//		Utils.RemoveAllChildsIn (itemContainer);
//		GameObject icon = Instantiate(Resources.Load("bosses/" + assetName, typeof(GameObject))) as GameObject;
//		icon.transform.SetParent (itemContainer);
//		icon.transform.localScale = Vector3.one;
//		icon.transform.localPosition = Vector3.zero;
	}
	void OnBossHitsUpdate(float actualHits)
	{
		progressBar.SetProgression (1-(actualHits / (float)totalHits));
	}
	void Loop()
	{		
		field.text = sec.ToString ();
		sec--;
		if (sec <= 9) {
			field.text = "0" + sec.ToString ();
			field.color = Color.red;
			StartCoroutine (SetBossTimer ());
		} 
		if (sec <=  0) {
			Data.Instance.events.OnGameOver (true);
			Data.Instance.events.FreezeCharacters (true);
		} else {
			Invoke ("Loop", 1);
		}

	}
	IEnumerator SetBossTimer()
	{
		bossTimer.SetActive (true);
		bossTimer.GetComponent<Text> ().text = sec.ToString ();
		yield return new WaitForSeconds (0.5f);
		bossTimer.SetActive (false);
	}
	void OnBossInit (int totalHits) {
		progressBar.SetProgression (1);
		this.totalHits = totalHits;
		panel.Play ("MissionTopOpen");
	}
	void OnBossSetTimer(int timer)
	{
		if (timer == 0)
			timer = 50;
		
		if (Game.Instance.level.charactersManager.getTotalCharacters () == 1) {
			timer += 10;
		}
		
		if (timer > 60)
			timer = 60;	

		sec = timer;

		field.color = Color.white;

		Loop ();

	}
	void OnBossActive (bool isOn)
	{
		progressBar.SetProgression (0);
		if (!isOn) {
			panel.Play ("MissionTopClose");
			CancelInvoke ();
		}
	}
}
