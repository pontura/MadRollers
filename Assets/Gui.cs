using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Gui : MonoBehaviour {
    
	public LevelComplete levelComplete;

    private Data data;   

	private int barWidth = 200;
    private bool MainMenuOpened = false;

	public Text genericField;
	public GameObject centerPanel;

	void Start()
	{
		centerPanel.SetActive (false);
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarCrash;

        if (Data.Instance.playMode != Data.PlayModes.SURVIVAL)
            Data.Instance.events.OnBossActive += OnBossActive;

		Data.Instance.events.OnGenericUIText += OnGenericUIText;
        Data.Instance.events.OnGameOver += OnGameOver;
        Data.Instance.events.ResetHandwritingText += ResetHandwritingText;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnAvatarFall -= OnAvatarCrash;
		Data.Instance.events.OnBossActive -= OnBossActive;
		Data.Instance.events.OnGenericUIText -= OnGenericUIText;
        Data.Instance.events.OnGameOver -= OnGameOver;
        Data.Instance.events.ResetHandwritingText -= ResetHandwritingText;

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
    void ResetHandwritingText()
    {
        CancelInvoke();
        genericField.text = "";
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
   
}
