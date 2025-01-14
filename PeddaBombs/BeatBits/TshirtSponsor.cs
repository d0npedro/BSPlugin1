using TMPro;
using UnityEngine;

namespace BeatBits
{
    public class TshirtSponsor : MonoBehaviour
    {
    	public TextMeshProUGUI sponsorName;

    	private bool _initialized;

    	public void Init()
    	{
    		base.transform.SetParent(null);
    		sponsorName.color = Settings.TshirtSponsorNameColor.value;
    		_initialized = true;
    	}

    	private void LateUpdate()
    	{
    		if (_initialized && Camera.main != null)
    		{
    			Transform transform = Camera.main.transform;
    			Vector3 forward = transform.forward;
    			forward.y = 0f;
    			forward = forward.normalized;
    			base.transform.position = transform.position - forward * Settings.TshirtSponsorNameDistance.value + Vector3.up * Settings.TshirtSponsorNameVerticalOffset.value;
    			base.transform.rotation = Quaternion.LookRotation(forward);
    		}
    	}
    }
}
