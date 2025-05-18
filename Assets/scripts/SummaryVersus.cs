using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class SummaryVersus : MonoBehaviour {

	public GameObject panel;
	private int countDown;

	public List<MainMenuButton> buttons;
	public int optionSelected = 0;
	public bool isOn;

	void Start()
	{
		GetComponent<JoystickController> ().SetOff ();
		panel.SetActive(false);
		//Data.Instance.events.OnJoystickBack += OnJoystickBack;
		Data.Instance.events.OnJoystickClick += OnJoystickClick;
		Data.Instance.events.OnJoystickDown += OnJoystickDown;
		Data.Instance.events.OnJoystickUp += OnJoystickUp;
		Data.Instance.events.OnJoystickLeft += OnJoystickDown;
		Data.Instance.events.OnJoystickRight += OnJoystickUp;
	}
	void OnDestroy()
	{
		//Data.Instance.events.OnJoystickBack -= OnJoystickBack;
		Data.Instance.events.OnJoystickClick -= OnJoystickClick;
		Data.Instance.events.OnJoystickDown -= OnJoystickDown;
		Data.Instance.events.OnJoystickUp -= OnJoystickUp;
		Data.Instance.events.OnJoystickLeft -= OnJoystickDown;
		Data.Instance.events.OnJoystickRight -= OnJoystickUp;
	}
	public void SetOn()
	{
		Invoke ("Delayed", 2);
	}
	public void Delayed()
	{
		GetComponent<JoystickController> ().SetOn ();
		Data.Instance.events.RalentaTo (1, 0.05f);
		isOn = true;
		panel.SetActive(true);
		SetSelected ();
	}

	float lastClickedTime = 0;
	bool processAxis;

	void OnJoystickUp () {
		if (!isOn)
			return;
		if (optionSelected >= buttons.Count - 1)
			return;
		optionSelected++;
		SetSelected ();
	}
	void OnJoystickDown () {
		if (!isOn)
			return;
		if (optionSelected <= 0)
			return;
		optionSelected--;
		SetSelected ();
	}
	void SetSelected()
	{
		foreach(MainMenuButton b in buttons)
			b.SetOn (false);
		buttons [optionSelected].SetOn (true);
	}

	void OnJoystickClick () {
		if (!isOn)
			return;
		Data.Instance.events.ForceFrameRate (1);
		UIVersus uiversus = GetComponent<UIVersus> ();
		if (optionSelected == 0) {
			StartCoroutine(uiversus.Reset (1));
		} else if (optionSelected == 1) {			
			StartCoroutine(uiversus.Reset (2));
		} else if (optionSelected == 2) {
			StartCoroutine(uiversus.Reset (3));
		}
		isOn = false;
	}
	void ResetMove()
	{
		processAxis = false;
		lastClickedTime = 0;
	}
}
