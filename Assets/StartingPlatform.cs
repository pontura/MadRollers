using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPlatform : SceneObject {

	//public GameObject[] platforms;
	public SpriteRenderer logo;
	public Player playerToInstantiate;
	public Animation[] containers;
	public List<int> ids;
	public int avatarID;
    public GameObject lights;

	public override void OnRestart(Vector3 pos)
	{
		base.OnRestart( pos );
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;

        int id = 0;
		//foreach (Transform t in containers) {
		//	Player newPlayer = Instantiate (playerToInstantiate, Vector3.zero, Quaternion.identity, t);
		//	newPlayer.transform.localPosition = Vector3.zero;
		//	newPlayer.isPlaying = false;	
		//	newPlayer.id = id;
		//	id++;
		//}
		Data.Instance.events.OnCharacterInit += OnCharacterInit;
//		foreach (GameObject go in platforms) {
//			Vector3 pos = go.transform.localPosition;
//			pos.y = 0.7f;
//			go.transform.localPosition = pos;
//		}
		logo.sprite = Data.Instance.videogamesData.GetActualVideogameData ().intro_logo;
	}
    void StartMultiplayerRace()
    {
        lights.SetActive(false);
    }
    void OnDestroy()
    {
        Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
    }
	public override void OnPool()
	{
        Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
        playerToInstantiate = null;
		Data.Instance.events.OnCharacterInit -= OnCharacterInit;
	}
	void OnCharacterInit(int _avatarID)
	{
		CheckIfStart (_avatarID);
	}
	void CheckIfStart(int _avatarID)
	{		
		foreach (int i in ids) {
			if (i == _avatarID)
				return;
		}
        containers[_avatarID].Play("usbOn");
        //ids.Add (_avatarID);
        //	Destroy (containers [_avatarID].gameObject);
    }
}
