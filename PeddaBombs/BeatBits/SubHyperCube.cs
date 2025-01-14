using System;
using UnityEngine;

namespace BeatBits
{
    public class SubHyperCube : HyperCube
    {
    	public Action<SubHyperCube> onDisabled;

    	public SubHyperCubeBursts hyperCubeBurst;

    	public string Username;

    	public string RecipientName;

    	public int Months;

    	public string Tier;

    	public int TierIndex;

    	public string Message;

    	private void Awake()
    	{
    		hyperCubeBurst.gameObject.SetActive(value: false);
    		hyperCubeBurst.transform.SetParent(null);
    	}

    	private void OnDisable()
    	{
    		if (onDisabled != null)
    		{
    			onDisabled(this);
    		}
    	}

    	private void OnDestroy()
    	{
    		if (hyperCubeBurst != null)
    		{
    			UnityEngine.Object.Destroy(hyperCubeBurst);
    		}
    	}

    	protected override void HandleNoteDidStartJumpEvent(NoteController noteController)
    	{
    		if (base.ghostNote)
    		{
    			noteCubeRenderer.SetRendererType(NoteCubeRenderer.RENDERER_TYPE.NONE);
    		}
    		else
    		{
    			noteCubeRenderer.SetRendererType(NoteCubeRenderer.RENDERER_TYPE.OUTLINE);
    		}
    	}

    	protected override void HandleNoteWasMissedEvent(NoteController noteController)
    	{
    		if (!(base.noteController != noteController))
    		{
    			base.noteController = null;
    			base.transform.SetParent(null);
    			base.gameObject.SetActive(value: false);
    		}
    	}

    	protected override void HandleNoteWasCutEvent(NoteController noteController, in NoteCutInfo noteCutInfo)
    	{
    		if (!(base.noteController != noteController))
    		{
    			base.noteController = null;
    			hyperCubeBurst.monts = Months;
    			hyperCubeBurst.tier = TierIndex;
    			hyperCubeBurst.transform.SetParent(null);
    			hyperCubeBurst.transform.rotation = Quaternion.identity;
    			hyperCubeBurst.transform.localScale = Vector3.one;
    			hyperCubeBurst.transform.position = base.transform.position;
    			hyperCubeBurst.gameObject.SetActive(value: true);
    			int count = Mathf.RoundToInt(SubscriberController.timeElapsedAfterBurst * (float)Settings.SubscriberParticlesPerSecond.value);
    			SubscriberController.timeElapsedAfterBurst = 0f;
    			hyperCubeBurst.Emit(count);
    			RemoveSongEvents();
    			base.transform.SetParent(null);
    			base.gameObject.SetActive(value: false);
    		}
    	}

    	public bool Spawn(NoteController noteController)
    	{
    		if (base.noteController != null)
    		{
    			return false;
    		}
    		if (noteController.GetComponentInChildren<HyperCube>() != null)
    		{
    			return false;
    		}
    		base.noteController = noteController;
    		base.transform.SetParent(noteController.noteTransform);
    		base.transform.localPosition = Vector3.zero;
    		base.transform.localRotation = Quaternion.identity;
    		base.transform.localScale = Vector3.one;
    		hyperCubeBurst.gameObject.SetActive(value: false);
    		base.gameObject.SetActive(value: true);
    		if (base.ghostNote)
    		{
    			noteCubeRenderer.SetRendererType(NoteCubeRenderer.RENDERER_TYPE.NONE);
    		}
    		else
    		{
    			noteCubeRenderer.SetRendererType(NoteCubeRenderer.RENDERER_TYPE.OUTLINE);
    		}
    		return true;
    	}
    }
}
