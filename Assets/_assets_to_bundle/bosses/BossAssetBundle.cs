using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAssetBundle : MonoBehaviour
{
    public Animation anim;

    public virtual void OnInit() {  }
    public virtual void Idle()  {
        anim.Play("idle");
    }
    public virtual void Attack()  {
        anim.Play("attack");
    }
}
