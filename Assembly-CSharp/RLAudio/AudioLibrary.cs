using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RLAudio
{
	// Token: 0x020008DC RID: 2268
	public abstract class AudioLibrary<T> : ScriptableObject where T : AudioLibraryEntry
	{
		// Token: 0x17001836 RID: 6198
		// (get) Token: 0x06004A70 RID: 19056 RVA: 0x0010BFF2 File Offset: 0x0010A1F2
		protected T Default
		{
			get
			{
				return this.m_default;
			}
		}

		// Token: 0x17001837 RID: 6199
		// (get) Token: 0x06004A71 RID: 19057 RVA: 0x0010BFFA File Offset: 0x0010A1FA
		// (set) Token: 0x06004A72 RID: 19058 RVA: 0x0010C002 File Offset: 0x0010A202
		public List<T> Entries
		{
			get
			{
				return this.m_entries;
			}
			set
			{
				if (!Application.isPlaying)
				{
					this.m_entries = value;
				}
			}
		}

		// Token: 0x06004A73 RID: 19059 RVA: 0x0010C012 File Offset: 0x0010A212
		protected T GetAudioLibraryEntry(string key)
		{
			if (this.m_entryTable == null)
			{
				this.InitializeTable();
			}
			if (this.m_entryTable.ContainsKey(key))
			{
				return this.m_entryTable[key];
			}
			return this.Default;
		}

		// Token: 0x06004A74 RID: 19060 RVA: 0x0010C044 File Offset: 0x0010A244
		private void InitializeTable()
		{
			this.m_entryTable = new Dictionary<string, T>();
			foreach (T t in this.Entries)
			{
				this.m_entryTable.Add(t.Key, t);
			}
		}

		// Token: 0x06004A75 RID: 19061 RVA: 0x0010C0B4 File Offset: 0x0010A2B4
		public bool GetContainsEntry(string key)
		{
			if (this.m_entryTable == null)
			{
				this.InitializeTable();
			}
			bool result = false;
			if (this.m_entryTable != null)
			{
				result = this.m_entryTable.ContainsKey(key);
			}
			return result;
		}

		// Token: 0x04003E94 RID: 16020
		[SerializeField]
		[FormerlySerializedAs("entries")]
		protected List<T> m_entries;

		// Token: 0x04003E95 RID: 16021
		[SerializeField]
		protected T m_default;

		// Token: 0x04003E96 RID: 16022
		protected Dictionary<string, T> m_entryTable;
	}
}
