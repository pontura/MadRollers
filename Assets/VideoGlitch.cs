using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Video;

public class VideoGlitch : MonoBehaviour {

	//public VideoPlayer videoPlayer;
	public GameObject panel;

	private void Start()
	{
		//panel.SetActive (false);
		//videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
		//videoPlayer.Prepare();
	//	Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
	//	Data.Instance.events.OnGameOver += OnGameOver;
	}
	public void Init()
	{
		//panel.SetActive (true);
	}
	void StartMultiplayerRace()
	{
		//videoPlayer.Pause ();
	}
	void OnGameOver(bool isTimeOver)
	{
		//videoPlayer.Play ();
	}
//	private void VideoPlayer_prepareCompleted(VideoPlayer source)
//	{
//	//	videoPlayer.Play();
//	}  
}
