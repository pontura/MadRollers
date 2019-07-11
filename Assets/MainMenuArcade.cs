using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuArcade : MonoBehaviour {

    public PlayerMainMenuUI[] playerMainMenuUI;
    public MainMenuCharacterActor[] mainMenuCharacterActor;
    private MultiplayerData multiplayerData;

    public AudioClip countdown_clip;

    public Text CountDownField;

	public Text[] missionFields;

	public Text playersField;
    public GameObject winnersText;

    public int sec = 10;
    bool isOn;
    public int totalPlayers = 0;
    private bool done;

    public MeshRenderer winnersPicture;
  //  public Light lightInScene;
    public Material[] backgrounds;
    public MeshRenderer backgruond;

	void Start () {
		Data.Instance.events.OnResetScores ();
		sec = 10;
		string desc = ""; //Data.Instance.missions.GetMissionActive ().description;
		foreach (Text t in missionFields) {
			t.text = desc;
		}
        //Data.Instance.events.OnInterfacesStart();
        multiplayerData = Data.Instance.multiplayerData;
		CountDownField.text = "";
        int id = 0;
        foreach (PlayerMainMenuUI pm in playerMainMenuUI)
        {
            pm.id = id;
            id++;
            pm.Init();
        }
		SetFields (0);
		Invoke ("TimeOver", 30);
		Loop ();
		playersField.text = "0 PLAYERS";
	}
	void TimeOver()
	{
		Data.Instance.LoadLevel("MainMenu");
	}
    void LoopBG()
    {
       // backgruond.material = backgrounds[Random.Range(0, backgrounds.Length-1)];
       // Invoke("LoopBG", Random.Range(10, 40) / 10);
    }
    int actualWinner;
    void LoopWinners()
    {
        float timeToLoop = 2;
        if (actualWinner > 0)
            timeToLoop = 0.7f;
        
       // winnersPicture.material.mainTexture = Data.Instance.GetComponent<ArcadeRanking>().all[actualWinner].texture;
        actualWinner++;
        if (actualWinner >= Data.Instance.GetComponent<ArcadeRanking>().all.Count)
            actualWinner = 0;
        Invoke("LoopWinners", timeToLoop);
        
    }
    void SetFields(int puesto)
    {
       // int hiscore = Data.Instance.GetComponent<ArcadeRanking>().all[puesto].score;
//        string actualCompetition = Data.Instance.GetComponent<MultiplayerCompetitionManager>().actualCompetition;
//        foreach (Text field in winnersText.GetComponentsInChildren<Text>())
//        {
//          //  if (puesto == 0)
//              //  field.text = "PUNTERx/S (" + hiscore + " PUNTOS) - CAMPEONATO " + actualCompetition + " -";
//           // else
//              //  field.text = "PUESTO " + (int)(puesto + 1);
//        }
    }
    void Update()
    {
        if (done) return;
		if (Data.Instance.inputManager.GetButtonDown(0, InputAction.action2))
        {
            Clicked(0);
        }
        if (Data.Instance.inputManager.GetButtonDown(1, InputAction.action2))
        {
            Clicked(1);
        }
        if (Data.Instance.inputManager.GetButtonDown(2, InputAction.action2))
        {
            Clicked(2);
        }
        if (Data.Instance.inputManager.GetButtonDown(3, InputAction.action2))
        {
            Clicked(3);
        }
    }
    void Clicked(int playerID)
    {		
        totalPlayers = 0;
        Data.Instance.events.OnSoundFX("coin", playerID);

        playerMainMenuUI[playerID].Toogle();

        GetTotalPlayers();
		isOn = true;
    }
    void GetTotalPlayers()
    {
        foreach (PlayerMainMenuUI pm in playerMainMenuUI)
        {
            if (pm.id == 0) { multiplayerData.player1 = pm.isActive; }
            if (pm.id == 1) { multiplayerData.player2 = pm.isActive; }
            if (pm.id == 2) { multiplayerData.player3 = pm.isActive; }
            if (pm.id == 3) { multiplayerData.player4 = pm.isActive; }

            if (pm.isActive)
            {
                totalPlayers++;
            }
        }
    }
    void Loop()
    {
		Invoke("Loop", 0.5f);
		if (!isOn) return;
//		if (Data.Instance.playMode == Data.PlayModes.VERSUS)
//		if (
//			(Data.Instance.multiplayerData.player1 || Data.Instance.multiplayerData.player2)
//			&&
//			(Data.Instance.multiplayerData.player3 || Data.Instance.multiplayerData.player4)) {
//			//sigue
//		} else
//			return;
		
		if(sec>=0)
       	 sec--;

		CountDownField.text = "0" + sec;

        playersField.text = totalPlayers + " PLAYERS";

        if (sec <= 0)
        {
            GetTotalPlayers();
			if (
				totalPlayers > 0
				//||  (Data.Instance.playMode== Data.PlayModes.VERSUS && totalPlayers > 1)
			)
            {
                
				if (done)
					return;

				done = true;

			//	if (Data.Instance.playMode == Data.PlayModes.COMPETITION ) {
					Data.Instance.LoadLevel ("Game");
//				} else {
//					Data.Instance.LoadLevel ("GameVersus");
//				}
//                return;
            }
        }
        
    }
}
