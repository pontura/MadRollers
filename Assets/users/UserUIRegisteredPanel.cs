using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserUIRegisteredPanel : MonoBehaviour
{
    public AvatarThumb avatarThumb;
    public Text field;
    UserDataUI userDataUI;

    public void Init(UserDataUI userDataUI, string userID, string _username)
    {
        this.userDataUI = userDataUI;
        avatarThumb.Init(userID );
        field.text = _username;
    }
    public void OnEditUserData()
    {
        if(field.text == "")
        {
            UsersEvents.OnPopup("Nombre de usuario incorrecto");
            return;
        }
        
       userDataUI.EditData();
    }
}
