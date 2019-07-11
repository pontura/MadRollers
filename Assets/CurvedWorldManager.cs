using UnityEngine;
using System.Collections;
using VacuumShaders.CurvedWorld;

public class CurvedWorldManager : MonoBehaviour {

    float bendingSpeed = 0.01f;
    float Bending_Start = -20;

    public CurvedWorld_Controller curvedWorld_Controller;

	public void Init () {
        
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
        Data.Instance.events.ChangeCurvedWorldX += ChangeCurvedWorldX;

	}
    void StartMultiplayerRace()
    {
        curvedWorld_Controller = GameObject.Find("CurvedWorld_Controller").GetComponent<CurvedWorld_Controller>();
        curvedWorld_Controller._V_CW_Bend_X = -15;
    }
    void ChangeCurvedWorldX(float _x)
    {
        if(curvedWorld_Controller==null)
            return;

        return;
        Hashtable ht = new Hashtable();
        ht.Add("from",curvedWorld_Controller._V_CW_Bend_X);
        ht.Add("to",_x);
        ht.Add("time",3);
        ht.Add("onupdate","SetNewBending");
        iTween.ValueTo(gameObject,ht);
    }
    void SetNewBending(float value)
    {
        curvedWorld_Controller._V_CW_Bend_X = value;
    }
   
}
