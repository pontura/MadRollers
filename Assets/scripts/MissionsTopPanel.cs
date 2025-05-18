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
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
		Data.Instance.events.OnMissionProgress += OnMissionProgress;

    }
    void OnDisable()
    {
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
		Data.Instance.events.OnMissionProgress -= OnMissionProgress;
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
