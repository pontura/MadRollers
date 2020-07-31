using UnityEngine;
using System.Collections;
using AlpacaSound.RetroPixelPro;
using DG.Tweening;
using Wilberforce.FinalVignette;

public class GameCamera : MonoBehaviour 
{
    public FinalVignetteCommandBuffer vignette;
    public SpriteRenderer[] backgrundImage;
    public float fieldOfView;
	public int team_id;
	RetroPixelPro retroPixelPro;
	public Camera cam;
    public states state;
    public  enum states
    {
		WAITING_TO_TRAVEL,
        START,
        PLAYING,
		EXPLOTING,
        END,
		SNAPPING_TO
    }
	public Vector3 snapTargetPosition;
    private CharactersManager charactersManager;

    public Vector3 startPosition = new Vector3(0, 0,0);

	public Vector3 cameraOrientationVector = new Vector3 (0, 4.5f, -0.8f);
	public Vector3 newCameraOrientationVector;

	public Vector3 defaultRotation =  new Vector3 (48,0,0);
    bool started;
    bool isAndroid;
    public bool onExplotion;

    int initialPixelSize;
	public float pixelSize;
	float pixel_speed_recovery = 20;
	private GameObject flow_target;
	float _Y_correction = 10;
    float targetZOffset = 6.5f;

    float camSensorSpeed = 1;
    float sensorSizeValueInitial = 9;
    float sensorSizeValue;

    private void Awake()
    {
        isAndroid = Data.Instance.isAndroid;
        initialPixelSize = Data.Instance.pixelSize;
        sensorSizeValue = sensorSizeValueInitial;
        cam.enabled = false;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
        Data.Instance.events.OnChangeMood += OnChangeMood;
        Data.Instance.events.OnVersusTeamWon += OnVersusTeamWon;
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnStartGameScene += OnStartGameScene;
        //if (Data.Instance.playMode != Data.PlayModes.SURVIVAL)
        //{
        //    Data.Instance.events.OnProjectilStartSnappingTarget += OnProjectilStartSnappingTarget;
        //    Data.Instance.events.OnCameraZoomTo += OnCameraZoomTo;
        //}
        Data.Instance.events.OnGameOver += OnGameOver;
        pixelSize = 10;

        if (Data.Instance.useRetroPixelPro)
        {
            Component rpp = Data.Instance.videogamesData.GetActualVideogameData().retroPixelPro;
            retroPixelPro = CopyComponent(rpp, cam.gameObject) as RetroPixelPro;
            retroPixelPro.pixelSize = initialPixelSize;
            SetPixels(30);
        }
    }
    void OnDestroy()
    {
        StopAllCoroutines();
        Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnChangeMood -= OnChangeMood;
        Data.Instance.events.OnVersusTeamWon -= OnVersusTeamWon;
        Data.Instance.events.OnGameOver -= OnGameOver;
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
        Data.Instance.events.OnStartGameScene -= OnStartGameScene;
    }
    void OnStartGameScene()
    {
        print("OnStartGameScene");
        SetPixels(50);
    }
    void OnMissionComplete(int levelID)
    {
        state = states.END;
        InitPixelsEnd();
    }
    Component CopyComponent(Component original, GameObject destination)
	{
		System.Type type = original.GetType();
		Component copy = destination.AddComponent(type);
		System.Reflection.FieldInfo[] fields = type.GetFields();
		foreach (System.Reflection.FieldInfo field in fields)
		{
			field.SetValue(copy, field.GetValue(original));
		}
		return copy;
	}
    public void Init()
	{
        vignette = GetComponentInChildren<FinalVignetteCommandBuffer>();
      

        cam.enabled = true;

        if (!isAndroid)
            cam.fieldOfView = 172;

        fieldOfView = cam.fieldOfView;
        charactersManager = Game.Instance.GetComponent<CharactersManager>();

        
        if(vignette != null)
        {
            if (isAndroid)
            {
                vignette.VignetteInnerValueDistance = 0;
                vignette.VignetteOuterValueDistance = 0.99f;
            }
        }

        if (isAndroid)
        {
            SetOrientation(Vector4.zero);
            cam.sensorSize = new Vector2(25, cam.sensorSize.y);
        }

		if (Data.Instance.isReplay) {
            state = states.START;
            transform.localPosition = new Vector3(0, 10, 0);
            cam.transform.localPosition = Vector3.zero;
            newPos.y = 0;
        }
        else
        {
            cam.transform.localPosition = new Vector3(0, 0, -4);
            cam.gameObject.transform.DOLocalMove(Vector3.zero, 3);
        }

        flow_target = new GameObject();
        flow_target.transform.SetParent(transform.parent);
        flow_target.name = "Camera_TARGET";
        flow_target.transform.localPosition = new Vector3(0, 5, targetZOffset);
        started = true;
    }
   
