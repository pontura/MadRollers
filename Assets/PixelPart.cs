using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPart : MonoBehaviour {

	public MeshRenderer mr;
	Color color;
    [SerializeField] private Rigidbody rb;

    public Rigidbody Rb
    {
        get { return rb; }
    }
	public void Init(Color newColor)
	{
		Invoke ("Reset", 2);

		if (color == newColor)
			return;
		
		this.color = newColor;
		mr.material.color = color;

	}
	void Reset()
	{
		ObjectPool.instance.pixelsPool.Pool (this);
	}
}
