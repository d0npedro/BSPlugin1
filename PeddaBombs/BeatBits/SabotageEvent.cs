using GameChanger.EventData;

namespace BeatBits
{
    public class SabotageEvent
    {
    	public Viewer Viewer;

    	public string Color;

    	public override string ToString()
    	{
    		return $"Viewer: {Viewer}, Color: {Color}";
    	}
    }
}
