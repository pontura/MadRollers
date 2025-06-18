using Firebase.Auth;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Yaguar.Auth;

public class UserData : MonoBehaviour
{
    string assetBundles = "https://pontura.github.io/madrollers/";
    // string url = "https://yaguar.xyz/madRollers/";
    //string url = "https://dev.yaguar.xyz/madRollers/";
   // public string URL { get { return url; } }
    public string URL_assetBundles { get { return assetBundles; } }
    //public string setUserURL = "setUser.php";
    //public string setUserURLUpload = "updateUser.php";
    //public string imageURLUploader = "uploadPhoto.php";
    //public string setUserDataURL = "setUserData.php";
    //public string imagesURL = "users/";

    const string PREFAB_PATH = "UserData";
    static UserData mInstance = null;
   // public string userID;
   // public string username;
    public ServerConnect.UserDataInServer data;

    public string userID { get { return data.userID;  } }
    public string username { get { return data.username; } }


    [SerializeField] private int lastScoreWon; //solo para hacer la animacion en el levelSelector

    public string path;
    public HiscoresByMissions hiscoresByMissions;
    public AvatarImages avatarImages;
   // public ServerConnect serverConnect;
    public int playerID; // Mad-Roller muñeco id
    private bool allDone;

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
        FirebaseAuthManager.Instance.OnFirebaseAuthenticated += OnFirebaseAuthenticated;
    }
    void OnFirebaseAuthenticated(string username, string email, string uid)
    {
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
        FirebaseAuthManager.Instance.OnFirebaseAuthenticated -= OnFirebaseAuthenticated;
    }
    void OnSaveScore()
    {
        print("OnSaveScore: " + Data.Instance.multiplayerData.score);
        if (Data.Instance.multiplayerData.score == 0)
            return;
        lastScoreWon = Data.Instance.multiplayerData.score;
        data.score += lastScoreWon;
        SaveUserDataToServer();
    }
    
    //public bool IsRegistered()
    //{
    //    return PlayerPrefs.GetString("username") != "";
    //}
    string SetRandomID()
    {
        string value = "";
#if UNITY_WEBGL
        value += "web_";
#else
        value += "exe_";
#endif

        for (int a= 0; a<20; a++)
        {
            value += Random.Range(0, 9).ToString();
        }
        return value;
    }
    public void UseLocalData()
    {
        allDone = true;

        //data.username = PlayerPrefs.GetString("username");
        //data.userID = PlayerPrefs.GetString("userID");

        //if (data.userID == "") data.userID = SystemInfo.deviceUniqueIdentifier;
        //if (data.username == "")  data.username = "MR (" + Random.Range(100, 10000) + ")";

        data.missionUnblocked = PlayerPrefs.GetInt("missionUnblocked");
        data.score = PlayerPrefs.GetInt("score");
    }
    //void OnLoaded(ServerConnect.UserDataInServer data)
    //{
    //    allDone = true;
    //    if (data != null)
    //    {
    //        this.data = data;
    //        data.missionUnblocked = PlayerPrefs.GetInt("missionUnblocked");
    //        Debug.Log("UserData OnLoaded . Login done!  username: " + data.username + " userID: " + data.userID);
    //    }
    //    else
    //        Debug.LogError("No user!");
    //}
    public void UserCreation()
    {
        PlayerPrefs.SetString("username", data.username);
        PlayerPrefs.SetString("userID", data.userID);
    }
    private Sprite LoadSprite(string path)
    {
        //Debug.Log("Busca imagen en: " + path);
        //if (string.IsNullOrEmpty(path)) return null;
        //if (System.IO.File.Exists(path))
        //{
        //    Debug.Log("Image exists in local");
        //    byte[] bytes = System.IO.File.ReadAllBytes(path);
        //    Texture2D texture = new Texture2D(300, 300);
        //    texture.LoadImage(bytes);
        //    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //    return sprite;
        //}
        return null;
    }
    public void UpdateData()
    {
        print("UpdateData");
      //  Data.Instance.serverManager.LoadUserData(userID);
    }
    public void SetMissionReady(int missionID)
    {
        print("SetMissionReady missionID: " + missionID);
        if (Data.Instance.playMode != Data.PlayModes.PARTYMODE)
        {
            int id = PlayerPrefs.GetInt("missionUnblocked");
            if (id < missionID)
            {
                PlayerPrefs.SetInt("missionUnblocked", missionID);
                data.missionUnblocked = missionID;
            }
        }
    }
    public int GetMissionUnblocked()
    {
        return data.missionUnblocked;
    }
    private async void GetLevelsPlayedCount()
    {
        data.missionUnblocked = await hiscoresByMissions.GetLevelsPlayedCount();
        Debug.Log("Niveles jugados: " + data.missionUnblocked);
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
        //StartCoroutine(SaveUserDataC());
    }
    //IEnumerator SaveUserDataC()
    //{
    //    string hash = Utils.Md5Sum(UserData.Instance.data.userID + data.score + data.missionUnblocked + "pontura");
    //    string post_url = URL + setUserDataURL + "?userID=" + WWW.EscapeURL(UserData.Instance.data.userID) + "&score=" + data.score
    //        + "&missionUnblocked=" + data.missionUnblocked
    //        + "&score=" + data.score
    //        + "&hash=" + hash;

    //    PlayerPrefs.SetInt("missionUnblocked", data.missionUnblocked);
    //    PlayerPrefs.SetInt("score", data.score);

    //    print("grabe: " + post_url);

    //    WWW www = new WWW(post_url);
    //    yield return www;

    //    if (www.error != null)
    //    {
    //        //UsersEvents.OnPopup("There was an error: " + www.error);
    //    }
    //    else
    //    {
    //        string result = www.text;
    //        if (result == "exists")
    //        {
    //            UsersEvents.OnPopup("ya existe");
    //        }
    //        else
    //        {
    //            Debug.Log("UserData updated " + post_url);
    //        }
    //    }
    //}
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
