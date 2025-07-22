using GamesTan.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionSelectorMobile : MonoBehaviour, ISuperScrollRectDataProvider
{
    public LevelsThumbsRecorder levelsThumbsRecorder;
    public GameObject scene;
    public Canvas canvas;
    public Animation anim;

    public MissionButtonMobile missionButton;

    public Text disketteField;
    public Image disketteLogo;
    public Image disketteFloppy;


    [Header("Basic")] public SuperScrollRect ScrollRect;
    public int Count = 0;

    public List<MissionButtonMobile> allButtons;

    [SerializeField] int missionID;

    public void Init()
    {
        scene.gameObject.SetActive(false);
       // title1.text = TextsManager.Instance.GetText("VIDEOGAMES");
        AddButtons();

        ChangeVideoGame();
        SetSelector();
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

        scene.gameObject.SetActive(true);
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
    bool isLoading;
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
       // scrollSnap.Init(Data.Instance.missions.MissionActiveID);
             
        
    }
    public void ChangeVideoGame()
    {
        Data.Instance.missions.MissionActiveID = UserData.Instance.data.missionUnlocked;
    }











    int missionUnblockedID;
    void AddButtons()
    {
        List<MissionsManager.MissionsData> missionData = MissionsManager.Instance.missions;
        missionUnblockedID = UserData.Instance.GetMissionUnlocked();

        int id = 0;
        Count = missionData.Count;

        foreach (MissionsManager.MissionsData data in missionData)
        {
            levelsThumbsRecorder.AddLevel(data.data[0].jsonName, id);
            data.data[0].id = id;
            // MissionButtonMobile m = Instantiate(missionButton, container);
            // m.transform.localPosition = Vector3.zero;
            // m.transform.localScale = Vector3.one;
            //// m.Init(this, data.data[0]);
            // allButtons.Add(m);
            id++;

        }
        ScrollRect.DoAwake(this);
        DoAwake();
       
        Invoke("ResetAll", 0.1f);
    }
    void ResetAll()
    {
        levelsThumbsRecorder.ResetAll(); 
        int mission = missionUnblockedID - 1;
        if (mission < 0) mission = 0;
        ScrollRect.JumpTo(mission);
    }

    protected virtual void DoAwake()
    {
    }
    
    public int GetCellCount()
    {
        return Count;
    }

    public void SetCell(GameObject cell, int index)
    {
        print("SetCell " + index);
        var item = cell.GetComponent<MissionButtonMobile>();
        item.Init(this, MissionsManager.Instance.missions[index].data[0]);
        //levelsThumbsRecorder.Activate(index);
    }
    public void Activate(int index)
    {
        levelsThumbsRecorder.Activate(index);
    }
    public void Inactive(int index)
    {
        levelsThumbsRecorder.Inactive(index);
    }
}
