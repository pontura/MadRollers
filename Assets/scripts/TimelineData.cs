using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class TimelineData
{
	public bool move;
	public bool rotate;
	public float duration;
	public Vector3 data;
	public easetypes easeType;		
	public enum easetypes{
		LINEAR,
		IN_OUT,
		OUT_IN
	}
}
