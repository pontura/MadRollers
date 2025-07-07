using GamesTan.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static HiscoresByMissions;

public class MissionButtonMobile : MonoBehaviour, IScrollCell
{
    public int videoGameID;
    public int missionID;
    public bool isBlocked;
    [SerializeField] GameObject blocked;
   // [SerializeField] Image logo;
    [SerializeField] Image image;
    [SerializeField] RawImage rawImage;
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
        RenderTexture rt = missionSelectorMobile.levelsThumbsRecorder.GetRenderTexture(missionID);
        if (rt != null)
            AddAnimatedTexture(rt);
        else
        {
            image.gameObject.SetActive(true);
            rawImage.gameObject.SetActive(false);
            Sprite s = missionSelectorMobile.levelsThumbsRecorder.GetSprite(missionID);
            if(s != null)
                image.sprite = s;
            else
            {
                VideogameData videogameData = Data.Instance.videogamesData.GetActualVideogameDataByID(videoGameID);
                image.sprite = videogameData.floppyCover;
            }
           
        }

        SetSelector(false);
        

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
                SetStars();
            }
        } 
        else
        {
            isBlocked = true;
            anim.Play("MissionButtonLocked");
            stars.Init(0);
        }

        missionSelectorMobile.Activate(missionID);
    }
    void OnDisable()
    {
        if(missionSelectorMobile != null)
            missionSelectorMobile.Inactive(missionID);
    }
    //public void BindData(DemoCellData data)
    //{
    //    _data = data;
    //    BtnItem.onClick.RemoveListener(OnClick_BtnItem);
    //    BtnItem.onClick.AddListener(OnClick_BtnItem);
    //    // TextCount.text = data.Count.ToString();
    //    // TextName.text = data.Name.ToString();
    //    name = "Cell " + data.Idx;
    //}
    void AddAnimatedTexture(RenderTexture rt)
    {
        rawImage.gameObject.SetActive(true);
        image.gameObject.SetActive(false);
        rawImage.texture = rt;
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
    void SetStars()
    {
        int starsNum = 0;
        ScoreData scoreData = UserData.Instance.hiscoresByMissions.GetScore(missionID);
        if (scoreData.score > 15000)
            starsNum = 3;
        else if (scoreData.score > 12000)
            starsNum = 2;
        else if (scoreData.score > 8000)
            starsNum = 1;

        stars.Init(starsNum);
    }
}
