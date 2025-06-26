using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automata : MonoBehaviour
{
    CharacterBehavior cb;
    float shootRandomTry = 2;
    float moveRandomTry = 0.5f;
    float jumpRandomTry = 2.5f;
    CharacterControls controls;

    public void Init(CharacterBehavior cb)
    {
        controls = cb.GetComponent<CharacterControls>();
        this.cb = cb;
        StopAllCoroutines();
        CancelInvoke();
        Invoke("MoveLoop", moveRandomTry);
        Invoke("ShootLoop", Random.Range(1f, shootRandomTry));
        Invoke("JumpLoop", jumpRandomTry);
        cb.GetComponent<CharacterControls>().isAutomata = true;
        if(Data.Instance.missions.MissionActiveID == 1)
        {
            shootRandomTry *= 2.8f;
            jumpRandomTry *= 1.75f;
        } else if (Data.Instance.missions.MissionActiveID == 2)
        {
            shootRandomTry *= 1.5f;
            jumpRandomTry *= 1.5f;
        }
        else if (Data.Instance.missions.MissionActiveID == 3)
        {
            shootRandomTry *= 1.25f;
            jumpRandomTry *= 1.2f;
        }
    }
    bool CanDoIt()
    {
        if (cb.state != CharacterBehavior.states.CRASH && cb.state != CharacterBehavior.states.DEAD)
            return true;
        return false;
    }
    void ShootLoop()
    {
        if(CanDoIt())
        {
            int rand = Random.Range(0, 45);
            if(rand<5)
                cb.shooter.SetFire(Weapon.types.TRIPLE, 0.3f);
            else if (rand < 10)
                cb.shooter.SetFire(Weapon.types.DOUBLE, 0.3f);
            else if (rand < 20)
                cb.shooter.SetFire(Weapon.types.SIMPLE, 0.3f);
        }
        if (Game.Instance.state != Game.states.GAME_OVER)
            Invoke("ShootLoop", Random.Range(0.25f,shootRandomTry));
    }
    void JumpLoop()
    {
        if (CanDoIt())
        {
            int rand = Random.Range(0, 100);
            if (rand < 50)
            {
                cb.Jump();
                if (Random.Range(0, 10)<3)
                    Invoke("DoubleJump", Random.Range(0.15f, 0.35f));
            }
        }
        if (Game.Instance.state != Game.states.GAME_OVER)
            Invoke("JumpLoop", Random.Range(1f, jumpRandomTry));
    }
    void DoubleJump()
    {
        if (Game.Instance.state != Game.states.GAME_OVER)
            cb.Jump();
    }
    void MoveLoop()
    {
        if (Game.Instance.state != Game.states.GAME_OVER && CanDoIt())
        {
            StopAllCoroutines();
            int rand = Random.Range(0, 100);
            if (rand < 70)
            {
                if (cb.transform.localPosition.x < 0)
                    StartCoroutine(Move(1));
                else
                    StartCoroutine(Move(-1));
            }  else
                cb.GetComponent<CharacterControls>().MoveInX(0);
        }
        Invoke("MoveLoop", moveRandomTry);
    }
  
    IEnumerator Move(float _x)
    {       
        float i = 0;
        float timer = (float)Random.Range(3, 15) / 10;
        while (i < timer)
        {
            if (cb.transform.localPosition.x < -8 || cb.transform.localPosition.x > 8)
                i *= 2;
            i += Time.deltaTime;
            float f = i * _x;
            if(controls != null)
                controls.MoveInX(f);
            yield return new WaitForEndOfFrame();
        }
    }
}
