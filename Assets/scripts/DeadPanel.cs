using System;
using UnityEngine;
using UnityEngine.UI;

public class DeadPanel : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject panel;
    [SerializeField] TMPro.TMP_Text field;
    [SerializeField] TMPro.TMP_Text teamField;
    [SerializeField] Image progressBar;
    [SerializeField] Image[] team;
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
        Events.OnCharacterInit += OnCharacterInit;
    }
    void AllDead()
    {
        Events.OnCameraChroma(CameraChromaManager.types.NONE);
        SetActive(false);
    }
    private void OnAvatarDie(CharacterBehavior characterBehavior)
    {
        print("OnAvatarDie " + isOn + " characterBehavior.player.id " + characterBehavior.player.id);
        if (isOn)
        {
            if (cm.totalCharacters < 1) return;
            SetTeam();
        }
        if (characterBehavior.player.id == 0)
            Init();
    }

    private void OnDestroy()
    {
        Events.OnAvatarDie -= OnAvatarDie;
        Events.AllDead -= AllDead;
        Events.OnCharacterInit -= OnCharacterInit;
    }

    private void OnCharacterInit(int obj)
    {
        if(isOn) 
            SetTeam();
    }

    public void Init()
    {
        Events.OnCameraChroma(CameraChromaManager.types.RED);
        MusicManager.Instance.ChangePitch(0.4f);
        value = 0;
        SetActive(true);
        SetTeam();
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
            Events.OnCameraChroma(CameraChromaManager.types.NONE);
        }
    }
    public void OnClick()
    {
        Events.OnSoundFX("popup");
        value += 0.2f;
        anim.Play("clicked",0,0);
    }
    void SetTeam()
    {
        foreach (Image i in team)
            i.gameObject.SetActive(false);

        foreach (CharacterBehavior cb in cm.characters)
        {
            if (cb.player.id == 1)
                team[0].gameObject.SetActive(true);
            else if (cb.player.id == 2)
                team[1].gameObject.SetActive(true);
            else if (cb.player.id == 3)
                team[2].gameObject.SetActive(true);
        }
        teamField.text = cm.characters.Count + " STILL ALIVE";
    }
}
