using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomataCollisionsChecker : MonoBehaviour
{
    CharacterBehavior cb;
    void Start()
    {
        if (!transform.parent.gameObject.GetComponent<CharacterControls>().isAutomata)
        {
            Destroy(gameObject);
        }
        else
        {
            cb = transform.parent.gameObject.GetComponent<CharacterBehavior>();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "destroyable":
            case "enemy":
                cb.shooter.CheckFire();
                break;
        }
    }
}
