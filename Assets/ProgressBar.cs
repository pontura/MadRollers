using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour {

    public Image sprite;
    public float progression;

	private void Start()
	{
		Reset();        
	}
	public void SetProgression(float progression)
	{
        this.progression = progression;
		if(progression<0) 
			progression = 0;
		sprite.fillAmount = progression;
	}
    public void Reset()
    {
		SetProgression(0);
		sprite.fillAmount = 0;
    }
	
}
