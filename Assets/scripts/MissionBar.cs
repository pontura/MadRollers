using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MissionBar : MonoBehaviour {

    [SerializeField] Animation anim;

    [SerializeField] GameObject bossSignal;
	[SerializeField] ProgressBar progressBar;

    [SerializeField] Text field;

    [SerializeField] GameObject bossTimer;
	public bool isOn;

    public float progress = 1;

    int sec;
    int totalHits;

    void Start() 
	{
        bossTimer.SetActive (false);
        bossSignal.gameObject.SetActive (false);
		Events.StartMultiplayerRace += StartMultiplayerRace;

        if (Data.Instance.playMode != Data.PlayModes.SURVIVAL)
        {
            Events.OnBossInit += OnBossInit;        
            Events.OnBossActive += OnBossActive;
		    Events.OnBossHitsUpdate += OnBossHitsUpdate;
		    Events.OnBossSetTimer += OnBossSetTimer;
        }

		Events.OnGameOver += OnGameOver;
        Events.OnContinue += OnContinue;
    }
	void OnDestroy () {
		Events.StartMultiplayerRace -= StartMultiplayerRace;
		Events.OnBossInit -= OnBossInit;
		Events.OnBossActive -= OnBossActive;
		Events.OnBossHitsUpdate -= OnBossHitsUpdate;
		Events.OnBossSetTimer -= OnBossSetTimer;
		Events.OnGameOver -= OnGameOver;
		Events.OnContinue -= OnContinue;
	}
	void StartMultiplayerRace()
	{
        bossSignal.gameObject.SetActive(false);
        anim.Play("bossSignalOff");
    }
	void OnContinue()
	{
		if(isOn)
			Loop();
    }
    void OnGameOver(bool isTimeOut)
	{
        if (isTimeOut)
			return;
        CancelInvoke ();
	}
	void OnBossHitsUpdate(float actualHits)
	{
		progress = 1 - (actualHits / (float)totalHits);
        progressBar.SetProgression (progress);
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
		if (sec <=  0)
        {
            Events.OnGameOver (true);
			Events.FreezeCharacters (true);
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
		isOn = true;
        progressBar.SetProgression (1);
		this.totalHits = totalHits;
        bossSignal.gameObject.SetActive(true);
		anim.Play("BossSignalOn");
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
		if (!isOn) {
            bossSignal.gameObject.SetActive(false);
			anim.Play("bossSignalOff");
            CancelInvoke ();
		}
        progressBar.SetProgression(progress);
	}
}
