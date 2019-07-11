using UnityEngine;
using System.Collections;

public class Mortal : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
		{
            CharacterBehavior cb;
            if (other.transform.GetComponent<CharacterBehavior>())
                cb = other.transform.GetComponent<CharacterBehavior>();
            else
                cb = other.transform.parent.GetComponent<CharacterBehavior>();

            //cb.Hit();
            Debug.Log("Mortal HIT PLAYER ____________ ARREGLAR!!!!!!!");
        }
        else if (other.gameObject.CompareTag("enemy"))
		{
            MmoCharacter enemy = other.gameObject.GetComponent<MmoCharacter>();
            if (enemy.state == MmoCharacter.states.DEAD) return;

            enemy.Die();
		}
	}
}
