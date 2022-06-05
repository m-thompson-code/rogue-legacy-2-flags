using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x0200000A RID: 10
public class SerializableDictionary<TKey, TValue, TValueStorage> : SerializableDictionaryBase<TKey, TValue, TValueStorage> where TValueStorage : SerializableDictionary.Storage<TValue>, new()
{
	// Token: 0x0600006B RID: 107 RVA: 0x0000606E File Offset: 0x0000426E
	public SerializableDictionary()
	{
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00006076 File Offset: 0x00004276
	public SerializableDictionary(IDictionary<TKey, TValue> dict) : base(dict)
	{
	}

	// Token: 0x0600006D RID: 109 RVA: 0x0000607F File Offset: 0x0000427F
	protected SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00006089 File Offset: 0x00004289
	protected override TValue GetValue(TValueStorage[] storage, int i)
	{
		return storage[i].data;
	}

	// Token: 0x0600006F RID: 111 RVA: 0x0000609C File Offset: 0x0000429C
	protected override void SetValue(TValueStorage[] storage, int i, TValue value)
	{
		storage[i] = Activator.CreateInstance<TValueStorage>();
		storage[i].data = value;
	}
}
