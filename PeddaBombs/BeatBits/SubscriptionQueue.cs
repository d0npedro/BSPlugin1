using System.Collections.Generic;
using GameChanger;
using GameChanger.EventData;

namespace BeatBits
{
    public static class SubscriptionQueue
    {
    	private static readonly Queue<SubscriptionEvent> PendingEvents = new Queue<SubscriptionEvent>();

    	public static bool IsPending => PendingEvents.Count != 0;

    	public static int TotalNewSubscriptions { get; private set; }

    	public static void Setup()
    	{
    		Events.TwitchSubscription += OnSubscriptionReceived;
    	}

    	private static void OnSubscriptionReceived(TwitchSubscriptionReceivedData eventData)
    	{
    		Plugin.Log($"OnSubscriptionReceived: {eventData}");
    		PendingEvents.Enqueue(new SubscriptionEvent
    		{
    			Context = eventData.context,
    			Buyer = eventData.buyer,
    			Recipient = eventData.recipient,
    			Message = eventData.message,
    			ConsecutiveMonths = eventData.consecutiveMonths,
    			SubscriptionTier = eventData.subscriptionTier,
    			SubscriptionTierName = eventData.subscriptionTierName
    		});
    	}

    	public static bool ObserveEvent(out SubscriptionEvent observedEvent)
    	{
    		observedEvent = null;
    		if (!IsPending)
    		{
    			return false;
    		}
    		observedEvent = PendingEvents.Peek();
    		return true;
    	}

    	public static bool ConsumeEvent(out SubscriptionEvent processedEvent)
    	{
    		processedEvent = null;
    		if (!IsPending)
    		{
    			return false;
    		}
    		processedEvent = PendingEvents.Dequeue();
    		TotalNewSubscriptions++;
    		return true;
    	}

        public static int GetTierIndex(string tier)
        {
            switch (tier)
            {
                case "prime":
                    return 0;
                case "tier1":
                    return 1;
                case "tier2":
                    return 2;
                case "tier3":
                    return 3;
                default:
                    return 0;
            }
        }
    }
}

