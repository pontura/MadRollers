using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class AvatarImages : MonoBehaviour
{
    public List<Data> all;
    [Serializable]
    public class Data
    {
        public string userID;
        public Texture2D texture;
    }
    UserData userData;

    System.Action OnLoaded;

    private void Awake()
    {
        UsersEvents.FileUploaded += FileUploaded;
        userData = GetComponent<UserData>();
    }
    void FileUploaded()
    {
        ResetAvatar(UserData.Instance.userID);
        GetImageFor(UserData.Instance.userID, null);
    }
    public void GetImageFor(string userID, System.Action<Texture2D> OnLoaded)
    {
        foreach (Data d in all)
        {
            if (d.userID == userID && d.texture != null)
            {
                OnLoaded(d.texture);
                return;
            }
        }

        StartCoroutine(DownloadImage(userID, OnLoaded));
    }
    IEnumerator DownloadImage(string userID, System.Action<Texture2D> OnLoaded)
    {
        string url = userData.URL + userData.imagesURL + userID + ".png";
        print("DownloadImage from url " + url);
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            Texture2D t = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Data data = new Data();
            data.userID = userID;
            data.texture = t;
            all.Add(data);
            if(OnLoaded != null)
                OnLoaded(t);
        }
    }
    public void ResetAvatar(string userID)
    {
        Data dataToRemove = null;
        foreach(Data d in all)
        {
            if (d.userID == userID)
                dataToRemove = d;
        }

        if(dataToRemove != null)
            all.Remove(dataToRemove);
    }
}
