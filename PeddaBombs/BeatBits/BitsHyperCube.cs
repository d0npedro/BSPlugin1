using System;
using UnityEngine;

namespace BeatBits
{
    public class BitsHyperCube : HyperCube
    {
    	public Action<BitsHyperCube> onSpawned;

    	public Action<BitsHyperCube> onDisabled;

    	public BitsHyperCubeBursts hyperCubeBurst;

    	public string Username;

    	public int Bits;

    	public int TierIndex;

    	public string Message;

    	private NoteRendererState noteRendererState;

    	public bool sabotage;

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
    		if (!(base.noteController != null) && !(noteController.GetComponentInChildren<HyperCube>() != null))
    		{
    			base.noteController = noteController;
    			CaptureNoteControllerRenderer();
    			base.transform.SetParent(noteController.noteTransform);
    			base.transform.localPosition = Vector3.zero;
    			base.transform.localRotation = Quaternion.identity;
    			base.transform.localScale = Vector3.one;
    			hyperCubeBurst.gameObject.SetActive(value: false);
    			base.gameObject.SetActive(value: true);
    			OnSpawn();
    		}
    	}

    	protected override void HandleNoteWasMissedEvent(NoteController noteController)
    	{
    		if (!(base.noteController != noteController))
    		{
    			RecoverNoteControllerRenderer();
    			base.noteController = null;
    			base.transform.SetParent(null);
    			base.gameObject.SetActive(value: false);
    		}
    	}

    	protected override void HandleNoteWasCutEvent(NoteController noteController, in NoteCutInfo noteCutInfo)
    	{
    		if (!(base.noteController != noteController))
    		{
    			RecoverNoteControllerRenderer();
    			base.noteController = null;
    			hyperCubeBurst.bits = Bits;
    			hyperCubeBurst.tierIndex = TierIndex;
    			hyperCubeBurst.userName.text = Username;
    			hyperCubeBurst.transform.SetParent(null);
    			hyperCubeBurst.transform.rotation = Quaternion.identity;
    			hyperCubeBurst.transform.localScale = Vector3.one;
    			hyperCubeBurst.transform.position = base.transform.position;
    			hyperCubeBurst.gameObject.SetActive(value: true);
    			RemoveSongEvents();
    			base.transform.SetParent(null);
    			base.gameObject.SetActive(value: false);
    			hyperCubeBurst.Init();
    		}
    	}

    	public void CaptureNoteControllerRenderer()
    	{
    		noteRendererState = default(NoteRendererState);
    		if (noteController == null)
    		{
    			return;
    		}
    		ColorNoteVisuals component = noteController.GetComponent<ColorNoteVisuals>();
    		if (component != null)
    		{
    			MeshRenderer fieldValueObject = GetFieldValueObject<MeshRenderer>(component, "_arrowMeshRenderer");
    			if (fieldValueObject != null)
    			{
    				noteRendererState._arrowMeshRenderer = fieldValueObject.enabled;
    			}
    			SpriteRenderer fieldValueObject2 = GetFieldValueObject<SpriteRenderer>(component, "_arrowGlowSpriteRenderer");
    			if (fieldValueObject2 != null)
    			{
    				noteRendererState._arrowGlowSpriteRenderer = fieldValueObject2.enabled;
    			}
    			SpriteRenderer fieldValueObject3 = GetFieldValueObject<SpriteRenderer>(component, "_circleGlowSpriteRenderer");
    			if (fieldValueObject3 != null)
    			{
    				noteRendererState._circleGlowSpriteRenderer = fieldValueObject3.enabled;
    			}
    		}
    		MaterialPropertyBlockController component2 = noteController.noteTransform.GetComponent<MaterialPropertyBlockController>();
    		if (!(component2 != null))
    		{
    			return;
    		}
    		Renderer[] fieldValueObject4 = GetFieldValueObject<Renderer[]>(component2, "_renderers");
    		if (fieldValueObject4 != null)
    		{
    			noteRendererState._renderers = new bool[fieldValueObject4.Length];
    			for (int i = 0; i < fieldValueObject4.Length; i++)
    			{
    				if (!(fieldValueObject4[i] == null))
    				{
    					noteRendererState._renderers[i] = fieldValueObject4[i].enabled;
    				}
    			}
    		}
    		else
    		{
    			noteRendererState._renderers = new bool[0];
    			Plugin.Log("renderers of MaterialPropertyBlock are null");
    		}
    	}

