using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomataFloorChecker : MonoBehaviour
{

    float num;
    CharacterBehavior cb;

    void Start()
    {
        if (!transform.parent.gameObject.GetComponent<CharacterControls>().isAutomata)
        {
            Destroy(gameObject);
            return;
        }
        cb = transform.parent.gameObject.GetComponent<CharacterBehavior>();
      
    }
    
    private void Update()
    {
        if (cb == null)
            return;
        if (cb.grounded)
            num += Time.deltaTime;
        if (num > 0.05f)
        {
            num = 0;
            RaycastHit coverHit;
            Vector3 dest = transform.position;
            dest.y -= 4;
            if (!Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out coverHit, Mathf.Infinity, 1 << 8))
            {
                cb.Jump();
                cb.JumpingPressed();
            }
        }
    }
}
