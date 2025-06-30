using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionButtonMobile : MonoBehaviour
{
    public Text field;
    public int videoGameID;
    public int missionID;
    public bool isBlocked;
    [SerializeField] GameObject blocked;
    [SerializeField] Image logo;
    [SerializeField] Image floppyCover;
    [SerializeField] GameObject selector;
    [SerializeField] Stars stars;
    [SerializeField] TMPro.TMP_Text nameField;
    [SerializeField] TMPro.TMP_Text missionNumField;

    MissionSelectorMobile missionSelectorMobile;
    HiscoresLevelSelectorUI hiscoresLevelSelectorUI;

    public void Init(MissionSelectorMobile missionSelectorMobile, MissionData missionData)
    {
        this.missionSelectorMobile = missionSelectorMobile;
        this.videoGameID = missionData.videoGameID;
        this.missionID = missionData.id;
        nameField.text = missionData.title;
        missionNumField.text = "MISSION " + (missionData.id+1);

        SetSelector(false);
        VideogameData videogameData = Data.Instance.videogamesData.GetActualVideogameDataByID(videoGameID);
        logo.sprite = videogameData.logo;
        floppyCover.sprite = videogameData.floppyCover;


        int unblockedID = UserData.Instance.GetMissionUnlocked();

        if (missionID <= unblockedID || Data.Instance.isAdmin)
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
            stars.Init(0);
            blocked.SetActive(true);
        }
        else
        {
            stars.Init(2);
            blocked.SetActive(false);
        }

        int id = missionID + 1;
        //field.text = "MISION " + id;
        if (id < 10)
            field.text = "0" + id;
        else
            field.text = id.ToString();

    }
    public void Clicked()
    {
        if (isBlocked)
            missionSelectorMobile.ClickedABlockedButton();
        else if (missionSelectorMobile != null)
            missionSelectorMobile.Clicked(missionID);
        else if (hiscoresLevelSelectorUI != null)
            hiscoresLevelSelectorUI.Clicked(videoGameID, missionID);
    }
    public void SetSelector(bool isOn)
    {
        selector.SetActive(isOn);
    }
}
