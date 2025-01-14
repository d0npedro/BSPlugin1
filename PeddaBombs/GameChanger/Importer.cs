using System;
using System.Collections.Generic;
using System.Text;
using BeatBits;
using SimpleJSON;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameChanger
{
    public class Importer
    {
    	[Serializable]
    	public class Hierarchy
    	{
    		public int[] dependencies;

    		public MonobehaviourItem[] monobehaviours;
    	}

    	[Serializable]
    	public class MonobehaviourItem
    	{
    		public int[] path;

    		public int instanceid;

    		public string type;

    		public string data;

    		public override string ToString()
    		{
    			return string.Format("path: {0}, instancedid: {1}, type {2}, data: {3}", string.Join(",", path), instanceid, type, data);
    		}
    	}

    	[Serializable]
    	public class ComponentItem
    	{
    		public int[] path;

    		public string type;

    		public int instanceid;
    	}

    	[Serializable]
    	public class GameObjectItem
    	{
    		public int[] path;

    		public int instanceid;
    	}

    	public class PrefabComponent : UnityEngine.Object
    	{
    		public GameObject targetGameObject;

    		public Type componentType;

    		public Component component;

    		public string componentSerializedData;
    	}

    	public class ExctractObjects : MonoBehaviour
    	{
    		public UnityEngine.Object[] objects;
    	}

    	private static bool debug;

    	public static void ReconstructPrefab(GameObject prefab, GameObject assetBundleMap, string jsonFile)
    	{
    		long ticks = DateTime.Now.Ticks;
    		if (prefab == null || assetBundleMap == null || string.IsNullOrEmpty(jsonFile))
    		{
    			Plugin.Log("Cannot Reconstruct prefab! Prefab: " + prefab?.ToString() + ", assetBundleMap: " + assetBundleMap?.ToString() + ", JsonFile: " + jsonFile);
    			return;
    		}
    		Plugin.Log("Starting reconstruion of Prefab: " + prefab?.ToString() + ", assetBundleMap: " + assetBundleMap?.ToString() + ", JsonFile: " + jsonFile);
    		if (debug)
    		{
    			Plugin.Log("Create AssetBundleMap");
    		}
    		Dictionary<int, int> dictionary = new Dictionary<int, int>();
    		Hierarchy hierarchy = JsonUtility.FromJson<Hierarchy>(jsonFile);
    		if (debug)
    		{
    			Plugin.Log("Deserialize AssetBundleMap");
    		}
    		JSONNode jSONNode = JSON.Parse(jsonFile);
    		hierarchy.monobehaviours = new MonobehaviourItem[jSONNode["monobehaviours"].Count];
    		for (int i = 0; i < jSONNode["monobehaviours"].Count; i++)
    		{
    			hierarchy.monobehaviours[i] = new MonobehaviourItem();
    			hierarchy.monobehaviours[i].path = new int[jSONNode["monobehaviours"][i]["path"].Count];
    			for (int j = 0; j < jSONNode["monobehaviours"][i]["path"].Count; j++)
    			{
    				hierarchy.monobehaviours[i].path[j] = jSONNode["monobehaviours"][i]["path"][j];
    			}
    			hierarchy.monobehaviours[i].instanceid = jSONNode["monobehaviours"][i]["instanceid"].AsInt;
    			hierarchy.monobehaviours[i].type = jSONNode["monobehaviours"][i]["type"].Value;
    			hierarchy.monobehaviours[i].data = jSONNode["monobehaviours"][i]["data"].Value;
    		}
    		List<int> list = new List<int>();
    		EventTrigger component = assetBundleMap.GetComponent<EventTrigger>();
    		for (int k = 0; k < component.triggers.Count; k++)
    		{
    			int persistentEventCount = component.triggers[k].callback.GetPersistentEventCount();
    			for (int l = 0; l < persistentEventCount; l++)
    			{
    				if (component.triggers[k].callback.GetPersistentTarget(l) != null)
    				{
    					list.Add(component.triggers[k].callback.GetPersistentTarget(l).GetInstanceID());
    				}
    				else
    				{
    					list.Add(0);
    				}
    			}
    		}
    		List<int> list2 = new List<int>(hierarchy.dependencies);
    		PrefabComponent[] array = new PrefabComponent[hierarchy.monobehaviours.Length];
    		for (int m = 0; m < hierarchy.monobehaviours.Length; m++)
    		{
    			if (debug)
    			{
    				Plugin.Log(m + " " + hierarchy.monobehaviours[m].type.ToString());
    			}
    			array[m] = new PrefabComponent();
    			int[] path = hierarchy.monobehaviours[m].path;
    			array[m].targetGameObject = GetGameObjectByPath(prefab, path);
    			if (array[m].targetGameObject != null)
    			{
    				array[m].componentSerializedData = DecodeFromBase64(hierarchy.monobehaviours[m].data);
    				string[] array2 = hierarchy.monobehaviours[m].type.Split(',');
    				if (array2[1].Contains("Assembly-CSharp"))
    				{
    					array[m].componentType = Type.GetType(array2[0]);
    				}
    				else
    				{
    					array[m].componentType = Type.GetType(hierarchy.monobehaviours[m].type);
    				}
    				if (array[m].componentType != null)
    				{
    					array[m].component = array[m].targetGameObject.GetComponent(array[m].componentType);
    					if (array[m].component == null)
    					{
    						if (debug)
    						{
    							Plugin.Log("Adding Component: " + array[m].componentType?.ToString() + ", to: " + array[m].targetGameObject.name);
    						}
    						array[m].component = array[m].targetGameObject.AddComponent(array[m].componentType);
    						int num = list2.IndexOf(hierarchy.monobehaviours[m].instanceid);
    						if (num != -1)
    						{
    							list[num] = array[m].component.GetInstanceID();
    							continue;
    						}
    						list2.Add(hierarchy.monobehaviours[m].instanceid);
    						list.Add(array[m].component.GetInstanceID());
    					}
    				}
    				else
    				{
    					Plugin.Log("Target Component cannot be updated or created, type cannot be found: " + hierarchy.monobehaviours[m].type);
    				}
    			}
    			else
    			{
    				Plugin.Log("TargetGameObject does not exist! Path: " + hierarchy.monobehaviours[m].path);
    			}
    		}
    		int num2 = Mathf.Min(list2.Count, list.Count);
    		for (int n = 0; n < num2; n++)
    		{
    			dictionary.Add(list2[n], list[n]);
    		}
    		if (debug)
    		{
    			Plugin.Log("Fill-up data and re-create references");
    		}
    		for (int num3 = 0; num3 < array.Length; num3++)
    		{
    			if (array[num3].targetGameObject != null)
    			{
    				if (array[num3].componentType != null)
    				{
    					ReplaceInstanceID(ref array[num3].componentSerializedData, dictionary);
    					if (!Application.isEditor)
    					{
    						array[num3].componentSerializedData = array[num3].componentSerializedData.Replace("instanceID", "m_FileID");
    					}
    					JsonUtility.FromJsonOverwrite(array[num3].componentSerializedData, array[num3].component);
    				}
    				else
    				{
    					Plugin.Log("Target Component cannot be updated or crated, type cannot be found: " + hierarchy.monobehaviours[num3].type);
    				}
    			}
    			else
    			{
    				Plugin.Log("TargetGameObject does not exist! Path: " + hierarchy.monobehaviours[num3].path);
    			}
    		}
    		TimeSpan timeSpan = TimeSpan.FromTicks(DateTime.Now.Ticks - ticks);
    		if (debug)
    		{
    			Plugin.Log("Reconstruction took: " + timeSpan.Milliseconds + "ms");
    		}
    		if (!debug)
    		{
    			return;
    		}
    		string text = "";
    		foreach (KeyValuePair<int, int> item in dictionary)
    		{
    			text = text + "key: " + item.Key + ", value: " + item.Value + "\n";
    		}
    		Plugin.Log(text);
    	}

    	public static GameObject GetGameObjectByPath(GameObject root, int[] path)
    	{
    		Transform transform = root.transform;
    		for (int i = 1; i < path.Length; i++)
    		{
    			transform = transform.GetChild(path[i]);
    		}
    		return transform.gameObject;
    	}

    	public static int[] GetGameObjectPath(GameObject gameObject, GameObject root)
    	{
    		List<int> list = new List<int>();
    		list.Add(0);
    		GetGameObjectPath(gameObject, root, list);
    		return list.ToArray();
    	}

    	public static void GetGameObjectPath(GameObject gameObject, GameObject root, List<int> path)
    	{
    		if (gameObject.transform.parent != null && gameObject.transform.parent != root)
    		{
    			GetGameObjectPath(gameObject.transform.parent.gameObject, root, path);
    		}
    		if (!(gameObject.transform.parent != null))
    		{
    			return;
    		}
    		for (int i = 0; i < gameObject.transform.parent.childCount; i++)
    		{
    			if (gameObject.transform.parent.GetChild(i) == gameObject.transform)
    			{
    				path.Add(i);
    			}
    		}
    	}

    	public static string DecodeFromBase64(string base64)
    	{
    		return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
    	}

    	public static bool ContainsWord(string word, ref string input, ref int charIndex)
    	{
    		if (charIndex + word.Length >= input.Length)
    		{
    			return false;
    		}
    		for (int i = 0; i < word.Length; i++)
    		{
    			charIndex++;
    			if (input[charIndex] != word[i])
    			{
    				return false;
    			}
    		}
    		return true;
    	}

    	public static List<int> FindInstanceIDs(ref string input, string instanceWord = "\"instanceID\":", char endChar = '}')
    	{
    		List<int> list = new List<int>();
    		int i = 0;
    		string text = "";
    		bool flag = false;
    		for (; i < input.Length; i++)
    		{
    			if (!flag)
    			{
    				if (ContainsWord(instanceWord, ref input, ref i))
    				{
    					text = "";
    					flag = true;
    				}
    			}
    			else if (input[i] == endChar)
    			{
    				flag = false;
    				if (!string.IsNullOrEmpty(text))
    				{
    					int result = 0;
    					if (int.TryParse(text, out result) && result != 0 && !list.Contains(result))
    					{
    						list.Add(result);
    					}
    				}
    			}
    			else
    			{
    				text += input[i];
    			}
    		}
    		return list;
    	}

    	public static void ReplaceInstanceID(ref string input, Dictionary<int, int> keyPairs, string instanceWord = "\"instanceID\":", char endChar = '}')
    	{
    		int i = 0;
    		string text = "";
    		int num = 0;
    		bool flag = false;
    		for (; i < input.Length; i++)
    		{
    			if (!flag)
    			{
    				if (ContainsWord(instanceWord, ref input, ref i))
    				{
    					text = "";
    					flag = true;
    					num = i;
    				}
    			}
    			else if (input[i] == endChar)
    			{
    				flag = false;
    				if (string.IsNullOrEmpty(text))
    				{
    					continue;
    				}
    				int result = 0;
    				if (int.TryParse(text, out result))
    				{
    					int value = 0;
    					if (keyPairs.TryGetValue(result, out value))
    					{
    						string text2 = input.Substring(0, num + 1);
    						int num2 = num + 1 + text.Length;
    						string text3 = input.Substring(num2, input.Length - num2);
    						input = text2 + value + text3;
    					}
    				}
    			}
    			else
    			{
    				text += input[i];
    			}
    		}
    	}

    	public static UnityEngine.Object[] GetDependencies(MonoBehaviour monoBehaviour)
    	{
    		string input = JsonUtility.ToJson(monoBehaviour);
    		List<int> list = FindInstanceIDs(ref input);
    		if (list.Count == 0)
    		{
    			return new UnityEngine.Object[0];
    		}
    		int num = list.Count - 1;
    		GameObject gameObject = new GameObject("ExctractObjects");
    		ExctractObjects exctractObjects = gameObject.AddComponent<ExctractObjects>();
    		exctractObjects.objects = new UnityEngine.Object[list.Count];
    		string text = "";
    		for (int i = 0; i < num; i++)
    		{
    			text = text + "{\"instanceID\":" + list[i] + "},";
    		}
    		text = text + "{\"instanceID\":" + list[num] + "}";
    		JsonUtility.FromJsonOverwrite("{\"objects\":[" + text + "]}", exctractObjects);
    		UnityEngine.Object[] result = (UnityEngine.Object[])exctractObjects.objects.Clone();
    		UnityEngine.Object.DestroyImmediate(gameObject);
    		return result;
    	}
    }
}
