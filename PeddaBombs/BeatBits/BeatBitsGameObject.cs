using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace BeatBits
{
    internal class BeatBitsGameObject : MonoBehaviour
    {
    	private bool waitframe = true;

    	private static BeatBitsGameObject _instance;

    	public static BeatBitsGameObject Instance
    	{
    		get
    		{
    			if (!_instance)
    			{
    				_instance = new GameObject("LIV.StreamerKit.BeatBits").AddComponent<BeatBitsGameObject>();
    				Object.DontDestroyOnLoad(_instance.gameObject);
    			}
    			return _instance;
    		}
    		private set
    		{
    			_instance = value;
    		}
    	}

    	public void Init()
    	{
    	}

    	public void Update()
    	{
    		if (waitframe)
    		{
    			WaitFrame();
    		}
    	}

    	internal void WaitFrame()
    	{
    		if (Plugin.gameSceneManager != null)
    		{
    			Plugin.gameSceneManager.transitionDidFinishEvent += TransitionDidFinishEvent;
    			waitframe = false;
    		}
    	}

        internal void TransitionDidFinishEvent(GameScenesManager.SceneTransitionType transitionType, ScenesTransitionSetupDataSO setupData, DiContainer container)
        {
            // Add logic as needed for handling the transition type or setup data
            ControllerSetup.LoadedScene(SceneManager.GetActiveScene());
        }

    }
}
