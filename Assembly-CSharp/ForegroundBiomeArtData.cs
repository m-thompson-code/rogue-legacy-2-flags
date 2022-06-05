using System;
using Foreground;
using UnityEngine;

// Token: 0x020006B6 RID: 1718
[Serializable]
public class ForegroundBiomeArtData
{
	// Token: 0x04002F17 RID: 12055
	public ForegroundLibrary Upper;

	// Token: 0x04002F18 RID: 12056
	public ForegroundLibrary Lower;

	// Token: 0x04002F19 RID: 12057
	[Range(0f, 100f)]
	public int UpperSpawnOdds = 50;

	// Token: 0x04002F1A RID: 12058
	[Range(0f, 100f)]
	public int BottomSpawnOdds = 50;

	// Token: 0x04002F1B RID: 12059
	public Material ForegroundSpriteMaterial;
}
