using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static BossAttacksManager;

public class IngameSettings : MonoBehaviour
{
    public GameObject panel;
    public GameObject[] musicIcons;
    public GameObject[] soundIcons;
    public TMPro.TMP_Text field;
    public TMPro.TMP_Text soundsField;
    public TMPro.TMP_Text fieldExit;
    bool audioOn = true;
    bool soundsOn = true;

    void Start()
    {
        panel.SetActive(false); 
        fieldExit.text = "MAIN MENU";
    }    
    public void Open()
    {
        audioOn = !MusicManager.Instance.mute;
        soundsOn = !MusicManager.Instance.soundManager.mute;
        Time.timeScale = 0;
        panel.SetActive(true);
        SetAudio();
        SetSounds();
        Events.OnGamePaused(true);
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
            musicIcons[0].SetActive(true);
            musicIcons[1].SetActive(false);
            field.text = "MUSIC ON";
        }
        else
        {
            musicIcons[1].SetActive(true);
            musicIcons[0].SetActive(false);
            field.text = "MUSIC OFF";
        }
    }
    public void ToggleSounds()
    {
        soundsOn = !soundsOn;
        SetSounds();
    }
    void SetSounds()
    {
        if (soundsOn)
        {
            soundIcons[0].SetActive(true);
            soundIcons[1].SetActive(false);
            soundsField.text = "SOUNDS ON";
        }
        else
        {
            soundIcons[1].SetActive(true);
            soundIcons[0].SetActive(false);
            soundsField.text = "SOUNDS OFF";
        }
    }
    public void Close()
    {
        Events.MuteMusic(!audioOn);
        Events.MuteSounds(!soundsOn);

        Events.RalentaTo(1, 0.15f);
        panel.SetActive(false);
        Events.OnGamePaused(false);
    }
}
