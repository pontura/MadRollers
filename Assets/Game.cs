using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    const string PREFAB_PATH = "Prefabs/Game";
    public GameCamera gameCamera;
    static Game mInstance = null;

	private float pausedSpeed = 0.005f;
	private float pausedMiniumSpeed = 0.05f;
	private bool paused;
	private bool unpaused;

    public MoodManager moodManager;
	public SceneObjectsManager sceneObjectsManager;
    public CombosManager combosManager;
    public Level level;

	public states state;
	public enum states
	{
		INTRO,
		ALLOW_ADDING_CHARACTERS,
		PLAYING
	}

    public static Game Instance
    {
        get
        {
            if (mInstance == null)
            {
                print("Algo llama a Game antes de inicializarse");
            }
            return mInstance;
        }
    }
    void Awake()
    {
        mInstance = this;  
		Data.Instance.GetComponent<Fade> ().FadeOut ();
    }
    void Start()
    {		
		if (Data.Instance.isReplay) {
			Invoke ("Delayed", 0.5f);
			state = states.PLAYING;
		} else {
            gameCamera.Init ();
		}
		GetComponent<CharactersManager>().Init();
		//GetComponent<RainManager> ().Init ();
		level.Init();
        Data.Instance.events.OnGamePaused += OnGamePaused;
        
        Init();
      //  Data.Instance.GetComponent<Tracker>().TrackScreen("Game Screen");
        Data.Instance.events.SetSettingsButtonStatus(false);
		Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
    }
	void Delayed()
	{
		gameCamera.Init ();
		Data.Instance.events.OnGameStart();
		Data.Instance.events.StartMultiplayerRace();
	}
    void OnDestroy()
    {
        Data.Instance.events.OnGamePaused -= OnGamePaused;
		Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
    }
	void StartMultiplayerRace()
	{
		state = states.PLAYING;
	}
	private void Init()
	{
		Data.Instance.events.MissionStart(Data.Instance.missions.MissionActiveID);
        Data.Instance.events.OnGamePaused(false);
	}
    public void Revive()
    {
        Data.Instance.events.OnGamePaused(false);

		if(gameCamera != null)
        	gameCamera.Init();
        
        CharacterBehavior cb = level.charactersManager.character;
        
        Vector3 pos = cb.transform.position;
        pos.y = 40;
        pos.x = 0;
        cb.transform.position = pos;

        cb.Revive();
    }
    public void ResetLevel()
	{		
        Data.Instance.events.OnResetLevel();
        Data.Instance.LoadLevel("Game");
	}
    public void OnGamePaused(bool paused)
    {
        if (paused)
        {
			Data.Instance.events.ForceFrameRate (0);
        }
        else
        {
			Data.Instance.events.ForceFrameRate (1);
        }
    }
	public void GotoVideogameComplete()
	{
		// Pause();
		Data.Instance.events.OnResetLevel();
		// Application.LoadLevel("LevelSelector");
		Data.Instance.events.ForceFrameRate (1);
		Data.Instance.LoadLevel("VideogameComplete");
	}
    public void GotoLevelSelector()
    {
       // Pause();
        Data.Instance.events.OnResetLevel();
       // Application.LoadLevel("LevelSelector");
		Data.Instance.events.ForceFrameRate (1);
        if (Data.Instance.isAndroid)
            Data.Instance.LoadLevel("LevelSelectorMobile");
        else
           Data.Instance.LoadLevel("LevelSelector");
    }
	public void LoadGame()
	{
		Data.Instance.inputSavedAutomaticPlay.RemoveAllData ();
		Data.Instance.events.OnResetLevel();
		Data.Instance.events.ForceFrameRate (1);
		Data.Instance.LoadLevel("Game");
	}
    public void GotoMainMenu()
    {
      //  Pause();
        Data.Instance.events.OnResetLevel();
		Data.Instance.events.ForceFrameRate (1);
        Data.Instance.LoadLevel("MainMenu");
    }
    public void GotoContinue()
    {
       // Pause();
        Data.Instance.events.OnResetLevel();
        Time.timeScale = 1;
        Data.Instance.LoadLevel("Continue");
    }
	public void ChangeVideogame(int videogameID)
	{

		Data.Instance.missions.times_trying_same_mission = 0;
		Data.Instance.missions.MissionActiveID++;
		Data.Instance.videogamesData.actualID = videogameID;
		Data.Instance.isReplay = true;
		ResetLevel ();
	}
	public void Continue()
	{
		Data.Instance.missions.times_trying_same_mission++;
		Data.Instance.multiplayerData.OnRefreshPlayersByActiveOnes ();
		Data.Instance.inputSavedAutomaticPlay.RemoveAllData ();
		Data.Instance.isReplay = true;
		Game.Instance.ResetLevel();  
	}
    public void GotoMainMobile()
    {
        Data.Instance.events.OnResetLevel();
        Data.Instance.events.ForceFrameRate(1);
        Data.Instance.LoadLevel("MainMenuMobile");
    }
}
