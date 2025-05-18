using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Subibaja : MonoBehaviour
{
    public float duration = 1.5f;
    
    void Start()
    {
        transform.DORotate(new Vector3(-transform.rotation.x, transform.rotation.y, transform.rotation.z), duration).SetLoops(5, LoopType.Yoyo).SetSpeedBased();
        // iTween.RotateBy(gameObject, iTween.Hash("x", -transform.rotation.x, "easeType", "linear", "time", duration, "looptype", iTween.LoopType.pingPong));
    }
}