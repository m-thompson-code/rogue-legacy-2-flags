using System;
using UnityEngine;

// Token: 0x02000B6E RID: 2926
public class BiomeData : ScriptableObject
{
	// Token: 0x04004192 RID: 16786
	[Header("Room Counts")]
	public int MinRoomCount;

	// Token: 0x04004193 RID: 16787
	public int MaxRoomCount;

	// Token: 0x04004194 RID: 16788
	[Space]
	public int MinFairyRoomCount;

	// Token: 0x04004195 RID: 16789
	public int MaxFairyRoomCount;

	// Token: 0x04004196 RID: 16790
	[Space]
	public int MinBonusRoomCount;

	// Token: 0x04004197 RID: 16791
	public int MaxBonusRoomCount;

	// Token: 0x04004198 RID: 16792
	[Space]
	public BonusRoomWeightEntry[] BonusRoomWeights;

	// Token: 0x04004199 RID: 16793
	[Space]
	public int MinTrapRoomCount;

	// Token: 0x0400419A RID: 16794
	public int MaxTrapRoomCount;

	// Token: 0x0400419B RID: 16795
	[Space]
	[Header("Enemies")]
	public float RoomLevelScale;

	// Token: 0x0400419C RID: 16796
	[Space]
	public int EnemyStartLevel;

	// Token: 0x0400419D RID: 16797
	public int BossStartLevel;

	// Token: 0x0400419E RID: 16798
	[Space]
	[Header("Odds")]
	public int AttachTopOdds;

	// Token: 0x0400419F RID: 16799
	public int AttachBottomOdds;

	// Token: 0x040041A0 RID: 16800
	public int AttachLeftOdds;

	// Token: 0x040041A1 RID: 16801
	public int AttachRightOdds;

	// Token: 0x040041A2 RID: 16802
	[Space]
	public int MergeTopOdds;

	// Token: 0x040041A3 RID: 16803
	public int MergeBottomOdds;

	// Token: 0x040041A4 RID: 16804
	public int MergeLeftOdds;

	// Token: 0x040041A5 RID: 16805
	public int MergeRightOdds;

	// Token: 0x040041A6 RID: 16806
	[Space]
	public int MaxMergeVerticalCount;

	// Token: 0x040041A7 RID: 16807
	public int MaxMergeHorizontalCount;

	// Token: 0x040041A8 RID: 16808
	[Space]
	[Header("Connection")]
	public BiomeType ConnectsTo;

	// Token: 0x040041A9 RID: 16809
	public RoomSide ConnectDirection;

	// Token: 0x040041AA RID: 16810
	[Space]
	[Header("Localization")]
	public string BiomeNameLocID;
}
