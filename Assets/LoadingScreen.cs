using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour {
	
    public MissionsManager missionsManager_in_scene;
    public BossesPool boss_pool_in_scene;
    public bool useLocalAssets;

    void Start () {
#if UNITY_ANDROID && !UNITY_EDITOR
        useLocalAssets = false;
#endif
        if (LevelDataDebug.Instance || useLocalAssets)
        {
            Data.Instance.missions.Init();
            // Data.Instance.isReplay = true;
            Data.Instance.LoadLevel("LevelSelector");
        } else 
            Invoke("LoadBundles", 1);
	}
	void LoadBundles () {
        if (Data.Instance.isAndroid)
        {
           // StartCoroutine( Data.Instance.assetsBundleLoader.DownloadAll(OnLoaded) ); 
            StartCoroutine(Data.Instance.assetsBundleLoader.DownloadAndCacheAssetBundle("missionsmanager.all", OnLoaded));
        }
        else
        {
#if UNITY_WEBGL
            Data.Instance.missions.Init();
            Data.Instance.LoadLevel("Intro");
#else
            //solo se juega Story Mode!
            Data.Instance.missions.Init();
            Data.Instance.LoadLevel("Intro");
           // Data.Instance.LoadLevel("Settings");
#endif
        }
	}
    void OnLoaded(string isSuccess)
    {
        Debug.Log("AssetsBundle OnLoaded isSuccess: " + isSuccess);

        if (isSuccess == "ok")
        {
            DestroyImmediate(missionsManager_in_scene.gameObject);
            Data.Instance.assetsBundleLoader.GetAsset("missionsmanager.all", "missionsmanager");
            StartCoroutine(Data.Instance.assetsBundleLoader.DownloadAndCacheAssetBundle("bossesmanager.all", OnBossesLoaded));
        }
       
        //Data.Instance.missions.Init();
        //Data.Instance.LoadLevel("Intro");
    }
    void OnBossesLoaded(string isSuccess)
    {
        Debug.Log("AssetsBundle OnBossesLoaded isSuccess: " + isSuccess);
      
        if (isSuccess == "ok")
        {
            GameObject go = Data.Instance.assetsBundleLoader.GetGo("bossesmanager.all", "bossesmanager");
            BossesPool bossesPool = go.GetComponent<BossesPool>();
            BossesPool bossesPoolInScene = boss_pool_in_scene.GetComponent<BossesPool>();
            bossesPoolInScene.assets = bossesPool.assets;
            bossesPoolInScene.modules = bossesPool.modules;
            bossesPoolInScene.AllLoaded();
            print("cargo boss_pool_in_scene");
        }
        Data.Instance.missions.Init();
        Data.Instance.LoadLevel("Intro");
    }




    //string url = "https://gamedb.doublespicegames.com/assets/sss-dev/Android/pontura/";
    //void LL()
    //{
    //    StartCoroutine(Load("shaders"));
    //    StartCoroutine(Load("materials"));

    //    Invoke("OnDone", 1);
    //}
    //void OnDone()
    //{
    //    StartCoroutine(Load("bosses", true));
    //}

    //IEnumerator Load(string uri, bool bosses = false)
    //{
    //    while (!Caching.ready)
    //        yield return null;

    //    var www = WWW.LoadFromCacheOrDownload(url + uri, 5);
    //    yield return www;
    //    if (!string.IsNullOrEmpty(www.error))
    //    {
    //        Debug.Log(www.error);
    //        yield return null;
    //    }
    //    AssetBundle myLoadedAssetBundle = www.assetBundle;
    //    if (bosses)
    //    {
    //        Instantiate(myLoadedAssetBundle.LoadAsset("apple"));
    //        Instantiate(myLoadedAssetBundle.LoadAsset("alien"));
    //    }
    //}
}
