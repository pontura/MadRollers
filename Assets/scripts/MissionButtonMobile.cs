using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionButtonMobile : MonoBehaviour
{
    public int videoGameID;
    public int missionID;
    public bool isBlocked;
    [SerializeField] GameObject blocked;
   // [SerializeField] Image logo;
    [SerializeField] Image image;
    [SerializeField] GameObject selector;
    [SerializeField] Stars stars;
    [SerializeField] TMPro.TMP_Text nameField;
    [SerializeField] TMPro.TMP_Text missionNumField;

    MissionSelectorMobile missionSelectorMobile;
    HiscoresLevelSelectorUI hiscoresLevelSelectorUI;

    public void Init(MissionSelectorMobile missionSelectorMobile, MissionData missionData)
    {
        Animation anim = GetComponent<Animation>();
        this.missionSelectorMobile = missionSelectorMobile;
        this.videoGameID = missionData.videoGameID;
        this.missionID = missionData.id;
        nameField.text = missionData.title;
        missionNumField.text = "MISSION " + (missionData.id+1);

        SetSelector(false);
        VideogameData videogameData = Data.Instance.videogamesData.GetActualVideogameDataByID(videoGameID);
        // logo.sprite = videogameData.logo;
        image.sprite = videogameData.floppyCover;

        int unblockedID = UserData.Instance.GetMissionUnlocked();

        if (missionID <= unblockedID || Data.Instance.isAdmin)
        {
            isBlocked = false;
            if (missionID == unblockedID)
            {
                anim.Play("MissionButtonActive");
            }
            else
            {
                anim.Play("MissionButtonOn");
                stars.Init(2);
            }
        } 
        else
        {
            isBlocked = true;
            anim.Play("MissionButtonLocked");
            stars.Init(0);
        }

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
       // selector.SetActive(isOn);
    }
}
