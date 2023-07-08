using UnityEngine;
using System.Collections;
using DG.Tweening;

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
		PLAYING,
        GAME_OVER
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
    }
    void Start()
    {        
        DOTween.Clear();
     
        if (Data.Instance.isReplay) {
			Invoke ("Delayed", 0.5f);
			state = states.PLAYING;
		} else {
           // 
		}
        Invoke("Timeout", 0.5f);
		level.Init();
        Data.Instance.events.OnGamePaused += OnGamePaused;
        
        Init();

        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        Data.Instance.events.SetSettingsButtonStatus(false);
		Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
    }
    void Timeout()
    {
        gameCamera.Init();
        Data.Instance.GetComponent<Fade>().FadeOut();
        GetComponent<CharactersManager>().Init();
    }
	void Delayed()
	{
        //gameCamera.Init ();
        Data.Instance.events.OnGameStart();
		Data.Instance.events.StartMultiplayerRace();
	}
    void OnDestroy()
    {
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
        Data.Instance.events.OnGamePaused -= OnGamePaused;
		Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
    }
    public void GameOver()
    {
        Data.Instance.events.OnSaveScore();
        state = states.GAME_OVER;
    }

    void StartMultiplayerRace()
	{
        Data.Instance.events.OnMadRollersSFXStatus(true);
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

		//if(gameCamera != null)
  //      	gameCamera.Init();
        
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
        if (Data.Instance.playMode == Data.PlayModes.STORYMODE)
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
    public void GotoNextGame()
    {
        //  Pause();
        Data.Instance.videogamesData.SetOtherGameActive();

        if(Data.Instance.videogamesData.actualID == 0)
            Data.Instance.missions.MissionActiveID++;

        Data.Instance.events.OnResetLevel();
        Data.Instance.events.ForceFrameRate(1);
        Data.Instance.LoadLevel("LevelSelectorMobile");
    }
    public void GotoMainMenu()
    {
      //  Pause();
        Data.Instance.events.OnResetLevel();
		Data.Instance.events.ForceFrameRate (1);
        if (Data.Instance.playMode == Data.PlayModes.PARTYMODE)
            Data.Instance.LoadLevel("MainMenu");
        else
            Data.Instance.LoadLevel("MainMenuMobile");
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
        if (Data.Instance.playMode == Data.PlayModes.PARTYMODE)
            Data.Instance.LoadLevel("MainMenu");
        else
            Data.Instance.LoadLevel("MainMenuMobile");
    }
    bool levelCompleted;
    private void OnListenerDispatcher(ListenerDispatcher.myEnum message)
    {
        
        if (message == ListenerDispatcher.myEnum.LevelFinish)
        {
            if (levelCompleted)
                return;
            levelCompleted = true;
            Debug.Log("<<<<<<<<<<<< Llego a un final de level:");
            level.Complete();
            Data.Instance.events.OnBossActive(false);
        }
    }
    private void Update()//CHEAT: pontura P para ganar level:
    {
        if (Input.GetKeyDown(KeyCode.P))
            OnListenerDispatcher(ListenerDispatcher.myEnum.LevelFinish);
    }
}
