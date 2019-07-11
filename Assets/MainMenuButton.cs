using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MainMenuButton : MonoBehaviour {
	
	public Text[] overs;
	public GameObject selected;
	public Color onColor;
	public Color offColor;

	public void SetOn(bool isOn)
	{
		if (isOn) {
			foreach (Text m in overs)
				m.color = onColor;
			selected.SetActive (true);
		} else {
			foreach (Text m in overs)
			{
				m.color = offColor;
			}
			selected.SetActive (false);
		}
	}

    //solo para Mobile: son los botones de cuando perdes!
    public int id;
    public void OnClicked()
    {

        switch (id)
        {
            case 0:
                Data.Instance.inputSavedAutomaticPlay.RemoveAllData();
                Data.Instance.isReplay = true;
                Game.Instance.ResetLevel();
                break;
            case 1:
                Data.Instance.inputSavedAutomaticPlay.RemoveAllData();
                Game.Instance.GotoLevelSelector();
                break;
        }
    }
}
