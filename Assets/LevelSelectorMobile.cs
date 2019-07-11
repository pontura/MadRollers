using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelSelectorMobile : MonoBehaviour
{
    public Text title;

    public MissionButton diskette1;
    public MissionButton diskette2;
    public MissionButton diskette3;

    VideogameData videogameData;
    public int videgameID;

    bool canInteract;
    float timePassed;

    public MissionSelectorMobile missionSelectorMobile;

    void Start()
    {
        missionSelectorMobile = GetComponent<MissionSelectorMobile>();
        Data.Instance.multiplayerData.ResetAll();
        Data.Instance.events.OnResetScores();

      //  title.text = "SELECT A VIDEOGAME TO DESTROY";
        videgameID = Data.Instance.videogamesData.actualID;
        Data.Instance.voicesManager.PlaySpecificClipFromList(Data.Instance.voicesManager.UIItems, 0);

        InitButton(diskette1, 0);
        InitButton(diskette2, 1);
        InitButton(diskette3, 2);

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
    void InitButton(MissionButton diskette, int id)
    {
        VideogameData data = Data.Instance.videogamesData.all[id];
        diskette.Init(data);
        diskette.SetMobile(this);
        diskette.GetComponent<MissionSelector>().LoadVideoGameData(id);    
        diskette.GetHiscore();
    }
    public void OnMissionButtonClicked(MissionButton button)
    {
        if (diskette1 != button)
            diskette1.SetMenuButtonOff();
        if (diskette2 != button)
            diskette2.SetMenuButtonOff();
        if (diskette3 != button)
            diskette3.SetMenuButtonOff();
    }
    public void Back()
    {
        Data.Instance.videogamesData.actualID = videgameID;
        Data.Instance.LoadLevel("MainMenu");
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
