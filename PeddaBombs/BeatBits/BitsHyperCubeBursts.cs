using TMPro;
using UnityEngine;

namespace BeatBits
{
    public class BitsHyperCubeBursts : MonoBehaviour
    {
    	[Header("Bits Visual")]
    	public ParticleSystem particles1bit;

    	public ParticleSystem particles1bitSpecial;

    	public ParticleSystem particles100bit;

    	public ParticleSystem particles100bitSpecial;

    	public ParticleSystem particles1000bit;

    	public ParticleSystem particles1000bitSpecial;

    	public ParticleSystem particles5000bit;

    	public ParticleSystem particles5000bitSpecial;

    	public ParticleSystem particles10000bit;

    	public ParticleSystem particles10000bitSpecial;

    	public BitsHyperCube hyperCube;

    	public ParticleSystem particleSystem;

    	public TextMeshPro userName;

    	public AnimationCurve userNameScale;

    	public AnimationCurve userNameAlpha;

    	public float duration;

    	public int bits;

    	public int tierIndex;

    	private float elapsed;

    	private float usernameMaxSize = 1f;

    	public void Init()
    	{
    		elapsed = 0f;
    		BitsAmountCustomization();
    		UpdateGraphics(0f);
    	}

    	private void BitsAmountCustomization()
    	{
    		Plugin.Log($"HyperCubeBurst, tierIndex: {tierIndex}");
    		switch (tierIndex)
    		{
    		case 0:
    			ParticleColor(Settings.bitsColor1.value);
    			ParticleSpeed(2f);
    			particles1bit.Emit(1);
    			if (particles1bitSpecial != null)
    			{
    				particles1bitSpecial.Emit(1000);
    			}
    			particleSystem.Emit(Settings.BitsTier0BurstParticles.value);
    			usernameMaxSize = Settings.BitsTier0NameSize.value;
    			duration = Settings.BitsTier0Duration.value;
    			break;
    		case 1:
    			ParticleColor(Settings.bitsColor100.value);
    			ParticleSpeed(5f);
    			particles100bit.Emit(1);
    			if (particles100bitSpecial != null)
    			{
    				particles100bitSpecial.Emit(1000);
    			}
    			particleSystem.Emit(Settings.BitsTier1BurstParticles.value);
    			usernameMaxSize = Settings.BitsTier1NameSize.value;
    			duration = Settings.BitsTier1Duration.value;
    			break;
    		case 2:
    			ParticleColor(Settings.bitsColor1000.value);
    			ParticleSpeed(10f);
    			particles1000bit.Emit(1);
    			if (particles1000bitSpecial != null)
    			{
    				particles1000bitSpecial.Emit(1000);
    			}
    			particleSystem.Emit(Settings.BitsTier2BurstParticles.value);
    			usernameMaxSize = Settings.BitsTier2NameSize.value;
    			duration = Settings.BitsTier2Duration.value;
    			break;
    		case 3:
    			ParticleColor(Settings.bitsColor5000.value);
    			ParticleSpeed(15f);
    			particles5000bit.Emit(1);
    			if (particles5000bitSpecial != null)
    			{
    				particles5000bitSpecial.Emit(1000);
    			}
    			particleSystem.Emit(Settings.BitsTier3BurstParticles.value);
    			usernameMaxSize = Settings.BitsTier3NameSize.value;
    			duration = Settings.BitsTier3Duration.value;
    			break;
    		default:
    			ParticleColor(Settings.bitsColor10000.value);
    			ParticleSpeed(20f);
    			particles10000bit.Emit(1);
    			if (particles10000bitSpecial != null)
    			{
    				particles10000bitSpecial.gameObject.SetActive(value: true);
    			}
    			particleSystem.Emit(Settings.BitsTier4BurstParticles.value);
    			usernameMaxSize = Settings.BitsTier4NameSize.value;
    			duration = Settings.BitsTier4Duration.value;
    			break;
    		}
    	}

    	private void ParticleColor(Color color)
    	{
    		ParticleSystem.MainModule main = particleSystem.main;
    		main.startColor = color;
    		userName.color = color;
    	}

    	private void ParticleSpeed(float speed)
    	{
    		ParticleSystem.MainModule main = particleSystem.main;
    		main.startSpeed = speed;
    	}

    	private void Update()
    	{
    		if (elapsed < duration)
    		{
    			elapsed = Mathf.Clamp(elapsed + Time.deltaTime, 0f, duration);
    			float progress = elapsed / duration;
    			UpdateGraphics(progress);
    		}
    		else
    		{
    			elapsed = 0f;
    			UpdateGraphics(0f);
    			base.transform.SetParent(hyperCube.transform);
    			base.gameObject.SetActive(value: false);
    		}
    	}

    	private void OnDisable()
    	{
    		if (particles10000bitSpecial != null)
    		{
    			particles10000bitSpecial.gameObject.SetActive(value: false);
    		}
    	}

    	private void UserNameAlpha(float alpha)
    	{
    		Color color = userName.color;
    		color.a = alpha;
    		userName.color = color;
    	}

    	private void UpdateGraphics(float progress)
    	{
    		UserNameAlpha(userNameAlpha.Evaluate(progress));
    		userName.transform.localScale = Vector3.one * userNameScale.Evaluate(progress) * usernameMaxSize;
    		userName.transform.localPosition = Vector3.forward * progress * 5f;
    	}
    }
}
