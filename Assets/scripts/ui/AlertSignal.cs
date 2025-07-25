using UnityEngine;

public class AlertSignal : MonoBehaviour
{
    public TMPro.TMP_Text field;
    public GameObject panel;
    bool isOn;

    void Start()
    {
        panel.SetActive(false);
        Events.OnAlertSignal += OnAlertSignal;
        UsersEvents.OnPopup += OnPopup;
    }
    void OnDestroy()
    {
        Events.OnAlertSignal -= OnAlertSignal;
        UsersEvents.OnPopup -= OnPopup;
    }
    void OnPopup(string text)
    {
        OnAlertSignal(text);
    }
    void OnAlertSignal(string text)
    {
        print("OnAlertSignal: " + text);
        CancelInvoke();
        field.text = text;
        panel.SetActive(true);
        isOn = true;
        Invoke("Close", 3);
    }
    public void Close()
    {
        if (!isOn)
            return;
        CancelInvoke();
        panel.GetComponent<Animation>().Play("alertSignalOff");
        Invoke("Reset", 0.25f);
    }
    private void Reset()
    {
        panel.SetActive(false);
        isOn = false;
    }
}
