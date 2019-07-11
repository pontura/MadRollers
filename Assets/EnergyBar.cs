using UnityEngine;
using System.Collections;

public class EnergyBar : MonoBehaviour {

    //public Transform bar;
    //public float fillValue = 0;
    //public bool isOn;

    
    //public void Init(Color color)
    //{
    //    gameObject.SetActive(true);
    //    isOn = true;
    //    fillValue = 1;
    //    SetScale();
    //    bar.GetComponentInChildren<MeshRenderer>().material.color = color;
    //}

    //private float timerInterval;
    //public void SetTimer(float timerInterval)
    //{
    //    this.timerInterval = timerInterval;
    //    Invoke("UnFillByTime", timerInterval);
    //}
    //public void UnFillByTime()
    //{
    //    if (!isOn) return;
    //    UnFill(0.02f);
    //    Invoke("UnFillByTime", timerInterval);
    //}
    ////public void SetOff()
    ////{
    ////    isOn = false;
    ////}
    //public void UnFill(float qty)
    //{
    //    this.fillValue -= qty;
    //    SetScale();
    //}
    //void SetScale()
    //{
    //    if (!isOn) return;
        
    //    Vector3 scale = bar.localScale;
    //    scale.x = fillValue;
    //    bar.localScale = scale;

    //    if (fillValue <= 0)
    //    {
    //        isOn = false;
    //        Data.Instance.events.OnAvatarProgressBarEmpty();
    //    }
    //}
}
