using System;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator anim;
    public AnimsData[] anims;
    int id;
    [Serializable] 
    public class AnimsData
    {
        public string animName;
        public int seconds;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        if(anim == null)
            Destroy(this);
    }

    void OnEnable()
    {
        id = 0;
    }
    void SetAnim()
    {
        if (id > anims.Length)
            id = 0;
        AnimsData animData = anims[id];
        if(animData != null)
        {
            anim.Play(animData.animName);
            Invoke("NextAnim", animData.seconds);
        }
    }
    void NextAnim()
    {
        id++;
        SetAnim();
    }
}
