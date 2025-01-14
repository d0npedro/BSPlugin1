using System.Collections.Generic;
using GameChanger;
using GameChanger.EventData;

namespace BeatBits
{
    public static class NewFollowerQueue
    {
    	private static readonly Queue<NewFollowerEvent> PendingEvents = new Queue<NewFollowerEvent>();

    	public static bool IsPending => PendingEvents.Count != 0;

    	public static int TotalNewFollowers { get; private set; }

    	public static void Setup()
    	{
    		Events.NewFollower += OnNewFollowerReceived;
    	}

    	private static void OnNewFollowerReceived(NewFollowerReceivedData eventData)
    	{
    		Plugin.Log($"OnNewFollowerReceived: {eventData}");
    		PendingEvents.Enqueue(new NewFollowerEvent
    		{
    			Viewer = eventData.viewer
    		});
    	}

    	public static bool ObserveEvent(out NewFollowerEvent observedEvent)
    	{
    		observedEvent = null;
    		if (!IsPending)
    		{
    			return false;
    		}
    		observedEvent = PendingEvents.Peek();
    		return true;
    	}

    	public static bool ConsumeEvent(out NewFollowerEvent processedEvent)
    	{
    		processedEvent = null;
    		if (!IsPending)
    		{
    			return false;
    		}
    		processedEvent = PendingEvents.Dequeue();
    		TotalNewFollowers++;
    		return true;
    	}
    }
}
