using System;
using UnityEngine;
using UnityEngine.UI;

public class DeadPanel : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject panel;
    [SerializeField] TMPro.TMP_Text field;
    [SerializeField] Image progressBar;
    bool isOn;
    float value;
    float speed = 1;
    float total = 10;
    CharactersManager cm;

    void Start()
    {
        cm = Game.Instance.GetComponentInChildren<CharactersManager>();
        isOn = false;
        //field.text = "YOU ARE DEAD"; 
        SetActive(false);
        Events.OnAvatarDie += OnAvatarDie;
        Events.AllDead += AllDead;
    }
    void AllDead()
    {
        SetActive(false);
    }
    private void OnAvatarDie(CharacterBehavior characterBehavior)
    {
        print("OnAvatarDie " + isOn + " characterBehavior.player.id " + characterBehavior.player.id);
        if (isOn || cm.totalCharacters < 1) return;
        if (characterBehavior.player.id == 0)
            Init();
    }

    private void OnDestroy()
    {
        Events.OnAvatarDie -= OnAvatarDie;
        Events.AllDead -= AllDead;
    }
    public void Init()
    {
        MusicManager.Instance.ChangePitch(0.4f);
        value = 0;
        SetActive(true);
    }
    void SetActive(bool isOn)
    {
        this.isOn = isOn;
        panel.SetActive(isOn);
    }
    void Update()
    {
        if (!this.isOn) return;
        value += speed * Time.deltaTime;
        progressBar.fillAmount = value / total;
        if (value >= total)
        {
            
            Vector3 pos = new Vector3(0, -5, cm.getDistance());
            cm.AddNewCharacter(0, false);
            value = 1;
            SetActive(false);
            Events.Respawn();
            MusicManager.Instance.ChangePitch(1);
            Events.OnSoundFX("continueClip");
        }
    }
    public void OnClick()
    {
        Events.OnSoundFX("popup");
        value += 0.5f;
        anim.Play("clicked",0,0);
    }
}
