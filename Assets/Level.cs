using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level : MonoBehaviour {

    public bool waitingToStart;
    public Dificult Dificulty;
    public enum Dificult
    {
        ALL,
        EASY,
        MEDIUM,
        HARD
    }
    public SceneObject explotion;
    public SceneObject wallExplotion;

    public SceneObject explotionEffect;
	public SceneObject explotionGift;

	public GameObject limitObject;
    public ScoreSignal scoreSignal;

	public GameObject missionDesc;

	//private AreasManager areasManager;
	
	private float lastDistanceToLoadLevel;

    /// para arcade
	public float nextDistanceVictoryArea;
    private int distanceVictoryArea = 550;

	static Area areaActive;
	public float areasLength = 0;
	private int nextPlatformSpace = 50;
	public SceneObjectsBehavior sceneObjects;
	Game game;

	private Area skyArea;	

	private bool showStartArea;
	private Data data;
    private bool playing;
    private int areasX;
    public CharactersManager charactersManager;
    private PowerupsManager powerupsManager;
	public bool isLastArea;
	Missions missions;
   
    public void SetDificultyByScore(int score)
    {
        if (score < 40) Dificulty = Dificult.EASY;
        else if (score < 85) Dificulty = Dificult.MEDIUM;
        else Dificulty = Dificult.HARD;
    }
    private void Awake()
    {
        Dificulty = Dificult.EASY;
		waitingToStart = true;
		missions = Data.Instance.missions;
    }
    public void Init()
	{
		data = Data.Instance;
		game = Game.Instance;

		data.events.OnResetLevel += reset;
		data.events.OnScoreOn += OnScoreOn;
		data.events.OnAddExplotion += OnAddExplotion;
		data.events.OnAddWallExplotion += OnAddWallExplotion;
		data.events.OnAddObjectExplotion += OnAddObjectExplotion;
		//data.events.OnAddHeartsByBreaking += OnAddHeartsByBreaking;
		data.events.StartMultiplayerRace += StartMultiplayerRace;
		Data.Instance.events.OnGameStart += OnGameStart;

		charactersManager = game.GetComponent<CharactersManager>();
		powerupsManager = GetComponent<PowerupsManager>();
//		floorManager = GetComponent<FloorManager>();
//		floorManager.Init(charactersManager);
		playing = true;
		powerupsManager.Init ();
		SetNewVideogameSettings ();

		missions.Init (this);

//		if (Data.Instance.playMode == Data.PlayModes.VERSUS) {
//			Area a = Data.Instance.versusManager.GetArea();
//			sceneObjects.replaceSceneObject(a, a.z_length/2, 0, false);
//			Area b = Data.Instance.versusManager.GetArea();
//			sceneObjects.replaceSceneObject(b, b.z_length/2, 0, true);
//			return;
//		}
    }
	void OnGameStart()
	{
		if(Data.Instance.isReplay)
		{
			if(Data.Instance.missions.times_trying_same_mission==0)
				Data.Instance.voicesManager.PlaySpecificClipFromList (Data.Instance.voicesManager.welcome, 1);
			else if(Data.Instance.missions.times_trying_same_mission<6) 
				Data.Instance.voicesManager.PlaySpecificClipFromList (Data.Instance.voicesManager.welcome, Data.Instance.missions.times_trying_same_mission+1);
			else
				Data.Instance.voicesManager.PlayRandom (Data.Instance.voicesManager.welcome);
		}else
			Data.Instance.voicesManager.PlaySpecificClipFromList (Data.Instance.voicesManager.welcome, 0);
	}
    
    public void OnDestroy()
    {
		Data.Instance.events.OnGameStart -= OnGameStart;
        data.events.OnResetLevel -= reset;
		data.events.OnScoreOn -= OnScoreOn;
        data.events.OnAddExplotion -= OnAddExplotion;
        data.events.OnAddWallExplotion -= OnAddWallExplotion;
        data.events.OnAddObjectExplotion -= OnAddObjectExplotion;
        data.events.StartMultiplayerRace -= StartMultiplayerRace;
       // data.events.OnAddHeartsByBreaking -= OnAddHeartsByBreaking;
    }
    void StartMultiplayerRace()
    {
		Data.Instance.events.OnChangeBackgroundSide (Data.Instance.missions.MissionActive.fondo);
        waitingToStart = false;
    }
	public void Complete()
	{
		charactersManager.OnLevelComplete ();
		showStartArea = true;

//		if (!missions.StartNext ())
//			return;
		
		data.events.MissionComplete ();
		Data.Instance.voicesManager.PlayRandom (Data.Instance.voicesManager.missionComplete);
//		areasManager = missions.getAreasManager();
//		areasManager.Init(0);
//		data.setMission(missions.MissionActiveID);   
		SetNewVideogameSettings ();
	}
	void SetNewVideogameSettings()
	{
		VideogameData videogameData = Data.Instance.videogamesData.GetActualVideogameData ();
		RenderSettings.fogColor = videogameData.fog;
        Game.Instance.gameCamera.cam.backgroundColor = videogameData.fog;
    }
	private void  reset()
	{
        if (!playing) return;
        playing = false;
		Game.Instance.sceneObjectsManager.PoolSceneObjectsInScene();
	}
    public void OnAddObjectExplotion(Vector3 position, int type)
    {      
		Data.Instance.events.OnSoundFX("FX break", -1);
		SceneObject explpotionEffect = null;
        switch (type)
        {
			case 1:
                explpotionEffect = ObjectPool.instance.GetObjectForType("ExplotionEffectBomb", true); break;
            case 2:
			explpotionEffect = ObjectPool.instance.GetObjectForType("ExplotionEffectEnemy", true); break;
            default:
			explpotionEffect  = ObjectPool.instance.GetObjectForType("ExplotionEffectSimpleObject", true); break;
        }
		if (explpotionEffect == null)
			return;
		
		Game.Instance.sceneObjectsManager.AddSceneObject(explpotionEffect, position);
      //  explpotionEffect.Restart(position);
    }
    public void OnAddExplotion(Vector3 position, Color color)
    {
        OnAddExplotion(position, explotion.name, explotionEffect.name, explotionGift.name, 8, color);
    }
    public void OnAddExplotion(Vector3 position, int force, Color color)
    {
        OnAddExplotion(position, explotion.name, explotionEffect.name, explotionGift.name, force, color);
    }
    public void OnAddWallExplotion(Vector3 position, Color color)
    {
        OnAddExplotion(position, wallExplotion.name, "ExplotionEffectWall", explotionGift.name, 12, color);
    }
    public void OnAddExplotion(Vector3 position, string _name, string _explotionEffect, string _explotionGift, int force, Color color)
	{
        Data.Instance.events.OnSoundFX("FX explot00", -1);
        Vector3 newPos = position;
        newPos.y -= 4;

        SceneObject explotionNew = ObjectPool.instance.GetObjectForType(_name, true);
        if (explotionNew == null)
        {
           // Debug.LogError("No hay explosion");
            return;
        } 
		FXExplotion fxExplotion = explotionNew.GetComponent<FXExplotion> ();

		if(fxExplotion != null)
			fxExplotion.SetSize (force);
       
       // explotionNew.GetComponent<FXExplotion>()._scale = force;

		if (explotionNew) {
			Game.Instance.sceneObjectsManager.AddSceneObjectAndInitIt(explotionNew, newPos);
			explotionNew.GetComponent<FXExplotion> ().SetColor (color);
			//explotionNew.Restart (newPos);
		}

        if (_explotionEffect != "")
        {
            SceneObject explpotionEffect = ObjectPool.instance.GetObjectForType(_explotionEffect, true);
            if (explpotionEffect)
            {
				Game.Instance.sceneObjectsManager.AddSceneObject(explpotionEffect, newPos);
               // explpotionEffect.Restart(newPos);
                ParticlesSceneObject ps = explpotionEffect.GetComponent<ParticlesSceneObject>();
                if (ps != null)
                    ps.SetColor(color);
            }

        }

		if (Random.Range(0,100)>70 && Data.Instance.isArcadeMultiplayer)
			Data.Instance.events.OnAddPowerUp (position);
		else
			AddHeartsByBreaking (position, 14, 470);
	}

    void AddHeartsByBreaking(Vector3 position, int NumOfParticles, int force = 700)
    {
        position.y += 1.7f;
		if (NumOfParticles > 25) NumOfParticles = 25;
        for (int a = 0; a < NumOfParticles; a++)
        {
            SceneObject newSO = ObjectPool.instance.GetObjectForType(explotionGift.name, true);
            if (newSO)
            {
				Game.Instance.sceneObjectsManager.AddSceneObjectAndInitIt(newSO, position);
                newSO.transform.localEulerAngles = new Vector3(0, a * (360 / NumOfParticles), 0);
                Vector3 direction = ((newSO.transform.forward * force) + (Vector3.up * (force*3)));
                newSO.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
				GrabbableItem gi = newSO.GetComponent<GrabbableItem> ();
				gi.SetGroundMaterial ();
            }
        }
    }
	private void Update () {
	
		if (Data.Instance.playMode == Data.PlayModes.VERSUS )
			return;
		float dist = charactersManager.getDistance ();

		missions.OnUpdateDistance (dist);
	}
    public void FallDown(int fallDownHeight)
    {
        GameCamera camera = game.gameCamera;
        camera.fallDown(fallDownHeight);
    }
	public void OnScoreOn(int playerID, Vector3 position, int score, ScoresManager.types type)
    {
		if (
			type == ScoresManager.types.DESTROY_FLOOR || 
			type == ScoresManager.types.DESTROY_WALL || 
			type == ScoresManager.types.KILL || 
			type == ScoresManager.types.BREAKING 
		) {
			if (position == Vector3.zero)
				return;


			SceneObject newSO = Instantiate (scoreSignal, position, Quaternion.identity) as SceneObject;
			Game.Instance.sceneObjectsManager.AddSceneObject(newSO, position);
			newSO.GetComponent<ScoreSignal> ().SetScore (playerID, score);
		}
    }


   
}
