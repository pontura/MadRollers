using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class Tracker : MonoBehaviour {

    private int mission_tries = 1;
    private Data data;
    public bool enableTracking;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            }
            else
            {
                Debug.LogError("Firebase no disponible: " + task.Result);
            }
        });
    }
    public void Init()
    {        
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnAvatarDie += OnAvatarDie;
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
    }
    private void StartMultiplayerRace()
    {
        mission_tries = 0;
        int id = Data.Instance.missions.MissionActiveID;
        Firebase.Analytics.FirebaseAnalytics.LogEvent("mission_init", "mission_id", id);
    }
    void OnAvatarDie(CharacterBehavior cb)
    {
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
        Firebase.Analytics.FirebaseAnalytics.LogEvent("mission_complete", "mission_id", id);
    }
    public void WatchAd()
    {
        int id = Data.Instance.missions.MissionActiveID;
        Firebase.Analytics.FirebaseAnalytics.LogEvent("watch_ad", "mission_id", id);
    }
    public void ContinuePaid()
    {
        int id = Data.Instance.missions.MissionActiveID;
        Firebase.Analytics.FirebaseAnalytics.LogEvent("continue_paid", "mission_id", id);
    }
}
