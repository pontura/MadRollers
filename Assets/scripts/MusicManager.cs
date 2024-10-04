using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    [SerializeField] private AudioClip explotionAudioClip;
    [SerializeField] private AudioClip interfaces;
    [SerializeField] private AudioClip heartClip;
    [SerializeField] private AudioClip consumeHearts;
    [SerializeField] private AudioClip deathFX;
    [SerializeField] private AudioClip enemyShout;
    [SerializeField] private AudioClip enemyDead;
    [SerializeField] private AudioClip credits;

    [SerializeField] private AudioClip[] bosses;
    [SerializeField] private AudioClip[] songs;
    [SerializeField] private AudioClip[] wins;
    [SerializeField] private AudioClip loading;

    private float heartsDelay = 0.1f;
    private AudioSource audioSource;
	float pitchSpeed = 0.015f;
    public bool mute;

    void Start()
    {

        string s = PlayerPrefs.GetString("mute");
        if (s.ToLower() == "true") mute = true;

        audioSource = GetComponent<AudioSource>();
		Data.Instance.GetComponent<Tracker> ().TrackScreen ("Main Menu");

		Data.Instance.events.OnVersusTeamWon += OnVersusTeamWon;
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
        Data.Instance.events.OnInterfacesStart += OnInterfacesStart;
		Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnGameOver += OnGameOver;
        Data.Instance.events.OnGamePaused += OnGamePaused;
        Data.Instance.events.SetVolume += SetVolume;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarCrash;
		Data.Instance.events.OnMusicStatus += OnMusicStatus;
		Data.Instance.events.FreezeCharacters += FreezeCharacters;

		if (!Data.Instance.musicOn)
			audioSource.enabled = false;
    }
	void OnMusicStatus(bool isOn)
	{
		audioSource.enabled = isOn;
	}
	void OnVersusTeamWon(int teamID)
    {
        if (mute) return;
        playSound( interfaces );
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
    //void OnSoundFX(string name)
    //{
    //    switch (name)
    //    {
    //        case "enemyShout": audioSource.PlayOneShot(enemyShout); break;
    //        case "enemyDead": audioSource.PlayOneShot(enemyDead); break;
    //        case "consumeHearts": audioSource.PlayOneShot(consumeHearts); break;
    //    }
    //}
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
		audioSource.clip = null;
    }

    float nextHeartSoundTime;
    public void addHeartSound()
    {
        if (mute) return;
        if (Time.time >= nextHeartSoundTime)
        {
          audioSource.PlayOneShot(heartClip);
          nextHeartSoundTime = Time.time + heartsDelay;
          //if (Random.Range(0, 500) > 490)
          //{
          //    VoicesManager.Instance.ComiendoCorazones();
          //}
        }
    }
    public void OnExplotionSFX()
    {
        if (mute) return;
        if (Time.time >= nextHeartSoundTime)
        {
            audioSource.PlayOneShot(explotionAudioClip);
            nextHeartSoundTime = Time.time + heartsDelay;
        }
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
        audioSource.clip = songs[Data.Instance.videogamesData.actualID];//) as AudioClip;
        //audioSource.clip = Data.Instance.assetsBundleLoader.GetAssetAsAudioClip("music.all", soundName);
        audioSource.Play();
		audioSource.loop = true;

	}
    public void ToggleMute()
    {
        Data.Instance.musicManager.mute = !Data.Instance.musicManager.mute;
        PlayerPrefs.SetString("mute", Data.Instance.musicManager.mute.ToString());
        if (mute)
            stopAllSounds();
        else
            audioSource.Play();
    }
}
