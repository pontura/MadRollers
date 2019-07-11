using UnityEngine;
using System.Collections;

public class VoicesAutomatic : MonoBehaviour {

    private int seconds_to_say_aburres = 6;
    private int seconds_to_say_shoot = 10;
    private int seconds_didnt_shoot = 0;

	void Start () {
        //Data.Instance.events.OnAvatarShoot += OnAvatarShoot;
        //Data.Instance.events.OnSoundFX += OnSoundFX;

        Invoke("Loop", 4);
	}
    void OnDestroy()
    {
       // Data.Instance.events.OnAvatarShoot -= OnAvatarShoot;
       // Data.Instance.events.OnSoundFX -= OnSoundFX;
    }
    void Loop()
    {
        Invoke("Loop", 2);
        seconds_didnt_shoot++;
        if (seconds_didnt_shoot == seconds_to_say_aburres)
        {
            Data.Instance.voicesManager.VoiceSecondaryFromResources("me_aburres");
        } else
        if (seconds_didnt_shoot > seconds_to_say_shoot)
        {
            Data.Instance.voicesManager.VoiceSecondaryFromResources("vamos_dispara_de_una_vez");
            seconds_didnt_shoot = 0;
        }
    }
   
    void OnAvatarShoot(int playerID)
    {
        seconds_didnt_shoot = 0;
    }
    string lastKill;
    void OnSoundFX(string name)
    {
        if (name == "enemyDead" &&  (Random.Range(0, 100) < 50))
        {
            string newKill = GetRandomKill();
            if(newKill == lastKill)
            {
                OnSoundFX(name);
                return;
            }
            lastKill = newKill;
            Data.Instance.voicesManager.VoiceSecondaryFromResources(lastKill);
        }
    }
    private string GetRandomKill()
    {
        if (Random.Range(0, 100) < 33)
           return "lo_Estas_disfrutando";
        else if (Random.Range(0, 100) < 66)
            return "te_gusta_la_sangre";
        else
            return "muy_bien";
    }
}
