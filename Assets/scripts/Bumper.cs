using UnityEngine;
using System.Collections;

public class Bumper : MonoBehaviour {

	//private float _y;
    public float force = 16;
    public AnimationClip animationClip;
    public bool backwardJump;

	void OnTriggerEnter(Collider other) 
	{
		switch (other.tag)
		{
		case "Player":
			CharacterCollisions ch = other.transform.GetComponent<CharacterCollisions> ();
			if(ch != null)
				ch.characterBehavior.SuperJumpByBumped ((int)force * 100, 0.5f, backwardJump);
			break;
		}
	}
//	void OnPooled()
//	{
//		//Destroy(gameObject.GetComponent("Bumper"));
//	}
}
