using UnityEngine;
using System.Collections;

public class FXExplotion : SceneObject {

	public MeshRenderer meshRenderer;
	float finalScale = 8;
	float speed = 6;	

	float timer;
	bool getBigger;

    public override void OnRestart(Vector3 position)
    {
		getBigger = true;
        position.y += 3;

		transform.localScale = Vector3.one;

        GameCamera camera = Game.Instance.gameCamera;

		float distance = transform.position.z - Game.Instance.level.charactersManager.distance;
        distance /= 2;

        float explotionPower = 5 - distance;

        if (explotionPower < 1.5f) explotionPower = 1.5f;
        else if (explotionPower > 3.5f) explotionPower = 3.5f;

        camera.explote(explotionPower);

        base.OnRestart(position);

        setScore();

        position.z += 0;
        position.y += 2;
		timer = 0f;
	}
	public void SetSize(float size = 8)
	{
		this.finalScale = size;
	}
	public void SetColor(Color color)
	{
		if (lastColor == color)
			return;		
		lastColor = color;
		color.a = 0.075f;
		meshRenderer.material.color = color;
	}
	void Update()
	{
		if (!isActive)
			return;

		if (transform.localScale.x <= 0.2f && !getBigger)
			Pool ();
		
		if(getBigger)
			transform.localScale = Vector3.Lerp(transform.localScale , new Vector3(finalScale,finalScale,finalScale), Time.deltaTime*speed);
		else
			transform.localScale = Vector3.Lerp(transform.localScale , Vector3.zero, Time.deltaTime*(speed*1.25f));

		if (getBigger && transform.localScale.x >= (finalScale - 0.4f))
			getBigger = false;
	}
    private void die()
    {
        Pool();
	}
	void OnTriggerEnter(Collider other)
	{
		WeakPlatform w = other.GetComponent<WeakPlatform> ();
		if (w!=null)
		{
			w.breakOut(transform.position);
			return;
		}
		Breakable breakable = other.GetComponent<Breakable> ();
		if (breakable!=null && !breakable.dontBeKilledByFloorExplotions)
		{
			breakable.breakOut(transform.position);
			return;
		}
	}
	
}