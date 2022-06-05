using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E44 RID: 3652
	[Serializable]
	public class AudioLibrarySnapshotEntry
	{
		// Token: 0x17002101 RID: 8449
		// (get) Token: 0x060066FD RID: 26365 RVA: 0x00038B32 File Offset: 0x00036D32
		public string SnapshotPath
		{
			get
			{
				return this.m_snapshotPath;
			}
		}

		// Token: 0x17002102 RID: 8450
		// (get) Token: 0x060066FE RID: 26366 RVA: 0x00038B3A File Offset: 0x00036D3A
		public AudioSource Source
		{
			get
			{
				return this.m_source;
			}
		}

		// Token: 0x0400538A RID: 21386
		[SerializeField]
		[EventRef]
		private string m_snapshotPath;

		// Token: 0x0400538B RID: 21387
		[SerializeField]
		private AudioSource m_source;
	}
}
