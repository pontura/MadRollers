using UnityEngine;
using System.Collections;

public class SliderEffect : MonoBehaviour {

    public float speed = 0;
    private CharacterBehavior cb;

    void Start()
    {
        if (GetComponent<CharacterBehavior>())
            cb = GetComponent<CharacterBehavior>();
    }

	void Update () {

        if (speed == 0) return;

        Vector3 pos = transform.localPosition;
        pos.x += (speed * 10) * Time.deltaTime;
        transform.localPosition = pos;

        if (cb == null) return;
        if (cb.state != CharacterBehavior.states.RUN && cb.state != CharacterBehavior.states.SHOOT)
            speed = 0;
	}

    void OnDisable()
    {
        Destroy(GetComponent<SliderEffect>());
    }
}
