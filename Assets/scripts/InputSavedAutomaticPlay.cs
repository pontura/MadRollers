using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputSavedAutomaticPlay : MonoBehaviour {

	public List<SavedData> allPlayersSavedData;

	[Serializable]
	public class SavedData
	{
		public List<InputSaverData> data;
		public int lastIdDataUsed;
		public float lastDirection;
	}
	public  List<CharacterBehavior> characters;

	CharactersManager charactersManager;
	bool isPlaying;
	int id = 1;

	public void Init(CharactersManager charactersManager) {
		//if (Data.Instance.playMode == Data.PlayModes.PARTYMODE)
			return;


		this.charactersManager = charactersManager;
		foreach (SavedData s in allPlayersSavedData) {
			AddCharacter ();
			Invoke ("Delayed", 0.25f);
		}
	}
	void Delayed()
	{
		isPlaying = true;
	}

	void AddCharacter()
	{		
		characters.Add( charactersManager.AddAutomaticPlayer (id));
		id++;
	}

	//InputSaverData lastSavedDataUsed;
	//void Update () {
		
	//	if (Data.Instance.playMode == Data.PlayModes.PARTYMODE)
	//		return;
	//	if (allPlayersSavedData.Count == 0 || !isPlaying || characters.Count==0)
	//		return;

	//	foreach(CharacterBehavior cb in characters)
	//	{
	//		if (cb.state == CharacterBehavior.states.DEAD ||
	//		    cb.state == CharacterBehavior.states.FALL ||
	//		    cb.state == CharacterBehavior.states.CRASH) 
	//		{
	//			//is dead;
	//		} else if(cb.player != null){
	//			UpdatePlayer (cb);
	//		}
	//	}
	//}
	void UpdatePlayer (CharacterBehavior cb) {

		int playerID = cb.player.id;
		SavedData savedData = allPlayersSavedData [playerID-1];

		if (savedData.lastIdDataUsed >= savedData.data.Count)
			return;
		
		InputSaverData isdata = savedData.data [savedData.lastIdDataUsed];

		InputSaverData nextdata = null;
		if(savedData.data.Count > savedData.lastIdDataUsed+1)
			nextdata = savedData.data [savedData.lastIdDataUsed+1];
		
		if (charactersManager.distance >= isdata.distance) {
			savedData.lastIdDataUsed++;
			if (isdata.shoot) {
				//shoot
				cb.shooter.CheckFire();
			} else if (isdata.jump) {
				//jump
				cb.Jump();
			} else {
				//move
				//savedData.lastDirection = isdata.direction;
//				Vector3 pos = cb.transform.position;
//				pos.x = isdata.posX;
//				cb.transform.position = pos;
			}
			//savedData.data.RemoveAt (0);
		}
		if (nextdata != null) {
			float moveX = 0.5f;
			if(isdata.posX > nextdata.posX)
				moveX *= -1;
			if (isdata.posX < nextdata.posX)
				moveX *= 1;
			else
				return;
			cb.controls.MoveInX (moveX);
		}
	}
	public void SaveNewList(List<InputSaverData> newList)
	{
        return;


		if (allPlayersSavedData.Count > 2)
			allPlayersSavedData.RemoveAt (0);
		//allPlayersSavedData.Clear ();

		SavedData newSavedData = new SavedData ();
		newSavedData.data = new List<InputSaverData> ();

		foreach (InputSaverData data in newList) {
			InputSaverData newData = data;
			newSavedData.data.Add (newData);
		}

		allPlayersSavedData.Add (newSavedData);
	}
	public void Reset()
	{
        return;


		foreach (SavedData data in allPlayersSavedData) {
			data.lastDirection = 0;
			data.lastIdDataUsed = 0;
		}
		characters.Clear();
		id = 1;
	}
	public void RemoveAllData()
	{
		Reset ();
		allPlayersSavedData.Clear ();
		id = 1;
	}
}
