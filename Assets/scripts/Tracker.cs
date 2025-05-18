using UnityEngine;
using System.Collections;

public class Tracker : MonoBehaviour {

    //private GoogleAnalyticsV3 googleAnalytics;
    private int tries = 1;
    private Data data;
    public bool enableTracking;

    public void Init()
    {
        
      //  Data.Instance.events.OnMissionComplete += OnMissionComplete;
     //   Data.Instance.events.OnAvatarDie += OnAvatarDie;
    }
    void OnAvatarDie(CharacterBehavior cb)
    {
        tries++;
    }
    void OnMissionComplete(int id)
    {
        TrackMission(id, tries);
        tries = 1;
    }
	public void TrackScreen (string SCreenName) 
    {
        if (enableTracking)
        {
           // googleAnalytics.LogScreen(SCreenName);
          //  GA.API.Design.NewEvent(SCreenName);
        }
	}

    public void TrackMission(int levelID,int tries)
    {
        if (enableTracking)
        {
          //  googleAnalytics.LogEvent("Mission", "Mission Complete", "mission " + levelID, tries);
           // GA.API.Design.NewEvent("Tries_Completed_Mission:" + levelID, tries);
        }
    }
}
