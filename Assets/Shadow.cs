using UnityEngine;
using System.Collections;

public class Shadow : SceneObject {

    private SceneObject target;
    private float ofssetY = 0.25f;

    void Start()
    {
        transform.Rotate(90, 0, 0);
    }
    public void Init(SceneObject target)
    {
        this.target = target;        
    }
	void Update()
	{
		return;
        if (target && target.isActive)
        {
            Vector3 pos = target.transform.position;
            pos.y = ofssetY;
            transform.position = pos;
        }
        else
        {
            Pool();
        }
    }
}
