using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000910 RID: 2320
	public class RoomMusicOverrideController : MonoBehaviour
	{
		// Token: 0x17001881 RID: 6273
		// (get) Token: 0x06004C1D RID: 19485 RVA: 0x001117A7 File Offset: 0x0010F9A7
		public SongID Music
		{
			get
			{
				return this.m_music;
			}
		}

		// Token: 0x06004C1E RID: 19486 RVA: 0x001117AF File Offset: 0x0010F9AF
		public void SetOverride(SongID songID)
		{
			this.m_music = songID;
		}

		// Token: 0x04004013 RID: 16403
		[SerializeField]
		private SongID m_music;
	}
}
