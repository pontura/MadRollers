using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FriendButton : MonoBehaviour {

    [SerializeField] 
    Text usernameLabel;
    [SerializeField]
    Text scoreLabel;
    [SerializeField]
    Image img;

    public void Init(string username, string score)
    {
        usernameLabel.text = username;
        scoreLabel.text = score.ToString();
    }
}
