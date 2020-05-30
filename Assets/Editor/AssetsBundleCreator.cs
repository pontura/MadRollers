#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetsBundleCreator : MonoBehaviour
{
    [MenuItem ("Tools/Build/AssetsBundles Android")]
    static void  BuildBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetsBundles/Android", BuildAssetBundleOptions.None, BuildTarget.Android);
    }
    [MenuItem("Tools/Build/AssetsBundles Web")]
    static void BuildWeb()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetsBundles/Web", BuildAssetBundleOptions.None, BuildTarget.WebGL);
    }
    [MenuItem("Tools/Delete PlayerPrefs")]
    static void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
#endif