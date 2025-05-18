using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ArcadeGUI : MonoBehaviour {

   // public int score;
    public bool ended;
   // public MultiplayerUIStatus[] multiplayerUI;
  //  public List<int> multiplayerUI_Y;
    private CharactersManager characterManager;
    public GameObject singleSignal;
    public GameObject singleSignalTexts;
    public List<int> avatarsThatShoot;
	public JoysticksCanvas joysticksCanvas;

	void Start () {
        singleSignal.SetActive(false);
        characterManager = Game.Instance.GetComponent<CharactersManager>();
        ended = false;
        
        Data.Instance.events.OnGameOver += OnGameOver;
		SetFields ("");

		if(!Data.Instance.isReplay && Data.Instance.playMode != Data.PlayModes.STORYMODE)
			Invoke ("AllowAddingCharacters", 2);
	}
	void AllowAddingCharacters()
	{
		Game.Instance.state = Game.states.ALLOW_ADDING_CHARACTERS;
	}
    void Update()
    {
        if (Data.Instance.isAndroid)
            return;

        if (Game.Instance.state ==  Game.states.INTRO || ended || Game.Instance.state == Game.states.GAME_OVER) return;
        
        if ((Data.Instance.inputManager.GetButton(0, InputAction.action1) || Data.Instance.inputManager.GetButton(0, InputAction.action2)) && joysticksCanvas.CanRevive(0))
        {
            if (!characterManager.existsPlayer(0))
                characterManager.AddNewCharacter(0, false);
        }
		else if ((Data.Instance.inputManager.GetButton(1, InputAction.action1) || Data.Instance.inputManager.GetButton(1, InputAction.action2)) && joysticksCanvas.CanRevive(1))
        {
            if (!characterManager.existsPlayer(1))
                characterManager.AddNewCharacter(1, false);
        }
		else if ((Data.Instance.inputManager.GetButton(2, InputAction.action1) || Data.Instance.inputManager.GetButton(2, InputAction.action2)) && joysticksCanvas.CanRevive(2))
        {
            if (!characterManager.existsPlayer(2))
                characterManager.AddNewCharacter(2, false);
        }
		else if ((Data.Instance.inputManager.GetButton(3, InputAction.action1) || Data.Instance.inputManager.GetButton(3, InputAction.action2)) && joysticksCanvas.CanRevive(3))
        {
            if (!characterManager.existsPlayer(3))
                characterManager.AddNewCharacter(3, false);
        }
    }
    void OnDestroy()
    {
        Data.Instance.events.OnGameOver -= OnGameOver;

    }
    void SetFields(string _text)
    {
        singleSignal.SetActive(true);
        foreach (Text field in singleSignalTexts.GetComponentsInChildren<Text>())
            field.text = _text;
        singleSignal.GetComponent<Animation>().Play("gameOver");
    }
	void OnGameOver(bool isTimeOver)
    {
		Data.Instance.LoseCredit ();
        Data.Instance.multiplayerData.distance = Game.Instance.GetComponent<CharactersManager>().distance;
        
		if (Data.Instance.credits > 0) {

			if (isTimeOver)
				SetFields ("TIME OVER");
			else if (Data.Instance.multiplayerData.GetTotalCharacters () == 1)
				SetFields ("DEAD!");
			else
				SetFields ("ALL DEAD!");

			Invoke("Reset", 2.2f);
		} else {
			SetFields ("GAME OVER");
			Invoke("Reset", 4);
		}

		GetComponent<CreditsUI> ().RemoveOne ();
        ended = true;
        Data.Instance.scoreForArcade = 0;
    }
    void Reset()
    {
		SetFields("");
    }
    

}
