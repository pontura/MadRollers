using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public int activeID = 0;
	public GameObject standaloneButtons;
	public GameObject partyGameButtons;

	List<MainMenuButton> buttons; 
	public List<MainMenuButton> buttonsStandalone; 
	public List<MainMenuButton> buttonsArcade; 
	MainMenuButton activeButton;
	public Text playersField;

	public Transform container;
	public Player player_to_instantiate;

	void Start()
	{		
		Cursor.visible = false;
		buttons = new List<MainMenuButton> ();
		if (Data.Instance.isArcadeMultiplayer) {
			partyGameButtons.SetActive (false);
			standaloneButtons.SetActive (false);
			buttons = buttonsArcade;
		} else  {
			partyGameButtons.SetActive (false);
			standaloneButtons.SetActive (true);
			buttons = buttonsStandalone;
		}
		Init ();
		Data.Instance.events.OnJoystickClick += OnJoystickClick;
		Data.Instance.events.OnJoystickDown += OnJoystickDown;
		Data.Instance.events.OnJoystickUp += OnJoystickUp;
		Data.Instance.events.OnJoystickLeft += OnJoystickDown;
		Data.Instance.events.OnJoystickRight += OnJoystickUp;
//
//		if (Data.Instance.totalJoysticks == 1)
//			playersField.text = "PLAYER (1)";
//		else 
//			playersField.text = "PLAYERS (" + Data.Instance.totalJoysticks.ToString () + ")";
		
		foreach (MainMenuButton m in buttons)
			m.SetOn (false);

		if(Data.Instance.isArcadeMultiplayer)
		{
			activeID = 0;		
		} else{
			activeID = 1;
		}
		SetButtons ();
		activeButton.SetOn (true);
		float _separation = 6;
		for (int a = 0; a < 4; a++) {
			Player p = Instantiate (player_to_instantiate);
			p.isPlaying = false;
			p.transform.SetParent (container);
			p.id = a;
			p.transform.localPosition = new Vector3((-(_separation*3)/2)+(_separation*a),0,0);
			p.transform.localScale = Vector3.one;
			p.transform.localEulerAngles = Vector3.zero;
		}
	}
	void OnDestroy()
	{
		Reset ();
	}
	void Reset()
	{
		Data.Instance.events.OnJoystickClick -= OnJoystickClick;
		Data.Instance.events.OnJoystickDown -= OnJoystickDown;
		Data.Instance.events.OnJoystickUp -= OnJoystickUp;
		Data.Instance.events.OnJoystickLeft -= OnJoystickDown;
		Data.Instance.events.OnJoystickRight -= OnJoystickUp;
	}
	void Init () {
		Data.Instance.events.OnInterfacesStart();
		SetButtons ();
	}
	void OnJoystickClick()
	{
//		if (Data.Instance.DEBUG) {
//			Data.Instance.playMode = Data.PlayModes.COMPETITION;
//			Data.Instance.multiplayerData.player1 = true;
//			Data.Instance.LoadLevel("Game");
//			return;
//		}
		if (Data.Instance.isArcadeMultiplayer) {
			if (activeID == 0)
				Compite ();
			else if (activeID == 1) 
				Versus ();
		} else {
			if (activeID == 0)
				MissionsScene ();
			else if (activeID == 1)			
				Compite ();
		 	else	
				Versus ();
		}

	}
	void OnJoystickUp()
	{
		if (activeID == 0)
			activeID = buttons.Count-1;
		else
			activeID--;
		SetButtons ();
	}
	void OnJoystickDown()
	{
		if (activeID == buttons.Count-1)
			activeID = 0;
		else
			activeID++;
		SetButtons ();
	}
	void SetButtons ()
	{
		if(activeButton != null)
			activeButton.SetOn (false);
		activeButton = buttons [activeID];
		activeButton.SetOn (true);
	}
	void MissionsScene()
	{
		Reset ();
        //Data.Instance.playMode = Data.PlayModes.STORY;
        if (Data.Instance.isAndroid)
            Data.Instance.LoadLevel("LevelSelectorMobile");
        else
            Data.Instance.LoadLevel("LevelSelector");
    }
	void Compite()
	{
		Reset ();
        //Data.Instance.playMode = Data.PlayModes.COMPETITION;
        if (Data.Instance.isAndroid)
            Data.Instance.LoadLevel("LevelSelectorMobile");
        else
            Data.Instance.LoadLevel("LevelSelector");
    }
	void Versus()
	{
		Reset ();
		//Data.Instance.playMode = Data.PlayModes.VERSUS;
		Data.Instance.LoadLevel("LevelSelector");
	}



}
