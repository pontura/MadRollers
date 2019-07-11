using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VideogamesData : MonoBehaviour {

	public int actualID;
	public VideogameData[] all;



	public void ChangeID(int id)
	{
		actualID = id;
	}
	public VideogameData GetActualVideogameData()
	{
		return all [actualID];
	}
	public VideogameData GetActualVideogameDataByID(int id)
	{
		return all [id];
	}
	public void SetOtherGameActive()
	{
		actualID++;
		if (actualID > all.Length-1)
			actualID = 0;
	}
	public void SetCredits()
	{
	}

}
