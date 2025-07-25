using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameSettings : MonoBehaviour
{
    public GameObject panel;
    public GameObject[] soundIcons;
    public TMPro.TMP_Text field;
    public TMPro.TMP_Text fieldExit;
    bool audioOn = true;

    void Start()
    {
        panel.SetActive(false); 
        fieldExit.text = "MAIN MENU";
    }    
    public void Open()
    {
        Time.timeScale = 0;
        panel.SetActive(true);
        SetAudio();
        AudioListener.volume = 0f;
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
            soundIcons[0].SetActive(true);
            soundIcons[1].SetActive(false);
            field.text = "ON";
        }
        else
        {
            soundIcons[1].SetActive(true);
            soundIcons[0].SetActive(false);
            field.text = "OFF";
        }
    }
    public void Close()
    {
        if(field.text == "ON")
            AudioListener.volume = 1f;
        Events.RalentaTo(1, 0.15f);
        panel.SetActive(false);
    }
}
