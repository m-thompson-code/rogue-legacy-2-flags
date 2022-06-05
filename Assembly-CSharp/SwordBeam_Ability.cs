using System;

// Token: 0x020001A9 RID: 425
public class SwordBeam_Ability : Sword_Ability
{
	// Token: 0x17000961 RID: 2401
	// (get) Token: 0x060010CF RID: 4303 RVA: 0x00030A34 File Offset: 0x0002EC34
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2.25f;
		}
	}

	// Token: 0x17000962 RID: 2402
	// (get) Token: 0x060010D0 RID: 4304 RVA: 0x00030A3B File Offset: 0x0002EC3B
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000963 RID: 2403
	// (get) Token: 0x060010D1 RID: 4305 RVA: 0x00030A42 File Offset: 0x0002EC42
	protected override float TellAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000964 RID: 2404
	// (get) Token: 0x060010D2 RID: 4306 RVA: 0x00030A49 File Offset: 0x0002EC49
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000965 RID: 2405
	// (get) Token: 0x060010D3 RID: 4307 RVA: 0x00030A50 File Offset: 0x0002EC50
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000966 RID: 2406
	// (get) Token: 0x060010D4 RID: 4308 RVA: 0x00030A57 File Offset: 0x0002EC57
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000967 RID: 2407
	// (get) Token: 0x060010D5 RID: 4309 RVA: 0x00030A5E File Offset: 0x0002EC5E
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000968 RID: 2408
	// (get) Token: 0x060010D6 RID: 4310 RVA: 0x00030A65 File Offset: 0x0002EC65
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000969 RID: 2409
	// (get) Token: 0x060010D7 RID: 4311 RVA: 0x00030A6C File Offset: 0x0002EC6C
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700096A RID: 2410
	// (get) Token: 0x060010D8 RID: 4312 RVA: 0x00030A73 File Offset: 0x0002EC73
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x00030A7A File Offset: 0x0002EC7A
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			"SwordWeaponBreakablesProjectile",
			"SwordBeamWeaponProjectile"
		};
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x00030AA4 File Offset: 0x0002ECA4
	protected override void FireProjectile()
	{
		base.FireProjectile();
		Projectile_RL projectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, "SwordBeamWeaponProjectile", this.ProjectileOffset, true, 0f, 1f, false, true, true, true);
		this.m_abilityController.InitializeProjectile(projectile);
	}

	// Token: 0x040011EC RID: 4588
	private const string BEAM_PROJECTILE_NAME = "SwordBeamWeaponProjectile";
}
