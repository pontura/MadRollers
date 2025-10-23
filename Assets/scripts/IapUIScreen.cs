using UnityEngine;

public class IapUIScreen : MonoBehaviour
{
    [SerializeField] GameObject panel;
    //[SerializeField] TMPro.TMP_Text field;

    void Start()
    {
        Close();
        Events.IAPInit += IAPInit;
    }
    void OnDestroy()
    {
        Events.IAPInit -= IAPInit;
    }
    void Close()
    {
        panel.gameObject.SetActive(false);
    }
    void IAPInit()
    {
        panel.gameObject.SetActive(true);
    }
    public void Pay()
    {
        Events.BuyIAP(OnReady);
    }
    public void Discord()
    {
        Application.OpenURL("https://discord.gg/jKMZ9dbr");
        Close();
    }
    void OnReady(bool payed)
    {
        Close();
    }
    public void CloseClicked()
    {
        Close();
    }
}
