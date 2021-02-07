using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectorField : MonoBehaviour
{
    public tpyes type;
    public enum tpyes
    {
        BOSS_HITS_TO_INACTIVE,
        DELAY,
        ACTIVE_BY_EVENT
    }
    public int boss_hits_ToInactive;
    public int delay;

    public Vector3 posOffset;
    public GameObject protectorField_to_instantiate;
    GameObject asset;
    public bool isOn;
    Boss boss;

    private void Start()
    {
        boss = GetComponent<BossPart>().boss;       
        Loop();
    }
    int sec;
    void Loop()
    {
        switch(type)
        {
            case tpyes.BOSS_HITS_TO_INACTIVE:
                if (boss.hits > boss_hits_ToInactive)
                    SetOff();
                break;
            case tpyes.DELAY:
                sec++;
                if(sec>delay)
                {
                    sec = 0;
                    if (isOn)
                        SetOff();
                    else
                        SetOn();
                }
                break;
        }
        Invoke("Loop", 1);     
    }
    private void OnEnable()
    {
        if(type != tpyes.ACTIVE_BY_EVENT)
            SetOn();
    }
    public void SetOn()
    {
        if (isOn)
            return;

        if (asset == null)
        {
            asset = Instantiate(protectorField_to_instantiate);
            asset.transform.SetParent(transform);
            asset.transform.localPosition = posOffset;
            asset.transform.localScale = Vector3.one;
            asset.transform.localEulerAngles = new Vector3(-90, 0, 0);
        }

        tag = "firewall";
        gameObject.layer = 10; //Va a Sceneobject;
        asset.SetActive(true);
        isOn = true;
        Invoke("SetOff", delay);
    }
    void SetOff()
    {
        if (isOn == false)
            return;
        tag = "destroyable";
        gameObject.layer = 17; //Va a Enemy;
        isOn = false;
        if(asset != null)
            asset.SetActive(false);
    }
}
