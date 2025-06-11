using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectsManager : MonoBehaviour
{

    public AreaSceneObjectManager areaSceneObjectManager;
    public CharactersManager charactersManager;
    List<SceneObject> sceneObjectsInScene;
    private ObjectPool Pool;
    bool isOn;
    int videoGameID; 

    public void ChangeVideogame(int videoGameID)
    {
        this.videoGameID = videoGameID;
    }
    void Awake()
    {
        areaSceneObjectManager = GetComponent<AreaSceneObjectManager>();
        Pool = Data.Instance.sceneObjectsPool;
        sceneObjectsInScene = new List<SceneObject>();
    }
    public void AddSceneObject(SceneObject so, Vector3 pos)
    {
        so.gameObject.SetActive(false);
        so.isActive = false;
        so.transform.SetParent(Pool.Scene.transform);
        so.transform.localPosition = pos;
        sceneObjectsInScene.Add(so);
        so.Init();
        so.CheckVideoGame(videoGameID);
    }
    public void AddSceneObject(SceneObject so, Vector3 pos, Transform container)
    {
        so.gameObject.SetActive(false);
        so.isActive = false;

        so.transform.localPosition = pos;
        sceneObjectsInScene.Add(so);
        so.Init();
        so.CheckVideoGame(videoGameID);

        so.transform.SetParent(container);
    }
    public void AddSceneObjectAndInitIt(SceneObject so, Vector3 pos, Transform container = null)
    {

        so.gameObject.SetActive(false);
        so.isActive = true;

        if (container != null)
            so.transform.SetParent(container);
        else
            so.transform.SetParent(Pool.Scene.transform);

        so.transform.localPosition = pos;
        sceneObjectsInScene.Add(so);
        so.Init();
        so.Restart(pos);
        so.CheckVideoGame(videoGameID);


    }
    public void RemoveSceneObject(SceneObject so)
    {
        sceneObjectsInScene.Remove(so);
    }
    void Start()
    {
        isOn = true;
        Invoke("Loop", tick);
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
    }
    private void OnDestroy()
    {
        isOn = false;
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
    }
    void OnMissionComplete(int levelID)
    {
        isOn = false;
    }
    float tick = 0.25f;
    void Loop()
    {
        if (isOn)
        {
            Invoke("Loop", tick);
            UpdateSObjects();
        }
    }
    void UpdateSObjects()
    {
        float distance = charactersManager.getDistance();
        int i = sceneObjectsInScene.Count;
        SceneObject so;
        SceneObject so_null = null;
        while (i > 0)
        {
            so = sceneObjectsInScene[i - 1];
            if (so == null)
            {
                // Debug.LogError("SceneObject is null in SceneObjectsManager.UpdateSObjects " + sceneObjectsInScene[i-1].name);
                so_null = sceneObjectsInScene[i - 1];
            }
            else
            {
                float _z = so.transform.position.z;
                if (so.transform.localPosition.y < -7)
                    so.Pool();
                else if (distance > _z + so.size_z + 22)
                    so.Pool();
                else if (distance > _z - 60)
                {
                    if (!so.isActive)
                        so.Restart(so.transform.position);
                    else
                        so.Updated(distance);
                }
            }
            i--;
        }
        if(so_null != null)
            sceneObjectsInScene.Remove(so_null);
    }
    public void PoolSceneObjectsInScene()
    {
        int i = sceneObjectsInScene.Count;
        while (i > 0)
        {
            SceneObject sceneObject = sceneObjectsInScene[i - 1];
            if (sceneObject != null)
                sceneObject.Pool();
            i--;
        }
        ObjectPool.instance.pixelsPool.PoolAll();
    }
}
