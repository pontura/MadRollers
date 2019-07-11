using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameObject shadow;
	public GameObject versusSignal;
    public Color color;

	public MadRoller madRoller;

	public bool isPlaying = true;

	private Game game;
	private Gui gui;

    public GameObject particles;

    public int id; //numero de player;
   // public EnergyBar progressBar;

    [HideInInspector]
    public Transport transport;
    public Transport[] transports;
    public GameObject transportContainer;

    public fxStates fxState;

	//public bool canJump = false;

    private CharacterBehavior characterBehavior;
    private IEnumerator progressBarCoroutine;

    public enum fxStates
    {
        NORMAL,
        SUPER
    }

   // public float energy = 90;
    private Material originalMaterial;
    public CharactersManager charactersManager;


	void Start()
	{		
		if (isPlaying)
			charactersManager = Game.Instance.GetComponent<CharactersManager>();
		
		if (!isPlaying) {
			
			SetSettings ();
			return;
		} 
		madRoller.SetFxOff ();
		Data.Instance.events.OnAvatarDie += OnAvatarDie;
	//	Data.Instance.events.OnMissionStart += OnMissionStart;
	//	Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
		Data.Instance.events.OnAvatarGetItem += OnAvatarGetItem;
		Data.Instance.events.OnAvatarProgressBarEmpty += OnAvatarProgressBarEmpty;


		if (Data.Instance.playMode == Data.PlayModes.VERSUS)
			versusSignal.SetActive (true);
		else
			versusSignal.SetActive (false);

	}
    void OnDestroy()
    {
        Data.Instance.events.OnAvatarDie -= OnAvatarDie;
   //     Data.Instance.events.OnMissionStart -= OnMissionStart;
   //     Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
        Data.Instance.events.OnAvatarGetItem -= OnAvatarGetItem;
        Data.Instance.events.OnAvatarProgressBarEmpty -= OnAvatarProgressBarEmpty;
    }
	void SetSettings()
	{
		shadow.SetActive (true);


		if(id>3)
			madRoller.Init (3);
		else
			madRoller.Init (id);

		if(id>3)
			color = Data.Instance.GetComponent<MultiplayerData>().colors[4];
		else
			color = Data.Instance.GetComponent<MultiplayerData>().colors[id];

		//foreach (TrailRenderer tr in GetComponentsInChildren<TrailRenderer>()) {
		//	if (id == 0)
		//		tr.enabled = false;
		//	else
		//		tr.material.color = color; 
	//	}
	}
    public void Init(int _id)
    {
		this.id = _id;
		SetSettings ();

		if(id>3)
			color = Data.Instance.GetComponent<MultiplayerData>().colors[4];
		else
			color = Data.Instance.GetComponent<MultiplayerData>().colors[id];

		//foreach (TrailRenderer tr in GetComponentsInChildren<TrailRenderer>()) {
			//if (id == 0)
			//	tr.enabled = false;
			//else
			//	tr.material.color = color; 
		//}


        characterBehavior = GetComponent<CharacterBehavior>();
		characterBehavior.shooter.ResetWeapons ();

        this.id = id;
       
        particles.SetActive(false);
        OnAvatarProgressBarEmpty();
    }
    void OnAvatarDie(CharacterBehavior cb)
    {
		if (cb != characterBehavior)
			return;
		
		shadow.SetActive (false);
        if (progressBarCoroutine != null)
        {
            try
            {
                StopCoroutine(progressBarCoroutine);
                if (fxState == fxStates.SUPER)
                    OnAvatarProgressBarEmpty();
            } catch
            {
                Debug.Log("ERROR en OnAvatarDie");
            }
        }
        cb.Die();
    }
    public void OnAvatarProgressBarStart(Color color)
    {
        //if (progressBar.isOn) return;
      //  progressBar.Init(color);
      //  progressBar.gameObject.SetActive(true);
    }
	public bool IsDebbugerPlayer()
	{
		if (Data.Instance.isEditor && id == 3)
			return true;
		return false;
	}
    public void OnAvatarProgressBarEmpty()
    {

        //print("OnAvatarProgressBarEmpty " + fxState);
       // progressBar.gameObject.SetActive(false);

		//DEBUG: para hacer inmortal al player 1
		if ( IsDebbugerPlayer() )
			return;
		
        if (fxState == fxStates.SUPER )
        {
            setNormalState();
            return;
        }

       // foreach (Transform child in transportContainer.transform)  Destroy(child.gameObject);

        transport = null;
    }
    public void OnAvatarProgressBarUnFill(float qty )
    {
     //   progressBar.UnFill(qty);
    }
    private void OnAvatarGetItem(int playerID, Powerup.types item)
    {
        if (playerID != id) return;

        if (item == Powerup.types.MISSILE)
        {
			characterBehavior.shooter.weapon.setOn();
        }
        else if (item == Powerup.types.JETPACK)
        {
//            if (fxState == fxStates.SUPER) setNormalState();
//            if (characterBehavior.state != CharacterBehavior.states.JETPACK)
//            {
//                transport = Instantiate(transports[0] as Transport, Vector3.zero, Quaternion.identity) as Transport;
//              //  transport.transform.parent = transportContainer.transform;
//                transport.transform.localPosition = Vector3.zero;
//                transport.transform.localEulerAngles = Vector3.zero;
//                transport.transform.localScale = Vector3.one;
//                Data.Instance.events.AdvisesOn("JETPACK!");
//                Data.Instance.events.VoiceFromResources("jetpack_Activado");
//            }
//            OnAvatarProgressBarStart(Color.green);
        }
        else if (item == Powerup.types.INVENSIBLE)
        {
            print("INVENSIBLE player id: " + id);
            if (gameObject == null) return;
           // if (characterBehavior.state == CharacterBehavior.states.JETPACK) return;
			SetInvensible (8);                   
        }
    }
	public void SetInvensible(float timer)
	{
		if (fxState == fxStates.SUPER) return;
		setSuperState();

	//	Data.Instance.events.AdvisesOn("INVENSIBLE!");
		progressBarCoroutine = StartProgressBarCoroutine(timer);

		StartCoroutine(progressBarCoroutine);    
	}
	IEnumerator StartProgressBarCoroutine(float timer)
    {
		yield return new WaitForSeconds(timer);
        OnAvatarProgressBarEmpty();
    }
//   private void OnListenerDispatcher(string message)
//    {
//        if (message == "ShowMissionName")
//			OnMissionStart(Data.Instance.missions.MissionActiveID);
//   }
//   public void OnMissionStart(int missionID)
//   {
//
////       if (Data.Instance.DEBUG 
////			|| Data.Instance.playMode == Data.PlayModes.COMPETITION)
////       {
////           canJump = true;
////       }
////       else
////       {
////
////           if (missionID > 1)
////               canJump = true;
////       }
//   }
   private void setStartingState()
   {
       fxState = fxStates.SUPER;
        //   gameObject.layer = LayerMask.NameToLayer("SuperFX");
    }
    private void setStartingState2()
   {
       fxState = fxStates.NORMAL;
        // gameObject.layer = LayerMask.NameToLayer("Character");
    }
    private void setNormalState()
    {
        Data.Instance.events.OnAvatarChangeFX(Player.fxStates.NORMAL);
        fxState = fxStates.NORMAL;
        //  gameObject.layer = LayerMask.NameToLayer("Character");
        particles.SetActive(false);        
    }
    private void setSuperState()
    {        
        fxState = fxStates.SUPER;        
       // gameObject.layer = LayerMask.NameToLayer("SuperFX");
        particles.SetActive(true);
    }
}
