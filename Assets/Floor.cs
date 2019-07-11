using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor : MonoBehaviour
{
	float offset = 40;
	public Transform container;
    public int z_length;
	int new_z_length;
	public bool isMoving;
	public float speed = 2;
	public List<BackgroundSideData> all;
	string lastBackgroundSideName;
	public CharactersManager charactersManager;
	VideogameBossPanel videoGameBossPanel;

	void Start()
    {
		videoGameBossPanel = Game.Instance.GetComponent<VideogameBossPanelsManager> ().actualBossPanel;
        isMoving = true;
        Data.Instance.events.OnGamePaused += OnGamePaused;
		Data.Instance.events.OnGameOver += OnGameOver;
		Data.Instance.events.OnChangeBackgroundSide += OnChangeBackgroundSide;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnGamePaused -= OnGamePaused;
		Data.Instance.events.OnGameOver -= OnGameOver;
		Data.Instance.events.OnChangeBackgroundSide -= OnChangeBackgroundSide;
      //  Data.Instance.events.OnChangeMood -= OnChangeMood;
    }
    void OnGamePaused(bool paused)
    {
        isMoving = !paused;
    }
    void OnChangeMood(int id)
    {
		return;
    }
	void OnGameOver(bool isTimeOver)
    {
        isMoving = false;
    }

	float pos_z = 0;
	float lastCharactersDistance;
    void Update()
    {
        if (!isMoving) return;
        if (!charactersManager) return;
		float charactersDistance = charactersManager.getDistance ();

		if(videoGameBossPanel != null)
			videoGameBossPanel.transform.localPosition = new Vector3 (0,0,charactersDistance);
		
		if (charactersDistance == lastCharactersDistance)
			return;
		
		lastCharactersDistance = charactersDistance;
		pos_z += (Time.deltaTime*speed);
		BackgroundSideData toDelete = null;
		foreach (BackgroundSideData go in all) {

			Vector3 pos = go.transform.localPosition;
			
			if (pos.z < charactersDistance - offset) 
				go.offset += z_length;
			
			
			pos.z = charactersDistance + go.offset - pos_z;
			go.transform.localPosition = pos;
		}

    }
	void OnChangeBackgroundSide(string backgroundName)
	{
		int i = container.childCount;
		while (i > 0) {
			Destroy (container.GetChild(i-1).gameObject);
			i--;
		}
		all.Clear ();

		for (int a = 0; a < 3; a++) {
			BackgroundSideData bsd = Instantiate (Resources.Load ("fondos/" + backgroundName, typeof(BackgroundSideData))) as BackgroundSideData;
			all.Add (bsd);
		}

		z_length = 0;
		foreach (BackgroundSideData data in all) {
			z_length += data.z_length;
			data.offset = z_length;
			AddNewBgSide (data);
		}
	}
	void AddNewBgSide(BackgroundSideData newGO)
	{
		newGO.transform.SetParent (container);
		newGO.transform.localPosition = new Vector3(0,0,z_length);
		newGO.transform.localScale = Vector3.one;
	}
}
