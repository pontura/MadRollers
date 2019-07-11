using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommodoreUI : MonoBehaviour {

	public Text field;
//	public string[] texts;

	public types type;
	public enum types
	{
		HISCORE,
		GAME_COMPLETED,
        CREDITS
	}

	public Sprite[] spritesToBG;
	public GameObject bg;
	public Image[] backgroundImages;
	float speed;
	bool isOn;

	void Start () {
		ChangeBG ();
		SetOn (true);
	}
	public void SetOn(bool _isOn)
	{
		this.isOn = _isOn;
		if (isOn) {
			
			//Data.Instance.events.OnSoundFX("loading", -1);
			field.text = "";	

			if(type == types.HISCORE)
				StartCoroutine (LoadingRoutine ());
			else if(type == types.GAME_COMPLETED)
				StartCoroutine (LoadingRoutine_GAME_COMPLETED ());
            else if (type == types.CREDITS)
                StartCoroutine(LoadingRoutine_CREDITS());
        }
	}
	IEnumerator LoadingRoutine()
	{
		field.text = "";		
		AddText("*** MAD ROLLERS ***");
		yield return new WaitForSeconds (0.8f);
		AddText("Hacking " + Data.Instance.videogamesData.GetActualVideogameData ().name + " -> scores.list");
		yield return new WaitForSeconds (0.5f);
		AddText("Commander64 P_HASH[ASDL??89348");
		yield return new WaitForSeconds (0.8f);
		AddText("Write Permisson Accepted!");
		yield return new WaitForSeconds (0.5f);
		yield return null;
	}
	IEnumerator LoadingRoutine_GAME_COMPLETED()
	{
			
		AddText("SYSTEM VIOLATED! INTEGRITY CONSTRAINT");
		yield return new WaitForSeconds (0.8f);
		AddText("(SYSTEM.SYS_C007150 " + Data.Instance.videogamesData.GetActualVideogameData ().name + " -> scores.list");
		yield return new WaitForSeconds (1.3f);
		AddText("Commander64 P_HASH[VIOLATED - PARENT KEY is fucked!!]");
		yield return new WaitForSeconds (2);
		AddText("*** ERROR 666 ***");
		yield return new WaitForSeconds (1.3f);
		AddText("DIE!");
		yield return null;
	}
    IEnumerator LoadingRoutine_CREDITS()
    {
        AddText("MAD ROLLERS");
        yield return new WaitForSeconds(0.8f);
        AddText("Es un juego independiente hecho en Argentina por 2 amigos");
        yield return new WaitForSeconds(1f);
        AddText("TUMBA-GAMES");
        yield return new WaitForSeconds(1.3f);
        AddText("PONTURA -> Idea, Programación y Desarrollo general");
        yield return new WaitForSeconds(0.5f);
        AddText("DARIO GEORGES -> Arte, músicas y magias");
        yield return new WaitForSeconds(2);
        AddText("-------");
        yield return new WaitForSeconds(1.3f);
        AddText("MATEO AMARAL -> Arte 3d de los MAD ROLLERS");
        yield return new WaitForSeconds(1.3f);
        AddText("NICO VIEGAS -> Arte de Meduzas y algunos assets de ese nivel");
        yield return new WaitForSeconds(1.3f);
        AddText("SEB LAB, JP AMATO -> sound FX");
        yield return new WaitForSeconds(2);
        AddText("-------");
        yield return new WaitForSeconds(1.3f);
        AddText("Agradecimientos especiales a:");
        yield return new WaitForSeconds(1.3f);
        AddText("HERNAN SAEZ, NICO COHEN, PALOMA, PELA, KIKO, TAM, LOS PATMORITAS, PEPSICO, BURRIS, MOCO, GAITA, TONI, SOFI, BER, MMM, TEJADA, YANI, GALLARDOS, POLO y todos los nocturnos que playtestearon y playtestean...");
        yield return null;
    }
    void AddText(string text)
	{
		field.text += text +'\n';
	}

	void Update () {

		if (!isOn)
			return;

		Vector2 pos = bg.transform.localPosition;
		pos.y += speed * Time.deltaTime;
		if (pos.y > 0) {
			ChangeBG ();
			pos.y = -90;
		}
		bg.transform.localPosition = pos;
		Invoke ("ChangeSpeed", (float)Random.Range (5, 30) / 10);
	}
	void ChangeSpeed()
	{
		speed = (float)Random.Range (100, 400);
	}
	void ChangeBG()
	{
		Sprite s = spritesToBG [Random.Range (0, spritesToBG.Length)];
		foreach (Image image in backgroundImages)
			image.sprite = s;
	}
}
