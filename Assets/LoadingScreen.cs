using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {
	
    public MissionsManager missionsManager_in_scene;
    public BossesPool boss_pool_in_scene;
    public bool useLocalAssets;
    public Text field;
    public Image progressBar;

    void Start () {
#if UNITY_ANDROID || UNITY_IOS && !UNITY_EDITOR
        useLocalAssets = false;
#endif
        if (LevelDataDebug.Instance || useLocalAssets)
            NextScreen();
        else 
            Invoke("LoadBundles", 0.1f);
	}
    private void Update()
    {
        progressBar.fillAmount = Data.Instance.assetsBundleLoader.Progress;
    }
    void NextScreen()
    {
        Data.Instance.missions.Init();
        if (LevelDataDebug.Instance)
        {
            Data.Instance.missions.MissionActiveID = LevelDataDebug.Instance.missionID;
            Data.Instance.videogamesData.actualID = LevelDataDebug.Instance.videogameID;
            Data.Instance.LoadLevel("Game");
        }
        else
        {

            if (Data.Instance.playMode == Data.PlayModes.PARTYMODE)
                Data.Instance.LoadLevel("MainMenu");
            else
                Data.Instance.LoadLevel("MainMenuMobile");
        }

        }
	void LoadBundles () {
        if (Data.Instance.isAndroid)
        {
            // StartCoroutine( Data.Instance.assetsBundleLoader.DownloadAll(OnLoaded) ); 
            field.text = "DOWNLOADING ROMS...";
            StartCoroutine(Data.Instance.assetsBundleLoader.DownloadAll(Data.ServerAssetsUrl(), OnLoaded));
        }
        else
        {
#if UNITY_WEBGL
            Data.Instance.missions.Init();
            Data.Instance.LoadLevel("Intro");
#else
            //solo se juega Story Mode!
            Data.Instance.missions.Init();
            if(Data.Instance.playMode == Data.PlayModes.PARTYMODE)
                Data.Instance.LoadLevel("MainMenu");
            else
                Data.Instance.LoadLevel("Intro");
           // Data.Instance.LoadLevel("Settings");
#endif
        }
	}
    void OnLoaded(string result)
    {
        Debug.Log("AssetsBundle OnLoaded isSuccess: " + result);
        field.text = "";
        if (result == "ok")
        {
            DestroyImmediate(VoicesManager.Instance.gameObject);
            DestroyImmediate(TextsManager.Instance.gameObject);
            DestroyImmediate(missionsManager_in_scene.gameObject);           

            Data.Instance.assetsBundleLoader.GetAsset("missionsmanager.all", "missionsmanager");           
            Data.Instance.assetsBundleLoader.GetAsset("voicesmanager.all", "voicesmanager");
            Data.Instance.assetsBundleLoader.GetAsset("madrollerssfx.all", "madrollerssfx");
            Data.Instance.assetsBundleLoader.GetAsset("textsmanager.all", "textsmanager");
            BossesLoaded();            
        }
        else
            NextScreen();
    }
    void BossesLoaded()
    {
        Debug.Log("BossesLoaded");        
        GameObject go = Data.Instance.assetsBundleLoader.GetGo("bossesmanager.all", "bossesmanager");
        BossesPool bossesPool = go.GetComponent<BossesPool>();
        BossesPool bossesPoolInScene = boss_pool_in_scene.GetComponent<BossesPool>();
        bossesPoolInScene.assets = bossesPool.assets;
        bossesPoolInScene.modules = bossesPool.modules;
        bossesPoolInScene.AllLoaded();
        print("cargo boss_pool_in_scene");
        Data.Instance.missions.Init();
        Data.Instance.LoadLevel("Intro");       
    }
}
