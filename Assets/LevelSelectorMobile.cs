using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelSelectorMobile : MonoBehaviour
{
    VideogameData videogameData;

    public MissionSelectorMobile missionSelectorMobile;

    void Start()
    {
        Data.Instance.events.OnMadRollersSFXStatus(false);
        missionSelectorMobile.Init();
        Data.Instance.multiplayerData.ResetAll();
        Data.Instance.events.OnResetMultiplayerData();

        Data.Instance.voicesManager.PlaySpecificClipFromList(Data.Instance.voicesManager.UIItems, 0);


        switch (UserData.Instance.playerID)
        {
            case 0:
                Data.Instance.multiplayerData.player1 = true;
                break;
            case 1:
                Data.Instance.multiplayerData.player2 = true;
                break;
            case 2:
                Data.Instance.multiplayerData.player3 = true;
                break;
            default:
                Data.Instance.multiplayerData.player4 = true;
                break;
        }
    }
    public void Go()
    {
        Data.Instance.LoadLevel("Game");
    }
    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        Start();
    }
}
