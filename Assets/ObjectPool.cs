using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[AddComponentMenu("Gameplay/ObjectPool")]
public class ObjectPool : MonoBehaviour
{
    #region member
    [Serializable]
    public class ObjectPoolEntry
    {
        [SerializeField]
        public SceneObject Prefab;

        [SerializeField]
        public int Count;
    }
    #endregion
    public ObjectPoolEntry[] Entries;
    [HideInInspector]
    public GameObject Scene;

    public static ObjectPool instance;
    
	List<SceneObject> pooledObjects;
    public List<SceneObject> pooledObjects_smallBlock;
    public List<SceneObject> pooledObjects_extraSmallBlock;

    protected GameObject containerObject;

	public PixelsPool pixelsPool;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
		
		pixelsPool = GetComponent<PixelsPool> ();
		pooledObjects = new List<SceneObject> ();
		pooledObjects_smallBlock = new List<SceneObject> ();
		pooledObjects_extraSmallBlock = new List<SceneObject> ();

        DontDestroyOnLoad(this);

        containerObject = new GameObject("ObjectPool");
        Scene = new GameObject("Scene");

		pixelsPool.Init (containerObject.transform, Scene.transform);

        DontDestroyOnLoad(containerObject);
        DontDestroyOnLoad(Scene);
        
//		foreach (ObjectPoolEntry poe in Entries) {
//			for (int a=0; a<poe.Count; a++) {
//				pooledObjects.Add (poe.Prefab);
//			}
//		}


        int id = 0;
        for (int i = 0; i < Entries.Length; i++)
        {
            var objectPrefab = Entries[i];
            //create the repository


            for (int n = 0; n < objectPrefab.Count; n++)
            {
				SceneObject newObj = CreateSceneObject (objectPrefab.Prefab);
				newObj.gameObject.SetActive(false);
//                SceneObject newObj = Instantiate(objectPrefab.Prefab) as SceneObject;
//				pooledObjects.Add (newObj);
//                newObj.name = objectPrefab.Prefab.name;
				PoolObject(newObj);
                newObj.id = id;
                id++;

            }
        }
        Restart();
        
    }
	SceneObject CreateSceneObject(SceneObject so)
	{
        print(so.name);
		SceneObject newSO = Instantiate(so) as SceneObject;
		newSO.name = so.name;
		return newSO;
	}
    void Restart()
    {

    }
    private int GetTotalObjectsOfType(string objectType)
    {
        int qty = 0;
        foreach (SceneObject so in containerObject.GetComponentsInChildren<SceneObject>())
        {
            if (so.name == objectType)
            {
                qty++;
            }
        }
        return qty;
    }


    public SceneObject GetObjectForType(string objectType, bool onlyPooled)
    {

		SceneObject pooledObject = null;
		if (objectType == "extraSmallBlock1_real") {
			if (pooledObjects_extraSmallBlock.Count > 0) {
				pooledObject = pooledObjects_extraSmallBlock [0];
				pooledObjects_extraSmallBlock.Remove (pooledObject);	
				pooledObject.transform.SetParent (Scene.transform);
				return pooledObject;
			}
		} else if (objectType == "smallBlock1_real") {
			if (pooledObjects_smallBlock.Count > 0) {
				pooledObject = pooledObjects_smallBlock [0];
				pooledObjects_smallBlock.Remove (pooledObject);	
				pooledObject.transform.SetParent (Scene.transform);
				return pooledObject;
			}
		} else {
			
			int i = pooledObjects.Count;
			while (i > 0) {
				SceneObject soPooled = pooledObjects [i - 1];
				if (soPooled.name == objectType || soPooled.name + "(Clone)" == objectType) {
					pooledObject = soPooled;
					pooledObject.transform.SetParent (Scene.transform);
					pooledObjects.Remove (pooledObject);	
					return pooledObject;
				}
				i--;
			}	
		}
		if (!onlyPooled)
		{
			foreach (ObjectPoolEntry poe in Entries) {
				if (poe.Prefab.name == objectType || poe.Prefab.name + "(Clone)" == objectType) {	
					SceneObject newSceneObject = CreateSceneObject(poe.Prefab);
					newSceneObject.transform.SetParent( Scene.transform );
					//pooledObjects.Remove(newSceneObject);
					return newSceneObject;
				}
			}
		} 
		//Debug.LogError("_________________NO HAY: " + objectType + "  bool " + onlyPooled);
		return null;
    }




    public void PoolObject(SceneObject obj)
    {
		if (obj.broken) {
			//print ("____________broken " + obj.name);
			Destroy (obj.gameObject);
			return;
		}
        for (int i = 0; i < Entries.Length; i++)
        {
            if (Entries[i].Prefab.name == obj.name || Entries[i].Prefab.name + "(Clone)" == obj.name)
            {        				
				obj.transform.SetParent(containerObject.transform);
				obj.gameObject.SetActive(false);
				if(obj.name == "extraSmallBlock1_real")
					pooledObjects_extraSmallBlock.Add(obj);
				else if(obj.name == "smallBlock1_real")
					pooledObjects_smallBlock.Add(obj);
				else
                	pooledObjects.Add(obj);
                return;
            }
        }
        Destroy(obj.gameObject);
    }



}