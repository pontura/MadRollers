using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArcadeMenuButton : MonoBehaviour {

    public Image bg;
    public Text field;

    public Color colorOn;
    public Color colorOff;

    public bool isOn;

    public void SetOn()
    {
        isOn = true;
        bg.color = colorOff;
        field.color = colorOn;
    }
    public void SetOff()
    {
        isOn = false;
        bg.color = colorOn;
        field.color = colorOff;
    }
}
