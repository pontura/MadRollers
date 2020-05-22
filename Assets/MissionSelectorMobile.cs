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

    void Start()
    {
        title1.text = "MISIONES POR VIDEOJUEGO";
    }

    public void Init()
    {   
        AddButtons(0);
        AddButtons(1);
        AddButtons(2);
    }
    void AddButtons(int videoGameID)
    {
        List<Missions.MissionsData> missionData = Data.Instance.missions.videogames[videoGameID].missions; 
        Transform container = null;
        switch(videoGameID)
        {
            case 0: container = videogame1_container; break;
            case 1: container = videogame2_container; break;
            default: container = videogame3_container; break;
        }
        int missionUnblockedID = Data.Instance.missions.GetMissionsByVideoGame(videoGameID).missionUnblockedID;
        

        int id = 0;
        foreach (Missions.MissionsData data in missionData)
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
        }

        switch (videoGameID)
        {
            case 0: scrollSnap_level1.Init(missionUnblockedID); break;
            case 1: scrollSnap_level2.Init(missionUnblockedID); break;
            default: scrollSnap_level3.Init(missionUnblockedID); break;
        }


    }
    public void Clicked(int videoGameID, int MissionActiveID)
    {

        List<VoicesManager.VoiceData> list = Data.Instance.voicesManager.videogames_names;
        Data.Instance.voicesManager.PlaySpecificClipFromList(list, videoGameID);

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
        Data.Instance.GetComponent<MusicManager>().OnLoadingMusic();
        yield return new WaitForSeconds(2.8f);
        Data.Instance.LoadLevel("Game");
    }
}
