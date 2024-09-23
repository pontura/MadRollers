using UnityEngine;
using System.Collections;
using Yaguar.Auth;
using System;
using UnityEngine.UI;

public class Testing : MonoBehaviour {

    [SerializeField] Text field;
    [SerializeField] SocialAuth socialAuth;

    void OnFirebaseAuthenticated(string username, string email, string uid)
    {
        Debug.Log("USERDATA OnFirebaseAuthenticated " + username + " email" + email + " uid: " + uid);
        field.text = "username: " + username;
        field.text += " email: " + email;
        field.text += " uid: " + uid;
        //OnLoaded(null);
    }
    private void Start()
    {
        FirebaseAuthManager.Instance.OnFirebaseAuthenticated += OnFirebaseAuthenticated;
        field.text += " START ";
        socialAuth.Init((authCode) => {
            field.text += " #socialAuth: " + authCode;
            if (authCode != "")
            {
                FirebaseAuthManager.Instance.SignInWithPlayGames(authCode, (success) =>
                {
                    if (!success) {
                        AllReady();
                    }
                });
            }
        });
    }
    void AllReady()
    {
        field.text += " SignInWithPlayGames READY!";
    }
}
