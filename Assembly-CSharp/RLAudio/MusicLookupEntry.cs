using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000923 RID: 2339
	[Serializable]
	public class MusicLookupEntry
	{
		// Token: 0x17001898 RID: 6296
		// (get) Token: 0x06004C93 RID: 19603 RVA: 0x00112E5C File Offset: 0x0011105C
		public SongID ID
		{
			get
			{
				return this.m_id;
			}
		}

		// Token: 0x17001899 RID: 6297
		// (get) Token: 0x06004C94 RID: 19604 RVA: 0x00112E64 File Offset: 0x00111064
		public string EventRef
		{
			get
			{
				return this.m_eventRef;
			}
		}

		// Token: 0x0400407D RID: 16509
		[SerializeField]
		private SongID m_id;

		// Token: 0x0400407E RID: 16510
		[SerializeField]
		[EventRef]
		private string m_eventRef;
	}
}
