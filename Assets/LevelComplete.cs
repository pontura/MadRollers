using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelComplete : MonoBehaviour {

	public GameObject panel;
    public Text[] fields;

	void Start()
	{
		panel.SetActive(false);
	}
     void OnDestroy()
     {
		fields = null;
     }
    public void Init(int missionNum)
    {
		Data.Instance.events.RalentaTo (0.6f, 0.05f);
		panel.SetActive (true);
	//	int maxScore = Data.Instance.GetComponent<Missions>().GetActualMissionData().maxScore;
      //  int missionScore = Data.Instance.userData.missionScore;
    //    int quarter = maxScore / 4;

		string titleText ="";

		foreach (Text label in fields)
			Data.Instance.handWriting.WriteTo(label, titleText, null);
      
       // Data.Instance.events.OnSetStarsToMission(missionNum, starsQty);

		CloseAfter (3);
    }
	void CloseAfter(float delay)
	{
		StartCoroutine (Closing(delay));
	}
	IEnumerator Closing(float delay)
	{
		yield return StartCoroutine(Utils.CoroutineUtil.WaitForRealSeconds (delay));
		Close ();
	}
	void OnDisable()
	{
		Close();
	}

	public void Close()
	{
		Data.Instance.events.RalentaTo (1, 0.05f);
		Game.Instance.level.charactersManager.ResetJumps ();
		panel.SetActive (false);
	}
}
