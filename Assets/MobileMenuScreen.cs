using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileMenuScreen : MonoBehaviour
{
    public GameObject controlsMangerButton;
    public Text controlsTypeField;
    public Text pixelateField;
    public Text langField;

    void Start()
    {
        if (Data.Instance.isAndroid)
        {
            controlsMangerButton.SetActive(false);
            SetFields();
        }
    }
    private void OnEnable()
    {
        SetTexts();
    }
    public void ChangeControlsType()
    {
        if (Data.Instance.controlsType == Data.ControlsType.GYROSCOPE)
            Data.Instance.controlsType = Data.ControlsType.VIRTUAL_JOYSTICK;
        else
            Data.Instance.controlsType = Data.ControlsType.GYROSCOPE;
        PlayerPrefs.SetString("controlsType", Data.Instance.controlsType.ToString());
        SetFields();
    }
    public void ChangeLang()
    {
        TextsManager.Instance.NextLang();
        langField.text = TextsManager.Instance.lang.ToUpper();
        PlayerPrefs.SetString("controlsType", Data.Instance.controlsType.ToString());
        SetFields();
    }
    void SetFields()
    {
        langField.text = TextsManager.Instance.lang.ToUpper();
        controlsTypeField.text = Data.Instance.controlsType.ToString();
    }
    public void SwitchPixels()
    {
        PlayerPrefs.SetString("useRetroPixelPro", Data.Instance.useRetroPixelPro.ToString());
        Data.Instance.useRetroPixelPro = !Data.Instance.useRetroPixelPro;
        SetTexts();
    }
    void SetTexts()
    {
        string useRetroPixelPro = PlayerPrefs.GetString("useRetroPixelPro", "true");

        if (useRetroPixelPro == "false")
            Data.Instance.useRetroPixelPro = false;

        pixelateField.text = "FULL " + Data.Instance.useRetroPixelPro;
    }
    public void Controls()
    {
      //  Data.Instance.controlMapper.Open();
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
