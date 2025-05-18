using UnityEngine;
using System.Collections;

public class DestroyByTags : MonoBehaviour {

    public SceneObject sceneObject;
    public bool destroyable;
    public bool enemy;
    public bool floor;

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "destroyable":
                if (destroyable)
                {
                    other.gameObject.SendMessage("breakOut", transform.position, SendMessageOptions.DontRequireReceiver);
                    Die();
                }
                break;
            case "enemy":
                if (enemy)
                {
					other.transform.SendMessage("Die", transform.position, SendMessageOptions.DontRequireReceiver);
                    Die();
                }
                break;
            case "floor":
                if (floor)
                {
                    other.gameObject.SendMessage("breakOut", transform.position, SendMessageOptions.DontRequireReceiver);
                    Die();
                }
                break;
        }
    }
    private void Die()
    {
        sceneObject.onDie();
    }
}
