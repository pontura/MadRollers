using UnityEngine;
using System.Collections;
using AlpacaSound.RetroPixelPro;

public class GameCamera : MonoBehaviour 
{
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

	public Vector3 defaultRotation =  new Vector3 (47,0,0);
//	public Vector3 newRotation;
    
    public bool onExplotion;
	float explotionForce = 0.25f;

   // public Animation anim;

	public float pixelSize;
	float pixel_speed_recovery = 8;
	private GameObject flow_target;
	float _Y_correction;

    float camSensorSpeed = 1.25f;
    float maxCamSensor = 18f;

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
    void Start()
	{

        fieldOfView = cam.fieldOfView;

        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
		Data.Instance.events.OnChangeMood += OnChangeMood;
		Data.Instance.events.OnVersusTeamWon += OnVersusTeamWon;
		Data.Instance.events.OncharacterCheer += OncharacterCheer;
        if (Data.Instance.playMode != Data.PlayModes.SURVIVAL)
        {
            Data.Instance.events.OnProjectilStartSnappingTarget += OnProjectilStartSnappingTarget;
            Data.Instance.events.OnCameraZoomTo += OnCameraZoomTo;
        }
		Data.Instance.events.OnGameOver += OnGameOver;

        //Data.Instance.events.OnGameStart += OnGameStart;
        if (!Data.Instance.isAndroid)
        {
            Component rpp = Data.Instance.videogamesData.GetActualVideogameData().retroPixelPro;
            retroPixelPro = CopyComponent(rpp, cam.gameObject) as RetroPixelPro;
            retroPixelPro.dither = 0;
            pixelSize = 1;
        }        	

		charactersManager = Game.Instance.GetComponent<CharactersManager>();  

		_Y_correction = 2;
		if (!Data.Instance.isReplay) {
           // Data.Instance.events.OnBossActive(true);
            //anim.Play ("cameraIntro");
            newPos.y = 4.5f;
            cam.sensorSize = new Vector2(6, cam.sensorSize.y);

            transform.localEulerAngles = new Vector3(3,0,0);
            transform.localPosition = new Vector3(0, 11, 0);
            cam.transform.localEulerAngles = new Vector3(26, 0, 0);
            cam.transform.localPosition = new Vector3(0,0,-15);
        } else {
            cam.sensorSize = new Vector2(18, cam.sensorSize.y);
            state = states.START;
            transform.localPosition = new Vector3(0, 2, -1.5f);
            newPos.y = 0;
        }

        if (Data.Instance.isAndroid)
        {
            maxCamSensor = 8f;
            transform.localPosition = new Vector3(0, 0, -1.5f);
        }

    }
    void OnDestroy()
    {
		StopAllCoroutines ();
        Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
        Data.Instance.events.OnChangeMood -= OnChangeMood;
		Data.Instance.events.OnVersusTeamWon -= OnVersusTeamWon;
		Data.Instance.events.OncharacterCheer -= OncharacterCheer;
		Data.Instance.events.OnProjectilStartSnappingTarget -= OnProjectilStartSnappingTarget;
		Data.Instance.events.OnCameraZoomTo -= OnCameraZoomTo;
		Data.Instance.events.OnGameOver -= OnGameOver;
    }
	void OnVersusTeamWon(int _team_id)
	{
		if (team_id == _team_id) {
			state = states.END;
		}
	}
    void StartMultiplayerRace()
    {
       // anim.Stop();
        Init();
        state = states.PLAYING;

	//	cam.transform.localPosition = new Vector3 (0, cam.transform.localPosition.y, 0);

		iTween.MoveTo(cam.gameObject, iTween.Hash(
			"position", new Vector3 (0, 2, 0),
			"islocal", true,
			"time", 4f,
			"easetype", iTween.EaseType.easeOutCirc
			// "axis", "x"
		));

    }
    void OnChangeMood(int id)
    {
		return;
    }
    public void Init() 
	{
        try
        {
             iTween.Stop();
        } catch
        {

        }        
		if (flow_target == null) {
			flow_target = new GameObject ();
			flow_target.transform.SetParent (transform.parent);
			flow_target.name = "Camera_TARGET";
		}
	}
	IEnumerator DoExploteCoroutine;
	void OncharacterCheer()
	{
		if (state != states.PLAYING)
			return;	
		if (DoExploteCoroutine != null)
			StopCoroutine (DoExploteCoroutine);
		state = states.EXPLOTING;

		SetPixels(1);

		DoExploteCoroutine = DoExplote ();
		StartCoroutine (DoExploteCoroutine);

	}
	public void explote(float explotionForce)
	{
		if (state != states.PLAYING)
			return;	
		if (DoExploteCoroutine != null)
			StopCoroutine (DoExploteCoroutine);
		state = states.EXPLOTING;

		SetPixels(4);

		this.explotionForce = explotionForce*3f;
		DoExploteCoroutine = DoExplote ();
		StartCoroutine (DoExploteCoroutine);
	}
	public IEnumerator DoExplote () {
		float delay = 0.04f;
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
		newPosTarget.x = Mathf.Lerp(newPosTarget.x, newPos.x, Time.deltaTime*3f);
		newPosTarget.z = transform.localPosition.z+6.5f;
        newPosTarget.y= Mathf.Lerp(newPosTarget.y, newPos.y, Time.deltaTime * 4f);
        //newPosTarget.y = 2;
		flow_target.transform.localPosition = newPosTarget;

		Vector3 pos = flow_target.transform.localPosition - transform.localPosition;
		var newRot = Quaternion.LookRotation(pos);

		cam.transform.localRotation = Quaternion.Lerp(cam.transform.localRotation, newRot, Time.deltaTime*20);
	}

