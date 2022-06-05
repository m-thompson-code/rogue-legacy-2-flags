using System;
using UnityEngine;

// Token: 0x02000B8F RID: 2959
public class RelicData : ScriptableObject
{
	// Token: 0x0400436B RID: 17259
	public string Name;

	// Token: 0x0400436C RID: 17260
	public int Rarity;

	// Token: 0x0400436D RID: 17261
	public int MaxStack;

	// Token: 0x0400436E RID: 17262
	public RelicCostType CostType;

	// Token: 0x0400436F RID: 17263
	public float CostAmount;

	// Token: 0x04004370 RID: 17264
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04004371 RID: 17265
	public string Description;

	// Token: 0x04004372 RID: 17266
	public string Description02;
}
