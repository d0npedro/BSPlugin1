using System;

namespace GameChanger
{
    internal class ProtocolEventNameAttribute : Attribute
    {
    	public string EventName { get; }

    	public ProtocolEventNameAttribute(string eventName)
    	{
    		EventName = eventName;
    	}
    }
}
