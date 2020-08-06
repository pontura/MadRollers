using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : SceneObject {

	public int time_to_kill;
	public float distance_from_avatars;
	public int hits;
	//Missions missions;
	public int totalHits;

	public override void OnRestart(Vector3 pos)
	{		
		base.OnRestart (pos);
		Data data = Data.Instance;
		data.events.OnBossActive (true);
		data.GetComponent<MusicManager> ().BossMusic (true);
        VoicesManager.Instance.PlayRandom (VoicesManager.Instance.killThemAll);
	}
	public void SetTotal(int totalHits)
	{
        print("SEt total : " + totalHits);
		this.totalHits = totalHits;
		Data.Instance.events.OnBossInit (totalHits);
	}
	public bool HasOnlyOneLifeLeft()
	{
		if (hits + 2 == totalHits)
			VoicesManager.Instance.PlaySpecificClipFromList (VoicesManager.Instance.UIItems, 2);
		else if (hits+1 >= totalHits)
			return true;

		return false;
	}
	public void breakOut()
	{     
		if (hits >= totalHits)
			Killed ();		
	}
    public void Hitted()
    {
        Data.Instance.events.OncharacterCheer();
        hits++;
        Data.Instance.events.OnBossHitsUpdate(hits);
    }
	public void Killed()
	{
		Data.Instance.events.OnSoundFX("FX explot00", -1);
		Death ();
		Invoke ("Died", 0.2f);
	}
	void Died()
	{
		Data.Instance.GetComponent<MusicManager> ().BossMusic (false);

        if(Data.Instance.playMode != Data.PlayModes.SURVIVAL)
		    Game.Instance.level.Complete ();
		
		Data.Instance.events.OnBossActive (false);
        Pool();
    }
	public virtual void Hit(){}
	public virtual void Death(){}
	public virtual void OnPartBroken(BossPart part) { }
}
