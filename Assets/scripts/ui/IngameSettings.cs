using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameSettings : MonoBehaviour
{
    public GameObject panel;
    public GameObject[] soundIcons;
    public Text field;
    bool audioOn = true;

    void Start()
    {
        panel.SetActive(false);
    }    
    public void Open()
    {
        Time.timeScale = 0;
        panel.SetActive(true);
        SetAudio();
    }
    public void Exit()
    {
        Game.Instance.GotoLevelSelector();
    }
    public void ToggleAudio()
    {
        audioOn = !audioOn;
        SetAudio();
    }
    void SetAudio()
    {
        if (audioOn)
        {
            AudioListener.volume = 1f;
            soundIcons[0].SetActive(true);
            soundIcons[1].SetActive(false);
            field.text = "ON";
        }
        else
        {
            AudioListener.volume = 0f;
            soundIcons[1].SetActive(true);
            soundIcons[0].SetActive(false);
            field.text = "OFF";
        }
    }
    public void Close()
    {
        Time.timeScale = 1;
        panel.SetActive(false);
    }
}
