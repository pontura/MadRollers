using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour {

	float lastClickedTime = 0;
	bool processAxis;
	bool isOff;
	float delayToReact = 0.3f;
    void Start()
    {
        if (Data.Instance.isAndroid)
            Destroy(this);
    }
	public void SetOff()
	{
		isOff = true;
	}
	public void SetOn()
	{
		isOff = false;
	}
	void Update()
	{
		if (isOff)
			return;
		lastClickedTime += Time.deltaTime;
		if (lastClickedTime > delayToReact)
			processAxis = true;
        UpdateStandalone();
    }
    void UpdateStandalone()
    {
        for (int a = 0; a < 4; a++)
        {
            if (Data.Instance.inputManager.GetButtonDown(a, InputAction.action1))
                OnJoystickClick();
            if (processAxis)
            {
                float v = Data.Instance.inputManager.GetAxis(a, InputAction.vertical);
                    if (v < -0.1f)
                    OnJoystickUp();
                else if (v > 0.1f)
                    OnJoystickDown();

                float h = Data.Instance.inputManager.GetAxis(a, InputAction.horizontal);
                if (h < -0.1f)
                    OnJoystickRight();
                else if (h > 0.1f)
                    OnJoystickLeft();
            }
        }
    }
	void OnJoystickUp () {
		Data.Instance.events.OnJoystickUp ();
		ResetMove ();
	}
	void OnJoystickDown () {
		Data.Instance.events.OnJoystickDown ();
		ResetMove ();
	}
	void OnJoystickRight () {
		Data.Instance.events.OnJoystickRight ();
		ResetMove ();
	}
	void OnJoystickLeft () {
		Data.Instance.events.OnJoystickLeft ();
		ResetMove ();
	}
	void OnJoystickClick () {
		Data.Instance.events.OnJoystickClick ();
	}
	void OnJoystickBack () {
		Data.Instance.events.OnJoystickBack ();
	}
	void ResetMove()
	{
		processAxis = false;
		lastClickedTime = 0;
	}
}
