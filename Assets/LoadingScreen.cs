using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour {
	
    public MissionsManager missionsManager_to_instantiate;

    void Start () {
        if (LevelDataDebug.Instance)
        {
            Data.Instance.missions.Init();
            // Data.Instance.isReplay = true;
            Data.Instance.LoadLevel("LevelSelector");
        } else if (Data.Instance.playMode == Data.PlayModes.STORYMODE)
        {
            Data.Instance.missions.Init();
            Data.Instance.LoadLevel("LevelSelector");
        } else
        Invoke("LoadBundles", 1);
	}
	void LoadBundles () {
       // print("::::::::::: LoadBundles: ");
        if (Data.Instance.isAndroid)
        {
            //LL();
            //return;

            Data.Instance.missions.Init();
            Data.Instance.LoadLevel("MainMenuMobile");
            // 
           // 
        }
        else
        {
            Instantiate(missionsManager_to_instantiate);
            Data.Instance.LoadLevel("Settings");
        }
	}
    void OnAllLoaded(string isSuccess)
    {
        print("::::::::::: isSuccess: " + isSuccess);
        Data.Instance.assetsBundleLoader.GetAsset("missionsmanager", "missionsmanager");
        ObjectPool.instance.bossesPool.AddAll();

        
        return;


        
        Data.Instance.missions.Init();
        Data.Instance.LoadLevel("MainMenuMobile");
    }




    string url = "https://gamedb.doublespicegames.com/assets/sss-dev/Android/pontura/";
    void LL()
    {
        StartCoroutine(Load("shaders"));
        StartCoroutine(Load("materials"));

        Invoke("OnDone", 1);
    }
    void OnDone()
    {
        StartCoroutine(Load("bosses", true));
    }

    IEnumerator Load(string uri, bool bosses = false)
    {
        while (!Caching.ready)
            yield return null;

        var www = WWW.LoadFromCacheOrDownload(url + uri, 5);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            yield return null;
        }
        AssetBundle myLoadedAssetBundle = www.assetBundle;
        if (bosses)
        {
            Instantiate(myLoadedAssetBundle.LoadAsset("apple"));
            Instantiate(myLoadedAssetBundle.LoadAsset("alien"));
        }
    }
}
