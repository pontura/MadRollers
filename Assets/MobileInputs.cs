using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputs : MonoBehaviour
{
    float jumpingPressedSince;
    public bool jumping;
    float jumpingPressedTime = 0.28f;
    public GameObject panel;

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
    CharacterBehavior cb;
    CharacterBehavior GetCharacter()
    {
        if (cb == null)
            cb = Game.Instance.level.charactersManager.getMainCharacter();       
        return cb;
    }
    void Update()
    {
        if (!jumping)
            return;
        if (Game.Instance.state == Game.states.GAME_OVER)
            return;
        jumpingPressedSince += Time.deltaTime;
        if (jumpingPressedSince > jumpingPressedTime)
            DOJump();
        else
            GetCharacter().JumpingPressed();
    }
   
    void DOJump()
    {
        jumping = false;
        jumpingPressedSince = 0;
        GetCharacter().Jump();
    }
    public void Jump()
    {
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
        if (GetCharacter() == null)
            return;
        GetCharacter().shooter.SetFire(Weapon.types.SIMPLE, 0.25f);
        // ButtonFire1.GetComponent<Animation>().Play();
    }
    public void Dash()
    {
        if (GetCharacter() == null)
            return;
        GetCharacter().characterMovement.DashForward();
        // ButtonDash.GetComponent<Animation>().Play();
    }
    public void ShootTriple()
    {
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



    //bool isActive;
    //private void Update()
    //{
    //    if (!isActive) return;
    //    if (Time.time > timer + 0.1f)
    //        Release();
    //}

    //float _y;
    //float timer;
    //public void OnMouseDown()
    //{
    //    isActive  = true;
    //    _y = Input.mousePosition.y;
    //    timer = Time.time;
    //}
    //void Release()
    //{
    //    isActive  = false;
    //    if (_y < Input.mousePosition.y - 5f)
    //        Data.Instance.events.OnJump();
    //    else
    //    if (_y > Input.mousePosition.y + 20f)
    //        Data.Instance.events.OnDash();
    //    else
    //        Data.Instance.events.OnShoot();
    //}
    //public void OnMouseUp()
    //{
    //    if (!isActive) return;
    //    Release();        
    //}

}
