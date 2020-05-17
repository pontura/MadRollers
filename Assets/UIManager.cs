using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject horizontal_UI;
    public GameObject vertical_UI;

    void Awake()
    {
        if (Data.Instance.isAndroid)
        {
            vertical_UI.SetActive(true);
            horizontal_UI.gameObject.SetActive(false);
        }
        else
        {
            horizontal_UI.SetActive(true);
            vertical_UI.gameObject.SetActive(false);
        }
    }
}
