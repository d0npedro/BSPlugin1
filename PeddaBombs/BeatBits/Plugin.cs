using System.Linq;
using BS_Utils.Utilities;
using GameChanger;
using IPA;
using IPA.Logging;
using JetBrains.Annotations;
using Sponsor;
using UnityEngine;
using UnityEngine.UI;

namespace BeatBits
{
    [UsedImplicitly]
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
    	internal enum GameMode
    	{
    		Solo,
    		Online
    	}

    	public static string PluginName;

    	public static bool debug;

    	internal static GameMode gameMode;

    	internal static GameScenesManager _gameSceneManager;

    	public string Name => PluginName;

    	public string Version => "1.39.1";

    	public static IPA.Logging.Logger Logger { get; internal set; }

    	internal static GameScenesManager gameSceneManager
    	{
    		get
    		{
    			if (_gameSceneManager == null)
    			{
    				_gameSceneManager = Object.FindObjectOfType<GameScenesManager>();
    			}
    			return _gameSceneManager;
    		}
    	}

    	static Plugin()
    	{
    		PluginName = "LIV StreamerKit";
    		debug = false;
    	}

    	[Init]
    	public void Init(IPA.Logging.Logger logger)
    	{
    		Logger = logger;
    	}

    	[OnStart]
    	public void OnApplicationStart()
    	{
    		Settings.Load();
    		debug = Settings.debug.value;
    		Service.Start();
    		Sponsor.ControllerSetup.OnApplicationStart();
    		ControllerSetup.OnApplicationStart();
    		BeatBitsGameObject.Instance.Init();
    		BSEvents.OnLoad();
    		BSEvents.lateMenuSceneLoadedFresh += BSEvents_lateMenuSceneLoadedFresh;
    	}

    	private void BSEvents_lateMenuSceneLoadedFresh(ScenesTransitionSetupDataSO obj)
    	{
    		Button button = Resources.FindObjectsOfTypeAll<Button>().First((Button x) => x.name == "OnlineButton");
    		Button button2 = Resources.FindObjectsOfTypeAll<Button>().First((Button x) => x.name == "SoloButton");
    		Button button3 = Resources.FindObjectsOfTypeAll<Button>().First((Button x) => x.name == "PartyButton");
    		Button button4 = Resources.FindObjectsOfTypeAll<Button>().First((Button x) => x.name == "CampaignButton");
    		button.onClick.AddListener(delegate
    		{
    			gameMode = GameMode.Online;
    		});
    		button2.onClick.AddListener(delegate
    		{
    			gameMode = GameMode.Solo;
    		});
    		button3.onClick.AddListener(delegate
    		{
    			gameMode = GameMode.Solo;
    		});
    		button4.onClick.AddListener(delegate
    		{
    			gameMode = GameMode.Solo;
    		});
    	}

    	[OnExit]
    	public void OnApplicationQuit()
    	{
    		gameSceneManager.transitionDidFinishEvent -= BeatBitsGameObject.Instance.TransitionDidFinishEvent;
    		ControllerSetup.OnApplicationQuit();
    		Service.Stop();
    	}

    	public static void Log(string message)
    	{
    		if (debug)
    		{
    			Logger.Info(message);
    		}
    	}

    	public static void Log(string format, params object[] args)
    	{
    		Log(string.Format(format, args));
    	}
    }
}
