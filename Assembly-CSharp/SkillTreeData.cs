using System;
using UnityEngine;

// Token: 0x02000B9A RID: 2970
public class SkillTreeData : ScriptableObject
{
	// Token: 0x040043A8 RID: 17320
	public string Name;

	// Token: 0x040043A9 RID: 17321
	public float FirstLevelStatGain;

	// Token: 0x040043AA RID: 17322
	public float AdditionalLevelStatGain;

	// Token: 0x040043AB RID: 17323
	public int BaseCost;

	// Token: 0x040043AC RID: 17324
	public int Appreciation;

	// Token: 0x040043AD RID: 17325
	public int MaxLevel;

	// Token: 0x040043AE RID: 17326
	public int OverloadLevelCap;

	// Token: 0x040043AF RID: 17327
	public int AdditiveSoulShopLevels;

	// Token: 0x040043B0 RID: 17328
	public bool DisplayBaseStat;

	// Token: 0x040043B1 RID: 17329
	public SkillUnlockState SkillUnlockState;

	// Token: 0x040043B2 RID: 17330
	public int SkillUnlockLevel;

	// Token: 0x040043B3 RID: 17331
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x040043B4 RID: 17332
	public string UnitOfMeasurement;

	// Token: 0x040043B5 RID: 17333
	public string StatTitle;

	// Token: 0x040043B6 RID: 17334
	public string Description;

	// Token: 0x040043B7 RID: 17335
	public string SoulShopTag;
}
