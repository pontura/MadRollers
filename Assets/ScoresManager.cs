using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoresManager : MonoBehaviour {

	public static int score_for_destroying_wall = 60;
	public static int score_for_destroying_floor = 50;
	public static int score_for_killing = 120;
	public static int score_for_breaking = 70;
	public static int score_for_boss = 200;

	public types type;
	public enum types
	{
		MISSION_COMPLETED,
		KILL,
		BREAKING,
		DESTROY_FLOOR,
		DESTROY_WALL,
		GRAB_PIXEL,
		BOSS,
		COMBO
	}
}
