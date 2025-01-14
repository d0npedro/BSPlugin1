using System;
using JetBrains.Annotations;

namespace GameChanger.EventData
{
    [Serializable]
    [UsedImplicitly]
    internal class TwitchCheerReceivedData : IEventData
    {
    	public Viewer viewer;

    	public int amount;

    	[CanBeNull]
    	public string message;

    	public string badge;

    	public override string ToString()
    	{
    		return $"viewer: {viewer}, amount: {amount}, message: {message}, badge: {badge}";
    	}
    }
}
