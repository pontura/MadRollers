using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	public types type;
	public enum types
	{
		NORMAL,
		DASHING_FORWARD,
		DASHING_BACK
	}
	float offset_Dash_Z = 6;
	private int heightToFall = -5;
	CharacterBehavior cb;
	public int characterScorePosition;
	public Vector3 offset;
	float DHSpeed =20f;
	float DHMoveTo;

	void Start()
	{
		cb = GetComponent<CharacterBehavior> ();
	//	Data.Instance.events.OnReorderAvatarsByPosition += OnReorderAvatarsByPosition;
		Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
	}

	void OnDestroy()
	{
	//	Data.Instance.events.OnReorderAvatarsByPosition -= OnReorderAvatarsByPosition;
		Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
	}
	public void DH(float value)
	{		
		DHMoveTo = value * DHSpeed;
	}
	void DHDone()
	{
		DHMoveTo = 0;
	}
	public void DashForward()
	{
		if (type == types.NORMAL) {
			type = types.DASHING_FORWARD;
			cb.madRoller.Play("dashForward");
		}
	}
	void Update()
	{
		if (type == types.NORMAL)
			return; 
		if (type == types.DASHING_FORWARD) {
			if (offset.z > offset_Dash_Z)
				type = types.DASHING_BACK;
			offset.z += Time.deltaTime * 100;
		} else if (type == types.DASHING_BACK)
		offset.z -= Time.deltaTime * 10;
		if (offset.z < 0) {
			offset.z = 0;
			type = types.NORMAL;
		}
	}
	public void UpdateByController(float rotationY)
	{
		Vector3 goTo = transform.position;

        //		if (cb.isOver)
        //		{
        //			goTo.x = cb.isOver.transform.localPosition.x;
        //			goTo.y = cb.isOver.transform.localPosition.y + 1;
        //			goTo.z = cb.isOver.transform.localPosition.z+0.2f;
        //		}
        //		else
        //		{

        float _z = cb.player.charactersManager.distance;
        if(!Data.Instance.isAndroid)
             _z -= (characterScorePosition/1.25f);

		if (cb.controls.isAutomata)
				_z -= 2;
	//		if (team_for_versus == 2) {
	//			rotationY *= -1;
	//			_z *= -1;
	//		}
			float speedRotation;
			if (Data.Instance.playMode == Data.PlayModes.VERSUS) {
				speedRotation = 2.2f;
			} else {
				speedRotation = 3;
			}

			if (DHMoveTo == 0)
				goTo.x += (rotationY / speedRotation) * Time.deltaTime;
			else
				goTo.x += DHMoveTo * Time.deltaTime;
		
			goTo.z = _z;
		//}

		goTo += offset;
		
		if (transform.position.y < -0.15f && cb.player.fxState == Player.fxStates.SUPER) {
			Vector3 pos = transform.position;
			pos.y = -0.16f;
			transform.position = pos;
			GetComponent<Rigidbody> ().velocity = Vector3.zero;
		} else if (transform.position.y < heightToFall) {
			 cb.Fall ();
		}

		if(cb.controls.isAutomata || cb.controls.ControlsEnabled)
			transform.position = Vector3.Lerp(transform.position, goTo, 6);
	}
	void StartMultiplayerRace()
	{
		//StartCoroutine (RecalculatePosition ());
		this.characterScorePosition = cb.player.id;
	}
	public void SetCharacterScorePosition()
	{
        characterScorePosition = Data.Instance.multiplayerData.GetPositionByScore (cb.player.id);
	}

}
