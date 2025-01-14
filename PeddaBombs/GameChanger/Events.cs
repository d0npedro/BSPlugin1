using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BeatBits;
using GameChanger.EventData;
using Newtonsoft.Json;
using SimpleJSON;

namespace GameChanger
{
    internal static class Events
    {
    	public delegate void GameChangerEventHandler<in T>(T eventData) where T : IEventData;

    	private static readonly ConcurrentDictionary<Type, EventHandlers> EventHandlers;

    	private static readonly Dictionary<string, Type> ProtocolEventToType;

    	[ProtocolEventName("newFollower")]
    	public static event GameChangerEventHandler<NewFollowerReceivedData> NewFollower
    	{
    		add
    		{
    			AddHandler(value);
    		}
    		remove
    		{
    			RemoveHandler(value);
    		}
    	}

    	[ProtocolEventName("twitchCheerReceived")]
    	public static event GameChangerEventHandler<TwitchCheerReceivedData> TwitchCheerReceived
    	{
    		add
    		{
    			AddHandler(value);
    		}
    		remove
    		{
    			RemoveHandler(value);
    		}
    	}

    	[ProtocolEventName("beatSaberSabotaged")]
    	public static event GameChangerEventHandler<SabotageReceivedData> BeatSaberSabotage
    	{
    		add
    		{
    			AddHandler(value);
    		}
    		remove
    		{
    			RemoveHandler(value);
    		}
    	}

    	[ProtocolEventName("twitchSubscription")]
    	public static event GameChangerEventHandler<TwitchSubscriptionReceivedData> TwitchSubscription
    	{
    		add
    		{
    			AddHandler(value);
    		}
    		remove
    		{
    			RemoveHandler(value);
    		}
    	}

    	static Events()
    	{
    		EventHandlers = new ConcurrentDictionary<Type, EventHandlers>();
    		ProtocolEventToType = new Dictionary<string, Type>();
    		foreach (EventInfo item in from evt in typeof(Events).GetEvents(BindingFlags.Static | BindingFlags.Public)
    			where evt.EventHandlerType.IsGenericType
    			where evt.EventHandlerType.GetGenericTypeDefinition() == typeof(GameChangerEventHandler<>)
    			select evt)
    		{
    			string eventName = (item.GetCustomAttribute<ProtocolEventNameAttribute>() ?? throw new Exception("All GameChanger events must have a ProtocolEventName attribute!")).EventName;
    			if (ProtocolEventToType.ContainsKey(eventName))
    			{
    				throw new Exception("Protocol event '" + eventName + "' already has an associated event!");
    			}
    			ProtocolEventToType[eventName] = item.EventHandlerType.GenericTypeArguments.First();
    		}
    	}

    	private static void AddHandler<T>(GameChangerEventHandler<T> handler) where T : IEventData
    	{
    		Type typeFromHandle = typeof(T);
    		if (!EventHandlers.ContainsKey(typeFromHandle))
    		{
    			EventHandlers[typeFromHandle] = Activator.CreateInstance<EventHandlers<T>>();
    		}
    		EventHandlers[typeFromHandle].AddHandler(handler);
    	}

    	private static void RemoveHandler<T>(GameChangerEventHandler<T> handler) where T : IEventData
    	{
    		Type typeFromHandle = typeof(T);
    		if (EventHandlers.ContainsKey(typeFromHandle))
    		{
    			EventHandlers[typeFromHandle].RemoveHandler(handler);
    		}
    	}

    	private static void RaiseEvent(Type eventDataType, object eventData)
    	{
    		if (EventHandlers.ContainsKey(eventDataType))
    		{
    			EventHandlers[eventDataType].Invoke(eventData);
    		}
    	}

    	public static void RaiseEventFromMessage(Event eventMessage, string json)
    	{
    		if (ProtocolEventToType.ContainsKey(eventMessage.name))
    		{
    			Type type = ProtocolEventToType[eventMessage.name];
    			object obj = DeserializeEventData(type, json);
    			if (obj != null)
    			{
    				RaiseEvent(type, obj);
    			}
    		}
    	}

    	private static object DeserializeEventData(Type targetType, string json)
    	{
            Plugin.Log(string.Format("DeserializeEventData: {0}", JSON.Parse(json)["data"].ToString()));
    		return JsonConvert.DeserializeObject(JSON.Parse(json)["data"].ToString(), targetType);
    	}
    }
}
