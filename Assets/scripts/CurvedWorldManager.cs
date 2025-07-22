using UnityEngine;
using System.Collections;

public class CurvedWorldManager : MonoBehaviour {

    float bendingSpeed = 0.01f;
    float Bending_Start = -20;


	public void Start () {
        Shader.SetGlobalFloat("_Curvature", 0.002f);
        Data.Instance.events.ChangeCurvedWorldX += ChangeCurvedWorldX;

    }
    public void OnDestroy()
    {
        Data.Instance.events.ChangeCurvedWorldX -= ChangeCurvedWorldX;
    }
   
    float _x;
    void ChangeCurvedWorldX(float _x)
    {
       // sharedMaterial.SetFloat("_Curvature", 0.03f);
        // Shader.SetGlobalFloat("_Curvature", 0.002f);
    }
    //private void FixedUpdate()
    //{
    //    if (Mathf.Abs(_x) - Mathf.Abs(curvedWorld_Controller._V_CW_Bend_X) < 0.01f) return;
    //    float newCurve = Mathf.Lerp(curvedWorld_Controller._V_CW_Bend_X, _x, 0.05f);
    //    curvedWorld_Controller._V_CW_Bend_X = newCurve;
    //}

}
