using System;
using JetBrains.Annotations;

namespace GameChanger.EventData
{
    [Serializable]
    [UsedImplicitly]
    internal class TwitchSubscriptionReceivedData : IEventData
    {
    	public string context;

    	public Viewer buyer;

    	public Viewer recipient;

    	[CanBeNull]
    	public string message;

    	public int consecutiveMonths;

    	public string subscriptionTier;

    	public string subscriptionTierName;

    	public override string ToString()
    	{
    		return $"context: {context}, buyer: {buyer}, recipient: {recipient}, message: {message}, consecutiveMonths: {consecutiveMonths}, subscriptionTier: {subscriptionTier}, subscriptionTierName: {subscriptionTierName}";
    	}
    }
}
