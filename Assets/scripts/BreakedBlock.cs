using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakedBlock : MonoBehaviour {

	Coroutine c;
	public void Init()
	{
		c = StartCoroutine (OnClear());
	}
	IEnumerator OnClear()
	{
		yield return new WaitForSeconds (2);
		Destroy (gameObject);
	}
	void OnDestroy()
	{
		if(c!=null)
			StopCoroutine (c);
	}
}
