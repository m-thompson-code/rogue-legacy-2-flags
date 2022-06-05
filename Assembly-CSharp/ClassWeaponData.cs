using System;
using UnityEngine;

// Token: 0x02000B78 RID: 2936
public class ClassWeaponData : ScriptableObject
{
	// Token: 0x040041FE RID: 16894
	public string ClassName;

	// Token: 0x040041FF RID: 16895
	public AbilityType[] WeaponAbilityArray;

	// Token: 0x04004200 RID: 16896
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04004201 RID: 16897
	public string Controls;

	// Token: 0x04004202 RID: 16898
	public string Description;
}
