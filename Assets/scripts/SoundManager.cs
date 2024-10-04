using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    

    public AudioSource audioSource;
    private AudioSource loopAudioSource;
    public float volume;

    void Start()
    {
        OnSoundsVolumeChanged(volume);		
        Data.Instance.events.OnSoundFX += OnSoundFX;
		Data.Instance.events.OnSFXStatus += OnSFXStatus;

		if (!Data.Instance.soundsFXOn)
			audioSource.enabled = false;
	}
	void OnSFXStatus(bool isOn)
	{
		audioSource.enabled = isOn;
	}
    void OnHeroDie()
    {
        OnSoundFXLoop("");
    }
    void OnDestroy()
    {
        Data.Instance.events.OnSoundFX -= OnSoundFX;
		Data.Instance.events.OnSFXStatus -= OnSFXStatus;
        if (loopAudioSource)
        {
            loopAudioSource = null;
            loopAudioSource.Stop();
        }
    }
    void OnSoundsVolumeChanged(float value)
    {
        audioSource.volume = value;
        volume = value;

        if (value == 0 || value == 1)
            PlayerPrefs.SetFloat("SFXVol", value);
    }
    void OnSoundFXLoop(string soundName)
    {
        if (volume == 0) return;

        if (!loopAudioSource)
            loopAudioSource = gameObject.AddComponent<AudioSource>() as AudioSource;

        if (soundName != "")
        {
            loopAudioSource.clip = Resources.Load("Sound/" + soundName) as AudioClip;
            loopAudioSource.Play();
            loopAudioSource.loop = true;
        }
        else
        {
            loopAudioSource.Stop();
        }
    }
    float nextSoundTime;
    float delayToNextSound = 0.05f;

    [SerializeField] AudioClip fire;
    [SerializeField] AudioClip floor;
    [SerializeField] AudioClip coin;
    [SerializeField] AudioClip combo;
    [SerializeField] AudioClip FX_break;
    [SerializeField] AudioClip explotion;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip laser;

    void OnSoundFX(string soundName, int playerID)
    {
        if (Data.Instance.musicManager.mute) return;
        if (soundName == "")
        {
            audioSource.Stop();
            return;
        }
        if (Time.time <= nextSoundTime && soundName != "combo")
        {
            return;
        }
        nextSoundTime = Time.time + delayToNextSound;
       
        if (volume == 0) return;
            audioSource.panStereo = 0;

        switch(soundName)
        {
            case "fire": audioSource.PlayOneShot(fire); break;
            case "floor": audioSource.PlayOneShot(floor); break;
            case "coin": audioSource.PlayOneShot(coin); break;
            case "combo": audioSource.PlayOneShot(combo); break;
            case "FX_break": audioSource.PlayOneShot(FX_break); break;
            case "explotion": audioSource.PlayOneShot(explotion); break;
            case "hit": audioSource.PlayOneShot(hit); break;
            case "laser": audioSource.PlayOneShot(laser); break;

        }
    }
    private string GetRandomSound(string[] arr)
    {
        return arr[Random.Range(0, arr.Length-1)];
    }
}
