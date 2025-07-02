using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class LevelsThumbsRecorder : MonoBehaviour
{
    int width = 120;
    int height = 200;

    Dictionary<int, GameObject> all;
    [SerializeField] Transform container;

    //DEBUG:
    [SerializeField] RawImage[] raws;
    [SerializeField] GameObject[] gameObjects;

    private void Awake()
    {
        all = new Dictionary<int, GameObject>();
    }
    public RenderTexture GetRenderTexture(int id)
    {
        if (all.ContainsKey(id))
        {
            GameObject go = all[id];
            if (go != null)
            {
                Camera cam = go.GetComponentInChildren<Camera>();
                if (cam != null)
                {
                    return cam.targetTexture;
                }
            }
        }
        return null;
    }
    //private void Start()
    //{
    //    int id = 0;
    //    all = new Dictionary<int, GameObject>();
    //    foreach (GameObject g in gameObjects)
    //    {
    //        AddLevel(id, g);
    //        GameObject go = GetGO(id);
    //        if(go !=null)
    //        {
    //            raws[id].texture = go.GetComponentInChildren<Camera>().targetTexture;
    //        }
    //        id++;
    //    }
    //}
    public void AddLevel(string levelName, int id)
    {
        GameObject go = MissionsManager.Instance.thumbs.GetThumb(levelName);
        if(go == null)
        {
            Debug.LogError("No se ha encontrado el prefab de la mision: " + levelName);
            return;
        }
        GameObject newGO = Instantiate(go, container);
        newGO.transform.position = new Vector2(id*10, 0);
        all.Add(id, newGO);
        StartCoroutine(Add(id));
        CreateRenderTexture(newGO);
    }
    GameObject GetGO(int id)
    {
        return all[id];
    }
    IEnumerator Add(int id)
    {
        GameObject go = GetGO(id);
        if (go != null)
        {
            go.SetActive(true);
            yield return new WaitForSeconds(0.5f);
          //  go.SetActive(false);
        }
    }
    public void Activate(int id)
    {
        GameObject go = GetGO(id);
        if(go != null)
        {
            go.SetActive(true);
        }
    }
    public void Inactive(int id)
    {
        GameObject go = GetGO(id);
        if (go != null)
        {
            go.SetActive(false);
        }
    }
    void CreateRenderTexture(GameObject go)
    {
        RenderTexture renderTexture = new RenderTexture(width, height, 24); // 24 = depth buffer
        renderTexture.graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm;
        renderTexture.Create();

        go.GetComponentInChildren<Camera>().targetTexture = renderTexture;

    }
}
