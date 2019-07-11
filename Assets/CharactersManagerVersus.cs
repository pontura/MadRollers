using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManagerVersus : CharactersManager {

    Vector3 characterPosition;

    public GameCamera camera_team_1;
	public GameCamera camera_team_2;

	public List<CharacterBehavior> charactersTeam1;
	public List<CharacterBehavior> charactersTeam2;
	public Transform team1Container;
	public Transform team2Container;
	public float totalDistance;


	public states state;
	public enum states
	{
		FIRST_PART,
		CENTER,
		LAST_PART,
		DONE
	}

	public override void Init()
	{
		print ("INIT");
		totalDistance = 2*( Data.Instance.versusManager.GetArea().z_length) - 2;
		distance = (Data.Instance.versusManager.GetArea().z_length) * -1;
		Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
		Data.Instance.events.OnAvatarFall += OnAvatarFall;
		Data.Instance.events.OnAvatarDie += OnAvatarDie;

		Vector3 pos;

		float _y = 2;

		if (Data.Instance.isReplay)
			_y = 15;

		Vector3 posTeam1 = new Vector3(0, _y, distance);	 
		Vector3 posTeam2 = new Vector3(0, _y, distance*-1);	

		CharacterBehavior cb;
		if (Data.Instance.multiplayerData.player1) { 
			cb = addCharacter (posTeam1, 0); 
			cb.team_for_versus = 1;
			cb.transform.SetParent (team1Container);
			cb.transform.localPosition = new Vector3 (-1, _y, distance);
			charactersTeam1.Add (cb);
			playerPositions.Add (0); 
		} 
		if (Data.Instance.multiplayerData.player2) { 
			cb = addCharacter (posTeam1, 1); 
			cb.team_for_versus = 1;
			cb.transform.SetParent (team1Container);
			cb.transform.localPosition = new Vector3 (1, _y, distance);
			charactersTeam1.Add (cb);
			playerPositions.Add (1); 
		} 
		if (Data.Instance.multiplayerData.player3) { 
			cb = addCharacter (posTeam2, 2); 
			cb.team_for_versus = 2;
			cb.transform.SetParent (team2Container);
			cb.transform.localPosition = new Vector3 (-1, _y, distance);
			charactersTeam2.Add (cb);
			playerPositions.Add (2);	
		} 
		if (Data.Instance.multiplayerData.player4) { 
			cb = addCharacter (posTeam2, 3); 
			cb.team_for_versus = 2;
			cb.transform.SetParent (team2Container);
			cb.transform.localPosition = new Vector3 (1, _y, distance);
			charactersTeam2.Add (cb);
			playerPositions.Add (3);
		}

		Invoke ("AddPowerUps", 0.5f);
	}

	void OnDestroy()
	{
		Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
		Data.Instance.events.OnAvatarFall -= OnAvatarFall;
		Data.Instance.events.OnAvatarDie -= OnAvatarDie;
	}
	void OnAvatarDie(CharacterBehavior cb)
	{
		if (gameOver)
			return;
		if (cb.team_for_versus == 1)
			charactersTeam1.Remove (cb);
		else if (cb.team_for_versus == 2)
			charactersTeam2.Remove (cb);

		if (charactersTeam1.Count == 0) {			
			Data.Instance.events.OnVersusTeamWon (2);
			gameOver = true;
		} else if (charactersTeam2.Count == 0) {
			Data.Instance.events.OnVersusTeamWon (1);
			gameOver = true;
		}
		
	}
	public override Vector3 getPositionByTeam(int teamId)
	{
		List<CharacterBehavior> charactersByTean;

		if(teamId == 1)
			charactersByTean = charactersTeam1;
		else
			charactersByTean = charactersTeam2;
		
		if (charactersByTean.Count > 1)
		{
			Vector3 normalPosition = Vector3.zero;
			Vector3 lastCharacterPosition = Vector3.zero;
			float MaxDistance = 0;
			
			foreach(CharacterBehavior cb in charactersByTean)
			{
				if(lastCharacterPosition != Vector3.zero)
				{
					float dist = Vector3.Distance(cb.transform.localPosition, lastCharacterPosition);
					if(dist>MaxDistance) MaxDistance = dist;
				}
				lastCharacterPosition = cb.transform.localPosition;
				normalPosition += lastCharacterPosition;
			}

			normalPosition /= charactersByTean.Count;
			normalPosition.y += 0.15f + (MaxDistance / 4f );
			normalPosition.z -= 0.2f + (MaxDistance/26);

			return normalPosition;
		}
		else if (charactersByTean.Count == 0) return characterPosition;
		else
			characterPosition = charactersByTean[0].transform.localPosition;

		return characterPosition;
	}
	public override void OnUpdate()
	{
		if (distance > -36 && state == states.FIRST_PART) {
			Data.Instance.events.RalentaTo (0.3f,0.02f);
			state = states.CENTER;
		} else if (distance > -7 && state == states.CENTER) {
			Data.Instance.events.RalentaTo (1,0.02f);
			state = states.LAST_PART;
			powerupsAdded = false;
		}
	}
	public void ResetPositions(float offsetBack)
	{		
		if (state == states.FIRST_PART)
			return;
		
		foreach (CharacterBehavior cb in characters) {
			cb.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			Vector3 pos = cb.transform.localPosition;
			pos.y = 16;
			cb.transform.localPosition = pos;
		}
		
		state = states.FIRST_PART;
		/////////////////////Invoke("AddAutomaticPlayersToAll", 0.5f);		
		distance = -(Data.Instance.versusManager.GetArea().z_length+offsetBack);
		camera_team_1.ResetVersusPosition ();
		camera_team_2.ResetVersusPosition ();
		AddPowerUps ();
	}
	void AddAutomaticPlayersToAll()
	{
		int totalAvatars = characters.Count;
		for (int a = 0; a < totalAvatars; a++) {
			if (!characters [a].controls.isAutomata) {
				CharacterBehavior cb = AddChildPlayer (characters [a]);
				cb.team_for_versus = characters [a].team_for_versus;
				if(cb.team_for_versus == 1)
					cb.transform.SetParent (team1Container);
				else if(cb.team_for_versus == 2)
					cb.transform.SetParent (team2Container);
			}
		}
	}
	bool powerupsAdded;
	void AddPowerUps()
	{
		if (powerupsAdded)
			return;
		
		Game.Instance.level.GetComponent<PowerupsManager> ().ResetVersusPowerups ();
		Data.Instance.events.OnAddSpecificPowerUp("Missile", new Vector3(12,10,0));
		Data.Instance.events.OnAddSpecificPowerUp("Invencible", new Vector3(-12,10,0));
		powerupsAdded = true;
	}
}
