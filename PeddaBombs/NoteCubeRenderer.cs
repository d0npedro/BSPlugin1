using System;
using UnityEngine;

public class NoteCubeRenderer : MonoBehaviour
{
	public enum RENDERER_TYPE
	{
		NONE,
		OUTLINE,
		NOTE,
		BOMB
	}

	public RENDERER_TYPE _type;

	public Transform _noteOutlineTransform;

	public Renderer _noteOutlineRenderer;

	public ParticleSystem _noteParticleSystem;

	public Transform _noteTransform;

	public Renderer _noteRenderer;

	public Transform _bombTransform;

	public Renderer _bombRenderer;

	public ParticleSystem _bombParticles;

	public SpriteRenderer _arrowGlow;

	public SpriteRenderer _circleGlow;

	[NonSerialized]
	private Material noteMaterial;

	[NonSerialized]
	private Material bombMaterial;

	public void SetRendererType(RENDERER_TYPE type)
	{
		_type = type;
		_noteTransform.gameObject.SetActive(_type == RENDERER_TYPE.NOTE);
		_bombTransform.gameObject.SetActive(_type == RENDERER_TYPE.BOMB);
		_noteOutlineTransform.gameObject.SetActive(_type == RENDERER_TYPE.OUTLINE);
	}

	public void SetArrowColor(Color color)
	{
		_arrowGlow.color = new Color(color.r, color.g, color.b, _arrowGlow.color.a);
	}

	public void SetCircleColor(Color color)
	{
		_circleGlow.color = new Color(color.r, color.g, color.b, _circleGlow.color.a);
	}

	public void SetNoteColor(Color color)
	{
		if (noteMaterial == null && _noteRenderer.sharedMaterial != null)
		{
			noteMaterial = UnityEngine.Object.Instantiate(_noteRenderer.material);
			_noteRenderer.sharedMaterial = noteMaterial;
		}
		if (noteMaterial != null)
		{
			noteMaterial.SetColor("_Color", color);
			noteMaterial.SetFloat("_FinalColorMul", 12f);
		}
	}

	public void SetParticleColor(Color color)
	{
		if (!(_noteParticleSystem == null))
		{
			ParticleSystem.MainModule main = _noteParticleSystem.main;
			main.startColor = color;
			_noteOutlineRenderer.material.color = color;
		}
	}

	public void SetParticleEmission(int count)
	{
		if (!(_noteParticleSystem == null))
		{
			ParticleSystem.EmissionModule emission = _noteParticleSystem.emission;
			emission.rateOverTime = new ParticleSystem.MinMaxCurve(count, count);
		}
	}

	public void SetBombColor(Color color)
	{
		if (bombMaterial == null && _bombRenderer.sharedMaterial != null)
		{
			bombMaterial = UnityEngine.Object.Instantiate(_bombRenderer.sharedMaterial);
			_bombRenderer.sharedMaterial = bombMaterial;
		}
		if (bombMaterial != null)
		{
			bombMaterial.SetColor("_Color", new Color(color.r, color.g, color.b, 1f));
		}
		ParticleSystem.MainModule main = _bombParticles.main;
		main.startColor = color;
	}

	public void SetColor(Color color)
	{
		SetArrowColor(color);
		SetCircleColor(color);
		SetNoteColor(color);
		SetBombColor(color);
	}
}
