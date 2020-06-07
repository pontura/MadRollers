using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileMenu : MonoBehaviour
{
    public GameObject panel;
    public MobileMenuScreen mobileMenuScreen;
    public AvatarThumb avatarThumb;
    public Text debugField;
    void Start()
    {
        if (Data.Instance.playMode != Data.PlayModes.STORYMODE)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            debugField.text = "version: [" + Application.version + "]";
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
        if (UserData.Instance.IsLogged())
            avatarThumb.Init(UserData.Instance.userID);
    }
    public void Close()
    {
        mobileMenuScreen.gameObject.SetActive(false);
    }
}
