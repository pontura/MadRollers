using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListenerDispatcher : MonoBehaviour {

	public myEnum message;

    Data data;
    bool ready;

    void Start()
    {
        data = Data.Instance;
    }
    public enum myEnum // your custom enumeration
    {
        ShowMissionId,
        ShowMissionName,
        LevelFinish,
        LevelFinish_easy,
        LevelFinish_medium,
        LevelFinish_hard,
        LevelTransition,
        Ralenta,
        BonusEntrande,
		LevelChangeRight,
		LevelChangeLeft,
		LevelChangeCenter
    }
	
	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player")
		{
			if (!ready)
				data.events.ListenerDispatcher(message);
			ready = true;		
		}
        
	}
    void OnDisable()
    {
        Reset();
    }
    void Reset()
    {
        ready = false;
    }

}

