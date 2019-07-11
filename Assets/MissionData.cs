using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MissionData  {

	public string jsonName;
	public string title;
	public int id;
	public int maxScore;
	public string fondo;
	public List<AreaSetData> areaSetData;

	[Serializable]
	public class AreaSetData
	{
        public bool boss;
		public int total_areas;
		public float bending;
		public bool randomize;
		public List<string> areas;
		public Vector3 cameraOrientation;
	}
}
