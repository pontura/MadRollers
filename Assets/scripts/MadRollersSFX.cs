using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MadRollersSFX : MonoBehaviour {

	public enum types
	{
		ENGINES,
		JUMP,
		CHEER,
		CRASH,
		TOUCH_GROUND,
		DOUBLE_JUMP,
        DASH,
        FALL,
        COLEADA
	}
	public PlayerClips[] playerClips;
	[Serializable]
	public class PlayerClips
	{
		public AudioClip engines;
		public AudioClip jump;
		public AudioClip crash;
		public AudioClip[] cheer;
		public AudioClip touchGround;
		public AudioClip doubleJump;
        public AudioClip dash;
        public AudioClip fall;
        public AudioClip coleada;
    }

	public AudioSource player1;
	public AudioSource player2;
	public AudioSource player3;
	public AudioSource player4;

	void Start () {
        DontDestroyOnLoad(this);

        Data.Instance.events.OnMadRollerFX += OnMadRollerFX;	
		Data.Instance.events.OnGameOver += OnGameOver;
		Data.Instance.events.OnMadRollersSFXStatus += OnMadRollersSFXStatus;
        
        OnMadRollersSFXStatus( Data.Instance.madRollersSoundsOn);
	}
    void OnDestroy()
    {
        Data.Instance.events.OnMadRollerFX -= OnMadRollerFX;
        Data.Instance.events.OnGameOver -= OnGameOver;
        Data.Instance.events.OnMadRollersSFXStatus -= OnMadRollersSFXStatus;
    }
    void OnMadRollersSFXStatus(bool isOn)
	{
		player1.enabled = isOn;
		player2.enabled = isOn;
		player3.enabled = isOn;
		player4.enabled = isOn;
	}
	void OnMadRollerFX(types type, int id)
    {
        if (Data.Instance.musicManager.mute) return;
        AudioSource audioSource;
		switch(id)
		{
		case 0: audioSource = player1; break;
		case 1: audioSource = player2; break;
		case 2: audioSource = player3; break;
		default: audioSource = player4; break;			
		}
		AudioClip ac = null;
		switch(type)
		{
		case types.ENGINES:
			ac = playerClips [id].engines;
			audioSource.loop = true; 
			break;
		case types.JUMP: 
			ac = playerClips[id].jump; 
			audioSource.loop = false; 
			break;
		case types.CRASH: 
			ac = playerClips[id].crash; 
			audioSource.loop = false; 
			break;
		case types.CHEER: 
			ac = GetRandom(playerClips[id].cheer); 
			audioSource.loop = false; 
			break;
		case types.TOUCH_GROUND: 
			ac = playerClips[id].touchGround; 
			audioSource.loop = false; 
			break;
		case types.DOUBLE_JUMP: 
			ac = playerClips[id].doubleJump; 
			audioSource.loop = false; 
			break;
        case types.DASH:
            ac = playerClips[id].dash;
            audioSource.loop = false;
            break;
        case types.COLEADA:
            ac = playerClips[id].coleada;
            audioSource.loop = false;
            break;
        }
		audioSource.clip = ac;
		audioSource.Play ();
	}
    AudioClip GetRandom(AudioClip[] arr)
    {
        return arr[UnityEngine.Random.Range(0, arr.Length - 1)];
    }
	void OnGameOver(bool isTimeOver)
    {
        if (Data.Instance.musicManager.mute) return;
        player1.Stop ();
		player2.Stop ();
		player3.Stop ();
		player4.Stop ();
	}


}
