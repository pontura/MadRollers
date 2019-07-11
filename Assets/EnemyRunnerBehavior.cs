using UnityEngine;
using System.Collections;

public class EnemyRunnerBehavior : MonoBehaviour {
	
	public float speed = 2;
    public float randomSpeedDiff  = 0;
    public bool runToCharacter = false;

    private float realSpeed;
    private MmoCharacter mmoCharacter;

    void Start()
    {
        if (randomSpeedDiff != 0)
            realSpeed = speed + Random.Range(0, randomSpeedDiff);
        else
            realSpeed = speed;

        speed *= 2;

        mmoCharacter = GetComponent<MmoCharacter>();
        OnReachFloor();
	}
    void OnDisable()
    {
        Destroy(gameObject.GetComponent("EnemyRunnerBehavior"));
    }
    public void OnReachFloor()
    {
        if (realSpeed > 0)
            mmoCharacter.setRotation(new Vector3(0, 0, 0));
        else
            mmoCharacter.setRotation(new Vector3(0,180, 0));

        if (Mathf.Abs(realSpeed) > 3)
            mmoCharacter.run();
        else
            mmoCharacter.walk();
    }
	void Update()
	{
        if (!mmoCharacter) return;
        if (mmoCharacter.state == MmoCharacter.states.DEAD) return;

        transform.Translate(-Vector3.forward * Mathf.Abs(realSpeed) * Time.deltaTime);
    }
	
}

