using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public string URL = "http://madrollers.com/game/";
    public string setUserURL = "setUser.php";
    public string setUserURLUpload = "updateUser.php";
    public string imageURLUploader = "uploadPhoto.php";
    public string setUserDataURL = "setUserData.php";
    public string imagesURL = "users/";

    const string PREFAB_PATH = "UserData";
    static UserData mInstance = null;
    public string userID;
    public string username;
    [SerializeField] private int score;
    [SerializeField] private int lastScoreWon; //solo para hacer la animacion en el levelSelector

    public string path;
    public HiscoresByMissions hiscoresByMissions;
    public AvatarImages avatarImages;
    public ServerConnect serverConnect;
    public int playerID;

    public bool logged;
    public bool onlyLocal;

    public int missionUnblockedID_1;
    public int missionUnblockedID_2;
    public int missionUnblockedID_3;

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

#if UNITY_EDITOR
        path = Application.persistentDataPath + "/";
#else
        path = Application.persistentDataPath + "/";
#endif
      
        serverConnect = GetComponent<ServerConnect>();
        avatarImages = GetComponent<AvatarImages>();
        hiscoresByMissions = GetComponent<HiscoresByMissions>();
    }
    private void Start()
    {
        Invoke("Delayed", 0.1f);        
    }
    private void Delayed()
    {
        if (Data.Instance.playMode != Data.PlayModes.PARTYMODE)
        {
            missionUnblockedID_1 = PlayerPrefs.GetInt("missionUnblockedID_1", 0);
            missionUnblockedID_2 = PlayerPrefs.GetInt("missionUnblockedID_2", 0);
            missionUnblockedID_3 = PlayerPrefs.GetInt("missionUnblockedID_3", 0);
            score = PlayerPrefs.GetInt("score");
            LoadUser();
        }

    }
    private void OnDestroy()
    {
        Data.Instance.events.OnSaveScore -= OnSaveScore;
    }
    void OnSaveScore()
    {
        print("OnSaveScore: " + Data.Instance.multiplayerData.score);
        if (Data.Instance.multiplayerData.score == 0)
            return;
        lastScoreWon = Data.Instance.multiplayerData.score;
        score += lastScoreWon;
        PlayerPrefs.SetInt("score", score);
        SaveUserDataToServer();
    }
    void LoadUser()
    {
        playerID = PlayerPrefs.GetInt("playerID");
        userID = PlayerPrefs.GetString("userID");
        
        if (userID.Length<2)
        {
#if UNITY_ANDROID || UNITY_IOS
            userID = SystemInfo.deviceUniqueIdentifier;
			SetUserID(userID);            
#else
            userID = SetRandomID();
            SetUserID(userID);
#endif
            logged = false;
        } else
        {
            logged = true;
            userID = PlayerPrefs.GetString("userID");
            username = PlayerPrefs.GetString("username");
           // avatarImages.GetImageFor(userID, null);
        }
        // serverConnect.LoadUserData(userID, OnLoaded);
        OnLoaded(null);
    }
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
    void OnLoaded(ServerConnect.UserDataInServer dataLoaded)
    {
        hiscoresByMissions.Init();
        if (Data.Instance.playMode == Data.PlayModes.STORYMODE || Data.Instance.playMode == Data.PlayModes.SURVIVAL)
            Data.Instance.events.OnSaveScore += OnSaveScore;

        score = PlayerPrefs.GetInt("score");
        missionUnblockedID_1 = PlayerPrefs.GetInt("missionUnblockedID_1");
        missionUnblockedID_2 = PlayerPrefs.GetInt("missionUnblockedID_2");
        missionUnblockedID_3 = PlayerPrefs.GetInt("missionUnblockedID_3");

        return;

        if (dataLoaded != null && dataLoaded.username != "")
        {
            logged = true;
            userID = dataLoaded.userID;
            username = dataLoaded.username;
            score = dataLoaded.score;
            missionUnblockedID_1 = dataLoaded.missionUnblockedID_1;
            missionUnblockedID_2 = dataLoaded.missionUnblockedID_2;
            missionUnblockedID_3 = dataLoaded.missionUnblockedID_3;
           
            onlyLocal = false;
        }
        else
            onlyLocal = true;

        print("User data loaded: " + userID + "  username: " + username + "  logged: " + logged + "  onlyLocal: " + onlyLocal);
    }
    public bool IsOnlyLocal()
    {
        if (logged)
            return false;
        else if (onlyLocal)
            return true;
        else
            return false;
    }
    public bool IsLogged()
    {
        if (logged)
            return true;
        else
            return false;
    }
    public void SetUserID(string userID)
    {
        this.userID = userID;
        PlayerPrefs.SetString("userID", userID);
    }

    public void UserCreation()
    {
        logged = true;
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("userID", userID);
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
    public void SetMissionReady(int videogameID, int missionID)
    {
        print("SetMissionReady videogameID" + videogameID + " missionID: " + videogameID);
        if (Data.Instance.playMode != Data.PlayModes.PARTYMODE)
        {
            int id = PlayerPrefs.GetInt("missionUnblockedID_" + videogameID);
            if (id < missionID)
            {
                PlayerPrefs.SetInt("missionUnblockedID_" + videogameID, missionID);
                switch (videogameID)
                {
                    case 1: missionUnblockedID_1 = missionID; break;
                    case 2: missionUnblockedID_2 = missionID; break;
                    case 3: missionUnblockedID_3 = missionID; break;
                }
            }
        }
    }
    public int GetMissionUnblockedByVideogame(int videogameID)
    {
        switch (videogameID)
        {
            case 1: return missionUnblockedID_1;
            case 2: return missionUnblockedID_2;
            default: return missionUnblockedID_3;
        }
    }
    public int Score()
    {
        return score;
    }
    public int ScoreFormated()
    {
        return score;
    }
    public int GetLastScoreWon()
    {
        int a = lastScoreWon;
        lastScoreWon = 0;
        return a;
    }

    public void SaveUserDataToServer()
    {
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.GetInt("missionUnblockedID_1", missionUnblockedID_1);
        PlayerPrefs.GetInt("missionUnblockedID_2", missionUnblockedID_2);
        PlayerPrefs.GetInt("missionUnblockedID_3", missionUnblockedID_3);
        return;
//
       // StartCoroutine(SaveUserDataC());
    }
    //IEnumerator SaveUserDataC()
    //{
    //    string hash = Utils.Md5Sum(UserData.Instance.userID + score + missionUnblockedID_1 + missionUnblockedID_2 + missionUnblockedID_3 + "pontura");
    //    string post_url = URL + setUserDataURL + "?userID=" + WWW.EscapeURL(UserData.Instance.userID) + "&score=" + score
    //        + "&missionUnblockedID_1=" + missionUnblockedID_1
    //        + "&missionUnblockedID_2=" + missionUnblockedID_2
    //        + "&missionUnblockedID_3=" + missionUnblockedID_3
    //        + "&hash=" + hash;

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
}
