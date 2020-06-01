using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionBar : MonoBehaviour {

	public GameObject bossSignal;
	public ProgressBar progressBar;

    public GameObject routeProgressSignal;
    public ProgressBar routeProgressBar;

	public Text field;
	
	public GameObject bossTimer;
	public Text videogameField;
	public Text missionField;

    int sec;
    int totalHits;
    public float totalDistance;
    public bool routeProgressOn;

    void Start () {
        
        routeProgressSignal.gameObject.SetActive(false);
        videogameField.text = Data.Instance.videogamesData.GetActualVideogameData ().name;
		missionField.text = Data.Instance.texts.genericTexts.mission + " " + (Data.Instance.missions.MissionActiveID+1);
		bossTimer.SetActive (false);
        bossSignal.gameObject.SetActive (false);
		Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;

        if (Data.Instance.playMode != Data.PlayModes.SURVIVAL)
        {
            Data.Instance.events.OnBossInit += OnBossInit;        
            Data.Instance.events.OnBossActive += OnBossActive;
		    Data.Instance.events.OnBossHitsUpdate += OnBossHitsUpdate;
		    Data.Instance.events.OnBossSetNewAsset += OnBossSetNewAsset;
		    Data.Instance.events.OnBossSetTimer += OnBossSetTimer;
            LoopDistance();
        }

		Data.Instance.events.OnGameOver += OnGameOver;
	}
    void LoopDistance()
    {
        if(routeProgressOn)
        {           
            float distance = Game.Instance.level.charactersManager.getDistance();
            print("distance : " + distance + "  totalDistance: " + totalDistance);
            routeProgressBar.SetProgression(distance/totalDistance);
            if (distance >= totalDistance)
            {
                routeProgressOn = false;
                routeProgressSignal.SetActive(false);
            }

        }
        Invoke("LoopDistance", 0.1f);
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
        totalDistance = Data.Instance.missions.GetTotalRoutDistance();
        routeProgressOn = true;
        routeProgressSignal.gameObject.SetActive(true);
        bossSignal.gameObject.SetActive(false);
    }
	void OnGameOver(bool isTimeOut)
	{
		if (isTimeOut)
			return;
        bossSignal.gameObject.SetActive(false);
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
        bossSignal.gameObject.SetActive(true);
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
            bossSignal.gameObject.SetActive(false);
            CancelInvoke ();
		}
        routeProgressSignal.SetActive(false);
        routeProgressOn = false;
	}
}
