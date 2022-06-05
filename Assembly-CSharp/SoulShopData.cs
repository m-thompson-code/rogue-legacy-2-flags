using System;
using UnityEngine;

// Token: 0x020006ED RID: 1773
public class SoulShopData : ScriptableObject
{
	// Token: 0x04003166 RID: 12646
	public string Name;

	// Token: 0x04003167 RID: 12647
	public bool Disabled;

	// Token: 0x04003168 RID: 12648
	public float FirstLevelStatGain;

	// Token: 0x04003169 RID: 12649
	public float AdditionalLevelStatGain;

	// Token: 0x0400316A RID: 12650
	public int Position;

	// Token: 0x0400316B RID: 12651
	public int BaseCost;

	// Token: 0x0400316C RID: 12652
	public int ScalingCost;

	// Token: 0x0400316D RID: 12653
	public int MaxLevel;

	// Token: 0x0400316E RID: 12654
	public int UnlockLevel;

	// Token: 0x0400316F RID: 12655
	public int OverloadMaxLevel;

	// Token: 0x04003170 RID: 12656
	public int MaxLevelScalingCap;

	// Token: 0x04003171 RID: 12657
	public int MaxSoulCostCap;

	// Token: 0x04003172 RID: 12658
	public int TotalCostCapFalseOverloadFalse;

	// Token: 0x04003173 RID: 12659
	public int TotalCostCapTrueOverloadFalse;

	// Token: 0x04003174 RID: 12660
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04003175 RID: 12661
	public string Description;

	// Token: 0x04003176 RID: 12662
	public string StatsTitle;
}
