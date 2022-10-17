using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetBundleBuild
{
    [MenuItem("�ڪ��u��/���]AssetBundle")]
    static void BuildAllAssetBundle()
    {
        string folder = Application.streamingAssetsPath + "/MyAssetBundle";
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        BuildPipeline.BuildAssetBundles(folder, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
