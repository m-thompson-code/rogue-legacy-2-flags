using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E76 RID: 3702
	public class MusicPlayer : MonoBehaviour
	{
		// Token: 0x0600687B RID: 26747 RVA: 0x00039D01 File Offset: 0x00037F01
		private void Start()
		{
			MusicManager.PlayMusic(this.m_song, false, false);
		}

		// Token: 0x0600687C RID: 26748 RVA: 0x00039D10 File Offset: 0x00037F10
		private void OnDestroy()
		{
			if (this.m_stopOnDestroy)
			{
				this.Stop();
			}
		}

		// Token: 0x0600687D RID: 26749 RVA: 0x00039D20 File Offset: 0x00037F20
		public void Stop()
		{
			MusicManager.StopMusic();
		}

		// Token: 0x040054E2 RID: 21730
		[SerializeField]
		private SongID m_song;

		// Token: 0x040054E3 RID: 21731
		[SerializeField]
		private bool m_stopOnDestroy = true;
	}
}
