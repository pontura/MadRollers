using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VideogamesData : MonoBehaviour {

	public int actualID;
	public VideogameData[] all;


	public VideogameData GetActualVideogameData()
	{
		return all [actualID];
	}
	public VideogameData GetActualVideogameDataByID(int id)
    {
        return all [id];
	}
	public void UpdateVideogame()
	{
		int missionID = Data.Instance.missions.MissionActiveID;
        actualID = MissionsManager.Instance.missions[missionID].data[0].videoGameID;
    }
	//public void SetOtherGameActive()
	//{
	//	actualID++;
	//	if (actualID > all.Length-1)
	//		actualID = 0;
 //   }
    public void Reset()
    {
        actualID = 0;
    }
    public void SetCredits()
	{

	}

}
