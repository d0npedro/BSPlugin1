using System;
using TMPro;
using UnityEngine;

namespace BeatBits
{
    public class NewFollowerController : MonoBehaviour
    {
    	private static int MAX_POOL_OBJECTS = 15;

    	public GameObject newFollowerPrefab;

    	public Transform startPosition;

    	public Transform endPosition;

    	public AnimationCurve textFadeout;

    	private Action _onComplete;

    	private bool _active;

    	private bool _initialized;

    	private ObjectPool<TextMeshProUGUI> newFollowerPool;

    	private NewFollowerEvent _currentFollowerEvent;

    	public void Init()
    	{
    		TextMeshProUGUI[] array = new TextMeshProUGUI[MAX_POOL_OBJECTS];
    		for (int i = 0; i < array.Length; i++)
    		{
    			array[i] = UnityEngine.Object.Instantiate(newFollowerPrefab).GetComponent<TextMeshProUGUI>();
    			array[i].gameObject.transform.SetParent(newFollowerPrefab.gameObject.transform.parent);
    			array[i].gameObject.transform.localPosition = Vector3.zero;
    			array[i].gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    			array[i].gameObject.transform.localScale = Vector3.one;
    			array[i].gameObject.SetActive(value: false);
    		}
    		newFollowerPool = new ObjectPool<TextMeshProUGUI>(array);
    		_initialized = true;
    	}

    	private void Update()
    	{
    		TextMeshProUGUI[] pool = newFollowerPool.pool;
    		Vector3 normalized = (endPosition.localPosition - startPosition.localPosition).normalized;
    		float num = Mathf.Min(endPosition.localPosition.y, startPosition.localPosition.y);
    		float num2 = Mathf.Max(endPosition.localPosition.y, startPosition.localPosition.y);
    		for (int i = 0; i < pool.Length; i++)
    		{
    			if (pool[i].isActiveAndEnabled)
    			{
    				Vector3 localPosition = pool[i].transform.localPosition;
    				float time = Mathf.InverseLerp(num, num2, localPosition.y);
    				pool[i].alpha = textFadeout.Evaluate(time);
    				if (localPosition.y < num || localPosition.y > num2)
    				{
    					pool[i].gameObject.SetActive(value: false);
    				}
    				else
    				{
    					pool[i].transform.localPosition += normalized * Time.deltaTime * Settings.NewFollowerScrollSpeed.value;
    				}
    			}
    		}
    	}

    	public void Begin(NewFollowerEvent followerEvent, Action onComplete)
    	{
    		_active = true;
    		_currentFollowerEvent = followerEvent;
    		_onComplete = onComplete;
    		TextMeshProUGUI textMeshProUGUI = newFollowerPool.Next();
    		textMeshProUGUI.text = followerEvent.Viewer.name;
    		_ = (endPosition.localPosition - startPosition.localPosition).normalized;
    		textMeshProUGUI.gameObject.transform.localPosition = startPosition.localPosition;
    		RectTransform rectTransform = textMeshProUGUI.gameObject.transform as RectTransform;
    		rectTransform.sizeDelta = new Vector2(0f, rectTransform.sizeDelta.y);
    		textMeshProUGUI.gameObject.SetActive(value: true);
    	}

    	public void End()
    	{
    		_active = false;
    		if (_onComplete != null)
    		{
    			_onComplete();
    		}
    	}
    }
}