    	public void ShowNoteControllerRenderer(bool visible)
    	{
    		ColorNoteVisuals component = noteController.GetComponent<ColorNoteVisuals>();
    		if (component != null)
    		{
    			MeshRenderer fieldValueObject = GetFieldValueObject<MeshRenderer>(component, "_arrowMeshRenderer");
    			if (fieldValueObject != null)
    			{
    				fieldValueObject.enabled = visible;
    			}
    			SpriteRenderer fieldValueObject2 = GetFieldValueObject<SpriteRenderer>(component, "_arrowGlowSpriteRenderer");
    			if (fieldValueObject2 != null)
    			{
    				fieldValueObject2.enabled = visible;
    			}
    			SpriteRenderer fieldValueObject3 = GetFieldValueObject<SpriteRenderer>(component, "_circleGlowSpriteRenderer");
    			if (fieldValueObject3 != null)
    			{
    				fieldValueObject3.enabled = visible;
    			}
    		}
    		MaterialPropertyBlockController component2 = noteController.noteTransform.GetComponent<MaterialPropertyBlockController>();
    		if (!(component2 != null))
    		{
    			return;
    		}
    		Renderer[] fieldValueObject4 = GetFieldValueObject<Renderer[]>(component2, "_renderers");
    		if (fieldValueObject4 != null)
    		{
    			for (int i = 0; i < fieldValueObject4.Length; i++)
    			{
    				if (!(fieldValueObject4[i] == null))
    				{
    					fieldValueObject4[i].enabled = visible;
    				}
    			}
    		}
    		else
    		{
    			Plugin.Log("renderers of MaterialPropertyBlock are null");
    		}
    	}

    	public void RecoverNoteControllerRenderer()
    	{
    		if (noteController == null)
    		{
    			return;
    		}
    		ColorNoteVisuals component = noteController.GetComponent<ColorNoteVisuals>();
    		if (component != null)
    		{
    			MeshRenderer fieldValueObject = GetFieldValueObject<MeshRenderer>(component, "_arrowMeshRenderer");
    			if (fieldValueObject != null)
    			{
    				fieldValueObject.enabled = noteRendererState._arrowMeshRenderer;
    			}
    			SpriteRenderer fieldValueObject2 = GetFieldValueObject<SpriteRenderer>(component, "_arrowGlowSpriteRenderer");
    			if (fieldValueObject2 != null)
    			{
    				fieldValueObject2.enabled = noteRendererState._arrowGlowSpriteRenderer;
    			}
    			SpriteRenderer fieldValueObject3 = GetFieldValueObject<SpriteRenderer>(component, "_circleGlowSpriteRenderer");
    			if (fieldValueObject3 != null)
    			{
    				fieldValueObject3.enabled = noteRendererState._circleGlowSpriteRenderer;
    			}
    		}
    		MaterialPropertyBlockController component2 = noteController.noteTransform.GetComponent<MaterialPropertyBlockController>();
    		if (!(component2 != null))
    		{
    			return;
    		}
    		Renderer[] fieldValueObject4 = GetFieldValueObject<Renderer[]>(component2, "_renderers");
    		if (fieldValueObject4 != null)
    		{
    			for (int i = 0; i < fieldValueObject4.Length; i++)
    			{
    				if (!(fieldValueObject4[i] == null))
    				{
    					fieldValueObject4[i].enabled = noteRendererState._renderers[i];
    				}
    			}
    		}
    		else
    		{
    			Plugin.Log("renderers of MaterialPropertyBlock are null");
    		}
    	}

    	private void OnSpawn()
    	{
    		if (!base.ghostNote)
    		{
    			if (sabotage)
    			{
    				ShowNoteControllerRenderer(visible: false);
    			}
    		}
    		else
    		{
    			noteCubeRenderer.SetRendererType(NoteCubeRenderer.RENDERER_TYPE.NONE);
    		}
    		Plugin.Log($"HyperCube Spawn, tierIndex: {TierIndex}");
    		switch (TierIndex)
    		{
    		case 0:
    			noteCubeRenderer.SetParticleEmission(10);
    			noteCubeRenderer.SetParticleColor(Settings.bitsColor1.value);
    			break;
    		case 1:
    			noteCubeRenderer.SetParticleEmission(50);
    			noteCubeRenderer.SetParticleColor(Settings.bitsColor100.value);
    			break;
    		case 2:
    			noteCubeRenderer.SetParticleEmission(100);
    			noteCubeRenderer.SetParticleColor(Settings.bitsColor1000.value);
    			break;
    		case 3:
    			noteCubeRenderer.SetParticleEmission(500);
    			noteCubeRenderer.SetParticleColor(Settings.bitsColor5000.value);
    			break;
    		default:
    			noteCubeRenderer.SetParticleEmission(1000);
    			noteCubeRenderer.SetParticleColor(Settings.bitsColor10000.value);
    			break;
    		}
    		if (onSpawned != null)
    		{
    			onSpawned(this);
    		}
    	}
    }
}
