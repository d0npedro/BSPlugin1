using UnityEngine;

namespace BeatBits
{
    public class SubHyperCubeBursts : MonoBehaviour
    {
    	public SubHyperCube hyperCube;

    	public ParticleSystem particleSystem;

    	public float duration;

    	public int monts;

    	public int tier;

    	private float elapsed;

    	private void OnEnable()
    	{
    		elapsed = 0f;
    	}

    	private void Update()
    	{
    		if (elapsed < duration)
    		{
    			elapsed = Mathf.Clamp(elapsed + Time.deltaTime, 0f, duration);
    			_ = elapsed / duration;
    		}
    		else
    		{
    			elapsed = 0f;
    			base.transform.SetParent(hyperCube.transform);
    			base.gameObject.SetActive(value: false);
    		}
    	}

    	public void Emit(int count)
    	{
    		particleSystem.Emit(count);
    	}
    }
}
