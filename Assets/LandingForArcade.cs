using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LandingForArcade : MonoBehaviour {

    public MultiplayerCompetitionButton button;
    public Transform container;
    public Toggle switchPlayers;
    public Toggle musicOn;
    public Text camerasField;

    public MeshRenderer rawImage;
    WebCamTexture webCamTexture;

    void Start () {
       // Data.Instance.WebcamID = WebCamTexture.devices.Length - 1;
        Invoke("Next", 1);
        UpdateWebCam();
        rawImage.material.mainTexture = webCamTexture;
    }
    public void ToogleCam()
    {
        webCamTexture.Stop();
//        if (Data.Instance.WebcamID < WebCamTexture.devices.Length - 1)
//            Data.Instance.WebcamID++;
//        else
//            Data.Instance.WebcamID = 0;
        Invoke("UpdateWebCam", 0.2f);
    }
    void UpdateWebCam()
    {
//		if (WebCamTexture.devices.Length == 0)
//			return;
//        camerasField.text = "cam: " + Data.Instance.WebcamID + "/" + WebCamTexture.devices.Length;
//        webCamTexture = new WebCamTexture(WebCamTexture.devices[Data.Instance.WebcamID].name, 800, 600, 12);
//        webCamTexture.Play();
//        Vector3 scale = rawImage.transform.localScale;
    }
    void Next()
    {
        if (Data.Instance.isArcadeMultiplayer)
            LoopUntilCompetitionsReady(); // Data.Instance.LoadLevel("MainMenuArcade");
        else
            Data.Instance.LoadLevel("MainMenu");
    }
    void LoopUntilCompetitionsReady()
    {
//        print(Data.Instance.GetComponent<MultiplayerCompetitionManager>().competitions.Count);
//        if (Data.Instance.GetComponent<MultiplayerCompetitionManager>().competitions.Count==0)
//            Invoke("LoopUntilCompetitionsReady", 1);
//        else
//        {
//            LoadCompetitions();
//        }
    }
    void LoadCompetitions()
    {
//        Cursor.visible = true;
//
//        foreach (string title in Data.Instance.GetComponent<MultiplayerCompetitionManager>().competitions)
//        {
//            MultiplayerCompetitionButton newButton = Instantiate(button);
//            newButton.Init(this, title);
//            newButton.transform.SetParent(container);
//        }
    }
	public void TurnOffCam()
	{
		Data.Instance.webcamOff = true;

		if(webCamTexture != null)
			webCamTexture.Stop();
	}
    public void Selected(string competitionTitle)
    {
//		if(webCamTexture != null)
//      	  webCamTexture.Stop();
//
//        Data.Instance.switchPlayerInputs = switchPlayers.isOn;
//        Cursor.visible = false;
//        Data.Instance.GetComponent<MultiplayerCompetitionManager>().actualCompetition = competitionTitle;
//        Data.Instance.GetComponent<PhotosManager>().LoadPhotos();
//        Data.Instance.LoadLevel("MainMenu");
    }
    
}
