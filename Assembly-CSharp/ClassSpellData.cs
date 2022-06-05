using System;
using UnityEngine;

// Token: 0x02000B75 RID: 2933
public class ClassSpellData : ScriptableObject
{
	// Token: 0x040041E4 RID: 16868
	public string ClassName;

	// Token: 0x040041E5 RID: 16869
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x040041E6 RID: 16870
	public string Controls;

	// Token: 0x040041E7 RID: 16871
	public string Description;

	// Token: 0x040041E8 RID: 16872
	public AbilityType[] SpellAbilityArray;
}
