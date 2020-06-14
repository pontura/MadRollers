using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileMenuScreen : MonoBehaviour
{
    public GameObject controlsMangerButton;
    public Text controlsTypeField;

    void Start()
    {
        if (Data.Instance.isAndroid)
        {
            controlsMangerButton.SetActive(false);
            SetControlField();
        }
    }
    public void ChangeControlsType()
    {
        if (Data.Instance.controlsType == Data.ControlsType.GYROSCOPE)
            Data.Instance.controlsType = Data.ControlsType.VIRTUAL_JOYSTICK;
        else
            Data.Instance.controlsType = Data.ControlsType.GYROSCOPE;
        PlayerPrefs.SetString("controlsType", Data.Instance.controlsType.ToString());
        SetControlField();
    }
    void SetControlField()
    {
        controlsTypeField.text = Data.Instance.controlsType.ToString();
    }
    public void Controls()
    {
        Data.Instance.controlMapper.Open();
    }
    public void ResetMissions()
    {
        Data.Instance.events.ResetMissionsBlocked();
        Data.Instance.LoadLevel("LevelSelectorMobile");
        PlayerPrefs.SetString("tutorial", "");
    }
    public void Credits()
    {
        Data.Instance.LoadLevel("Credits");
    }
    public void Debug()
    {
        FPSMeter fps = Data.Instance.GetComponent<FPSMeter>();
        fps.enabled = !fps.enabled;
    }
}
