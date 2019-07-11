using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    private AudioSource loopAudioSource;
    public float volume;
    public string coin;

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
    void OnSoundFX(string soundName, int playerID)
    {
        if (soundName == "")
        {
            audioSource.Stop();
            return;
        }
        if (volume == 0) return;

        if (playerID == 0)
            audioSource.panStereo = -0.8f;
        else if (playerID == 1)
            audioSource.panStereo = -0.3f;
        else if (playerID == 2)
            audioSource.panStereo = 0.3f;
        else if (playerID == 4)
            audioSource.panStereo = 0.8f;
        else
            audioSource.panStereo = 0;

        audioSource.PlayOneShot(Resources.Load("Sound/" + soundName) as AudioClip);
    }
    private string GetRandomSound(string[] arr)
    {
        return arr[Random.Range(0, arr.Length-1)];
    }
}
