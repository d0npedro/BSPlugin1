using System.Collections.Generic;
using GameChanger;
using GameChanger.EventData;

namespace BeatBits
{
    public static class SabotageQueue
    {
    	private static readonly Queue<SabotageEvent> PendingEvents = new Queue<SabotageEvent>();

    	public static bool IsPending => PendingEvents.Count != 0;

    	public static void Setup()
    	{
    		Events.BeatSaberSabotage += BeatSaberSabotage;
    	}

    	private static void BeatSaberSabotage(SabotageReceivedData eventData)
    	{
    		Plugin.Log($"BeatSaberSabotage: {eventData}");
    		PendingEvents.Enqueue(new SabotageEvent
    		{
    			Viewer = eventData.viewer,
    			Color = eventData.color
    		});
    	}

    	public static bool ObserveEvent(out SabotageEvent observedEvent)
    	{
    		observedEvent = null;
    		if (!IsPending)
    		{
    			return false;
    		}
    		observedEvent = PendingEvents.Peek();
    		return true;
    	}

    	public static bool ConsumeEvent(out SabotageEvent processedEvent)
    	{
    		processedEvent = null;
    		if (!IsPending)
    		{
    			return false;
    		}
    		processedEvent = PendingEvents.Dequeue();
    		return true;
    	}
    }
}
