using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour {

	public CreditIcon creditIcon;
	public Transform container;
	public GameObject newCreditPanel;
	public Text field;

	void Start () {

		newCreditPanel.SetActive (false);

		if (Data.Instance.playMode != Data.PlayModes.PARTYMODE)
			return;
		
		Data.Instance.events.AddNewCredit += AddNewCredit;

		int totalCredits = Data.Instance.credits;
		for (int a = 0; a < totalCredits; a++) {
			AddCredit ();
		}
	}
	void OnDestroy()
	{
		Data.Instance.events.AddNewCredit -= AddNewCredit;
	}
	void AddNewCredit()
	{
		Data.Instance.voicesManager.PlaySpecificClipFromList (Data.Instance.voicesManager.UIItems, 3);
		Data.Instance.credits++;
		newCreditPanel.SetActive (true);
		field.text = Data.Instance.texts.genericTexts.newCredit;
		AddCredit ();
		StartCoroutine (ClosePanel ());
	}
	IEnumerator ClosePanel()
	{
		yield return new WaitForSeconds (2);
		newCreditPanel.SetActive (false);
	}
	void AddCredit()
	{
		CreditIcon go = Instantiate (creditIcon);
		go.transform.SetParent (container);
		go.transform.localScale = new Vector3 (0.6f,0.6f,0.6f);
	}
	public void RemoveOne()
	{
		CreditIcon[] all = container.GetComponentsInChildren<CreditIcon> ();
		if(all.Length>0)
			all [all.Length - 1].SetOff ();
	}
}
