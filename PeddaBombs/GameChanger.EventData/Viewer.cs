using System;

namespace GameChanger.EventData
{
    [Serializable]
    public class Viewer
    {
    	public string service;

    	public string serviceId;

    	public string name;

    	public string color;

    	public ViewerAvatar avatar;

    	public string lastEngaged;

    	public override string ToString()
    	{
    		return $"service: {service}, serviceId: {serviceId}, name: {name}, color: {color}, avatar: {avatar}, lastEngaged: {lastEngaged},";
    	}
    }
}
