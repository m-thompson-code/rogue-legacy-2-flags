using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008D5 RID: 2261
	[Serializable]
	public class AudioLibraryEventsEntry
	{
		// Token: 0x17001820 RID: 6176
		// (get) Token: 0x06004A4A RID: 19018 RVA: 0x0010BB86 File Offset: 0x00109D86
		public string[] EventPaths
		{
			get
			{
				return this.m_eventPaths;
			}
		}

		// Token: 0x17001821 RID: 6177
		// (get) Token: 0x06004A4B RID: 19019 RVA: 0x0010BB8E File Offset: 0x00109D8E
		public AudioSource Source
		{
			get
			{
				return this.m_source;
			}
		}

		// Token: 0x04003E73 RID: 15987
		[SerializeField]
		[EventRef]
		private string[] m_eventPaths;

		// Token: 0x04003E74 RID: 15988
		[SerializeField]
		private AudioSource m_source;
	}
}
