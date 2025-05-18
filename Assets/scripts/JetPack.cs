using UnityEngine;
using System.Collections;

public class JetPack : Transport {

    public ParticleSystem particles1;
    public ParticleSystem particles2;

    override public void OnSetOn()
    {
        print("OnSetOn");
        particles1.Play();
        particles2.Play();  
    }
    override public void OnSetOff()
    {
        particles1.Stop();
        particles2.Stop();  
        print("OnSetOff");
    }
}
