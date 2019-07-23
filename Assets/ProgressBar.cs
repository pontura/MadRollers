using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour {

    public Image sprite;
    public GameObject bar;
    public float progression;

	public void SetProgression(float progression)
	{
        this.progression = progression;
		if(progression<0) 
			progression = 0;
        SetBar();
    }
    public void Reset()
    {
		SetProgression(0);
        SetBar();
    }
    void SetBar()
    {
        if (bar != null)
        {
            Vector3 s = bar.transform.localScale;
            s.x = progression;
            bar.transform.localScale = s;
        }
        else if(sprite != null)
        {
            sprite.fillAmount = progression;
        }
    }
	
}
