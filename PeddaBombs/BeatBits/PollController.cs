using System;
using TMPro;
using UnityEngine;

namespace BeatBits
{
    [ExecuteInEditMode]
    public class PollController : MonoBehaviour
    {
    	public RectTransform pollVoteAImage;

    	public TextMeshProUGUI pollVoteAName;

    	public RectTransform pollVoteBImage;

    	public TextMeshProUGUI pollVoteBName;

    	public TextMeshProUGUI pollName;

    	public TextMeshProUGUI pollTime;

    	private bool _initialized;

    	[Range(0f, 1f)]
    	public float progress;

    	public float time;

    	public void Init()
    	{
    		_initialized = true;
    	}

    	private void UpdatePolls(float progress)
    	{
    		pollVoteAImage.anchorMin = new Vector2(0f, 0f);
    		pollVoteAImage.anchorMax = new Vector2(progress, 1f);
    		pollVoteAImage.sizeDelta = Vector2.zero;
    		pollVoteBImage.anchorMin = new Vector2(progress, 0f);
    		pollVoteBImage.anchorMax = new Vector2(1f, 1f);
    		pollVoteBImage.sizeDelta = Vector2.zero;
    		pollTime.text = TimeSpan.FromSeconds(time).ToString("hh\\:mm\\:ss");
    	}

    	private void Update()
    	{
    		UpdatePolls(progress);
    	}
    }
}
