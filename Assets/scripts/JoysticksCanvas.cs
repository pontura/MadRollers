using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoysticksCanvas : MonoBehaviour {

	public List<JoystickPlayer> players;
	bool isMissionComplete;
    public GameObject standalonePanel;

	void Start()
	{
        if (Data.Instance.isAndroid)
        {
            standalonePanel.SetActive(false);
            return;
        }
        Data.Instance.events.OnGameOver += OnGameOver;
        Data.Instance.events.OnAvatarDie += OnAvatarDie;
        Data.Instance.events.OnAddNewPlayer += OnAddNewPlayer;
        Data.Instance.events.OnMissionComplete += OnMissionComplete;

        if (Data.Instance.playMode != Data.PlayModes.PARTYMODE)
            Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;

        standalonePanel.SetActive(true);
      
	}
	void OnDestroy()
	{
		Data.Instance.events.OnAddNewPlayer -= OnAddNewPlayer;
		Data.Instance.events.OnAvatarDie -= OnAvatarDie;
		Data.Instance.events.OnGameOver -= OnGameOver;
		Data.Instance.events.OnMissionComplete -= OnMissionComplete;
		Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
	}
	void OnListenerDispatcher(ListenerDispatcher.myEnum message)
	{
		if (isMissionComplete  && message == ListenerDispatcher.myEnum.LevelFinish) {
			isMissionComplete = false;
			foreach (JoystickPlayer jp in players)
				jp.HideResults ();
		}
	}
	void OnMissionComplete(int missionID)
	{
		isMissionComplete = true;
		foreach (JoystickPlayer jp in players)
			jp.ShowResults ();
	}
	void OnGameOver(bool isTimeOver)
	{
        foreach (JoystickPlayer jp in players)
            jp.OnGameOver(false);
        
	}
	public void RefreshStates() 
	{
		Invoke ("RefreshStatesDelayed", 0.1f);
	}
	public void OnAddNewPlayer(int playerID) 
	{
		players [playerID].SetState (JoystickPlayer.states.PLAYING);
	}
	public void OnAvatarDie(CharacterBehavior cb) 
	{
		if (cb.controls.isAutomata)
			return;
		players [cb.player.id].SetState (JoystickPlayer.states.DEAD);
	}
	public void RefreshStatesDelayed()	
	{
		foreach (JoystickPlayer jp in players)
			jp.RefreshStates ();
	}
	public bool CanRevive(int playerID)
	{
		if (players [playerID].state == JoystickPlayer.states.INSERT_COIN)
			return true;
		return false;
	}

}
