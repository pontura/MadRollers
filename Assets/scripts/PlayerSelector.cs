using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    public Transform container;
    public Player player_to_instantiate;
    List<Player> all;
    public GameObject buttons;

    void Start()
    {
        if (!Data.Instance.isAndroid)
            buttons.SetActive(false);
        AddPlayers();
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
        if (UserData.Instance.playerID > 3)
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

        foreach (Player p in all)
            p.gameObject.SetActive(false);

        all[id].gameObject.SetActive(true);
    }
    int playerID;
    void AddPlayers()
    {
        all = new List<Player>();
        for (int a = 0; a < 4; a++)
        {
            Player p = Instantiate(player_to_instantiate);
            p.isPlaying = false;
            p.transform.SetParent(container);
            p.id = a;
            p.transform.localPosition = Vector3.zero;
            p.transform.localScale = Vector3.one;
            p.transform.localEulerAngles = Vector3.zero;
            all.Add(p);
        }
    }
}