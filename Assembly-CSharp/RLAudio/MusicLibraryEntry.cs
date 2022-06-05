using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000922 RID: 2338
	[Serializable]
	public class MusicLibraryEntry : AudioLibraryEntry
	{
		// Token: 0x17001897 RID: 6295
		// (get) Token: 0x06004C91 RID: 19601 RVA: 0x00112E4C File Offset: 0x0011104C
		public SongID[] MusicTracks
		{
			get
			{
				return this.m_musicTracks;
			}
		}

		// Token: 0x0400407C RID: 16508
		[SerializeField]
		private SongID[] m_musicTracks;
	}
}
