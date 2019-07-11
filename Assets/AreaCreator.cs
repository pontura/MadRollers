using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class AreaCreator : MonoBehaviour {

	AreaSceneObjectManager areaSceneobjectManager;
	public AreaData areaData;

	public List<AreaSceneObjectData> data;
	public AreaCreatorSceneObject areaCreatorSceneObject;

	GameObject lastSceneObjectContainer;
	public void AddSceneObjectsToNewArea(string areaName, AreaData areaData, float _length)
	{
		GameObject newArea = new GameObject ();
		Area area = newArea.AddComponent<Area> ();
		area.z_length = areaData.z_length;
		area.totalCoins = areaData.totalCoins;
		newArea.transform.SetParent (transform);
		newArea.name = areaName;
		foreach (AreaSceneObjectData areaSceneObjectData in areaData.data) {
			GameObject go = areaCreatorSceneObject.GetSceneObjectToAdd (areaSceneObjectData);
			if (go != null) {

				if (lastSceneObjectContainer != null && areaSceneObjectData.isChild)
					go.transform.SetParent (lastSceneObjectContainer.transform);
				else
					go.transform.SetParent (newArea.transform);
				

				if (go.name == "Container") {
					lastSceneObjectContainer = go;
				}
				



				go.transform.position = areaSceneObjectData.pos;
				go.transform.eulerAngles = areaSceneObjectData.rot;
			}
		}
		newArea.transform.localPosition = new Vector3 (0, 0, _length);
	}
	public void CreateData(Area areaReal)
	{
		areaSceneobjectManager = GetComponent<AreaSceneObjectManager> ();
		GameObject area = areaReal.gameObject;
		data.Clear ();
		int totalCoins = 0;
		foreach (Transform t in area.GetComponentsInChildren<Transform>()) {
			if (t.tag == "sceneObject") {
				AddSceneObjectToFile (t.gameObject);
				if (t.name == "Coin" || t.name == "bloodx1")
					totalCoins++;
			}

		}
		var a = new AreaData { data = data };
		a.totalCoins = totalCoins;
		a.z_length = areaReal.z_length;
		string json = JsonUtility.ToJson (a);
		using (FileStream fs = new FileStream ("Assets/Resources/areas/" + area.name + ".json", FileMode.Create)) {
			using (StreamWriter writer = new StreamWriter (fs)) {
				writer.Write (json);
			}
		}
	}
	void AddSceneObjectToFile(GameObject go)
	{
		AreaSceneObjectData newSOdata = new AreaSceneObjectData ();
		if ( go.transform.parent.tag == "sceneObject")
			newSOdata.isChild = true;
		if (go.name.Contains ("(")) {
			string[] arr = go.name.Split (" (" [0]);
			go.name = arr [0];
		}
		newSOdata.name = go.name;
		areaSceneobjectManager.AddComponentsToJson (newSOdata, go);

		newSOdata.pos.x = float.Parse(go.transform.position.x.ToString("F2"));
		newSOdata.pos.y = float.Parse(go.transform.position.y.ToString("F2"));
		newSOdata.pos.z = float.Parse(go.transform.position.z.ToString("F2"));

		newSOdata.rot = go.transform.eulerAngles;

		data.Add (newSOdata);
	}
}
