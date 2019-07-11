using UnityEngine;
using System.Collections;

public class PincheDientes : MonoBehaviour {

   public  GameObject[] pinches;
   public bool isOn;
   private float initialY;
   private float speed = 0.5f;
   private float attackHeight = 1.6f;

   void Awake()
   {
       initialY = transform.position.y - 0.5f;
   }
   public void setOn()
   {

       if (isOn) return;
       isOn = true;
       foreach (GameObject pinche in pinches)
       {
           if (!pinche) continue;
           Hashtable tweenData = new Hashtable();
           tweenData.Add("y", initialY + attackHeight);
           tweenData.Add("time", speed);
           tweenData.Add("easeType", iTween.EaseType.easeOutQuad);
           tweenData.Add("onCompleteTarget", this.gameObject);
           tweenData.Add("onComplete", "setOff");           
           iTween.MoveTo(pinche.gameObject, tweenData);
       }
   }
   public void setOff()
   {
       if (!isOn) return;
       isOn = false;

        return;

       foreach (GameObject pinche in pinches)
       {
           if (!pinche) continue;
           Hashtable tweenData = new Hashtable();
           tweenData.Add("y", initialY);
           tweenData.Add("time", speed);
           tweenData.Add("easeType", iTween.EaseType.easeInQuad);

           iTween.MoveTo(pinche.gameObject, tweenData);
       }
   }
}
