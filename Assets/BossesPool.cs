using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossesPool : MonoBehaviour
{
    public Material[] materials;
    public GameObject[] assets;
    public GameObject[] modules;

    public Dictionary<string, GameObject> assetsPool;

    //public void AddAll()
    //{
    //    //AddAsset(Data.Instance.assetsBundleLoader.GetGo("bossasset", "apple"));
    //    GameObject bossesPool = Data.Instance.assetsBundleLoader.GetAsset("bosses", "alien");
    //    print("_____bossesPool: " + bossesPool);
    //    foreach (GameObject go in bossesPool.GetComponentsInChildren<GameObject>())
    //        AddAsset(go);
    //}
    public void AllLoaded()
    {
        foreach (GameObject go in assets)
            ChangeManterials(go);

       // materials = null;
    }
    public void ChangeManterials(GameObject go)
    {
        foreach (MeshRenderer mr in go.GetComponentsInChildren<MeshRenderer>())
        {
            print(mr.sharedMaterial);
            Material m = ChangeMaterialByName(mr.sharedMaterial.name);
            if (m != null)
                mr.sharedMaterial = m;
            else
                Debug.Log("Falta el material " + mr.sharedMaterial.name + " para el boss");
        }
    }
    Material ChangeMaterialByName(string matName)
    {
        foreach (Material m in materials)
            if (m.name == matName)
                return m;
        return null;
    }
    public void AddAsset(GameObject go)
    {
        print(go);
        GameObject newGO = Instantiate(go);
        print(newGO + newGO.name);
        newGO.transform.SetParent(transform);

        if (assetsPool == null)
            assetsPool = new Dictionary<string, GameObject>();

        assetsPool.Add(go.name, newGO);
    }
    public GameObject GetBossModule(string name)
    {
        foreach (GameObject go in modules)
            if (go.name == name)
                return go;
        return null;
    }
    public GameObject GetBossAsset(string name)
    {
        foreach (GameObject go in assets)
            if (go.name == name)
                return go;
        return null;
    }
}
