﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMobile : MonoBehaviour
{
    public GameObject DonePanel;
    public GameObject RegisterPanel;

    public Transform container;
    public Player player_to_instantiate;

    private void Start()
    {
        Data.Instance.events.OnJoystickClick += OnJoystickClick;
        Data.Instance.events.OnInterfacesStart();
        DonePanel.SetActive(false);
        RegisterPanel.SetActive(false);

        if (UserData.Instance.IsLogged())
            DonePanel.SetActive(true);
        else
            RegisterPanel.SetActive(true);

        AddPlayers();
    }
    private void OnDestroy()
    {
        Data.Instance.events.OnJoystickClick -= OnJoystickClick;
    }
    void OnJoystickClick()
    {
        if (UserData.Instance.IsLogged())
            Next();
        else
            RegisterPressed();
    }
    public void RegisterPressed()
    {
        Data.Instance.LoadLevel("Registration");
    }
    public void Next()
    {
        Data.Instance.LoadLevel("LevelSelectorMobile");
    }
    void AddPlayers()
    {
        float _separation = 5;
        for (int a = 0; a < 4; a++)
        {
            Player p = Instantiate(player_to_instantiate);
            p.isPlaying = false;
            p.transform.SetParent(container);
            p.id = a;
            p.transform.localPosition = new Vector3((-(_separation * 3) / 2) + (_separation * a), 0, 0);
            p.transform.localScale = Vector3.one;
            p.transform.localEulerAngles = Vector3.zero;
        }
    }
}
