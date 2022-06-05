using System;

// Token: 0x02000BD0 RID: 3024
[Flags]
public enum ConditionFlag
{
	// Token: 0x040045E1 RID: 17889
	None = 0,
	// Token: 0x040045E2 RID: 17890
	Heirloom_Dash = 1,
	// Token: 0x040045E3 RID: 17891
	Heirloom_Memory = 2,
	// Token: 0x040045E4 RID: 17892
	Heirloom_DoubleJump = 4,
	// Token: 0x040045E5 RID: 17893
	Heirloom_Downstrike = 8,
	// Token: 0x040045E6 RID: 17894
	Heirloom_VoidDash = 16,
	// Token: 0x040045E7 RID: 17895
	Insight_HeirloomSpinKick_Hidden_Discovered = 1024,
	// Token: 0x040045E8 RID: 17896
	Insight_HeirloomDoubleJump_Hidden_Discovered = 2048,
	// Token: 0x040045E9 RID: 17897
	Insight_CastleBoss_DoorOpened_Resolved = 32768,
	// Token: 0x040045EA RID: 17898
	Insight_CastleBoss_DoorOpened_Discovered = 65536,
	// Token: 0x040045EB RID: 17899
	BossDefeated_Castle = 1048576,
	// Token: 0x040045EC RID: 17900
	BossDefeated_Bridge = 2097152,
	// Token: 0x040045ED RID: 17901
	BossDefeated_Forest = 4194304,
	// Token: 0x040045EE RID: 17902
	BossDefeated_Study = 8388608,
	// Token: 0x040045EF RID: 17903
	BossDefeated_Tower = 16777216,
	// Token: 0x040045F0 RID: 17904
	BossDefeated_Cave = 33554432,
	// Token: 0x040045F1 RID: 17905
	BossDefeated_Final = 67108864,
	// Token: 0x040045F2 RID: 17906
	BossDefeated_Garden = 134217728
}
