using UnityEngine;
using UnityEngine.UI;

public class ToturialStep : MonoBehaviour
{
    public Text field;
    public void Open(string text)
    {
        gameObject.SetActive(true);
        field.text = text;
    }   
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
