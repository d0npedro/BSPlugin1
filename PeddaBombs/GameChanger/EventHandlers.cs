using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GameChanger
{
    internal abstract class EventHandlers
    {
    	public abstract void Invoke(object eventData);

    	public abstract void AddHandler(Delegate handler);

    	public abstract void RemoveHandler(Delegate handler);
    }
    [UsedImplicitly]
    internal class EventHandlers<T> : EventHandlers where T : IEventData
    {
    	private readonly List<Events.GameChangerEventHandler<T>> _handlers = new List<Events.GameChangerEventHandler<T>>();

    	public override void Invoke(object eventData)
    	{
    		if (!(eventData is T))
    		{
    			throw new ArgumentException();
    		}
    		T typedEventData = (T)eventData;
    		lock (_handlers)
    		{
    			_handlers.ForEach(delegate(Events.GameChangerEventHandler<T> handler)
    			{
    				handler(typedEventData);
    			});
    		}
    	}

    	public override void AddHandler(Delegate handler)
    	{
    		if (!(handler is Events.GameChangerEventHandler<T>))
    		{
    			throw new ArgumentException();
    		}
    		Events.GameChangerEventHandler<T> item = (Events.GameChangerEventHandler<T>)handler;
    		lock (_handlers)
    		{
    			if (!_handlers.Contains(item))
    			{
    				_handlers.Add(item);
    			}
    		}
    	}

    	public override void RemoveHandler(Delegate handler)
    	{
    		if (!(handler is Events.GameChangerEventHandler<T>))
    		{
    			throw new ArgumentException();
    		}
    		Events.GameChangerEventHandler<T> item = (Events.GameChangerEventHandler<T>)handler;
    		lock (_handlers)
    		{
    			if (_handlers.Contains(item))
    			{
    				_handlers.Remove(item);
    			}
    		}
    	}
    }
}
