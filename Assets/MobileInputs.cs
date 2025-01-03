﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputs : MonoBehaviour
{
    float jumpingPressedSince;
    public bool jumping;
    float jumpingPressedTime = 0.28f;
    public GameObject panel;

    //public GameObject ButtonJump;
    //public GameObject ButtonFire1;
    //public GameObject ButtonFire2;
    //public GameObject ButtonDash;

    public Tutorial tutorial;

    public GameObject panel_gyroscope;
    public GameObject panel_virtualJoystick;

    private void Start()
    {
        panel.SetActive(false);
        if (Data.Instance.isAndroid)
        {
            if(Data.Instance.controlsType == Data.ControlsType.GYROSCOPE)
            {
                panel_gyroscope.SetActive(true);
                panel_virtualJoystick.SetActive(false);
            }
            else
            {
                panel_gyroscope.SetActive(false);
                panel_virtualJoystick.SetActive(true);
            }
            Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
            Data.Instance.events.OnGameOver += OnGameOver;
        }
        else
        {
            Destroy(this);
        }
    }
    void OnDestroy()
    {
        Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
        Data.Instance.events.OnGameOver -= OnGameOver;
    }
    void OnGameOver(bool a)
    {
        panel.SetActive(false);
    }
    void StartMultiplayerRace()
    {
        panel.SetActive(true);
    }
    CharacterBehavior GetCharacter()
    {
        CharactersManager cm = Game.Instance.level.charactersManager;
        if (cm == null)
            return null;
        CharacterBehavior cb = cm.getMainCharacter();
        return cb;
    }
    //void Update()
    //{
    //    if (!jumping)
    //        return;
    //    if (Game.Instance.state == Game.states.GAME_OVER)
    //        return;
    //    jumpingPressedSince += Time.deltaTime;
    //    if (jumpingPressedSince > jumpingPressedTime)
    //        DOJump();
    //    else
    //        GetCharacter().JumpingPressed();
    //}
    public void Jump(float value)
    {
        float v = 900 + (value * 2500);
        GetCharacter().SetJumpHeight( v );
        GetCharacter().Jump();
    }
    void DOJump()
    {
        jumping = false;
        jumpingPressedSince = 0;
        GetCharacter().Jump();
       // ButtonJump.GetComponent<Animation>().Play();
    }
    public void Jump()
    {
        ResetTutorial();
        if (GetCharacter() == null)
            return;
        if (GetCharacter().state != CharacterBehavior.states.RUN)
            DOJump();
        else
        {
            jumpingPressedSince = 0;
            jumping = true;
        }
    }
    public void JumpRelease()
    {
        if (GetCharacter() == null)
            return;
       jumping = false;
        if (GetCharacter().state == CharacterBehavior.states.RUN)
            DOJump();
    }
    public void Shoot()
    {
        ResetTutorial();
        if (GetCharacter() == null)
            return;
        GetCharacter().shooter.SetFire(Weapon.types.SIMPLE, 0.25f);
        // ButtonFire1.GetComponent<Animation>().Play();
    }
    public void Dash()
    {
        ResetTutorial();
        if (GetCharacter() == null)
            return;
        GetCharacter().characterMovement.DashForward();
        // ButtonDash.GetComponent<Animation>().Play();
    }
    public void ShootTriple()
    {
        ResetTutorial();
        if (GetCharacter() == null)
            return;
        GetCharacter().shooter.SetFire(Weapon.types.TRIPLE, 0.45f);
        // ButtonFire2.GetComponent<Animation>().Play();
    }
    public void HorizontalDash(float v)
    {
        if (GetCharacter() == null)
            return;
        GetCharacter().characterMovement.DH(v);
    }
    void ResetTutorial()
    {
        if (tutorial != null)
            tutorial.ResetTimeScale();
    }

}
