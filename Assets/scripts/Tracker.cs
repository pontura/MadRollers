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
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnAvatarDie += OnAvatarDie;
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
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
        FirebaseAnalytics.LogEvent("mission_init", "mission_id", id);
    }
    void OnAvatarDie(CharacterBehavior cb)
    {
        if (!FirebaseOn()) return;
        int id = Data.Instance.missions.MissionActiveID;
        FirebaseAnalytics.LogEvent("die",
            new Parameter[] {
                new Parameter("mission_id", id),
                new Parameter("mission_tries", mission_tries)
            }
        );
        mission_tries++;
    }
    void OnMissionComplete(int id)
    {
        if (!FirebaseOn()) return;
        FirebaseAnalytics.LogEvent("mission_complete", "mission_id", id);
    }
    public void WatchAd()
    {
        if (!FirebaseOn()) return;
        int id = Data.Instance.missions.MissionActiveID;
        FirebaseAnalytics.LogEvent("watch_ad", "mission_id", id);
    }
    public void ContinuePaid()
    {
        if (!FirebaseOn()) return;
        int id = Data.Instance.missions.MissionActiveID;
        FirebaseAnalytics.LogEvent("continue_paid", "mission_id", id);
    }
}
