using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x0200000D RID: 13
public class SerializableDictionary<TKey, TValue, TValueStorage> : SerializableDictionaryBase<TKey, TValue, TValueStorage> where TValueStorage : SerializableDictionary.Storage<TValue>, new()
{
	// Token: 0x06000073 RID: 115 RVA: 0x0000301B File Offset: 0x0000121B
	public SerializableDictionary()
	{
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00003023 File Offset: 0x00001223
	public SerializableDictionary(IDictionary<TKey, TValue> dict) : base(dict)
	{
	}

	// Token: 0x06000075 RID: 117 RVA: 0x0000302C File Offset: 0x0000122C
	protected SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00003036 File Offset: 0x00001236
	protected override TValue GetValue(TValueStorage[] storage, int i)
	{
		return storage[i].data;
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00003049 File Offset: 0x00001249
	protected override void SetValue(TValueStorage[] storage, int i, TValue value)
	{
		storage[i] = Activator.CreateInstance<TValueStorage>();
		storage[i].data = value;
	}
}
