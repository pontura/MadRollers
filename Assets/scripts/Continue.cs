using UnityEngine;
using UnityEngine.UI;

public class Continue : MonoBehaviour {

	public Image icon;
	public GameObject panel;
	private int num = 9;
	public Text countdown_txt;
	public Text credits_txt;
	private float speed = 0.5f;
    private bool canClick;

	void Start () {
		canClick = false;
		panel.SetActive (false);

        

        //if (Data.Instance.isAndroid)
         //   return;


        if (Data.Instance.playMode == Data.PlayModes.CONTINUEMODE) {
			countdown_txt.fontSize = 41;
			icon.enabled = false;
		}
        Data.Instance.events.OnGameOver += OnGameOver;



    }
	void Update()
	{
		if (canClick) {
            for (int a = 0; a < 4; a++)
            {
                if (Input.GetMouseButtonDown(0))
                    OnJoystickClick();
            }
		}
	}
    void OnDestroy()
	{
		Data.Instance.events.OnGameOver -= OnGameOver;
	}
	void OnGameOver(bool isTimeOver)
	{	
		Invoke ("OnGameOverDelayed", 2);
	}	
	public void OnGameOverDelayed()
	{
        if (Data.Instance.playMode == Data.PlayModes.STORYMODE || Data.Instance.playMode == Data.PlayModes.SURVIVAL)
        {
            Invoke("Done", 2);
            return;
        }
        if (Data.Instance.playMode == Data.PlayModes.PARTYMODE) {
			if (!Data.Instance.canContinue || Data.Instance.credits == 0) {
				Invoke ("Done", 2);
				return;
			}	
			credits_txt.text = Data.Instance.credits + " " + TextsManager.Instance.GetText("CREDITS"); 
		} else
			credits_txt.text = "";
		panel.SetActive (true);
		num = 9;
		countdown_txt.text = num.ToString();
		Invoke ("Loop", 0.3f);
	}	
	public void Loop()
	{
		canClick = true;
		num--;
		if(num<=0)
		{
			canClick = false;
			panel.GetComponent<Animation> ().Play ("signalOff");
			Invoke ("Done", 1f);
			return;
		}
		countdown_txt.text = num.ToString();
		Invoke ("Loop", speed);
	}	
	void Done()
	{
        if (Data.Instance.playMode == Data.PlayModes.STORYMODE || Data.Instance.playMode == Data.PlayModes.SURVIVAL)
        {
          //  GetComponent<SummaryCompetitions>().SetOn();
        }
        else
        {
            GetComponent<HiscoresComparison>().Init();
        }
		panel.SetActive (false);
	}
	void OnJoystickClick()
	{
		if (canClick) {
			canClick = false;
			CancelInvoke ();
			Game.Instance.Continue();  
		}
	}

}
