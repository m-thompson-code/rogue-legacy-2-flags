using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// Token: 0x02000009 RID: 9
public abstract class SerializableDictionaryBase<TKey, TValue, TValueStorage> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	// Token: 0x06000065 RID: 101 RVA: 0x00002FDB File Offset: 0x000011DB
	public SerializableDictionaryBase()
	{
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00042D10 File Offset: 0x00040F10
	public SerializableDictionaryBase(IDictionary<TKey, TValue> dict) : base(dict.Count)
	{
		foreach (KeyValuePair<TKey, TValue> keyValuePair in dict)
		{
			base[keyValuePair.Key] = keyValuePair.Value;
		}
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00002FE3 File Offset: 0x000011E3
	protected SerializableDictionaryBase(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	// Token: 0x06000068 RID: 104
	protected abstract void SetValue(TValueStorage[] storage, int i, TValue value);

	// Token: 0x06000069 RID: 105
	protected abstract TValue GetValue(TValueStorage[] storage, int i);

	// Token: 0x0600006A RID: 106 RVA: 0x00042D74 File Offset: 0x00040F74
	public void CopyFrom(IDictionary<TKey, TValue> dict)
	{
		base.Clear();
		foreach (KeyValuePair<TKey, TValue> keyValuePair in dict)
		{
			base[keyValuePair.Key] = keyValuePair.Value;
		}
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00042DD0 File Offset: 0x00040FD0
	public void OnAfterDeserialize()
	{
		if (this.m_keys != null && this.m_values != null && this.m_keys.Length == this.m_values.Length)
		{
			base.Clear();
			int num = this.m_keys.Length;
			for (int i = 0; i < num; i++)
			{
				base[this.m_keys[i]] = this.GetValue(this.m_values, i);
			}
			this.m_keys = null;
			this.m_values = null;
		}
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00042E48 File Offset: 0x00041048
	public void OnBeforeSerialize()
	{
		int count = base.Count;
		this.m_keys = new TKey[count];
		this.m_values = new TValueStorage[count];
		int num = 0;
		foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
		{
			this.m_keys[num] = keyValuePair.Key;
			this.SetValue(this.m_values, num, keyValuePair.Value);
			num++;
		}
	}

	// Token: 0x040000FA RID: 250
	[SerializeField]
	private TKey[] m_keys;

	// Token: 0x040000FB RID: 251
	[SerializeField]
	private TValueStorage[] m_values;
}
