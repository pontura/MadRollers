using UnityEngine;
using Yaguar.Auth;

public class RegisterScreen : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text field;
    [SerializeField] GameObject panel;

    private void Start()
    {
        Close();
        Events.OpenRegister += OpenRegister;
    }
    private void OnDestroy()
    {
        Events.OpenRegister -= OpenRegister;
    }
    public void OpenRegister()
    {
        panel.SetActive(true);
        field.text = "You're not registered to view this content";      
    }
    public void Register()
    {
        Data.Instance.socialAuth.Init((authCode) =>
        {
            Debug.Log("#socialAuth: " + authCode);
            if (authCode != "")
            {
                PlayerPrefs.SetString("authCode", authCode);
                SignInWithPlayGames(authCode);
            }
            else
                Close();
        });
    }
    void SignInWithPlayGames(string authCode)
    {
        FirebaseAuthManager.Instance.SignInWithPlayGames(authCode, (success) =>
        {
            if (success)
                LoopForUserReady();
            else
                Close();
        });
    }
    void LoopForUserReady()
    {
        print("LoopForUserReady IsReadyToInit " + UserData.Instance.IsReadyToInit());
        if (UserData.Instance.IsReadyToInit())
        {
            Data.Instance.LoadLevel("MainMenuMobile");
            Close();
        }
        else
            Invoke("LoopForUserReady", 0.1f);
    }
    public void Close()
    {
        panel.SetActive(false);
    }

}
