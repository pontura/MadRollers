using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelComplete : MonoBehaviour {

	public GameObject panel;
    public TMPro.TMP_Text[] fields;

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
		Events.RalentaTo (0.6f, 0.05f);
		panel.SetActive (true);
	//	int maxScore = Data.Instance.GetComponent<Missions>().GetActualMissionData().maxScore;
      //  int missionScore = Data.Instance.userData.missionScore;
    //    int quarter = maxScore / 4;

		string titleText ="";

		foreach (TMPro.TMP_Text label in fields)
			Data.Instance.handWriting.WriteTo(label, titleText, null);

        // Events.OnSetStarsToMission(missionNum, starsQty);
        if (Data.Instance.playMode == Data.PlayModes.STORYMODE || Data.Instance.playMode == Data.PlayModes.SURVIVAL)
            return;
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
		Events.RalentaTo (1, 0.05f);
		Game.Instance.level.charactersManager.ResetJumps ();
		panel.SetActive (false);
	}
}
