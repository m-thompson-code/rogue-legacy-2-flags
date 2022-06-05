using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RLAudio
{
	// Token: 0x02000E4A RID: 3658
	public abstract class AudioLibrary<T> : ScriptableObject where T : AudioLibraryEntry
	{
		// Token: 0x17002115 RID: 8469
		// (get) Token: 0x06006720 RID: 26400 RVA: 0x00038C8F File Offset: 0x00036E8F
		protected T Default
		{
			get
			{
				return this.m_default;
			}
		}

		// Token: 0x17002116 RID: 8470
		// (get) Token: 0x06006721 RID: 26401 RVA: 0x00038C97 File Offset: 0x00036E97
		// (set) Token: 0x06006722 RID: 26402 RVA: 0x00038C9F File Offset: 0x00036E9F
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

		// Token: 0x06006723 RID: 26403 RVA: 0x00038CAF File Offset: 0x00036EAF
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

		// Token: 0x06006724 RID: 26404 RVA: 0x0017C858 File Offset: 0x0017AA58
		private void InitializeTable()
		{
			this.m_entryTable = new Dictionary<string, T>();
			foreach (T t in this.Entries)
			{
				this.m_entryTable.Add(t.Key, t);
			}
		}

		// Token: 0x06006725 RID: 26405 RVA: 0x0017C8C8 File Offset: 0x0017AAC8
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

		// Token: 0x040053A9 RID: 21417
		[SerializeField]
		[FormerlySerializedAs("entries")]
		protected List<T> m_entries;

		// Token: 0x040053AA RID: 21418
		[SerializeField]
		protected T m_default;

		// Token: 0x040053AB RID: 21419
		protected Dictionary<string, T> m_entryTable;
	}
}
