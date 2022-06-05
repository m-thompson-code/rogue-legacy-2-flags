using System;
using UnityEngine;

// Token: 0x020006EA RID: 1770
public class RuneData : ScriptableObject
{
	// Token: 0x0400313D RID: 12605
	public RuneType RuneType;

	// Token: 0x0400313E RID: 12606
	public int Location;

	// Token: 0x0400313F RID: 12607
	public int BaseItemLevel;

	// Token: 0x04003140 RID: 12608
	public int GoldCost;

	// Token: 0x04003141 RID: 12609
	public int BlackStoneCost;

	// Token: 0x04003142 RID: 12610
	public bool Disabled;

	// Token: 0x04003143 RID: 12611
	public int BaseWeight;

	// Token: 0x04003144 RID: 12612
	public float StatMod01;

	// Token: 0x04003145 RID: 12613
	public float StatMod02;

	// Token: 0x04003146 RID: 12614
	public float StatMod03;

	// Token: 0x04003147 RID: 12615
	public int MaximumLevel;

	// Token: 0x04003148 RID: 12616
	public int ScalingItemLevel;

	// Token: 0x04003149 RID: 12617
	public int ScalingGoldCost;

	// Token: 0x0400314A RID: 12618
	public int ScalingBlackStoneCost;

	// Token: 0x0400314B RID: 12619
	public int ScalingWeight;

	// Token: 0x0400314C RID: 12620
	public float ScalingStatMod01;

	// Token: 0x0400314D RID: 12621
	public float ScalingStatMod02;

	// Token: 0x0400314E RID: 12622
	public float ScalingStatMod03;

	// Token: 0x0400314F RID: 12623
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04003150 RID: 12624
	public string Description;

	// Token: 0x04003151 RID: 12625
	public string Controls;
}
