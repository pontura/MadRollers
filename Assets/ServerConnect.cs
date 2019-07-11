using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ServerConnect : MonoBehaviour
{
    private string secretKey = "pontura";
    string getUserURL = "http://madrollers.com/game/getUser.php";

    public bool loaded;

    [Serializable]
    public class UserDataInServer
    {
        public string username;
        public string userID;
    }
   
    public void LoadUserData(string userID, System.Action<UserDataInServer> OnDone)
    {
        string post_url = getUserURL;
        post_url += "?userID=" + userID;

        StartCoroutine(Send(post_url, OnDone));
    }
    IEnumerator Send(string post_url, System.Action<UserDataInServer> OnDone)
    {
        print(post_url);
        WWW www = new WWW(post_url);
        yield return www;

        if (www.error != null)
            UsersEvents.OnPopup("Error en UserData server: " + www.error);
        else
        {
            if (OnDone != null)
                OnDataSended(www.text, OnDone);
        }
    }
    void OnDataSended(string result, System.Action<UserDataInServer> OnDone)
    {
        if (result != null && result.Length>5)
        {
            UserDataInServer ud = JsonUtility.FromJson<UserDataInServer>(result);
            OnDone(ud);
        }
    }

}
