using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVerticalAttackModule : BossAttacksManager
{
    int attackID = 0;
    public AnimationClip[] attacks;

    public override void Attack()
    {
        base.Attack();

        attackID++;
        if (attackID > attacks.Length-1)
            attackID = 0;

        bossPart.anim.Play(attacks[attackID].name);

    }
}
