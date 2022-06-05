using System;
using UnityEngine;

// Token: 0x02000B7D RID: 2941
public class EquipmentData : ScriptableObject
{
	// Token: 0x0400423C RID: 16956
	public string Name;

	// Token: 0x0400423D RID: 16957
	public int BlacksmithUIIndex;

	// Token: 0x0400423E RID: 16958
	public EquipmentCategoryType CategoryType;

	// Token: 0x0400423F RID: 16959
	public EquipmentType EquipmentType;

	// Token: 0x04004240 RID: 16960
	public int ChestLevelRequirement;

	// Token: 0x04004241 RID: 16961
	public int ChestRarityRequirement;

	// Token: 0x04004242 RID: 16962
	public int GoldCost;

	// Token: 0x04004243 RID: 16963
	public int OreCost;

	// Token: 0x04004244 RID: 16964
	public bool Disabled;

	// Token: 0x04004245 RID: 16965
	public int BaseWeight;

	// Token: 0x04004246 RID: 16966
	public int BaseHealth;

	// Token: 0x04004247 RID: 16967
	public int BaseMana;

	// Token: 0x04004248 RID: 16968
	public int BaseArmor;

	// Token: 0x04004249 RID: 16969
	public int BaseStrengthDamage;

	// Token: 0x0400424A RID: 16970
	public int BaseMagicDamage;

	// Token: 0x0400424B RID: 16971
	public float BaseStrengthCritChance;

	// Token: 0x0400424C RID: 16972
	public float BaseMagicCritChance;

	// Token: 0x0400424D RID: 16973
	public float BaseStrengthCritDamage;

	// Token: 0x0400424E RID: 16974
	public float BaseMagicCritDamage;

	// Token: 0x0400424F RID: 16975
	public int MaximumLevel;

	// Token: 0x04004250 RID: 16976
	public int ScalingItemLevel;

	// Token: 0x04004251 RID: 16977
	public int ScalingGoldCost;

	// Token: 0x04004252 RID: 16978
	public int ScalingOreCost;

	// Token: 0x04004253 RID: 16979
	public int ScalingWeight;

	// Token: 0x04004254 RID: 16980
	public int ScalingHealth;

	// Token: 0x04004255 RID: 16981
	public int ScalingMana;

	// Token: 0x04004256 RID: 16982
	public int ScalingArmor;

	// Token: 0x04004257 RID: 16983
	public int ScalingStrengthDamage;

	// Token: 0x04004258 RID: 16984
	public int ScalingMagicDamage;

	// Token: 0x04004259 RID: 16985
	public float ScalingStrengthCritChance;

	// Token: 0x0400425A RID: 16986
	public float ScalingMagicCritChance;

	// Token: 0x0400425B RID: 16987
	public float ScalingStrengthCritDamage;

	// Token: 0x0400425C RID: 16988
	public float ScalingMagicCritDamage;

	// Token: 0x0400425D RID: 16989
	public int ScalingGoldSpike01;

	// Token: 0x0400425E RID: 16990
	public int ScalingOreSpike01;

	// Token: 0x0400425F RID: 16991
	public int ScalingGoldSpike02;

	// Token: 0x04004260 RID: 16992
	public int ScalingOreSpike02;

	// Token: 0x04004261 RID: 16993
	public int BaseEquipmentSetLevel;

	// Token: 0x04004262 RID: 16994
	public int ScalingEquipmentSetLevel;

	// Token: 0x04004263 RID: 16995
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04004264 RID: 16996
	public string Description;
}
