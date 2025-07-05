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
        Data.Instance.events.OnSaveScore += OnSaveScore;
        Data.Instance.events.OnPayPixeles += OnPayPixeles;
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
        GetLevelsPlayedCount();
        _ =  GetScore();
    }
    private void OnDestroy()
    {
        Data.Instance.events.OnSaveScore -= OnSaveScore;
        Data.Instance.events.OnPayPixeles -= OnPayPixeles;
        FirebaseAuthManager.Instance.OnFirebaseAuthenticated -= OnFirebaseAuthenticated;
    }
    public bool CanPay(int price)
    {
        return (data.score >= price);
    }
    void OnPayPixeles(int pay)
    {
        data.score -= pay;
        if (data.score < 0) data.score = 0;
        SaveUserDataToServer();
    }
    void OnSaveScore()
    {
        if (Data.Instance.multiplayerData.score == 0)
            return;
        Debug.Log("OnSaveScore: " + data.score + " + " + Data.Instance.multiplayerData.score);
        lastScoreWon = Data.Instance.multiplayerData.score;
        data.score += lastScoreWon;
        SaveUserDataToServer();
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

    public void SaveUserDataToServer()
    {
        _ = UpdateTotalScore(data.score);
    }
    public async Task<int?> GetScore()
    {
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var db = FirebaseFirestore.DefaultInstance;

        DocumentReference userRef = db.Collection("users").Document(userId);
        DocumentSnapshot snapshot = await userRef.GetSnapshotAsync();

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

    public async Task UpdateTotalScore(int score)
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

        Debug.Log($"✅ score actualizado a {score}");
    }

}
