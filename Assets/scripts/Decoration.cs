using UnityEngine;
using System.Collections;

public class Decoration : SceneObject {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "explotion")
        {
            Pool();
        }
    }

}
