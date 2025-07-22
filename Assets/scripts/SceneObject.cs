using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneObject : MonoBehaviour
{
    public int size_z = 0;

    public bool broken;
    public int id;

    [HideInInspector]
    public bool isActive;
    public int score;

    public int distanceFromCharacter;

    private Transform[] childs;

    //se dibuja solo si hay mas de un avatar vivo:
    public bool onlyMultiplayers;

    SceneObjectData soData;
    public SceneObjectData SoData
    {
        get
        {
            if (soData == null) soData = GetComponent<SceneObjectData>(); return soData;
        }
    }
    public virtual void CheckVideoGame(int newVideoGameID) { }
    public virtual void Init()
    {
        if(soData == null)
            soData = GetComponent<SceneObjectData>();
    }
    public SceneObjectsManager Manager
    {
        get
        {
            return Game.Instance.sceneObjectsManager;
        }
    }
    public void Restart(Vector3 pos)
    {
        gameObject.SetActive(true);
        OnRestart(pos);
        isActive = true;
    }
    public void setRotation(Vector3 rot)
    {
        if (transform.localEulerAngles == rot) return;
        transform.localEulerAngles = rot;
    }
    public void lookAtCharacter()
    {
        // transform.LookAt(characterTransform);
    }
    public void Pool()
    {
        isActive = false;
        Vector3 newPos = new Vector3(2000, 0, 2000);

        if (transform != null)
            transform.position = newPos;

        ObjectPool.instance.PoolObject(this);
        if (Manager == null)
        {
            OnPool();
            return;
        }
        Manager.RemoveSceneObject(this);
        OnPool();
    }
    public virtual void Updated(float distance)
    {
        distanceFromCharacter = (int)(transform.position.z - distance);
    }
    public virtual void OnRestart(Vector3 pos)
    {
        transform.position = pos;
    }
    public virtual void changeMaterial(string materialName) { }
    public virtual void OnPool() { }
    public virtual void onDie() { }
    public virtual void setScore() { }

    public void SetMaterialByVideoGame()
    {
    }
}
