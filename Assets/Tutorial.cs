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
       //TRIPLE_SHOOT,
      //  DONE_TRIPLE_SHOOT,
        DONE
    }
    CharactersManager charactersManager;

    public GameObject moveDevice;
    public GameObject signalJump;
    public GameObject signalJump2;
    public GameObject signalFire;
   // public GameObject signalFire2;

    void ResetSignals()
    {
        moveDevice.SetActive(false);
        signalJump.SetActive(false);
        signalJump2.SetActive(false);
        signalFire.SetActive(false);
      //  signalFire2.SetActive(false);
    }
    void Start()
    {
        ResetSignals();
        ResetAnim();
        if (!Data.Instance.isAndroid || PlayerPrefs.GetString("tutorial") == "done" || UserData.Instance.missionUnblockedID_1>0)
        {
            Destroy(anim.gameObject);
            Destroy(this);
        }            
        else
        {
            
            mobileInputs.ButtonJump.SetActive(false);
            mobileInputs.ButtonFire1.SetActive(false);
            mobileInputs.ButtonFire2.SetActive(false);
            mobileInputs.ButtonDash.SetActive(false);

            Data.Instance.events.TutorialContinue += TutorialContinue;
            Data.Instance.events.OnAvatarShoot += OnAvatarShoot;
            Data.Instance.events.OnAvatarJump += OnAvatarJump;
            Data.Instance.events.OnAvatarDie += OnAvatarDie;
        }
    }
   
    private void OnDestroy()
    {
        Data.Instance.events.TutorialContinue -= TutorialContinue;
        Data.Instance.events.OnAvatarShoot -= OnAvatarShoot;
        Data.Instance.events.OnAvatarJump -= OnAvatarJump;
        Data.Instance.events.OnAvatarDie -= OnAvatarDie;
    }
    void OnAvatarDie(CharacterBehavior cb)
    {
        ResetTimeScale();
    }
    void ResetMove()
    {
        Data.Instance.events.RalentaTo(1, 1);
        ResetAnim();
        ResetSignals();
        state = states.ROTATE_DONE;
    }
    void TutorialContinue()
    {
        print("TutorialContinue");
        OnAvatarJump(0);
        OnAvatarShoot(0);
    }
    void OnAvatarJump(int a)
    {
        ResetTimeScale();
        print("JUMP " + state);
        if (state == states.JUMP)
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
        ResetTimeScale();
        print("Shoot " + state);
        if (state == states.SHOOT)
        {
            ResetAnim();
            ResetSignals();
            Data.Instance.GetComponent<MusicManager>().ChangePitch(1);
            state = states.DONE_SHOOT;
        }
        //else if (state == states.TRIPLE_SHOOT)
        //{
        //    ResetAnim();
        //    ResetSignals();
        //    Data.Instance.GetComponent<MusicManager>().ChangePitch(1);
        //    state = states.DONE_TRIPLE_SHOOT;
        //}
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
        if (distance > 175 && state == states.ROTATE_DONE)
        {
            Anim("jump");
            mobileInputs.ButtonJump.SetActive(true);
            signalJump.SetActive(true);
            Data.Instance.events.RalentaTo(0.05f, 0.9f);
            Data.Instance.GetComponent<MusicManager>().ChangePitch(0);
            state = states.JUMP;
        }
        else if (distance > 260 && state == states.DONE_JUMP)
        {
            Anim("doubleJump");
            signalJump2.SetActive(true);
            Data.Instance.events.RalentaTo(0.05f, 0.9f);
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
            print("Souble jump");
            Anim("fire1");
            mobileInputs.ButtonFire1.SetActive(true);
            signalFire.SetActive(true);
            Data.Instance.events.RalentaTo(0.05f, 0.9f);
            Data.Instance.GetComponent<MusicManager>().ChangePitch(0);
            state = states.SHOOT;
            PlayerPrefs.SetString("tutorial", "done");
        }
        else if (distance > 390 && voiceSaid == 1)
        {
            print("die: " + Data.Instance.voicesManager.tutorials[4].audioClip.name);
            Data.Instance.voicesManager.PlayClip(Data.Instance.voicesManager.tutorials[4].audioClip);
            voiceSaid++;
        }
        else if (distance > 400 && state == states.DONE_SHOOT)
        {
           // Anim("fire2");
            mobileInputs.ButtonFire2.SetActive(true);
            mobileInputs.ButtonDash.SetActive(true);
            //signalFire2.SetActive(true);
           // Data.Instance.events.RalentaTo(0, 0.9f);
            //Data.Instance.GetComponent<MusicManager>().ChangePitch(0);
            //state = states.TRIPLE_SHOOT;
            //state = states.DONE;
        }
        else if (distance > 490 && voiceSaid == 2)
        {            
            Data.Instance.voicesManager.PlayClip(Data.Instance.voicesManager.tutorials[2].audioClip);
            voiceSaid++;
            state = states.DONE;
            OnDestroy();
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
