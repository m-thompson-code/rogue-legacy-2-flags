using System;
using UnityEngine;

// Token: 0x02000B71 RID: 2929
[CreateAssetMenu(menuName = "Custom/XML Processing Data/Burden")]
public class BurdenData : ScriptableObject
{
	// Token: 0x040041B2 RID: 16818
	public string Name;

	// Token: 0x040041B3 RID: 16819
	public int MaxBurdenLevel;

	// Token: 0x040041B4 RID: 16820
	public int InitialBurdenCost;

	// Token: 0x040041B5 RID: 16821
	public float ScalingBurdenCost;

	// Token: 0x040041B6 RID: 16822
	public bool Disabled;

	// Token: 0x040041B7 RID: 16823
	public float StatsGain;

	// Token: 0x040041B8 RID: 16824
	public string Title;

	// Token: 0x040041B9 RID: 16825
	public string Description;

	// Token: 0x040041BA RID: 16826
	public string Description2;

	// Token: 0x040041BB RID: 16827
	public string Hint;
}
