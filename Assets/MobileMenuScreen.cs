using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileMenuScreen : MonoBehaviour
{
    public void EditUser()
    {
        Data.Instance.LoadLevel("Registration");
    }
    public void ResetMissions()
    {
        Data.Instance.events.ResetMissionsBlocked();
        Data.Instance.LoadLevel("LevelSelectorMobile");
        PlayerPrefs.SetString("tutorial", "");
    }
    public void Credits()
    {
        Data.Instance.LoadLevel("Credits");
    }
}
