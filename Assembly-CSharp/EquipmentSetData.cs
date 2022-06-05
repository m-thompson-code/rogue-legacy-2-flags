using System;
using UnityEngine;

// Token: 0x02000B81 RID: 2945
public class EquipmentSetData : ScriptableObject
{
	// Token: 0x0400428A RID: 17034
	public string Name;

	// Token: 0x0400428B RID: 17035
	[Space]
	[Header("Bonus(es)")]
	[Space]
	public EquipmentSetBonus SetBonus01;

	// Token: 0x0400428C RID: 17036
	public int SetRequirement01;

	// Token: 0x0400428D RID: 17037
	public EquipmentSetBonus SetBonus02;

	// Token: 0x0400428E RID: 17038
	public int SetRequirement02;

	// Token: 0x0400428F RID: 17039
	public EquipmentSetBonus SetBonus03;

	// Token: 0x04004290 RID: 17040
	public int SetRequirement03;

	// Token: 0x04004291 RID: 17041
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04004292 RID: 17042
	public string Description;
}
