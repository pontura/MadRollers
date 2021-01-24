using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPartyMode : MonoBehaviour {

	public GameObject panel;
	public GameObject[] panelsToHide;
	bool canClick;
	public Text gameOverField;
	public ScoreLine scoreLineToInstatiate;
	public Transform hsicoresContainer;
	bool isOn;

	void Start()
	{
        if (Data.Instance.playMode == Data.PlayModes.STORYMODE || Data.Instance.playMode == Data.PlayModes.SURVIVAL)
            this.enabled = false;

		panel.SetActive (false);
	}
	public void Init()
	{
		if (isOn)
			return;
		isOn = true;

		StartCoroutine (Loop ());
		foreach (GameObject go in panelsToHide)
			go.SetActive (false);
		
		LoadHiscores ();

        gameOverField.text = TextsManager.Instance.GetText("GAME OVER");
	}
	IEnumerator Loop()
	{
		yield return new WaitForSeconds (3);
		panel.SetActive (true);
		yield return new WaitForSeconds (3);
		canClick = true;
	}
	void Update()
	{
		if (canClick) {
            if (Data.Instance.playMode == Data.PlayModes.STORYMODE || Data.Instance.playMode == Data.PlayModes.SURVIVAL)
            {
                UpdateAndroid();
            }
            else
            {
                for (int a = 0; a < 4; a++)
                {
                    if (Data.Instance.inputManager.GetButtonDown(a, InputAction.action1)
                        || Data.Instance.inputManager.GetButtonDown(a, InputAction.action2))
                        OnJoystickClick();
                }
            }
		}
	}
    void UpdateAndroid()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Ended)
                OnJoystickClick();
        }

    }
    void OnJoystickClick()
	{
		canClick = false;
		Data.Instance.events.OnResetMultiplayerData();
		Data.Instance.inputSavedAutomaticPlay.RemoveAllData ();
        if (Data.Instance.playMode == Data.PlayModes.SURVIVAL)
            Game.Instance.LoadGame();
        else
            Game.Instance.GotoMainMenu();	
	}
	void LoadHiscores()
	{
		int num = 1;
		foreach(ArcadeRanking.Hiscore data in Data.Instance.GetComponent<ArcadeRanking>().all)
		{	
			if (num > 10)
				return;
			AddSignal (data, num);
			num++;
		}
	}
	void AddSignal(ArcadeRanking.Hiscore data, int puesto)
	{
		ScoreLine newSignal = Instantiate (scoreLineToInstatiate);
		newSignal.Init (puesto, data.username, data.hiscore);
		newSignal.transform.SetParent (hsicoresContainer);
		newSignal.transform.localScale = Vector3.one;
	}
	void OnDisable()
	{
		StopAllCoroutines ();
	}
}
