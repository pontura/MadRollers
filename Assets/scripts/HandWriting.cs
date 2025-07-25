using System.Collections;
using UnityEngine;

public class HandWriting : MonoBehaviour {
	
	float speed = 0.05f;
    private void Start()
    {
         Events.OnGameOver += OnGameOver;
    }
    void OnDestroy()
    {
        StopAllCoroutines();
        Events.OnGameOver -= OnGameOver;
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
        
        Events.ResetHandwritingText();
        
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
        Events.OnSoundFX("typing");
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
        Events.OnSoundFX("");
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
