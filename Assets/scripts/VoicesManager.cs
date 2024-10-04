using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VoicesManager : MonoBehaviour
{
	public List<VoiceData> tutorials;
	public List<VoiceData> intros;
	public List<VoiceData> welcome;
	public List<VoiceData> missionComplete;

	public List<VoiceData> deads;

	public List<VoiceData> killThemAll;

	public List<VoiceData> lose_bad;
	public List<VoiceData> lose_good;
	public List<VoiceData> lose_great;
    public List<VoiceData> countDown;

    public List<VoiceData> videogames_names;

	public List<VoiceData> UIItems;

	[Serializable]
	public class VoiceData
	{
		public string text;
		public AudioClip audioClip;
	}
	public AudioSource audioSource;
    public static VoicesManager mInstance;
    public static VoicesManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                Debug.LogError("Algo llama a DATA antes de inicializarse");
            }
            return mInstance;
        }
    }
    void Awake()
    {

        if (!mInstance)
            mInstance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }
    bool IsBundleReady()
    {
        if(tutorials.Count == 0)
            return false;
        return true;
    }
    public void Init()
    {
		audioSource.enabled = Data.Instance.voicesOn;

		if (!Data.Instance.voicesOn)
			return;
		
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarFall;
        Data.Instance.events.OnAvatarChangeFX += OnAvatarChangeFX;
        Data.Instance.events.SetVolume += SetVolume;
        Data.Instance.events.VoiceFromResources += VoiceFromResources; 
		Data.Instance.events.OnVoicesStatus += OnVoicesStatus;

		if (!Data.Instance.voicesOn)
			audioSource.enabled = false;
	}
	void OnVoicesStatus(bool isOn)
	{
		audioSource.enabled = isOn;
	}
    void SetVolume(float vol)
    {
        audioSource.volume = vol;
    }
    private void OnMissionComplete(int id)
    {
        if (Data.Instance.musicManager.mute) return;
		PlayRandom (missionComplete);

		if(Data.Instance.playMode == Data.PlayModes.PARTYMODE)
			Invoke ("NextDestination", 7);
    }
	void NextDestination()
    {
        if (Data.Instance.musicManager.mute) return;
        VoicesManager.Instance.PlaySpecificClipFromList (VoicesManager.Instance.UIItems, 6);
	}
    private void OnAvatarCrash(CharacterBehavior cb)
    {
        if (Data.Instance.musicManager.mute) return;
        if (Game.Instance.level.charactersManager.getTotalCharacters()<=1)
			Dead ();
    }
    private void OnAvatarFall(CharacterBehavior cb)
    {
		PlayRandom (deads);
    }
	void Dead()
    {
        if (Data.Instance.musicManager.mute) return;
        float distance = Game.Instance.level.charactersManager.distance;
		if (distance < 100)
			PlayRandom (lose_bad);
		else if (distance < 1000)
			PlayRandom (lose_good);
		else
			PlayRandom (lose_great);
	}
    private void OnAvatarChangeFX(Player.fxStates state)
    {
    }
    public void PlayCountDown(int id)
    {
        if (Data.Instance.musicManager.mute) return;
        if (IsBundleReady())
            PlayClip(countDown[id].audioClip);
    }

	int sequenceID = 0;
	bool onSequence = false;
	List<VoiceData> sequenceSaying;
	public void PlaySequence( List<VoiceData> clips)
    {
        if (Data.Instance.musicManager.mute) return;
        if (!IsBundleReady())
            return;
        if (clips.Count == 0)
			return;
		sequenceID = 0;
		talking = false;
		audioSource.Stop ();
		this.sequenceSaying = clips;
		onSequence = true;
		PlayNextSequencedClip ();
	}
	void PlayNextSequencedClip()
    {
        if (Data.Instance.musicManager.mute) return;
        if (!IsBundleReady())
            return;
        VoiceData newAudio = sequenceSaying[sequenceID];
		print (onSequence + " " + newAudio.audioClip + " " + sequenceID + "    count: " + sequenceSaying.Count);
		PlayClip(newAudio.audioClip); 
		sequenceID++;
		if (sequenceSaying.Count == sequenceID)
		{
			onSequence = false;
			Done ();
		}
	}
	public void PlaySpecificClipFromList( List<VoiceData> clips, int id)
    {
        if (Data.Instance.musicManager.mute) return;
        if (!IsBundleReady())
            return;
        PlayClip(clips[id].audioClip); 
	}
	public void PlayRandom( List<VoiceData> clips)
    {
        if (Data.Instance.musicManager.mute) return;
        if (!IsBundleReady())
            return;
        int rand = UnityEngine.Random.Range(0, clips.Count);
		PlayClip(clips[rand].audioClip); 
    }
    public void ComiendoCorazones()
    {
    }
    public void VoiceSecondaryFromResources(string name)
    {
    }
    public void VoiceFromResources(string name)
    {
    }
	bool talking;
	public void PlayClip(AudioClip audioClip)
    {
        if (!IsBundleReady() || audioClip == null)
            return;
        talking = true;
		//audioSpectrum.SetOn ();
        audioSource.clip = audioClip;
        audioSource.Play();
		Data.Instance.events.OnTalk (true);
    }
	float timer;
	void Update()
	{
        if (!IsBundleReady())
            return;
        if (!talking)
			return;
		
		if (audioSource.clip != null && audioSource.clip.length>0.1f && audioSource.time >= (audioSource.clip.length-0.02f)) {
			Done ();
		}
	}
	void Done()
	{
		if (onSequence)
			PlayNextSequencedClip ();
		else {
			//audioSpectrum.SetOff ();
			talking = false;			
			Data.Instance.events.OnTalk (false);
		}
	}
}
