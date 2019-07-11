using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour {

	public SceneObject myProjectile;

    private MmoCharacter mmoCharacter;
    private bool ready;
	void Start()
	{
		mmoCharacter = GetComponent<MmoCharacter>();
	}
    void OnEnable()
    {
        ready = false;
    }
	void Update()
	{
        if (ready) return;
        if (!mmoCharacter) return;        
        if (mmoCharacter.state == MmoCharacter.states.DEAD) return;
        if (mmoCharacter.distanceFromCharacter < 14) Shoot();
    }
    void Shoot()
    {
        mmoCharacter.Shoot();
		Vector3 pos = transform.position;
		pos.y += 3;
		pos.z -= 3;
		SceneObject sceneObject = Instantiate(myProjectile, pos, Quaternion.identity) as SceneObject;
		Game.Instance.sceneObjectsManager.AddSceneObject(sceneObject, pos);
		sceneObject.transform.localEulerAngles = new Vector3(0,180,0);
        ready = true;
		Invoke ("ResetAnim", 0.2f);
    }
	void ResetAnim()
	{
		mmoCharacter.shooterAnimation.Play ("idle");
	}

}

