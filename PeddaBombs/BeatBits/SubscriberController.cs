using System;
using IPA.Utilities;
using TMPro;
using UnityEngine;

namespace BeatBits
{
    public class SubscriberController : MonoBehaviour
    {
    	private static int MAX_POOL_OBJECTS = 30;

    	private static SubscriberControllerEvent controllerEvent;

    	public static float timeElapsedAfterBurst;

    	public GameObject subHyperCubePrefab;

    	public Canvas subscriptionCanvas;

    	public TextMeshProUGUI userName;

    	public TextMeshProUGUI message;

    	public RectTransform progressbar;

    	public CanvasGroup canvasGroup;

    	public GameObject endParticles;

    	private Action _onComplete;

    	private float targetProgress;

    	private float progress;

    	private float targetAlpha;

    	private bool _active;

    	private bool _initialized;

    	private int _spawnedCubes;

    	private GamePause _pauseManager;

    	private Signal didPauseSignal;

    	private Signal didResumeSignal;

    	private BeatmapObjectManager _beatMapObjectManager;

    	private ObjectPool<SubHyperCube> _subHyperCubePool;

    	private SubscriptionEvent _currentSubscriptionEvent;

    	private bool _gamePaused;

    	public void Init()
    	{
    		targetProgress = (progress = 0f);
    		targetAlpha = 0f;
    		canvasGroup.alpha = 0f;
    		SubHyperCube[] array = new SubHyperCube[MAX_POOL_OBJECTS];
    		for (int i = 0; i < array.Length; i++)
    		{
    			array[i] = UnityEngine.Object.Instantiate(subHyperCubePrefab).GetComponent<SubHyperCube>();
    			array[i].gameObject.SetActive(value: false);
    		}
    		endParticles.gameObject.SetActive(value: false);
    		_subHyperCubePool = new ObjectPool<SubHyperCube>(array);
    		_initialized = true;
    	}

    	private void Update()
    	{
    		if (!_initialized || _gamePaused)
    		{
    			return;
    		}
    		progress += (targetProgress - progress) * Time.deltaTime * 5f;
    		canvasGroup.alpha += (targetAlpha - canvasGroup.alpha) * Time.deltaTime * 5f;
    		SetProgress(progress);
    		if (_active && controllerEvent != null)
    		{
    			if (controllerEvent.subscriptionElapsed < controllerEvent.subscriptionDuration)
    			{
    				controllerEvent.subscriptionElapsed = Mathf.Min(controllerEvent.subscriptionElapsed + Time.deltaTime, controllerEvent.subscriptionDuration);
    				timeElapsedAfterBurst += Time.deltaTime;
    			}
    			else if (_spawnedCubes == 0)
    			{
    				End();
    			}
    		}
    	}

    	private void InitPauseManager()
    	{
    		if (_pauseManager == null && Plugin.gameMode == Plugin.GameMode.Solo)
    		{
    			PauseController obj = UnityEngine.Object.FindObjectOfType<PauseController>();
    			_pauseManager = obj.GetField<IGamePause, PauseController>("_gamePause") as GamePause;
    			_pauseManager.didPauseEvent += GameDidPause;
    			_pauseManager.didResumeEvent += GameDidResume;
    		}
    	}

    	private void DestroyPauseManager()
    	{
    		if (_pauseManager != null && Plugin.gameMode == Plugin.GameMode.Solo)
    		{
    			_pauseManager.didPauseEvent -= GameDidPause;
    			_pauseManager.didResumeEvent -= GameDidResume;
    			_pauseManager = null;
    		}
    	}

    	private void GameDidPause()
    	{
    		_gamePaused = true;
    	}

    	private void GameDidResume()
    	{
    		_gamePaused = false;
    	}

    	public void SetProgress(float value)
    	{
    		progressbar.anchorMax = new Vector2(value, 1f);
    		progressbar.sizeDelta = Vector2.zero;
    	}

