using System.Collections;
using UnityEngine;

public class LevelChanger : SceneObject {

	public Transform logo_videogame_0;
	public Transform logo_videogame_1;
	public Transform logo_videogame_2;

	int actualVideogameID;
	int videogame1 = 0;
	int videogame2 = 0;
	bool isDone;

	public override void OnRestart(Vector3 pos)
	{
		isDone = false;
		Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
		base.OnRestart( pos );
		VideogamesData videogamesData = Data.Instance.videogamesData;
		actualVideogameID = videogamesData.actualID;

//		if (actualVideogameID == 0) {
//			videogame1 = 1;
//			videogame2 = 2;
//		} else if (actualVideogameID == 1) {
//			videogame1 = 0;
//			videogame2 = 2;
//		} else {
//			videogame1 = 0;
//			videogame2 = 1;
//		}
//		logo_videogame_0.sprite = videogamesData.all[0].floppyCover;
//		logo_videogame_1.sprite = videogamesData.all[1].floppyCover;
//		logo_videogame_2.sprite = videogamesData.all[2].floppyCover;
	}
	public virtual void OnPool()  { 
		Reset ();
	}
	void OnDisable()  { 
		Reset ();
	}
	void OnDestroy()  { 
		Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
	}
	void Reset()  { 
		StopAllCoroutines ();
		Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
	}
	void OnListenerDispatcher(ListenerDispatcher.myEnum message)
	{
		if (isDone)
			return;
		isDone = true;
		int videogameId = 0;
		Transform t = null;
		if (message == ListenerDispatcher.myEnum.LevelChangeLeft) {
			videogameId = 1;
			t = logo_videogame_1.transform;
		} else if (message == ListenerDispatcher.myEnum.LevelChangeRight) {
			videogameId = 2;
			t = logo_videogame_2.transform;
		} else if (message == ListenerDispatcher.myEnum.LevelChangeCenter) {
			videogameId = 0;
			t = logo_videogame_0.transform;
		}
		
		if (t == null)
			return;

		Data.Instance.voicesManager.PlaySpecificClipFromList (Data.Instance.voicesManager.videogames_names, videogameId);
		Game.Instance.level.charactersManager.gameOver = true;

		Data.Instance.events.OnCameraZoomTo (t.position);
		StartCoroutine (GotoVideogame(videogameId));

	}
	IEnumerator GotoVideogame(int videogameID)
	{
		yield return new WaitForSecondsRealtime (1f);
		Game.Instance.ChangeVideogame(videogameID);
	}
}
