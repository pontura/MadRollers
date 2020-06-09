using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSelectorByJoystick : MonoBehaviour
{
    public MissionSelectorMobile missionSelectorMobile;

    void Start()
    {
        if (Data.Instance.isAndroid)
            Destroy(this);
                
        Data.Instance.events.OnJoystickUp += OnJoystickUp;
        Data.Instance.events.OnJoystickDown += OnJoystickDown;
        Data.Instance.events.OnJoystickLeft += OnJoystickLeft;
        Data.Instance.events.OnJoystickRight += OnJoystickRight;
        Data.Instance.events.OnJoystickClick += OnJoystickClick;
    }

    void OnDestroy()
    {
        Data.Instance.events.OnJoystickUp -= OnJoystickUp;
        Data.Instance.events.OnJoystickDown -= OnJoystickDown;
        Data.Instance.events.OnJoystickLeft -= OnJoystickLeft;
        Data.Instance.events.OnJoystickRight -= OnJoystickRight;
        Data.Instance.events.OnJoystickClick -= OnJoystickClick;
    }
    void OnJoystickUp()
    {
        Data.Instance.videogamesData.actualID++;
        if (Data.Instance.videogamesData.actualID >2 )
            Data.Instance.videogamesData.actualID = 0;

        missionSelectorMobile.ChangeVideoGame();
        missionSelectorMobile.SetSelector();
    }
    void OnJoystickDown()
    {
        Data.Instance.videogamesData.actualID--;
        if (Data.Instance.videogamesData.actualID < 0)
            Data.Instance.videogamesData.actualID = 3;

        missionSelectorMobile.ChangeVideoGame();
        missionSelectorMobile.SetSelector();
    }
    void OnJoystickRight()
    {
        Data.Instance.missions.MissionActiveID--;
        if (Data.Instance.missions.MissionActiveID < 0)
            Data.Instance.missions.MissionActiveID = 0;

        missionSelectorMobile.SetSelector();
    }
    void OnJoystickLeft()
    {
        Data.Instance.missions.MissionActiveID++;
        if (Data.Instance.missions.MissionActiveID > 15)
            Data.Instance.missions.MissionActiveID = 5;

        missionSelectorMobile.SetSelector();
    }
    void OnJoystickClick()
    {
        missionSelectorMobile.Clicked(Data.Instance.videogamesData.actualID, Data.Instance.missions.MissionActiveID);
    }
}
