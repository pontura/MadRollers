using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombThrowed : Projectil {

	public override void OnRestart(Vector3 pos)
	{	
		playerID = -1;
		base.OnRestart (pos);
	}
	public override void SetColor(Color color)
	{
	}
	public override void ResetWeapons()
	{
	}
	public override void RectificaRotation()
	{
	}
}
