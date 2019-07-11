using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSideData : MonoBehaviour {

	public int z_length;
	[HideInInspector]
	public float offset;
	[HideInInspector]
	public string backgroundSideName;

    public GameObject hideOnLowRes;

    void Start()
    {
        if (hideOnLowRes == null)
            return;

        if (Data.Instance.isAndroid)
            hideOnLowRes.SetActive(false);
        else
            hideOnLowRes.SetActive(true);
    }
}
