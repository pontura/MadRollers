using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.SimpleLUT;

public class CameraChromaManager : MonoBehaviour {

	public SimpleLUT simpleLut;
	public types type;
	public enum types
	{
		NONE,
		RED
	}

	void Start () {
        if (Data.Instance.isAndroid)
            return;

		Data.Instance.events.OnCameraChroma += OnCameraChroma;
	}
	void OnDestroy () {
		Data.Instance.events.OnCameraChroma -= OnCameraChroma;
	}
	void OnCameraChroma(types type)
	{
		simpleLut.enabled = true;
		this.type = type;
		StartCoroutine (Tint (Color.red, 0.005f));
		//StartCoroutine (ChangeHue (150, 1));
	}
	IEnumerator Tint(Color c, float speed)
	{
		Color color = simpleLut.TintColor;
		float finalValue = 175;
		float value = 0.2f;

		color.r = 1;
		color.g = value;
		color.b =  value;
		simpleLut.TintColor = color;

		if (c == Color.red) {
			while (value < 1) {
				value += speed;
				color.g = value;
				color.b = value;
				simpleLut.TintColor = color;
				yield return new WaitForEndOfFrame ();
			}
		}
		yield return null;
	}
	IEnumerator ChangeHue(float newHue, float speed)
	{
		float hue = simpleLut.Hue;
		while (hue < 150) {
			hue += speed;
			simpleLut.Hue = hue;
			yield return new WaitForEndOfFrame ();
		}
		yield return null;
	}
}
