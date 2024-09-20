using UnityEngine;
using System.Collections;

public class BombTeledirigida : Bomb {

    private bool startedAnim;
    private bool right;

    public override void OnRestart(Vector3 pos)
    {
        trailRenderer = GetComponent<TrailRenderer>();
        pos.y = 0f;
        base.OnRestart(pos);
        pos.x = -10;
        //breakable.OnBreak += OnBreak;
        startedAnim = false;
        if (Random.Range(0, 10) < 5)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            right = true;
        }
        else
        {
            right = false;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    
	void Update()
	{
		if (!isActive)
			return;

        trailRenderer.time = 10;

        Vector3 pos = base.gameObject.transform.position;
        if (!startedAnim)
        {
            pos.x = -10;
            if (right) pos.x *= -1;
            pos.y = 0;
            startedAnim = true;
        }
    }
}
