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
    public GameObject Scene, containerObject;

    public static ObjectPool instance;

    private Dictionary<string, List<SceneObject>> pool = new Dictionary<string, List<SceneObject>>();

    public PixelsPool pixelsPool;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pixelsPool = GetComponent<PixelsPool>();
        DontDestroyOnLoad(this);

        containerObject = new GameObject("ObjectPool");
        Scene = new GameObject("Scene");

        pixelsPool.Init(containerObject.transform, Scene.transform);

        DontDestroyOnLoad(containerObject);
        DontDestroyOnLoad(Scene);

        pool = new Dictionary<string, List<SceneObject>>();

        foreach (ObjectPoolEntry poe in Entries)
        {
            for (int a = 0; a < poe.Count; a++)
            {
                if (pool.ContainsKey(poe.Prefab.name) == false)
                {
                    pool.Add(poe.Prefab.name, new List<SceneObject>());
                }
                SceneObject so = CreateSceneObject(poe.Prefab);
                pool[so.name].Add(so);
            }
        }
    }
    SceneObject CreateSceneObject(SceneObject so)
    {
        SceneObject instance = Instantiate(so) as SceneObject;        
        instance.name = so.name;
        instance.transform.SetParent(containerObject.transform);
        instance.gameObject.SetActive(false);
        
        return instance;
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

    public SceneObject GetObjectForType(string instanceName, bool onlyPooled)
    {
        if (pool.ContainsKey(instanceName))
        {
            
            if (pool[instanceName].Count > 0)
            {
                SceneObject instance = pool[instanceName][0];
                pool[instanceName].Remove(instance);
                return instance;
            }
            else
            {
                foreach (ObjectPoolEntry poe in Entries)
                {
                    if (poe.Prefab.name == instanceName)
                    {
                        SceneObject so = CreateSceneObject(poe.Prefab);
                        return so;
                    }
                }
            }
            return null;
        }
        else
        {
            Debug.LogError(" no existe: " + instanceName);
            return null;
        }
    }

    public void PoolObject(SceneObject obj)
    {
        if (obj.broken)
        {
            Destroy(obj.gameObject);
            return;
        }
        else  if (pool.ContainsKey(obj.name))
        {
            obj.transform.SetParent(containerObject.transform);
            obj.gameObject.SetActive(false);
            pool[obj.name].Add(obj);
        }
        else
            Destroy(obj.gameObject);
    }
}