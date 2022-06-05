using System;
using System.Collections.Generic;

namespace GameEventTracking
{
	// Token: 0x02000DEB RID: 3563
	[Serializable]
	public class EnemyPreviewEntry
	{
		// Token: 0x0600642D RID: 25645 RVA: 0x00173A14 File Offset: 0x00171C14
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

		// Token: 0x040051A9 RID: 20905
		public EnemyType Type;

		// Token: 0x040051AA RID: 20906
		public List<EnemyRankEntry> RankEntries;
	}
}
