using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersusManager : MonoBehaviour {

	public int team_1_score;
	public int team_2_score;

	public Area[] areas;
	public int id = 0;

	void Start()
	{
		Data.Instance.events.OnVersusTeamWon += OnVersusTeamWon;
	}
	void OnVersusTeamWon(int teamid)
	{
		id++;
		if (id == areas.Length)
			id = 0;
		if (teamid == 1)
			team_1_score++;
		else 
			team_2_score++;
	}
	public void ResetScores()
	{
		team_1_score = 0;
		team_2_score = 0;
	}
	public Area GetArea()
	{
		return areas[id];
	}
}
