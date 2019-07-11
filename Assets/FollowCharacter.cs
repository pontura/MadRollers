using UnityEngine;
using System.Collections;

public class FollowCharacter : MmoCharacter {

	float activaTionDistance = 7;
	float speed = 4;
	CharacterBehavior characterBehavior;
	float realPositionZ = 0;
	CharactersManager charactersManager;

	public override void OnRestart(Vector3 pos)
	{
		charactersManager = Game.Instance.level.charactersManager;
		transform.localEulerAngles = Vector3.zero;
		realPositionZ = 0;
		base.OnRestart(pos);
		Repositionate ();
        int id = Data.Instance.videogamesData.GetActualVideogameData().id;
        // si estas en el espacio:
        Rigidbody rb = GetComponent<Rigidbody>();
        if (id==1)
        {           
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        else
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
	void Repositionate()
	{
		CharacterBehavior ch = charactersManager.getMainCharacter ();
        if (ch == null)
            return;
		Vector3 myPos = transform.position;
		myPos.z = ch.transform.position.z - activaTionDistance;
		transform.position = myPos;
	}
	void Update()
	{
		if (!isActive)
			return;	
		CharacterBehavior cb = charactersManager.getMainCharacter ();
		if (cb == null)
			return;
		
		realPositionZ +=  (speed * Time.deltaTime);

        Vector3 pos = transform.position;
		pos.z = cb.transform.position.z - activaTionDistance + realPositionZ;
		transform.position = pos;

		if (transform.position.z > cb.transform.position.z + 40)
			Pool ();
    }
}

