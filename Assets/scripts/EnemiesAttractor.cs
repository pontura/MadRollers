using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesAttractor : MonoBehaviour {
	
	public Projectil projectil;

	void OnTriggerEnter(Collider other)
	{
		if (projectil == null)
			return;
		projectil.StartFollowing (other.gameObject);
	}
}
