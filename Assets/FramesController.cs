using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramesController : MonoBehaviour {

	IEnumerator ralentaCoroutine;
	float speedEveryFrame;
	float frameRate = 1;

	void Start () {
		Data.Instance.events.RalentaTo += RalentaTo;
		Data.Instance.events.ForceFrameRate += ForceFrameRate;
	}
	public void ForceFrameRate(float newFrameRate)
	{
		if (ralentaCoroutine != null)
			StopCoroutine (ralentaCoroutine);
		
		this.frameRate = newFrameRate;
		Time.timeScale = frameRate;
	}
	void RalentaTo (float newFrameRate, float speedEveryFrame = 0.01f) {
		this.speedEveryFrame = speedEveryFrame;

		if (ralentaCoroutine != null)
			StopCoroutine (ralentaCoroutine);
		
		ralentaCoroutine = OnChangingSpeed (newFrameRate);
		StartCoroutine ( ralentaCoroutine );
	}
	IEnumerator OnChangingSpeed(float newFrameRate)
	{
		frameRate = Time.timeScale;
		float Resto = 0;
		if(newFrameRate<frameRate)
			Resto = -speedEveryFrame;
		else if(newFrameRate>frameRate)
			Resto = speedEveryFrame;
		while (Mathf.Abs (frameRate - newFrameRate) > 0.05f ) {
			
			frameRate += Resto;
			SetNewTimeScale (frameRate);

			if (Time.timeScale == 1 || Time.timeScale == 0) {
				break;
				yield return null;
			}
			
			yield return new WaitForSecondsRealtime(speedEveryFrame);
		}
		SetNewTimeScale (newFrameRate);
		yield return null;
	}
	void SetNewTimeScale(float newFrameRate)
	{
		if (newFrameRate > 1)
			newFrameRate = 1;
		else if (newFrameRate < 0)
			newFrameRate = 0;
		if (newFrameRate == 0) {
			if (ralentaCoroutine != null)
				StopCoroutine (ralentaCoroutine);
		}
		Time.timeScale = newFrameRate;
	}
}
