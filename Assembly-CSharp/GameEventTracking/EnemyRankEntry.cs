using System;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x020008B3 RID: 2227
	[Serializable]
	public class EnemyRankEntry
	{
		// Token: 0x060048AC RID: 18604 RVA: 0x00104BCD File Offset: 0x00102DCD
		public EnemyRankEntry(EnemyRank rank)
		{
			this.Rank = rank;
		}

		// Token: 0x04003D4D RID: 15693
		public EnemyRank Rank = EnemyRank.None;

		// Token: 0x04003D4E RID: 15694
		public Sprite PreviewImage;
	}
}
