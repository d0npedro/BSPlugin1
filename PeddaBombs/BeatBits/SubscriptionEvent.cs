using GameChanger.EventData;

namespace BeatBits
{
    public class SubscriptionEvent
    {
    	public string Context;

    	public Viewer Buyer;

    	public Viewer Recipient;

    	public string Message;

    	public int ConsecutiveMonths;

    	public string SubscriptionTier;

    	public string SubscriptionTierName;

    	public int SubscriptionTierIndex
    	{
    		get
    		{
    			if (string.IsNullOrEmpty(SubscriptionTier))
    			{
    				return 0;
    			}
    			return SubscriptionQueue.GetTierIndex(SubscriptionTier);
    		}
    	}
    }
}
