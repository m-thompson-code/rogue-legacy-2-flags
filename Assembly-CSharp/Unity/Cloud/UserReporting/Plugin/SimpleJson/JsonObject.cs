using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Unity.Cloud.UserReporting.Plugin.SimpleJson
{
	// Token: 0x02000D36 RID: 3382
	[GeneratedCode("simple-json", "1.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class JsonObject : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
	{
		// Token: 0x06006084 RID: 24708 RVA: 0x00035389 File Offset: 0x00033589
		public JsonObject()
		{
			this._members = new Dictionary<string, object>();
		}

		// Token: 0x06006085 RID: 24709 RVA: 0x0003539C File Offset: 0x0003359C
		public JsonObject(IEqualityComparer<string> comparer)
		{
			this._members = new Dictionary<string, object>(comparer);
		}

		// Token: 0x17001FA3 RID: 8099
		public object this[int index]
		{
			get
			{
				return JsonObject.GetAtIndex(this._members, index);
			}
		}

		// Token: 0x06006087 RID: 24711 RVA: 0x0016708C File Offset: 0x0016528C
		internal static object GetAtIndex(IDictionary<string, object> obj, int index)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (index >= obj.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int num = 0;
			foreach (KeyValuePair<string, object> keyValuePair in obj)
			{
				if (num++ == index)
				{
					return keyValuePair.Value;
				}
			}
			return null;
		}

		// Token: 0x06006088 RID: 24712 RVA: 0x000353BE File Offset: 0x000335BE
		public void Add(string key, object value)
		{
			this._members.Add(key, value);
		}

		// Token: 0x06006089 RID: 24713 RVA: 0x000353CD File Offset: 0x000335CD
		public bool ContainsKey(string key)
		{
			return this._members.ContainsKey(key);
		}

		// Token: 0x17001FA4 RID: 8100
		// (get) Token: 0x0600608A RID: 24714 RVA: 0x000353DB File Offset: 0x000335DB
		public ICollection<string> Keys
		{
			get
			{
				return this._members.Keys;
			}
		}

		// Token: 0x0600608B RID: 24715 RVA: 0x000353E8 File Offset: 0x000335E8
		public bool Remove(string key)
		{
			return this._members.Remove(key);
		}

		// Token: 0x0600608C RID: 24716 RVA: 0x000353F6 File Offset: 0x000335F6
		public bool TryGetValue(string key, out object value)
		{
			return this._members.TryGetValue(key, out value);
		}

		// Token: 0x17001FA5 RID: 8101
		// (get) Token: 0x0600608D RID: 24717 RVA: 0x00035405 File Offset: 0x00033605
		public ICollection<object> Values
		{
			get
			{
				return this._members.Values;
			}
		}

		// Token: 0x17001FA6 RID: 8102
		public object this[string key]
		{
			get
			{
				return this._members[key];
			}
			set
			{
				this._members[key] = value;
			}
		}

		// Token: 0x06006090 RID: 24720 RVA: 0x0003542F File Offset: 0x0003362F
		public void Add(KeyValuePair<string, object> item)
		{
			this._members.Add(item.Key, item.Value);
		}

		// Token: 0x06006091 RID: 24721 RVA: 0x0003544A File Offset: 0x0003364A
		public void Clear()
		{
			this._members.Clear();
		}

		// Token: 0x06006092 RID: 24722 RVA: 0x00035457 File Offset: 0x00033657
		public bool Contains(KeyValuePair<string, object> item)
		{
			return this._members.ContainsKey(item.Key) && this._members[item.Key] == item.Value;
		}

		// Token: 0x06006093 RID: 24723 RVA: 0x00167108 File Offset: 0x00165308
		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int num = this.Count;
			foreach (KeyValuePair<string, object> keyValuePair in this)
			{
				array[arrayIndex++] = keyValuePair;
				if (--num <= 0)
				{
					break;
				}
			}
		}

		// Token: 0x17001FA7 RID: 8103
		// (get) Token: 0x06006094 RID: 24724 RVA: 0x0003548A File Offset: 0x0003368A
		public int Count
		{
			get
			{
				return this._members.Count;
			}
		}

		// Token: 0x17001FA8 RID: 8104
		// (get) Token: 0x06006095 RID: 24725 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06006096 RID: 24726 RVA: 0x00035497 File Offset: 0x00033697
		public bool Remove(KeyValuePair<string, object> item)
		{
			return this._members.Remove(item.Key);
		}

		// Token: 0x06006097 RID: 24727 RVA: 0x000354AB File Offset: 0x000336AB
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return this._members.GetEnumerator();
		}

		// Token: 0x06006098 RID: 24728 RVA: 0x000354AB File Offset: 0x000336AB
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._members.GetEnumerator();
		}

		// Token: 0x06006099 RID: 24729 RVA: 0x000354BD File Offset: 0x000336BD
		public override string ToString()
		{
			return SimpleJson.SerializeObject(this);
		}

		// Token: 0x04004EDB RID: 20187
		private readonly Dictionary<string, object> _members;
	}
}
