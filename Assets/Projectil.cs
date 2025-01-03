using UnityEngine;
using System.Collections;

public class Projectil : SceneObject {

    public int playerID = -1;
	float realSpeed;
	int speed = 50;
	int myRange = 5;
	int damage = 10;

	private float myDist;
	private bool exploted;

    private Vector3 rotation;

    private Color color;
    [SerializeField] MeshRenderer meshToColorize;
	//public int team_for_versus;

	[SerializeField] GameObject BulletPlayer0;
    [SerializeField] GameObject BulletPlayer1;
    [SerializeField] GameObject BulletPlayer2;
    [SerializeField] GameObject BulletPlayer3;

    Color lastColor;

    public virtual void SetColor(Color color)
    {
		if (lastColor == color)
			return;
		
        this.color = color;
		lastColor = color;

        MaterialPropertyBlock mat = new MaterialPropertyBlock();
        meshToColorize.GetPropertyBlock(mat);
        mat.SetColor("_Color", color);
        meshToColorize.SetPropertyBlock(mat);

        //old: meshToColorize.material.color = color;
    }
	int lastPlayerID = -1;
	public virtual void ResetWeapons()
	{
		BulletPlayer0.SetActive (false);
		BulletPlayer1.SetActive (false);
		BulletPlayer2.SetActive (false);
		BulletPlayer3.SetActive (false);
	}
    public override void OnRestart(Vector3 pos)
    {		
		base.OnRestart(pos);

		realSpeed = speed;
		target = null;    

        myDist = 0;
        exploted = false;

		ResetWeapons ();
		switch (playerID) {
		case 0:
			BulletPlayer0.SetActive (true);
			break;
		case 1:
			BulletPlayer1.SetActive (true);
			break;
		case 2:
			BulletPlayer2.SetActive (true);
			break;
		case 3:
			BulletPlayer3.SetActive (true);
			break;
		}	

		if (lastPlayerID != playerID) {

			MultiplayerData multiplayerData = Data.Instance.multiplayerData;
			Color playerColor;
			lastPlayerID = playerID;

			if (playerID < 4 && playerID >= 0) {
                TrailRenderer tr = GetComponent<TrailRenderer>();
                playerColor = multiplayerData.colors [playerID];
				playerColor.a = 0.35f;
                tr.startColor = playerColor;
                tr.endColor = playerColor;
			} else {
				playerColor = multiplayerData.colors [4];
			}
		}

    }
    
