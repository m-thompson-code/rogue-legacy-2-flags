using System;
using UnityEngine;

// Token: 0x02000B76 RID: 2934
public class ClassStatsData : ScriptableObject
{
	// Token: 0x040041E9 RID: 16873
	public string ClassName;

	// Token: 0x040041EA RID: 16874
	public float HealthMod;

	// Token: 0x040041EB RID: 16875
	public float ManaMod;

	// Token: 0x040041EC RID: 16876
	public float StrengthDamageMod;

	// Token: 0x040041ED RID: 16877
	public float MagicDamageMod;

	// Token: 0x040041EE RID: 16878
	public float StrengthCritChanceMod;

	// Token: 0x040041EF RID: 16879
	public float MagicCritChanceMod;

	// Token: 0x040041F0 RID: 16880
	public float CooldownMod;

	// Token: 0x040041F1 RID: 16881
	public float ManaRegenTimeMod;

	// Token: 0x040041F2 RID: 16882
	public float ManaRegenDamageMod;

	// Token: 0x040041F3 RID: 16883
	public float ManaRegenKillMod;

	// Token: 0x040041F4 RID: 16884
	public float ArmorEquipMod;

	// Token: 0x040041F5 RID: 16885
	public float RuneEquipMod;

	// Token: 0x040041F6 RID: 16886
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x040041F7 RID: 16887
	public string Controls;

	// Token: 0x040041F8 RID: 16888
	public string Description;
}
