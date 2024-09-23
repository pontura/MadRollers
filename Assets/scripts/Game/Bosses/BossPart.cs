using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossPart : MonoBehaviour {

    [HideInInspector]
	public Boss boss;
	bool called;
	bool isOn;
    public Vector3 progressBarPosition = new Vector3(0,5,0);
    public int lifes = 2;
    int totalLife;
    Vector3 initialScale;
    //  ProgressBar progressBar;
    public GameObject asset;
   // [HideInInspector]
    public Animation anim;
    BossAttacksManager bossAttackManager;

    private void Start()
    {
        this.bossAttackManager = GetComponent<BossAttacksManager>();
    }
    public void Init(Boss _boss, BossSettings settings)
	{
        isOn = true;
        this.totalLife = lifes;

        initialScale = transform.localScale;
        this.boss = _boss;
		Utils.RemoveAllChildsIn (transform);

        if (asset == null && settings.asset != null)
        {
            GameObject go = ObjectPool.instance.bossesPool.GetBossAsset(settings.asset).gameObject;
            if (go != null)
                asset = Instantiate(go);
            else
                asset = Instantiate(Resources.Load("bosses/assets/" + settings.asset, typeof(GameObject))) as GameObject;

            asset.transform.SetParent (transform);
            asset.transform.localScale = Vector3.one;
            asset.transform.localEulerAngles = Vector3.zero;
            asset.transform.localPosition = Vector3.zero;
            anim = asset.GetComponentInChildren<Animation>();
        }
        //if (progressBar == null)
        //{
        //    progressBar = (Instantiate(Resources.Load("ProgressBar", typeof(GameObject))) as GameObject).GetComponent<ProgressBar>();
        //    progressBar.SetProgression(1);
        //    progressBar.transform.SetParent(transform);
        //    progressBar.transform.localScale = Vector3.one;
        //    progressBar.transform.localEulerAngles = Vector3.zero;
        //    progressBar.transform.localPosition = progressBarPosition;
        //}
    }
	void Update()
	{
		if (!isOn)
			return;
        if (transform.position.y < -22)
            Die();

    }
	public void Hitted()
	{
		if (called)
			return;
        if (bossAttackManager != null)
            bossAttackManager.Reset();

        ParticlesSceneObject effect = ObjectPool.instance.GetObjectForType("ExplotionEffectBoss", false) as ParticlesSceneObject;
        Data.Instance.events.OnSoundFX("punch", -1);
        lifes--;
        if (lifes > 0)
        {
           // progressBar.SetProgression((float)lifes / (float)totalLife);
            HittedAnim();
            effect.SetColor(Color.green);
            Game.Instance.sceneObjectsManager.AddSceneObjectAndInitIt(effect, transform.position);
        }
        else
        {
            effect.SetColor(Color.red);
            Game.Instance.sceneObjectsManager.AddSceneObjectAndInitIt(effect, transform.position);
        }
        boss.Hitted();
    }


    //lo llama breakable:
    public void Die()
    {
        lifes = 0;

        called = true;
        CancelInvoke();

     //   if (boss.HasOnlyOneLifeLeft())
        //    Data.Instance.events.OnProjectilStartSnappingTarget(transform.position);

        boss.OnPartBroken(this);
        gameObject.SetActive(false);
    }
	public void OnActive()
	{
		SendMessage ("OnBossPartActive", SendMessageOptions.DontRequireReceiver);
		gameObject.SetActive (false);
		Invoke ("Reactive", 4);
	}
	void Reactive()
	{
		gameObject.SetActive (true);
	}
    private void HittedAnim()
    {        
        transform.localScale = initialScale * 2.5f;
        gameObject.transform.DOScale(initialScale, 0.5f);
    }
    void PlayAnim(string animName)
    {        
        if (anim != null)
        {
            AnimationClip clip = anim.GetClip(animName);
            if(clip != null)
                anim.Play(animName);
            else
                print("no tiene la anim " + animName);
        }
    }
}
