using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {

	public GameObject myProjectile;
	public float reloadTime = 1f;
	public float turnSpeed = 30f;
	public float firePauseTime = .05f;
	public GameObject muzzleEffect;
	public float errorAmount = .001f;
	public Transform myTarget;
	public Transform[] muzzlePositions;
	public Transform turretBall;
	
	private float nextFireTime;
	private Quaternion desiredRotation;
	private Vector3 aimPosition;
	
	
	void Start () {
	
	}
	
	void Update () {
		if(myTarget)
		{		
				aimPosition = new Vector3(myTarget.position.x, myTarget.position.y, myTarget.position.z+6);
				var rotate = Quaternion.LookRotation(aimPosition - transform.position); 
				transform.rotation = rotate;
			if(Time.time >= nextFireTime)
			{
				FireProjectile(rotate);
			}
		} 
		
		//transform.rotation.z = 0;
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			nextFireTime = Time.time+(reloadTime*.5f);
			myTarget = other.gameObject.transform;
		}
	}
	void OnTriggerExit(Collider other)
	{
		
	}
	void FireProjectile(Quaternion rotate)
	{
		nextFireTime = Time.time + reloadTime;
        SceneObject projectile = ObjectPool.instance.GetObjectForType(myProjectile.name, true);
		Game.Instance.sceneObjectsManager.AddSceneObject(projectile, transform.position);
	}
}
