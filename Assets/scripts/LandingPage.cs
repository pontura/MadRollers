using UnityEngine;
using System.Collections;

public class LandingPage : MonoBehaviour {

	void Start () {
		Events.OnJoystickClick += OnJoystickClick;
		Events.OnJoystickUp += OnJoystickUp;
	}
	void OnDestroy () {
		Events.OnJoystickClick -= OnJoystickClick;
		Events.OnJoystickUp -= OnJoystickUp;
	}
	void OnJoystickClick () {
		GetComponent<AudioWriter> ().Done ();
	}
	void OnJoystickUp()
	{
		OnJoystickClick ();
	}
}
