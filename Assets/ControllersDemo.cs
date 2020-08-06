using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class ControllersDemo : MonoBehaviour
{
    public MobileInputs mobileInputs;

    enum actions
    {
        SHOOT,
        DOUBLESHOOT,
        JUMP,
        FIRE,
        DASH,
        LEFT,
        RIGHT
    }
    float timeToResetAction = 0.3f;
    float offset = 0.05f;
    float doubleShotOffset = 0.3f;
    //public Text field;
    bool isDown;
    bool upForced;
    float timer;
    float lastShoot;
    float _y, _x = 0;
    bool isOn;

    void Start()
    {
       // SetOn();
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
        Data.Instance.events.OnGameOver += OnGameOver;
    }
    private void OnDestroy()
    {
        Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
        Data.Instance.events.OnGameOver -= OnGameOver;
    }
    void StartMultiplayerRace()
    {
        SetOn();
    }
    void OnGameOver(bool a)
    {
        SetOff();
    }
    public void SetOn()
    {
        isOn = true;
    }
    public void SetOff()
    {
        isOn = false;
    }
    
    void Update()
    {
        if (!isOn)
            return;
        if (Input.touchCount > 0)
        {
            for (int a = 0; a< Input.touchCount; a++)
            {
                Touch t = Input.GetTouch(a);
                if (t.position.x > Screen.width/2)
                {
                    if(t.phase == TouchPhase.Began)
                        InitPressing();
                    else if(t.phase == TouchPhase.Ended)
                        if(!upForced )
                            OnUp();
                    else if(!upForced)
                        OnDown();
                    isDown = true;
                    return;
                }
            }
        }
        isDown = false;
        upForced = false;

        //else if (Input.mousePosition.x > Screen.width / 2 && Input.GetMouseButton(0) && isDown && !upForced)
        //{
        //    OnDown();
        //}
        //else if (Input.GetMouseButtonUp(0) && !upForced)
        //{
        //    if(isDown)
        //        OnUp();
        //    upForced = false;
        //}
    }
    void OnDown()
    {
        timer += Time.deltaTime;
        if (timer > timeToResetAction)
            OnForceUp();
    }
    void InitPressing()
    {
        _x = Input.mousePosition.x;
        _y = Input.mousePosition.y;
       // field.text = " Press";
        timer = 0;
        isDown = true;
        upForced = false;
    }
    void OnForceUp()
    {
        upForced = true;
        DoUp();
       // field.text += " ForceUp";
    }
    void OnUp()
    {
        isDown = false;
        DoUp();
        //field.text += " Release";
    }
    void DoUp()
    {
        _x = (_x - Input.mousePosition.x) / Screen.width;
        _y = (_y - Input.mousePosition.y) / Screen.height;
        float difX = Mathf.Abs(_x);
        float difY = Mathf.Abs(_y);

        if ((difX + difY) < offset)
        {
            if (lastShoot>0 && Time.time - lastShoot - doubleShotOffset < 0)
                DoAction(actions.DOUBLESHOOT);
            else
            {
                lastShoot = Time.time;
                DoAction(actions.SHOOT);
            }
        }
        else if (difX > difY)
        {
            if (_x > 0)
                DoAction(actions.RIGHT);
            else
                DoAction(actions.LEFT);
        }
        else
        {
            if (_y > 0)
                DoAction(actions.DASH);
            else
                DoAction(actions.JUMP);
        }
    }
    void DoAction(actions action)
    {
        if (mobileInputs == null)
            Debug.Log(action);
        else
        {
            switch (action)
            {
                case actions.JUMP:
                    mobileInputs.Jump(-_y); break;
                case actions.SHOOT:
                    mobileInputs.Shoot(); break;
                case actions.DOUBLESHOOT:
                    mobileInputs.ShootTriple(); break;
                case actions.DASH:
                    mobileInputs.Dash(); break;
                case actions.RIGHT:
                    mobileInputs.HorizontalDash(1); break;
                case actions.LEFT:
                    mobileInputs.HorizontalDash(-1); break;
            }
        }
        
        //field.text += " action:" + action;
    }
}
