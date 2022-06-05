using System;

// Token: 0x02000C42 RID: 3138
public enum SummonRuleType
{
	// Token: 0x04004AA6 RID: 19110
	None,
	// Token: 0x04004AA7 RID: 19111
	SummonEnemies = 100,
	// Token: 0x04004AA8 RID: 19112
	SetSpawnPoints = 200,
	// Token: 0x04004AA9 RID: 19113
	SetSummonPool = 300,
	// Token: 0x04004AAA RID: 19114
	SetSummonPoolLevelMod = 400,
	// Token: 0x04004AAB RID: 19115
	SetSummonPoolDifficulty = 500,
	// Token: 0x04004AAC RID: 19116
	SaveCurrentEnemyHP = 580,
	// Token: 0x04004AAD RID: 19117
	WaitForXSeconds = 600,
	// Token: 0x04004AAE RID: 19118
	WaitUntilAllEnemiesDead = 700,
	// Token: 0x04004AAF RID: 19119
	WaitUntilXRemaining = 800,
	// Token: 0x04004AB0 RID: 19120
	WaitUntilChestCollected = 805,
	// Token: 0x04004AB1 RID: 19121
	WaitUntilEnemyHP = 810,
	// Token: 0x04004AB2 RID: 19122
	WaitUntilModeShift = 815,
	// Token: 0x04004AB3 RID: 19123
	TeleportPlayer = 850,
	// Token: 0x04004AB4 RID: 19124
	TogglePlayerInvincibility = 870,
	// Token: 0x04004AB5 RID: 19125
	StartArena = 900,
	// Token: 0x04004AB6 RID: 19126
	EndArena = 1000,
	// Token: 0x04004AB7 RID: 19127
	AwardHeirloom = 1100,
	// Token: 0x04004AB8 RID: 19128
	SpawnChest = 1105,
	// Token: 0x04004AB9 RID: 19129
	KillAllEnemies = 1110,
	// Token: 0x04004ABA RID: 19130
	SetEnemiesDefeated = 1115,
	// Token: 0x04004ABB RID: 19131
	SlowTime = 1120,
	// Token: 0x04004ABC RID: 19132
	PlayMusic = 1130,
	// Token: 0x04004ABD RID: 19133
	SetGlobalTimer = 1140,
	// Token: 0x04004ABE RID: 19134
	RunDialogue = 1200,
	// Token: 0x04004ABF RID: 19135
	DisplayObjectiveComplete = 1210,
	// Token: 0x04004AC0 RID: 19136
	WaitForBroadcast = 1300,
	// Token: 0x04004AC1 RID: 19137
	EndChallenge = 1400,
	// Token: 0x04004AC2 RID: 19138
	DebugTrace = 999999
}
