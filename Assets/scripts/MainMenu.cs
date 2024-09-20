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
        ObjectPool.instance.PoolSceneObjectsInScene(); // Borra si quedó alguno:
        Data.Instance.musicManager.ChangePitch(1);
        Data.Instance.events.OnInterfacesStart();
        Data.Instance.isReplay = false;
        Data.Instance.videogamesData.Reset();
        Data.Instance.missions.Reset();

        Resources.UnloadUnusedAssets();

        if(Data.Instance.playMode != Data.PlayModes.STORYMODE)
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
	//	Data.Instance.events.OnJoystickDown += OnJoystickDown;
	//	Data.Instance.events.OnJoystickUp += OnJoystickUp;
	//	Data.Instance.events.OnJoystickLeft += OnJoystickDown;
	//	Data.Instance.events.OnJoystickRight += OnJoystickUp;
		
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
		float _separation = 4.5f;
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
    //private void Update()
    //{
    //    if(Input.GetMouseButtonUp(0))
    //        Data.Instance.LoadLevel("Settings");
    //}
    void OnDestroy()
	{
		Reset ();
	}
	void Reset()
	{
		Data.Instance.events.OnJoystickClick -= OnJoystickClick;
		//Data.Instance.events.OnJoystickDown -= OnJoystickDown;
		//Data.Instance.events.OnJoystickUp -= OnJoystickUp;
	//	Data.Instance.events.OnJoystickLeft -= OnJoystickDown;
		//Data.Instance.events.OnJoystickRight -= OnJoystickUp;
	}
	void Init () {
		SetButtons ();
	}
	void OnJoystickClick()
	{
        Data.Instance.LoadLevel("Intro");
        return;
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
        if (Data.Instance.playMode == Data.PlayModes.CONTINUEMODE || Data.Instance.playMode == Data.PlayModes.STORYMODE)
            Data.Instance.LoadLevel("LevelSelectorMobile");
        else
            Data.Instance.LoadLevel("LevelSelector");
    }
	void Compite()
	{
		Reset ();
        if (Data.Instance.playMode == Data.PlayModes.CONTINUEMODE || Data.Instance.playMode == Data.PlayModes.STORYMODE)
            Data.Instance.LoadLevel("LevelSelectorMobile");
        else
            Data.Instance.LoadLevel("LevelSelector");
    }
	void Versus()
	{
		Reset ();
		Data.Instance.LoadLevel("LevelSelector");
	}



}
