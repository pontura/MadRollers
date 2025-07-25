using UnityEngine;
public class Gui : MonoBehaviour {
    
	public LevelComplete levelComplete;

    private Data data;   

	private int barWidth = 200;
    private bool MainMenuOpened = false;

	public TMPro.TMP_Text genericField;
	public GameObject centerPanel;

	void Start()
	{
		centerPanel.SetActive (false);
        Events.OnAvatarCrash += OnAvatarCrash;
        Events.OnAvatarFall += OnAvatarCrash;

        if (Data.Instance.playMode != Data.PlayModes.SURVIVAL)
            Events.OnBossActive += OnBossActive;

		Events.OnGenericUIText += OnGenericUIText;
        Events.OnGameOver += OnGameOver;
        Events.ResetHandwritingText += ResetHandwritingText;
    }
    void OnDestroy()
    {
        Events.OnAvatarCrash -= OnAvatarCrash;
        Events.OnAvatarFall -= OnAvatarCrash;
		Events.OnBossActive -= OnBossActive;
		Events.OnGenericUIText -= OnGenericUIText;
        Events.OnGameOver -= OnGameOver;
        Events.ResetHandwritingText -= ResetHandwritingText;

        levelComplete = null;
    }
    void OnGameOver(bool isOver)
    {
        CancelInvoke();
        Reset();
    }
    void OnBossActive(bool isOn)
	{
		CancelInvoke ();
		Reset ();
		if (isOn) {
			OnGenericUIText( "Kill 'em all");
		} else {
            if (Data.Instance.playMode == Data.PlayModes.STORYMODE)
            {
                GetComponent<SummaryMobile>().Init();
                return;
            }
            else
            {
                levelComplete.gameObject.SetActive(true);
                levelComplete.Init(Data.Instance.missions.MissionActiveID);
            }
		}
		Invoke ("Reset", 2);
	}
	void OnGenericUIText(string text)
	{
		centerPanel.SetActive (true);
		Data.Instance.handWriting.WriteTo(genericField, text, null);
		CancelInvoke ();
		Invoke ("Reset", 2);
	}
    void ResetHandwritingText()
    {
        CancelInvoke();
        genericField.text = "";
    }
    void Reset()
	{
		levelComplete.gameObject.SetActive(false); 
		centerPanel.SetActive (false);
	}
    void OnAvatarCrash(CharacterBehavior cb)
    {
        levelComplete.gameObject.SetActive(false); 
    }
   
}
