using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacksManager : MonoBehaviour
{
    public types type;
    public enum types
    {
        NONE,
        RANDOM_ATTACK,
        BY_ANIMATION
    }
    public sounds sound;
    public enum sounds
    {
        NONE,
        LASER,
        LATIGO
    }
    public float delayToSoundAttack;
    public int RandomBetween;    
    public float attackDuration;
    [HideInInspector] public BossPart bossPart;
    [HideInInspector] public Animation bossPartAnim;
    [HideInInspector] public bool attacking;
    [HideInInspector] public float speedAnim;
   // public SceneObject myProjectile;

    public virtual void Start()
    {
        bossPartAnim = GetComponent<Animation>();
        bossPart = GetComponent<BossPart>();
        speedAnim = bossPartAnim[bossPartAnim.clip.name].normalizedSpeed;

        if (type == types.RANDOM_ATTACK)
            RandomAttack();
    }
    public virtual void OnDisable()
    {
        CancelInvoke();
    }
    void RandomAttack()
    {        
        Invoke("Loop", 3);
    }
    void Loop()
    {
        Invoke("Loop", 1);
        CanRandomAttack();
    }
    public virtual void CanRandomAttack()
    {
        if (attacking)
            return;
        if (CanAttackByRandom())
            Attack();
    }
    bool CanAttackByRandom()
    {
        if (Random.Range(0, RandomBetween) == 0)
            return true;
        return false;
    }
    public virtual void Attack()
    {
        if(delayToSoundAttack>0)
            Data.Instance.events.OnSoundFX("subida", -1);
        attacking = true;
        bossPart.anim.Play("attack");
        bossPartAnim[bossPartAnim.clip.name].normalizedSpeed = 0;
        Invoke("ResetAttack", attackDuration);
       Invoke ("PlaySound", delayToSoundAttack);
    }
    void PlaySound()
    {
        if(sound == sounds.LATIGO)
            Data.Instance.events.OnSoundFX("whip", -1); 
        else
            Data.Instance.events.OnSoundFX("laser", -1);
    }
    public virtual void ResetAttack()
    {
        bossPart.anim.Play("idle");
        attacking = false;
        bossPartAnim[bossPartAnim.clip.name].normalizedSpeed = speedAnim;        
    }
    public void Reset()
    {
        return;
        if(attacking)
        {
            CancelInvoke();
            Invoke("Loop", 0.5f);
            attacking = false;
            bossPartAnim[bossPartAnim.clip.name].normalizedSpeed = speedAnim;
        }           
    }
}
