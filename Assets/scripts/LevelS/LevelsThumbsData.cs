using UnityEngine;

public class LevelsThumbsData : MonoBehaviour
{
    public GameObject[] thumbs;
    public Sprite[] sprites;

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
    public Sprite GetSprite(string name)
    {
        foreach (Sprite s in sprites)
        {
            if (s.name == name)
            {
                return s;
            }
        }
        return null;
    }
}
