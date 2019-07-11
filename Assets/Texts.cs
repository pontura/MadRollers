using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Texts : MonoBehaviour {
	
	public GenericTexts genericTexts;
	public VideogameDataJson videoGamesDataJson;

	[Serializable]
	public class VideogameDataJson
	{
		public List<VideogameData> videogames;
	}

	[Serializable]
	public class GenericTexts
	{
		public string gameOver;
		public string mission;
		public string newCredit;
	}

	void Start () {
		LoadGenericTexts ();
		LoadCredits ();
	}
	private void LoadGenericTexts()
	{
		string filePath = Application.streamingAssetsPath + "/texts/texts.json";
		if (File.Exists (filePath)) {
			string dataAsJson = File.ReadAllText (filePath);
			genericTexts = JsonUtility.FromJson<GenericTexts> (dataAsJson);
		}
	}
	private void LoadCredits()
	{
		string filePath = Application.streamingAssetsPath + "/texts/credits.json";

		if (File.Exists (filePath)) {
			string dataAsJson = File.ReadAllText (filePath);
			videoGamesDataJson = JsonUtility.FromJson<VideogameDataJson> (dataAsJson);
			VideogamesData videoGamesData = GetComponent<VideogamesData> ();
			int a = 0;
			foreach (VideogameData data in videoGamesData.all) {
				data.name = videoGamesDataJson.videogames [a].name;
				data.credits = videoGamesDataJson.videogames [a].credits;
				a++;
			}
		}
	}
}
