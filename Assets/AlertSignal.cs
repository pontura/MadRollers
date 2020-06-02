using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertSignal : MonoBehaviour
{
    public Text field;
    public GameObject panel;
    bool isOn;

    void Start()
    {
        panel.SetActive(false);
        Data.Instance.events.OnAlertSignal += OnAlertSignal;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnAlertSignal -= OnAlertSignal;
    }
    void OnAlertSignal(string text)
    {
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
