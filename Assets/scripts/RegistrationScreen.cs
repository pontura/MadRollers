using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationScreen : MonoBehaviour
{
    public Text readyField;
    public Text yourNameField;

    void Start()
    {
        readyField.text = TextsManager.Instance.GetText("READY") + "!";
        yourNameField.text = TextsManager.Instance.GetText("Your Name") + "...";
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
