using System;
using System.Collections.Generic;

namespace GameEventTracking
{
	// Token: 0x020008B4 RID: 2228
	[Serializable]
	public class EnemyPreviewEntry
	{
		// Token: 0x060048AD RID: 18605 RVA: 0x00104BE4 File Offset: 0x00102DE4
		public EnemyPreviewEntry(EnemyType enemyType)
		{
			this.Type = enemyType;
			this.RankEntries = new List<EnemyRankEntry>();
			foreach (object obj in Enum.GetValues(typeof(EnemyRank)))
			{
				EnemyRank enemyRank = (EnemyRank)obj;
				if (enemyRank != EnemyRank.Any && enemyRank != EnemyRank.None)
				{
					this.RankEntries.Add(new EnemyRankEntry(enemyRank));
				}
			}
		}

		// Token: 0x04003D4F RID: 15695
		public EnemyType Type;

		// Token: 0x04003D50 RID: 15696
		public List<EnemyRankEntry> RankEntries;
	}
}
