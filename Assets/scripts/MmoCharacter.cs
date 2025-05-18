using UnityEngine;
using System.Collections;

public class MmoCharacter : SceneObject
{
	public Animation shooterAnimation;

    public enum states
    {
        IDLE,
        DEAD,
        WALK,
        RUN,
        JUMP,
        WAIT_TO_JUMP,
        FLY
    }

    public states state;

    private Animation _animation;
    private ObjectPool ObjectPool;
    [SerializeField] private Breakable breakable;

    void Start()
    {
        breakable = GetComponent<Breakable>();
        ObjectPool = Data.Instance.sceneObjectsPool;
    }
    void OnEnable()
    {
        _animation = GetComponentInChildren<Animation>();
        
    }
    public override void OnRestart(Vector3 pos)
    {
        gameObject.GetComponent<Collider>().enabled = true;
        base.OnRestart(pos);
        state = states.IDLE;        
    }

	public void Die() {
		if(state== states.DEAD) return;

        Data.Instance.events.OnSoundFX("FX muerte malo00", -1);
        setScore();
		       
		state = states.DEAD;

		SendMessage("isDead", SendMessageOptions.DontRequireReceiver);
		
		Vector3 pos = transform.position;
		pos.y+= 2.1f;
        breakable.breakOut(pos, false);
		//SendMessage("breakOut",pos,SendMessageOptions.DontRequireReceiver);        
        gameObject.GetComponent<Collider>().enabled = false;

		//nuevo:
		Data.Instance.events.OncharacterCheer();
		Pool();
	}
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "destroyable" || other.gameObject.tag == "enemy")
            Die();
    }
	public void run() {
       // _animation.Play("enemyRun");
        state = states.RUN;
	}
    public void fly()
    {
       // _animation.Play("enemyFly");
        state = states.FLY;
    }
    public void walk()
    {
      //  _animation.Play("enemyWalk");
        state = states.WALK;
    }
	public void idle() {
       // _animation.Play("enemyIdle");
        state = states.IDLE;
	}
    public void waitToJump()
    {
       // _animation.Play("enemyIdle");
        state = states.WAIT_TO_JUMP;
    }
	public void jump() {
       // _animation.Play("enemyJump");
        state = states.JUMP;
	}
    public void reachFloor()
    {
        //run();
        //SendMessage("OnReachFloor", SendMessageOptions.DontRequireReceiver);
    }

    public void ChangeSkinMaterial(Material material)
    {
        SkinnedMeshRenderer skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinnedMesh)
             skinnedMesh.material = material;
    }
    public void ChangeClothesColor(Color color)
    {
        SkinnedMeshRenderer skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinnedMesh)
        {
            skinnedMesh.materials[1].color = color;
            skinnedMesh.materials[4].color = color;
        }
    }
    public void ChangeSkinColor(Color color)
    {
        SkinnedMeshRenderer skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinnedMesh)
        {
            skinnedMesh.materials[2].color = color;
        }
    }
	public void Shoot()
	{
		if (shooterAnimation != null)
			shooterAnimation.Play ("shoot");
		state = states.IDLE;
	}
}
