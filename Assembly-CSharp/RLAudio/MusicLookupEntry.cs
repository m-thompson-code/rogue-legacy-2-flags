using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000EA2 RID: 3746
	[Serializable]
	public class MusicLookupEntry
	{
		// Token: 0x17002195 RID: 8597
		// (get) Token: 0x0600699A RID: 27034 RVA: 0x0003AA28 File Offset: 0x00038C28
		public SongID ID
		{
			get
			{
				return this.m_id;
			}
		}

		// Token: 0x17002196 RID: 8598
		// (get) Token: 0x0600699B RID: 27035 RVA: 0x0003AA30 File Offset: 0x00038C30
		public string EventRef
		{
			get
			{
				return this.m_eventRef;
			}
		}

		// Token: 0x040055E9 RID: 21993
		[SerializeField]
		private SongID m_id;

		// Token: 0x040055EA RID: 21994
		[SerializeField]
		[EventRef]
		private string m_eventRef;
	}
}
