using System.Threading;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioSource audioSourceGrab;
    public AudioSource audioSourceFloorExplotions;

    [SerializeField] AudioClip popup;
    [SerializeField] AudioClip popupClose;

    [SerializeField] AudioClip typing;
    [SerializeField] AudioClip boss_punch;
    [SerializeField] AudioClip boss_attack_init;
    [SerializeField] AudioClip bossDie;
    [SerializeField] AudioClip fire_me;
    [SerializeField] AudioClip fire_other;
    [SerializeField] AudioClip floor;
    [SerializeField] AudioClip coin;
    [SerializeField] AudioClip combo;
    [SerializeField] AudioClip FX_break;
    [SerializeField] AudioClip explotion;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip laser;
    [SerializeField] AudioClip whip;
    [SerializeField] AudioClip countDownStart;
    [SerializeField] AudioClip countDown;
    [SerializeField] AudioClip laserDrop;

    [SerializeField] AudioClip pixelGrab;
    [SerializeField] AudioClip deathFX;
    [SerializeField] AudioClip enemyDead;
    [SerializeField] AudioClip continueClip;

    public bool mute;

    private AudioSource loopAudioSource;
    public float volume;

    void Start()
    {
        audioSourceFloorExplotions.clip = explotion;
        audioSourceGrab.clip = pixelGrab;
        
        OnSoundsVolumeChanged(volume);

        Events.OnGamePaused += OnGamePaused;
        Events.OnAvatarShoot += OnAvatarShoot;
        Events.OnSoundFX += OnSoundFX;
		Events.OnSFXStatus += OnSFXStatus;
        Events.OnGrabHeart += OnGrabHeart;
        Events.OnAddExplotion += OnAddExplotion;
        Events.SetSoundsVolume += SetSoundsVolume;
        Events.MuteSounds += MuteSounds;

        if (!Data.Instance.soundsFXOn)
			audioSource.enabled = false;
	}
	void OnSFXStatus(bool isOn)
	{
		audioSource.enabled = isOn;
	}
    void OnHeroDie()
    {
    }
    void OnDestroy()
    {
        Events.OnGamePaused -= OnGamePaused;
        Events.OnAvatarShoot -= OnAvatarShoot;
        Events.OnSoundFX -= OnSoundFX;
		Events.OnSFXStatus -= OnSFXStatus;
        Events.OnGrabHeart -= OnGrabHeart;
        Events.OnAddExplotion -= OnAddExplotion;
        Events.SetSoundsVolume -= SetSoundsVolume;
        Events.MuteSounds -= MuteSounds;

        if (loopAudioSource)
        {
            loopAudioSource = null;
            loopAudioSource.Stop();
        }
    }
    void MuteSounds(bool mute)
    {
        this.mute = mute;
    }
    private void SetSoundsVolume(float v)
    {
        OnSoundsVolumeChanged(v);
    }

    private void OnGamePaused(bool isOn)
    {
        if(isOn)
            OnSoundFX("popup");
        else
            OnSoundFX("popupClose");
    }

    void OnMissionComplete(int id)
    {
        audioSource.volume = 0;
    }
    private void OnAvatarShoot(int playerID)
    {
        if (playerID == 0)
            OnSoundFX("fire_me");
        else
            OnSoundFX("fire_other");
    }

    void OnAddExplotion(Vector3 pos, Color c)
    {
        if (mute) return;
        audioSourceFloorExplotions.Play();
    }
    void OnSoundsVolumeChanged(float value)
    {
        audioSource.volume = value;
        audioSourceGrab.volume = value;
        audioSourceFloorExplotions.volume = value;
        volume = value;
    }
   
    float nextSoundTime;
    float delayToNextSound = 0.05f;

  

    void OnSoundFX(string soundName)
    {
        if (mute) return;
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
            case "popup": audioSource.PlayOneShot(popup); break;
            case "popupClose": audioSource.PlayOneShot(popupClose); break;
            case "typing": audioSource.PlayOneShot(typing); break;
            case "countDownStart": audioSource.PlayOneShot(countDownStart); break;
            case "countDown": audioSource.PlayOneShot(countDown); break;
            case "fire_me": audioSource.PlayOneShot(fire_me); break;
            case "fire_other": audioSource.PlayOneShot(fire_other); break;
            case "floor": audioSource.PlayOneShot(floor); break;
            case "coin": audioSource.PlayOneShot(coin); break;
            case "combo": audioSource.PlayOneShot(combo); break;
            case "FX_break": audioSource.PlayOneShot(FX_break); break;
            case "hit": audioSource.PlayOneShot(hit); break;
            case "laser": audioSource.PlayOneShot(laser); break;
            case "whip": audioSource.PlayOneShot(whip); break;
            case "enemyDead": audioSource.PlayOneShot(enemyDead); break;
            case "deathFX": audioSource.PlayOneShot(deathFX); break;
            case "continueClip": audioSource.PlayOneShot(continueClip); break;
            case "bossDie": audioSource.PlayOneShot(bossDie); break;
            case "boss_attack_init": audioSource.PlayOneShot(boss_attack_init); break;
            case "boss_punch": audioSource.PlayOneShot(boss_punch); break;
            case "laserDrop": audioSource.PlayOneShot(laserDrop); break;
        }
    }
    private float heartsDelay = 0.1f;
    float nextHeartSoundTime;
    public void OnGrabHeart()
    {
        if (mute) return;
        if (Time.time >= nextHeartSoundTime)
        {
            audioSourceGrab.Play();
            nextHeartSoundTime = Time.time + heartsDelay;
        }
    }
    private string GetRandomSound(string[] arr)
    {
        return arr[Random.Range(0, arr.Length-1)];
    }
}
