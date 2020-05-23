using System.Collections;
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
    public Image logo;
    public Image floppyCover;

    public void Init(MissionSelectorMobile missionSelectorMobile, int videoGameID, int missionID, MissionsManager.MissionsData data)
    {
        VideogameData videogameData = Data.Instance.videogamesData.GetActualVideogameDataByID(videoGameID);
        logo.sprite = videogameData.logo;
        floppyCover.sprite = videogameData.floppyCover;

        this.missionSelectorMobile = missionSelectorMobile;
        this.videoGameID = videoGameID;
        this.missionID = missionID;

        int unblockedID = MissionsManager.Instance.videogames[videoGameID].missionUnblockedID;

        //bloquea todo si no jugaste:
        if(videoGameID>0)
        {
            int level1blockedID = MissionsManager.Instance.videogames[0].missionUnblockedID;
            if (level1blockedID == 0)
                unblockedID = -1;
        }
        //////////////////
        
        if (missionID <= unblockedID)
        {
            isBlocked = false;
            if (missionID == unblockedID)
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
        if (isBlocked) return;
        missionSelectorMobile.Clicked(videoGameID, missionID);
    }
}
