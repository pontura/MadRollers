using UnityEngine;
using System.Collections;

public class Bomb : SceneObject {

	private float start_Y = 15;
    private float speed = 9;

    public Breakable breakable;

    public AudioClip soundFX;
    public TrailRenderer trailRenderer;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public override void OnRestart(Vector3 pos)
    {
        trailRenderer = GetComponent<TrailRenderer>();
        pos.y = start_Y;
        base.OnRestart(pos);
    }
    public override void onDie()
    {
        trailRenderer.time = 0;
        Game.Instance.level.OnAddExplotion(transform.position, 16, Color.red);
        Pool();
    }
    private void Update()
    {
        if (distanceFromCharacter > 65)
            return;        

        if (!GetComponent<AudioSource>().isPlaying)
        {
            trailRenderer.time = 10;
            GetComponent<AudioSource>().clip = soundFX;
            GetComponent<AudioSource>().Play();
        }
        Vector3 pos = base.gameObject.transform.position;
        pos.y -= speed * Time.deltaTime;
        transform.position = pos;
    }
    public override void OnPool()
    {
		StopAllCoroutines ();
        if (audioSource != null)
            audioSource.Stop();

		if(trailRenderer!=null)
       		trailRenderer.time = 0;
    }
}
