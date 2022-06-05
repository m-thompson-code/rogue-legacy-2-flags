using System;
using UnityEngine;

// Token: 0x020006E1 RID: 1761
public class ProjectileData : ScriptableObject
{
	// Token: 0x040030FD RID: 12541
	public string Name;

	// Token: 0x040030FE RID: 12542
	public DamageType DamageType;

	// Token: 0x040030FF RID: 12543
	public float StrengthScale;

	// Token: 0x04003100 RID: 12544
	public float MagicScale;

	// Token: 0x04003101 RID: 12545
	public float CooldownReductionPerHit;

	// Token: 0x04003102 RID: 12546
	public float ManaGainPerHit;

	// Token: 0x04003103 RID: 12547
	public float StunStrength;

	// Token: 0x04003104 RID: 12548
	public float KnockbackStrength;

	// Token: 0x04003105 RID: 12549
	public float KnockbackModX;

	// Token: 0x04003106 RID: 12550
	public float KnockbackModY;

	// Token: 0x04003107 RID: 12551
	public float Speed;

	// Token: 0x04003108 RID: 12552
	public float TurnSpeed;

	// Token: 0x04003109 RID: 12553
	public float LifeSpan;

	// Token: 0x0400310A RID: 12554
	public float RepeatHitCheckTimer;

	// Token: 0x0400310B RID: 12555
	public string Title;

	// Token: 0x0400310C RID: 12556
	public string Controls;

	// Token: 0x0400310D RID: 12557
	public string Description;
}
