using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008D6 RID: 2262
	[Serializable]
	public class AudioLibrarySnapshotEntry
	{
		// Token: 0x17001822 RID: 6178
		// (get) Token: 0x06004A4D RID: 19021 RVA: 0x0010BB9E File Offset: 0x00109D9E
		public string SnapshotPath
		{
			get
			{
				return this.m_snapshotPath;
			}
		}

		// Token: 0x17001823 RID: 6179
		// (get) Token: 0x06004A4E RID: 19022 RVA: 0x0010BBA6 File Offset: 0x00109DA6
		public AudioSource Source
		{
			get
			{
				return this.m_source;
			}
		}

		// Token: 0x04003E75 RID: 15989
		[SerializeField]
		[EventRef]
		private string m_snapshotPath;

		// Token: 0x04003E76 RID: 15990
		[SerializeField]
		private AudioSource m_source;
	}
}
