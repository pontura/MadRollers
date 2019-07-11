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
    public Sprite sprite;
    public bool RESET_ALL_DATA;
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

                if (mInstance == null)
                {
                    return null;
                }
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

        if (RESET_ALL_DATA)
            PlayerPrefs.DeleteAll();

#if UNITY_EDITOR
        path = Application.persistentDataPath + "/";
#else
        path = Application.persistentDataPath + "/";
#endif
      
        serverConnect = GetComponent<ServerConnect>();
        avatarImages = GetComponent<AvatarImages>();
        hiscoresByMissions = GetComponent<HiscoresByMissions>();

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
        } else
        {
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
            userID = dataLoaded.userID;
            username = dataLoaded.username;
            print("User data loaded: " + userID + "   username: " + username);
        }
    }
    public bool IsLogged()
    {
        if (username == null || username.Length == 0)
            return false;
        return true;
    }
    public void SetUserID(string userID)
    {
        this.userID = userID;
        PlayerPrefs.SetString("userID", userID);
    }

    public void UserCreation()
    {

        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("userID", userID);
    }

	System.Action func;
	public void LoopUntilPhotoIsLoaded(System.Action func)
	{
		this.func = func;
		LoopUntilPhotoIsLoadedLoop();
	}
	public void LoopUntilPhotoIsLoadedLoop()
	{
		Debug.Log("Loading image from local...");
		if(sprite == null)
			Invoke("LoopUntilPhotoIsLoadedLoop", 2);
		else
			func();
		LoadUserPhoto();
	}
    void LoadUserPhoto()
    {
        sprite = LoadSprite(path + userID + ".png");
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
}
