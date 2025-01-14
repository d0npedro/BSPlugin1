using System;
using JetBrains.Annotations;

namespace GameChanger.EventData
{
    [Serializable]
    [UsedImplicitly]
    internal class SabotageReceivedData : IEventData
    {
    	public Viewer viewer;

    	public string color;

    	public override string ToString()
    	{
    		return $"viewer: {viewer}, color: {color}";
    	}
    }
}
