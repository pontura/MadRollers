using UnityEngine;
using System.Collections;

public class WeakPlatform : SceneObject {

	Collider collider;
	[SerializeField] private  GameObject to;
	public int videoGame_ID;

    [SerializeField] private Renderer render;

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
                videoGame_ID = newVideoGameID;
                VideogameData vd = Data.Instance.videogamesData.GetActualVideogameData();
                Color newColorTop = vd.floor_top;
                if (floor_top == null || newColorTop != floor_top)
                {
                    floor_top = newColorTop;
                    floor_border = vd.floor_border;

                    MaterialPropertyBlock floor_top_mat = new MaterialPropertyBlock();
                    MaterialPropertyBlock floor_border_mat = new MaterialPropertyBlock();

                    render.GetPropertyBlock(floor_top_mat, 1);
                    render.GetPropertyBlock(floor_border_mat, 0);

                    floor_top_mat.SetColor("_Color", floor_top);
                    floor_border_mat.SetColor("_Color", floor_border);

                    render.SetPropertyBlock(floor_top_mat, 1);
                    render.SetPropertyBlock(floor_border_mat, 0);
                }
			} else {
                if (render == null)
                    render = GetComponent<Renderer>();

                render.material = Data.Instance.videogamesData.GetActualVideogameData ().wallMaterial;
			}
		}
	}

	public void breakOut(Vector3 impactPosition) {

		collider.enabled = false;
		if (!to)
		{
			Fall();
			return;
		}

		float MidX = transform.lossyScale.x / 200;
		float MidZ = transform.lossyScale.z / 200;

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
                Manager.areaSceneObjectManager.ResetEveryaditionalComponent (newSO);			

			Vector3 newPos;
			switch (a)
			{
			case 0: newPos = pos + transform.forward * MidZ + transform.right * MidX; break;
			case 1: newPos = pos + transform.forward * MidZ - transform.right * MidX; break;
			case 2: newPos = pos - transform.forward * MidZ - transform.right * MidX; break;
			default: newPos = pos - transform.forward * MidZ + transform.right * MidX; break;
			}

            Manager.AddSceneObjectAndInitIt(newSO, newPos, container);
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
	}
	public override void OnPool()
	{
        if (falling)
            CancelInvoke();
        falling = false;

        if (collider != null)
			collider.enabled = false;
	}

}
