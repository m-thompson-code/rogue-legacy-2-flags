using System;

// Token: 0x02000781 RID: 1921
public enum SummonRuleType
{
	// Token: 0x040037F6 RID: 14326
	None,
	// Token: 0x040037F7 RID: 14327
	SummonEnemies = 100,
	// Token: 0x040037F8 RID: 14328
	SetSpawnPoints = 200,
	// Token: 0x040037F9 RID: 14329
	SetSummonPool = 300,
	// Token: 0x040037FA RID: 14330
	SetSummonPoolLevelMod = 400,
	// Token: 0x040037FB RID: 14331
	SetSummonPoolDifficulty = 500,
	// Token: 0x040037FC RID: 14332
	SaveCurrentEnemyHP = 580,
	// Token: 0x040037FD RID: 14333
	WaitForXSeconds = 600,
	// Token: 0x040037FE RID: 14334
	WaitUntilAllEnemiesDead = 700,
	// Token: 0x040037FF RID: 14335
	WaitUntilXRemaining = 800,
	// Token: 0x04003800 RID: 14336
	WaitUntilChestCollected = 805,
	// Token: 0x04003801 RID: 14337
	WaitUntilEnemyHP = 810,
	// Token: 0x04003802 RID: 14338
	WaitUntilModeShift = 815,
	// Token: 0x04003803 RID: 14339
	TeleportPlayer = 850,
	// Token: 0x04003804 RID: 14340
	TogglePlayerInvincibility = 870,
	// Token: 0x04003805 RID: 14341
	StartArena = 900,
	// Token: 0x04003806 RID: 14342
	EndArena = 1000,
	// Token: 0x04003807 RID: 14343
	AwardHeirloom = 1100,
	// Token: 0x04003808 RID: 14344
	SpawnChest = 1105,
	// Token: 0x04003809 RID: 14345
	KillAllEnemies = 1110,
	// Token: 0x0400380A RID: 14346
	SetEnemiesDefeated = 1115,
	// Token: 0x0400380B RID: 14347
	SlowTime = 1120,
	// Token: 0x0400380C RID: 14348
	PlayMusic = 1130,
	// Token: 0x0400380D RID: 14349
	SetGlobalTimer = 1140,
	// Token: 0x0400380E RID: 14350
	RunDialogue = 1200,
	// Token: 0x0400380F RID: 14351
	DisplayObjectiveComplete = 1210,
	// Token: 0x04003810 RID: 14352
	WaitForBroadcast = 1300,
	// Token: 0x04003811 RID: 14353
	EndChallenge = 1400,
	// Token: 0x04003812 RID: 14354
	DebugTrace = 999999
}
