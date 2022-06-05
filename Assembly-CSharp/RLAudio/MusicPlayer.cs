using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008FC RID: 2300
	public class MusicPlayer : MonoBehaviour
	{
		// Token: 0x06004B92 RID: 19346 RVA: 0x0010FB43 File Offset: 0x0010DD43
		private void Start()
		{
			MusicManager.PlayMusic(this.m_song, false, false);
		}

		// Token: 0x06004B93 RID: 19347 RVA: 0x0010FB52 File Offset: 0x0010DD52
		private void OnDestroy()
		{
			if (this.m_stopOnDestroy)
			{
				this.Stop();
			}
		}

		// Token: 0x06004B94 RID: 19348 RVA: 0x0010FB62 File Offset: 0x0010DD62
		public void Stop()
		{
			MusicManager.StopMusic();
		}

		// Token: 0x04003F90 RID: 16272
		[SerializeField]
		private SongID m_song;

		// Token: 0x04003F91 RID: 16273
		[SerializeField]
		private bool m_stopOnDestroy = true;
	}
}
