using System.IO;
using GameChanger;
using BeatBits;
using UnityEngine;
using UnityEngine.SceneManagement;

    public static class ControllerSetup
    {
    	private static GameObject gameChangerGameObjectPrefab;

    	public static string assetBundleName = "livstreamerkitassets";

    	public static string assetName = "TwitchController";

    	public static string reconstructionFileName = "TwitchController.txt";

    	private static string assetBundlePath = Path.Combine(Utils.GetAssemblyPath(), assetBundleName);

    	public static void OnApplicationStart()
    	{
    		SabotageQueue.Setup();
    		SubscriptionQueue.Setup();
    		CheerQueue.Setup();
    		NewFollowerQueue.Setup();
    	}

    	public static void OnApplicationQuit()
    	{
    	}

    	public static void LoadedScene(Scene scene)
    	{
    		Debug.Log("LoadedScene: " + scene.name);
    		if (!(scene.name != "GameCore"))
    		{
    			Settings.Load();
    			LoadGameChangerAsset();
    		}
    	}

    	private static void LoadGameChangerAsset()
    	{
    		if (gameChangerGameObjectPrefab == null)
    		{
    			gameChangerGameObjectPrefab = Utils.LoadGameChangerAsset(assetBundlePath, reconstructionFileName, assetName);
    		}
    		Object.Instantiate(gameChangerGameObjectPrefab);
    	}
    }

