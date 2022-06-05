using System;
using UnityEngine;

// Token: 0x02000B74 RID: 2932
public class ClassPassiveData : ScriptableObject
{
	// Token: 0x040041D2 RID: 16850
	public string ClassName;

	// Token: 0x040041D3 RID: 16851
	public float MaxHPMod;

	// Token: 0x040041D4 RID: 16852
	public float MaxManaMod;

	// Token: 0x040041D5 RID: 16853
	public float VitalityMod;

	// Token: 0x040041D6 RID: 16854
	public float StrengthMod;

	// Token: 0x040041D7 RID: 16855
	public float IntelligenceMod;

	// Token: 0x040041D8 RID: 16856
	public float DexterityMod;

	// Token: 0x040041D9 RID: 16857
	public float FocusMod;

	// Token: 0x040041DA RID: 16858
	public float ArmorMod;

	// Token: 0x040041DB RID: 16859
	public float WeaponCritChanceAdd;

	// Token: 0x040041DC RID: 16860
	public float MagicCritChanceAdd;

	// Token: 0x040041DD RID: 16861
	public float WeaponCritDamageAdd;

	// Token: 0x040041DE RID: 16862
	public float MagicCritDamageAdd;

	// Token: 0x040041DF RID: 16863
	public string Special;

	// Token: 0x040041E0 RID: 16864
	public ManaRegenType ManaRegenType;

	// Token: 0x040041E1 RID: 16865
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x040041E2 RID: 16866
	public string Controls;

	// Token: 0x040041E3 RID: 16867
	public string Description;
}
