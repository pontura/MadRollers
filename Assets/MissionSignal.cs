using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionSignal : MonoBehaviour {

	public GameObject panel;
	public Text subtitle;
	Missions missions;
	void Start () {
		missions =  Data.Instance.GetComponent<Missions> ();
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        Data.Instance.events.ResetHandwritingText += ResetHandwritingText;

        SetState(false);
	}
    void OnDestroy()
    {
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
        Data.Instance.events.ResetHandwritingText -= ResetHandwritingText;
    }
	void SetState(bool isOff)
    {
		panel.SetActive (isOff);
    }
    void ResetHandwritingText()
    {
        subtitle.text = "";
    }

    private void OnListenerDispatcher(ListenerDispatcher.myEnum message)
    {
        if (message == ListenerDispatcher.myEnum.ShowMissionName)
		{
            MissionData missionData = missions.GetActualMissionData();
            if (missions.MissionActiveID == 0)
                return;

			subtitle.text = "";
			SetState(true);
			string text = "";			
			text += "MISION " + (missions.MissionActiveID+1) + "\n";
			text += missionData.title;

            Data.Instance.handWriting.WriteTo(subtitle,text , DoneText2);
		}
	}
	void DoneText2()
	{
		StartCoroutine( CloseAfter(2.5f) );
	}
	IEnumerator CloseAfter(float delay)
	{
		yield return StartCoroutine(Utils.CoroutineUtil.WaitForRealSeconds (delay));
		SetState(false);
    }
}
