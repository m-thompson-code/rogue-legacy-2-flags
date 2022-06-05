using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// Token: 0x02000007 RID: 7
public abstract class SerializableDictionaryBase<TKey, TValue, TValueStorage> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	// Token: 0x0600005E RID: 94 RVA: 0x00005E60 File Offset: 0x00004060
	public SerializableDictionaryBase()
	{
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00005E68 File Offset: 0x00004068
	public SerializableDictionaryBase(IDictionary<TKey, TValue> dict) : base(dict.Count)
	{
		foreach (KeyValuePair<TKey, TValue> keyValuePair in dict)
		{
			base[keyValuePair.Key] = keyValuePair.Value;
		}
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00005ECC File Offset: 0x000040CC
	protected SerializableDictionaryBase(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	// Token: 0x06000061 RID: 97
	protected abstract void SetValue(TValueStorage[] storage, int i, TValue value);

	// Token: 0x06000062 RID: 98
	protected abstract TValue GetValue(TValueStorage[] storage, int i);

	// Token: 0x06000063 RID: 99 RVA: 0x00005ED8 File Offset: 0x000040D8
	public void CopyFrom(IDictionary<TKey, TValue> dict)
	{
		base.Clear();
		foreach (KeyValuePair<TKey, TValue> keyValuePair in dict)
		{
			base[keyValuePair.Key] = keyValuePair.Value;
		}
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00005F34 File Offset: 0x00004134
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

	// Token: 0x06000065 RID: 101 RVA: 0x00005FAC File Offset: 0x000041AC
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

	// Token: 0x040000F2 RID: 242
	[SerializeField]
	private TKey[] m_keys;

	// Token: 0x040000F3 RID: 243
	[SerializeField]
	private TValueStorage[] m_values;
}
