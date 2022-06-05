using System;
using UnityEngine;

// Token: 0x020006E4 RID: 1764
public class RelicData : ScriptableObject
{
	// Token: 0x0400311C RID: 12572
	public string Name;

	// Token: 0x0400311D RID: 12573
	public int Rarity;

	// Token: 0x0400311E RID: 12574
	public int MaxStack;

	// Token: 0x0400311F RID: 12575
	public RelicCostType CostType;

	// Token: 0x04003120 RID: 12576
	public float CostAmount;

	// Token: 0x04003121 RID: 12577
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04003122 RID: 12578
	public string Description;

	// Token: 0x04003123 RID: 12579
	public string Description02;
}
