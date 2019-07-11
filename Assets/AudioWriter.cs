using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioWriter : MonoBehaviour {

	public GameObject enrollButton;

	public Text field;
	public float speed = 0.1f;

	int sentenceID = 0;
	int letterId = 0;
	int totalWords;
	string sentence;
	bool done;

	void Start () {
		enrollButton.SetActive (false);
		Next ();
	}
	public void Done()
	{
		CancelInvoke ();
		if(done)
			Data.Instance.LoadLevel("MainMenu");
		else {
			done = true;
			sentenceID = Data.Instance.voicesManager.intros.Count - 1;
			SetText(Data.Instance.voicesManager.intros[sentenceID].text);
			Data.Instance.voicesManager.PlayClip (Data.Instance.voicesManager.intros[sentenceID].audioClip);
			enrollButton.SetActive (true);
		}
	}
	void Next()
	{
		if (sentenceID == Data.Instance.voicesManager.intros.Count) {		
			done = true;
			enrollButton.SetActive (true);
			return;
		}
		SetText(Data.Instance.voicesManager.intros[sentenceID].text);
		Data.Instance.voicesManager.PlayClip (Data.Instance.voicesManager.intros[sentenceID].audioClip);
		sentenceID++;
	}
	void SetText (string sentence) {
		this.sentence = sentence;
		field.text = ">";
		letterId = 0;
		totalWords = sentence.Length;
		WriteLoop ();
	}
	void WriteLoop()
	{
		if (letterId == totalWords) {
			if (!done) {
				field.text = field.text.Remove (field.text.Length - 1, 1);
				ChangeSentence ();
			} else {
				field.text = "";
			}
			return;
		}
		field.text = field.text.Remove (field.text.Length-1,1);
		field.text += sentence [letterId] + "_";
		letterId++;
		Invoke ("WriteLoop", speed);
	}
	void ChangeSentence()
	{
		Invoke ("Next", 3);
	}

}
