using UnityEngine;
using System.Collections;

public class SceneObject : MonoBehaviour {

    public Level.Dificult Dificult;

	public Color lastColor;

    public int size_z = 0;

	public bool broken;
    public int id;

    [HideInInspector]
    public Transform characterTransform;

    [HideInInspector]
    public bool isActive;
    public int score;

   // [HideInInspector]
   // public CharactersManager charactersMmanager;

    public int distanceFromCharacter;

    private Transform[] childs;

    //se dibuja solo si hay mas de un avatar vivo:
    public bool onlyMultiplayers;
	SceneObjectsManager manager;

	public void Init(SceneObjectsManager manager)
	{
		this.manager = manager;
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

		if(transform !=null)
      		transform.position = newPos;  
		
        ObjectPool.instance.PoolObject(this);
		if (manager == null)
			Debug.LogError ("manager null " + gameObject.name);
		manager.RemoveSceneObject (this);
        OnPool();
    }
	public void Updated(float distance)
	{
		distanceFromCharacter = (int)(transform.position.z - distance);
	//	OnSceneObjectUpdate();
	}
//    public virtual void OnSceneObjectUpdate()
//    {
//        SendMessage("OnSceneObjectUpdated", SendMessageOptions.DontRequireReceiver);
//    }
    public virtual void OnRestart(Vector3 pos)
    {
        transform.position = pos;
    }

	public virtual void ChangeColor(Color newColor)  { 
		if (newColor == lastColor)
			return;
		lastColor = newColor;
		MeshRenderer mr = GetComponent<MeshRenderer> ();
		mr.material.color = newColor;
	}
    public virtual void changeMaterial(string materialName)  {   }
    public virtual void OnPool()  {  }
    public virtual void onDie()  { }
    public virtual void setScore()   { }    

	Color matColor;
	int videoGameID = -1;
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
	void ChangeMaterials(Renderer renderer)
	{
		renderer.material.color = matColor;
	}
}
