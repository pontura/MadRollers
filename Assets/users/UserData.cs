using Firebase.Auth;
using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Yaguar.Auth;

public class UserData : MonoBehaviour
{

    public string test_username = "MAD-ROLLER";
    public string test_email = "test@gmail.com";
    public string test_password = "1234567890";
    public string test_userID = "DMZgakyMpdTm8qTECRdgllItjJQ2";
    
    public bool IsTester()
    {
#if UNITY_EDITOR
        return false;
#endif
        return data.userID == test_userID;
    }

    string assetBundles = "https://pontura.github.io/madrollers/";
    public string URL_assetBundles { get { return assetBundles; } }

    const string PREFAB_PATH = "UserData";
    static UserData mInstance = null;
    public ServerConnect.UserDataInServer data;

    public string userID { get { return data.userID;  } }
    public string username { get { return data.username; } }

    [SerializeField] private int lastScoreWon; //solo para hacer la animacion en el levelSelector

    public string path;
    public HiscoresByMissions hiscoresByMissions;
    public AvatarImages avatarImages;
    public int playerID; // Mad-Roller muñeco id
    private bool allDone;
    const string missionUnlocked = "missionUnlocked";

   
    public bool IsReadyToInit() //if its logged or new in the game:
    {
        return allDone;
    }

