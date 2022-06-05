using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x02000008 RID: 8
public class SerializableDictionary<TKey, TValue> : SerializableDictionaryBase<TKey, TValue, TValue>
{
	// Token: 0x06000066 RID: 102 RVA: 0x00006040 File Offset: 0x00004240
	public SerializableDictionary()
	{
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00006048 File Offset: 0x00004248
	public SerializableDictionary(IDictionary<TKey, TValue> dict) : base(dict)
	{
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00006051 File Offset: 0x00004251
	protected SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	// Token: 0x06000069 RID: 105 RVA: 0x0000605B File Offset: 0x0000425B
	protected override TValue GetValue(TValue[] storage, int i)
	{
		return storage[i];
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00006064 File Offset: 0x00004264
	protected override void SetValue(TValue[] storage, int i, TValue value)
	{
		storage[i] = value;
	}
}
