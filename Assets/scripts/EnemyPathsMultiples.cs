using UnityEngine;
using System.Collections;

public class EnemyPathsMultiples : MonoBehaviour {

	public float speed = 2;
	public float randomSpeedDiff  = 0;
	public Vector3[] paths;
	public Vector3 originalPath;
	public Vector3 destPath;

	public int pathID = 0;
	float realSpeed;
	MmoCharacter mmoCharacter;    

	public void Start()
	{
		mmoCharacter = GetComponent<MmoCharacter>();
		if (randomSpeedDiff != 0)
			realSpeed = speed + Random.Range(0, randomSpeedDiff);
		else
			realSpeed = speed;
	}
	public void OnEnable()
	{
		Invoke ("Delayed", 0.1f);
	}
	public void Delayed()
	{
		originalPath = transform.position;
		transform.position = originalPath + paths [pathID];
		SetNewPath();      
	}
	void OnDisable()
	{
		CancelInvoke ();
		Destroy(gameObject.GetComponent("EnemyPathsMultiples"));
	}
	public void SetNewPath()
	{
		pathID++;

		if (pathID == paths.Length)
			pathID = 0;

		destPath = originalPath+paths [pathID];
	}

	void Update()
	{
		if (destPath == null)
			return;
		if (mmoCharacter.state == MmoCharacter.states.DEAD) return;

		Vector3 pos = transform.position;
		float diff = Vector3.Distance(pos, destPath);
		if (Mathf.Abs (diff) < 0.25f) {
			SetNewPath ();            
		} else {
			float adder = realSpeed * Time.deltaTime;
			if (pos.x < destPath.x)
				pos.x += adder;
			else if (pos.x > destPath.x)
				pos.x -= adder;

			if (pos.y < destPath.y)
				pos.y += adder;
			else if (pos.y > destPath.y)
				pos.y -= adder;

			if (pos.z < destPath.z)
				pos.z += adder;
			else if (pos.z > destPath.z)
				pos.z -= adder;

			transform.position = pos;
		}

	}

}

