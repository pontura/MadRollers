using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MultiplayerCompetitionButton : MonoBehaviour {
    
    private LandingForArcade container;
    private string title;

    public void Init(LandingForArcade container, string _title) {
        this.title = _title;
        foreach (Text field in GetComponentsInChildren<Text>())
            field.text = _title;

        this.container = container;
    }
	public void Clicked () {
        container.Selected(title);
    }
}
