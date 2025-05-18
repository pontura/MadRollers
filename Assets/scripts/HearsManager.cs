using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HearsManager : MonoBehaviour {

	public Text[] labels;
    public int total;
    public int newHearts = 0;
  // public Animation anim;
    public ParticleSystem particles;

	void Start () {
        particles.gameObject.SetActive(false);
        newHearts = 0;
        total = PlayerPrefs.GetInt("totalHearts");
        Data.Instance.events.OnGrabHeart += OnGrabHeart;
        Data.Instance.events.OnAvatarFall += OnAvatarDie;
        Data.Instance.events.OnAvatarCrash += OnAvatarDie;
        Data.Instance.events.OnUseHearts += OnUseHearts;
        SetHearts();
	}
    void OnDestroy () {
        Data.Instance.events.OnGrabHeart -= OnGrabHeart;
        Data.Instance.events.OnAvatarFall -= OnAvatarDie;
        Data.Instance.events.OnAvatarCrash -= OnAvatarDie;
        Data.Instance.events.OnUseHearts -= OnUseHearts;
	}
    void OnUseHearts(int qty)
    {
        particles.gameObject.SetActive(true);

        particles.Play();
        total -= qty;
        SetHearts();
        Invoke("SetOff", 1);
    }
    void SetOff()
    {
        particles.gameObject.SetActive(false);
    }
    void OnAvatarDie(CharacterBehavior cb)
    {
        //print("GRABA__________totalHearts" + total);
      //  PlayerPrefs.SetInt("totalHearts", total);
    }
    void OnGrabHeart()
    {
        //anim["UIHeartAnim"].normalizedTime = 0;
       // anim.Play("UIHeartAnim");
        newHearts++;
        total++;
        SetHearts();
    }
    void SetHearts()
    {
		foreach(Text label in labels)
       		label.text = total.ToString();
    }
}
