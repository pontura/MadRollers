using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


public class AssetsBundleLoader : MonoBehaviour
{
    states state;
    enum states
    {
        FIRST_BUNDLES,
        SECOND_BUNDLES
    }
    private float downloadProgress = 0.0f;
    private List<string> dataPaths;
    string loadedHashes;
    private float downloadedBytes = 0f;
    private string currentPack;
    public Dictionary<string, AssetBundle> bundles;
    public bool allLoaded;
    int totalFirstBundles = 0;
    int totalGirls = 0;
    string url;
    bool isFirstTime;

#if UNITY_IOS
    string mainBundlePath = "iOS";
#elif UNITY_WEBGL
    string mainBundlePath = "Web";
#else
    string mainBundlePath = "Android";
#endif

    public string CurrentPack
    {
        get => currentPack;
        set => currentPack = value;
    }

    public float Progress
    {
        get => downloadProgress;
    }
    public void ResetAll()
    {
        foreach (string assetName in dataPaths)
        {
            try
            {
                AssetBundle ab = bundles[assetName];
                if (ab != null)
                    ab.Unload(false);
            }
            catch
            {
                Debug.Log("Dictionary empty");
            }

        }
    }
    public void SetAssetsBundleServer()
    {
        dataPaths = new List<string>();

        dataPaths.Add("missionsmanager.all");
        dataPaths.Add("bossesmanager.all");
        dataPaths.Add("voicesmanager.all");
       // dataPaths.Add("music.all");
        dataPaths.Add("madrollerssfx.all");
        dataPaths.Add("textsmanager.all");
        totalFirstBundles = dataPaths.Count;
        bundles = new Dictionary<string, AssetBundle>();
    }
    public string GetHashFor(string key)
    {
        return manifest.GetAssetBundleHash(key).ToString();
    }
    public float DownloadedMegas()
    {
        return downloadedBytes / 1000000.0f;
    }
    AssetBundleManifest manifest;
    Action<string> onSuccess;
    AssetBundle mainBundle;



    public IEnumerator DownloadAll(string _url, Action<string> onSuccess)
    {
        this.url = _url;

        if (dataPaths == null)
        {
            SetAssetsBundleServer();
            isFirstTime = true;
        }
        else
        {
            isFirstTime = false;
        }

        Debug.Log("Vuelve a Bajar: " + isFirstTime + "   mainBundle " + mainBundle + "   dataPaths: " + dataPaths);
        this.onSuccess = onSuccess;

        using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url + mainBundlePath ))
        {
            Debug.Log("Loading from url : " + url + mainBundlePath + "/" + mainBundlePath);
            AsyncOperation op = request.SendWebRequest();
            while (!op.isDone)
            {
                downloadProgress = request.downloadProgress;
                downloadedBytes = request.downloadedBytes;
                yield return new WaitForEndOfFrame();
            }
            Debug.Log("Loading Manifest done");
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                onSuccess("error");
            }
            else
            {
                mainBundle = DownloadHandlerAssetBundle.GetContent(request);
                manifest = mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                StartCoroutine(LoadBundlesFromManifest());
                mainBundle.Unload(false);
            }
        }
       
    }


    public IEnumerator LoadBundlesFromManifest()
    {
        int bundleID = 0;
        foreach (string key in dataPaths)
        {
            Hash128 hash = manifest.GetAssetBundleHash(key);
            if (isFirstTime)
                loadedHashes += hash.ToString() + "_";
            else
            {
                if (CheckIfHashIsNew(hash.ToString(), bundleID))
                {
                    StopAllCoroutines();
                    // Data.Instance.ResetAll();
                }
            }
            bundleID++;
            if (isFirstTime)
            {
                CurrentPack = key;
                yield return DownloadAndCacheAssetBundle(key, hash, OnLoaded);
            }
        }
        if (!isFirstTime)
            onSuccess("nothing new!");
    }
    bool CheckIfHashIsNew(string hash, int id)
    {
        string[] arr = loadedHashes.Split("_"[0]);
        if (id <= arr.Length)
        {
            //Debug.Log("saved hash: " + arr[id] + "   the hash: " + hash);
            if (arr[id] == hash)
                return false;
        }
        return true;
    }

    int loadedParts = 0;
    void OnLoaded(bool isLoaded)
    {       
        loadedParts++;
        if (loadedParts >= dataPaths.Count)
            onSuccess("ok");
    }
    IEnumerator DownloadAndCacheAssetBundle(string uri, Hash128 hash, System.Action<bool> OnLoaded)
    {
        string realURL = url + uri;
        Debug.Log("Load: " + realURL);
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(realURL, hash); //, hash, 0);
        request.SendWebRequest();
        while (!request.isDone)
        {
            downloadProgress = request.downloadProgress;
            downloadedBytes = request.downloadedBytes;
            yield return new WaitForEndOfFrame();
        }
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Error downloading assetBundle: " + realURL);
            onSuccess("error");
            yield break;
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
            if (bundles == null)
                bundles = new Dictionary<string, AssetBundle>();
            bundles.Add(uri, bundle);

            OnLoaded(true);
        }
    }

    public GameObject GetGo(string bundleName, string asset)
    {
        AssetBundle assetBundle = bundles[bundleName];
        return assetBundle.LoadAsset(asset) as GameObject;
    }
    public GameObject GetAsset(string bundleName, string asset)
    {
        Debug.Log("GET Asset  bundleName: " + bundleName + " asset: " + asset);
        AssetBundle assetBundle = bundles[bundleName];
        GameObject go = assetBundle.LoadAsset(asset) as GameObject;
        return Instantiate(go);
    }
    public TextAsset GetAssetAsText(string bundleName, string asset)
    {
        //   print("GET Asset  bundleName: " + bundleName + " asset: " + asset);
        AssetBundle assetBundle = bundles[bundleName];
        TextAsset go = assetBundle.LoadAsset(asset) as TextAsset;
        return Instantiate(go);
    }
    public AudioClip GetAssetAsAudioClip(string bundleName, string asset)
    {
        AssetBundle assetBundle = bundles[bundleName];
        AudioClip go = assetBundle.LoadAsset(asset) as AudioClip;
        return go;
    }
}