	void OnVersusTeamWon(int _team_id)
	{
		if (team_id == _team_id) {
			state = states.END;
		}
	}
    void StartMultiplayerRace()
    {
        sensorSizeValue = sensorSizeValueInitial;
        state = states.PLAYING;
        cam.gameObject.transform.DOLocalMove(Vector3.zero, 3);
    }
    void OnChangeMood(int id)
    {
		return;
    }
	IEnumerator DoExploteCoroutine;
	public void explote(float explotionForce)
	{
		if (state != states.PLAYING)
			return;	
		if (DoExploteCoroutine != null)
			StopCoroutine (DoExploteCoroutine);
		state = states.EXPLOTING;

		SetPixels(initialPixelSize*10);

		DoExploteCoroutine = DoExplote (explotionForce * 6f);
		StartCoroutine (DoExploteCoroutine);
	}
	public IEnumerator DoExplote (float explotionForce)
    {
		float delay = 0.06f;
        for (int a = 0; a < 6; a++)
        {
			rotateRandom( Random.Range(-explotionForce, explotionForce) );
            yield return new WaitForSeconds(delay);
        }
        rotateRandom(0);
		if(state == states.EXPLOTING)
			state = states.PLAYING;
	}
	private void rotateRandom(float explotionForce)
	{
		Vector3 v = cam.transform.localEulerAngles;
		v.z = explotionForce;
		cam.transform.localEulerAngles = v;
	}
	Vector3 newPos;
	int secondsToJump = 5;
	float sec;
	void LookAtFlow()
	{
		Vector3 newPosTarget = flow_target.transform.localPosition;
		newPosTarget.x = Mathf.Lerp(newPosTarget.x, newPos.x, Time.deltaTime*10f);
		newPosTarget.z = transform.localPosition.z+targetZOffset;
        newPosTarget.y= Mathf.Lerp(newPosTarget.y, newPos.y, Time.deltaTime * 1f);
        //newPosTarget.y = 2;
		flow_target.transform.localPosition = newPosTarget;

		Vector3 pos = flow_target.transform.localPosition - transform.localPosition;
		var newRot = Quaternion.LookRotation(pos);

		cam.transform.localRotation = Quaternion.Lerp(cam.transform.localRotation, newRot, Time.deltaTime*20);
	}    
	void LateUpdate () 
	{
        if (!started)
            return;

            float sensorSizeX = Mathf.Lerp(cam.sensorSize.x, sensorSizeValue, camSensorSpeed * Time.deltaTime);  
            cam.sensorSize = new Vector2(sensorSizeX, cam.sensorSize.y);

        //      if (state == states.SNAPPING_TO) { 
        //	Vector3 dest = snapTargetPosition;
        //	dest.y += 1.5f;
        //          //dest.z -= 3f;
        //          dest.z = transform.localPosition.z;
        //          dest.x /= 2;
        //	transform.localPosition = Vector3.Lerp (transform.localPosition, dest, 0);
        //	cam.transform.LookAt (snapTargetPosition);
        //	return;	
        //}  else 
        if (state == states.END)
        {
            UpdatePixelsTillEnd();
            return;
        }

        if (Data.Instance.useRetroPixelPro)
        {
            if (retroPixelPro.pixelSize > Data.Instance.pixelSize)
                UpdatePixels();
        }
        if (isAndroid)
        {
            Vector3 rot = transform.localEulerAngles;
            CharacterBehavior cb = charactersManager.getMainCharacter();
            if (cb)
                rot.z = -(cb.rotationY / 5);
            transform.localEulerAngles = rot;
        }

        newPos = charactersManager.getCameraPosition ();

		Vector3 _newPos  = newPos;
		_newPos += newCameraOrientationVector;

        if (_newPos.x < -15) _newPos.x = -15;
		else if (_newPos.x > 15) _newPos.x = 15;

		//_newPos.z = Mathf.Lerp (transform.localPosition.z, _newPos.z, Time.deltaTime*10);
		_newPos.x = Mathf.Lerp (transform.localPosition.x, _newPos.x, Time.deltaTime*10);
		_newPos.y = Mathf.Lerp (transform.localPosition.y, _newPos.y, (Time.deltaTime*_Y_correction)/3 );

		transform.localPosition = _newPos;
		if(state != states.EXPLOTING)
			LookAtFlow ();
	}
	void OnGameOver(bool isTimeOver)
	{
       // print("_____CAM OnGameOver " + isTimeOver + " state: " + state);

        if (state == states.END) return;
		state = states.END;
        if (!isTimeOver)
            return;
        cam.gameObject.transform.localEulerAngles = new Vector3 (40, 0, 0);
        cam.gameObject.transform.DOMoveZ(cam.gameObject.transform.position.z + 85, 1);

	}
    void OnAvatarCrash(CharacterBehavior player)
    {
		if (Game.Instance.GetComponent<CharactersManager>().getTotalCharacters() > 0) return;
        if (state == states.END) return;

		DoExploteCoroutine = DoExplote (105);
		StartCoroutine (DoExploteCoroutine);

        state = states.END;

        cam.gameObject.transform.DOMove(new Vector3(player.transform.localPosition.x, transform.localPosition.y - 1.5f, transform.localPosition.z - 1f), 3).SetDelay(0.1f);
        cam.gameObject.transform.DOLookAt(player.transform.localPosition, 1.5f).SetDelay(0.2f);
    }

