using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandWriting : MonoBehaviour {
	
	float speed = 0.05f;

	public void WriteTo(Text field, string textToWrite,  System.Action OnReadyFunc)
	{
        StopAllCoroutines();
		field.text = "";
		StartCoroutine (WriteLoop (field, textToWrite, OnReadyFunc));
	}
	IEnumerator WriteLoop(Text field, string textToWrite,  System.Action OnReadyFunc)
	{
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
		if(OnReadyFunc != null)
			OnReadyFunc ();
		yield return null;
	}
	void OnDestroy()
	{
		StopAllCoroutines ();
	}
	void OnDisable()
	{
		StopAllCoroutines ();
	}
}
