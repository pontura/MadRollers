using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    [SerializeField] private AudioClip interfaces;
    [SerializeField] private AudioClip[] bosses;
    [SerializeField] private AudioClip[] songs;
    [SerializeField] private AudioClip[] wins;
    [SerializeField] private AudioClip loading;
    public SoundManager soundManager;
           
    [SerializeField] AudioSource audioSource;
	float pitchSpeed = 0.015f;
    public bool mute;

    static MusicManager mInstance = null;
    private void Awake()
    {
        if (!mInstance)  mInstance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }
    public static MusicManager Instance
    {
        get{if (mInstance == null) Debug.LogError("Algo llama a MusicManager antes de inicializarse"); return mInstance; }
    }
    void Start()
    {
        Events.OnContinue += OnContinue;
        Events.StartMultiplayerRace += StartMultiplayerRace;
        Events.OnInterfacesStart += OnInterfacesStart;
		Events.OnMissionComplete += OnMissionComplete;
        Events.OnGameOver += OnGameOver;
        Events.OnGamePaused += OnGamePaused;
        Events.SetVolume += SetVolume;
        Events.OnAvatarCrash += OnAvatarCrash;
        Events.OnAvatarFall += OnAvatarCrash;
		Events.OnMusicStatus += OnMusicStatus;
		Events.FreezeCharacters += FreezeCharacters;
        Events.MuteMusic += MuteMusic;

        if (!Data.Instance.musicOn)
			audioSource.enabled = false;
    }
    void OnDestroy()
    {
        Events.OnContinue -= OnContinue;
        Events.StartMultiplayerRace -= StartMultiplayerRace;
        Events.OnInterfacesStart -= OnInterfacesStart;
        Events.OnMissionComplete -= OnMissionComplete;
        Events.OnGameOver -= OnGameOver;
        Events.OnGamePaused -= OnGamePaused;
        Events.SetVolume -= SetVolume;
        Events.OnAvatarCrash -= OnAvatarCrash;
        Events.OnAvatarFall -= OnAvatarCrash;
        Events.OnMusicStatus -= OnMusicStatus;
        Events.FreezeCharacters -= FreezeCharacters;
        Events.MuteMusic -= MuteMusic;
    }
    void OnMusicStatus(bool isOn)
	{
		audioSource.enabled = isOn;
	}
	
	void FreezeCharacters(bool freezeThem)
    {
        if (mute) return;
        if (freezeThem)
			ChangePitch (0.7f);
		else
			ChangePitch (1);
	}
	public void ChangePitch(float pitchValue)
    {
        if (mute) return;
        StopAllCoroutines ();
		StartCoroutine (ChangePitchCoroutine (pitchValue));
	}

	IEnumerator ChangePitchCoroutine(float pitchValue)
	{
		if (pitchValue < audioSource.pitch) {
			while (pitchValue < audioSource.pitch) {
				audioSource.pitch -= pitchSpeed;
				yield return new WaitForEndOfFrame ();
			}
		} else {
			while (pitchValue > audioSource.pitch) {
				audioSource.pitch += pitchSpeed;
				yield return new WaitForEndOfFrame ();
			}
		}
		yield return null;
	}
    
    void ResetFilter()
    {
		ChangePitch (1);
    }
    void OnContinue()
    {
        ChangePitch(1);
    }
    void OnAvatarCrash(CharacterBehavior cb)
    {
        if (mute) return;
        if (Game.Instance.GetComponent<CharactersManager>().getTotalCharacters() > 0) return;

		ChangePitch (0.2f);
        //audioSource.Stop();
    }
    public void SetVolume(float vol)
    {
        if (mute) return;
        audioSource.volume = vol;
    }
    void playSound(AudioClip _clip, bool looped = true)
    {
        if (mute) return;
        print(audioSource + "playSound " + _clip.name);
		if (audioSource.clip!=null && audioSource.clip.name == _clip.name) return;
        stopAllSounds();
        audioSource.clip = _clip;
        audioSource.Play();
        audioSource.loop = looped;
    }
    public void OnGamePaused(bool paused)
    {
        if (mute) return;
        if (paused)
            audioSource.Stop();
        else
            audioSource.Play();
    }
    void OnInterfacesStart()
    {
        if (mute) return;
        playSound( interfaces );
    }
	public void OnLoadingMusic()
    {
        if (mute) return;
        audioSource.pitch = 1;
		audioSource.clip = loading;
		audioSource.Play();
		audioSource.loop = true;
	}
    void StartMultiplayerRace()
    {
        print("StartMultiplayerRace " + mute);
        if (mute) return;
        audioSource.pitch = 1;
		PlayMainTheme ();
    }
	public void BossMusic(bool isBoss)
    {
        if (mute) return;
        int videogameID = Data.Instance.videogamesData.actualID;
//		if (videogameID > 0)
//			return;
		if (isBoss) {
			audioSource.pitch = 1;
			audioSource.clip = bosses [videogameID];//) as AudioClip;
		//	audioSource.clip = Data.Instance.assetsBundleLoader.GetAssetAsAudioClip("music.all", "boss" + videogameID);
			audioSource.Play ();
			audioSource.loop = true;
		}
	}
//    void OnAvatarChangeFX(Player.fxStates state)
//    {
//		if (state == Player.fxStates.NORMAL)
//			PlayMainTheme ();
//        else
//            playSound(IndestructibleFX);
//    }
    void OnGameOver(bool gameOver)
    {
		ChangePitch (0.2f);
    }
    public void stopAllSounds()
    {
        audioSource.Stop();
    }

    void OnMissionComplete(int newm)
    {
        if (mute) return;
        StopAllCoroutines ();
		audioSource.pitch = 1;
		audioSource.volume = 1;
        audioSource.clip = wins[Data.Instance.videogamesData.actualID];//) as AudioClip;
      //  audioSource.clip = Data.Instance.assetsBundleLoader.GetAssetAsAudioClip("music.all", "win" + Data.Instance.videogamesData.actualID);
        audioSource.Play();
		audioSource.loop = false;

      //  if(Data.Instance.playMode != Data.PlayModes.STORYMODE)
		    //Invoke ("PlayMainTheme", 7);
	}
	void PlayMainTheme()
    {
        if (mute) return;
        audioSource.pitch = 1;
		audioSource.volume = 1;
        audioSource.clip = songs[Data.Instance.videogamesData.actualID];
        audioSource.Play();
		audioSource.loop = true;

	}
    public void ToggleMute()
    {
        MusicManager.Instance.mute = !MusicManager.Instance.mute;
        MuteMusic(mute);
    }
    public void MuteMusic(bool mute)
    {
        this.mute = mute;
        if (mute)
            stopAllSounds();
        else
            audioSource.Play();
    }
}
