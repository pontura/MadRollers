using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectorUI : MonoBehaviour {

	public GameObject panel;
	public Text field;
	public int id;
	public int uiID;
	public PlayerMainMenuUI ui;
	public bool moveing;
	Vector3 newPos;

	public void Init(int id)
	{
		this.uiID = uiID;
		this.id = id;
		field.text = "P" + ((int)id+1).ToString();
	}
	public void SetUI(PlayerMainMenuUI ui)
	{
		if (moveing)
			return;
		moveing = true;
		this.ui = ui;
		uiID = ui.id;
		newPos = ui.transform.localPosition;
		Invoke ("Reset", 0.3f);
	}
	void Update()
	{
		if (moveing)
			transform.localPosition = Vector3.Lerp (transform.localPosition, newPos, 0.2f);
	}
	void Reset()
	{
		moveing = false;
	}
	public void SetOff()
	{
		panel.SetActive (false);
	}

}
