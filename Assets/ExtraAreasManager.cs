using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraAreasManager : MonoBehaviour
{
    Missions missions;
    public MissionData Mission_Xtras;
    int xtrasID;
    
    public bool isBossOn;
    float delayToExtraArea = 4;
    int startingDelay = 6;
    int nextBossArea = 4;
    int id;
    

    void Start()
    {
        Data.Instance.events.OnBossActive += OnBossActive;
        Data.Instance.events.OnGameOver += OnGameOver;
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
    }
    void NotAvailable()
    {
        Data.Instance.events.OnBossActive -= OnBossActive;
        Data.Instance.events.OnGameOver -= OnGameOver;
        Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
    }
    void StartMultiplayerRace()
    {
        if (Data.Instance.playMode != Data.PlayModes.SURVIVAL)
        {
            NotAvailable();
            return;
        }  
        Reset();
        Invoke("Loop", 6);
        ShuffleMissions(missions.MissionActive.areaSetData);
       // ShuffleMissions(Mission_Xtras.areaSetData);
    }
    void OnGameOver(bool a)
    {
        CancelInvoke();
        Reset();
    }
    void Loop()
    {
        Invoke("Loop", delayToExtraArea);
        SetExtraArea();
    }
    void Reset()
    {
        id = 0;
        isBossOn = false;
    }
    void OnBossActive(bool isOn)
    {
        this.isBossOn = isOn;
    }
    public void Init()
    {
        missions = GetComponent<Missions>();
        Mission_Xtras = missions.LoadDataFromMission("survival", "boyland_extras").data[0];
       
    }    
    void SetExtraArea()
    {
        MissionData.AreaSetData areaSetData;
        if (id % nextBossArea == 0 && !isBossOn)
            areaSetData = GetBossArea();
        else
            areaSetData = GetArea();

        print("new XTRA: " + areaSetData.areas.Count);
        missions.CreateCurrentArea(areaSetData.areas[UnityEngine.Random.Range(0, areaSetData.areas.Count)], true);
        id++;
    }
    MissionData.AreaSetData GetBossArea()
    {
        isBossOn = true;
        return Mission_Xtras.areaSetData[0];
    }
    MissionData.AreaSetData GetArea()
    {
        return Mission_Xtras.areaSetData[1];
        // return Mission_Xtras.areaSetData[Random.Range(1, Mission_Xtras.areaSetData.Count) ];
    }
    void ShuffleMissions(List<MissionData.AreaSetData> arr)
    {
        for (int a = 0; a < 50; a++)
        {
            int rand = UnityEngine.Random.Range(1, arr.Count);
            MissionData.AreaSetData randomMission1 = arr[2];
            MissionData.AreaSetData randomMission2 = arr[rand];

            arr[rand] = randomMission1;
            arr[2] = randomMission2;
        }
    }
}
