using System;
using JetBrains.Annotations;

namespace GameChanger.EventData
{
    [Serializable]
    [UsedImplicitly]
    internal class NewFollowerReceivedData : IEventData
    {
    	public Viewer viewer;
    }
}
