using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelsPool : MonoBehaviour {

	Transform poolContainer;
	//Transform sceneContainer;
	List<PixelPart> all;
	[SerializeField] PixelPart pixelPart;

	public void Init (Transform poolContainer, Transform sceneContainer) {
        all = new List<PixelPart>();
        this.poolContainer = poolContainer;
		for (int a = 0; a < 200; a++) {
			PixelPart pp = Instantiate (pixelPart);
			pp.gameObject.SetActive (false);
			all.Add (pp);
			pp.transform.SetParent (poolContainer); 
			pp.Rb.isKinematic = false;
        }
	}
	PixelPart GetPart()
	{
		if (all.Count == 0)
			return null;
		foreach (PixelPart pp in all)
		{
			if(!pp.gameObject.activeSelf)
			{
                pp.gameObject.SetActive(true);
                return pp;

            }
		}
		return null;
	}
	public void Pool(PixelPart pp)
	{
        pp.gameObject.SetActive (false);
	}
	public void AddPixelsByBreaking(Vector3 position, Color[] colors, Vector3[] pos, float[] scale)
	{
		int force = 6;
		int NumOfParticles = colors.Length;
		for (int a = 0; a < NumOfParticles; a++)
		{
			PixelPart pp = GetPart();
			if (pp == null) return;
			pp.gameObject.SetActive (true);
			pp.transform.position = pos[a];
            float s = scale[a];
            if (s > .8f)
                s = .7f;
            else if (s > .4f)
                s /= 1.2f;
            pp.transform.localScale = new Vector3(s,s,s);
            float rot_y;
            if (NumOfParticles < 2)
                rot_y = Random.Range(-90, 90);
            else
                rot_y = a * (360 / NumOfParticles);
            pp.transform.localEulerAngles = new Vector3 (0, rot_y, 0);
			Vector3 direction = ((pp.transform.forward * force) + (Vector3.up * (force * 2)));				

			pp.Rb.linearVelocity = Vector3.zero;
            pp.Rb.AddForce (direction, ForceMode.Impulse);
			pp.Init(colors[a]);
		}
	}
	public void PoolAll()
	{
		foreach(PixelPart pp in all)
			Pool(pp);
	}
}
