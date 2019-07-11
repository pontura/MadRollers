using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAutomata : MonoBehaviour {

	CharacterBehavior cb;

	void Start()
	{
		cb = GetComponent<CharacterBehavior> ();
	}
	public void Init()
	{
		Invoke("LoopShoot", Random.Range(1,8));
		Invoke("LoopJump", Random.Range(1,8));
	}
	public void Reset()
	{
		CancelInvoke ();
	}
	void LoopShoot()
	{
		if (cb.state == CharacterBehavior.states.RUN)
			cb.shooter.CheckFire ();
		Invoke("LoopShoot", Random.Range(1,8));
	}
	void LoopJump()
	{
		if (cb.state == CharacterBehavior.states.RUN)
			cb.Jump ();
		Invoke("LoopJump", Random.Range(1,8));
	}
}
