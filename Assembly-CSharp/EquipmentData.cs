using System;
using UnityEngine;

// Token: 0x020006D2 RID: 1746
public class EquipmentData : ScriptableObject
{
	// Token: 0x04002FED RID: 12269
	public string Name;

	// Token: 0x04002FEE RID: 12270
	public int BlacksmithUIIndex;

	// Token: 0x04002FEF RID: 12271
	public EquipmentCategoryType CategoryType;

	// Token: 0x04002FF0 RID: 12272
	public EquipmentType EquipmentType;

	// Token: 0x04002FF1 RID: 12273
	public int ChestLevelRequirement;

	// Token: 0x04002FF2 RID: 12274
	public int ChestRarityRequirement;

	// Token: 0x04002FF3 RID: 12275
	public int GoldCost;

	// Token: 0x04002FF4 RID: 12276
	public int OreCost;

	// Token: 0x04002FF5 RID: 12277
	public bool Disabled;

	// Token: 0x04002FF6 RID: 12278
	public int BaseWeight;

	// Token: 0x04002FF7 RID: 12279
	public int BaseHealth;

	// Token: 0x04002FF8 RID: 12280
	public int BaseMana;

	// Token: 0x04002FF9 RID: 12281
	public int BaseArmor;

	// Token: 0x04002FFA RID: 12282
	public int BaseStrengthDamage;

	// Token: 0x04002FFB RID: 12283
	public int BaseMagicDamage;

	// Token: 0x04002FFC RID: 12284
	public float BaseStrengthCritChance;

	// Token: 0x04002FFD RID: 12285
	public float BaseMagicCritChance;

	// Token: 0x04002FFE RID: 12286
	public float BaseStrengthCritDamage;

	// Token: 0x04002FFF RID: 12287
	public float BaseMagicCritDamage;

	// Token: 0x04003000 RID: 12288
	public int MaximumLevel;

	// Token: 0x04003001 RID: 12289
	public int ScalingItemLevel;

	// Token: 0x04003002 RID: 12290
	public int ScalingGoldCost;

	// Token: 0x04003003 RID: 12291
	public int ScalingOreCost;

	// Token: 0x04003004 RID: 12292
	public int ScalingWeight;

	// Token: 0x04003005 RID: 12293
	public int ScalingHealth;

	// Token: 0x04003006 RID: 12294
	public int ScalingMana;

	// Token: 0x04003007 RID: 12295
	public int ScalingArmor;

	// Token: 0x04003008 RID: 12296
	public int ScalingStrengthDamage;

	// Token: 0x04003009 RID: 12297
	public int ScalingMagicDamage;

	// Token: 0x0400300A RID: 12298
	public float ScalingStrengthCritChance;

	// Token: 0x0400300B RID: 12299
	public float ScalingMagicCritChance;

	// Token: 0x0400300C RID: 12300
	public float ScalingStrengthCritDamage;

	// Token: 0x0400300D RID: 12301
	public float ScalingMagicCritDamage;

	// Token: 0x0400300E RID: 12302
	public int ScalingGoldSpike01;

	// Token: 0x0400300F RID: 12303
	public int ScalingOreSpike01;

	// Token: 0x04003010 RID: 12304
	public int ScalingGoldSpike02;

	// Token: 0x04003011 RID: 12305
	public int ScalingOreSpike02;

	// Token: 0x04003012 RID: 12306
	public int BaseEquipmentSetLevel;

	// Token: 0x04003013 RID: 12307
	public int ScalingEquipmentSetLevel;

	// Token: 0x04003014 RID: 12308
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04003015 RID: 12309
	public string Description;
}
