using UnityEngine;
using System.Collections;

public class EnemyTankBehavior : MonoBehaviour {
	
	
    //public float activationArea = 5f;
    //public float rotationSpeed;
    //public float speed;
    //public float speedX;
    //private Vector3 aimPosition;	
    //private Transform characterTransform;
    //private EnemyBehavior enemyBehavior;

	
    //void Start () {
    //    characterTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBehavior>().transform;
    //    enemyBehavior = GetComponent<EnemyBehavior>();
    //    transform.rotation = Quaternion.Euler(0, 180, 0);
    //}
    //void Update () {

    //    if(enemyBehavior.checkHitted()) return;
    //    if(enemyBehavior.checkDie()) { enemyBehavior.Die(); return; }
    //    if(enemyBehavior.checkJumpedOver()) return;
		
    //    float distance = characterTransform.position.z;
    //    //muera al irse de pantalla
    //    if(distance-2 > transform.position.z){
    //        Destroy(gameObject);
    //    } else
    //    if(distance > transform.position.z)
    //    {
    //        transform.rotation = Quaternion.Euler(0, 180, 0);
    //        enemyBehavior.setTranslation(new Vector3(0,0,Time.deltaTime*speed));
    //        return;
    //    }
    //    //no se activa hasta que no este a esta distancia:
    //    if(distance+30 < transform.position.z)
    //        return;
		
    //    if(distance+activationArea > transform.position.z)
    //    {		
    //        float characterX = characterTransform.position.x;
    //        float moveX = 0;
    //        if( (enemyBehavior.getState() == 0 ) && Mathf.Abs(characterX-gameObject.transform.position.x)>0.2f)
    //        {
    //            if(characterX>gameObject.transform.position.x)
    //                moveX = -speedX*Time.deltaTime;
    //            else if(characterX<gameObject.transform.position.x)
    //                moveX = +speedX*Time.deltaTime;
    //        }
			
    //        enemyBehavior.setTranslation(new Vector3(moveX,0,Time.deltaTime*speed));
			
    //        aimPosition = new Vector3(characterTransform.position.x, characterTransform.position.y, characterTransform.position.z);
    //        var rotate = Quaternion.LookRotation(aimPosition - transform.position); 
				
    //        transform.rotation = rotate;
    //    }		
    //}
	
}

