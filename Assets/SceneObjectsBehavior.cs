using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneObjectsBehavior : MonoBehaviour
{

    AreaSceneObjectManager areaSceneObjectManager;
    SceneObjectsManager manager;
    public ArrayList unused = new ArrayList();
    public SceneObject Catapulta;
    public SceneObject Star;
    public SceneObject Water;
    public SceneObject Lava;
    public SceneObject Boss1;

    public SceneObject BossCreator;
    public SceneObject BossSpace1;
    public SceneObject BossCalecitas1;
    public SceneObject BossPacmans;
    public SceneObject BossGalaga;
    public SceneObject Starting;
    public SceneObject Ending;
    public SceneObject FloorSlider;
    public SceneObject FloorSurface;
    public SceneObject house1;
    public SceneObject house2;
    public SceneObject house3;
    public SceneObject house4;
    public SceneObject PisoPinche;
    public SceneObject rampa;
    public SceneObject rampaHuge;
    public SceneObject rampaSmall;
    public SceneObject bomb1;
    public SceneObject Laser;
    public SceneObject Container;
    public SceneObject enemyGhost;
    public SceneObject cilindro;
    public SceneObject cilindroBig;

    public SceneObject Fish;
    public SceneObject borde1;

    public SceneObject fences;

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
        areaSceneObjectManager = GetComponent<AreaSceneObjectManager>();
        manager = GetComponent<SceneObjectsManager>();
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
            if (go && go.name == name + "_real(Clone)")
            {
                unused.Remove(go);
                return go;
            }
        }
        return null;
    }
    private void resetGO(GameObject go)
    {
        go.GetComponentInChildren<Renderer>().enabled = true;
    }

    public void AddSceneObjects(AreaData areaData, float z_length)
    {
        id = 0;
        AddSingleSO(areaData, z_length);
      //  StartCoroutine(AddSingleSO(areaData, z_length));
    }
    int id;
   // IEnumerator AddSingleSO(AreaData areaData, float z_length)
    void AddSingleSO(AreaData areaData, float z_length)
    {
        foreach (AreaSceneObjectData go in areaData.data)
        {
            //if (id % 2 == 0)
               // yield return new WaitForEndOfFrame();
           // id++;

            // if (!canBeDisplayed(go)) 
            //	    continue;

            SceneObject sceneObject = null;
            Vector3 pos = go.pos;
            Vector3 rot = go.rot;
            pos.z += z_length;
            string goName = go.name; 
            switch (goName)
            {
                case "smallBlock1":
                case "extraSmallBlock1":
                    pos.y += 1;
                    sceneObject = SetSceneObject( Pool.GetObjectForType(goName + "_real", true), pos, rot);
                    break;

                case "palm":
                    string soName = goName;
                    int randNum = Random.Range(0, 3);
                    if (randNum == 1)
                        soName = "palm2";
                    else if (randNum == 2)
                        soName = "palm3";
                    sceneObject = SetSceneObject(Pool.GetObjectForType(goName + "_real", false), pos, rot);
                    break;

                case "extralargeBlock1":
                case "largeBlock1":
                case "mediumBlock1":
                    pos.y += 1;
                    sceneObject = SetSceneObject(Pool.GetObjectForType(goName + "_real", false), pos, rot);
                    break;

                case "Coin":
                case "bloodx1":
                    sceneObject = SetSceneObject(Pool.GetObjectForType(goName + "_real", false), pos, rot);
                    sceneObject.GetComponent<GrabbableItem>().SetComboGrabbable(z_length, areaData.totalCoins);//area.totalCoins);
                    break;

                case "LevelChanger":
                case "Dropper":
                case "Sapo":
                case "flyer":
              
                //case "Yuyo":
                case "enemyRunner":
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
                case "palmTall":
                case "palmSmall":
                    sceneObject = SetSceneObject(Pool.GetObjectForType(goName + "_real", false), pos, rot);
                    break;
                default:
                    sceneObject = SetOtherGO(goName, pos, go);
                    break;
            }
            if (sceneObject != null)
            {
                areaSceneObjectManager.AddComponentsToSceneObject(go, sceneObject.gameObject);
                //SceneObjectData soData = sceneObject.SoData;

                //if (soData != null)
                //{
                //    if (soData.random_pos_x != 0)
                //    {
                //        pos.x += Random.Range(-soData.random_pos_x, soData.random_pos_x);
                //    }
                //}
                if (lastSceneObjectContainer != null && go.isChild)
                    manager.AddSceneObject(sceneObject, pos, lastSceneObjectContainer);
                else
                    manager.AddSceneObject(sceneObject, pos);
            }
            if (goName == "Container")
            {
                lastSceneObjectContainer = sceneObject.transform;
            }
        }
    }
    SceneObject SetSceneObject(SceneObject so, Vector3 pos, Vector3 rot)
    {
        so.isActive = false;
        so.transform.position = pos;
        so.transform.localEulerAngles = rot;
        return so;
    }
    SceneObject SetOtherGO(string goName, Vector3 pos, AreaSceneObjectData go)
    {
        SceneObject clone = null;
        if (goName == "FloorSurface")
            clone = FloorSurface;
        if (goName == "PisoPinche")
            clone = PisoPinche;
        else if (goName == "Catapulta")
            clone = Catapulta;
        else if (goName == "house1")
            clone = house1;
        else if (goName == "house2")
            clone = house2;
        else if (goName == "house3")
            clone = house3;
        else if (goName == "house4")
            clone = house4;
        else if (goName == "rampa")
            clone = rampa;
        else if (goName == "rampaHuge")
            clone = rampaHuge;
        else if (goName == "rampaSmall")
            clone = rampaSmall;
        else if (goName == "wallBig")
        {
            //  addDecorationWithRotation("Graffiti_Real", pos, go.transform.localEulerAngles);
            clone = wallBig;
        }
        else if (goName == "wallMedium")
            clone = wallMedium;
        else if (goName == "wallSmall")
            clone = wallSmall;
        else if (goName == "wallSuperSmall")
            clone = wallSuperSmall;
        else if (goName == "jumper")
            clone = jumper;
        else if (goName == "Lava")
            clone = Lava;
        else if (goName == "Star")
            clone = Star;
        else if (goName == "Water")
            clone = Water;
        else if (goName == "Boss1")
            clone = Boss1;
        else if (goName == "BossCalecitas1")
            clone = BossCalecitas1;
        else if (goName == "BossCreator")
            clone = BossCreator;
        else if (goName == "BossSpace1")
            clone = BossSpace1;
        else if (goName == "BossPacmans")
            clone = BossPacmans;
        else if (goName == "BossGalaga")
            clone = BossGalaga;
        else if (goName == "Starting")
            clone = Starting;
        else if (goName == "Ending")
            clone = Ending;
        else if (goName == "bomb1")
        {
            clone = bomb1;
        }
        else if (goName == "Laser")
        {
            clone = Laser;
            Data.Instance.events.OnBossDropBomb();
        }
        else if (goName == "tunel1")
            clone = tunel1;
        else if (goName == "tunel2")
            clone = tunel2;
        else if (goName == "cilindro")
            clone = cilindro;
        else if (goName == "cilindroBig")
            clone = cilindroBig;
        else if (goName == "enemyGhost")
            clone = enemyGhost;
        else if (goName == "streetFloor")
            clone = streetFloor;
        else if (goName == "Container")
            clone = Container;
        else if (goName == "Fish")
            clone = Fish;
        else if (goName == "streetFloorSmall")
            clone = streetFloorSmall;
        else if (goName == "levelSignal")
            clone = levelSignal;
        else if (goName == "GrabbableItem")
            clone = GrabbableItem;
        else if (goName == "borde1")
            clone = borde1;
        else if (goName == "fences")
            clone = fences;
        else if (goName == "cruz")
            clone = cruz;
        else if (goName == "CruzGrande")
            clone = CruzGrande;
        else if (goName == "rueda1")
            clone = rueda1;
        else if (goName == "helice1")
            clone = helice1;
        else if (goName == "pisoRotatorio")
            clone = pisoRotatorio;
        else if (goName == "sombrilla")
            clone = sombrilla;
        else if (goName == "FloorSlider")
            clone = FloorSlider;

        if (clone)
        {
            SceneObject sceneObject = Instantiate(clone, pos, Quaternion.identity) as SceneObject;
            sceneObject.transform.localEulerAngles = go.rot;
            return sceneObject;
        }
        return null;
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
        manager.AddSceneObject(newSceneObject, pos);
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
        if (go.soData.Count > 0)
        {
            SceneObjectDataGeneric data = go.soData[0];
            if (data.minPayers > 0 && data.minPayers > Game.Instance.level.charactersManager.getTotalCharacters())
                return false;
        }
        return true;
    }

}
