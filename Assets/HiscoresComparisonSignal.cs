using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiscoresComparisonSignal : MonoBehaviour {

	public Text teamNameField;
	public Text scoreField;
	public Text puestoField;

	public void Init(string teamName, int score, int puesto)
	{
		teamNameField.text = "";
		puestoField.text = "";
		scoreField.text = "";

		if (puesto > 0)
		{
			scoreField.text = Utils.FormatNumbers(score);
			teamNameField.text = teamName;
			SetPuesto(puesto);
		}
	}
	public void SetPuesto(int puesto)
	{
		if (puesto == 0)
			puestoField.text = "";
		else
			puestoField.text = "PUESTO " + puesto;
	}
}
