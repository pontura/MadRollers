using System.Collections;
using UnityEngine;
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
		Events.OnCameraChroma += OnCameraChroma;
	}
	void OnDestroy () {
		Events.OnCameraChroma -= OnCameraChroma;
	}
	void OnCameraChroma(types type)
	{
		simpleLut.enabled = true;
		this.type = type;
		StartCoroutine (Tint (type, 1));
		//StartCoroutine (ChangeHue (150, 1));
	}
	IEnumerator Tint(types type, float speed)
	{
		Color color = simpleLut.TintColor;
		float value = 0f;
		
		simpleLut.TintColor = color;

		if (type == types.RED) {

            value = 1;
            color.r = value;
            color.g = value;
            color.b = value;
            while (value >0.5f)
            {
                value -= speed*Time.deltaTime;
                color.g = value;
                color.b = value;
                simpleLut.TintColor = color;
				yield return new WaitForEndOfFrame ();
			}
		} else if (type == types.NONE)
        {
            value = 0.5f;
            color.r = 1;
            color.g = value;
            color.b = value;
            while (value < 1)
            {
                value += speed * Time.deltaTime;
                color.g = value;
                color.b = value;
                simpleLut.TintColor = color;
                yield return new WaitForEndOfFrame();
            }
            simpleLut.enabled = false;
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
