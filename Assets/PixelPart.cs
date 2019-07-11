using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPart : MonoBehaviour {

	public MeshRenderer mr;
	Color color;

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
