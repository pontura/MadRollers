using UnityEngine;
using System.Collections;

public class BurnableItem : MonoBehaviour {
	
	private CharacterBehavior character;
	public float damage;

	
	void OnTriggerEnter (Collider other) {
		if(other.tag == "Player")
		{
			other.SendMessage("burned", damage);
		}
	}
}
