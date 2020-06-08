using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Intro : MonoBehaviour {

    List<Player> all;
    public List<GameObject> bosses;
    public Transform containerVideogames;
    public Transform containerBosses;
    public Transform container;
    public Player player_to_instantiate;
    public Text field;
    int id;

    void Start () {
        Data.Instance.events.OnJoystickClick += OnJoystickClick;
        container.gameObject.SetActive(true);
        containerBosses.gameObject.SetActive(false);
        containerVideogames.gameObject.SetActive(false);
        id = 1;
        Invoke("Step", 1);
        AddPlayers();
    }
    private void OnDestroy()
    {
        Data.Instance.events.OnJoystickClick -= OnJoystickClick;
    }
    void Step()
    {
        switch(id)
        {
            case 1:
                Data.Instance.handWriting.WriteTo(field, "YOU ARE A COMPUTER VIRUS", OnDone);
                break;
            case 2:
                container.gameObject.SetActive(false);
                containerBosses.gameObject.SetActive(true);
                containerVideogames.gameObject.SetActive(false);
                Loop();
                Data.Instance.handWriting.WriteTo(field, "...A PIXEL DEVOURING SOFTWARE", OnDone);
                break;
            case 3:
                Loop();
                Data.Instance.handWriting.WriteTo(field, "AND YOUR MISSION IS...", OnDone);
                break;
            case 4:
                container.gameObject.SetActive(false);
                containerBosses.gameObject.SetActive(false);
                containerVideogames.gameObject.SetActive(true);
                Data.Instance.handWriting.WriteTo(field, "DESTROY ALL THE VIDEO-GAMES OVER THE FACE OF THE EARTH.", OnDone);
                break;
            case 5:
                Data.Instance.handWriting.WriteTo(field, "GOOD LUCK!", OnDone);
                break;
            case 6:
                Data.Instance.LoadLevel("MainMenuMobile");
                break;
        }       
    }
    public void OnDone()
    {
        Invoke("NextStep", 6);
    }
    void OnJoystickClick()
    {
        NextStep();
    }
    public void NextStep()
    {
        CancelInvoke();
        id++;
        Step();
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
        Loop();
    }
    void Loop()
    {
        switch (id)
        {
            case 1:
                foreach (Player p in all)
                    p.gameObject.SetActive(false);
                all[playerID].gameObject.SetActive(true);
                playerID++;
                if (playerID > 3)
                    playerID = 0;
                break;
            case 2:
            case 3:
                foreach (GameObject p in bosses)
                    p.gameObject.SetActive(false);
                bosses[playerID].gameObject.SetActive(true);
                playerID++;
                if (playerID >= bosses.Count)
                    playerID = 0;
                break;
        }
        Invoke("Loop", 0.5f);
    }

}
