using UnityEngine;
using System.Collections;

public class About : MonoBehaviour {

    void Start()
    {
        Data.Instance.GetComponent<Tracker>().TrackScreen("About Screen");
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Data.Instance.LoadLevel("MainMenuMobile");
        }
    }
    public void Back()
    {
        Data.Instance.LoadLevel("MainMenuMobile");
    }
}
