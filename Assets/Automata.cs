using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automata : MonoBehaviour
{
    public CharacterBehavior cb;
    int shootRandomTry = 10;
    float moveRandomTry = 1.5f;
    int jumpRandomTry = 10;

    public void Init(CharacterBehavior cb)
    {
        this.cb = cb;
        Invoke("MoveLoop", moveRandomTry*2);
        Invoke("ShootLoop", shootRandomTry*2);
        Invoke("JumpLoop", jumpRandomTry*2);
        cb.GetComponent<CharacterControls>().isAutomata = true;
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
            int rand = Random.Range(0, 100);
            if(rand<5)
                cb.shooter.SetFire(Weapon.types.TRIPLE, 0.3f);
            else if (rand < 10)
                cb.shooter.SetFire(Weapon.types.DOUBLE, 0.3f);
            else if (rand < 20)
                cb.shooter.SetFire(Weapon.types.SIMPLE, 0.3f);
        }
        if (Game.Instance.state != Game.states.GAME_OVER)
            Invoke("ShootLoop", shootRandomTry);
    }
    void JumpLoop()
    {
        if (CanDoIt())
        {
            int rand = Random.Range(0, 100);
            if (rand < 30)
                cb.Jump();
        }
        if (Game.Instance.state != Game.states.GAME_OVER)
            Invoke("JumpLoop", jumpRandomTry);
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
            cb.GetComponent<CharacterControls>().MoveInX(f);
            yield return new WaitForEndOfFrame();
        }
    }
}
