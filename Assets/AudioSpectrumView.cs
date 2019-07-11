using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSpectrumView : MonoBehaviour {

	public float multiplier = 6;
	public GameObject panel;

	public Image image1;
	public Image image2;
	public Image image3;
	public Image image4;
	public Image image5;
	AudioSpectrum audioSpectrum;

	void Start () {
        if(Data.Instance.isAndroid)
        {
            Destroy(this.gameObject);
            return;
        }
		audioSpectrum = Data.Instance.voicesManager.audioSpectrum;
		Data.Instance.events.OnTalk += OnTalk;
		panel.SetActive (false);
	}
	void OnDestroy () {
		Data.Instance.events.OnTalk -= OnTalk;
	}
	bool isTalking;
	void OnTalk(bool isTalking)
	{
		panel.SetActive (isTalking);
		this.isTalking = isTalking;
	}
	void Update () {
		if (!isTalking) {
			if(panel.activeSelf)
				panel.SetActive (false);
			return;
		}
		SetSize (image1, audioSpectrum.result1);
		SetSize (image2, audioSpectrum.result2);
		SetSize (image3, audioSpectrum.result3);
		SetSize (image4, audioSpectrum.result4);
		SetSize (image5, audioSpectrum.result5);
	}
	void SetSize(Image image, float value)
	{
		image.transform.localScale = new Vector3 (1, value*multiplier, 1);
	}
}
