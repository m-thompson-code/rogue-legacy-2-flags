using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E43 RID: 3651
	[Serializable]
	public class AudioLibraryEventsEntry
	{
		// Token: 0x170020FF RID: 8447
		// (get) Token: 0x060066FA RID: 26362 RVA: 0x00038B22 File Offset: 0x00036D22
		public string[] EventPaths
		{
			get
			{
				return this.m_eventPaths;
			}
		}

		// Token: 0x17002100 RID: 8448
		// (get) Token: 0x060066FB RID: 26363 RVA: 0x00038B2A File Offset: 0x00036D2A
		public AudioSource Source
		{
			get
			{
				return this.m_source;
			}
		}

		// Token: 0x04005388 RID: 21384
		[SerializeField]
		[EventRef]
		private string[] m_eventPaths;

		// Token: 0x04005389 RID: 21385
		[SerializeField]
		private AudioSource m_source;
	}
}
