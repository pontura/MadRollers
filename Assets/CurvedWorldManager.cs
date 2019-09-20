using UnityEngine;
using System.Collections;
using VacuumShaders.CurvedWorld;
using DG.Tweening;

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
        curvedWorld_Controller._V_CW_Bend_X = -12;
    }
    void ChangeCurvedWorldX(float _x)
    {
        if(curvedWorld_Controller==null)
            return;

        _x /= 1.2f;

        DOTween.To(() => curvedWorld_Controller._V_CW_Bend_X, x => curvedWorld_Controller._V_CW_Bend_X = x, _x, 3);
    }
   
}
