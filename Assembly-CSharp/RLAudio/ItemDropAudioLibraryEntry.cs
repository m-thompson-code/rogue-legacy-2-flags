using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E6B RID: 3691
	[Serializable]
	public class ItemDropAudioLibraryEntry : AudioLibraryEntry
	{
		// Token: 0x17002146 RID: 8518
		// (get) Token: 0x06006824 RID: 26660 RVA: 0x000399E1 File Offset: 0x00037BE1
		public string SpawnSingleEventPath
		{
			get
			{
				return this.m_spawnSingleEventPath;
			}
		}

		// Token: 0x17002147 RID: 8519
		// (get) Token: 0x06006825 RID: 26661 RVA: 0x000399E9 File Offset: 0x00037BE9
		public string SpawnManyEventPath
		{
			get
			{
				return this.m_spawnManyEventPath;
			}
		}

		// Token: 0x17002148 RID: 8520
		// (get) Token: 0x06006826 RID: 26662 RVA: 0x000399F1 File Offset: 0x00037BF1
		public string LandEventPath
		{
			get
			{
				return this.m_landEventPath;
			}
		}

		// Token: 0x17002149 RID: 8521
		// (get) Token: 0x06006827 RID: 26663 RVA: 0x000399F9 File Offset: 0x00037BF9
		public string CollectEventPath
		{
			get
			{
				return this.m_collectEventPath;
			}
		}

		// Token: 0x1700214A RID: 8522
		// (get) Token: 0x06006828 RID: 26664 RVA: 0x00039A01 File Offset: 0x00037C01
		public bool UseManyAudio
		{
			get
			{
				return this.m_useManyAudio;
			}
		}

		// Token: 0x04005496 RID: 21654
		[SerializeField]
		[EventRef]
		private string m_spawnSingleEventPath;

		// Token: 0x04005497 RID: 21655
		[SerializeField]
		private bool m_useManyAudio;

		// Token: 0x04005498 RID: 21656
		[SerializeField]
		[EventRef]
		private string m_spawnManyEventPath;

		// Token: 0x04005499 RID: 21657
		[SerializeField]
		[EventRef]
		private string m_landEventPath;

		// Token: 0x0400549A RID: 21658
		[SerializeField]
		[EventRef]
		private string m_collectEventPath;
	}
}
