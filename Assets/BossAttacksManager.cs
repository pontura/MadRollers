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
        LASER
    }
    public float delayToSoundAttack;
    public int RandomBetween;    
    public float attackDuration;
    BossPart bossPart;
    Animation bossPartAnim;
    bool attacking;
    float speedAnim;
   // public SceneObject myProjectile;

    private void Start()
    {
        bossPartAnim = GetComponent<Animation>();
        bossPart = GetComponent<BossPart>();
        speedAnim = bossPartAnim[bossPartAnim.clip.name].normalizedSpeed;

        if (type == types.RANDOM_ATTACK)
            RandomAttack();
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
    public void CanRandomAttack()
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
    public void Attack()
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
       // if(sound == sounds.LASER)
            Data.Instance.events.OnSoundFX("laser", -1);
    }
    void ResetAttack()
    {
        bossPart.anim.Play("idle");
        attacking = false;
        bossPartAnim[bossPartAnim.clip.name].normalizedSpeed = speedAnim;        
    }
    public void Reset()
    {
        CancelInvoke();
        if(attacking)
        {
            attacking = false;
            bossPartAnim[bossPartAnim.clip.name].normalizedSpeed = speedAnim;
        }           
    }
    //public void Shoot()
    //{
    //    if (!CanAttackByRandom())
    //        return;
    //    PlaySound();
    //    Vector3 pos = transform.position;
    //    pos.y += 3;
    //    pos.z -= 3;
    //    SceneObject sceneObject = Instantiate(myProjectile, pos, Quaternion.identity) as SceneObject;
    //    Game.Instance.sceneObjectsManager.AddSceneObject(sceneObject, pos);
    //    sceneObject.transform.localEulerAngles = new Vector3(0, 180, 0);
    //}
}
