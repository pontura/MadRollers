using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarThumb : MonoBehaviour
{
    public Image image;
    bool loaded;

    public void Init(string userID)
    {
        UserData.Instance.avatarImages.GetImageFor(userID, OnLoaded);      
    }
    void OnLoaded(Texture2D texture2d)
    {
        image.sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2(0.5f, 0.5f));
    }
    public void Reset()
    {
        image.sprite = null;
    }
}
