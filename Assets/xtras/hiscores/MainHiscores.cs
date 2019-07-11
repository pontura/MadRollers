using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class MainHiscores : MonoBehaviour {

    public ScoreLine scoreLineNewHiscore;
    public ScoreLine scoreLine;
    public Transform container;

    public Text titleField;
    public Text puestoField;
    public Text field;

    public LeterChanger[] letters;
    public int lettersID;
    public LeterChanger letterActive;
    public int puesto;

	int _hiscore = 0;

    public List<Hiscore> arrengedHiscores;
    public List<Hiscore> hiscores;

	bool done;

    [Serializable]
    public class Hiscore
    {
        public string username;
        public int hiscore;       
    }
	void Start () {
		done = false;
		_hiscore = Data.Instance.multiplayerData.GetTotalScore ();
		
        puesto = 1;
        Screen.fullScreen = true;
		LoadHiscores(Data.Instance.GetComponent<ArcadeRanking>().path);
        puestoField.text = "PUESTO " + puesto;
		field.text = _hiscore.ToString ();

		Data.Instance.events.OnJoystickRight += OnJoystickRight;
		Data.Instance.events.OnJoystickLeft += OnJoystickLeft;
		Data.Instance.events.OnJoystickClick += OnJoystickClick;
	//	Data.Instance.events.OnJoystickBack += OnJoystickBack;

		foreach (LeterChanger letterChanger in letters)
		{
			letterChanger.SetActivate(false);
			letterChanger.GetComponent<Animation>().Play("letterOff");
		}
		letters [0].SetActivate (true);
		letters [0].GetComponent<Animation>().Play("letterOn");
	}
	void OnDestroy()
	{
		Data.Instance.events.OnJoystickRight -= OnJoystickRight;
		Data.Instance.events.OnJoystickLeft -= OnJoystickLeft;
		Data.Instance.events.OnJoystickClick -= OnJoystickClick;
		//	Data.Instance.events.OnJoystickBack -= OnJoystickBack;
	}
	void OnJoystickRight()
	{
		letterActive.ChangeLetter(false);
	}
	void OnJoystickLeft()
	{
		letterActive.ChangeLetter(true);
	}
	void OnJoystickClick()
	{
		SetLetterActive(true);
	}
//	void OnJoystickBack()
//	{
//		SetLetterActive(false);
//	}
//    void Update()
//    {
//        if (Input.GetKeyUp(KeyCode.LeftArrow))
//            letterActive.ChangeLetter(false);
//        else if (Input.GetKeyUp(KeyCode.RightArrow))
//            letterActive.ChangeLetter(true);
//        else if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.Space))
//            SetLetterActive(true);
//        else if (Input.GetKeyUp(KeyCode.Alpha1))
//            SetLetterActive(false);
//    }
    void SetLetterActive(bool next)
    {
        foreach (LeterChanger letterChanger in letters)
        {
            letterChanger.SetActivate(false);
            letterChanger.GetComponent<Animation>().Play("letterOff");
        }
        if (next) lettersID++;
        else lettersID--;
        if (lettersID < 0)
        {
			lettersID = 0;
            return;
        }
        if (lettersID > letters.Length - 1)
        {
            Save();
            return;
        }

        letterActive = letters[lettersID];

        letterActive.SetActivate(true);
        letterActive.GetComponent<Animation>().Play("letterOn");
    }
    private bool yaAgrego;
    void LoadHiscores(string fileName)
    {
            String[] arrLines = File.ReadAllLines(fileName);
            int num = 1;
            foreach (string line in arrLines)
            {
                string[] lines = line.Split("_"[0]);
                Hiscore hiscore = new Hiscore();
                hiscore.username = lines[0];
                hiscore.hiscore = int.Parse(lines[1]);
                hiscores.Add(hiscore);

                if (hiscore.hiscore < _hiscore && !yaAgrego)
                {
                    yaAgrego = true;
                    puesto = num;
                    if (num < 16)
                    {
                        ScoreLine newScoreLine = Instantiate(scoreLineNewHiscore);
						newScoreLine.Init(num, "XXXXX", _hiscore);
                        newScoreLine.transform.SetParent(container);
                        newScoreLine.transform.localScale = Vector3.one;
                        num++;
                    }                    
                }

                if(num<16)
                {
                    ScoreLine newScoreLine = Instantiate(scoreLine);                
                    newScoreLine.Init(num, hiscore.username, hiscore.hiscore);               
                    newScoreLine.transform.SetParent(container);
                    newScoreLine.transform.localScale = Vector3.one;
                }               

                num++;
            } 
     }
    void Save()
    {
		
		if (done)
			return;
		
		done = true;

        string username = "";
        foreach (LeterChanger letterChanger in letters)
        {
            string letra = letterChanger.field.text;
            if (letra == "_") letra = " ";
            username += letra;
        }
		SaveNew(Data.Instance.GetComponent<ArcadeRanking>().path, username, _hiscore);
    }
    public void SaveNew(string fileName, string username, int newHiscoreToSave)
    {
		Hiscore newHiscore = new Hiscore ();
		newHiscore.username = username;
		newHiscore.hiscore = newHiscoreToSave;
		hiscores.Add (newHiscore);

		arrengedHiscores = OrderByHiscore (hiscores);

		String[] arrLines = new String[hiscores.Count];
		int a = 0;
		foreach (Hiscore hs in arrengedHiscores) {
			arrLines [a] = hs.username + "_" + hs.hiscore;
			a++;
		}
		File.WriteAllLines (fileName, arrLines);
		Invoke ("grabaEnd", 0.25f);
    }
    void grabaEnd()
    {
		Data.Instance.events.RefreshHiscores ();
		Data.Instance.missions.MissionActiveID = 0;
		Data.Instance.events.OnResetScores();
		Data.Instance.events.ForceFrameRate (1);
		Data.Instance.LoadLevel("MainMenu");
    }
    List<Hiscore> OrderByHiscore(List<Hiscore> hs)
    {
        return hs.OrderBy(go => go.hiscore).Reverse().ToList();
    }

}
