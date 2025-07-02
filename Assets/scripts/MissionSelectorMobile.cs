using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionSelectorMobile : MonoBehaviour
{
    public Transform container;
    public LevelsThumbsRecorder levelsThumbsRecorder;

    public Canvas canvas;
    public Animation anim;

    public Text title1;    
    public MissionButtonMobile missionButton;

    public Text disketteField;
    public Image disketteLogo;
    public Image disketteFloppy;

    public ScrollSnapTo scrollSnap;

    public List<MissionButtonMobile> allButtons;

    int videogameID = -1;

    public void Init()
    {
        title1.text = TextsManager.Instance.GetText("VIDEOGAMES");
        videogameID = Data.Instance.videogamesData.actualID;
        AddButtons();

        ChangeVideoGame();
        SetSelector();
    }
    void AddButtons()
    {
        List < MissionsManager.MissionsData> missionData = MissionsManager.Instance.missions; 
        int missionUnblockedID = UserData.Instance.GetMissionUnlocked();

        Utils.RemoveAllChildsIn(container); 
        int id = 0;
        foreach (MissionsManager.MissionsData data in missionData)
        {
            levelsThumbsRecorder.AddLevel(data.data[0].jsonName, id);
            MissionButtonMobile m = Instantiate(missionButton, container);
            m.transform.localPosition = Vector3.zero;
            m.transform.localScale = Vector3.one;
            data.data[0].id = id;
            m.Init(this, data.data[0]);
            id++;
            allButtons.Add(m);
        }
        scrollSnap.Init(missionUnblockedID); 
    }
    public void ClickedABlockedButton()
    {
        Data.Instance.events.OnAlertSignal("UNLOCK ALL PREVIOUS MISSIONS FIRST");
    }
    bool clicked;
    public void Clicked(int MissionActiveID)
    {
        if (clicked)   return;  clicked = true;

        if(Data.Instance.playMode == Data.PlayModes.SURVIVAL)
            Data.Instance.playMode = Data.PlayModes.STORYMODE;

        if (Data.Instance.playMode == Data.PlayModes.STORYMODE)
        {
            foreach (MissionButtonMobile mbm in allButtons)
            {
                if (mbm.missionID == Data.Instance.missions.MissionActiveID)
                {
                    if (mbm.isBlocked)
                    {
                        ClickedABlockedButton();
                        return;
                    }
                }
            }
        }

        Data.Instance.events.OnSoundFX("whip", -1);
        List<VoicesManager.VoiceData> list = VoicesManager.Instance.videogames_names;        
        int videoGameID = MissionsManager.Instance.GetMission(MissionActiveID).videoGameID;
        VoicesManager.Instance.PlaySpecificClipFromList(list, videoGameID);


        print("SELECT videogame: " + videoGameID + " MissionActiveID: " + MissionActiveID);

        if (canvas != null)
            canvas.enabled = false;

        string m = (MissionActiveID + 1).ToString();

        if (Data.Instance.playMode == Data.PlayModes.PARTYMODE)
            m = (Data.Instance.multiplayerData.levelID_for_partyMode + 1).ToString();

        if (MissionActiveID < 10)
            disketteField.text = "0" + m;
        else
            disketteField.text = m;

        VideogameData videogameData = Data.Instance.videogamesData.GetActualVideogameDataByID(videoGameID);
        disketteLogo.sprite = videogameData.logo;
        disketteFloppy.sprite = videogameData.floppyCover;

        Data.Instance.videogamesData.actualID = videoGameID;
        Data.Instance.missions.MissionActiveID = MissionActiveID;
        anim.Play("levelSelectorOn");

        StartCoroutine(LoadGame());
        Data.Instance.events.SetHamburguerButton(false);
    }


    //Skip animation:
    bool isLoading;
    private void Update()
    {
        if (!isLoading) return;
        if (Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            Data.Instance.LoadLevel("Game");
            isLoading = false;
        }

    }
    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(0.05f);
        isLoading = true;
        yield return new WaitForSeconds(3);
        Data.Instance.musicManager.OnLoadingMusic();
        yield return new WaitForSeconds(2.8f);
        //Data.Instance.playMode = Data.PlayModes.STORYMODE;
        Data.Instance.LoadLevel("Game");
    }
    public void SetSelector()
    {
       // Debug.Log("Set selector: videogamesData.actualID " + Data.Instance.videogamesData.actualID + "   mission id: " + Data.Instance.missions.MissionActiveID);
        foreach (MissionButtonMobile mbm in allButtons)
        {
            if(mbm.videoGameID == Data.Instance.videogamesData.actualID && mbm.missionID == Data.Instance.missions.MissionActiveID)
                mbm.SetSelector(true);
            else
                mbm.SetSelector(false);
        }
        scrollSnap.Init(Data.Instance.missions.MissionActiveID);
             
        
    }
    public void ChangeVideoGame()
    {
        Data.Instance.missions.MissionActiveID = UserData.Instance.data.missionUnlocked;
        //switch(Data.Instance.videogamesData.actualID)
        //{
        //    case 0: Data.Instance.missions.MissionActiveID = UserData.Instance.data.missionUnblocked; break;
        //    case 1: Data.Instance.missions.MissionActiveID = UserData.Instance.data.missionUnblockedID_2; break;
        //    case 2: Data.Instance.missions.MissionActiveID = UserData.Instance.data.missionUnblockedID_3; break;
        //}
    }
}
