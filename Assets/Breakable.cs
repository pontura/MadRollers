using UnityEngine;
using System.Collections;

public class Breakable : MonoBehaviour {

    public ExplotionType explotionType;
    public enum ExplotionType
    {
        SIMPLE,
        BOMB,        
        ENEMY
    }
    public bool isOn;
	public float NumOfParticles = 30;
	public Breakable[] childs;
    public bool killAtHit;

    //nunca mata
    public bool dontBeKilledByFloorExplotions;

    //nunca mata
    public bool dontKillPlayers;

	//una vez roto no mata
	[SerializeField] private bool dontDieOnHit;

	//si está saltando vuelve a hacer un salto y no muere:
	public bool ifJumpingDontKill;

    private Vector3 originalPosition;
    private SceneObject sceneObject;
    BossPart bossPart;
    public SceneObject GetSceneObject()
    {
        return sceneObject;
    }
	void Start()
	{
        bossPart = GetComponent<BossPart>();

        sceneObject = GetComponent<SceneObject> ();
		if (sceneObject == null)
			sceneObject = GetComponentInParent<SceneObject> ();

	}
    public void OnEnable()
    {               
        isOn = true;
    }
    public void breakOut(Vector3 position)
    {
        breakOut(position, true);
    }

    public void breakOut (Vector3 position, bool destroyedByWeapon) {

        if (!isOn || sceneObject == null)
            return;

        if (bossPart != null)
        {
            bossPart.Hitted();
            if (bossPart.lifes > 0)
                return;
        }		

		sceneObject.broken = true;
        if (destroyedByWeapon)
        {
            Data.Instance.events.OnAddObjectExplotion(transform.position, (int)explotionType);

            // si no es un enemigo agrega una explosion que rompe los objetos cercanos:
            if (gameObject.layer != 17)
            {
                MeshRenderer firstMeshRenderer = GetComponentInChildren<MeshRenderer>();
                Color color = Color.black;
                Data.Instance.events.AddWallExplotion(transform.position, color);
            }
        }

        foreach (Breakable breakable in childs)
            if (breakable && breakable.isOn) breakable.hasGravity();

		breaker();

        if (bossPart != null)
            bossPart.Die();


        isOn = false;

		if (dontDieOnHit)
			dontKillPlayers = true;
		else
			Destroy(gameObject);

    }
	public void hasGravity() {
        isOn = false;
		dontKillPlayers = true;
			
		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();

		if(rb == null)
			rb = gameObject.AddComponent<Rigidbody>();
		
		rb.isKinematic = false;
		rb.useGravity = true;

        Vector3 newPosition = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.5f, 1.1f), Random.Range(0, 0.9f));
        rb.AddForce((Time.deltaTime * newPosition) * 2000, ForceMode.Impulse);

        Vector3 rot = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20));
        gameObject.transform.localEulerAngles += rot;

		//GetComponent<Collider>().isTrigger = true;
		//StartCoroutine(makeItTrigger());

		//SendMessage("OnActivate", SendMessageOptions.DontRequireReceiver);

		if(childs.Length>0)
		{
			foreach (Breakable breakable  in childs)
			{
                if (breakable && breakable.isOn)
					breakable.hasGravity();
			}
		}
	}
	private void breaker(){
		BreakStandard ();
//		if (sceneObject.distanceFromCharacter > 25)
//			return;
//		BreakEveryBlock ();
	}
	void BreakEveryBlock()
	{
		Transform container = Data.Instance.sceneObjectsPool.Scene.transform;
		MeshRenderer[] all = GetComponentsInChildren<MeshRenderer> ();
		int id = 0;
		float force = 500;

		Rigidbody rb;
		foreach (MeshRenderer mr in all) {
			rb = mr.gameObject.GetComponent<Rigidbody> ();

			if (rb == null) 
				rb = mr.gameObject.AddComponent< Rigidbody >();
			
			BreakedBlock bb = mr.gameObject.AddComponent< BreakedBlock >();

			rb.mass = 100;
			rb.useGravity = true;
			rb.isKinematic = false;

			bb.Init ();
			mr.transform.SetParent (container);
			mr.sortingLayerName = "Default";
			mr.gameObject.AddComponent<BoxCollider> ();
			mr.transform.localEulerAngles = new Vector3(0, id * (360 / all.Length), 0);
			Vector3 direction = ((mr.transform.forward * force) + (Vector3.up * (force*2)));
			rb.AddForce(direction, ForceMode.Impulse);

			id++;
		}
	}
	void BreakStandard(){

		MeshRenderer[] all = GetComponentsInChildren<MeshRenderer> ();
		Color[] colors = new Color[all.Length];
		Vector3[] pos = new Vector3[all.Length];
        float[] scale = new float[all.Length];

        int id = 0;
		foreach (MeshRenderer mr in all) {
			if ( mr.material.HasProperty ("_Color"))
				colors [id] = mr.material.color;
			else
				colors [id] = Color.black;
			
			pos [id] = mr.transform.position;
            scale[id] = mr.transform.localScale.x;
            id++;
		}
		ObjectPool.instance.pixelsPool.AddPixelsByBreaking(transform.position, colors, pos, scale);
	}
}
