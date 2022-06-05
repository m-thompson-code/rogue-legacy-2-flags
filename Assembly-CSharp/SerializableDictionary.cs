using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x0200000A RID: 10
public class SerializableDictionary<TKey, TValue> : SerializableDictionaryBase<TKey, TValue, TValue>
{
	// Token: 0x0600006D RID: 109 RVA: 0x00002FED File Offset: 0x000011ED
	public SerializableDictionary()
	{
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00002FF5 File Offset: 0x000011F5
	public SerializableDictionary(IDictionary<TKey, TValue> dict) : base(dict)
	{
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00002FFE File Offset: 0x000011FE
	protected SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00003008 File Offset: 0x00001208
	protected override TValue GetValue(TValue[] storage, int i)
	{
		return storage[i];
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00003011 File Offset: 0x00001211
	protected override void SetValue(TValue[] storage, int i, TValue value)
	{
		storage[i] = value;
	}
}
