using UnityEngine;
using System.Collections;

public class GrabbableItem : SceneObject
{
	public Material groundMaterial;
	public MeshRenderer meshRenderer;
	public int energy = 1;
    //[HideInInspector]
    public bool hitted;
    [HideInInspector]
    public float sec = 0;

    [SerializeField] Collider TriggerCollider;
    [SerializeField] Collider FloorCollider;

    [HideInInspector]
    public Player player;
    Transform player_transform;
   // public AudioClip heartClip;
   

    public float areaID;
    public int totalGrabbablesInArea;

    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnRestart(Vector3 pos)
    {
		base.OnRestart(pos);
        player = null;
        player_transform = null;

        if (TriggerCollider == null)
            TriggerCollider = gameObject.GetComponent<SphereCollider>();
        if (FloorCollider == null)
            FloorCollider = gameObject.GetComponent<BoxCollider>();

        TriggerCollider.enabled = true;
        FloorCollider.enabled = true;

         hitted = false;
        transform.localEulerAngles = new Vector3(0, 0, 0);

        if (rb && !rb.isKinematic)
           rb.velocity = Vector3.zero;

        sec = 0;
    }
	bool isGround;
	public void SetMaterial(Material mat)
	{
		isGround = false;
		meshRenderer.material = mat;
	}
	public void SetGroundMaterial()
	{
		if (isGround)
			return;
		isGround = true;
		meshRenderer.material = groundMaterial;
	}
    public override void OnPool()
    {
        player = null;
    }
	void Update()
	{
		if (!isActive)
			return;

		if(hitted)
		{
            if (player == null) return;
            sec += Time.deltaTime * 100;
			Vector3 position = transform.position;
            Vector3 characterPosition = player_transform.position;
			characterPosition.y+=1f;
			characterPosition.z+=1.2f;
			transform.position = Vector3.MoveTowards(position, characterPosition, 18 * Time.deltaTime);
			if(sec>20)
			{
				Data.Instance.events.OnScoreOn(player.id, Vector3.zero, 10, ScoresManager.types.GRAB_PIXEL);
                Data.Instance.events.OnGrabHeart();
                Data.Instance.musicManager.addHeartSound();
                player = null;
                CheckIfIsPartOfCombo();
                Pool();
			}
		}
	}
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
		if(other.gameObject.CompareTag("Player"))
		{
            player = other.transform.GetComponent<Player>();

            if (player == null)
                player = other.transform.parent.GetComponent<Player>();

            if (player.GetComponent<CharacterBehavior>().state == CharacterBehavior.states.DEAD) return;
            player_transform = player.transform;
			hitted = true;
            TriggerCollider.enabled = false;
            FloorCollider.enabled = false;
		}
	}
    public void SetComboGrabbable(float areaID, int totalGrabbablesInArea)
    {
        this.areaID = areaID;
        this.totalGrabbablesInArea = totalGrabbablesInArea;
    } 
    public void CheckIfIsPartOfCombo()
    {
        if(this.areaID>0)
        {
            Game.Instance.combosManager.GetNewItemToCombo(areaID, totalGrabbablesInArea);
        }
    }
}
