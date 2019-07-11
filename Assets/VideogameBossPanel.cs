using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideogameBossPanel : MonoBehaviour {

	public states state;
	public enum states
	{
		OFF,
		IDLE,
		LAUGHING,
		ATTACK,
		MAD,
		DROPPING_BOMB
	}
	public GameObject panel;
	public Animator anim;
	public Animation animation;

	void Start()
	{
      
        panel.SetActive (false);

        Data.Instance.events.OnGameStart += OnGameStart;
		Data.Instance.events.OnBossActive += OnBossActive;

		if (Data.Instance.videogamesData.actualID == 0)
			Data.Instance.events.OnBossDropBomb += OnBossDropBomb;
		
		if (Data.Instance.videogamesData.actualID == 1)
			Data.Instance.events.OnBossDropRay += OnBossDropRay;
		
		Data.Instance.events.OnAvatarDie += OnAvatarDie;
		Data.Instance.events.OnGameOver += OnGameOver;
        Data.Instance.events.OnBossSpecial += OnBossSpecial;
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;

        if (!Data.Instance.isReplay)
            OnBossActive(true);
    }
    void StartMultiplayerRace()
    {
        print("StartMultiplayerRace");
        if(!Data.Instance.isReplay)
        {
            OnBossActive(false);
        }
    }

    void OnGameStart()
	{
		StartCoroutine (InitCoroutine ());
	}
	IEnumerator InitCoroutine ()
	{
		panel.SetActive (true);
		if(Data.Instance.videogamesData.actualID == 0)
			PlayAnim ("mad");
		yield return new WaitForSeconds (2);
		animation.Play ("videoGameBossOut");
		yield return new WaitForSeconds (1);
		panel.SetActive (false);
	}

    void OnDestroy()
	{
        Data.Instance.events.OnGameStart -= OnGameStart;
		Data.Instance.events.OnBossActive -= OnBossActive;
		Data.Instance.events.OnBossDropBomb -= OnBossDropBomb;
		Data.Instance.events.OnBossDropRay -= OnBossDropRay;		
        Data.Instance.events.OnBossSpecial -= OnBossSpecial;
        Data.Instance.events.OnAvatarDie -= OnAvatarDie;	
		Data.Instance.events.OnGameOver -= OnGameOver;
        Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
    }
	void OnGameOver(bool isTimeOver)
	{
		Laugh (10);
	}
    void OnBossSpecial(int _x)
    {
        if (Data.Instance.videogamesData.actualID != 2)
            return;
        StopAllCoroutines();
        panel.SetActive(true);

        state = states.ATTACK;
        if (_x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(SnakesCoroutine());

    }
    IEnumerator SnakesCoroutine()
    {
        if (Data.Instance.videogamesData.actualID == 2)
            PlayAnim("attack");
        yield return new WaitForSeconds(3);
        animation.Play("videoGameBossOut");
        yield return new WaitForSeconds(2);
        SetOff();
    }

    void OnBossDropRay(int _x)
	{
		if (Data.Instance.videogamesData.actualID != 1)
			return;
		
		StopAllCoroutines ();
		panel.SetActive (true);

		state = states.ATTACK;
		if(_x<0)
			transform.localScale = new Vector3 (-1, 1, 1);
		else 
			transform.localScale = new Vector3 (1, 1, 1);
		StartCoroutine (RayCoroutine ());

	}
	IEnumerator RayCoroutine ()
	{
		if(Data.Instance.videogamesData.actualID == 1)
			PlayAnim ("ray");
		yield return new WaitForSeconds (3);
		animation.Play ("videoGameBossOut");
		yield return new WaitForSeconds (2);
		SetOff ();
	}
	void OnBossDropBomb()
	{		
		if (state == states.OFF)
			StartCoroutine (AxeCoroutine ());
		else {
			if(Data.Instance.videogamesData.actualID == 0)
			PlayAnim ("axe_throw");
		}
	}
	IEnumerator AxeCoroutine ()
	{
		state = states.DROPPING_BOMB;

		if(Data.Instance.videogamesData.actualID == 0)
			PlayAnim ("axe_idle");
		yield return new WaitForEndOfFrame();
		panel.SetActive (true);

		yield return new WaitForSeconds (1f);
		if(Data.Instance.videogamesData.actualID == 0)
			PlayAnim ("axe_throw");
		yield return new WaitForSeconds (1f);
		if (state == states.DROPPING_BOMB) {
			SetOff ();
		}			
	}
	void OnBossActive(bool isOn)
	{
        VideogameData vd = Data.Instance.videogamesData.GetActualVideogameData();
        if (isOn) {
            
            Game.Instance.gameCamera.cam.backgroundColor = vd.bossFog;
            RenderSettings.fogColor = vd.bossFog;
            panel.SetActive (true);
			state = states.IDLE;
			Laugh(3);
		} else {
            Game.Instance.gameCamera.cam.backgroundColor = vd.fog;
            RenderSettings.fogColor = vd.fog;
            Mad (3);
		}
	}

	void OnAvatarDie(CharacterBehavior cb)
	{
		Laugh (1.5f);
	}
	void Idle()
	{
		if (state == states.OFF)
			return;
		if (state == states.IDLE)
			return;
		PlayAnim ("idle");

		if(Data.Instance.videogamesData.actualID != 0)
			return;
		
		StartCoroutine (IdleCoroutine());
	}
	void Laugh(float timer)
	{
		if (state == states.OFF || state == states.MAD)
			return;
		StopAllCoroutines ();
		state = states.LAUGHING;
		StartCoroutine (LaughCoroutine(timer));
	}
	void Mad(float timer)
	{
		
		if (state == states.MAD)
			return;
        StopAllCoroutines();
		state = states.MAD;
		StartCoroutine (MadCoroutine(timer));
	}
	void Attack()
	{
		if (state == states.IDLE || state == states.MAD)
			return;
		state = states.ATTACK;
		StartCoroutine (AttackCoroutine());
	}
	IEnumerator AttackCoroutine ()
	{
		if(Data.Instance.videogamesData.actualID == 0)
			PlayAnim ("pinskull_attack");
		yield return new WaitForSeconds (2);
		Idle ();
	}
	IEnumerator IdleCoroutine ()
	{
		yield return new WaitForSeconds (Random.Range(2,5));
		Attack();
	}
	IEnumerator LaughCoroutine (float timer)
	{
		if(Data.Instance.videogamesData.actualID == 0)
			PlayAnim ("laugh");
		yield return new WaitForEndOfFrame();
		panel.SetActive (true);

		yield return new WaitForSeconds (timer);
		if (state == states.LAUGHING)
			Idle ();
	}
	IEnumerator MadCoroutine (float timer)
	{
		if(Data.Instance.videogamesData.actualID == 0 || Data.Instance.videogamesData.actualID == 2)
			PlayAnim ("mad");
		yield return new WaitForSeconds (timer);
		animation.Play ("videoGameBossOut");
		yield return new WaitForSeconds (1);
		SetOff ();
	}
	void PlayAnim(string animName)
	{
		if(anim != null)
			anim.Play (animName);
	}
	void SetOff()
	{
		panel.SetActive (false);
		state = states.OFF;
	}
}
