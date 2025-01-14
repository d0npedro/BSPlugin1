using System;
using BeatBits;
using UnityEngine;
using WebSocketSharp;

namespace GameChanger
{
    internal static class Service
    {
    	public static string SocketAddress = "ws://localhost:{0}";

    	private static WebSocket _socket;

    	private static bool _autoReconnect;

    	public static bool IsConnected { get; private set; }

    	public static bool IsConnecting { get; private set; }

    	public static void Start(bool autoReconnect = true)
    	{
    		_autoReconnect = autoReconnect;
    		string text = string.Format(SocketAddress, Settings.Port.value);
    		Plugin.Log("Connecting to GameChanger on " + text + "...");
    		if (_socket == null)
    		{
    			_socket = new WebSocket(text);
    			_socket.OnOpen += OnSocketOpen;
    			_socket.OnClose += OnSocketClose;
    			_socket.OnError += OnSocketError;
    			_socket.OnMessage += OnSocketMessage;
    			_socket.Log.Output = delegate
    			{
    			};
    		}
    		_socket.ConnectAsync();
    		IsConnecting = true;
    	}

    	public static void Stop()
    	{
    		if (_socket != null)
    		{
    			Plugin.Log("Disconnecting from GameChanger...");
    			_socket.Close(CloseStatusCode.Normal);
    			IsConnected = false;
    			IsConnecting = false;
    			_autoReconnect = false;
    		}
    	}

    	private static void OnSocketOpen(object sender, EventArgs eventArgs)
    	{
    		Plugin.Log("Connected to GameChanger!");
    		IsConnecting = false;
    		IsConnected = true;
    	}

    	private static void OnSocketError(object sender, ErrorEventArgs errorEventArgs)
    	{
    		if (errorEventArgs.Exception != null)
    		{
    			Plugin.Log("GameChanger socket exception: " + errorEventArgs.Exception);
    		}
    		if (errorEventArgs.Message != null)
    		{
    			Plugin.Log("GameChanger socket error: " + errorEventArgs.Message);
    		}
    	}

    	private static void OnSocketClose(object sender, CloseEventArgs closeEventArgs)
    	{
    		if (!closeEventArgs.WasClean)
    		{
    			Plugin.Log($"Socket Error: ({closeEventArgs.Code}) {closeEventArgs.Reason} ");
    		}
    		Plugin.Log("Disconnected from GameChanger.");
    		IsConnecting = false;
    		IsConnected = false;
    		if (_autoReconnect)
    		{
    			Start(_autoReconnect);
    		}
    	}

    	private static void OnSocketMessage(object sender, MessageEventArgs message)
    	{
    		if (message.Data != null && JsonUtility.FromJson<Message>(message.Data).type == "event")
    		{
    			HandleEventMessage(message.Data);
    		}
    	}

    	private static void HandleEventMessage(string eventMessage)
    	{
    		Event @event = JsonUtility.FromJson<Event>(eventMessage);
    		if (@event != null)
    		{
    			Events.RaiseEventFromMessage(@event, eventMessage);
    		}
    	}
    }
}
