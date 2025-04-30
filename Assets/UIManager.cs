using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject horizontal_UI_Partymode;
    public GameObject horizontal_UI;

    void Awake()
    {
        if (Data.Instance.isAndroid)
        {
            horizontal_UI.gameObject.SetActive(false);
            horizontal_UI_Partymode.SetActive(false);
        }
        else if(Data.Instance.playMode == Data.PlayModes.PARTYMODE)
        {
            horizontal_UI_Partymode.SetActive(true);
        }
        else
        {
            horizontal_UI.SetActive(true);
            horizontal_UI_Partymode.SetActive(false);
        }
    }
}
