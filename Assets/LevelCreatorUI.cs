#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(LevelCreator))]
public class LevelCreatorUI : Editor {

	string[] videogames = new string[]{"Boy-Land", "Gallax", "Inferno"};
	string[] videogame1;
	string[] videogame2;
	string[] videogame3;

	int _choiceIndex = 0;
	int _videogameIndex = 0;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();
		LevelCreator levelCreator = (LevelCreator)target;
        _videogameIndex = levelCreator.videoGameID - 1;
		_choiceIndex = levelCreator.missionID;

		string[] videogameData;
		int vid = _videogameIndex;

		videogameData = new string[levelCreator.missions.videogames[vid].missions.Count];
		int id = 0;
		foreach (Missions.MissionsData c in levelCreator.missions.videogames[vid].missions) {
			videogameData [id] = id + "-" + c.data[0].title + " - " + c.data[0].jsonName;
			id++;
		}

		levelCreator.missionID = _choiceIndex;
		levelCreator.videoGameID = _videogameIndex+1;

	

		EditorUtility.SetDirty (target);

		if(GUILayout.Button("Load Area"))
		{
			levelCreator.LoadArea ();
		}
		GUILayout.Space (20);

		_videogameIndex = EditorGUILayout.Popup (_videogameIndex, videogames);
		_choiceIndex = EditorGUILayout.Popup (_choiceIndex, videogameData);


		if (GUILayout.Button ("Load Mission")) {
			levelCreator.LoadMissions (); 
		}

		GUILayout.Space (20);

		if(GUILayout.Button("Update Missions from json"))
		{
			LevelCreator t = (LevelCreator)target;
			t.UpdateMissions ();
		}
		if(GUILayout.Button("Clear"))
		{
			LevelCreator t = (LevelCreator)target;
			t.Clear ();
		}
		if(GUILayout.Button("Save"))
		{
			LevelCreator t = (LevelCreator)target;
			t.SaveArea ();
		}

		//		if(GUILayout.Button("Show Mission"))
		//		{			
		//			LevelCreator levelCreator = (LevelCreator)target;
		//			levelCreator.ResetAreas ();
		//			MissionData mission = levelCreator.GetMission ();
		////			foreach (AreaSet areaSet in mission.GetComponent<AreasManager> ().areaSets) {
		////				foreach (Area area in areaSet.areas) {
		////					levelCreator.AddArea (area.gameObject, area.z_length);
		////				}
		////			}
		//		}
	}
}
#endif



