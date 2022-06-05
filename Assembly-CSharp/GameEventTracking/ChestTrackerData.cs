using System;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x02000DDE RID: 3550
	[Serializable]
	public struct ChestTrackerData : IGameEventData
	{
		// Token: 0x060063C2 RID: 25538 RVA: 0x00172CB0 File Offset: 0x00170EB0
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

		// Token: 0x17002032 RID: 8242
		// (get) Token: 0x060063C3 RID: 25539 RVA: 0x00036FFE File Offset: 0x000351FE
		public int ChestIndex
		{
			get
			{
				return this.m_chestIndex;
			}
		}

		// Token: 0x17002033 RID: 8243
		// (get) Token: 0x060063C4 RID: 25540 RVA: 0x00037006 File Offset: 0x00035206
		public BiomeType Biome
		{
			get
			{
				return this.m_biome;
			}
		}

		// Token: 0x17002034 RID: 8244
		// (get) Token: 0x060063C5 RID: 25541 RVA: 0x0003700E File Offset: 0x0003520E
		public int BiomeControllerIndex
		{
			get
			{
				return this.m_biomeControllerIndex;
			}
		}

		// Token: 0x17002035 RID: 8245
		// (get) Token: 0x060063C6 RID: 25542 RVA: 0x00037016 File Offset: 0x00035216
		public ChestType ChestType
		{
			get
			{
				return this.m_chestType;
			}
		}

		// Token: 0x17002036 RID: 8246
		// (get) Token: 0x060063C7 RID: 25543 RVA: 0x0003701E File Offset: 0x0003521E
		public bool ContainsGold
		{
			get
			{
				return this.m_containsGold;
			}
		}

		// Token: 0x17002037 RID: 8247
		// (get) Token: 0x060063C8 RID: 25544 RVA: 0x00037026 File Offset: 0x00035226
		public float TimeStamp
		{
			get
			{
				return this.m_timeStamp;
			}
		}

		// Token: 0x17002038 RID: 8248
		// (get) Token: 0x060063C9 RID: 25545 RVA: 0x0003702E File Offset: 0x0003522E
		public int TimesLoaded
		{
			get
			{
				return this.m_timesLoaded;
			}
		}

		// Token: 0x04005163 RID: 20835
		private float m_timeStamp;

		// Token: 0x04005164 RID: 20836
		private ChestType m_chestType;

		// Token: 0x04005165 RID: 20837
		private int m_timesLoaded;

		// Token: 0x04005166 RID: 20838
		private bool m_containsGold;

		// Token: 0x04005167 RID: 20839
		private BiomeType m_biome;

		// Token: 0x04005168 RID: 20840
		private int m_biomeControllerIndex;

		// Token: 0x04005169 RID: 20841
		private int m_chestIndex;
	}
}
