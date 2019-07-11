using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserRegistrationForm : MonoBehaviour
{
    public UserDataUI ui;
    private string secretKey = "pontura";

    UserData userData;
    public bool imageUploaded;

    public void Init()
    {
        userData = UserData.Instance;
       // LoadUser();
    }
//    void LoadUser()
//    {
//        if (PlayerPrefs.GetString("userID") != "")
//        {
//            userData.userID = PlayerPrefs.GetString("userID");
//            userData.username = PlayerPrefs.GetString("username");
//        }
//        else
//        {
//#if UNITY_EDITOR
//            userData.userID = Random.Range(0, 10000).ToString();
//            userData.SetUserID(userData.userID);
//#elif UNITY_ANDROID
//				userData.userID = SystemInfo.deviceUniqueIdentifier;
//				userData.SetUserID(userData.userID);
//#endif
//        }
//    }

    void UserCreation()
    {
        UsersEvents.OnPopup("new User Created " + UserData.Instance.username);
        UserData.Instance.UserCreation();
        UsersEvents.OnRegistartionDone();
    }
    void UserUploaded()
    {
        UsersEvents.OnPopup( "User uploaded " + UserData.Instance.username );
        UserData.Instance.UserCreation();
        UsersEvents.OnUserUploadDone();
    }

    public void SaveUser(string username)
    {
        UsersEvents.OnPopup( "Checking data...");
        UserData.Instance.username = username;
        StartCoroutine(SendData(UserData.Instance.username));
    }
    public void UploadUser(string username)
    {
        UsersEvents.OnPopup(  "Uploading data...");
        UserData.Instance.username = username;
        StartCoroutine(UploadData(UserData.Instance.username));

    }
    public void SavePhoto()
    {
       
        string imagePath = UserData.Instance.path;

        if (UserData.Instance.userID == "")
        {
            Debug.LogError("NO EXISTE EL USUARIO");
            return;
        }

        StartCoroutine(UploadFileCo(imagePath + UserData.Instance.userID + ".png", userData.URL + userData.imageURLUploader));
    }
    IEnumerator SendData(string username)
    {
        string hash = Utils.Md5Sum(UserData.Instance.userID + username + secretKey);
        string post_url = userData.URL + userData.setUserURL + "?userID=" + WWW.EscapeURL(UserData.Instance.userID) + "&username=" + username + "&hash=" + hash;
        print(post_url);
        WWW www = new WWW(post_url);
        yield return www;

        if (www.error != null)
        {
            UsersEvents.OnPopup( "There was an error: " + www.error);
        }
        else
        {
            string result = www.text;
            if (result == "exists")
            {
                UsersEvents.OnPopup(  "ya existe");
            }
            else
            {
                UserCreation();
            }
        }
    }
    IEnumerator UploadData(string username)
    {
        string hash = Utils.Md5Sum(UserData.Instance.userID + username + secretKey);
        string post_url = userData.URL + userData.setUserURLUpload + "?userID=" + WWW.EscapeURL(UserData.Instance.userID) + "&username=" + username + "&hash=" + hash;
        print(post_url);
        WWW www = new WWW(post_url);
        yield return www;

        if (www.error != null)
        {
            UsersEvents.OnPopup( "There was an error: " + www.error);
        }
        else
        {
            string result = www.text;
            if (result == "exists")
            {
                UsersEvents.OnPopup( "ya existe");
            }
            else
            {
                UserUploaded();
            }
        }
    }

    IEnumerator UploadFileCo(string localFileName, string uploadURL)
    {
        print("file" + localFileName + " to: " + uploadURL);
        WWW localFile = new WWW("file:///" + localFileName);
        yield return localFile;
        if (localFile.error == null)
            Debug.Log("Loaded file successfully");
        else
        {
            Debug.Log("Open file error: " + localFile.error);
            yield break; // stop the coroutine here
        }
        WWWForm postForm = new WWWForm();
        postForm.AddBinaryData("theFile", localFile.bytes, localFileName, "text/plain");
        WWW upload = new WWW(uploadURL, postForm);
        yield return upload;
        if (upload.error == null)
            UsersEvents.FileUploaded();
        else
            UsersEvents.OnPopup("Error during upload: " + upload.error);
    }
}
