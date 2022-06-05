using System;
using UnityEngine;

// Token: 0x020006C3 RID: 1731
public class BiomeData : ScriptableObject
{
	// Token: 0x04002F43 RID: 12099
	[Header("Room Counts")]
	public int MinRoomCount;

	// Token: 0x04002F44 RID: 12100
	public int MaxRoomCount;

	// Token: 0x04002F45 RID: 12101
	[Space]
	public int MinFairyRoomCount;

	// Token: 0x04002F46 RID: 12102
	public int MaxFairyRoomCount;

	// Token: 0x04002F47 RID: 12103
	[Space]
	public int MinBonusRoomCount;

	// Token: 0x04002F48 RID: 12104
	public int MaxBonusRoomCount;

	// Token: 0x04002F49 RID: 12105
	[Space]
	public BonusRoomWeightEntry[] BonusRoomWeights;

	// Token: 0x04002F4A RID: 12106
	[Space]
	public int MinTrapRoomCount;

	// Token: 0x04002F4B RID: 12107
	public int MaxTrapRoomCount;

	// Token: 0x04002F4C RID: 12108
	[Space]
	[Header("Enemies")]
	public float RoomLevelScale;

	// Token: 0x04002F4D RID: 12109
	[Space]
	public int EnemyStartLevel;

	// Token: 0x04002F4E RID: 12110
	public int BossStartLevel;

	// Token: 0x04002F4F RID: 12111
	[Space]
	[Header("Odds")]
	public int AttachTopOdds;

	// Token: 0x04002F50 RID: 12112
	public int AttachBottomOdds;

	// Token: 0x04002F51 RID: 12113
	public int AttachLeftOdds;

	// Token: 0x04002F52 RID: 12114
	public int AttachRightOdds;

	// Token: 0x04002F53 RID: 12115
	[Space]
	public int MergeTopOdds;

	// Token: 0x04002F54 RID: 12116
	public int MergeBottomOdds;

	// Token: 0x04002F55 RID: 12117
	public int MergeLeftOdds;

	// Token: 0x04002F56 RID: 12118
	public int MergeRightOdds;

	// Token: 0x04002F57 RID: 12119
	[Space]
	public int MaxMergeVerticalCount;

	// Token: 0x04002F58 RID: 12120
	public int MaxMergeHorizontalCount;

	// Token: 0x04002F59 RID: 12121
	[Space]
	[Header("Connection")]
	public BiomeType ConnectsTo;

	// Token: 0x04002F5A RID: 12122
	public RoomSide ConnectDirection;

	// Token: 0x04002F5B RID: 12123
	[Space]
	[Header("Localization")]
	public string BiomeNameLocID;
}
