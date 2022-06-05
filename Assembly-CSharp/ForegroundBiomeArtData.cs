using System;
using Foreground;
using UnityEngine;

// Token: 0x02000B61 RID: 2913
[Serializable]
public class ForegroundBiomeArtData
{
	// Token: 0x04004166 RID: 16742
	public ForegroundLibrary Upper;

	// Token: 0x04004167 RID: 16743
	public ForegroundLibrary Lower;

	// Token: 0x04004168 RID: 16744
	[Range(0f, 100f)]
	public int UpperSpawnOdds = 50;

	// Token: 0x04004169 RID: 16745
	[Range(0f, 100f)]
	public int BottomSpawnOdds = 50;

	// Token: 0x0400416A RID: 16746
	public Material ForegroundSpriteMaterial;
}
