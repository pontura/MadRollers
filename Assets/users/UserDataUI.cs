using UnityEngine;

public class UserDataUI : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField m_InputField;
    [SerializeField] GameObject panel;

    private void Start()
    {
        Close();
        Events.UpdateUserData += UpdateUserData;
    }
    private void OnDestroy()
    {
        Events.UpdateUserData -= UpdateUserData;
    }
    public void UpdateUserData()
    {
        panel.SetActive(true);
        m_InputField.text = UserData.Instance.username;
    }
    public void UpdateName()
    {
        string newName = m_InputField.text;
        if(newName.Length < 3 || newName.Length>15 || newName == UserData.Instance.username)
            Events.OnAlertSignal("Name was not changed");
        else
            UserData.Instance.UpdateUserName(newName, OnSaved);
    }
    void OnSaved(bool success, string result)
    {
        if(!success)
            Events.OnAlertSignal(result);
        else
            Events.OnUserDataUpdated();
        Close();
    }
    public void Close()
    {
        panel.SetActive(false);
    }
    
}
