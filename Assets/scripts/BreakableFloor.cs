using UnityEngine;
using System.Collections;

public class BreakableFloor : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "explotion":
                SendMessage("breakOut", other.transform.position, SendMessageOptions.DontRequireReceiver);
                break;
            case "destroyable":
                SendMessage("breakOut", other.transform.position, SendMessageOptions.DontRequireReceiver);
                break;
        }

    }
}
