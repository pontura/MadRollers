using UnityEngine;
using System;

public class CharacterCollisions : MonoBehaviour {

	public CharacterBehavior characterBehavior;
    private Player player;

	void Start()
	{
        characterBehavior = gameObject.transform.parent.GetComponent<CharacterBehavior>();
        player = gameObject.transform.parent.GetComponent<Player>();
	}	
	void OnTriggerEnter(Collider other) {
		
		if (characterBehavior == null) return;
        if (
			characterBehavior.state == CharacterBehavior.states.DEAD
			|| characterBehavior.state == CharacterBehavior.states.CRASH
			|| characterBehavior.state == CharacterBehavior.states.FALL
		) 
			return;

		if (other.tag == "wall" || other.tag == "firewall") 
		{
            if (characterBehavior.state == CharacterBehavior.states.SHOOT) return;
            if (player.fxState == Player.fxStates.NORMAL)
            {
                Data.Instance.events.AddExplotion(transform.position, Color.red);
                characterBehavior.Hit();
            }
        }
        if (other.tag == "destroyable") 
		{
            if (characterBehavior.state == CharacterBehavior.states.SHOOT) return;
            if (player.fxState == Player.fxStates.NORMAL)
			{
				Breakable breakable = other.GetComponent<Breakable> ();
				if (breakable != null) {
                    if (breakable.ifJumpingDontKill && characterBehavior.IsJumping() && breakable.transform.position.y < transform.position.y)
                        characterBehavior.SuperJumpByHittingSomething();
                    else if (!breakable.dontKillPlayers)
                        characterBehavior.HitWithObject(other.transform.position, false);//breakable.killAtHit);
				}
			}
        }
        else if (other.tag == "floor")
        {
			CharacterAnimationForcer chanimF = other.GetComponent<CharacterAnimationForcer> ();
			if (chanimF != null) {				
				switch (chanimF.characterAnimation) {
				case CharacterAnimationForcer.animate.SLIDE:
					characterBehavior.Slide ();
					break;
				}
			}
            float difY = transform.position.y - other.transform.position.y;
          
           // calcula el -90 de la rotacion del piso:
            if (other.transform.eulerAngles.x == 270 && difY < 0.8f)
               {
                print(other.transform.eulerAngles.x + "    Ernytroooo: " + difY);
                //si es una plataforma rotada se va:
                if (other.transform.localEulerAngles.x != 270)
                    return;

                Vector3 pos = characterBehavior.transform.position;
                if (difY < 0)
                {
                    characterBehavior.CollideToObject();
                    return;
                }
                else if (difY < 0.15f)
                    characterBehavior.SuperJumpByBumped(2600, 0.5f, false);
                else if (difY < 0.5f)
                    characterBehavior.SuperJumpByBumped(2000, 0.5f, false);
                else
                    characterBehavior.SuperJumpByBumped(1200, 0.5f, false);
               
                pos.y += difY;
                characterBehavior.transform.position = pos;
            }
        }
        else if ( other.tag == "enemy" )
        {          
			MmoCharacter mmoCharacter = other.GetComponent<MmoCharacter> ();
			if (mmoCharacter != null) {		
				other.GetComponent<MmoCharacter> ().Die ();
			}
            if (characterBehavior.IsJumping())
            {
                characterBehavior.SuperJumpByBumped(920, 0.5f, false);
                return;
            }
            else if (player.fxState == Player.fxStates.NORMAL)
                characterBehavior.HitWithObject(other.transform.position, false);

        } else if (
			other.tag == "fallingObject"
			&& characterBehavior.state != CharacterBehavior.states.FALL
		)
		{
            if (player.fxState == Player.fxStates.NORMAL)
                characterBehavior.HitWithObject(other.transform.position, false);

        }
    }
}
