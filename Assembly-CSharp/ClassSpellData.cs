using System;
using UnityEngine;

// Token: 0x020006CA RID: 1738
public class ClassSpellData : ScriptableObject
{
	// Token: 0x04002F95 RID: 12181
	public string ClassName;

	// Token: 0x04002F96 RID: 12182
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04002F97 RID: 12183
	public string Controls;

	// Token: 0x04002F98 RID: 12184
	public string Description;

	// Token: 0x04002F99 RID: 12185
	public AbilityType[] SpellAbilityArray;
}
