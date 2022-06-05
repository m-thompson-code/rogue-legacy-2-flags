using System;
using UnityEngine;

// Token: 0x020006C6 RID: 1734
[CreateAssetMenu(menuName = "Custom/XML Processing Data/Burden")]
public class BurdenData : ScriptableObject
{
	// Token: 0x04002F63 RID: 12131
	public string Name;

	// Token: 0x04002F64 RID: 12132
	public int MaxBurdenLevel;

	// Token: 0x04002F65 RID: 12133
	public int InitialBurdenCost;

	// Token: 0x04002F66 RID: 12134
	public float ScalingBurdenCost;

	// Token: 0x04002F67 RID: 12135
	public bool Disabled;

	// Token: 0x04002F68 RID: 12136
	public float StatsGain;

	// Token: 0x04002F69 RID: 12137
	public string Title;

	// Token: 0x04002F6A RID: 12138
	public string Description;

	// Token: 0x04002F6B RID: 12139
	public string Description2;

	// Token: 0x04002F6C RID: 12140
	public string Hint;
}
