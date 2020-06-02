using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public string URL = "http://madrollers.com/game/";
    public string setUserURL = "setUser.php";
    public string setUserURLUpload = "updateUser.php";
    public string imageURLUploader = "uploadPhoto.php";
    public string imagesURL = "users/";

    const string PREFAB_PATH = "UserData";
    static UserData mInstance = null;
    public string userID;
    public string username;

	public string path;
    public HiscoresByMissions hiscoresByMissions;
    public AvatarImages avatarImages;
    public ServerConnect serverConnect;
    public int playerID;
    public bool logged;

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
                print("ASDSASDAADSAS");
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
        missionUnblockedID_1 = PlayerPrefs.GetInt("missionUnblockedID_1", 0);
        missionUnblockedID_2 = PlayerPrefs.GetInt("missionUnblockedID_2", 0);
        missionUnblockedID_3 = PlayerPrefs.GetInt("missionUnblockedID_3", 0);

    }
    private void Start()
    {
        LoadUser();

        hiscoresByMissions.Init();

        
    }
    void LoadUser()
    {
        playerID = PlayerPrefs.GetInt("playerID");
        userID = PlayerPrefs.GetString("userID");
        if (userID.Length<2)
        {
#if UNITY_EDITOR
            userID = Random.Range(0, 10000).ToString();
            SetUserID(userID);
#elif UNITY_ANDROID
			userID = SystemInfo.deviceUniqueIdentifier;
			SetUserID(userID);            
#endif
            logged = false;
        } else
        {
            logged = true;
            userID = PlayerPrefs.GetString("userID");
            username = PlayerPrefs.GetString("username");
            avatarImages.GetImageFor(userID, null);
        }
        serverConnect.LoadUserData(userID, OnLoaded);
    }
    void OnLoaded(ServerConnect.UserDataInServer dataLoaded)
    {
        if (dataLoaded != null && dataLoaded.username != "")
        {
            logged = true;
            userID = dataLoaded.userID;
            username = dataLoaded.username;
            print("User data loaded: " + userID + "   username: " + username);
        }
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
        Debug.Log("Busca imagen en: " + path);
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            Debug.Log("Image exists in local");
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(300, 300);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }
    public void UpdateData()
    {
        print("UpdateData");
      //  Data.Instance.serverManager.LoadUserData(userID);
    }
    public void SetMissionReady(int videogameID, int missionID)
    {
        int id = PlayerPrefs.GetInt("missionUnblockedID_" + videogameID);
        if (id < missionID)
        {
            PlayerPrefs.SetInt("missionUnblockedID_" + videogameID, missionID);
            switch(videogameID)
            {
                case 1:  missionUnblockedID_1 = missionID; break;
                case 2: missionUnblockedID_2 = missionID; break;
                case 3: missionUnblockedID_3 = missionID; break;
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
}
