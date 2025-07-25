using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionsTopPanel : MonoBehaviour
{
    private Animation anim;
	public Text field;

    void Start()
    {
        anim =  GetComponent<Animation>();
        Events.OnMissionComplete += OnMissionComplete;
		Events.OnMissionProgress += OnMissionProgress;

    }
    void OnDisable()
    {
        Events.OnMissionComplete -= OnMissionComplete;
		Events.OnMissionProgress -= OnMissionProgress;
    }
    private void OnMissionComplete(int levelID)
    {
        anim.Play("MissionTopClose");
    }

	void OnMissionProgress()
	{
		print ("OnMissionProgres");
		anim.Play ("MissionActive");
	}
}
