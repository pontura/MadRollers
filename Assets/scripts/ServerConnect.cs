using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ServerConnect : MonoBehaviour
{
    private string secretKey = "pontura";
    string getUserURL = "getUser.php";

    public bool loaded;

    [Serializable]
    public class UserDataInServer
    {
        public string username;
        public string userID = "sJgYfFhH7wWHwmoPLOAFT6cbvKt1";
        public int score;
        public int missionUnlocked;
    }

}
