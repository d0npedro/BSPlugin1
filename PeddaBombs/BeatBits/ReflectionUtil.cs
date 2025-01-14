using System;
using System.Reflection;

namespace BeatBits
{
    internal class ReflectionUtil
    {
    	public static void SetPrivateField(object obj, string fieldName, object value)
    	{
    		obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(obj, value);
    	}

    	public static T GetPrivateField<T>(object obj, string fieldName)
    	{
    		return (T)obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj);
    	}

    	public static object GetPrivateField(Type type, object obj, string fieldName)
    	{
    		return obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj);
    	}

    	public static void InvokePrivateMethod(object obj, string methodName, object[] methodParams)
    	{
    		obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(obj, methodParams);
    	}
    }
}
