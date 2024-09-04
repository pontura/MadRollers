using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickWeaponUI : MonoBehaviour {

 //   public Sprite[] weaponSprites;
	//public Image image;
	//public GameObject panel;
	//int playerID;
	//public GameObject forbidden;

	//void Start () {
	//	forbidden.SetActive (false);
	//	playerID = GetComponent<JoystickPlayer> ().playerID;
	//	Reset ();
	//	Data.Instance.events.OnChangeWeapon += OnChangeWeapon;
	//	Data.Instance.events.OnAvatarShoot += OnAvatarShoot;
	//	image.color = Data.Instance.multiplayerData.colors [playerID];
	//}
	//void OnDestroy () {
	//	Data.Instance.events.OnChangeWeapon -= OnChangeWeapon;
	//	Data.Instance.events.OnAvatarShoot -= OnAvatarShoot;
	//}
	//void OnAvatarShoot(int _playerID)
	//{
	//	if (playerID != _playerID)
	//		return;
	//	if(Game.Instance.state != Game.states.PLAYING)
	//		forbidden.SetActive (true);
	//	Invoke("Reset", 1);
	//}
	//void OnChangeWeapon (int _playerID, Weapon.types type) {
		
	//	if (playerID != _playerID)
	//		return;
		
	//	panel.SetActive (true);

 //       switch(type)
 //       {
 //           case Weapon.types.SIMPLE: image.sprite = weaponSprites[0]; break;
 //           case Weapon.types.DOUBLE: image.sprite = weaponSprites[1]; break;
 //           case Weapon.types.TRIPLE: image.sprite = weaponSprites[2]; break;
 //       }		

	//	Invoke ("Reset", 1);

	//}
	//public void Reset()
	//{
	//	panel.SetActive (false);
	//	forbidden.SetActive (false);
	//}
}
