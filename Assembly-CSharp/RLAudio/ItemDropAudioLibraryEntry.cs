using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008F4 RID: 2292
	[Serializable]
	public class ItemDropAudioLibraryEntry : AudioLibraryEntry
	{
		// Token: 0x17001859 RID: 6233
		// (get) Token: 0x06004B4D RID: 19277 RVA: 0x0010EE36 File Offset: 0x0010D036
		public string SpawnSingleEventPath
		{
			get
			{
				return this.m_spawnSingleEventPath;
			}
		}

		// Token: 0x1700185A RID: 6234
		// (get) Token: 0x06004B4E RID: 19278 RVA: 0x0010EE3E File Offset: 0x0010D03E
		public string SpawnManyEventPath
		{
			get
			{
				return this.m_spawnManyEventPath;
			}
		}

		// Token: 0x1700185B RID: 6235
		// (get) Token: 0x06004B4F RID: 19279 RVA: 0x0010EE46 File Offset: 0x0010D046
		public string LandEventPath
		{
			get
			{
				return this.m_landEventPath;
			}
		}

		// Token: 0x1700185C RID: 6236
		// (get) Token: 0x06004B50 RID: 19280 RVA: 0x0010EE4E File Offset: 0x0010D04E
		public string CollectEventPath
		{
			get
			{
				return this.m_collectEventPath;
			}
		}

		// Token: 0x1700185D RID: 6237
		// (get) Token: 0x06004B51 RID: 19281 RVA: 0x0010EE56 File Offset: 0x0010D056
		public bool UseManyAudio
		{
			get
			{
				return this.m_useManyAudio;
			}
		}

		// Token: 0x04003F52 RID: 16210
		[SerializeField]
		[EventRef]
		private string m_spawnSingleEventPath;

		// Token: 0x04003F53 RID: 16211
		[SerializeField]
		private bool m_useManyAudio;

		// Token: 0x04003F54 RID: 16212
		[SerializeField]
		[EventRef]
		private string m_spawnManyEventPath;

		// Token: 0x04003F55 RID: 16213
		[SerializeField]
		[EventRef]
		private string m_landEventPath;

		// Token: 0x04003F56 RID: 16214
		[SerializeField]
		[EventRef]
		private string m_collectEventPath;
	}
}
