using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleJSON
{
    public class JSONObject : JSONNode
    {
    	private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

    	private bool inline;

    	public override bool Inline
    	{
    		get
    		{
    			return inline;
    		}
    		set
    		{
    			inline = value;
    		}
    	}

    	public override JSONNodeType Tag => JSONNodeType.Object;

    	public override bool IsObject => true;

    	public override JSONNode this[string aKey]
    	{
    		get
    		{
    			if (m_Dict.ContainsKey(aKey))
    			{
    				return m_Dict[aKey];
    			}
    			return new JSONLazyCreator(this, aKey);
    		}
    		set
    		{
    			if (value == null)
    			{
    				value = JSONNull.CreateOrGet();
    			}
    			if (m_Dict.ContainsKey(aKey))
    			{
    				m_Dict[aKey] = value;
    			}
    			else
    			{
    				m_Dict.Add(aKey, value);
    			}
    		}
    	}

    	public override JSONNode this[int aIndex]
    	{
    		get
    		{
    			if (aIndex < 0 || aIndex >= m_Dict.Count)
    			{
    				return null;
    			}
    			return m_Dict.ElementAt(aIndex).Value;
    		}
    		set
    		{
    			if (value == null)
    			{
    				value = JSONNull.CreateOrGet();
    			}
    			if (aIndex >= 0 && aIndex < m_Dict.Count)
    			{
    				string key = m_Dict.ElementAt(aIndex).Key;
    				m_Dict[key] = value;
    			}
    		}
    	}

    	public override int Count => m_Dict.Count;

    	public override IEnumerable<JSONNode> Children
    	{
    		get
    		{
    			foreach (KeyValuePair<string, JSONNode> item in m_Dict)
    			{
    				yield return item.Value;
    			}
    		}
    	}

    	public override Enumerator GetEnumerator()
    	{
    		return new Enumerator(m_Dict.GetEnumerator());
    	}

    	public override void Add(string aKey, JSONNode aItem)
    	{
    		if (aItem == null)
    		{
    			aItem = JSONNull.CreateOrGet();
    		}
    		if (!string.IsNullOrEmpty(aKey))
    		{
    			if (m_Dict.ContainsKey(aKey))
    			{
    				m_Dict[aKey] = aItem;
    			}
    			else
    			{
    				m_Dict.Add(aKey, aItem);
    			}
    		}
    		else
    		{
    			m_Dict.Add(Guid.NewGuid().ToString(), aItem);
    		}
    	}

    	public override JSONNode Remove(string aKey)
    	{
    		if (!m_Dict.ContainsKey(aKey))
    		{
    			return null;
    		}
    		JSONNode result = m_Dict[aKey];
    		m_Dict.Remove(aKey);
    		return result;
    	}

    	public override JSONNode Remove(int aIndex)
    	{
    		if (aIndex < 0 || aIndex >= m_Dict.Count)
    		{
    			return null;
    		}
    		KeyValuePair<string, JSONNode> keyValuePair = m_Dict.ElementAt(aIndex);
    		m_Dict.Remove(keyValuePair.Key);
    		return keyValuePair.Value;
    	}

    	public override JSONNode Remove(JSONNode aNode)
    	{
    		try
    		{
    			KeyValuePair<string, JSONNode> keyValuePair = m_Dict.Where((KeyValuePair<string, JSONNode> k) => k.Value == aNode).First();
    			m_Dict.Remove(keyValuePair.Key);
    			return aNode;
    		}
    		catch
    		{
    			return null;
    		}
    	}

    	internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
    	{
    		aSB.Append('{');
    		bool flag = true;
    		if (inline)
    		{
    			aMode = JSONTextMode.Compact;
    		}
    		foreach (KeyValuePair<string, JSONNode> item in m_Dict)
    		{
    			if (!flag)
    			{
    				aSB.Append(',');
    			}
    			flag = false;
    			if (aMode == JSONTextMode.Indent)
    			{
    				aSB.AppendLine();
    			}
    			if (aMode == JSONTextMode.Indent)
    			{
    				aSB.Append(' ', aIndent + aIndentInc);
    			}
    			aSB.Append('"').Append(JSONNode.Escape(item.Key)).Append('"');
    			if (aMode == JSONTextMode.Compact)
    			{
    				aSB.Append(':');
    			}
    			else
    			{
    				aSB.Append(" : ");
    			}
    			item.Value.WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
    		}
    		if (aMode == JSONTextMode.Indent)
    		{
    			aSB.AppendLine().Append(' ', aIndent);
    		}
    		aSB.Append('}');
    	}

    	public override void SerializeBinary(BinaryWriter aWriter)
    	{
    		aWriter.Write((byte)2);
    		aWriter.Write(m_Dict.Count);
    		foreach (string key in m_Dict.Keys)
    		{
    			aWriter.Write(key);
    			m_Dict[key].SerializeBinary(aWriter);
    		}
    	}
    }
}
