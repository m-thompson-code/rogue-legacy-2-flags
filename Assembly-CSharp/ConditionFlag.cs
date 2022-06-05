using System;

// Token: 0x0200071D RID: 1821
[Flags]
public enum ConditionFlag
{
	// Token: 0x04003366 RID: 13158
	None = 0,
	// Token: 0x04003367 RID: 13159
	Heirloom_Dash = 1,
	// Token: 0x04003368 RID: 13160
	Heirloom_Memory = 2,
	// Token: 0x04003369 RID: 13161
	Heirloom_DoubleJump = 4,
	// Token: 0x0400336A RID: 13162
	Heirloom_Downstrike = 8,
	// Token: 0x0400336B RID: 13163
	Heirloom_VoidDash = 16,
	// Token: 0x0400336C RID: 13164
	Insight_HeirloomSpinKick_Hidden_Discovered = 1024,
	// Token: 0x0400336D RID: 13165
	Insight_HeirloomDoubleJump_Hidden_Discovered = 2048,
	// Token: 0x0400336E RID: 13166
	Insight_CastleBoss_DoorOpened_Resolved = 32768,
	// Token: 0x0400336F RID: 13167
	Insight_CastleBoss_DoorOpened_Discovered = 65536,
	// Token: 0x04003370 RID: 13168
	BossDefeated_Castle = 1048576,
	// Token: 0x04003371 RID: 13169
	BossDefeated_Bridge = 2097152,
	// Token: 0x04003372 RID: 13170
	BossDefeated_Forest = 4194304,
	// Token: 0x04003373 RID: 13171
	BossDefeated_Study = 8388608,
	// Token: 0x04003374 RID: 13172
	BossDefeated_Tower = 16777216,
	// Token: 0x04003375 RID: 13173
	BossDefeated_Cave = 33554432,
	// Token: 0x04003376 RID: 13174
	BossDefeated_Final = 67108864,
	// Token: 0x04003377 RID: 13175
	BossDefeated_Garden = 134217728
}
