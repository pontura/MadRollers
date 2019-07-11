using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEnemy : SceneObject {
	
	public int speed = 50;
	bool exploted;
	void Start () {

	}
	public override void OnRestart(Vector3 pos)
	{			
		target = null;
		base.OnRestart(pos);
		exploted = false;
	}

	void Update()
	{
		if (!isActive)
			return;
		if (target != null) {
			if (target.transform.position.z < transform.position.z) {
				target = null;
			} else {		
				Vector3 lookAtPos = target.transform.position;
				lookAtPos.y += 1.5f;
				transform.LookAt (lookAtPos);
			}
		}
		Vector3 pos = transform.localPosition;
		pos += transform.forward * speed  * Time.deltaTime;		
		transform.localPosition = pos;
	}
	void OnTriggerEnter(Collider other) 
	{
		if (!isActive) return;
		if(exploted) return;

		switch (other.tag)
		{
		case "wall":
			addExplotionWall();
			Reset();
			break;
		case "floor":
			addExplotion(0.2f);
			Reset();
			break;
		case "enemy":
			MmoCharacter enemy = other.gameObject.GetComponent<MmoCharacter> ();

			if (enemy) {
				if (enemy.state == MmoCharacter.states.DEAD)
					return;
				enemy.Die ();
			} else {
				other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
			}
			Reset();
			break;
		case "destroyable":
			other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
			Reset();
			break;
		case "boss":
			other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
			Reset();
			break;
		case "firewall":
			Vector3 rot = transform.localEulerAngles;
			rot.y += 180+other.gameObject.GetComponentInParent<SceneObject>().transform.localEulerAngles.y;
			transform.localEulerAngles = rot;
			break;
		case "Player":
			if (Data.Instance.playMode != Data.PlayModes.VERSUS)
				return;
			CharacterBehavior cb = other.gameObject.GetComponentInParent<CharacterBehavior> ();
			if (cb == null
				|| cb.state == CharacterBehavior.states.CRASH
				|| cb.state == CharacterBehavior.states.FALL
				|| cb.state == CharacterBehavior.states.DEAD)
				return;

			Data.Instance.GetComponent<FramesController> ().ForceFrameRate (0.05f);
			Data.Instance.events.RalentaTo (1, 0.05f);
			cb.Hit ();
			Reset();
			break;
		}
	}
	void addExplotion(float _y)
	{
		if (!isActive) return;
		exploted = true;        
		Data.Instance.events.AddExplotion(transform.position, Color.red);
	}
	void addExplotionWall()
	{
		if (!isActive) return;
		exploted = true;
		Data.Instance.events.AddWallExplotion(transform.position, Color.red);
	}
	void Reset()
	{
		target = null;
		Pool();
	}

	GameObject target;
	public void StartFollowing(GameObject target)
	{
		if (this.target)
			return;

		this.target = target;
	}
}
