using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class ScoreLine : MonoBehaviour {

    public Image avatarImage;
    public Text num;
    public Text username;
    public Text score;

	public void Init (int _puesto, string _username, int _score) {
        if (num != null)
        {
            if (_puesto != 0)
                num.text = _puesto.ToString();
            else
                num.text = "";
        }
		username.text = _username;
		score.text = Utils.FormatNumbers(_score);
	}
    public void SetImage(string userID)
    {
        UserData.Instance.avatarImages.GetImageFor(userID, OnLoaded);
    }
    void OnLoaded(Texture2D texture2d)
    {
        if(avatarImage != null)
        avatarImage.sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2(0.5f, 0.5f));
    }
}
