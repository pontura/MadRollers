using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSaver : MonoBehaviour {

	public List<InputSaverData> recordingList;

	void Start () {
  //      Events.OnAvatarShoot += OnAvatarShoot;
		//Events.OnAvatarJump += OnAvatarJump;
		//Events.OnGameOver += OnGameOver;
	}
	public void MoveInX(float posX)
	{
		//InputSaverData isd = new InputSaverData ();
		////isd.direction = value;

		////print("GRABA pos.x: " + posX);
		//isd.posX = posX;
		//AddToRecordingList (isd);
	}
	void OnAvatarShoot(int value)
	{
  //      InputSaverData isd = new InputSaverData ();
		//isd.shoot = true;
		//AddToRecordingList (isd);
	}
	void OnAvatarJump(int id)
	{
  //      InputSaverData isd = new InputSaverData ();
		//isd.jump = true;
		//AddToRecordingList (isd);
	}
	void AddToRecordingList(InputSaverData isd)
	{
  //      isd.distance = Game.Instance.level.charactersManager.distance;
		//recordingList.Add (isd);
	}
	void OnGameOver(bool isTimeOver)
	{
  //      GetComponent<InputSavedAutomaticPlay> ().Reset();
		//GetComponent<InputSavedAutomaticPlay> ().SaveNewList( recordingList );
		//recordingList.Clear ();
	}
}
