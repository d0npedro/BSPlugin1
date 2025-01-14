using GameChanger.EventData;

namespace BeatBits
{
    public class CheerEvent
    {
    	public Viewer Viewer;

    	public int Bits;

    	public string Message;

    	public int TierIndex
    	{
    		get
    		{
    			if (Bits >= Settings.BitsTier4Start.value)
    			{
    				return 4;
    			}
    			if (Bits >= Settings.BitsTier3Start.value)
    			{
    				return 3;
    			}
    			if (Bits >= Settings.BitsTier2Start.value)
    			{
    				return 2;
    			}
    			if (Bits >= Settings.BitsTier1Start.value)
    			{
    				return 1;
    			}
    			return 0;
    		}
    	}

    	public override string ToString()
    	{
    		return $"Viewer: {Viewer}, Bits: {Bits}, Message: {Message}";
    	}
    }
}
