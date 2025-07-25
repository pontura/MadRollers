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

        Events.OnJoystickClick += OnJoystickClick;

        if (Data.Instance.playMode == Data.PlayModes.PARTYMODE)
            return;
        Events.OnJoystickUp += OnJoystickUp;
        Events.OnJoystickDown += OnJoystickDown;
        Events.OnJoystickLeft += OnJoystickLeft;
        Events.OnJoystickRight += OnJoystickRight;
        
    }

    void OnDestroy()
    {
        Events.OnJoystickUp -= OnJoystickUp;
        Events.OnJoystickDown -= OnJoystickDown;
        Events.OnJoystickLeft -= OnJoystickLeft;
        Events.OnJoystickRight -= OnJoystickRight;
        Events.OnJoystickClick -= OnJoystickClick;
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
    bool clicked;
    void OnJoystickClick()
    {
        if (clicked)
            return;
        clicked = true;
        missionSelectorMobile.Clicked(Data.Instance.missions.MissionActiveID);
    }
}
