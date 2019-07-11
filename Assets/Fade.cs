using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fade : MonoBehaviour
{
    public Image masker;
 //   public GraphicRaycaster graphicRaycaster;

    private string m_LevelName = "";
    private int m_LevelIndex = 0;    
    private bool fading;

    private void Start()
    {
        masker.enabled = true;
        masker.color = new Color(0, 0, 0, 0);
        Data.Instance.events.OnFadeALittle += OnFadeALittle;
    }
	public void FadeOut()
	{
		StartCoroutine(FadeOutCoroutine(0.5f, 0.025f, Color.black));
	}
	IEnumerator FadeOutCoroutine(float delay, float aFadeTime, Color aColor)
	{
		masker.gameObject.SetActive(true);
		masker.color = new Color(0, 0, 0, 1);
		yield return new WaitForSeconds(delay);
		masker.gameObject.SetActive(true);
		aFadeTime /= 50;
		float t = 1;
		while (t > 0)
		{
			yield return new WaitForEndOfFrame();
			t -= Time.deltaTime + aFadeTime;
			masker.color = new Color(0, 0, 0, t);
		}
		masker.gameObject.SetActive(false);
	}
	public void FadeToBlack()
	{
		StartCoroutine(FadeCoroutine(0.05f, Color.black));
	}
	IEnumerator FadeCoroutine(float aFadeTime, Color aColor)
	{
		masker.gameObject.SetActive(true);
		aFadeTime /= 10;
		float t = 0;
		while (t < 1)
		{
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime + aFadeTime;
			masker.color = new Color(0, 0, 0, t);
		}
	}
    void OnFadeALittle(bool fadeIn)
    {
        if (fadeIn)
            StartCoroutine(FadeALittleIn(0.05f, Color.black));
        else
            StartCoroutine(FadeALittleOut(0.05f, Color.black));
    }
    public IEnumerator FadeALittleIn(float aFadeTime, Color aColor)
    {
        masker.gameObject.SetActive(true);
        aFadeTime /= 10;
        float t = 0;
        while (t < 0.7f)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime + aFadeTime;
            masker.color = new Color(0, 0, 0, t);
        }
    }
    IEnumerator FadeALittleOut(float aFadeTime, Color aColor)
    {
        masker.gameObject.SetActive(true);
        aFadeTime /= 10;
        float t = 0.7f;
        while (t >= 0.01f)
        {
            yield return new WaitForEndOfFrame();
            t -= Time.deltaTime + aFadeTime;
            masker.color = new Color(0, 0, 0, t);
        }
        if(t<=0.02f)
            masker.gameObject.SetActive(false);
    }
    IEnumerator FadeStart(float aFadeTime, Color aColor)
    {		
        masker.gameObject.SetActive(true);
        aFadeTime /= 10;
        float t = 0;
        while (t < 1)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime + aFadeTime;
            masker.color = new Color(0, 0, 0, t);
        }

        if (m_LevelName != "")
            Application.LoadLevel(m_LevelName);
        else
            Application.LoadLevel(m_LevelIndex);

		if (m_LevelName == "Game") {
			yield return new WaitForSeconds (0.5f);
		} else {
			while (t > 0f) {
				yield return new WaitForEndOfFrame ();
				t -= Time.deltaTime + aFadeTime;
				masker.color = new Color (0, 0, 0, t);
			}
			fading = false;
			masker.gameObject.SetActive(false);
			Data.Instance.LoadingReady ();
		}    
		yield return null;
    }
	void StartFade(float aFadeTime, Color aColor)
    {
        fading = true;
        StartCoroutine(FadeStart( aFadeTime, aColor));
    }

    public void LoadLevel(string aLevelName, float aFadeTime, Color aColor)
    {
		StopAllCoroutines ();
		fading = false;
		print ("load: " + aLevelName);
        if (fading) return;
        m_LevelName = aLevelName;
        StartFade(aFadeTime, aColor);
    }
	public void LoadSceneNotFading(string aLevelName)
	{
		m_LevelName = aLevelName;
		StartCoroutine(FadeStartOutOnly( 0.6f, Color.black));
	}
	IEnumerator FadeStartOutOnly(float aFadeTime, Color aColor)
	{
		float t = 1;
		if (m_LevelName != "")
			Application.LoadLevel(m_LevelName);
		else
			Application.LoadLevel(m_LevelIndex);

		while (t > 0f)
		{
			yield return new WaitForEndOfFrame();
			t -= Time.deltaTime + aFadeTime;
			masker.color = new Color(0, 0, 0, t);
		}
		fading = false;
		masker.gameObject.SetActive(false);

	}
    
}