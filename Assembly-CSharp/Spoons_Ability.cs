using System;

// Token: 0x020001A8 RID: 424
public class Spoons_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x17000956 RID: 2390
	// (get) Token: 0x060010C2 RID: 4290 RVA: 0x00030975 File Offset: 0x0002EB75
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000957 RID: 2391
	// (get) Token: 0x060010C3 RID: 4291 RVA: 0x0003097C File Offset: 0x0002EB7C
	protected virtual float Spoon_Anim_Setter
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x17000958 RID: 2392
	// (get) Token: 0x060010C4 RID: 4292 RVA: 0x00030983 File Offset: 0x0002EB83
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000959 RID: 2393
	// (get) Token: 0x060010C5 RID: 4293 RVA: 0x0003098A File Offset: 0x0002EB8A
	protected override float TellAnimSpeed
	{
		get
		{
			return this.Spoon_Anim_Setter;
		}
	}

	// Token: 0x1700095A RID: 2394
	// (get) Token: 0x060010C6 RID: 4294 RVA: 0x00030992 File Offset: 0x0002EB92
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700095B RID: 2395
	// (get) Token: 0x060010C7 RID: 4295 RVA: 0x00030999 File Offset: 0x0002EB99
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return this.Spoon_Anim_Setter;
		}
	}

	// Token: 0x1700095C RID: 2396
	// (get) Token: 0x060010C8 RID: 4296 RVA: 0x000309A1 File Offset: 0x0002EBA1
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700095D RID: 2397
	// (get) Token: 0x060010C9 RID: 4297 RVA: 0x000309A8 File Offset: 0x0002EBA8
	protected override float AttackAnimSpeed
	{
		get
		{
			return this.Spoon_Anim_Setter;
		}
	}

	// Token: 0x1700095E RID: 2398
	// (get) Token: 0x060010CA RID: 4298 RVA: 0x000309B0 File Offset: 0x0002EBB0
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.45f;
		}
	}

	// Token: 0x1700095F RID: 2399
	// (get) Token: 0x060010CB RID: 4299 RVA: 0x000309B7 File Offset: 0x0002EBB7
	protected override float ExitAnimSpeed
	{
		get
		{
			return this.Spoon_Anim_Setter;
		}
	}

	// Token: 0x17000960 RID: 2400
	// (get) Token: 0x060010CC RID: 4300 RVA: 0x000309BF File Offset: 0x0002EBBF
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060010CD RID: 4301 RVA: 0x000309C8 File Offset: 0x0002EBC8
	protected override void FireProjectile()
	{
		if (!string.IsNullOrEmpty(this.ProjectileName))
		{
			Projectile_RL projectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, true, this.m_fireAngle, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(projectile);
			this.ApplyAbilityCosts();
		}
	}

	// Token: 0x040011EB RID: 4587
	private float m_fireAngle = 48f;
}
