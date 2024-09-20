using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyRunner : MonoBehaviour
{
    private MmoCharacter mmoCharacter;
    public Animation anim;
    public states state;
    float speed = 10;
    public enum states
    {
        IDLE,
        ROTATING,
        RUN,
        DEAD
    }
    void Start()
    {
        mmoCharacter = GetComponent<MmoCharacter>();
    }
    void OnEnable()
    {
        Idle();
    }
    void Idle()
    {
        state = states.IDLE;
        anim.Play("idle");
    }
    void Update()
    {
        if (!mmoCharacter) return;
        if (mmoCharacter.state == MmoCharacter.states.DEAD) return;
        if (mmoCharacter.distanceFromCharacter < 28 && state == states.IDLE)
            Rotate();
        else if (state == states.RUN)
            Running();
    }
    void Rotate()
    {
        float rand = Random.Range(-360, 360);
        state = states.ROTATING;
        transform.DORotate(new Vector3(0, rand, 0), 0.2f, RotateMode.Fast).OnComplete(RotateDone);
    }
    void RotateDone()
    {
        state = states.RUN;
        anim.Play("run");
    }
    void Running()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

}