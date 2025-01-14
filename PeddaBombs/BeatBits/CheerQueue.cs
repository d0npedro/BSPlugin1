using System.Collections.Generic;
using GameChanger;
using GameChanger.EventData;

namespace BeatBits
{
    public static class CheerQueue
    {
    	private static readonly Queue<CheerEvent> PendingEvents = new Queue<CheerEvent>();

    	public static bool IsPending => PendingEvents.Count != 0;

    	public static int TotalBitsReceived { get; private set; }

    	public static void Setup()
    	{
    		Events.TwitchCheerReceived += OnTwitchCheerReceived;
    	}

    	private static void OnTwitchCheerReceived(TwitchCheerReceivedData eventData)
    	{
    		Plugin.Log($"OnTwitchCheerReceived: {eventData}");
    		PendingEvents.Enqueue(new CheerEvent
    		{
    			Viewer = eventData.viewer,
    			Bits = eventData.amount,
    			Message = eventData.message
    		});
    	}

    	public static bool ObserveEvent(out CheerEvent observedEvent)
    	{
    		observedEvent = null;
    		if (!IsPending)
    		{
    			return false;
    		}
    		observedEvent = PendingEvents.Peek();
    		return true;
    	}

    	public static bool ConsumeEvent(out CheerEvent processedEvent)
    	{
    		processedEvent = null;
    		if (!IsPending)
    		{
    			return false;
    		}
    		processedEvent = PendingEvents.Dequeue();
    		TotalBitsReceived += processedEvent.Bits;
    		return true;
    	}
    }
}
