using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSMeter : MonoBehaviour
{
	float deltaTime = 0.0f;

	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
	}

	void OnGUI()
	{
		//float msec = deltaTime * 1000.0f;
		//float fps = 1.0f / deltaTime;
		//field.text = string.Format("{0:0.0} fps",fps);


        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 10 / 100;
        style.normal.textColor = Color.white;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = fps.ToString();
        GUI.Label(rect, text, style);

    }
}
