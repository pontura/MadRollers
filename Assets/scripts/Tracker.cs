using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class Tracker : MonoBehaviour {

    private int mission_tries = 1;
    private Data data;
    public bool enableTracking;

    private void Start()
    {
        Invoke("Loop", 1);
    }
    private void Loop()
    {
        if (FirebaseOn())
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        else
            Invoke("Loop", 1);
    }
    public void Init()
    {        
        Events.OnMissionComplete += OnMissionComplete;
        Events.OnAvatarDie += OnAvatarDie;
        Events.StartMultiplayerRace += StartMultiplayerRace;
    }
    bool FirebaseOn()
    {
        return (FirebaseApp.DefaultInstance != null);
    }
    private void StartMultiplayerRace()
    {
        if (!FirebaseOn()) return;

        if (FirebaseApp.DefaultInstance == null)
            mission_tries = 0;
        int id = Data.Instance.missions.MissionActiveID;
        FirebaseAnalytics.LogEvent(
           "mission_init",
           new Parameter("mission_init_mission_id", id)
       );
    }
    void OnAvatarDie(CharacterBehavior cb)
    {
        if (!FirebaseOn()) return;
        int id = Data.Instance.missions.MissionActiveID;
        FirebaseAnalytics.LogEvent("die",
            new Parameter[] {
                new Parameter("die_mission_id", id),
                new Parameter("die_mission_tries", mission_tries)
            }
        );
        mission_tries++;
    }
    void OnMissionComplete(int id)
    {
        if (!FirebaseOn()) return;

        FirebaseAnalytics.LogEvent(
            "mission_complete",
            new Parameter("mission_complete_mission_id", id)
        );
    }
    public void WatchAd()
    {
        if (!FirebaseOn()) return;
        int id = Data.Instance.missions.MissionActiveID;
        FirebaseAnalytics.LogEvent("watch_ad",
            new Parameter("watch_ad_mission_id", id)
            );
    }
    public void ContinuePaid()
    {
        if (!FirebaseOn()) return;
        int id = Data.Instance.missions.MissionActiveID;
        FirebaseAnalytics.LogEvent("continue_paid",
           new Parameter("continue_paid_mission_id", id)
           );
    }
}
