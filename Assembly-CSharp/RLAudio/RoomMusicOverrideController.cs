using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E8D RID: 3725
	public class RoomMusicOverrideController : MonoBehaviour
	{
		// Token: 0x1700217A RID: 8570
		// (get) Token: 0x06006918 RID: 26904 RVA: 0x0003A3B6 File Offset: 0x000385B6
		public SongID Music
		{
			get
			{
				return this.m_music;
			}
		}

		// Token: 0x06006919 RID: 26905 RVA: 0x0003A3BE File Offset: 0x000385BE
		public void SetOverride(SongID songID)
		{
			this.m_music = songID;
		}

		// Token: 0x04005573 RID: 21875
		[SerializeField]
		private SongID m_music;
	}
}
