using UnityEngine;
using System.Collections;

public class WeakPlatform : SceneObject {

	Collider collider;
	[SerializeField] private  GameObject to;
	public int videoGame_ID;

	Color floor_top;
	Color floor_border ;

	//Rigidbody rb;
	bool falling;

	public types type;
	public enum types
	{
		FLOOR,
		WALL
	}
    public override void Init(SceneObjectsManager manager)
    {
        base.Init(manager);
        collider = GetComponent<Collider>();
    }
    public override void OnRestart(Vector3 pos)
	{		
		falling = false;

		base.OnRestart(pos);
		collider.enabled = true;

		int newVideoGameID = Data.Instance.videogamesData.actualID;
		if (newVideoGameID != videoGame_ID) {
			if (type == types.FLOOR) {
                Color newColorTop = Data.Instance.videogamesData.GetActualVideogameData().floor_top;
                if (floor_top == null || newColorTop != floor_top)
                {
                    floor_top = newColorTop;
                    floor_border = Data.Instance.videogamesData.GetActualVideogameData().floor_border;
                    videoGame_ID = newVideoGameID;
                    Renderer[] renderers = GetComponentsInChildren<Renderer>();
                    foreach (Renderer r in renderers)
                        ChangeMaterials(r);
                }
			} else {
				GetComponent<Renderer>().material = Data.Instance.videogamesData.GetActualVideogameData ().wallMaterial;
			}
		}
	}
	void ChangeMaterials(Renderer renderer)
	{
        if (renderer.gameObject.name == "floorAllInOne")
        {
            renderer.materials[1].color = floor_top;
            renderer.materials[0].color = floor_border;
        }
        else if (renderer.gameObject.name == "top")
			renderer.material.color = floor_top;
		else
			renderer.material.color = floor_border;
	}

	public void breakOut(Vector3 impactPosition) {

		collider.enabled = false;
		if (!to)
		{
			Fall();
			return;
		}

		float MidX = transform.lossyScale.x / 4;
		float MidZ = transform.lossyScale.z / 4;

		Transform container = null;

		SceneObject soc = transform.parent.gameObject.GetComponent<SceneObject> ();
        if (soc != null)
        {
            container = soc.transform;
            soc = null;
        }
		
		
		Vector3 pos = transform.position;

		for (int a = 0; a < 4; a++)
		{
			SceneObject newSO = ObjectPool.instance.GetObjectForType(to.name, false);
			if (newSO.name != "extraSmallBlock1_real" && newSO.name != "extraSmallBlock1_real")
                manager.areaSceneObjectManager.ResetEveryaditionalComponent (newSO);			

			Vector3 newPos = new Vector3(0, 0, 0);
			switch (a)
			{
			case 0: newPos = pos + transform.forward * MidZ + transform.right * MidX; break;
			case 1: newPos = pos + transform.forward * MidZ - transform.right * MidX; break;
			case 2: newPos = pos - transform.forward * MidZ - transform.right * MidX; break;
			case 3: newPos = pos - transform.forward * MidZ + transform.right * MidX; break;
			}

			manager.AddSceneObjectAndInitIt(newSO, newPos, container);
			newSO.transform.rotation = transform.rotation;
		}
		Pool();

	}
	private void Fall()
	{
		if (falling)
			return;
		falling = true;

        Pool();
		//float r = (float)Random.Range (1f, 10f) / 50f;

		//Invoke("Pool", r );
		//return;



		////if(rb==null)
		////	rb = gameObject.AddComponent<Rigidbody>();

		//rb.isKinematic = false;
		//rb.useGravity = true;
		//if (type == types.FLOOR) {
		//	rb.mass = 40;
		//	rb.velocity = Vector3.zero;
		//	Vector3 dir = (Vector3.up * Random.Range(120,260));
		//	rb.AddForce(dir, ForceMode.Impulse);
		//} else {
		//	rb.mass = 10;
		//	rb.velocity = Vector3.zero;
		//	Vector3 dir = (Vector3.forward * Random.Range(250,460));
		//	dir += new Vector3 (Random.Range (-5, 5), Random.Range (0, 20), 0);
		//	rb.AddForce(dir, ForceMode.Impulse);
		//}
	}
	public override void OnPool()
	{
        if (falling)
            CancelInvoke();
        falling = false;

        if (collider != null)
			collider.enabled = false;
		//if (rb != null)
		//	Destroy (rb);
		//rb.useGravity = false;
		//rb.isKinematic = true;
	}

}
