using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

public class ArcadeSummary : MonoBehaviour {

    public Text ScoreLabel;
    public ArcadeMenuButton button1;
    public ArcadeMenuButton button2;
    public ArcadeMenuButton button3;

    public bool ready;

    public int score;

    public int id;

	void Start () {
        SetOn();
        score = Data.Instance.scoreForArcade;
        ScoreLabel.text = score.ToString();
	}

    void Update()
    {
        if(ready) return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Application.Quit();
        } else
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            id++;
            if (id == 4) id = 1;
            SetOn();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            id--;
            if (id == 0) id = 3;
            SetOn();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (id)
            {
                case 1: OpenHiscores(); break;
                case 2: Data.Instance.LoadLevel("GameForArcade"); break;
                case 3: Application.Quit(); break;
            }
        }
    }
	
	// Update is called once per frame
	void SetOn () {
        button1.SetOff();
        button2.SetOff();
        button3.SetOff();
        switch (id)
        {
            case 1: button1.SetOn(); break;
            case 2: button2.SetOn(); break;
            case 3: button3.SetOn(); break;
        }

	}
    void OpenHiscores()
    {

        ready = true;
        string[] content = new string[] { "MR_" + score };
#if UNITY_STANDALONE
        File.WriteAllLines("C:\\tumbagames\\hiscores\\data.txt", content);
        Invoke("timeOut1", 2);

        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
#endif
    }
    #if UNITY_STANDALONE
    void timeOut1()
    {
        Process foo = new Process();
        foo.StartInfo.FileName = "openHiscores.bat";
        foo.Start();
        Invoke("timeOut2", 2);
    }
    void timeOut2()
    {
        Application.Quit();
    }
#endif
}
