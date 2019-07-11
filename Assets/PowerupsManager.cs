using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupsManager : MonoBehaviour {

    public SceneObject Invencible;
    public SceneObject Missile;
    private bool powerUpOn;
	public List<SceneObject> all;
   // private Player player;

	public  void Init()
    {
        Data.Instance.events.OnAddPowerUp += OnAddPowerUp;
		Data.Instance.events.OnAddSpecificPowerUp += OnAddSpecificPowerUp;
    }    
    public void OnDestroy()
    {
        Data.Instance.events.OnAddPowerUp -= OnAddPowerUp;
		Data.Instance.events.OnAddSpecificPowerUp -= OnAddSpecificPowerUp;
		CancelInvoke ();
    }
    public bool CanBeThrown()
    {
        if (powerUpOn) return false;
        return true;
    }
	public void ResetVersusPowerups()
	{
		if (all.Count == 0)
			return;
		foreach (SceneObject so in all) {
			if (so != null)
				so.Pool ();
		}
		all.Clear ();
	}
	void OnAddSpecificPowerUp(string POName, Vector3 pos)
	{		
		powerUpOn = true;
		SceneObject newSO = null;

		if(POName == "Missile")
			newSO = ObjectPool.instance.GetObjectForType("GrabbableMissile_real", false);
		else if(POName == "Invencible")
			newSO = ObjectPool.instance.GetObjectForType("GrabbableInvensible_real", false);

		if (newSO)
		{
			Game.Instance.sceneObjectsManager.AddSceneObject(newSO, pos);
			//newSO.Restart(pos);
			newSO.transform.localEulerAngles = Vector3.zero;
			all.Add (newSO);
		}
		Invoke("Reset", 5);
	}
    void OnAddPowerUp(Vector3 pos)
    {
		if (!CanBeThrown ())
			return;
        powerUpOn = true;
        SceneObject newSO = null;
        int rand = Random.Range(0, 10); 

       // if(rand<70)
            newSO = ObjectPool.instance.GetObjectForType(Missile.name, true);
       // else
          //  newSO = ObjectPool.instance.GetObjectForType(Invencible.name, true);

        if (newSO)
        {
            int force = 600;
            pos.y += 1.2f;
			Game.Instance.sceneObjectsManager.AddSceneObject(newSO, pos);
           // newSO.Restart(pos);
            newSO.transform.localEulerAngles = Vector3.zero;
            Vector3 direction = ((newSO.transform.forward * force) + (Vector3.up * (force * 1.8f)));
            newSO.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Acceleration);
        }
        Invoke("Reset", 5);
    }
    void Reset()
    {
        powerUpOn = false;
    }
}
