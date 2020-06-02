﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionButtonMobile : MonoBehaviour
{
    public Text field;
    int videoGameID;
    int missionID;
    public bool isBlocked;
    public GameObject blocked;
    MissionSelectorMobile missionSelectorMobile;
    HiscoresLevelSelectorUI hiscoresLevelSelectorUI;
    public Image logo;
    public Image floppyCover;
    bool nevelLock;

    public void Init(MissionSelectorMobile missionSelectorMobile, int videoGameID, int missionID, MissionsManager.MissionsData data)
    {
        this.missionSelectorMobile = missionSelectorMobile;
        this.videoGameID = videoGameID;
        this.missionID = missionID;
        nevelLock = false;
        OnInit(data);
    }
    public void Init(HiscoresLevelSelectorUI hiscoresLevelSelectorUI, int videoGameID, int missionID, MissionsManager.MissionsData data)
    {
        this.hiscoresLevelSelectorUI = hiscoresLevelSelectorUI;
        this.videoGameID = videoGameID;
        this.missionID = missionID;
        nevelLock = true;
        OnInit(data);
    }
    public void OnInit(MissionsManager.MissionsData data)
    {
        VideogameData videogameData = Data.Instance.videogamesData.GetActualVideogameDataByID(videoGameID);
        logo.sprite = videogameData.logo;
        floppyCover.sprite = videogameData.floppyCover;


        int unblockedID = UserData.Instance.GetMissionUnblockedByVideogame(videoGameID + 1);
        if(UserData.Instance.missionUnblockedID_1 == 0 && videoGameID == 1)
        {
            unblockedID = -1;
        }
        if (UserData.Instance.missionUnblockedID_2 == 0 && videoGameID == 2)
        {
            unblockedID = -1;
        }

        if (missionID <= unblockedID || nevelLock)
        {
            isBlocked = false;
            if (missionID == unblockedID && !nevelLock)
            {
                Animation anim = GetComponent<Animation>();
                anim[anim.clip.name].time = Random.Range(0, 300) / 10;
                anim.Play();
            }
        } 
        else
            isBlocked = true;

        if (isBlocked)
        {
            blocked.SetActive(true);
        }
        else
        {
            blocked.SetActive(false);
        }

        int id = missionID + 1;
        //field.text = "MISION " + id;
        if (id < 10)
            field.text = "0" + id;
        else
            field.text = id.ToString();


    }
    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        { 
        } else
        {
        }
    }
    public void Clicked()
    {
        if (isBlocked)
        {
            Data.Instance.events.OnAlertSignal("Solo puedes jugar las misiones sin candado");
        }           
        else if (missionSelectorMobile != null)
            missionSelectorMobile.Clicked(videoGameID, missionID);
        else if (hiscoresLevelSelectorUI != null)
            hiscoresLevelSelectorUI.Clicked(videoGameID, missionID);
    }
}
