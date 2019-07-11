using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Intro : MonoBehaviour {

    public GameObject hero1;
    public GameObject hero2;
    public GameObject hero3;
    public GameObject hero4;

    public MeshRenderer gorro1;
    public MeshRenderer gorro2;
    public MeshRenderer gorro3;
    public MeshRenderer gorro4;

    public GameObject fonts;
    private string title = "ROMPAN-TODO " ;
    private int id;

	void Start () {

        MultiplayerData multiplayerData = Data.Instance.multiplayerData;

        gorro1.material.color = multiplayerData.colors[0];
        gorro2.material.color = multiplayerData.colors[1];
        gorro3.material.color = multiplayerData.colors[2];
        gorro4.material.color = multiplayerData.colors[3];

        if (!multiplayerData.player1) hero1.SetActive(false);
        if (!multiplayerData.player2) hero2.SetActive(false);
        if (!multiplayerData.player3) hero3.SetActive(false);
        if (!multiplayerData.player4) hero4.SetActive(false);

        Invoke("Loop", 2);

	}
    void Loop()
    {
            Data.Instance.LoadLevel("Game");
    }

}
