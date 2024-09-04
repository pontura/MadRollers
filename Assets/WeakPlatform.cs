using UnityEngine;
using System.Collections;

public class WeakPlatform : SceneObject {

	Collider _collider;
	[SerializeField] private  GameObject to;
    [SerializeField] int videoGame_ID;

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
        if(_collider == null)
            _collider = GetComponent<Collider>();
    }
    public override void OnRestart(Vector3 pos)
	{
        falling = false;

		base.OnRestart(pos);
        _collider.enabled = true;

        VideogamesData videogamesData = Data.Instance.videogamesData;


        int newVideoGameID = videogamesData.actualID;
		if (newVideoGameID != videoGame_ID) {
            if (type == types.FLOOR) {
                videoGame_ID = newVideoGameID;
                VideogameData vd = videogamesData.GetActualVideogameData();
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
				GetComponent<Renderer>().material = Data.Instance.videogamesData.GetActualVideogameData ().wallMaterial;
			}
		}
	}

	public void breakOut(Vector3 impactPosition) {

        _collider.enabled = false;
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

        ObjectPool pool = ObjectPool.instance;
        SceneObject newSO;
        Vector3 f_MidZ = transform.forward * MidZ;
        Vector3 r_MidZ  = transform.right * MidX;
        for (int a = 0; a < 4; a++)
		{
            newSO = pool.GetObjectForType(to.name, false);
			if (newSO.name != "extraSmallBlock1_real" && newSO.name != "extraSmallBlock1_real")
                Manager.areaSceneObjectManager.ResetEveryaditionalComponent (newSO);			

			Vector3 newPos;
			switch (a)
			{
			case 0: newPos = pos + f_MidZ + r_MidZ; break;
			case 1: newPos = pos + f_MidZ - r_MidZ; break;
			case 2: newPos = pos - f_MidZ - r_MidZ; break;
			default: newPos = pos - f_MidZ + r_MidZ; break;
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

        if (_collider != null)
            _collider.enabled = false;
	}

}
