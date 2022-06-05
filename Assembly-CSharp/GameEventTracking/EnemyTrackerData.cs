using System;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x02000DDB RID: 3547
	[Serializable]
	public struct EnemyTrackerData : IGameEventData
	{
		// Token: 0x17002021 RID: 8225
		// (get) Token: 0x060063AE RID: 25518 RVA: 0x00036F3F File Offset: 0x0003513F
		public BiomeType Biome
		{
			get
			{
				return this.m_biome;
			}
		}

		// Token: 0x17002022 RID: 8226
		// (get) Token: 0x060063AF RID: 25519 RVA: 0x00036F47 File Offset: 0x00035147
		public int BiomeControllerIndex
		{
			get
			{
				return this.m_biomeControllerIndex;
			}
		}

		// Token: 0x17002023 RID: 8227
		// (get) Token: 0x060063B0 RID: 25520 RVA: 0x00036F4F File Offset: 0x0003514F
		public int EnemyIndex
		{
			get
			{
				return this.m_enemyIndex;
			}
		}

		// Token: 0x17002024 RID: 8228
		// (get) Token: 0x060063B1 RID: 25521 RVA: 0x00036F57 File Offset: 0x00035157
		public EnemyType EnemyType
		{
			get
			{
				return this.m_enemyType;
			}
		}

		// Token: 0x17002025 RID: 8229
		// (get) Token: 0x060063B2 RID: 25522 RVA: 0x00036F5F File Offset: 0x0003515F
		public EnemyRank EnemyRank
		{
			get
			{
				return this.m_enemyRank;
			}
		}

		// Token: 0x17002026 RID: 8230
		// (get) Token: 0x060063B3 RID: 25523 RVA: 0x00036F67 File Offset: 0x00035167
		public float TimeStamp
		{
			get
			{
				return this.m_timeStamp;
			}
		}

		// Token: 0x17002027 RID: 8231
		// (get) Token: 0x060063B4 RID: 25524 RVA: 0x00036F6F File Offset: 0x0003516F
		public int TimesLoaded
		{
			get
			{
				return this.m_timesLoaded;
			}
		}

		// Token: 0x060063B5 RID: 25525 RVA: 0x00172C08 File Offset: 0x00170E08
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

		// Token: 0x04005151 RID: 20817
		private BiomeType m_biome;

		// Token: 0x04005152 RID: 20818
		private int m_enemyIndex;

		// Token: 0x04005153 RID: 20819
		private EnemyType m_enemyType;

		// Token: 0x04005154 RID: 20820
		private EnemyRank m_enemyRank;

		// Token: 0x04005155 RID: 20821
		private int m_biomeControllerIndex;

		// Token: 0x04005156 RID: 20822
		private float m_timeStamp;

		// Token: 0x04005157 RID: 20823
		private int m_timesLoaded;
	}
}
