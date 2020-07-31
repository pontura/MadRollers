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

    private float heartsDelay = 0.1f;
    private AudioSource audioSource;
	float pitchSpeed = 0.015f;

    void Start()
    {		
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
     //   Data.Instance.events.OnSoundFX += OnSoundFX;
      //  Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
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
		playSound( interfaces );
	}
	void FreezeCharacters(bool freezeThem)
	{
		if(freezeThem)
			ChangePitch (0.7f);
		else
			ChangePitch (1);
	}
	public void ChangePitch(float pitchValue)
	{
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
        if (Game.Instance.GetComponent<CharactersManager>().getTotalCharacters() > 0) return;

		ChangePitch (0.2f);
        //audioSource.Stop();
    }
    public void SetVolume(float vol)
    {
        audioSource.volume = vol;
    }
    void playSound(AudioClip _clip, bool looped = true)
    {
        print(audioSource + "playSound " + _clip.name);
		if (audioSource.clip!=null && audioSource.clip.name == _clip.name) return;
        stopAllSounds();
        audioSource.clip = _clip;
        audioSource.Play();
        audioSource.loop = looped;
    }
    public void OnGamePaused(bool paused)
    {
        if(paused)
            audioSource.Stop();
        else
            audioSource.Play();
    }
    void OnInterfacesStart()
    {
        playSound( interfaces );
    }
	public void OnLoadingMusic()
	{
		audioSource.pitch = 1;
		audioSource.clip = Resources.Load("Sound/loading") as AudioClip;
		audioSource.Play();
		audioSource.loop = true;
	}
    void StartMultiplayerRace()
    {
		audioSource.pitch = 1;
		PlayMainTheme ();
    }
	public void BossMusic(bool isBoss)
	{
		int videogameID = Data.Instance.videogamesData.actualID;
//		if (videogameID > 0)
//			return;
		if (isBoss) {
			audioSource.pitch = 1;
		//	audioSource.clip = Resources.Load ("songs/boss" + videogameID) as AudioClip;
			audioSource.clip = Resources.Load ("songs/boss" + videogameID) as AudioClip;
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
        if (Time.time >= nextHeartSoundTime)
        {
          audioSource.PlayOneShot(heartClip);
          nextHeartSoundTime = Time.time + heartsDelay;
          //if (Random.Range(0, 500) > 490)
          //{
          //    Data.Instance.voicesManager.ComiendoCorazones();
          //}
        }
    }
    public void OnExplotionSFX()
    {
        if (Time.time >= nextHeartSoundTime)
        {
            audioSource.PlayOneShot(explotionAudioClip);
            nextHeartSoundTime = Time.time + heartsDelay;
        }
    }
    void OnMissionComplete(int newm)
	{
		StopAllCoroutines ();
		audioSource.pitch = 1;
		audioSource.volume = 1;
		audioSource.clip = Resources.Load("songs/win"+Data.Instance.videogamesData.actualID) as AudioClip;
		//audioSource.clip = Resources.Load("songs/win1") as AudioClip;
		audioSource.Play();
		audioSource.loop = false;

        if(Data.Instance.playMode != Data.PlayModes.STORYMODE)
		    Invoke ("PlayMainTheme", 7);
	}
	void PlayMainTheme()
	{
		string soundName = "song0";
		switch(Data.Instance.videogamesData.actualID)
		{
		case 0:
			soundName = "song0";
			break;
		case 1:
			soundName = "song1";
			break;
		case 2:
			soundName = "song2";
			break;
		}
		audioSource.pitch = 1;
		audioSource.volume = 1;
		audioSource.clip = Resources.Load("songs/" + soundName) as AudioClip;
		audioSource.Play();
		audioSource.loop = true;

	}
}
