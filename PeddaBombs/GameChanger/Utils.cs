using System.IO;
using System.Reflection;
using BeatBits;
using UnityEngine;

namespace GameChanger
{
    public static class Utils
    {
    	private static AssetBundle assetBundle;

    	private static GameObject gameObjectReference;

    	public static GameObject FindChildByName(GameObject parent, string name)
    	{
    		if (parent.name == name)
    		{
    			return parent;
    		}
    		for (int i = 0; i < parent.transform.childCount; i++)
    		{
    			GameObject gameObject = FindChildByName(parent.transform.GetChild(i).gameObject, name);
    			if (gameObject != null)
    			{
    				return gameObject;
    			}
    		}
    		return null;
    	}

    	public static string GetAssemblyPath()
    	{
    		if (Application.isEditor)
    		{
    			return Path.Combine(Application.dataPath, "StreamingAssets");
    		}
    		return new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
    	}

    	public static GameObject LoadGameChangerAsset(string assetBundlePath, string reconstructionFileName, string assetName)
    	{
    		assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
    		if (assetBundle == null)
    		{
    			Plugin.Log("Unable to load Asset Bundle at path: " + assetBundlePath);
    			AssetBundle.UnloadAllAssetBundles(unloadAllObjects: true);
    			return null;
    		}
    		if (string.IsNullOrEmpty(assetName))
    		{
    			Plugin.Log(string.Format("Asset bundle asset name cannot be empty or null!", assetName, assetBundlePath));
    			AssetBundle.UnloadAllAssetBundles(unloadAllObjects: true);
    			return null;
    		}
    		GameObject gameObject = assetBundle.LoadAsset(assetName) as GameObject;
    		if (gameObject == null)
    		{
    			Plugin.Log($"Cannot find asset: {assetName} in AssetBundle at path: {assetBundlePath}");
    			AssetBundle.UnloadAllAssetBundles(unloadAllObjects: true);
    			return null;
    		}
    		GameObject gameObject2 = assetBundle.LoadAsset("AssetBundleMap") as GameObject;
    		if (gameObject2 == null)
    		{
    			Plugin.Log(string.Format("Cannot find asset: {0} in AssetBundle at path: {1}", "AssetBundleMap", assetBundlePath));
    			AssetBundle.UnloadAllAssetBundles(unloadAllObjects: true);
    			return null;
    		}
    		TextAsset textAsset = assetBundle.LoadAsset(reconstructionFileName) as TextAsset;
    		if (textAsset == null || string.IsNullOrEmpty(textAsset.text))
    		{
    			Plugin.Log("Unable to load Reconstruction File at path: " + reconstructionFileName);
    			AssetBundle.UnloadAllAssetBundles(unloadAllObjects: true);
    			return null;
    		}
    		Importer.ReconstructPrefab(gameObject, gameObject2, textAsset.text);
    		Plugin.Log("Loading GameChangerAsset done.");
    		return gameObject;
    	}
    }
}
