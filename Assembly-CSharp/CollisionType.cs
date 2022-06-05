using System;

// Token: 0x02000719 RID: 1817
[Flags]
public enum CollisionType
{
	// Token: 0x04003344 RID: 13124
	None = 0,
	// Token: 0x04003345 RID: 13125
	Player = 1,
	// Token: 0x04003346 RID: 13126
	PlayerProjectile = 2,
	// Token: 0x04003347 RID: 13127
	Enemy = 4,
	// Token: 0x04003348 RID: 13128
	EnemyProjectile = 8,
	// Token: 0x04003349 RID: 13129
	ItemDrop = 16,
	// Token: 0x0400334A RID: 13130
	Breakable = 32,
	// Token: 0x0400334B RID: 13131
	Hazard = 64,
	// Token: 0x0400334C RID: 13132
	Platform = 128,
	// Token: 0x0400334D RID: 13133
	Chest = 256,
	// Token: 0x0400334E RID: 13134
	NPC = 512,
	// Token: 0x0400334F RID: 13135
	TriggerHazard = 1024,
	// Token: 0x04003350 RID: 13136
	Player_Dodging = 2048,
	// Token: 0x04003351 RID: 13137
	FlimsyBreakable = 16384,
	// Token: 0x04003352 RID: 13138
	Generic_Bounceable = 32768,
	// Token: 0x04003353 RID: 13139
	NonResonant_Bounceable = 65536,
	// Token: 0x04003354 RID: 13140
	All = -1
}
