using UnityEngine;
using System.Collections;

public class ParticlesSceneObject : SceneObject {

    public ParticleSystem _particleSystem;
    public ParticleSystem explotion;
    public ParticleSystem[] explotions_to_colorize;

    public override void OnRestart(Vector3 pos)
    {
        base.OnRestart(pos);

        if (_particleSystem != null)
        {
            _particleSystem.Clear();
            _particleSystem.Play();
        }
		if (explotion != null) {
			explotion.Clear ();
			explotion.Play ();
		}
    }
	public void SetColor(Color color, float alpha = 0.45f)
    {      
		if (color == lastColor)
			return;
		lastColor = color;
		color.a = alpha;

        foreach (ParticleSystem ps in explotions_to_colorize)
            ps.startColor = color;
    }

}
