using UnityEngine;
using System.Collections;
using VacuumShaders.CurvedWorld;
using DG.Tweening;

public class CurvedWorldManager : MonoBehaviour {

    float bendingSpeed = 0.01f;
    float Bending_Start = -20;

    public CurvedWorld_Controller curvedWorld_Controller;

	public void Start () {        
        Data.Instance.events.ChangeCurvedWorldX += ChangeCurvedWorldX;
	}
    public void OnDestroy()
    {
        Data.Instance.events.ChangeCurvedWorldX -= ChangeCurvedWorldX;
    }
    public void SetController(CurvedWorld_Controller c)
    {
        curvedWorld_Controller = c;
        //  curvedWorld_Controller._V_CW_Bend_X = -12;
       // curvedWorld_Controller.bendSize = new Vector3(-12, 0,0);
    }
    void ChangeCurvedWorldX(float _x)
    {
        if(curvedWorld_Controller==null)
            return;

        _x /= 1.2f;

       // curvedWorld_Controller.bendSize = Vector3.Lerp(curvedWorld_Controller.bendSize , new Vector3(_x, 0, 0), 0.01f);

        DOTween.To(() => curvedWorld_Controller._V_CW_Bend_X, x => curvedWorld_Controller._V_CW_Bend_X = x, _x, 3);
    }
   
}
