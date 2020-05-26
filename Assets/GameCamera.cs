using UnityEngine;
using System.Collections;
using AlpacaSound.RetroPixelPro;
using DG.Tweening;

public class GameCamera : MonoBehaviour 
{
    public SpriteRenderer backgrundImage;
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

    public bool onExplotion;

	public float pixelSize;
	float pixel_speed_recovery = 16;
	private GameObject flow_target;
	float _Y_correction = 1;
    float targetZOffset = 6.5f;

    float camSensorSpeed = 0.04f;
    float sensorSizeValueInitial = 9;
    float sensorSizeValue;

    private void Awake()
    {
        sensorSizeValue = sensorSizeValueInitial;
        cam.enabled = false;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
        Data.Instance.events.OnChangeMood += OnChangeMood;
        Data.Instance.events.OnVersusTeamWon += OnVersusTeamWon;
        //if (Data.Instance.playMode != Data.PlayModes.SURVIVAL)
        //{
        //    Data.Instance.events.OnProjectilStartSnappingTarget += OnProjectilStartSnappingTarget;
        //    Data.Instance.events.OnCameraZoomTo += OnCameraZoomTo;
        //}
        Data.Instance.events.OnGameOver += OnGameOver;
    }
    void OnDestroy()
    {
        StopAllCoroutines();
        Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnChangeMood -= OnChangeMood;
        Data.Instance.events.OnVersusTeamWon -= OnVersusTeamWon;
        Data.Instance.events.OnProjectilStartSnappingTarget -= OnProjectilStartSnappingTarget;
        Data.Instance.events.OnCameraZoomTo -= OnCameraZoomTo;
        Data.Instance.events.OnGameOver -= OnGameOver;
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
        cam.enabled = true;
        fieldOfView = cam.fieldOfView;
        charactersManager = Game.Instance.GetComponent<CharactersManager>();

        if (Data.Instance.useRetroPixelPro)
        {
            Component rpp = Data.Instance.videogamesData.GetActualVideogameData().retroPixelPro;
            retroPixelPro = CopyComponent(rpp, cam.gameObject) as RetroPixelPro;
            retroPixelPro.dither = 0;
            pixelSize = 1;
        }     		

        if (Data.Instance.isAndroid)
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

		SetPixels(8);

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

        if (pixelSize < 1)
			pixelSize = 1;
		else 
			pixelSize -= pixel_speed_recovery * Time.deltaTime;

        retroPixelPro.pixelSize = (int)(pixelSize);

	}
   
	void LateUpdate () 
	{
        if (!started)
            return;

            float sensorSizeX = Mathf.Lerp(cam.sensorSize.x, sensorSizeValue, camSensorSpeed);  
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
            return;
        }
        if (Data.Instance.useRetroPixelPro)
        {
            if (retroPixelPro.pixelSize > 1)
                UpdatePixels();
        }
        else
        {            
            Vector3 rot = transform.localEulerAngles;
            CharacterBehavior cb = charactersManager.getMainCharacter();
            if(cb)
                rot.z = -(cb.rotationY / 6);
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
        print("_____CAM OnGameOver " + isTimeOver + " state: " + state);

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
        sensorSizeValue = sensorSizeValueInitial - (orientation.z * 10);
        Debug.Log("Change sensor value to: " + sensorSizeValue);
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
	void OnCameraZoomTo(Vector3 targetPos)
	{
		//Data.Instance.events.FreezeCharacters (true);
		//Data.Instance.events.RalentaTo (0.5f, 0.1f);
		//this.snapTargetPosition = targetPos;
	//	state = states.SNAPPING_TO;
	}
	void OnProjectilStartSnappingTarget(Vector3 targetPos)
	{
      //  if (Data.Instance.isAndroid)
        //    OnCameraZoomTo(targetPos);

       // Data.Instance.events.RalentaTo (0.5f, 0.1f);
  //      if(!Data.Instance.isAndroid)
  //          StartCoroutine(ResetSnappingCoroutine(3));
    }
	//IEnumerator ResetSnappingCoroutine(float delay)
	//{
	//	yield return new WaitForSecondsRealtime(delay);
	//	Data.Instance.events.RalentaTo (1f, 0.01f);
	//}
}