    public void OnAvatarFall(CharacterBehavior player)
	{
		if (Game.Instance.GetComponent<CharactersManager>().getTotalCharacters() > 0) return;
        if (state == states.END) return;

        state = states.END;
        cam.gameObject.transform.DOMove(new Vector3(transform.localPosition.x, transform.localPosition.y + 3f, transform.localPosition.z - 4f), 1f).SetEase(Ease.OutCubic);
        cam.gameObject.transform.DOLookAt(player.transform.localPosition, 1.5f);

    }
	public void SetOrientation(Vector4 orientation)
	{
        orientation /= 6;
        sensorSizeValue = sensorSizeValueInitial - (orientation.z * 12);
       // Debug.Log("orientation: " + orientation + "   Change sensor value to: " + sensorSizeValue);
        newCameraOrientationVector = cameraOrientationVector + new Vector3(orientation.x, orientation.y, 0); //, orientation.z);
	}
    public void fallDown(int fallDownHeight)
    {
    }
	public void ResetVersusPosition()
	{
		Vector3 pos = transform.localPosition;
		pos.y = 0;
		transform.localPosition = pos; 
	}


    //pixeles
    public void SetPixels(float _pixelSize)
    {
        if (!Data.Instance.useRetroPixelPro)
            return;

        this.pixelSize = _pixelSize;
        retroPixelPro.pixelSize = (int)(pixelSize);
    }
    void UpdatePixels()
    {
        if (!Data.Instance.useRetroPixelPro)
            return;

        if (pixelSize < initialPixelSize)
            pixelSize = initialPixelSize;
        else
            pixelSize -= pixel_speed_recovery * Time.deltaTime;

        retroPixelPro.pixelSize = (int)(pixelSize);

    }
    void InitPixelsEnd()
    {
        pixelsToEndValue = 1;
    }
    float pixelsToEndSpeed = 8;
    float pixelsToEndValue;
    void UpdatePixelsTillEnd()
    {
        if (pixelsToEndValue < 30)
        {
            pixelsToEndValue += pixelsToEndSpeed * Time.deltaTime;
            SetPixels(pixelsToEndValue);
        }
    }
    
}