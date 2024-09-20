using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMainMenuUI : MonoBehaviour {

    public int id;
    public Image masker;
    public Image background;
    public RawImage rawimage;
    public bool isActive;
    public Animator anim;
	public RenderTexture renderTexture;

    public void Init()
    {
        anim = GetComponent<Animator>();
        Color color = Data.Instance.multiplayerData.colors[id];
        background.color = color;
        color.a = 0.5f;
        masker.color = Data.Instance.multiplayerData.colors[id];

      //  Invoke("SetInActive", (float)id/2.5f);
    }
	void OnDestroy()
	{
		renderTexture.DiscardContents ();
	}
    public void Toogle()
    {
		SetActive ();
		isActive = true;
		return;
        isActive = !isActive;
        if (isActive)
            SetActive();
        else
            SetInActive();
    }
	void SetActive () {        
        anim.Play("active",0,0);
	}
    void SetInActive()
    {
        anim.Play("inactive", 0, 0);
    }
}
