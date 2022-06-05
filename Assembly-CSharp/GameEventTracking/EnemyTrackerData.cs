using System;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x020008A8 RID: 2216
	[Serializable]
	public struct EnemyTrackerData : IGameEventData
	{
		// Token: 0x170017A7 RID: 6055
		// (get) Token: 0x0600484E RID: 18510 RVA: 0x00103F5E File Offset: 0x0010215E
		public BiomeType Biome
		{
			get
			{
				return this.m_biome;
			}
		}

		// Token: 0x170017A8 RID: 6056
		// (get) Token: 0x0600484F RID: 18511 RVA: 0x00103F66 File Offset: 0x00102166
		public int BiomeControllerIndex
		{
			get
			{
				return this.m_biomeControllerIndex;
			}
		}

		// Token: 0x170017A9 RID: 6057
		// (get) Token: 0x06004850 RID: 18512 RVA: 0x00103F6E File Offset: 0x0010216E
		public int EnemyIndex
		{
			get
			{
				return this.m_enemyIndex;
			}
		}

		// Token: 0x170017AA RID: 6058
		// (get) Token: 0x06004851 RID: 18513 RVA: 0x00103F76 File Offset: 0x00102176
		public EnemyType EnemyType
		{
			get
			{
				return this.m_enemyType;
			}
		}

		// Token: 0x170017AB RID: 6059
		// (get) Token: 0x06004852 RID: 18514 RVA: 0x00103F7E File Offset: 0x0010217E
		public EnemyRank EnemyRank
		{
			get
			{
				return this.m_enemyRank;
			}
		}

		// Token: 0x170017AC RID: 6060
		// (get) Token: 0x06004853 RID: 18515 RVA: 0x00103F86 File Offset: 0x00102186
		public float TimeStamp
		{
			get
			{
				return this.m_timeStamp;
			}
		}

		// Token: 0x170017AD RID: 6061
		// (get) Token: 0x06004854 RID: 18516 RVA: 0x00103F8E File Offset: 0x0010218E
		public int TimesLoaded
		{
			get
			{
				return this.m_timesLoaded;
			}
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x00103F98 File Offset: 0x00102198
		public EnemyTrackerData(BiomeType biome, int biomeControllerIndex, EnemyType enemyType, EnemyRank enemyRank, int enemyIndex)
		{
			this.m_biome = biome;
			this.m_biomeControllerIndex = biomeControllerIndex;
			this.m_enemyType = enemyType;
			this.m_enemyRank = enemyRank;
			this.m_enemyIndex = enemyIndex;
			this.m_timeStamp = (float)Time.frameCount;
			this.m_timesLoaded = SaveManager.StageSaveData.TimesTrackerWasLoaded;
		}

		// Token: 0x04003D0A RID: 15626
		private BiomeType m_biome;

		// Token: 0x04003D0B RID: 15627
		private int m_enemyIndex;

		// Token: 0x04003D0C RID: 15628
		private EnemyType m_enemyType;

		// Token: 0x04003D0D RID: 15629
		private EnemyRank m_enemyRank;

		// Token: 0x04003D0E RID: 15630
		private int m_biomeControllerIndex;

		// Token: 0x04003D0F RID: 15631
		private float m_timeStamp;

		// Token: 0x04003D10 RID: 15632
		private int m_timesLoaded;
	}
}
