using UnityEngine;
using System.Collections;

public class Data : MonoBehaviour {

    public IInputManager inputManager;
    public bool isArcadeMultiplayer;

	public bool DEBUG;
	[HideInInspector]
	public string testAreaName;

    public bool playOnlyBosses;
	public bool canContinue;
	public int totalCredits;
	public int credits;
	public bool voicesOn;
	public bool soundsFXOn;
	public bool madRollersSoundsOn;
    public bool musicOn;
    public bool switchPlayerInputs;
	public int timeToRespawn;
	public bool isReplay;
	public bool RESET;
    public int competitionID = 1;    
    public int scoreForArcade;
    public bool isAdmin;

    public int pixelSize = 1;

    [HideInInspector]
    public Events events;
    public ObjectPool sceneObjectsPool;
    [HideInInspector]
    public Missions missions;
    [HideInInspector]
    public MultiplayerData multiplayerData;
	[HideInInspector]
	public VideogamesData videogamesData;
	[HideInInspector]
	public InputSaver inputSaver;
	[HideInInspector]
	public InputSavedAutomaticPlay inputSavedAutomaticPlay;
	[HideInInspector]
	public HandWriting handWriting;

    public Rewired.UI.ControlMapper.ControlMapper controlMapper;

    static Data mInstance = null;

	[HideInInspector]
	public bool isEditor;

	public VersusManager versusManager;

	public LoadingAsset loadingAsset;
    public AssetsBundleLoader assetsBundleLoader ;
    public MusicManager musicManager;
    public FramesController framesController;

    public bool singlePlayer;
    public bool isAndroid;
    public bool useRetroPixelPro;
    public bool useOptimizedSettings;

    public PlayModes playMode;
    public enum PlayModes
    {
        STORYMODE,
        PARTYMODE,
        VERSUS,
        CONTINUEMODE,
        SURVIVAL
	//	GHOSTMODE
    }
    public ControlsType controlsType;
    public enum ControlsType
    {
        KEYBOARD,
        VIRTUAL_JOYSTICK,
        GYROSCOPE
    }
    public bool hasContinueOnce;
    public static string ServerAssetsUrl()
    {
        return "www.madrollers.com/bundles/" + Application.version + "/";
        
    }
    public static Data Instance
    {
        get
        {
            if (mInstance == null)
            {
                Debug.LogError("Algo llama a DATA antes de inicializarse");
            }
            return mInstance;
        }
    }
	void Awake () {

        if (RESET)
			PlayerPrefs.DeleteAll ();
#if UNITY_ANDROID || UNITY_IOS
        isAndroid = true;
        isAdmin = false;
        controlsType = ControlsType.GYROSCOPE;
        useOptimizedSettings = true;
        playMode = PlayModes.STORYMODE;
        isAndroid = true;
        Application.targetFrameRate = 60;
#elif UNITY_WEBGL
        useOptimizedSettings = true;
        playMode = PlayModes.STORYMODE;
        isAndroid = false;
#elif UNITY_EDITOR
        Application.targetFrameRate = 60;
#elif UNITY_STANDALONE
        Application.targetFrameRate = 60;
#endif


        string _controlsType = PlayerPrefs.GetString("controlsType");
        if (_controlsType == "GYROSCOPE")
            controlsType = ControlsType.GYROSCOPE;
        else if (_controlsType == "VIRTUAL_JOYSTICK")
            controlsType = ControlsType.VIRTUAL_JOYSTICK;


        if (isAndroid)
        {
           // pixelSize = (int)((float)Screen.height * (0.003f));
            if (SystemInfo.graphicsShaderLevel >= 30)
                useRetroPixelPro = true;
            else
                useRetroPixelPro = false;
        }
        //  Cursor.visible = false;



        if (!mInstance)
            mInstance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }
		DontDestroyOnLoad(this);

        events = GetComponent<Events>();
        missions = GetComponent<Missions>();
        multiplayerData = GetComponent<MultiplayerData>();
        videogamesData = GetComponent<VideogamesData>();
        inputSaver = GetComponent<InputSaver>();
        inputSavedAutomaticPlay = GetComponent<InputSavedAutomaticPlay>();
        versusManager = GetComponent<VersusManager>();
        handWriting = GetComponent<HandWriting>();
        assetsBundleLoader = GetComponent<AssetsBundleLoader > ();
        framesController = GetComponent<FramesController>();

        if (LevelDataDebug.Instance) {
			playMode = PlayModes.STORYMODE;
			DEBUG = LevelDataDebug.Instance.isDebbug;
			this.isArcadeMultiplayer = LevelDataDebug.Instance.isArcadeMultiplayer;
            this.playOnlyBosses = LevelDataDebug.Instance.playOnlyBosses;   
            this.playMode = LevelDataDebug.Instance.playMode;
			this.testAreaName =  LevelDataDebug.Instance.testArea;
            if (Data.Instance.playMode == PlayModes.STORYMODE)
                multiplayerData.player1 = multiplayerData.player1_played = true;
		}

       // GetComponent<Tracker>().Init();
        GetComponent<CurvedWorldManager>().Init();

	}
	void Start()
	{
#if UNITY_EDITOR
        isEditor = true;
#endif
		loadingAsset.SetOn (false);
        //GetComponent<PhotosManager>().LoadPhotos();
        inputManager = InputManager.instance;

    }
	public void setMission(int num)
	{
		missions.MissionActiveID = num;
		int idByVideogame = missions.GetActualMissionByVideogame ();
	}
    public void LoadLevel(string levelName)
    {
		Data.Instance.events.ForceFrameRate (1);
		float delay = 0.1f;
        events.OnChangeScene(levelName);

        if (playMode == PlayModes.PARTYMODE && levelName == "Game")
        {
            loadingAsset.SetOn(true);
            return;
        }
		if (!isReplay && levelName == "Game") {
			loadingAsset.SetOn (true);
			return;
		}
		GetComponent<Fade> ().LoadLevel (levelName, 1f, Color.black);
	}
	public void LoadingReady()
	{
		loadingAsset.SetOn (false);
	}
	public void LoadLevelNotFading(string levelName)
	{
		Data.Instance.events.ForceFrameRate (1);
		GetComponent<Fade>().LoadSceneNotFading (levelName);
	}
	public void RefreshCredits()
	{
		credits = totalCredits;
	}
	public void LoseCredit()
	{
		if (playMode != PlayModes.PARTYMODE)
			return;
		credits--;
		if (credits < 1)
			credits = 0;
	}
	public void WinCredit()
	{
		credits++;
	}
}
