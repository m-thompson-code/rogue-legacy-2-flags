using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Unity.Cloud.UserReporting.Plugin.SimpleJson
{
	// Token: 0x02000848 RID: 2120
	[GeneratedCode("simple-json", "1.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class JsonObject : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
	{
		// Token: 0x06004625 RID: 17957 RVA: 0x000FA25A File Offset: 0x000F845A
		public JsonObject()
		{
			this._members = new Dictionary<string, object>();
		}

		// Token: 0x06004626 RID: 17958 RVA: 0x000FA26D File Offset: 0x000F846D
		public JsonObject(IEqualityComparer<string> comparer)
		{
			this._members = new Dictionary<string, object>(comparer);
		}

		// Token: 0x17001764 RID: 5988
		public object this[int index]
		{
			get
			{
				return JsonObject.GetAtIndex(this._members, index);
			}
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x000FA290 File Offset: 0x000F8490
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

		// Token: 0x06004629 RID: 17961 RVA: 0x000FA30C File Offset: 0x000F850C
		public void Add(string key, object value)
		{
			this._members.Add(key, value);
		}

		// Token: 0x0600462A RID: 17962 RVA: 0x000FA31B File Offset: 0x000F851B
		public bool ContainsKey(string key)
		{
			return this._members.ContainsKey(key);
		}

		// Token: 0x17001765 RID: 5989
		// (get) Token: 0x0600462B RID: 17963 RVA: 0x000FA329 File Offset: 0x000F8529
		public ICollection<string> Keys
		{
			get
			{
				return this._members.Keys;
			}
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x000FA336 File Offset: 0x000F8536
		public bool Remove(string key)
		{
			return this._members.Remove(key);
		}

		// Token: 0x0600462D RID: 17965 RVA: 0x000FA344 File Offset: 0x000F8544
		public bool TryGetValue(string key, out object value)
		{
			return this._members.TryGetValue(key, out value);
		}

		// Token: 0x17001766 RID: 5990
		// (get) Token: 0x0600462E RID: 17966 RVA: 0x000FA353 File Offset: 0x000F8553
		public ICollection<object> Values
		{
			get
			{
				return this._members.Values;
			}
		}

		// Token: 0x17001767 RID: 5991
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

		// Token: 0x06004631 RID: 17969 RVA: 0x000FA37D File Offset: 0x000F857D
		public void Add(KeyValuePair<string, object> item)
		{
			this._members.Add(item.Key, item.Value);
		}

		// Token: 0x06004632 RID: 17970 RVA: 0x000FA398 File Offset: 0x000F8598
		public void Clear()
		{
			this._members.Clear();
		}

		// Token: 0x06004633 RID: 17971 RVA: 0x000FA3A5 File Offset: 0x000F85A5
		public bool Contains(KeyValuePair<string, object> item)
		{
			return this._members.ContainsKey(item.Key) && this._members[item.Key] == item.Value;
		}

		// Token: 0x06004634 RID: 17972 RVA: 0x000FA3D8 File Offset: 0x000F85D8
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

		// Token: 0x17001768 RID: 5992
		// (get) Token: 0x06004635 RID: 17973 RVA: 0x000FA448 File Offset: 0x000F8648
		public int Count
		{
			get
			{
				return this._members.Count;
			}
		}

		// Token: 0x17001769 RID: 5993
		// (get) Token: 0x06004636 RID: 17974 RVA: 0x000FA455 File Offset: 0x000F8655
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004637 RID: 17975 RVA: 0x000FA458 File Offset: 0x000F8658
		public bool Remove(KeyValuePair<string, object> item)
		{
			return this._members.Remove(item.Key);
		}

		// Token: 0x06004638 RID: 17976 RVA: 0x000FA46C File Offset: 0x000F866C
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return this._members.GetEnumerator();
		}

		// Token: 0x06004639 RID: 17977 RVA: 0x000FA47E File Offset: 0x000F867E
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._members.GetEnumerator();
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x000FA490 File Offset: 0x000F8690
		public override string ToString()
		{
			return SimpleJson.SerializeObject(this);
		}

		// Token: 0x04003B79 RID: 15225
		private readonly Dictionary<string, object> _members;
	}
}
