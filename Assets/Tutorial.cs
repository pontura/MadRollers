using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Animator anim;
    public MobileInputs mobileInputs;
    public states state;
    public enum states
    {
        ON, 
        ROTATE,
        ROTATE_DONE,
        JUMP,
        DONE_JUMP,
        DOUBLE_JUMP,
        DONE_DOUBLE_JUMP,
        SHOOT,
        DONE_SHOOT,
        TRIPLE_SHOOT,
        DONE_TRIPLE_SHOOT,
        DONE
    }
    CharactersManager charactersManager;

    public GameObject moveDevice;
    public GameObject signalJump;
    public GameObject signalJump2;
    public GameObject signalFire;
    public GameObject signalFire2;

    void ResetSignals()
    {
        moveDevice.SetActive(false);
        signalJump.SetActive(false);
        signalJump2.SetActive(false);
        signalFire.SetActive(false);
        signalFire2.SetActive(false);
    }
    void Start()
    {
        ResetSignals();
        if (Data.Instance.playMode != Data.PlayModes.SURVIVAL)
        {
            Destroy(this);
            return;
        }
        ResetAnim();
        if (!Data.Instance.isAndroid || PlayerPrefs.GetString("tutorial") == "done")
            Destroy(this);
        else
        {
            
            mobileInputs.ButtonJump.SetActive(false);
            mobileInputs.ButtonFire1.SetActive(false);
            mobileInputs.ButtonFire2.SetActive(false);

            Data.Instance.events.OnBossActive += OnBossActive;
            Data.Instance.events.OnAvatarShoot += OnAvatarShoot;
            Data.Instance.events.OnAvatarJump += OnAvatarJump;
            Data.Instance.events.OnAvatarDie += OnAvatarDie;
        }
    }
   
    private void OnDestroy()
    {
        Data.Instance.events.OnBossActive += OnBossActive;
        Data.Instance.events.OnAvatarShoot -= OnAvatarShoot;
        Data.Instance.events.OnAvatarJump -= OnAvatarJump;
        Data.Instance.events.OnAvatarDie -= OnAvatarDie;
    }
    void OnAvatarDie(CharacterBehavior cb)
    {
        ResetTimeScale();
    }
    void OnBossActive(bool isActive)
    {
        if (!isActive)
        {
            PlayerPrefs.SetString("tutorial", "done");
            Destroy(this);
        }
    }
    void ResetMove()
    {
        Data.Instance.events.RalentaTo(1, 1);
        ResetAnim();
        ResetSignals();
        state = states.ROTATE_DONE;
    }
    void OnAvatarJump(int a)
    {
        if(state == states.JUMP)
        {
            ResetAnim();
            ResetSignals();
            Data.Instance.GetComponent<MusicManager>().ChangePitch(1);
            state = states.DONE_JUMP;
        }
        if (state == states.DOUBLE_JUMP)
        {
            ResetAnim();
            ResetSignals();
            Data.Instance.GetComponent<MusicManager>().ChangePitch(1);
            state = states.DONE_DOUBLE_JUMP;
        }
    }
    void OnAvatarShoot(int a)
    {
        if (state == states.SHOOT)
        {
            ResetAnim();
            ResetSignals();
            Data.Instance.GetComponent<MusicManager>().ChangePitch(1);
            state = states.DONE_SHOOT;
        }
        else if (state == states.TRIPLE_SHOOT)
        {
            ResetAnim();
            ResetSignals();
            Data.Instance.GetComponent<MusicManager>().ChangePitch(1);
            state = states.DONE_TRIPLE_SHOOT;
        }
    }
    void Update()
    {
        float distance = Game.Instance.level.charactersManager.getDistance();
        CheckTutorial(distance);
    }
    int voiceSaid = -1;
    void CheckTutorial(float distance)
    {
        if (state == states.DONE)
            return;
        else if (distance > 40 && voiceSaid == -1)
        {            
            Data.Instance.voicesManager.PlayClip(Data.Instance.voicesManager.tutorials[5].audioClip);
            voiceSaid++;
        }
        if (distance > 45 && state == states.ON)
        {
            Anim("device");
            moveDevice.SetActive(true);
            state = states.ROTATE;
            Data.Instance.events.RalentaTo(0.5f, 0.5f);
            Invoke("ResetMove", 1.45f);
        } else
        if (distance > 176 && state == states.ROTATE_DONE)
        {
            Anim("jump");
            mobileInputs.ButtonJump.SetActive(true);
            signalJump.SetActive(true);
            Data.Instance.events.RalentaTo(0, 0.9f);
            Data.Instance.GetComponent<MusicManager>().ChangePitch(0);
            state = states.JUMP;
        }
        else if (distance > 260 && state == states.DONE_JUMP)
        {
            Anim("doubleJump");
            signalJump2.SetActive(true);
            Data.Instance.events.RalentaTo(0, 0.9f);
            Data.Instance.GetComponent<MusicManager>().ChangePitch(0);
            state = states.DOUBLE_JUMP;
        }
        else if (distance > 290 && voiceSaid == 0)
        {
            Data.Instance.voicesManager.PlayClip(Data.Instance.voicesManager.tutorials[3].audioClip);
            voiceSaid++;
        }
        else if (distance > 315 && state == states.DONE_DOUBLE_JUMP)
        {
            Anim("fire1");
            mobileInputs.ButtonFire1.SetActive(true);
            signalFire.SetActive(true);
            Data.Instance.events.RalentaTo(0, 0.9f);
            Data.Instance.GetComponent<MusicManager>().ChangePitch(0);
            state = states.SHOOT;
        }
        else if (distance > 405 && voiceSaid == 1)
        {
            Data.Instance.voicesManager.PlayClip(Data.Instance.voicesManager.tutorials[4].audioClip);
            voiceSaid++;
        }
        else if (distance > 425 && state == states.DONE_SHOOT)
        {
            Anim("fire2");
            mobileInputs.ButtonFire2.SetActive(true);
            signalFire2.SetActive(true);
            Data.Instance.events.RalentaTo(0, 0.9f);
            Data.Instance.GetComponent<MusicManager>().ChangePitch(0);
            state = states.TRIPLE_SHOOT;
        }
        else if (distance > 490 && voiceSaid == 2)
        {
            Data.Instance.voicesManager.PlayClip(Data.Instance.voicesManager.tutorials[2].audioClip);
            voiceSaid++;
            state = states.DONE;
        }
    }
    void Anim(string animName)
    {
        anim.gameObject.SetActive(true);
        anim.Play(animName);
    }
    void ResetAnim()
    {
        anim.gameObject.SetActive(false);
    }
    public void ResetTimeScale()
    {
        if (state == states.DONE)
            return;
        Data.Instance.events.RalentaTo(1, 1);
    }
}
