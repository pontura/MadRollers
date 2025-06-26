using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountDown : MonoBehaviour {

	public GameObject panel;

	public Text countDownField;
	int countDown = 3;

	void Start () {	
		panel.SetActive (false);

        if (Data.Instance.isReplay)
            Done();
        Invoke("OnStartGameSceneDelayed", 2.5f);
    }
    void OnStartGameSceneDelayed()
    {
        Data.Instance.musicManager.OnGamePaused(true);
		panel.SetActive (true);
		Data.Instance.events.OnGameStart ();
		SetNextCountDown ();
	}
	void SetNextCountDown()
	{		
		panel.GetComponent<Animation>().Play("logo");

        VoicesManager.Instance.PlayCountDown(countDown);
        if (countDown == 0)
        {
            Data.Instance.events.StartMultiplayerRace();
            countDownField.text = TextsManager.Instance.GetText("GO!"); ;
            Invoke("Done", 0.5f);
            Data.Instance.events.OnSoundFX("FX upgrade003", -1);
            return;
        }
        else
        {            
            countDownField.text = countDown.ToString();
            Data.Instance.events.OnSoundFX("FX upgrade002", -1);
        }
        countDown--;
		Invoke ("SetNextCountDown", 0.75f);
	}
    void Done()
    {        
        panel.SetActive(false);
        Data.Instance.events.OnGenericUIText( TextsManager.Instance.GetText("DESTROY") + "!" );
        Data.Instance.musicManager.ChangePitch(1);
        Destroy(this);
    }
}
