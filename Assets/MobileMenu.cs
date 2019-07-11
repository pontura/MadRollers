using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileMenu : MonoBehaviour
{
    public GameObject panel;
    public MobileMenuScreen mobileMenuScreen;

    void Start()
    {
        if (!Data.Instance.isAndroid)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            mobileMenuScreen.gameObject.SetActive(false);
            panel.SetActive(false);
            Data.Instance.events.OnChangeScene += OnChangeScene;
            Data.Instance.events.SetHamburguerButton += SetHamburguerButton;
        }
    }
    void OnDestroy()
    {
        Data.Instance.events.OnChangeScene -= OnChangeScene;
        Data.Instance.events.SetHamburguerButton -= SetHamburguerButton;
    }
    void  SetHamburguerButton(bool isOn)
    {
        panel.SetActive(isOn);
    }
    void OnChangeScene(string sceneName)
    {
        if(sceneName == "LevelSelectorMobile")
        {
            panel.SetActive(true);
            Close();
        } else
            panel.SetActive(false);
    }
    public void Open()
    {
        mobileMenuScreen.gameObject.SetActive(true);
    }
    public void Close()
    {
        mobileMenuScreen.gameObject.SetActive(false);
    }
}
