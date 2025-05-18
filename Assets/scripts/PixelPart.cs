using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPart : MonoBehaviour {

	[SerializeField] MeshRenderer mr;
	private Color color;
    [SerializeField] private Rigidbody rb;
    private MaterialPropertyBlock mat;

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

        if(mat == null)
            mat = new MaterialPropertyBlock();

        mr.GetPropertyBlock(mat);
        mat.SetColor("_Color", color);
        mr.SetPropertyBlock(mat);
	}
	void Reset()
	{
		ObjectPool.instance.pixelsPool.Pool (this);
	}
}
