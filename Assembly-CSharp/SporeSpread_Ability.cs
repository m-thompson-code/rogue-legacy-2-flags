using System;

// Token: 0x02000178 RID: 376
public class SporeSpread_Ability : AimedAbilityFast_RL, ISpell, IAbility
{
	// Token: 0x1700071B RID: 1819
	// (get) Token: 0x06000D28 RID: 3368 RVA: 0x00027E44 File Offset: 0x00026044
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x1700071C RID: 1820
	// (get) Token: 0x06000D29 RID: 3369 RVA: 0x00027E4B File Offset: 0x0002604B
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700071D RID: 1821
	// (get) Token: 0x06000D2A RID: 3370 RVA: 0x00027E52 File Offset: 0x00026052
	protected override float TellAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x1700071E RID: 1822
	// (get) Token: 0x06000D2B RID: 3371 RVA: 0x00027E59 File Offset: 0x00026059
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700071F RID: 1823
	// (get) Token: 0x06000D2C RID: 3372 RVA: 0x00027E60 File Offset: 0x00026060
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000720 RID: 1824
	// (get) Token: 0x06000D2D RID: 3373 RVA: 0x00027E67 File Offset: 0x00026067
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000721 RID: 1825
	// (get) Token: 0x06000D2E RID: 3374 RVA: 0x00027E6E File Offset: 0x0002606E
	protected override float AttackAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000722 RID: 1826
	// (get) Token: 0x06000D2F RID: 3375 RVA: 0x00027E75 File Offset: 0x00026075
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000723 RID: 1827
	// (get) Token: 0x06000D30 RID: 3376 RVA: 0x00027E7C File Offset: 0x0002607C
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000724 RID: 1828
	// (get) Token: 0x06000D31 RID: 3377 RVA: 0x00027E83 File Offset: 0x00026083
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000D32 RID: 3378 RVA: 0x00027E8C File Offset: 0x0002608C
	protected override void FireProjectile()
	{
		if (!string.IsNullOrEmpty(this.ProjectileName))
		{
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, true, this.m_aimAngle + this.m_fireAngle, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, true, this.m_aimAngle + this.m_fireAngle2, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, true, this.m_aimAngle + -this.m_fireAngle2, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, true, this.m_aimAngle + this.m_fireAngle3, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, true, this.m_aimAngle + -this.m_fireAngle3, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			this.ApplyAbilityCosts();
			if (this.m_aimLine)
			{
				this.m_aimLine.gameObject.SetActive(false);
			}
			this.m_fireProjectileRelay.Dispatch(this.m_firedProjectile);
			base.CancelTimeSlow();
		}
	}

	// Token: 0x040010C1 RID: 4289
	private float m_fireAngle;

	// Token: 0x040010C2 RID: 4290
	private float m_fireAngle2 = 7.5f;

	// Token: 0x040010C3 RID: 4291
	private float m_fireAngle3 = 15f;
}