	void LateUpdate()
	{
		if (!isActive)
			return;
		
		if (target != null) {
			if (target.transform.position.z < transform.position.z) {
				target = null;
			} else {		
				Vector3 lookAtPos = target.transform.position;
				lookAtPos.y += 0.9f;
				Vector3 myPos = transform.position;
				myPos.z = lookAtPos.z;
				Vector3 newLookAt = Vector3.Lerp(myPos, lookAtPos, 0.15f);
				transform.LookAt (newLookAt);
			}
		}
		Vector3 pos = transform.localPosition;
		myDist += Time.deltaTime * realSpeed;
        rotation = transform.localEulerAngles;
		RectificaRotation ();
		
       // rotation.y = 0;
		if (pos.y < - 3) ResetProjectil();
        else
		if(myDist >= myRange)
		{
            rotation.x += 15 * Time.deltaTime;					
            transform.localEulerAngles = rotation;
		}
		pos += transform.forward * speed  * Time.deltaTime;		
		transform.localPosition = pos;
	}
	public virtual void RectificaRotation()
	{
		//RECTIFICA
		float gotoRot = 0;
//		if(team_for_versus == 2)
//			gotoRot = 180;
//		else 
			if (rotation.y > 180)
			gotoRot = 360;

		rotation.y = Mathf.Lerp(rotation.y , gotoRot, Time.deltaTime*4);
	}
	void OnTriggerEnter(Collider other) 
	{
        if (!isActive) return;
		if(exploted) return;
        Breakable breakable;

        switch (other.tag)
		{
            case "wall":
                addExplotionWall();
				SetScore( ScoresManager.score_for_destroying_wall, ScoresManager.types.DESTROY_WALL);
                ResetProjectil();
                break;
			case "floor":
				addExplotion(0.2f);
				SetScore( ScoresManager.score_for_destroying_floor, ScoresManager.types.DESTROY_FLOOR);
                ResetProjectil();
				break;
		case "enemy":
				MmoCharacter enemy = other.gameObject.GetComponent<MmoCharacter> ();
                //esto funca para los bosses:-----------------------
                if (enemy) {
					if (enemy.state == MmoCharacter.states.DEAD)
						return;
                   // Debug.Log(other.gameObject.name + " total:  score_for_breaking " + ScoresManager.score_for_breaking + "score: " + enemy.score);

                    SetScore( ScoresManager.score_for_killing + enemy.score, ScoresManager.types.KILL);
					enemy.Die ();
				} else {
                    breakable = other.gameObject.GetComponent<Breakable>();
                    breakable.breakOut(other.gameObject.transform.position, true);
                  //  Debug.Log(other.gameObject.name + "   score_for_breaking " + ScoresManager.score_for_breaking + "score: " + breakable.GetSceneObject().score);

                    SetScore(ScoresManager.score_for_killing + breakable.GetSceneObject().score, ScoresManager.types.KILL);
                    //other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
                }
                //---------------------------------------------------

                ResetProjectil();
				break;
			case "destroyable":
                breakable = other.gameObject.GetComponent<Breakable>();
                int total = ScoresManager.score_for_breaking + breakable.GetSceneObject().score;
                //Debug.Log(other.gameObject.name + " total: " + total + "   score_for_breaking " + ScoresManager.score_for_breaking + "score: " + breakable.GetSceneObject().score);
				SetScore(total, ScoresManager.types.BREAKING);
                breakable.breakOut(other.gameObject.transform.position, true);
                // other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
                ResetProjectil();
				break;
			case "boss":
				SetScore( ScoresManager.score_for_boss, ScoresManager.types.BOSS);
                other.gameObject.GetComponent<Boss>().breakOut();

                // other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
                ResetProjectil();
				break;
		case "firewall":
				//SetScore(70);
			//	other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
				Vector3 rot = transform.localEulerAngles;
				rot.y += 180+other.gameObject.GetComponentInParent<SceneObject>().transform.localEulerAngles.y;
				transform.localEulerAngles = rot;
				break;
		case "Player":
			if (Data.Instance.playMode != Data.PlayModes.VERSUS)
				return;
			CharacterBehavior cb = other.gameObject.GetComponentInParent<CharacterBehavior> ();
			if (cb == null
			    || cb.player.id == playerID
			    || cb.state == CharacterBehavior.states.CRASH
			    || cb.state == CharacterBehavior.states.FALL
			    || cb.state == CharacterBehavior.states.DEAD)
				return;

                //chequea si el projectil es del otro team
                //if (team_for_versus == cb.team_for_versus)
                //	return;

                Data.Instance.framesController.ForceFrameRate(0.05f);
			    Data.Instance.events.RalentaTo (1, 0.05f);
			    cb.Hit ();
                ResetProjectil();
			break;
		}
	}
	void SetScore(int score, ScoresManager.types type)
    {
		if(playerID>=0 && score >0)
			Data.Instance.events.OnScoreOn(playerID, transform.position, score, type);
    }
	void addExplotion(float _y)
	{
        if (!isActive) return;
		exploted = true;        
        Data.Instance.events.AddExplotion(transform.position, color);
	}
    void addExplotionWall()
    {
        if (!isActive) return;
        exploted = true;
        Data.Instance.events.AddWallExplotion(transform.position, color);
    }
	void ResetProjectil()
    {
		target = null;
        Pool();
    }
	public override void OnPool()  
	{
		target = null;
	}
	GameObject target = null;
	public void StartFollowing(GameObject _target)
	{
		
		if (target != null)
			return;
		
		realSpeed /= 2;
		
		this.target = _target;
	}
}
