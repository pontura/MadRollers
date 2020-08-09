using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionSelectorMobile : MonoBehaviour
{
    public Transform videogame1_container;
    public Transform videogame2_container;
    public Transform videogame3_container;

    public Canvas canvas;
    public Animation anim;

    public Text title1;    
    public MissionButtonMobile missionButton;

    public Text disketteField;
    public Image disketteLogo;
    public Image disketteFloppy;

    public ScrollSnapTo scrollSnap_level1;
    public ScrollSnapTo scrollSnap_level2;
    public ScrollSnapTo scrollSnap_level3;

    public List<MissionButtonMobile> allButtons;

    int videogameID = -1;

    void Start()
    {
        title1.text = TextsManager.Instance.GetText("VIDEOGAMES");
    }

    public void Init()
    {
        videogameID = Data.Instance.videogamesData.actualID;
        AddButtons(0);
        AddButtons(1);
        AddButtons(2);

        ChangeVideoGame();
        SetSelector();
    }
    void AddButtons(int videoGameID)
    {
        List < MissionsManager.MissionsData> missionData = MissionsManager.Instance.videogames[videoGameID].missions; 
        Transform container = null;
        switch(videoGameID)
        {
            case 0: container = videogame1_container; break;
            case 1: container = videogame2_container; break;
            default: container = videogame3_container; break;
        }
        int missionUnblockedID = UserData.Instance.GetMissionUnblockedByVideogame(videoGameID + 1);
        

        int id = 0;
        foreach (MissionsManager.MissionsData data in missionData)
        {
            MissionButtonMobile m = Instantiate(missionButton);
            m.transform.SetParent(container);
            m.transform.localPosition = Vector3.zero;
            m.transform.localScale = Vector3.one;
            m.Init(this, videoGameID, id, data);

            if (id == missionUnblockedID)
                m.SetSelected(true);
            else
                m.SetSelected(false);

            id++;
            allButtons.Add(m);
        }

        switch (videoGameID)
        {
            case 0: scrollSnap_level1.Init(missionUnblockedID); break;
            case 1: scrollSnap_level2.Init(missionUnblockedID); break;
            default: scrollSnap_level3.Init(missionUnblockedID); break;
        }
    }
    public void ClickedABlockedButton()
    {
        Data.Instance.events.OnAlertSignal("YOU MUST DESTROY ALL PREVIOUS DISKETTES!");
    }
    public void Clicked(int videoGameID, int MissionActiveID)
    {
        foreach (MissionButtonMobile mbm in allButtons)
        {
            if (mbm.videoGameID == Data.Instance.videogamesData.actualID && mbm.missionID == Data.Instance.missions.MissionActiveID)
            {
                if (mbm.isBlocked)
                {
                    ClickedABlockedButton();
                    return;
                }
            }
        }

        Data.Instance.events.OnSoundFX("whip", -1);
        List<VoicesManager.VoiceData> list = VoicesManager.Instance.videogames_names;
        VoicesManager.Instance.PlaySpecificClipFromList(list, videoGameID);

        canvas.enabled = false;

        string m = (MissionActiveID + 1).ToString();

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
    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(3);
        Data.Instance.musicManager.OnLoadingMusic();
        yield return new WaitForSeconds(2.8f);
        Data.Instance.playMode = Data.PlayModes.STORYMODE;
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
        switch (Data.Instance.videogamesData.actualID)
        {
            case 0:                
                scrollSnap_level1.Init(Data.Instance.missions.MissionActiveID);
                break;
            case 1:
                scrollSnap_level2.Init(Data.Instance.missions.MissionActiveID);
                break;
            case 2:
                scrollSnap_level3.Init(Data.Instance.missions.MissionActiveID);
                break;
        }
        
    }
    public void ChangeVideoGame()
    {
        switch(Data.Instance.videogamesData.actualID)
        {
            case 0: Data.Instance.missions.MissionActiveID = UserData.Instance.missionUnblockedID_1; break;
            case 1: Data.Instance.missions.MissionActiveID = UserData.Instance.missionUnblockedID_2; break;
            case 2: Data.Instance.missions.MissionActiveID = UserData.Instance.missionUnblockedID_3; break;
        }
    }
}
