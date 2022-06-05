using System;

// Token: 0x0200071A RID: 1818
[Flags]
public enum ProjectileCollisionFlag
{
	// Token: 0x04003356 RID: 13142
	None = 0,
	// Token: 0x04003357 RID: 13143
	ResonantBounceable = 1,
	// Token: 0x04003358 RID: 13144
	VoidDashable = 2,
	// Token: 0x04003359 RID: 13145
	WeaponStrikeable = 4,
	// Token: 0x0400335A RID: 13146
	ReflectWeak = 8,
	// Token: 0x0400335B RID: 13147
	ReflectStrong = 16,
	// Token: 0x0400335C RID: 13148
	Bounceable = 32,
	// Token: 0x0400335D RID: 13149
	All = -1
}
