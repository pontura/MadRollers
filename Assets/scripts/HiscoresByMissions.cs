using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Firebase;
using Firebase.Firestore;
using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class HiscoresByMissions : MonoBehaviour
{
    FirebaseFirestore db;
    FirebaseAuth auth;

    public bool loaded;

    public List<MissionHiscoreData> all;

    [Serializable]
    public class MissionHiscoreData
    {
        public int mission;
        public List<MissionHiscoreUserData> all;
    }
    [Serializable]
    public class MissionHiscoreUserData
    {
        [HideInInspector]
        public int mission;
        public string userID;
        public string username;
        public int score;
    }
    public void Init()
    {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        if (Data.Instance.playMode == Data.PlayModes.STORYMODE)
            Data.Instance.events.OnMissionComplete += OnMissionComplete;
    }
    public void ResetAllHiscores()
    {
        all.Clear();
    }
    private void OnDestroy()
    {
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
    }    
    public void SaveSurvivalScore()
    {
        Save(0, Data.Instance.multiplayerData.score);
    }
    void OnMissionComplete(int missionID)
    {
        Save(missionID, Data.Instance.multiplayerData.score);
        Invoke("Delayed", 1);
    }
    void Delayed()
    {
        Data.Instance.multiplayerData.score = 0;
    }
    public void CheckToAddNewHiscore(string userID, int score, int videogame, int mission)
    {
        Save(mission, score);
    }
    public void LoadHiscore(int mission, System.Action<MissionHiscoreData> OnDone)
    {
        LoadHiscoreC(mission, OnDone);
    }

    private async void LoadHiscoreC(int mission, System.Action<MissionHiscoreData> OnDone)
    {
        var topScores = await GetTopScores(mission, 50);
        MissionHiscoreData m = new MissionHiscoreData();
        m.all = new List<MissionHiscoreUserData>();
        foreach (var entry in topScores)
        {
            MissionHiscoreUserData data = new MissionHiscoreUserData
            {
                username = entry.username,
                score = entry.score,
                mission = mission
            };
            m.all.Add(data);
            Debug.Log($"{entry.username}: {entry.score}");
        }
        OnDone(m);
    }
   
    public async Task<List<(string username, int score)>> GetTopScores(int level, int limit = 50)
    {
        var db = FirebaseFirestore.DefaultInstance;

        Query query = db
            .Collection("leaderboards")
            .Document("level_" + level)
            .Collection("scores")
            .OrderByDescending("score")
            .Limit(limit);

        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        List<(string username, int score)> topList = new List<(string, int)>();

        foreach (var doc in snapshot.Documents)
        {
            string username = doc.ContainsField("username") ? doc.GetValue<string>("username") : "Anon";
            int score = doc.GetValue<int>("score");
            topList.Add((username, score));
        }

        return topList;
    }
    public async Task<int> GetLevelsPlayedCount()
    {
        string userId = UserData.Instance.userID;
        var db = FirebaseFirestore.DefaultInstance;

        CollectionReference scoresRef = db
            .Collection("users")
            .Document(userId)
            .Collection("scores");

        QuerySnapshot snapshot = await scoresRef.GetSnapshotAsync();

        int count = snapshot.Count;
        Debug.Log("🎮 El usuario " + userId  + " jugó " + count + " niveles.");
        return count;
    }
    //public async Task GetScore(int levelNumber, System.Action<MissionHiscoreData> OnDone)
    //{
    //    if (FirebaseAuth.DefaultInstance.CurrentUser == null)
    //    {
    //        Debug.LogError("⚠️ El usuario no está autenticado.");
    //        return;
    //    }
    //    string userId = UserData.Instance.userID;
    //    var db = FirebaseFirestore.DefaultInstance;

    //    DocumentReference docRef = db
    //        .Collection("users")
    //        .Document(userId)
    //        .Collection("scores")
    //        .Document("level_" + levelNumber);

    //    try
    //    {
    //        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

    //        if (snapshot.Exists && snapshot.ContainsField("score"))
    //        {
    //            int score = snapshot.GetValue<int>("score");
    //            Debug.Log($"📥 Score del nivel {levelNumber}: {score}");
    //            OnDone(null);
    //        }
    //        else
    //        {
    //            Debug.Log($"❌ No hay score guardado para nivel {levelNumber}.");
    //            OnDone(null);
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError($"Error al obtener el score del nivel {levelNumber}: {e.Message}");
    //        OnDone(null);
    //    }
        
    //}
   
    public void Save(int mission, int score)
    {
        _ = SaveScore(mission, score);       
    }
    public async Task SaveScore(int levelNumber, int score)
    {
        string userId = UserData.Instance.userID;
        var db = FirebaseFirestore.DefaultInstance;

        DocumentReference docRef = db
            .Collection("users")
            .Document(userId)
            .Collection("scores")
            .Document("level_" + levelNumber);

        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        bool shouldUpdate = false;

        if (!snapshot.Exists)
        {
            // No hay score previo → grabar
            shouldUpdate = true;
        }
        else
        {
            int previousScore = snapshot.GetValue<int>("score");
            if (score > previousScore)
            {
                // El nuevo score es mayor → actualizar
                shouldUpdate = true;
            }
        }

        if (shouldUpdate)
        {
            Dictionary<string, object> scoreData = new Dictionary<string, object>
            {
                { "score", score },
                { "timestamp", Timestamp.GetCurrentTimestamp() }
            };

            await docRef.SetAsync(scoreData);
            Debug.Log($"✅ Nuevo highscore guardado: nivel {levelNumber} → {score}");
            _ = SaveLeaderboardScore(levelNumber, score, UserData.Instance.userID, UserData.Instance.username);
        }
        else
        {
            Debug.Log($"🔁 No se guardó: el score actual ({score}) no supera el anterior.");
        }
    }
    public async Task SaveLeaderboardScore(int levelNumber, int score, string userID, string username)
    {
        var db = FirebaseFirestore.DefaultInstance;

        DocumentReference docRef = db
            .Collection("leaderboards")
            .Document("level_" + levelNumber)
            .Collection("scores")
            .Document(userID);

        // Traer el score actual del usuario (si existe)
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        bool update = true;

        if (snapshot.Exists)
        {
            int previousScore = snapshot.GetValue<int>("score");
            if (score <= previousScore)
            {
                update = false; // No lo supera, no actualizamos
            }
        }

        if (update)
        {
            Dictionary<string, object> scoreData = new Dictionary<string, object>
        {
            { "score", score },
            { "timestamp", Timestamp.GetCurrentTimestamp() },
            { "username", username },
            { "userID", userID }
        };

            await docRef.SetAsync(scoreData);
            Debug.Log($"🏆 Nuevo highscore para level {levelNumber}: {score}");
        }
    }
}
