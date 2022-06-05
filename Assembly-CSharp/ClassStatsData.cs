using System;
using UnityEngine;

// Token: 0x020006CB RID: 1739
public class ClassStatsData : ScriptableObject
{
	// Token: 0x04002F9A RID: 12186
	public string ClassName;

	// Token: 0x04002F9B RID: 12187
	public float HealthMod;

	// Token: 0x04002F9C RID: 12188
	public float ManaMod;

	// Token: 0x04002F9D RID: 12189
	public float StrengthDamageMod;

	// Token: 0x04002F9E RID: 12190
	public float MagicDamageMod;

	// Token: 0x04002F9F RID: 12191
	public float StrengthCritChanceMod;

	// Token: 0x04002FA0 RID: 12192
	public float MagicCritChanceMod;

	// Token: 0x04002FA1 RID: 12193
	public float CooldownMod;

	// Token: 0x04002FA2 RID: 12194
	public float ManaRegenTimeMod;

	// Token: 0x04002FA3 RID: 12195
	public float ManaRegenDamageMod;

	// Token: 0x04002FA4 RID: 12196
	public float ManaRegenKillMod;

	// Token: 0x04002FA5 RID: 12197
	public float ArmorEquipMod;

	// Token: 0x04002FA6 RID: 12198
	public float RuneEquipMod;

	// Token: 0x04002FA7 RID: 12199
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04002FA8 RID: 12200
	public string Controls;

	// Token: 0x04002FA9 RID: 12201
	public string Description;
}
