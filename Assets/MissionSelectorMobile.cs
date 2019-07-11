using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionSelectorMobile : MonoBehaviour
{
    public GameObject panel;
    public Text title1;
    public Text title2;

    public GameObject hiscores;

    public Transform missionsContainer;
    public MissionButtonMobile missionButton;
    public List<MissionButtonMobile> allMissions;
    public Button playButton;
    public HiscoresMobile hiscoresMobile;
    int videoGameID;

    void Start()
    {
        panel.SetActive(false);
        hiscores.SetActive(false);
        title2.text = "Hi-scores";
    }

    public void Init()
    {
        Data.Instance.events.SetHamburguerButton(false);
        panel.SetActive(true);
        Utils.RemoveAllChildsIn(missionsContainer);
        videoGameID = Data.Instance.videogamesData.actualID;
        title1.text = Data.Instance.videogamesData.GetActualVideogameData().name;
        int id = 0;
        List<Missions.MissionsData> missionData = Data.Instance.missions.videogames[videoGameID].missions;

        int missionUnblockedID = Data.Instance.missions.GetMissionsByVideoGame(videoGameID).missionUnblockedID;

        foreach (Missions.MissionsData data in missionData)
        {
            MissionButtonMobile m = Instantiate(missionButton);
            m.transform.SetParent(missionsContainer);
            m.transform.localPosition = Vector3.zero;
            m.transform.localScale = Vector3.one;
            allMissions.Add(m);
            m.Init(this, videoGameID, id, data);

          

            if (id == missionUnblockedID)
                m.SetSelected(true);
            else
                m.SetSelected(false);

            id++;
        }
        hiscoresMobile.Init(videoGameID, missionUnblockedID, OnMyScoreLoaded);
    }
    public void Clicked(int id)
    {
        ResetAllMissions();
        hiscores.SetActive(true);
        allMissions[id].SetSelected(true);

        SetBlockedOrNot(id);

        Data.Instance.missions.MissionActiveID = id;
        hiscoresMobile.Init(videoGameID, id, OnMyScoreLoaded);
    }

    //esta para volver a darle por ganada una mission por si reseteo el score en el device:
    void OnMyScoreLoaded(int myscore)
    {
        if (Data.Instance.missions.GetMissionsByVideoGame(videoGameID).missionUnblockedID < Data.Instance.missions.MissionActiveID)
        {
            Data.Instance.missions.GetMissionsByVideoGame(videoGameID).missionUnblockedID = Data.Instance.missions.MissionActiveID+1;
        }
        SetPlayButton(true);
    }
    void ResetAllMissions()
    {
        foreach (MissionButtonMobile b in allMissions)
            b.SetSelected(false);
    }
    public void SetPlayButton(bool isOn)
    {
        playButton.gameObject.SetActive(isOn);
    }
    public void Back()
    {
        Data.Instance.events.SetHamburguerButton(true);
        Data.Instance.LoadLevel("LevelSelectorMobile");
    }
    public void SetBlockedOrNot(int missionID)
    {
        if (missionID <= Data.Instance.missions.GetMissionsByVideoGame(Data.Instance.videogamesData.actualID).missionUnblockedID)
            SetPlayButton(true);
        else
            SetPlayButton(false);
    }
    public void CloseHiscorea()
    {
        hiscores.SetActive(false);
    }
}
