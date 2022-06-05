using System;
using UnityEngine;

// Token: 0x020006CC RID: 1740
public class ClassTalentData : ScriptableObject
{
	// Token: 0x04002FAA RID: 12202
	public string ClassName;

	// Token: 0x04002FAB RID: 12203
	public AbilityType[] TalentAbilityArray;

	// Token: 0x04002FAC RID: 12204
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04002FAD RID: 12205
	public string Controls;

	// Token: 0x04002FAE RID: 12206
	public string Description;
}
