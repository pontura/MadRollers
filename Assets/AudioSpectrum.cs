﻿using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSpectrum : MonoBehaviour
{
	public AudioSource audioSource;
	public bool isOn;
	public float result1;
	public float result2;
	public float result3;
	public float result4;
	public float result5;

    private void Start()
    {
        if (Data.Instance.isAndroid)
            Destroy(this);
    }
    public void SetOn()
	{
		isOn = true;
	}
	public void SetOff()
	{
		isOn = false;
		//result = 0;
	}

	void Update()
	{
		if (!isOn)
			return;
		
		float[] spectrum = new float[256];

		audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

		//AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
		float a = 0;
		int frag = (int)(spectrum.Length / 4);
		result1 = spectrum [(frag*0)]+ spectrum [(frag*0)+1]+ spectrum [(frag*0)+2];
		result2 = result1 * (Random.Range (0, 50) - 100) / 60;
		result3 = result1 * (Random.Range (0, 50) - 100) / 60;
		result4 = result1 * (Random.Range (0, 50) - 100) / 60;
		result5 = result1 * (Random.Range (0, 50) - 100) / 60;
		//result2 = spectrum [(frag*1)]+ spectrum [(frag*1)+1]+ spectrum [(frag*1)+2];
		//result3 = spectrum [(frag*2)]+ spectrum [(frag*2)+1]+ spectrum [(frag*2)+2];
		//result4 = spectrum [(frag*3)]+ spectrum [(frag*3)+1]+ spectrum [(frag*3)+2];
		//result5 = spectrum [(frag*4)-3]+ spectrum [(frag*4)-2]+ spectrum [(frag*4)-1];
		//a /= spectrum.Length;
		//print(spectrum.Length);
	//	result = (int)Mathf.Lerp (1, 100, (a / spectrum.Length) * 1500);

	}
}