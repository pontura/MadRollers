using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistrationScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UsersEvents.OnRegistartionDone += Done;
        UsersEvents.OnUserRegisterCanceled += Done;
        UsersEvents.OnUserUploadDone += Done;
    }

    public void Done()
    {
        if (UserData.Instance.username == "")
            return;
        Data.Instance.LoadLevel("LevelSelectorMobile");
    }
}
