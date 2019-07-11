using UnityEngine;
using System.Collections;

public class TextureChanger : MonoBehaviour {

    Material material;
    [SerializeField]
    Texture texture_1;
    [SerializeField]
    Texture texture_2;

    [SerializeField]
    Texture dead;

    public bool dontChange;

	void Start () {
        material = GetComponent<SkinnedMeshRenderer>().materials[0];
        if (!dontChange)
        texture1();
	}
    void texture1()
    {
        if (dontChange) return;
        material.mainTexture = texture_1;
        Invoke("texture2", Random.Range(1.5f, 2.5f));
    }
    void texture2()
    {
        if (dontChange) return;
        material.mainTexture = texture_2;
        Invoke("texture1", 0.1f);
    }
    public void Dead()
    {
        material.mainTexture = dead;
    }
}
