using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SliderFloor : MonoBehaviour {

    public float scrollSpeed = 0.5F;
    public Renderer rend;
    private float scroll;
    void Start()
    {
        scroll = scrollSpeed;
        if (transform.parent.transform.localEulerAngles.y > 90) scroll *= -1;
    }
    void Update()
    {
		if (rend == null)
			return;
        float offset = Time.time * scrollSpeed * 3;
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "floor" && other.tag != "grabbable")
        {
            GameObject go = null;
            if (other.tag == "Player")
            {
                go = other.transform.parent.gameObject;
            }
            else
            {
                go = other.gameObject;
            }
            if (go == null) return;
            
            if (go.GetComponent<SliderEffect>())
            {
                go.GetComponent<SliderEffect>().speed = scroll;
            }
            else
            {
                go.AddComponent<SliderEffect>().speed = scroll;
            }
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "enemy" )
        {
            GameObject go = other.gameObject;
            if (go.GetComponent<SliderEffect>())
            {
                go.GetComponent<SliderEffect>().speed = scroll;
            }
            else
            {
                go.AddComponent<SliderEffect>().speed = scroll;
            }
        }
    }
}
