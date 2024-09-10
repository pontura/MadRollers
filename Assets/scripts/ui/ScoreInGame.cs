using UnityEngine;
using UnityEngine.UI;

public class ScoreInGame : MonoBehaviour
{
    [SerializeField] Text field;
    bool isActive;
    public bool IsActive { get{ return isActive;  } } 

    public void SetActive( bool isOn, int score = 0)
    {
        this.isActive = isOn;
        gameObject.SetActive(isOn);
        if(isOn)
        {
            field.text = "+" + score;
            Invoke("Reset", 0.5f);
        }
    }
    void Reset()
    {
        SetActive(false);
    }
}
