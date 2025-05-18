using UnityEngine;
using System.Collections;

public class MainMenuCharacterActor : MonoBehaviour {

    public int id;
	public Player player_to_instantiate;
	public Transform container;

	void Start () {
		Player p = Instantiate (player_to_instantiate);
		p.id = id - 1;
		p.transform.SetParent (container);
		p.transform.localScale = Vector3.one;
		p.transform.localPosition = Vector3.zero;
        p.isPlaying = false;

    }
}
