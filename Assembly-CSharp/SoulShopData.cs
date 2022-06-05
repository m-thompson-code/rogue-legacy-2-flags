using System;
using UnityEngine;

// Token: 0x02000B9B RID: 2971
public class SoulShopData : ScriptableObject
{
	// Token: 0x040043B8 RID: 17336
	public string Name;

	// Token: 0x040043B9 RID: 17337
	public bool Disabled;

	// Token: 0x040043BA RID: 17338
	public float FirstLevelStatGain;

	// Token: 0x040043BB RID: 17339
	public float AdditionalLevelStatGain;

	// Token: 0x040043BC RID: 17340
	public int Position;

	// Token: 0x040043BD RID: 17341
	public int BaseCost;

	// Token: 0x040043BE RID: 17342
	public int ScalingCost;

	// Token: 0x040043BF RID: 17343
	public int MaxLevel;

	// Token: 0x040043C0 RID: 17344
	public int UnlockLevel;

	// Token: 0x040043C1 RID: 17345
	public int OverloadMaxLevel;

	// Token: 0x040043C2 RID: 17346
	public int MaxLevelScalingCap;

	// Token: 0x040043C3 RID: 17347
	public int MaxSoulCostCap;

	// Token: 0x040043C4 RID: 17348
	public int TotalCostCapFalseOverloadFalse;

	// Token: 0x040043C5 RID: 17349
	public int TotalCostCapTrueOverloadFalse;

	// Token: 0x040043C6 RID: 17350
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x040043C7 RID: 17351
	public string Description;

	// Token: 0x040043C8 RID: 17352
	public string StatsTitle;
}
