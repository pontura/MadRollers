using UnityEngine;
using System.Collections;

public class BurnableItem : MonoBehaviour {
	
	private CharacterBehavior character;
	//private bool active;
	public float damage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter (Collider other) {
		if(other.tag == "Player")
		{
			other.SendMessage("burned", damage);
		}
	}
}
