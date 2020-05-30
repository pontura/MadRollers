using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiscoresLevelSelectorUI : MonoBehaviour
{
    public Text videogameTitleField;
    public MissionButtonMobile missionButton;
    public GameObject panel;
    public HiscoresMobile hiscoresMobile;
    int videoGameID = 0;
    int missionID = 0;
    public Transform missionscontainer;

    void Start()
    {
        panel.SetActive(false);
    }
    public void Init()
    {
        Data.Instance.events.SetHamburguerButton(false);
        panel.SetActive(true);
        InitMissions();        
    }
    public void NextVideogame(bool next)
    {
        if (next)
            videoGameID++;
        else
            videoGameID--;
        if (videoGameID < 0)
            videoGameID = 0;
        else if (videoGameID > 2)
            videoGameID = 2;
        InitMissions();
    }
    void InitMissions()
    {
        Clicked(videoGameID, missionID);
        Utils.RemoveAllChildsIn(missionscontainer);
        List<MissionsManager.MissionsData> missionData = MissionsManager.Instance.videogames[videoGameID].missions;
        videogameTitleField.text = Data.Instance.videogamesData.all[videoGameID].name;
        int missionUnblockedID = Data.Instance.missions.GetMissionsByVideoGame(videoGameID).missionUnblockedID;


        int id = 0;
        foreach (MissionsManager.MissionsData data in missionData)
        {
            MissionButtonMobile m = Instantiate(missionButton);
            m.transform.SetParent(missionscontainer);
            m.transform.localPosition = Vector3.zero;
            m.transform.localScale = Vector3.one;
            m.Init(this, videoGameID, id, data);

            if (id == missionUnblockedID)
                m.SetSelected(true);
            else
                m.SetSelected(false);

            id++;
        }
    }
    public void Clicked(int videoGameID, int MissionActiveID)
    {
        hiscoresMobile.Init(videoGameID, MissionActiveID, MyScoreLoaded);
    }
    void MyScoreLoaded(int a)
    { }
    public void Close()
    {
        Data.Instance.events.SetHamburguerButton(true);
        panel.SetActive(false);
    }
}
