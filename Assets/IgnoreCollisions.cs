using UnityEngine;
using System.Collections;

public class IgnoreCollisions : MonoBehaviour {

    public SceneObject parentSceneObject;

    void Start()
    {
       // parentSceneObject.GetComponent<SceneObject>();
    }
    void SceneObjectStart(Vector3 pos)
    {
		//Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		//Physics.IgnoreCollision(playerTransform.collider, collider);
	}
}
