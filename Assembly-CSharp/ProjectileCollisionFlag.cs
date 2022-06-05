using System;

// Token: 0x02000BCD RID: 3021
[Flags]
public enum ProjectileCollisionFlag
{
	// Token: 0x040045D1 RID: 17873
	None = 0,
	// Token: 0x040045D2 RID: 17874
	ResonantBounceable = 1,
	// Token: 0x040045D3 RID: 17875
	VoidDashable = 2,
	// Token: 0x040045D4 RID: 17876
	WeaponStrikeable = 4,
	// Token: 0x040045D5 RID: 17877
	ReflectWeak = 8,
	// Token: 0x040045D6 RID: 17878
	ReflectStrong = 16,
	// Token: 0x040045D7 RID: 17879
	Bounceable = 32,
	// Token: 0x040045D8 RID: 17880
	All = -1
}
