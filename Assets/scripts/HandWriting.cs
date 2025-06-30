using System.Collections;
using UnityEngine;

public class HandWriting : MonoBehaviour {
	
	float speed = 0.05f;
    private void Start()
    {
         Data.Instance.events.OnGameOver += OnGameOver;
    }
    void OnDestroy()
    {
        StopAllCoroutines();
        Data.Instance.events.OnGameOver -= OnGameOver;
    }
    void OnGameOver(bool isOn)
    {
        StopAllCoroutines();
        if (field != null)
            field.text = "";
        field = null;
    }
    TMPro.TMP_Text field;
    public void WriteTo(TMPro.TMP_Text field, string textToWrite,  System.Action OnReadyFunc)
	{
        
        Data.Instance.events.ResetHandwritingText();
        
        this.field = field;
        StopAllCoroutines();
		field.text = "";
		StartCoroutine (WriteLoop (textToWrite, OnReadyFunc));
	}
	IEnumerator WriteLoop(string textToWrite,  System.Action OnReadyFunc)
	{
        if (field == null)
        {
            yield return null;
            StopAllCoroutines();
        }
        Data.Instance.events.OnSoundFX("typing", -1);
        field.text = ">";
		int letterId = 0;
		int totalWords = textToWrite.Length;
		while (letterId < totalWords) {		
			if (field == null) {
				yield return null;
				StopAllCoroutines ();
			}	
			if (field != null) {
				field.text = field.text.Remove (field.text.Length - 1, 1);
				field.text += textToWrite [letterId] + "_";
				letterId++;
				yield return new WaitForSeconds (speed);
			}
		}
        Data.Instance.events.OnSoundFX("", -1);
        if (OnReadyFunc != null)
			OnReadyFunc ();
		yield return null;
	}
	void OnDisable()
	{
        field = null;
		StopAllCoroutines ();
	}
}
