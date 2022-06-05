using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000EA1 RID: 3745
	[Serializable]
	public class MusicLibraryEntry : AudioLibraryEntry
	{
		// Token: 0x17002194 RID: 8596
		// (get) Token: 0x06006998 RID: 27032 RVA: 0x0003AA20 File Offset: 0x00038C20
		public SongID[] MusicTracks
		{
			get
			{
				return this.m_musicTracks;
			}
		}

		// Token: 0x040055E8 RID: 21992
		[SerializeField]
		private SongID[] m_musicTracks;
	}
}
