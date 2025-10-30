using UnityEngine;

public class ScoreLine : MonoBehaviour {

    Animator anim;
    public TMPro.TMP_Text num;
    public TMPro.TMP_Text username;
    public TMPro.TMP_Text score;

	public void Init (int _puesto, string _username, int _score) {
        anim = GetComponent<Animator>();
        if (num != null)
        {
            if (_puesto != 0)
                num.text = _puesto.ToString();
            else
                num.text = "";
        }
		username.text = TruncateText(_username, 14);
		score.text = Utils.FormatNumbers(_score);

        if (anim == null) return;
        if(_username == UserData.Instance.username)
            anim.Play("me");
        else if (_puesto == 1)
            anim.Play("first");
        else
            anim.Play("idle");
    }
    public string TruncateText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text)) return "";
        if (text.Length <= maxLength) return text;
        return text.Substring(0, maxLength - 3) + "...";
    }
    public void SetImage(string userID)
    {
      //  UserData.Instance.avatarImages.GetImageFor(userID, OnLoaded);
    }
    void OnLoaded(Texture2D texture2d)
    {
        //if(avatarImage != null)
        //avatarImage.sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2(0.5f, 0.5f));
    }
}
