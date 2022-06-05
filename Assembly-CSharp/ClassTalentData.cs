using System;
using UnityEngine;

// Token: 0x02000B77 RID: 2935
public class ClassTalentData : ScriptableObject
{
	// Token: 0x040041F9 RID: 16889
	public string ClassName;

	// Token: 0x040041FA RID: 16890
	public AbilityType[] TalentAbilityArray;

	// Token: 0x040041FB RID: 16891
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x040041FC RID: 16892
	public string Controls;

	// Token: 0x040041FD RID: 16893
	public string Description;
}
