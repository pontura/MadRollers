using UnityEngine;
using System.Collections;
using DG.Tweening;
using VacuumShaders.CurvedWorld;

public class Game : MonoBehaviour {

    const string PREFAB_PATH = "Prefabs/Game";
    public GameCamera gameCamera;
    static Game mInstance = null;
    [SerializeField] CurvedWorld_Controller curvedWorld_Controller;

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
        Data.Instance.curvedWorldManager.SetController(curvedWorld_Controller);
        sceneObjectsManager.ChangeVideogame(Data.Instance.videogamesData.actualID);
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
        Events.OnGamePaused += OnGamePaused;

        Events.MissionStart(Data.Instance.missions.MissionActiveID);
        Events.OnGamePaused(false);

        Events.OnListenerDispatcher += OnListenerDispatcher;
        Events.SetSettingsButtonStatus(false);
		Events.StartMultiplayerRace += StartMultiplayerRace;
    }
    void Timeout()
    {
        print("Timeout");
        gameCamera.Init();
        Data.Instance.GetComponent<Fade>().FadeOut();
        GetComponent<CharactersManager>().Init();
    }
	void Delayed()
	{
        //gameCamera.Init ();
        Events.OnGameStart();
		Events.StartMultiplayerRace();
	}
    void OnDestroy()
    {
        Events.OnListenerDispatcher -= OnListenerDispatcher;
        Events.OnGamePaused -= OnGamePaused;
		Events.StartMultiplayerRace -= StartMultiplayerRace;
    }
    public void GameOver()
    {
        Events.OnSaveScore();
        state = states.GAME_OVER;
    }

    void StartMultiplayerRace()
	{
        Events.OnMadRollersSFXStatus(true);
        state = states.PLAYING;
	}
    public void Revive()
    {
        Events.OnGamePaused(false);

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
        Events.OnResetLevel();
        Data.Instance.LoadLevel("Game");
	}
    public void OnGamePaused(bool paused)
    {
        if (paused)
        {
			Events.ForceFrameRate (0);
        }
        else
        {
			Events.ForceFrameRate (1);
        }
    }
	public void GotoVideogameComplete()
	{
		Events.OnResetLevel();
		Events.ForceFrameRate (1);
		Data.Instance.LoadLevel("VideogameComplete");
	}
    public void GotoLevelSelector()
    {
        Events.OnResetScores();
        Events.OnResetLevel();
		Events.ForceFrameRate (1);
        Data.Instance.LoadLevel("LevelSelectorMobile");
    }
	public void LoadGame()
	{
		Events.OnResetLevel();
		Events.ForceFrameRate (1);
		Data.Instance.LoadLevel("Game");
	}
    public void GotoNextGame()
    {
        if(Data.Instance.videogamesData.actualID == 0)
            Data.Instance.missions.MissionActiveID++;

        Events.OnResetLevel();
        Events.ForceFrameRate(1);
        Data.Instance.LoadLevel("LevelSelectorMobile");
    }
    public void GotoMainMenu()
    {
        Events.OnResetLevel();
		Events.ForceFrameRate (1);
        if (Data.Instance.playMode == Data.PlayModes.PARTYMODE)
            Data.Instance.LoadLevel("MainMenu");
        else
            Data.Instance.LoadLevel("MainMenuMobile");
    }
    public void GotoContinue()
    {
        Events.OnResetLevel();
        Time.timeScale = 1;
        Data.Instance.LoadLevel("Continue");
    }
    public void Continue()
    {
        //Events.RalentaTo(1, 1f);
        state = states.PLAYING;
        level.charactersManager.Continue();
        gameCamera.Continue();
        Events.OnContinue();
    }
    public void PlayAgain()
	{
		Data.Instance.missions.times_trying_same_mission++;
		Data.Instance.multiplayerData.OnRefreshPlayersByActiveOnes ();
		Data.Instance.isReplay = true;
		Game.Instance.ResetLevel();  
	}
    public void GotoMainMobile()
    {
        Events.OnResetLevel();
        Events.ForceFrameRate(1);
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
            Events.OnBossActive(false);
        }
    }
#if UNITY_EDITOR
    private void Update()//CHEAT: pontura P para ganar level:
    {
        if (Input.GetKeyDown(KeyCode.P))
            OnListenerDispatcher(ListenerDispatcher.myEnum.LevelFinish);
    }
#endif
}
