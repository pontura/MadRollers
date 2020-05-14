using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Summary : MonoBehaviour {
    
    private int countDown;
    public Animation anim;

    public GameObject mobilePanel;

	public int optionSelected = 0;
    private bool isOn;

	float delayToReact = 0.3f;

    void Start()
    {
        mobilePanel.SetActive(false);
        if (Data.Instance.isAndroid)
            Data.Instance.events.OnGameOver += OnGameOver;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnGameOver -= OnGameOver;
    }
    void OnGameOver(bool isTimeOver)
    {
        if (isOn) return;
        isOn = true;
        Invoke("SetOn", 2F);
    }
    void SetOn()
    {
        Data.Instance.events.RalentaTo(1, 0.05f);
        mobilePanel.SetActive(true);
        StartCoroutine(Play(anim, "popupOpen", false, null));
    }
    public void Restart()
	{
		Data.Instance.isReplay = true;
		Game.Instance.ResetLevel();        
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
