using UnityEngine;

public class MainMenuMobile : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text welcomeField;
    public GameObject DonePanel;
    public GameObject RegisterPanel;

    public Transform container;
    public Player player_to_instantiate;

    private void Start()
    {
        Events.OnResetMultiplayerData();
        Data.Instance.isReplay = false;
        Data.Instance.videogamesData.Reset();
        Data.Instance.missions.Reset();

        welcomeField.text = "HELLO " + UserData.Instance.username.ToUpper();
       // registerField.text = TextsManager.Instance.GetText("REGISTER");

        Events.OnJoystickClick += OnJoystickClick;
        Events.OnInterfacesStart();
        DonePanel.SetActive(false);
        RegisterPanel.SetActive(false);
        
       // if (UserData.Instance.IsRegistered())
            DonePanel.SetActive(true);  
        //else
        //    RegisterPanel.SetActive(true);

        AddPlayers();
    }
    private void OnDestroy()
    {
        Events.OnJoystickClick -= OnJoystickClick;
    }
    bool done;
    void OnJoystickClick()
    {
        if (done) return; done = true;
     //   if (Data.Instance.playMode != Data.PlayModes.STORYMODE || UserData.Instance.IsRegistered())
            Next();
        //else
        //    RegisterPressed();
    }
    //public void RegisterPressed()
    //{
    //    Data.Instance.LoadLevel("Registration");
    //}
    public void Next()
    {
        //if(Data.Instance.playMode == Data.PlayModes.PARTYMODE)
        //    Data.Instance.LoadLevel("LevelSelector");
        //else
        if (UserData.Instance.GetMissionUnlocked() == 0)
            GotoTutorial();
        else
            Data.Instance.LoadLevel("LevelSelectorMobile");
    }
    void GotoTutorial()
    {
        Data.Instance.InitTutorial();
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
