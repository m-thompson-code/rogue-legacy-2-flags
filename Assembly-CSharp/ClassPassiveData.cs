using System;
using UnityEngine;

// Token: 0x020006C9 RID: 1737
public class ClassPassiveData : ScriptableObject
{
	// Token: 0x04002F83 RID: 12163
	public string ClassName;

	// Token: 0x04002F84 RID: 12164
	public float MaxHPMod;

	// Token: 0x04002F85 RID: 12165
	public float MaxManaMod;

	// Token: 0x04002F86 RID: 12166
	public float VitalityMod;

	// Token: 0x04002F87 RID: 12167
	public float StrengthMod;

	// Token: 0x04002F88 RID: 12168
	public float IntelligenceMod;

	// Token: 0x04002F89 RID: 12169
	public float DexterityMod;

	// Token: 0x04002F8A RID: 12170
	public float FocusMod;

	// Token: 0x04002F8B RID: 12171
	public float ArmorMod;

	// Token: 0x04002F8C RID: 12172
	public float WeaponCritChanceAdd;

	// Token: 0x04002F8D RID: 12173
	public float MagicCritChanceAdd;

	// Token: 0x04002F8E RID: 12174
	public float WeaponCritDamageAdd;

	// Token: 0x04002F8F RID: 12175
	public float MagicCritDamageAdd;

	// Token: 0x04002F90 RID: 12176
	public string Special;

	// Token: 0x04002F91 RID: 12177
	public ManaRegenType ManaRegenType;

	// Token: 0x04002F92 RID: 12178
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04002F93 RID: 12179
	public string Controls;

	// Token: 0x04002F94 RID: 12180
	public string Description;
}
