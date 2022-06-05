using System;
using UnityEngine;

// Token: 0x02000B8C RID: 2956
public class ProjectileData : ScriptableObject
{
	// Token: 0x0400434C RID: 17228
	public string Name;

	// Token: 0x0400434D RID: 17229
	public DamageType DamageType;

	// Token: 0x0400434E RID: 17230
	public float StrengthScale;

	// Token: 0x0400434F RID: 17231
	public float MagicScale;

	// Token: 0x04004350 RID: 17232
	public float CooldownReductionPerHit;

	// Token: 0x04004351 RID: 17233
	public float ManaGainPerHit;

	// Token: 0x04004352 RID: 17234
	public float StunStrength;

	// Token: 0x04004353 RID: 17235
	public float KnockbackStrength;

	// Token: 0x04004354 RID: 17236
	public float KnockbackModX;

	// Token: 0x04004355 RID: 17237
	public float KnockbackModY;

	// Token: 0x04004356 RID: 17238
	public float Speed;

	// Token: 0x04004357 RID: 17239
	public float TurnSpeed;

	// Token: 0x04004358 RID: 17240
	public float LifeSpan;

	// Token: 0x04004359 RID: 17241
	public float RepeatHitCheckTimer;

	// Token: 0x0400435A RID: 17242
	public string Title;

	// Token: 0x0400435B RID: 17243
	public string Controls;

	// Token: 0x0400435C RID: 17244
	public string Description;
}
