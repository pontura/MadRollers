using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class UsersEvents {
	public static System.Action OnRegistartionDone = delegate { };
    public static System.Action OnUserUploadDone = delegate { };
    public static System.Action OnUserRegisterCanceled = delegate { };
    public static System.Action<string> OnPopup = delegate { };
    public static System.Action FileUploaded = delegate { };
}
