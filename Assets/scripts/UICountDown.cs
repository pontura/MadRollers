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
        MusicManager.Instance.OnGamePaused(true);
		panel.SetActive (true);
		Events.OnGameStart ();
		SetNextCountDown ();
	}
	void SetNextCountDown()
	{		
		panel.GetComponent<Animation>().Play("logo");

        VoicesManager.Instance.PlayCountDown(countDown);
        if (countDown == 0)
        {
            Events.StartMultiplayerRace();
            countDownField.text = TextsManager.Instance.GetText("GO!"); ;
            Invoke("Done", 0.5f);
            Events.OnSoundFX("countDownStart");
            return;
        }
        else
        {            
            countDownField.text = countDown.ToString();
            Events.OnSoundFX("countDown");
        }
        countDown--;
		Invoke ("SetNextCountDown", 0.75f);
	}
    void Done()
    {        
        panel.SetActive(false);
        Events.OnGenericUIText( TextsManager.Instance.GetText("DESTROY") + "!" );
        MusicManager.Instance.ChangePitch(1);
        Destroy(this);
    }
}
