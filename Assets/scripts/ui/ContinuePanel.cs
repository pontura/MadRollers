using UnityEngine;
using UnityEngine.UI;

public class ContinuePanel : MonoBehaviour {

    [SerializeField] Text titleField;
    [SerializeField] Text field;
    [SerializeField] Text field2;
    [SerializeField] GameObject panel;
    [SerializeField] Button continueBtn;
    int price;
    bool canPay;
    bool clicked;

    public void Init(int price)
    {
        clicked = false;
        this.price = price;
        panel.SetActive(true);
        canPay = UserData.Instance.CanPay(price);
        titleField.text = TextsManager.Instance.GetText("CONTINUE") + " ?";
        field.text = "x " + Utils.FormatNumbers(price);
        field2.text = "TOTAL: " + Utils.FormatNumbers(UserData.Instance.data.score);
    }
    public void Accept()
    {
        if (clicked) return;
        if (canPay)
        {
            clicked = true;
            Data.Instance.events.OnPayPixeles(5000);
            Game.Instance.Continue();
            //GetComponent<Summary>().SetOff();
        }
        else
        {
            string s = "TENES " + Utils.FormatNumbers(UserData.Instance.data.score) + ". NECESITAS AL MENOS " + Utils.FormatNumbers(price) + " PIXELES";
            Debug.Log(s);
            Data.Instance.events.OnAlertSignal(s);
        }
    }
    bool adClicked;
    public void AdClicked()
    {
        if (adClicked) return;
        adClicked = true;
        Data.Instance.ads.ShowAd(OnAdDone);
    }
    void OnAdDone(bool isOK)
    {
        adClicked = false;
        if (isOK)
            Game.Instance.Continue();
        else
            Data.Instance.events.OnAlertSignal("Algo falló con el ad");
    }
    public void SetOff()
    {
        panel.SetActive(false);
    }
}
