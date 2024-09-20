using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArcadeUILevelTransitions : MonoBehaviour {

    public GameObject panel;
    public GameObject texts;
    public GameObject texts2;
    public Image blackMask;

    private int level;

	void Start () {
        SetOff();
        level = 1;
        panel.SetActive(false);
      //  Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
		Data.Instance.events.ShowNotification += ShowNotification;
	}
    void OnDestroy()
    {
      //  Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
		Data.Instance.events.ShowNotification -= ShowNotification;
    }
    int percent = 0;
    bool ready;

	string lastNotification;
	void ShowNotification(string notification)
	{
		panel.SetActive(true);
		foreach (Text field in texts.GetComponentsInChildren<Text>())
			field.text = notification;

		foreach (Text field in texts2.GetComponentsInChildren<Text>())
			field.text = "";

		lastNotification = notification;

		Invoke("ResetNotification", 2);
	}
	void ResetNotification()
	{
		if (lastNotification == texts.GetComponentsInChildren<Text>()[0].text)
			SetOff ();
	}
//    void OnListenerDispatcher(string type)
//    {    
//		if (!Game.Instance.level.isLastArea)
//			return;
//        if (type == "Ralenta")
//        {
//            StopAllCoroutines();
//
//			StartCoroutine(DoFade(0.2f, 0.25f));
//			
//            panel.SetActive(true);
//            foreach (Text field in texts.GetComponentsInChildren<Text>())
//                field.text = "Rock!";
//             foreach (Text field in texts2.GetComponentsInChildren<Text>())
//                field.text = "";
//            return;
//        }
//        else if (type == "BonusEntrande")
//        {
//            panel.SetActive(true);
//            foreach (Text field in texts.GetComponentsInChildren<Text>())
//                field.text = "B O N U S !!!";
//            foreach (Text field in texts2.GetComponentsInChildren<Text>())
//                field.text = "";
//            return;
//        }
//
//        if (type == "LevelFinish_hard") percent += 100;
//        else if (type == "LevelFinish_medium") percent += 66;
//        else if (type == "LevelFinish_easy") percent += 33;
//
//        if(percent==0) return;
//        Invoke("Delay", 2f);
//	}
    
    void Delay()
    {
        if (ready) return;
        int totalCharacters = Game.Instance.level.charactersManager.getTotalCharacters();

        //puede que se hayan muerto todos antes
        if (totalCharacters == 0)
        {
            Reset();
            return;
        }

        float suma = (percent / totalCharacters);
        Game.Instance.level.SetDificultyByScore( (int)suma );

        panel.SetActive(true);
        //  panel.GetComponent<Animation>().Play("levelTransition");
//		if (Data.Instance.playMode == Data.PlayModes.STORY) {
//			SetTexts ("STAGE CLEAR!", "");
//		} else {
//			// StartCoroutine(DoFade());
//			foreach (Text field in texts.GetComponentsInChildren<Text>())
//				field.text = "Nivel " + (level + 1).ToString ();
//			foreach (Text field in texts2.GetComponentsInChildren<Text>()) {
//				switch (Game.Instance.level.Dificulty) {
//				case Level.Dificult.EASY:
//					field.text = "modo FáCIL";
//					break;
//				case Level.Dificult.MEDIUM:
//					field.text = "dificultad MEDIA";
//					break;
//				case Level.Dificult.HARD:
//					field.text = "modo EXTREMO!";
//					break;
//				}
//			}
//		}

        level++;
        ready = true;
        Invoke("Reset", 1);
    }
	void SetTexts(string title, string subtitle)
	{
		foreach (Text field in texts.GetComponentsInChildren<Text>())
			field.text = title;
		foreach (Text field in texts2.GetComponentsInChildren<Text>())
			field.text = subtitle;
	}
    void Reset()
    {
        percent = 0;
        ready = false;
    }
    void SetOff()
    {
        panel.SetActive(false);
        blackMask.enabled = false;
    }

	public IEnumerator DoFade(float delay, float waitInBlack)
    {
        yield return new WaitForSeconds(delay);
        blackMask.enabled = true;
        float t = 0;
        while (t < 1)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime / 3f;
            Color color = blackMask.color;
            color.a = t;
            blackMask.color = color;
        }
		yield return new WaitForSeconds(waitInBlack);
        Data.Instance.events.OnAlignAllCharacters();
        while (t > 0)
        {
            yield return new WaitForEndOfFrame();
            t -= Time.deltaTime / 3f;
            Color color = blackMask.color;
            color.a = t;
            blackMask.color = color;
        }
        SetOff();
    }
}
