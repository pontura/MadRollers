using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AssetsBundleLoader : MonoBehaviour
{
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

#if UNITY_WEBGL
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

        
        dataPaths.Add("missionsmanager");
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



    public IEnumerator DownloadAll(Action<string> onSuccess)
    {
        Debug.Log("____________ DownloadAll _____________");
        this.onSuccess = onSuccess;

        if (dataPaths == null)
        {
            url = Data.ServerAssetsUrl();
            SetAssetsBundleServer();
            isFirstTime = true;
            Debug.Log("Baja mainBundle");
        }
        else
        {
            isFirstTime = false;
            Debug.Log("Vuelve a Bajar: " + isFirstTime + "   mainBundle " + mainBundle + "   dataPaths: " + dataPaths);
        }

        string realURL = url + mainBundlePath + "/" + mainBundlePath;
        using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(realURL))
        {
            Debug.Log("Loading from url : " + realURL);
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

                StartCoroutine(LoadBundlesFromManifest(onSuccess));
            }
        }
        mainBundle.Unload(false);
    }
    public IEnumerator LoadBundlesFromManifest(Action<string> onSuccess)
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
                    //Data.Instance.ResetAll();
                }
            }
            bundleID++;
            if (isFirstTime)
            {
                CurrentPack = key;
                yield return DownloadAndCacheAssetBundle(key, null);
                //   yield return DownloadAndCacheAssetBundle(key, hash, OnLoaded);
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
        {
            Debug.Log("All Assetsbundles  downloaded count: " + dataPaths.Count);
            allLoaded = true;
            onSuccess("ok");
        }
    }
    public IEnumerator DownloadAndCacheAssetBundle(string uri, System.Action<string> OnSuccess) //Hash128 hash,   //System.Action<bool> OnLoaded)
    {
        url = Data.ServerAssetsUrl();
        string realURL = url + mainBundlePath + "/" + uri;
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(realURL); //, hash, 0);
        Debug.Log("Downloading AssetBundle: " + realURL);
        request.SendWebRequest();
        while (!request.isDone)
        {
            downloadProgress = request.downloadProgress;
            downloadedBytes = request.downloadedBytes;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        if (request.isNetworkError || request.isHttpError)
        {
            OnSuccess("error");
            Debug.Log("Error downloading assetBundle: " + url + uri);
           // Events.OpenStandardSignal(Data.Instance.ResetAll, "No internet access, try again later");
            yield break;
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
            if (bundles == null)
                bundles = new Dictionary<string, AssetBundle>();

            bundles.Add(uri, bundle);
            OnSuccess("ok");
            //OnLoaded(true);
        }
    }
   
    public GameObject GetGo(string bundleName, string asset)
    {
        AssetBundle assetBundle = bundles[bundleName];
        return assetBundle.LoadAsset(asset) as GameObject;
    }
    public GameObject GetAsset(string bundleName, string asset)
    {
        //  print("GET Asset  bundleName: " + bundleName + " asset: " + asset);
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

}