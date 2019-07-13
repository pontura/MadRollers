using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformChanger : MonoBehaviour
{
    public List<Vector3> RotateAtStart;

    //rota 90 grados en las 4 posiciones:
    public bool rotate_in_4_sizes;

    void Start()
    {
        if (rotate_in_4_sizes)
        {
            RotateAtStart.Add(new Vector3(0, 0, 0));
            RotateAtStart.Add(new Vector3(0, 90, 0));
            RotateAtStart.Add(new Vector3(0, 180, 0));
            RotateAtStart.Add(new Vector3(0, 270, 0));
        }
    }

    void OnEnable()
    {
        if (RotateAtStart.Count > 0)
        {
            transform.localEulerAngles += RotateAtStart[Random.Range(0,RotateAtStart.Count)];
        }
    }
}
