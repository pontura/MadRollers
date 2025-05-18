using UnityEngine;
using System.Collections;

public class Transport : MonoBehaviour
{
    public bool isOn;

    public void SetOn()
    {
        if (isOn) return;
        isOn = true;
        OnSetOn();
    }
    public void SetOff()
    {
        if (!isOn) return;
        isOn = false;
        OnSetOff();
    }
    public virtual void OnSetOn() {}
    public virtual void OnSetOff() {}

}