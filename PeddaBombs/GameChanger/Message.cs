using JetBrains.Annotations;

namespace GameChanger
{
    [UsedImplicitly]
    internal class Message
    {
    	public const string TypeResponse = "response";

    	public const string TypeEvent = "event";

    	public string type;
    }
}
