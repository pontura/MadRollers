using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSignal : SceneObject
{
    public Text field;
	public SpriteRenderer sr1;
	public SpriteRenderer sr2;

    public override void OnRestart(Vector3 pos)
    {
        base.OnRestart(pos);
		//sr1.material.mainTexture = 
    }
}
