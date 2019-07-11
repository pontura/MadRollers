using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerData : MonoBehaviour
{
	public int NextScoreToWinCredit;
	public int ScoreToWinCredit;
	public int score;
   	public int creditsWon;
    public float distance;

    public bool player1;
    public bool player2;
    public bool player3;
    public bool player4;

    public Color[] colors;

    public List<int> players;

    public int score_player1;
    public int score_player2;
    public int score_player3;
    public int score_player4;

	public bool player1_played;
	public bool player2_played;
	public bool player3_played;
	public bool player4_played;

    void Start()
    {
		Data.Instance.events.OnGameStart += OnGameStart;
		Data.Instance.events.OnMissionComplete += OnMissionComplete;
		Data.Instance.events.OnScoreOn += OnScoreOn;
        Data.Instance.events.OnReorderAvatarsByPosition += OnReorderAvatarsByPosition;
		Data.Instance.events.OnResetScores += OnResetScores;
    }
	void OnGameStart()
	{
		SetNextScoreToWinCredit ();
	}
	void OnResetScores()
	{
		NextScoreToWinCredit = 0;
		score_player1 = score_player2 = score_player3 = score_player4 = 0;
		player1 = player2 = player3 = player4 = false;
		player1_played = player2_played = player3_played = player4_played = false;
		score = 0;
		distance = 0;
		creditsWon = 0;
		Data.Instance.RefreshCredits ();
	}
    void OnReorderAvatarsByPosition(List<int> _players)
    {
        players = _players;
    }
	public void SelectAllPlayers()
	{
		player1 = true;
		player2 = true;
		player3 = true;
		player4 = true;
	}
	public int GetTotalCharacters()
	{
		int total = 0;
		if (player1) total++;
		if (player2) total++;
		if (player3) total++;
		if (player4) total++;
		return total;
	}
	public void AddNewCharacter(int playerID)
	{
		switch (playerID)
		{
		case 0: player1 = true; break;
		case 1: player2 = true; break;
		case 2: player3 = true; break;
		case 3: player4 = true; break;
		}
	}
    public int GetScore(int playerID)
    {
        switch (playerID)
        {
            case 0: return score_player1;
            case 1: return score_player2;
            case 2: return score_player3;
            default: return score_player4;
        }
    }
	public void PlayerPlayed(int playerID)
	{
		switch (playerID)
		{
		case 0: player1_played = true; break;
		case 1: player2_played = true;break;
		case 2: player3_played = true;break;
		default: player4_played = true;break;
		}
	}

	public int GetTotalScore()
	{
		return score;
	}
	public int GetPositionByScore(int _playerID)
	{
		int myScore = score_player1;
		if (_playerID == 1)
			myScore = score_player2;
		else if (_playerID == 2)
			myScore = score_player3;
		else if (_playerID == 3)
			myScore = score_player4;
		
		int puesto = 1;
		if (myScore < score_player1)
			puesto++;
		if (myScore < score_player2)
			puesto++;
		if (myScore < score_player3)
			puesto++;
		if (myScore < score_player4)
			puesto++;

		return puesto;
	}
	public void OnRefreshPlayersByActiveOnes()
	{
		//pontura: para que nunca en un replay si nadie toco nada haya cero jugadores!
		if (!player1_played && !player2_played && !player3_played && !player4_played)
			return;
		
		if (player1 && !player1_played)
			player1 = false;
		if (player2 && !player2_played)
			player2 = false;
		if (player3 && !player3_played)
			player3 = false;
		if (player4 && !player4_played)
			player4 = false;		

		player1_played = false;
		player2_played = false;
		player3_played = false;
		player4_played = false;
	}
	public void ResetAll()
	{
		player1 = false;
		player2 = false;
		player3 = false;
		player4 = false;		
		
		player1_played = false;
		player2_played = false;
		player3_played = false;
		player4_played = false;

		OnResetScores ();
	}
	void OnScoreOn(int playerID, Vector3 pos, int points, ScoresManager.types type)
	{
		
		if (Game.Instance.level.charactersManager.getTotalCharacters () == 1)
			points *= 2;
		
		if (NextScoreToWinCredit < score) {
			creditsWon ++;
			SetNextScoreToWinCredit ();
			Data.Instance.events.AddNewCredit ();
		}
		score += points;

		switch (playerID)
		{
		case 0: score_player1 += points; break;
		case 1: score_player2 += points;  break;
		case 2: score_player3 += points;  break;
		case 3: score_player4 += points;  break;
		}
		string desc = type.ToString ().ToLower ();
		Data.Instance.events.OnDrawScore (points, desc);
	}
	void OnMissionComplete(int id)
	{
		int scoreForWinningMission = 500;

		if (player1) { OnScoreOn (0, Vector3.zero, scoreForWinningMission, ScoresManager.types.MISSION_COMPLETED);  }
		if (player2) { OnScoreOn (1, Vector3.zero, scoreForWinningMission, ScoresManager.types.MISSION_COMPLETED);  }
		if (player3) { OnScoreOn (2, Vector3.zero, scoreForWinningMission, ScoresManager.types.MISSION_COMPLETED);  }
		if (player4) { OnScoreOn (3, Vector3.zero, scoreForWinningMission, ScoresManager.types.MISSION_COMPLETED);  }

	}
	void SetNextScoreToWinCredit()
	{
		NextScoreToWinCredit = ((creditsWon+1)*ScoreToWinCredit);

		if(creditsWon>0)
			NextScoreToWinCredit += NextScoreToWinCredit / 2;
		
		//print ("NextScoreToWinCredit " + NextScoreToWinCredit);
	}
}
