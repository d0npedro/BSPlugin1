using System.Collections;
using IPA.Utilities;
using JetBrains.Annotations;
using UnityEngine;

namespace BeatBits
{
    public class TwitchController : MonoBehaviour
    {
    	private static float eventUpdateRate = 0.5f;

    	private static object _currentObservedEvent;

    	private static SabotageEvent _currentSabotageEvent;

    	private static CheerEvent _currentCheerEvent;

    	private static SubscriptionEvent _currentSubscriptionEvent;

    	private NewFollowerEvent _currentFollowerEvent;

    	private BeatmapObjectManager _beatmapObjectManager;

    	public TshirtSponsor tshirtSponsor;

    	public BitsController bitsController;

    	public SubscriberController subscriberController;

    	public NewFollowerController newFollowerController;

    	private float eventUpdateProgress;

    	private float observeEventDelay;

    	private bool canObserveEvents;

    	private bool _initialized;

    	private BeatmapObjectManager beatMapObjectManager
    	{
    		get
    		{
    			if (_beatmapObjectManager == null)
    			{
    				if (Plugin.gameMode == Plugin.GameMode.Solo)
    				{
    					BeatmapObjectSpawnController obj = Object.FindObjectOfType<BeatmapObjectSpawnController>();
    					_beatmapObjectManager = obj.GetField<IBeatmapObjectSpawner, BeatmapObjectSpawnController>("_beatmapObjectSpawner") as BeatmapObjectManager;
    				}
    				else
    				{
    					MultiplayerLocalActiveClient multiplayerLocalActiveClient = Object.FindObjectOfType<MultiplayerLocalActiveClient>();
    					if (multiplayerLocalActiveClient != null)
    					{
    						_beatmapObjectManager = multiplayerLocalActiveClient.GetField<BeatmapObjectManager, MultiplayerLocalActiveClient>("_beatmapObjectManager");
    					}
    				}
    			}
    			return _beatmapObjectManager;
    		}
    	}

    	private void ProcessSabotageEvent()
    	{
    		observeEventDelay = 0f;
    		Plugin.Log("ObserveEvent: " + _currentSabotageEvent);
    		canObserveEvents = false;
    		bitsController.Begin(beatMapObjectManager, _currentSabotageEvent, delegate
    		{
    			Plugin.Log("ConsumeEvent: " + _currentSabotageEvent);
    			SabotageQueue.ConsumeEvent(out _currentSabotageEvent);
    			canObserveEvents = true;
    			_currentSabotageEvent = null;
    		});
    	}

    	private void ProcessSubscriptionEvent()
    	{
    		observeEventDelay = 1f;
    		Plugin.Log("ObserveEvent: " + _currentSubscriptionEvent);
    		canObserveEvents = false;
    		subscriberController.Begin(beatMapObjectManager, _currentSubscriptionEvent, delegate
    		{
    			Plugin.Log("ConsumeEvent: " + _currentSubscriptionEvent);
    			SubscriptionQueue.ConsumeEvent(out _currentSubscriptionEvent);
    			canObserveEvents = true;
    			_currentSubscriptionEvent = null;
    		});
    	}

    	private void ProcessCheerEvent()
    	{
    		observeEventDelay = 1f;
    		Plugin.Log("ObserveEvent: " + _currentCheerEvent);
    		canObserveEvents = false;
    		bitsController.Begin(beatMapObjectManager, _currentCheerEvent, delegate
    		{
    			Plugin.Log("ConsumeEvent: " + _currentCheerEvent);
    			CheerQueue.ConsumeEvent(out _currentCheerEvent);
    			canObserveEvents = true;
    			_currentCheerEvent = null;
    		});
    	}

    	private void ObtainNewEvent()
    	{
    		if (SabotageQueue.ObserveEvent(out _currentSabotageEvent))
    		{
    			ProcessSabotageEvent();
    		}
    		else if (Settings.enableSubs.value && SubscriptionQueue.ObserveEvent(out _currentSubscriptionEvent))
    		{
    			ProcessSubscriptionEvent();
    		}
    		else if (Settings.enableBits.value && CheerQueue.ObserveEvent(out _currentCheerEvent))
    		{
    			ProcessCheerEvent();
    		}
    	}

    	private void ProcessEvents()
    	{
    		if (_currentSabotageEvent != null)
    		{
    			ProcessSabotageEvent();
    		}
    		else if (_currentSubscriptionEvent != null)
    		{
    			ProcessSubscriptionEvent();
    		}
    		else if (_currentCheerEvent != null)
    		{
    			ProcessCheerEvent();
    		}
    		else
    		{
    			ObtainNewEvent();
    		}
    	}

    	private void ProcessFollowerEvents()
    	{
    		if (NewFollowerQueue.ConsumeEvent(out _currentFollowerEvent))
    		{
    			Plugin.Log("ProcessEvent: " + _currentFollowerEvent);
    			newFollowerController.Begin(_currentFollowerEvent, delegate
    			{
    			});
    		}
    	}

    	private void Init()
    	{
    		bitsController.gameObject.SetActive(value: true);
    		bitsController.Init();
    		subscriberController.gameObject.SetActive(value: true);
    		subscriberController.Init();
    		newFollowerController.gameObject.SetActive(value: true);
    		newFollowerController.Init();
    	}

    	[UsedImplicitly]
    	private void Awake()
    	{
    		Plugin.Log("Twitch Controller Awake: " + Time.frameCount);
    		StartCoroutine(DelayedAwake());
    	}

    	private IEnumerator DelayedAwake()
    	{
    		Plugin.Log("Calling  DelayedAwake Coroutine: " + Time.frameCount);
    		yield return new WaitForSeconds(1f);
    		Init();
    		if (Plugin.gameMode == Plugin.GameMode.Online)
    		{
    			yield return new WaitForSeconds(0.1f);
    			yield return new WaitUntil(() => beatMapObjectManager != null);
    		}
    		_initialized = true;
    		canObserveEvents = true;
    		Plugin.Log("Delayed Awake: " + Time.frameCount);
    	}

    	[UsedImplicitly]
    	private void Update()
    	{
    		if (!_initialized)
    		{
    			return;
    		}
    		if (eventUpdateProgress < eventUpdateRate)
    		{
    			eventUpdateProgress += Time.deltaTime;
    			return;
    		}
    		ProcessFollowerEvents();
    		if (observeEventDelay > 0f)
    		{
    			observeEventDelay = Mathf.Max(observeEventDelay - Time.deltaTime, 0f);
    			return;
    		}
    		eventUpdateProgress = 0f;
    		if (canObserveEvents)
    		{
    			ProcessEvents();
    		}
    	}

    	private void OnDestroy()
    	{
    		Plugin.Log("Twitch Controller OnDestroy: " + Time.frameCount);
    	}
    }
}
