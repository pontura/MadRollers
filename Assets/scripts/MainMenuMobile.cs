using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuMobile : MonoBehaviour
{
    public Text playField;
    public Text registerField;

    public GameObject DonePanel;
    public GameObject RegisterPanel;

    public Transform container;
    public Player player_to_instantiate;

    private void Start()
    {
        Data.Instance.events.OnResetMultiplayerData();
        Data.Instance.isReplay = false;
        Data.Instance.videogamesData.Reset();
        Data.Instance.missions.Reset();

        playField.text = TextsManager.Instance.GetText("PLAY");
        registerField.text = TextsManager.Instance.GetText("REGISTER");

        Data.Instance.events.OnJoystickClick += OnJoystickClick;
        Data.Instance.events.OnInterfacesStart();
        DonePanel.SetActive(false);
        RegisterPanel.SetActive(false);
        
        if (UserData.Instance.IsRegistered())
            DonePanel.SetActive(true);  
        else
            RegisterPanel.SetActive(true);

        AddPlayers();
    }
    private void OnDestroy()
    {
        Data.Instance.events.OnJoystickClick -= OnJoystickClick;
    }
    bool done;
    void OnJoystickClick()
    {
        if (done) return; done = true;
        if (Data.Instance.playMode != Data.PlayModes.STORYMODE || UserData.Instance.IsRegistered())
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
        if(Data.Instance.playMode == Data.PlayModes.PARTYMODE)
            Data.Instance.LoadLevel("LevelSelector");
        else
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
