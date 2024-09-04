using UnityEngine;
using System.Collections;

public class SceneObject : MonoBehaviour {
    
    public int size_z = 0;

	public bool broken;
    public int id;

    [HideInInspector]
    public bool isActive;
    public int score;

    public float distanceFromCharacter;
    Transform _transform;
    private Transform[] childs;

    //se dibuja solo si hay mas de un avatar vivo:
    public bool onlyMultiplayers;
    SceneObjectsManager manager;

    private void Awake()
    {
        _transform = transform;
    }
    public virtual void Init(SceneObjectsManager manager)
	{
		this.manager = manager;
	}
    public SceneObjectsManager Manager
    {
        get
        {
            return manager;
        }
    }
    public void Restart(Vector3 pos)
    {
        _transform = transform;
        gameObject.SetActive(true);
        OnRestart(pos);
		isActive = true;
    }
    public void setRotation(Vector3 rot)
    {
        if (_transform.localEulerAngles == rot) return;
        _transform.localEulerAngles = rot;
    }
    public void lookAtCharacter()
    {
       // transform.LookAt(characterTransform);
    }
    public void Pool()
    {
        isActive = false;
        Vector3 newPos = new Vector3(2000, 0, 2000);

		if(_transform != null)
            _transform.position = newPos;  
		
        ObjectPool.instance.PoolObject(this);
        if (manager == null)
        {
            OnPool();
            return;
        }
		manager.RemoveSceneObject (this);
        OnPool();
        manager = null;
    }
	public virtual void Updated(float distance)
	{
		distanceFromCharacter = _transform.position.z - distance;
	}
    public virtual void OnRestart(Vector3 pos)
    {
        transform.position = pos;
    }

    //public virtual void ChangeColor(Color newColor)
    //{
    //    if (newColor == lastColor)
    //        return;
    //    lastColor = newColor;
    //    MeshRenderer mr = GetComponent<MeshRenderer>();
    //    mr.material.color = newColor;
    //}
    public virtual void changeMaterial(string materialName)  {   }
    public virtual void OnPool()  {  }
    public virtual void onDie()  { }
    public virtual void setScore()   { }    

	//Color matColor;
	//int videoGameID = -1;
	public void SetMaterialByVideoGame()
	{
//		matColor = Data.Instance.videogamesData.GetActualVideogameData ().floor_top;
//		Renderer[] renderers = GetComponentsInChildren<Renderer>();
//		int newVideoGameID = Data.Instance.videogamesData.actualID;
//		if (newVideoGameID != videoGameID) {
//			videoGameID = newVideoGameID;
//			foreach(Renderer r in renderers)
//				ChangeMaterials(r);
//		}
	}
	//void ChangeMaterials(Renderer renderer)
	//{
	//	renderer.material.color = matColor;
	//}
}
