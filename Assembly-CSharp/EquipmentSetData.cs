using System;
using UnityEngine;

// Token: 0x020006D6 RID: 1750
public class EquipmentSetData : ScriptableObject
{
	// Token: 0x0400303B RID: 12347
	public string Name;

	// Token: 0x0400303C RID: 12348
	[Space]
	[Header("Bonus(es)")]
	[Space]
	public EquipmentSetBonus SetBonus01;

	// Token: 0x0400303D RID: 12349
	public int SetRequirement01;

	// Token: 0x0400303E RID: 12350
	public EquipmentSetBonus SetBonus02;

	// Token: 0x0400303F RID: 12351
	public int SetRequirement02;

	// Token: 0x04003040 RID: 12352
	public EquipmentSetBonus SetBonus03;

	// Token: 0x04003041 RID: 12353
	public int SetRequirement03;

	// Token: 0x04003042 RID: 12354
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04003043 RID: 12355
	public string Description;
}
