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
			Data.Instance.LoadLevel("MainMenuMobile");
		else {
			done = true;
			sentenceID = VoicesManager.Instance.intros.Count - 1;
			SetText(VoicesManager.Instance.intros[sentenceID].text);
            VoicesManager.Instance.PlayClip (VoicesManager.Instance.intros[sentenceID].audioClip);
			enrollButton.SetActive (true);
		}
	}
	void Next()
	{
		if (sentenceID == VoicesManager.Instance.intros.Count) {		
			done = true;
			enrollButton.SetActive (true);
			return;
		}
		SetText(VoicesManager.Instance.intros[sentenceID].text);
        VoicesManager.Instance.PlayClip (VoicesManager.Instance.intros[sentenceID].audioClip);
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
