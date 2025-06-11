using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideogameFilter : MonoBehaviour {

	public GameObject[] only_show_in_videogame_1;
	public GameObject[] only_show_in_videogame_2;
	public GameObject[] only_show_in_videogame_3;
	int lastVideogame = 0;

	void OnEnable () {
		int id = Data.Instance.videogamesData.GetActualVideogameData ().id;
		if (lastVideogame == id) return;
		lastVideogame = id;

        if (id == 0) {
			SetEverythingIn (only_show_in_videogame_1, true);
			SetEverythingIn (only_show_in_videogame_2, false);
			SetEverythingIn (only_show_in_videogame_3, false);
		} else if (id == 1) {
			SetEverythingIn (only_show_in_videogame_2, true);
			SetEverythingIn (only_show_in_videogame_1, false);
			SetEverythingIn (only_show_in_videogame_3, false);
		} else
        {
            SetEverythingIn (only_show_in_videogame_3, true);
            SetEverythingIn (only_show_in_videogame_2, false);
			SetEverythingIn (only_show_in_videogame_1, false);
		}
	}
	void SetEverythingIn(GameObject[] arr, bool showIt)
	{
		foreach (GameObject go in arr)
			go.SetActive (showIt );
	}
}
