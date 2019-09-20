using UnityEngine;
using System.Collections;
using DG.Tweening;

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
            pinche.gameObject.transform.DOMoveY(initialY + attackHeight, speed).OnComplete(setOff);
       }
   }
   public void setOff()
   {
       if (!isOn) return;
       isOn = false;
   }
}
