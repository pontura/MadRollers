using UnityEngine;
using System.Collections;

public class Subibaja : MonoBehaviour
{
    public float duration = 1.5f;
    
    void Start()
    {
        iTween.RotateBy(gameObject, iTween.Hash("x", -transform.rotation.x, "easeType", "linear", "time", duration, "looptype", iTween.LoopType.pingPong));
    }
}