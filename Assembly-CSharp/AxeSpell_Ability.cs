using System;

// Token: 0x02000171 RID: 369
public class AxeSpell_Ability : BaseAbility_RL, ISpell, IAbility
{
	// Token: 0x170006DE RID: 1758
	// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x00027246 File Offset: 0x00025446
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170006DF RID: 1759
	// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x0002724D File Offset: 0x0002544D
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006E0 RID: 1760
	// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x00027254 File Offset: 0x00025454
	protected override float TellAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170006E1 RID: 1761
	// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0002725B File Offset: 0x0002545B
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006E2 RID: 1762
	// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x00027262 File Offset: 0x00025462
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170006E3 RID: 1763
	// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x00027269 File Offset: 0x00025469
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006E4 RID: 1764
	// (get) Token: 0x06000CCA RID: 3274 RVA: 0x00027270 File Offset: 0x00025470
	protected override float AttackAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170006E5 RID: 1765
	// (get) Token: 0x06000CCB RID: 3275 RVA: 0x00027277 File Offset: 0x00025477
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170006E6 RID: 1766
	// (get) Token: 0x06000CCC RID: 3276 RVA: 0x0002727E File Offset: 0x0002547E
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170006E7 RID: 1767
	// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00027285 File Offset: 0x00025485
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000CCE RID: 3278 RVA: 0x0002728C File Offset: 0x0002548C
	protected override void FireProjectile()
	{
		if (this.ProjectileName != null)
		{
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, true, this.m_fireAngle, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, true, this.m_fireAngle2, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, true, this.m_fireAngle3, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			this.ApplyAbilityCosts();
		}
	}

	// Token: 0x040010AA RID: 4266
	private float m_fireAngle = 85f;

	// Token: 0x040010AB RID: 4267
	private float m_fireAngle2 = 75f;

	// Token: 0x040010AC RID: 4268
	private float m_fireAngle3 = 65f;
}
