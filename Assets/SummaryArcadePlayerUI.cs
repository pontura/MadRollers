using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SummaryArcadePlayerUI : MonoBehaviour {

    public Image background;
    public GameObject titleFields;
    public GameObject scoreFields;
    public GameObject positionFields;

	public void Init(Color color, string title, int score, int percent, int position) {

        if (score == 0)
            background.enabled = false;
        else
            background.color = color;

        foreach (Text field in titleFields.GetComponentsInChildren<Text>())
            field.text = title;

        foreach (Text field in scoreFields.GetComponentsInChildren<Text>())
        {
            if (score == 0)
            {
                field.text = "";
                positionFields.SetActive(false);
            }
            else
            {
                field.text = "PEGÓ " + score + " PUNTOS ( UN " + percent + "%)";
            }
        }

        foreach (Text field in positionFields.GetComponentsInChildren<Text>())
            field.text = position.ToString();

        
	}
}
