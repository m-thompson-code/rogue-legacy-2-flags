using System;
using UnityEngine;

// Token: 0x020006CD RID: 1741
public class ClassWeaponData : ScriptableObject
{
	// Token: 0x04002FAF RID: 12207
	public string ClassName;

	// Token: 0x04002FB0 RID: 12208
	public AbilityType[] WeaponAbilityArray;

	// Token: 0x04002FB1 RID: 12209
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04002FB2 RID: 12210
	public string Controls;

	// Token: 0x04002FB3 RID: 12211
	public string Description;
}
