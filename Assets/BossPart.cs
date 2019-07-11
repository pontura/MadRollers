using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPart : MonoBehaviour {

	public Boss boss;
	bool called;
	bool isOn;
	BossPartMarker marker;

	public void Init(Boss _boss, string bossAssetPath = null)
	{
		this.boss = _boss;
		Utils.RemoveAllChildsIn (transform);
		if (bossAssetPath != null) {
			GameObject newGO = Instantiate(Resources.Load("bosses/" + bossAssetPath, typeof(GameObject))) as GameObject;
			newGO.transform.SetParent (transform);
			newGO.transform.localScale = Vector3.one;
			newGO.transform.localEulerAngles = Vector3.zero;
			newGO.transform.localPosition = Vector3.zero;
		}
		isOn = true;

		if(gameObject.activeSelf)
		{
			marker = ObjectPool.instance.GetObjectForType("BossPartMarker", false) as BossPartMarker;
			Game.Instance.sceneObjectsManager.AddSceneObjectAndInitIt(marker, transform.position, transform);
			marker.transform.localEulerAngles = Vector3.zero;
		}
	}
	void Update()
	{
		if (!isOn)
			return;
		if (transform.position.y < -15)
			OnActivate ();
	}
	public void OnActivate()
	{
		if (called)
			return;

		ParticlesSceneObject effect = ObjectPool.instance.GetObjectForType("ExplotionEffectBoss", false) as ParticlesSceneObject;
		effect.SetColor (Color.red);
		Game.Instance.sceneObjectsManager.AddSceneObjectAndInitIt(effect, transform.position);

		if(marker != null)
			marker.Pool ();
		
		called = true;
		CancelInvoke ();

		if( boss.HasOnlyOneLifeLeft() )
			Data.Instance.events.OnProjectilStartSnappingTarget (transform.position);		

		boss.OnPartBroken (this);
		gameObject.SetActive (false);

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
}
