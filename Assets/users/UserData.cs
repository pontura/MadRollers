﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    string url = "https://yaguar.xyz/madRollers/";
    public string URL { get { return url; } }
    public string setUserURL = "setUser.php";
    public string setUserURLUpload = "updateUser.php";
    public string imageURLUploader = "uploadPhoto.php";
    public string setUserDataURL = "setUserData.php";
    public string imagesURL = "users/";

    const string PREFAB_PATH = "UserData";
    static UserData mInstance = null;
   // public string userID;
   // public string username;
    public ServerConnect.UserDataInServer data;

    public string userID { get { return data.userID;  } }
    public string username { get { return data.username; } }


    [SerializeField] private int score;
    [SerializeField] private int lastScoreWon; //solo para hacer la animacion en el levelSelector

    public string path;
    public HiscoresByMissions hiscoresByMissions;
    public AvatarImages avatarImages;
    public ServerConnect serverConnect;
    public int playerID;

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
            //data.missionUnblockedID_1 = PlayerPrefs.GetInt("missionUnblockedID_1", 0);
            //data.missionUnblockedID_2 = PlayerPrefs.GetInt("missionUnblockedID_2", 0);
            //data.missionUnblockedID_3 = PlayerPrefs.GetInt("missionUnblockedID_3", 0);
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
        data.userID = PlayerPrefs.GetString("userID");
        
        if (data.userID.Length<2)
        {
            data.userID = SystemInfo.deviceUniqueIdentifier;
        } else
        {
            data.userID = PlayerPrefs.GetString("userID");
        }
         serverConnect.LoadUserData(data.userID, OnLoaded);
        //OnLoaded(null);
    }
    public bool IsLogged()
    {
        return data.username != "";
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
    void OnLoaded(ServerConnect.UserDataInServer data)
    {
        this.data = data;
        print("user on server.  username: " + data.username + " userID: " + data.userID );
        hiscoresByMissions.Init();
        if (Data.Instance.playMode == Data.PlayModes.STORYMODE || Data.Instance.playMode == Data.PlayModes.SURVIVAL)
            Data.Instance.events.OnSaveScore += OnSaveScore;

        //score = PlayerPrefs.GetInt("score");
        //missionUnblockedID_1 = PlayerPrefs.GetInt("missionUnblockedID_1");
        //missionUnblockedID_2 = PlayerPrefs.GetInt("missionUnblockedID_2");
        //missionUnblockedID_3 = PlayerPrefs.GetInt("missionUnblockedID_3");

        //return;

    }
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
                    case 1: data.missionUnblockedID_1 = missionID; break;
                    case 2: data.missionUnblockedID_2 = missionID; break;
                    case 3: data.missionUnblockedID_3 = missionID; break;
                }
            }
        }
    }
    public int GetMissionUnblockedByVideogame(int videogameID)
    {
        switch (videogameID)
        {
            case 1: return data.missionUnblockedID_1;
            case 2: return data.missionUnblockedID_2;
            default: return data.missionUnblockedID_3;
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
        //PlayerPrefs.SetInt("score", score);
        //PlayerPrefs.GetInt("missionUnblockedID_1", data.missionUnblockedID_1);
        //PlayerPrefs.GetInt("missionUnblockedID_2", data.missionUnblockedID_2);
        //PlayerPrefs.GetInt("missionUnblockedID_3", data.missionUnblockedID_3);
        //return;
//
        StartCoroutine(SaveUserDataC());
    }
    IEnumerator SaveUserDataC()
    {
        string hash = Utils.Md5Sum(UserData.Instance.data.userID + score + data.missionUnblockedID_1 + data.missionUnblockedID_2 + data.missionUnblockedID_3 + "pontura");
        string post_url = URL + setUserDataURL + "?userID=" + WWW.EscapeURL(UserData.Instance.data.userID) + "&score=" + score
            + "&missionUnblockedID_1=" + data.missionUnblockedID_1
            + "&missionUnblockedID_2=" + data.missionUnblockedID_2
            + "&missionUnblockedID_3=" + data.missionUnblockedID_3
            + "&hash=" + hash;

        print("grabe: " + post_url);

        WWW www = new WWW(post_url);
        yield return www;

        if (www.error != null)
        {
            //UsersEvents.OnPopup("There was an error: " + www.error);
        }
        else
        {
            string result = www.text;
            if (result == "exists")
            {
                UsersEvents.OnPopup("ya existe");
            }
            else
            {
                Debug.Log("UserData updated " + post_url);
            }
        }
    }
}
