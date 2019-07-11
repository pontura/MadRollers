using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    public RawImage rawImage;
    public RenderTexture[] all;
    public MainMenuCharacterActor[] characters;

    void Start()
    {
        Reset();
        SetActive(UserData.Instance.playerID);
        Data.Instance.events.ChangePlayer += ChangePlayer;
    }
    void OnDestroy()
    {
        Data.Instance.events.ChangePlayer -= ChangePlayer;
    }
    public void Next()
    {
        UserData.Instance.playerID--;
        if (UserData.Instance.playerID < 0)
            UserData.Instance.playerID = 3;
        ChangePlayer(UserData.Instance.playerID);
    }
    public void Prev()
    {
        UserData.Instance.playerID++;
        if (UserData.Instance.playerID >3)
            UserData.Instance.playerID = 0;
        ChangePlayer(UserData.Instance.playerID);
    }
    void ChangePlayer(int id)
    {
        PlayerPrefs.SetInt("playerID", id);
        SetActive(id);
    }
    void SetActive(int id)
    {
        UserData.Instance.playerID = id;
        rawImage.texture = all[id];
        UserData.Instance.playerID = id;
        characters[id].gameObject.SetActive(true);
    }
    void Reset()
    {
        foreach (MainMenuCharacterActor ch in characters)
            ch.gameObject.SetActive(false);
    }
}
