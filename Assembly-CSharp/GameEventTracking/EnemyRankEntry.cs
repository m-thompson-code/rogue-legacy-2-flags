using System;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x02000DEA RID: 3562
	[Serializable]
	public class EnemyRankEntry
	{
		// Token: 0x0600642C RID: 25644 RVA: 0x0003759E File Offset: 0x0003579E
		public EnemyRankEntry(EnemyRank rank)
		{
			this.Rank = rank;
		}

		// Token: 0x040051A7 RID: 20903
		public EnemyRank Rank = EnemyRank.None;

		// Token: 0x040051A8 RID: 20904
		public Sprite PreviewImage;
	}
}
