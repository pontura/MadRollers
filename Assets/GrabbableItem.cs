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

     [HideInInspector]
    public Collider TriggerCollider;
     [HideInInspector]
    public Collider FloorCollider;

    [HideInInspector]
    public Player player;
   // public AudioClip heartClip;
   

    public float areaID;
    public int totalGrabbablesInArea;

    public override void OnRestart(Vector3 pos)
    {
		base.OnRestart(pos);
        player = null;

        TriggerCollider = gameObject.GetComponent<SphereCollider>();
        FloorCollider = gameObject.GetComponent<BoxCollider>();

        TriggerCollider.enabled = true;
        FloorCollider.enabled = true;

       
        hitted = false;
        transform.localEulerAngles = new Vector3(0, 0, 0);

        if (GetComponent<Rigidbody>() && !GetComponent<Rigidbody>().isKinematic)
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

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
            Vector3 characterPosition = player.transform.position;
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

            if (other.transform.GetComponent<Player>())
                player = other.transform.GetComponent<Player>();
            else
                player = other.transform.parent.GetComponent<Player>();

            if (player.GetComponent<CharacterBehavior>().state == CharacterBehavior.states.DEAD) return;
            
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
