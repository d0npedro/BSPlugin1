using System;
using UnityEngine;

namespace BeatBits
{
    [Serializable]
    public class BitsBurstVisual
    {
    	public ParticleSystem bits;

    	public ParticleSystem mesh;

    	public void OnEnable()
    	{
    		if (mesh != null)
    		{
    			mesh.gameObject.SetActive(value: false);
    		}
    	}

    	public void Play()
    	{
    		if (bits != null)
    		{
    			bits.Emit(1);
    		}
    		if (mesh != null)
    		{
    			mesh.gameObject.SetActive(value: true);
    			mesh.Play();
    		}
    	}
    }
}
