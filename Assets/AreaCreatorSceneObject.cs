using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCreatorSceneObject : MonoBehaviour {

	public GameObject[] all;
	public AreaSceneObjectManager areaSceneObjectManager;

	public GameObject GetSceneObjectToAdd (AreaSceneObjectData areaSceneObjectData ) {
		foreach (GameObject go in all) {
			if (go.name == areaSceneObjectData.name)
			{
				GameObject newGo = Instantiate(go) as GameObject;
				newGo.name = newGo.name.Substring (0,newGo.name.Length-7);
				areaSceneObjectManager.AddComponentsToSceneObject (areaSceneObjectData, newGo);
				return newGo;
			}
		}
		Debug.Log ("No existe en AreaCreatorSceneObject el objeto " + areaSceneObjectData.name + ", agregarlo!");
		return null;
	}
}
