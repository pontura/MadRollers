using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneObjectsBehavior : MonoBehaviour {

	AreaSceneObjectManager areaSceneObjectManager;
	SceneObjectsManager manager;
	public ArrayList unused = new ArrayList();
	public SceneObject Catapulta;
	public SceneObject Star;
	public SceneObject Water;
	public SceneObject Lava;
	public SceneObject Boss1;
	public SceneObject Boss2;
	public SceneObject BossCreator;
	public SceneObject BossSpace1;
	public SceneObject BossCalecitas1;
	public SceneObject BossPacmans;
	public SceneObject BossGalaga;
	public SceneObject Starting;
	public SceneObject FloorSlider;
	public SceneObject FloorSurface;
	public SceneObject house1;
	public SceneObject house2;
	public SceneObject house3;
	public SceneObject house4;
	public SceneObject PisoPinche;
	public SceneObject rampa;
	public SceneObject rampaHuge;
	public SceneObject bomb1;
	public SceneObject Laser;
	public SceneObject Container;
	public SceneObject enemyGhost;
	public SceneObject cilindro;
	public SceneObject cilindroBig;

	public SceneObject Fish;
	public SceneObject borde1;

	public SceneObject fences;
	public SceneObject Listener;

	public SceneObject tunel1;
	public SceneObject tunel2;
	public SceneObject jumper;

	public SceneObject cruz;
	public SceneObject CruzGrande;
	public SceneObject rueda1;
	public SceneObject helice1;
	public SceneObject levelSignal;
	public SceneObject streetFloor;
	public SceneObject streetFloorSmall;
	public SceneObject pisoRotatorio;
	public SceneObject wallBig;
	public SceneObject wallMedium;
	public SceneObject wallSmall;
	public SceneObject wallSuperSmall;
	public SceneObject sombrilla;
	public SceneObject GrabbableItem;

	public Game game;
	private ObjectPool Pool;

	private void Awake()
	{
		areaSceneObjectManager = GetComponent<AreaSceneObjectManager> ();
		manager = GetComponent<SceneObjectsManager> ();
		Pool = Data.Instance.sceneObjectsPool;
	}
	public void Add(GameObject go)
	{
		go.transform.parent = transform;
		unused.Add(go);
	}
	public GameObject GetUnusedObject(string name)
	{
		foreach (GameObject go in unused)
		{
			if(go && go.name == name + "_real(Clone)")
			{
				unused.Remove(go);
				return go;
			}
		}
		return null;
	}
	private void resetGO(GameObject go) {
		go.GetComponentInChildren<Renderer>().enabled = true;
	}

	public void AddSceneObjects(AreaData areaData, float z_length)
	{
		//print (area.name + " AREA");
		bool nubesOn = false;

		foreach (AreaSceneObjectData go in areaData.data)
		{
			if (!canBeDisplayed(go)) 
				continue;
			SceneObject sceneObject = null;
			Vector3 pos = go.pos;
			pos.z += z_length;
//			pos.x += areasX;
//			if (oposite) {
//				pos.z *= -1;
//			}
			//  if (!nubesOn)
			//  {
			//  nubesOn = true;
			//   addDecoration("Nubes_real", pos, new Vector3(0, Random.Range(0,2), 5));

			//  }

			switch (go.name)
			{
			case "LevelChanger":
			case "Dropper":
			case "Sapo":
			case "extralargeBlock1":
			case "flyer":
			case "largeBlock1":
			case "mediumBlock1":
			case "smallBlock1":
			case "extraSmallBlock1":
			case "Coin":
			case "bloodx1":
			//case "Yuyo":
			case "enemyFrontal":   
			case "enemyShooter": 
			case "enemyWater":   
			case "enemySide":  
			case "ExplotionEffectBoss": 
			case "enemyBack":  
			case "castle":
			case "BossPartMarker":
			case "SideMountain":
			case "bonusEntrance":   
			case "Cascade": 
			case "firewall":        
			case "Baranda1":  
			case "Ray":
            case "Special3":
            case "enemyNaveSimple":  
			case "BichoVuela":
			case "palm":
			case "palmTall":
				if (go.name == "extralargeBlock1" || go.name == "largeBlock1")
					pos.y += (float)Random.Range (-10, 10) / 1000;
				if (go.name == "smallBlock1" || go.name == "extraSmallBlock1")
					sceneObject = Pool.GetObjectForType (go.name + "_real", true);
				else {
					if (go.name == "palm") {
						string soName = go.name;
						int randNum = Random.Range (0, 3);
						if (randNum == 1)
							soName = "palm2";
						else if (randNum == 2)
							soName = "palm3";
						sceneObject = Pool.GetObjectForType (soName + "_real", false);  
					} else
						sceneObject = Pool.GetObjectForType (go.name + "_real", false);  
				}

				if (sceneObject)
				{
					sceneObject.isActive = false;
					sceneObject.transform.position = pos;
					sceneObject.transform.localEulerAngles = go.rot;

					if(go.name == "Coin" || go.name =="bloodx1")
					{
						//print (z_length + "       total coins   " +  areaData.totalCoins);
						sceneObject.GetComponent<GrabbableItem> ().SetComboGrabbable (z_length, areaData.totalCoins);//area.totalCoins);
					} 
					//else if (go.GetComponent<DecorationManager>())
//					{
//						addDecoration("Baranda1_real", pos, new Vector3(5.5f, 0, 3));
//						addDecoration("Baranda1_real", pos, new Vector3(-5.5f, 0, 3));
//						addDecoration("Baranda1_real", pos, new Vector3(5.5f, 0, -3));
//						addDecoration("Baranda1_real", pos, new Vector3(-5.5f, 0, -3));
//					}


				}
				else
				{
					Debug.LogError("___________NO EXISTIO EL OBJETO: " + go.name);
					//Data.Instance.events.ForceFrameRate (0);
				}
				break;
			}




			SceneObject clone = null;


			if (go.name == "FloorSurface")
				clone = FloorSurface;
			if (go.name == "PisoPinche")
				clone = PisoPinche;			
			else if (go.name == "Catapulta")
				clone = Catapulta;
			else if (go.name == "house1")
				clone = house1;
			else if (go.name == "house2")
				clone = house2;
			else if (go.name == "house3")
				clone = house3;
			else if (go.name == "house4")
				clone = house4;
			else if (go.name == "rampa")
				clone = rampa;
			else if (go.name == "rampaHuge")
				clone = rampaHuge;
			else if (go.name == "wallBig") {
				//  addDecorationWithRotation("Graffiti_Real", pos, go.transform.localEulerAngles);
				clone = wallBig;
			} else if (go.name == "wallMedium")
				clone = wallMedium;
			else if (go.name == "wallSmall")
				clone = wallSmall;
			else if (go.name == "wallSuperSmall")
				clone = wallSuperSmall;
			else if (go.name == "jumper")
				clone = jumper;
			else if (go.name == "Lava")
				clone = Lava;
			else if (go.name == "Star")
				clone = Star;
			else if (go.name == "Water")
				clone = Water;
			else if (go.name == "Boss1")
				clone = Boss1;
			else if (go.name == "Boss2")
				clone = Boss2;
			else if (go.name == "BossCalecitas1")
				clone = BossCalecitas1;
			else if (go.name == "BossCreator")
				clone = BossCreator;
			else if (go.name == "BossSpace1")
				clone = BossSpace1;
			else if (go.name == "BossPacmans")
				clone = BossPacmans;
			else if (go.name == "BossGalaga")
				clone = BossGalaga;
			else if (go.name == "Starting")
				clone = Starting;
			else if (go.name == "bomb1") {
				Data.Instance.events.OnBossDropBomb ();
				clone = bomb1;
			} else if (go.name == "Laser") {
				clone = Laser;
				Data.Instance.events.OnBossDropBomb ();
			}
			else if (go.name == "tunel1")
				clone = tunel1;
			else if (go.name == "tunel2")
				clone = tunel2;
			else if (go.name == "cilindro")
				clone = cilindro;
			else if (go.name == "cilindroBig")
				clone = cilindroBig;
			else if (go.name == "enemyGhost")
				clone = enemyGhost;
			else if (go.name == "streetFloor")
				clone = streetFloor;
			else if (go.name == "Container")
				clone = Container;
			else if (go.name == "Fish")
				clone = Fish;
			else if (go.name == "streetFloorSmall")
				clone = streetFloorSmall;
			else if (go.name == "levelSignal")
				clone = levelSignal;
			else if (go.name == "GrabbableItem")
				clone = GrabbableItem;
			else if (go.name == "borde1")
				clone = borde1;
			else if (go.name == "fences")
				clone = fences;
			else if (go.name == "Listener")
			{
				clone = Listener;
			}
			else if (go.name == "cruz")
				clone = cruz;
			else if (go.name == "CruzGrande")
				clone = CruzGrande;
			else if (go.name == "rueda1")
				clone = rueda1;
			else if (go.name == "helice1")
				clone = helice1;
			else if (go.name == "pisoRotatorio")
				clone = pisoRotatorio;
			else if (go.name == "sombrilla")
				clone = sombrilla;
			else if (go.name == "FloorSlider")
				clone = FloorSlider;

			if (clone)
			{
				sceneObject = Instantiate(clone, pos, Quaternion.identity) as SceneObject;
				sceneObject.transform.localEulerAngles = go.rot;
			}			

			if (sceneObject != null) {
				areaSceneObjectManager.AddComponentsToSceneObject (go, sceneObject.gameObject);
				SceneObjectData soData = sceneObject.GetComponent<SceneObjectData> ();

				if (soData != null )
				{
					if (soData.random_pos_x != 0) {
						pos.x += Random.Range (-soData.random_pos_x, soData.random_pos_x);
					}
				}
				if (lastSceneObjectContainer != null && go.isChild)
					manager.AddSceneObject (sceneObject, pos, lastSceneObjectContainer);
				else
					manager.AddSceneObject (sceneObject, pos);
				
			}// else
			//	Debug.Log (go.name + "_______________ (No existe) " );
			

			if (go.name == "Container") {
				lastSceneObjectContainer = sceneObject.transform;
			}
		}
	}
	Transform lastSceneObjectContainer;

	Component CopyComponent(Component original, GameObject destination)
	{
		System.Type type = original.GetType();
		Component copy = destination.AddComponent(type);
		System.Reflection.FieldInfo[] fields = type.GetFields();
		foreach (System.Reflection.FieldInfo field in fields)
		{
			field.SetValue(copy, field.GetValue(original));
		}
		return copy;
	}

	public void addDecoration(string name, Vector3 pos, Vector3 offset)
	{
		SceneObject newSceneObject = Pool.GetObjectForType(name, true);
		if (newSceneObject == null)
			return;
		pos.z += offset.z;
		pos.x += offset.x;
		manager.AddSceneObject (newSceneObject, pos);
	}

	public void deleteAll()
	{
		GameObject[] objects = GameObject.FindGameObjectsWithTag("sceneObject");
		foreach (var go in objects)
		{
			Destroy(go);
		}
	}

	bool canBeDisplayed(AreaSceneObjectData go)
	{
		if (go == null)
			return false;
		if (go.soData.Count > 0) {
			SceneObjectDataGeneric data = go.soData [0];
			if (data.minPayers > 0 && data.minPayers > Game.Instance.level.charactersManager.getTotalCharacters ())
				return false;
		}
		return true;
	}

}