	public void SetPixels(float _pixelSize)
	{
        if (Data.Instance.isAndroid)
            return;

        this.pixelSize = _pixelSize;
        retroPixelPro.pixelSize = (int)(pixelSize);
	}
	void UpdatePixels()
	{
        if (Data.Instance.isAndroid)
            return;
        if (pixelSize < 1)
			pixelSize = 1;
		else 
			pixelSize -= pixel_speed_recovery * Time.deltaTime;

        retroPixelPro.pixelSize = (int)(pixelSize);

	}
   
	void LateUpdate () 
	{
        if (state == states.START || state == states.WAITING_TO_TRAVEL)
            return;

        if (cam.sensorSize.x < maxCamSensor)
        {
            float cms = cam.sensorSize.x;
            cms += camSensorSpeed * Time.deltaTime;
            cam.sensorSize = new Vector2(cms, cam.sensorSize.y);
        }
        else
        {
            cam.sensorSize = new Vector2(maxCamSensor, cam.sensorSize.y);
        }

        if (state == states.SNAPPING_TO) { 
			Vector3 dest = snapTargetPosition;
			dest.y += 1.5f;
            //dest.z -= 3f;
            dest.z = transform.localPosition.z;
            dest.x /= 2;
			transform.localPosition = Vector3.Lerp (transform.localPosition, dest, 0.02f);
			cam.transform.LookAt (snapTargetPosition);
			return;	
		}       
		//else if (state == states.END && Data.Instance.playMode == Data.PlayModes.VERSUS) {
		//	if (flow_target != null) {
		//		cam.transform.LookAt (flow_target.transform);
		//		cam.transform.RotateAround (Vector3.zero, cam.transform.up, 50 * Time.deltaTime);
		//	}
		//	return;
		//}
		if (state == states.END || state == states.WAITING_TO_TRAVEL)
        {
            return;
        }
        if (!Data.Instance.isAndroid)
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
		//if (team_id == 0)
			newPos = charactersManager.getCameraPosition ();
	//	else
		//	newPos = charactersManager.getPositionByTeam (team_id);

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
		if (!isTimeOver)
			return;
		if (state == states.END) return;
		state = states.END;

		cam.gameObject.transform.localEulerAngles = new Vector3 (40, 0, 0);

		iTween.MoveTo(cam.gameObject, iTween.Hash(
			"z", cam.gameObject.transform.position.z+140,
			"time", 1,
			"easetype", iTween.EaseType.easeOutCubic
			// "axis", "x"
		));
	}
    public void OnAvatarCrash(CharacterBehavior player)
    {
		if (Game.Instance.GetComponent<CharactersManager>().getTotalCharacters() > 0) return;
        if (state == states.END) return;

		this.explotionForce = 105;
		DoExploteCoroutine = DoExplote ();
		StartCoroutine (DoExploteCoroutine);

        state = states.END;
		iTween.MoveTo(cam.gameObject, iTween.Hash(
            "position", new Vector3(player.transform.localPosition.x, transform.localPosition.y - 1.5f, transform.localPosition.z - 0.8f),
            "time", 2f,
            "easetype", iTween.EaseType.easeOutCubic,
            "looktarget", player.transform
           // "axis", "x"
            ));
    }

    public void OnAvatarFall(CharacterBehavior player)
	{
		
		if (Game.Instance.GetComponent<CharactersManager>().getTotalCharacters() > 0) return;
        if (state == states.END) return;

        state = states.END;
		iTween.MoveTo(cam.gameObject, iTween.Hash(
            "position", new Vector3(transform.localPosition.x, transform.localPosition.y+3f, transform.localPosition.z-3.5f),
            "time", 2f,
            "easetype", iTween.EaseType.easeOutCubic,
            "looktarget", player.transform,
            "axis", "x"
            ));
	}
	public void SetOrientation(Vector4 orientation)
	{
       // if (Data.Instance.isAndroid)
            orientation /= 8;
        newCameraOrientationVector = cameraOrientationVector + new Vector3 (orientation.x, orientation.y, orientation.z);
	//	newRotation = defaultRotation + new Vector3 (orientation.w, 0, 0);
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
		Data.Instance.events.FreezeCharacters (true);
		Data.Instance.events.ForceFrameRate (0.5f);
		Data.Instance.events.RalentaTo (0.1f, 0.2f);
		this.snapTargetPosition = targetPos;
		state = states.SNAPPING_TO;
	}
	void OnProjectilStartSnappingTarget(Vector3 targetPos)
	{
		Data.Instance.events.FreezeCharacters (true);
		//Data.Instance.events.ForceFrameRate (0.5f);
		Data.Instance.events.RalentaTo (0.5f, 0.2f);
		this.snapTargetPosition = targetPos;

		if(snapTargetPosition.y < 1)
			snapTargetPosition.y = 1;
		
		state = states.SNAPPING_TO;

        if (!Data.Instance.isAndroid)
            ResetSnapping(3);
    }
    public void ResetSnapping(float delay)
    {
        StartCoroutine(ResetSnappingCoroutine(delay));
    }
	IEnumerator ResetSnappingCoroutine(float delay)
	{
		yield return new WaitForSecondsRealtime(delay);
		if (state != states.SNAPPING_TO)
			yield return null;
		else {			
			StopAllCoroutines ();
			Data.Instance.events.RalentaTo (1f, 0.005f);
			state = states.PLAYING;
			Data.Instance.events.FreezeCharacters (false);
		}
	}
}