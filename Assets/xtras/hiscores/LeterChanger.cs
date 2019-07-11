using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LeterChanger : MonoBehaviour {

    private string[] letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "_" };
    public int num;
    public Text field;
    public Image bg;

    public Color BG_color_on;
    public Color BG_color_off;
    public Color BG_letter_on;
    public Color BG_letter_off;

    void Start()
    {
        SetText();
    }
    public void SetActivate(bool active)
    {
        if (active)
        {
            field.color = BG_letter_on;
            bg.color = BG_color_on;
        }
        else
        {
            field.color = BG_letter_off;
            bg.color = BG_color_off;
        }
    }

	public void ChangeLetter(bool right)
    {
        if(right) num++;
        else num--;

        if (num < 0) num = letters.Length - 1;
        else if (num >= letters.Length - 1) num =0;

        SetText();
    }
    void SetText()
    {
        field.text = letters[num];
    }
}
