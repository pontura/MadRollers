using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideogameLights : MonoBehaviour {

	public GameObject boyland;
	public GameObject gallax;
	public GameObject inferno;

	void Start () {
		boyland.SetActive (false);
		gallax.SetActive (false);
		inferno.SetActive (false);

		switch (Data.Instance.videogamesData.actualID) {
			case 0:
			boyland.SetActive (true);
				break;
			case 1:
			gallax.SetActive (true);
				break;
			case 2:
			inferno.SetActive (true);
				break;
		}
	}
}
