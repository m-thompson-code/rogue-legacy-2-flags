using System;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x020008AB RID: 2219
	[Serializable]
	public struct ChestTrackerData : IGameEventData
	{
		// Token: 0x06004862 RID: 18530 RVA: 0x001040C8 File Offset: 0x001022C8
		public ChestTrackerData(ChestType chestType, bool containsGold, BiomeType biome, int biomeControllerIndex, int chestIndex)
		{
			this.m_chestType = chestType;
			this.m_containsGold = containsGold;
			this.m_timesLoaded = SaveManager.StageSaveData.TimesTrackerWasLoaded;
			this.m_timeStamp = (float)Time.frameCount;
			this.m_biome = biome;
			this.m_biomeControllerIndex = biomeControllerIndex;
			this.m_chestIndex = chestIndex;
		}

		// Token: 0x170017B8 RID: 6072
		// (get) Token: 0x06004863 RID: 18531 RVA: 0x00104116 File Offset: 0x00102316
		public int ChestIndex
		{
			get
			{
				return this.m_chestIndex;
			}
		}

		// Token: 0x170017B9 RID: 6073
		// (get) Token: 0x06004864 RID: 18532 RVA: 0x0010411E File Offset: 0x0010231E
		public BiomeType Biome
		{
			get
			{
				return this.m_biome;
			}
		}

		// Token: 0x170017BA RID: 6074
		// (get) Token: 0x06004865 RID: 18533 RVA: 0x00104126 File Offset: 0x00102326
		public int BiomeControllerIndex
		{
			get
			{
				return this.m_biomeControllerIndex;
			}
		}

		// Token: 0x170017BB RID: 6075
		// (get) Token: 0x06004866 RID: 18534 RVA: 0x0010412E File Offset: 0x0010232E
		public ChestType ChestType
		{
			get
			{
				return this.m_chestType;
			}
		}

		// Token: 0x170017BC RID: 6076
		// (get) Token: 0x06004867 RID: 18535 RVA: 0x00104136 File Offset: 0x00102336
		public bool ContainsGold
		{
			get
			{
				return this.m_containsGold;
			}
		}

		// Token: 0x170017BD RID: 6077
		// (get) Token: 0x06004868 RID: 18536 RVA: 0x0010413E File Offset: 0x0010233E
		public float TimeStamp
		{
			get
			{
				return this.m_timeStamp;
			}
		}

		// Token: 0x170017BE RID: 6078
		// (get) Token: 0x06004869 RID: 18537 RVA: 0x00104146 File Offset: 0x00102346
		public int TimesLoaded
		{
			get
			{
				return this.m_timesLoaded;
			}
		}

		// Token: 0x04003D1C RID: 15644
		private float m_timeStamp;

		// Token: 0x04003D1D RID: 15645
		private ChestType m_chestType;

		// Token: 0x04003D1E RID: 15646
		private int m_timesLoaded;

		// Token: 0x04003D1F RID: 15647
		private bool m_containsGold;

		// Token: 0x04003D20 RID: 15648
		private BiomeType m_biome;

		// Token: 0x04003D21 RID: 15649
		private int m_biomeControllerIndex;

		// Token: 0x04003D22 RID: 15650
		private int m_chestIndex;
	}
}
