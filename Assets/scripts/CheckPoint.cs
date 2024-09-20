using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

	private int activeAreaSetID;
	private Data data;

	void Start () {	

//		Missions missions = GameObject.Find("Game").GetComponent<Missions>();
//		AreasManager areasManager = missions.getAreasManager();
//		activeAreaSetID = areasManager.activeAreaSetID;
//		areasManager = null;
//		data = GameObject.Find("data").GetComponent<Data>();
	}
	
	//void OnTriggerEnter (Collider other) {
//		if(other.gameObject.CompareTag("Player"))
//		{
//			other.gameObject.SendMessage("Walk");
//			Gui gui = GameObject.Find("GUI").GetComponent<Gui>();
//			data.setMission(activeAreaSetID+1);
//			data.setEnergy(gui.energy);
//		}
	//}
}
