using UnityEngine;
using System.Collections;

public class Bomb : SceneObject {

	private float start_Y = 15;
    private float speed = 9.2f;

    public Breakable breakable;

    public AudioClip soundFX;
    public bool alive;
    public TrailRenderer trailRenderer;


    public override void OnRestart(Vector3 pos)
    {
        trailRenderer = GetComponent<TrailRenderer>();
        pos.y = start_Y;
        base.OnRestart(pos);
        breakable.OnBreak += OnBreak;
        alive = true;        
    }
    public void OnBreak()
    {
        GetComponent<AudioSource>().Stop();
       // setScore();
		//Data.Instance.events.OnDestroySceneObject("bomb");

        alive = false;        

        if(isActive)
            StartCoroutine(waitToPool());

        isActive = false;
    }
    IEnumerator waitToPool()
    {
        trailRenderer.time = 0;
        yield return new WaitForSeconds(3);
        //if (isActive)
            Pool();
    }
    public override void onDie()
    {
        trailRenderer.time = 0;
        addExplotion(0.2f);        
        Pool();
    }
	void Update()
	{
		if (!isActive)
			return;

        trailRenderer.time = 10;

        if (transform.localPosition.y < 5.5f && !GetComponent<AudioSource>().isPlaying)
            startAudio();
       
        //if(alive)
        //{
            Vector3 pos = base.gameObject.transform.position;
			pos.y -= speed * Time.deltaTime;
            base.gameObject.transform.position = pos;
        //} 
	}

	void addExplotion(float _y)
	{
        if (!alive) return;
        if (!isActive) return;
		Game.Instance.level.OnAddExplotion(transform.position, 16, Color.red);
	}
    public override void OnPool()
    {
		StopAllCoroutines ();
        GetComponent<AudioSource>().Stop();

		if(trailRenderer!=null)
       		trailRenderer.time = 0;
    }
    private void startAudio()
    {
        GetComponent<AudioSource>().clip = soundFX;
        GetComponent<AudioSource>().Play();
    }
}
