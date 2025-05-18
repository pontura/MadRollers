using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPacmans : Boss {

	public GameObject bossPartsContainer;
	public float time_to_init_enemies = 0.25f;
	[HideInInspector]
	public BossPart[] parts;

	public override void OnRestart(Vector3 pos)
	{
		Data.Instance.events.OnBossSetNewAsset ("pacman");
		Data.Instance.events.OnBossSetTimer (38);

		base.OnRestart (pos);	
		parts = bossPartsContainer.GetComponentsInChildren<BossPart> ();

		foreach( BossPart part in parts)
			part.gameObject.SetActive (false);

		SetTotal (parts.Length);
		Init ();
	}
	void Update()
	{
		if (!isActive)
			return;
		float avatarsDistance = Game.Instance.level.charactersManager.getDistance ();
		if (avatarsDistance + distance_from_avatars < transform.localPosition.z)
			return;
		float _z = avatarsDistance + distance_from_avatars;

		Vector3 pos = transform.localPosition;
		pos.z = _z;
		transform.localPosition = pos;
	} 
	int partID;
	void Init()	
	{
		if (partID >= parts.Length)
			return;
		parts [partID].gameObject.SetActive (true);
		Invoke ("Init", time_to_init_enemies);
		partID++;
	}
	public override void OnPartBroken(BossPart part)
	{
		breakOut ();
	}

}
