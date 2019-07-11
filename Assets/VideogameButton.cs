using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideogameButton : MonoBehaviour {

	public Image thumb;
	public Image bg;

	public void Init (Sprite sprite) {
		thumb.sprite = sprite;
		bg.color = Color.black;
	}
	public void SetSelected(bool isOn)
	{
		if (isOn) {
			bg.color = Color.green;
		} else {
			bg.color = Color.black;
		}
	}
}
