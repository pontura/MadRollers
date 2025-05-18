using UnityEngine;
using System.Collections;

public class LandingPage : MonoBehaviour {

	void Start () {
		Data.Instance.events.OnJoystickClick += OnJoystickClick;
		Data.Instance.events.OnJoystickUp += OnJoystickUp;
	}
	void OnDestroy () {
		Data.Instance.events.OnJoystickClick -= OnJoystickClick;
		Data.Instance.events.OnJoystickUp -= OnJoystickUp;
	}
	void OnJoystickClick () {
		GetComponent<AudioWriter> ().Done ();
	}
	void OnJoystickUp()
	{
		OnJoystickClick ();
	}
}
