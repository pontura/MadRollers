using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSMeter : MonoBehaviour
    {
	public Text field;
	float deltaTime = 0.0f;

	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
	}

	void OnGUI()
	{
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		field.text = ((int)fps).ToString();
		//field.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
	}
}
