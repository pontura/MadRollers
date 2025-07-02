using UnityEngine;

public class LevelsThumbsData : MonoBehaviour
{
    public GameObject[] thumbs;

    public GameObject GetThumb(string name)
    {
        foreach (GameObject thumb in thumbs)
        {
            if (thumb.name == name)
            {
                return thumb;
            }
        }
        return null;
    }
}