    public static UserData Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<UserData>();
            }
            return mInstance;
        }
    }
    void Awake()
    {
        if (!mInstance)
            mInstance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        path = Application.persistentDataPath + "/";
      
       // serverConnect = GetComponent<ServerConnect>();
        avatarImages = GetComponent<AvatarImages>();
        hiscoresByMissions = GetComponent<HiscoresByMissions>();
    }
    private void Start()
    {
        hiscoresByMissions.Init();
        Events.OnSaveScore += OnSaveScore;
        Events.OnPayPixeles += OnPayPixeles;
        FirebaseAuthManager.Instance.OnFirebaseAuthenticated += OnFirebaseAuthenticated;
    }
    void OnFirebaseAuthenticated(string username, string email, string uid)
    {
        if(username == "" && email == "" && uid == "")
        {
            Debug.Log("Enter as TESTER");
            uid = UserData.Instance.test_userID;
            username = UserData.Instance.test_username;
        }
        Debug.Log("USERDATA OnFirebaseAuthenticated " + username + " email" + email + " uid: " + uid);
        playerID = PlayerPrefs.GetInt("playerID");

        data.userID = uid;
        data.username = username;

        allDone = true;

        if (!IsTester())
        {
            GetLevelsPlayedCount();
            _ = GetUserData();
        }
    }
    public async Task<int?> GetUserData()
    {
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var db = FirebaseFirestore.DefaultInstance;

        DocumentReference userRef = db.Collection("users").Document(userId);
        DocumentSnapshot snapshot = await userRef.GetSnapshotAsync();

        if (snapshot.Exists && snapshot.ContainsField("username"))
        {
            data.username = snapshot.GetValue<string>("username");
            Debug.Log($"📥 Nombre del usuario: {data.username}");
        }
        else
        {
            Debug.Log("❌ No se encontró username para este usuario.");
        }

        if (snapshot.Exists && snapshot.ContainsField("score"))
        {
            data.score = snapshot.GetValue<int>("score");
            Debug.Log($"📥 score del usuario: {data.score}");
            return data.score;
        }
        else
        {
            Debug.Log("❌ No se encontró score para este usuario.");
            return null;
        }
       
    }
    private void OnDestroy()
    {
        Events.OnSaveScore -= OnSaveScore;
        Events.OnPayPixeles -= OnPayPixeles;
        FirebaseAuthManager.Instance.OnFirebaseAuthenticated -= OnFirebaseAuthenticated;
    }
    public void UpdateUserName(string newName, System.Action<bool, string> OnDone)
    {
        if(IsTester())
        {
            OnDone(false, "You are not logged in");
            return;
        }
        print("new name: " + newName + " old name: " + username);
        if (newName != username)
            TrySetUsername(newName, OnDone);
        else
            OnDone(false, "No username changed");
    }

    private async void TrySetUsername(string newName, System.Action<bool, string> OnDone)
    {
        // 1️⃣ Verificar si ya existe ese username
        bool exists = await CheckIfUsernameExists(newName);

        if (exists)
        {
            Debug.LogWarning("❌ Ese nombre de usuario ya está en uso.");
            if (OnDone != null) OnDone(false, "❌ Ese nombre de usuario ya está en uso.");
            return;
        }

        _ = UpdateName(newName, OnDone);
        Debug.Log("✅ Username actualizado correctamente.");
    }

    private async Task<bool> CheckIfUsernameExists(string username)
    {
        var db = FirebaseFirestore.DefaultInstance;
        Query query = db.Collection("users").WhereEqualTo("username", username);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        return snapshot.Count > 0; // si hay resultados, el username ya existe
    }


    public async Task UpdateName(string newName, System.Action<bool, string> OnDone)
    {
        string userId = UserData.Instance.userID;
        var db = FirebaseFirestore.DefaultInstance;

        DocumentReference userRef = db.Collection("users").Document(userId);

        Dictionary<string, object> d = new Dictionary<string, object>
        {
            { "username", newName },
            { "updatedAt", Timestamp.GetCurrentTimestamp() }
        };

        await userRef.SetAsync(d, SetOptions.MergeAll);
        data.username = newName;
        if (OnDone != null) OnDone(true, "");
        Debug.Log($"✅ Name actualizado a {newName}");
        int mission = GetMissionUnlocked();
        Debug.Log($"✅ missions total: {mission}");
        if (mission>0)
        {
            for (int a = 0; a <= mission; a++)
            {
                _ = hiscoresByMissions.UpdateUserName(a, userId, newName);
            }
        }
        _ = hiscoresByMissions.UpdateUserName(MissionsManager.Instance.MissionTorneo, userId, newName);
    }
    public bool CanPay(int price)
    {
        return (data.score >= price);
    }
    void OnPayPixeles(int pay)
    {
        data.score -= pay;
        if (data.score < 0) data.score = 0;
        SaveUserDataToServer(null);
    }
    void OnSaveScore()
    {
        if (Data.Instance.multiplayerData.score == 0)
            return;
        Debug.Log("OnSaveScore: " + data.score + " + " + Data.Instance.multiplayerData.score);
        lastScoreWon = Data.Instance.multiplayerData.score;
        data.score += lastScoreWon;
        SaveUserDataToServer(null);
    }
    public void UseLocalData()
    {
        allDone = true;

        data.missionUnlocked = PlayerPrefs.GetInt(missionUnlocked);
        data.score = PlayerPrefs.GetInt("score");
    }
    public void UserCreation()
    {
        PlayerPrefs.SetString("username", data.username);
        PlayerPrefs.SetString("userID", data.userID);
    }
    private Sprite LoadSprite(string path)
    {
        return null;
    }
    public void SetMissionReady(int missionID)
    {
        print("SetMissionReady missionID: " + missionID);
        if (Data.Instance.playMode != Data.PlayModes.PARTYMODE)
        {
            int id = PlayerPrefs.GetInt(missionUnlocked);
            if (id < missionID)
                SetNewUnlockedMission(missionID);
        }
    }
    public void SetNewUnlockedMission(int missionID)
    {
        PlayerPrefs.SetInt(missionUnlocked, missionID);
        data.missionUnlocked = missionID;
    }
    public int GetMissionUnlocked()
    {
        return data.missionUnlocked;
    }
    private async void GetLevelsPlayedCount()
    {
        data.missionUnlocked = await hiscoresByMissions.GetLevelsPlayedCount();
        Debug.Log("Niveles jugados: " + data.missionUnlocked);
        hiscoresByMissions.LoadHiscore(MissionsManager.Instance.MissionTorneo, OnTorneoHiscoreLoaded);
    }
    void OnTorneoHiscoreLoaded(HiscoresByMissions.MissionHiscoreData data)
    {
        Debug.Log("Torneo hiscore cargado RANK: " + hiscoresByMissions.torneoRank);
    }
    public int Score()
    {
        return data.score;
    }
    public int GetLastScoreWon()
    {
        int a = lastScoreWon;
        lastScoreWon = 0;
        return a;
    }

    public void SaveUserDataToServer(System.Action OnDone)
    {
        if(IsTester())
        {
            Debug.Log("Tester user doesnt save data to server!");
            if(OnDone != null)
                OnDone(); 
        } else
            _ = UpdateTotalScore(data.score, OnDone);
    }

    public async Task UpdateTotalScore(int score, System.Action OnDone)
    {
        string userId = UserData.Instance.userID;
        var db = FirebaseFirestore.DefaultInstance;

        DocumentReference userRef = db.Collection("users").Document(userId);

        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "score", score },
            { "updatedAt", Timestamp.GetCurrentTimestamp() }
        };

        await userRef.SetAsync(data, SetOptions.MergeAll);
        if (OnDone != null) OnDone();
        Debug.Log($"✅ score actualizado a {score}");
    }

}