    	public void Begin(BeatmapObjectManager beatMapObjectManager, SubscriptionEvent subscriptionEvent, Action onComplete)
    	{
    		_active = true;
    		timeElapsedAfterBurst = 0f;
    		targetAlpha = 1f;
    		_beatMapObjectManager = beatMapObjectManager;
    		_currentSubscriptionEvent = subscriptionEvent;
    		_onComplete = onComplete;
    		if (controllerEvent == null)
    		{
    			controllerEvent = new SubscriberControllerEvent();
    			controllerEvent.subscriptionElapsed = 0f;
    			controllerEvent.subscriptionDuration = GetSubscriptionDuration();
    			Plugin.Log($"Creating subscription for: {controllerEvent.subscriptionDuration} seconds");
    		}
    		else
    		{
    			Plugin.Log($"Continuing subscription for: {controllerEvent.subscriptionDuration - controllerEvent.subscriptionElapsed} seconds");
    		}
    		if (_currentSubscriptionEvent.Recipient == null)
    		{
    			message.text = "";
    			userName.text = _currentSubscriptionEvent.Buyer.name;
    		}
    		else
    		{
    			message.text = $"Gifted by {_currentSubscriptionEvent.Buyer.name}.";
    			userName.text = _currentSubscriptionEvent.Recipient.name;
    		}
    		userName.fontSize = 44f;
    		_beatMapObjectManager.noteDidStartJumpEvent += HandleNoteDidStartJumpEvent;
    		_beatMapObjectManager.noteWasCutEvent += NoteWasCutEvent;
    		_beatMapObjectManager.noteWasMissedEvent += NoteWasMissedEvent;
    		endParticles.gameObject.SetActive(value: false);
    		InitPauseManager();
    	}

    	public void End()
    	{
    		_active = false;
    		controllerEvent = null;
    		_beatMapObjectManager.noteWasCutEvent -= NoteWasCutEvent;
    		_beatMapObjectManager.noteWasMissedEvent -= NoteWasMissedEvent;
    		_beatMapObjectManager.noteDidStartJumpEvent -= HandleNoteDidStartJumpEvent;
    		targetProgress = 0f;
    		targetAlpha = 0f;
    		timeElapsedAfterBurst = 0f;
    		endParticles.gameObject.SetActive(value: true);
    		DestroyPauseManager();
    		if (_onComplete != null)
    		{
    			_onComplete();
    		}
    	}

        private int GetSubscriptionDuration()
        {
            if (_currentSubscriptionEvent == null)
                return Settings.SubsTier1Duration.value;

            switch (_currentSubscriptionEvent.SubscriptionTierIndex)
            {
                case 0:
                    return Settings.SubsTier1Duration.value;
                case 1:
                    return Settings.SubsTier2Duration.value;
                case 2:
                    return Settings.SubsTier3Duration.value;
                case 3:
                    return Settings.SubsTier4Duration.value;
                default:
                    return Settings.SubsTier1Duration.value;
            }
        }


        private void NoteWasCutEvent(NoteController noteController, in NoteCutInfo noteCutInfo)
    	{
    		if (controllerEvent != null)
    		{
    			targetProgress = Mathf.Clamp01(controllerEvent.subscriptionElapsed / controllerEvent.subscriptionDuration);
    		}
    	}

    	private void NoteWasMissedEvent(NoteController noteController)
    	{
    		if (controllerEvent != null)
    		{
    			targetProgress = Mathf.Clamp01(controllerEvent.subscriptionElapsed / controllerEvent.subscriptionDuration);
    		}
    	}

    	private void HandleNoteDidStartJumpEvent(NoteController noteController)
    	{
    		if (controllerEvent == null || !(controllerEvent.subscriptionDuration - controllerEvent.subscriptionElapsed < 2f))
    		{
    			SubHyperCube subHyperCube = _subHyperCubePool.Next();
    			subHyperCube.Username = _currentSubscriptionEvent.Buyer.name;
    			if (_currentSubscriptionEvent.Recipient != null)
    			{
    				subHyperCube.RecipientName = _currentSubscriptionEvent.Recipient.name;
    			}
    			else
    			{
    				subHyperCube.RecipientName = "";
    			}
    			subHyperCube.Message = _currentSubscriptionEvent.Message;
    			subHyperCube.Tier = _currentSubscriptionEvent.SubscriptionTier;
    			subHyperCube.TierIndex = _currentSubscriptionEvent.SubscriptionTierIndex;
    			subHyperCube.Months = _currentSubscriptionEvent.ConsecutiveMonths;
    			subHyperCube.AddSongEvents(_beatMapObjectManager);
    			subHyperCube.onDisabled = (Action<SubHyperCube>)Delegate.Combine(subHyperCube.onDisabled, new Action<SubHyperCube>(OnHyperCubeDisable));
    			subHyperCube.Spawn(noteController);
    			_spawnedCubes++;
    		}
    	}

    	private void OnHyperCubeDisable(SubHyperCube hyperCube)
    	{
    		hyperCube.onDisabled = (Action<SubHyperCube>)Delegate.Remove(hyperCube.onDisabled, new Action<SubHyperCube>(OnHyperCubeDisable));
    		_spawnedCubes--;
    	}
    }
}
