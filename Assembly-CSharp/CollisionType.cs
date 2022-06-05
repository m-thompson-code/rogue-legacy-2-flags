using System;

// Token: 0x02000BCC RID: 3020
[Flags]
public enum CollisionType
{
	// Token: 0x040045BF RID: 17855
	None = 0,
	// Token: 0x040045C0 RID: 17856
	Player = 1,
	// Token: 0x040045C1 RID: 17857
	PlayerProjectile = 2,
	// Token: 0x040045C2 RID: 17858
	Enemy = 4,
	// Token: 0x040045C3 RID: 17859
	EnemyProjectile = 8,
	// Token: 0x040045C4 RID: 17860
	ItemDrop = 16,
	// Token: 0x040045C5 RID: 17861
	Breakable = 32,
	// Token: 0x040045C6 RID: 17862
	Hazard = 64,
	// Token: 0x040045C7 RID: 17863
	Platform = 128,
	// Token: 0x040045C8 RID: 17864
	Chest = 256,
	// Token: 0x040045C9 RID: 17865
	NPC = 512,
	// Token: 0x040045CA RID: 17866
	TriggerHazard = 1024,
	// Token: 0x040045CB RID: 17867
	Player_Dodging = 2048,
	// Token: 0x040045CC RID: 17868
	FlimsyBreakable = 16384,
	// Token: 0x040045CD RID: 17869
	Generic_Bounceable = 32768,
	// Token: 0x040045CE RID: 17870
	NonResonant_Bounceable = 65536,
	// Token: 0x040045CF RID: 17871
	All = -1
}
