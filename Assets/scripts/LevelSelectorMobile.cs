using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorMobile : MonoBehaviour
{
    VideogameData videogameData;
    public MissionSelectorMobile missionSelectorMobile;

    public AvatarThumb avatarThumb;
    public Text scoreField;
    public Text avatarName;

    int score;
    int scoreTo;
    
    void Start()
    {
        Events.OnUserDataUpdated += OnUserDataUpdated;
        if (Data.Instance.playMode == Data.PlayModes.PARTYMODE)
            InitParty();
        else
            InitStoryMode();
    }
    private void OnDestroy()
    {
        Events.OnUserDataUpdated -= OnUserDataUpdated;
    }
    void OnUserDataUpdated()
    {
        avatarName.text = UserData.Instance.username.ToUpper();
    }
    void InitParty()
    {
        Events.OnMadRollersSFXStatus(false);
        missionSelectorMobile.Clicked(Data.Instance.missions.MissionActiveID);
    }
    void InitStoryMode()
    { 
        avatarName.text = UserData.Instance.username.ToUpper();

        scoreTo =  UserData.Instance.Score();
        scoreField.text = Utils.FormatNumbers(scoreTo);

        Events.SetHamburguerButton(true);
        Events.OnMadRollersSFXStatus(false);
        missionSelectorMobile.Init();
        Data.Instance.multiplayerData.ResetAll();
        Events.OnResetMultiplayerData();
        Data.Instance.isReplay = false;
        VoicesManager.Instance.PlaySpecificClipFromList(VoicesManager.Instance.UIItems, 0);

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
    void LoopForScore()
    {
        if (score == scoreTo)
            return;
        score += (int)(((float)scoreTo - (float)score) / 4f);
        if (score > scoreTo)
            score = scoreTo;
        scoreField.text = Utils.FormatNumbers(score);
        Invoke("LoopForScore", Time.deltaTime * 5);
    }
    public void Go()
    {
        Data.Instance.LoadLevel("Game");
    }
    public void Torneo()
    {
        //if(UserData.Instance.data.missionUnlocked<=0)
        //{
        //    Events.OnAlertSignal("TORNEO: Solo para Avanzados (desbloqueá los 3 juegos)");
        //    return;
        //}
        Data.Instance.videogamesData.actualID = 1;
        Data.Instance.missions.MissionActiveID = 0;
        Data.Instance.playMode = Data.PlayModes.SURVIVAL;
        Data.Instance.LoadLevel("Game");
    }
    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        Start();
    }
    public void EditUser()
    {
        Events.UpdateUserData();
    }
    public void Plugins()
    {
        Events.OnAlertSignal("Todavía no puedes gastar tus pixeles para construir plugins! (Próximamente)");
    }    
}
