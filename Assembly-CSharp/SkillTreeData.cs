using System;
using UnityEngine;

// Token: 0x020006EC RID: 1772
public class SkillTreeData : ScriptableObject
{
	// Token: 0x04003156 RID: 12630
	public string Name;

	// Token: 0x04003157 RID: 12631
	public float FirstLevelStatGain;

	// Token: 0x04003158 RID: 12632
	public float AdditionalLevelStatGain;

	// Token: 0x04003159 RID: 12633
	public int BaseCost;

	// Token: 0x0400315A RID: 12634
	public int Appreciation;

	// Token: 0x0400315B RID: 12635
	public int MaxLevel;

	// Token: 0x0400315C RID: 12636
	public int OverloadLevelCap;

	// Token: 0x0400315D RID: 12637
	public int AdditiveSoulShopLevels;

	// Token: 0x0400315E RID: 12638
	public bool DisplayBaseStat;

	// Token: 0x0400315F RID: 12639
	public SkillUnlockState SkillUnlockState;

	// Token: 0x04003160 RID: 12640
	public int SkillUnlockLevel;

	// Token: 0x04003161 RID: 12641
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04003162 RID: 12642
	public string UnitOfMeasurement;

	// Token: 0x04003163 RID: 12643
	public string StatTitle;

	// Token: 0x04003164 RID: 12644
	public string Description;

	// Token: 0x04003165 RID: 12645
	public string SoulShopTag;
}
