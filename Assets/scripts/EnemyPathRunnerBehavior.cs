using UnityEngine;
using System.Collections;

public class EnemyPathRunnerBehavior : MonoBehaviour {
	
	public float speed = 2;
    public float randomSpeedDiff  = 0;
    public Vector3[] paths;
    private int pathID;
    private float realSpeed;
    private MmoCharacter mmoCharacter;    

    public void Start()
    {
        pathID = 0;
        if (randomSpeedDiff != 0)
            realSpeed = speed + Random.Range(0, randomSpeedDiff);
        else
            realSpeed = speed;

        mmoCharacter = GetComponent<MmoCharacter>();

        changeDirection();

        //if (Mathf.Abs(realSpeed) > 3)
          //  mmoCharacter.run();
      //  else
         //   mmoCharacter.walk();        
	}
    void OnDisable()
    {
        Destroy(gameObject.GetComponent("EnemyPathRunnerBehavior"));
    }
    public void changeDirection()
    {
        if (pathID == paths.Length)
            pathID = 0;

		if (mmoCharacter == null)
			return;
		
        if (paths[pathID].x < transform.localPosition.x) 
			mmoCharacter.setRotation(new Vector3(0, 90, 0));		
        else  
			mmoCharacter.setRotation(new Vector3(0, -90, 0));
        
    }

	void Update()
	{
        if (mmoCharacter.state == MmoCharacter.states.DEAD) return;

        float diff = Mathf.Abs(transform.localPosition.x - paths[pathID].x);
        if (Mathf.Abs(diff) < 0.2f)
        {
            pathID++;
            changeDirection();            
        }

        transform.Translate(-Vector3.forward * Mathf.Abs(realSpeed) * Time.deltaTime);
    }
	
}

