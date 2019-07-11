using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersSelector : MonoBehaviour {

	public PlayerSelectorUI[] playerSelectors;
	public PlayerMainMenuUI[] playersMainMenu;

	void Start () {
		Data.Instance.events.OnJoystickClick += OnJoystickClick;
		Data.Instance.events.OnJoystickBack += OnJoystickBack;
		int id = 0;
		foreach (PlayerSelectorUI playerSelector in playerSelectors) {
			if (id >= Data.Instance.totalJoysticks) {
				playerSelector.SetOff ();
			}
			playerSelector.Init (id);
			playerSelector.SetUI (playersMainMenu [id]);
			id++;
		}
	}
	void OnDestroy()
	{
		Data.Instance.events.OnJoystickClick -= OnJoystickClick;
		Data.Instance.events.OnJoystickBack -= OnJoystickBack;
	}
	void OnJoystickBack()
	{
		Data.Instance.LoadLevel("MainMenu");
	}
	void OnJoystickClick()
	{
		Data.Instance.LoadLevel("MainMenu");
	}
	void Update()
	{
		for (int playerId = 0; playerId < Data.Instance.totalJoysticks; playerId++) {
            float _speed = Data.Instance.inputManager.GetAxis(playerId, InputAction.horizontal);
                if (_speed < -0.5f)
				MoveLeft (playerId);
			else if(_speed > 0.5f)
				MoveRight (playerId);
		}
	}
	void MoveLeft(int playerID)
	{
		PlayerSelectorUI playerSelector = playerSelectors [playerID];
		if (playerSelector.moveing)
			return;
		PlayerMainMenuUI p = null;
		if (playerSelector.uiID == 0)
			playerSelector.uiID  = playersMainMenu.Length-1;
		else
			playerSelector.uiID  = playerSelector.uiID-1;
		p = GetPlayerMainMenuUI (playerSelector.uiID );
		if (p == null)
			MoveLeft (playerID);
		else 
			playerSelector.SetUI (p);
	}
	void MoveRight(int playerID)
	{
		PlayerSelectorUI playerSelector = playerSelectors [playerID];
		if (playerSelector.moveing)
			return;
		PlayerMainMenuUI p = null;
		if (playerSelector.uiID == playersMainMenu.Length-1)
			playerSelector.uiID  = 0;
		else
			playerSelector.uiID  = playerSelector.uiID+1;
		p = GetPlayerMainMenuUI (playerSelector.uiID );
		if (p == null)
			MoveRight (playerID);
		else
			playerSelector.SetUI (p);
	}
	PlayerMainMenuUI GetPlayerMainMenuUI(int id)
	{
		if (!IsUsed (id))
			return playersMainMenu [id];
		else
			return null;
	}
	bool IsUsed(int uiID)
	{
		int id = 0;
		foreach (PlayerSelectorUI playerSelector in playerSelectors)
		{
			if (id >= Data.Instance.totalJoysticks)
				return false;
			if (playerSelector.ui.id == uiID)
				return true;
			id++;
		}
		return false;
	}
	
}
