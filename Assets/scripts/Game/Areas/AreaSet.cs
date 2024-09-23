using UnityEngine;
using System.Collections;

public class AreaSet : MonoBehaviour {

    public int competitionsPriority;
    public bool randomize = true;
	public TextAsset[] areasData;
	public Area[] areas;
	public int totalAreasInSet;

	public Vector3 cameraOrientation;
	public float cameraRotationX;

	public bool isLastArea;

    //[HideInInspector]
    int id = 0;

	public void Restart()
	{
		this.id = 0;
	}
//	public Vector4 getCameraOrientation ()  {
//		return new Vector4(cameraOrientation.x,cameraOrientation.y,cameraOrientation.z,cameraRotationX);
//	}

	public Area getArea () {
        Area area;


        if (randomize)
            area = areas[Random.Range(0, areas.Length)];
		else
			area = areas[id];


        if (id < areas.Length - 1)
            id++;

       // print("AREA: " + area.name);

        return area;
	}
}
