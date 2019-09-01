using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsaneModeChanger : MonoBehaviour
{
    public bool player0;
    public bool player1;
    public bool player2;
    public bool player3;

    void Start()
    {
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
    }
    void OnDestroy()
    {
        Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
    }
    void StartMultiplayerRace()
    {
        Destroy(this);
    }
    void Update()
    {
        if (Data.Instance.inputManager.GetAxis(0, InputAction.vertical) < 0)
            player0 = true;
        else
            player0 = false;
        if (Data.Instance.inputManager.GetAxis(1, InputAction.vertical) < 0)
            player1 = true;
        else
            player1 = false;
        if (Data.Instance.inputManager.GetAxis(2, InputAction.vertical) < 0)
            player2 = true;
        else
            player2 = false;
        if (Data.Instance.inputManager.GetAxis(3, InputAction.vertical) < 0)
            player3 = true;
        else
            player3 = false;

        if(player0 && player1 && player2 && player3)
        {
            Data.Instance.events.OnResetScores();
            Data.Instance.inputSavedAutomaticPlay.RemoveAllData();
            Data.Instance.isReplay = false;
            if (Data.Instance.playMode == Data.PlayModes.SURVIVAL)
                Data.Instance.playMode = Data.PlayModes.CONTINUEMODE;
            else
                Data.Instance.playMode = Data.PlayModes.SURVIVAL;

            Data.Instance.missions.Init();
            Game.Instance.GotoMainMenu();
            Destroy(this);
        }
    }
}
