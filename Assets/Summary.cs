using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Summary : MonoBehaviour {

    public GameObject panel;
    private int countDown;
    public Animation anim;

    public GameObject standalonePanel;
    public GameObject mobilePanel;

    public List<MainMenuButton> buttons;
	public int optionSelected = 0;
    private bool isOn;

	float delayToReact = 0.3f;

    void Start()
    {
        if(Data.Instance.isAndroid)
        {
            standalonePanel.SetActive(false);
            mobilePanel.SetActive(true);
        }
        else
        {
            standalonePanel.SetActive(true);
            mobilePanel.SetActive(false);
        }
        panel.SetActive(false);
		if (Data.Instance.playMode == Data.PlayModes.STORYMODE) {		
			Data.Instance.events.OnGameOver += OnGameOver;
			Data.Instance.events.OnFireUI += OnFireUI;
		}
    }
	void OnFireUI()
	{
		if (!isOn)
			return;
		isOn = false;
		Restart ();
	}
    void OnDestroy()
    {
		Data.Instance.events.OnGameOver -= OnGameOver;
		Data.Instance.events.OnFireUI -= OnFireUI;
    }
	void OnGameOver(bool isTimeOver)
    {
        if (isOn) return;
		isOn = true;
        Invoke("SetOn", 2F);
    }
    void SetOn()
    {
		Data.Instance.events.RalentaTo (1, 0.05f);

        panel.SetActive(true);
        
		SetSelected ();
        StartCoroutine(Play(anim, "popupOpen", false, null));
	}
	public void Restart()
	{
		Data.Instance.isReplay = true;
		Game.Instance.ResetLevel();        
	}
	void Update()
	{
		if (!isOn)
			return;

		lastClickedTime += Time.deltaTime;

		if (lastClickedTime > delayToReact)
			processAxis = true;
		
		for (int a = 0; a < 4; a++) {
			if ( Data.Instance.inputManager.GetButtonDown(a, InputAction.action1) )
				OnJoystickClick ();
            if (Data.Instance.inputManager.GetButtonDown(a, InputAction.action2))
                OnJoystickClick ();
			if (processAxis) {
                float v = Data.Instance.inputManager.GetAxis(a, InputAction.vertical);
                if (v < -0.5f)
					OnJoystickDown ();
				else if (v > 0.5f)
					OnJoystickUp ();
			}
		}
	}


	float lastClickedTime = 0;
	bool processAxis;

	void OnJoystickUp () {
		if (optionSelected >= buttons.Count - 1)
			return;
		optionSelected++;
		SetSelected ();
		ResetMove ();
	}
	void OnJoystickDown () {
		if (optionSelected <= 0)
			return;
		optionSelected--;
		SetSelected ();
		ResetMove ();
	}
	void SetSelected()
	{
		foreach(MainMenuButton b in buttons)
			b.SetOn (false);
		buttons [optionSelected].SetOn (true);
	}

	void OnJoystickClick () {
		if (optionSelected == 0) {
			Data.Instance.inputSavedAutomaticPlay.RemoveAllData ();
			Restart ();
		} else if (optionSelected == 1) {			
			Restart ();
		} else if (optionSelected == 2) {
			Data.Instance.inputSavedAutomaticPlay.RemoveAllData ();
			Game.Instance.GotoLevelSelector ();	
		}
		isOn = false;
	}
	void OnJoystickBack () {
		//Data.Instance.events.OnJoystickBack ();
	}
	void ResetMove()
	{
		processAxis = false;
		lastClickedTime = 0;
	}
    IEnumerator Play(Animation animation, string clipName, bool useTimeScale, Action onComplete)
    {

        //We Don't want to use timeScale, so we have to animate by frame..
        if (!useTimeScale)
        {
            AnimationState _currState = animation[clipName];
            bool isPlaying = true;
            float _progressTime = 0F;
            float _timeAtLastFrame = 0F;
            float _timeAtCurrentFrame = 0F;
            float deltaTime = 0F;


            animation.Play(clipName);

            _timeAtLastFrame = Time.realtimeSinceStartup;
            while (isPlaying)
            {
                _timeAtCurrentFrame = Time.realtimeSinceStartup;
                deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
                _timeAtLastFrame = _timeAtCurrentFrame;

                _progressTime += deltaTime;
                _currState.normalizedTime = _progressTime / _currState.length;
                animation.Sample();

                if (_progressTime >= _currState.length)
                {
                    if (_currState.wrapMode != WrapMode.Loop)
                    {
                        isPlaying = false;
                    }
                    else
                    {
                        _progressTime = 0.0f;
                    }

                }

                yield return new WaitForEndOfFrame();
            }
            yield return null;
            if (onComplete != null)
            {
                onComplete();
            }
        }
        else
            animation.Play(clipName);
    }  
}
