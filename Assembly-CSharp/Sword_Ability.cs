using System;
using UnityEngine;

// Token: 0x020001AA RID: 426
public class Sword_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x1700096B RID: 2411
	// (get) Token: 0x060010DC RID: 4316 RVA: 0x00030AF6 File Offset: 0x0002ECF6
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2.25f;
		}
	}

	// Token: 0x1700096C RID: 2412
	// (get) Token: 0x060010DD RID: 4317 RVA: 0x00030AFD File Offset: 0x0002ECFD
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700096D RID: 2413
	// (get) Token: 0x060010DE RID: 4318 RVA: 0x00030B04 File Offset: 0x0002ED04
	protected override float TellAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x1700096E RID: 2414
	// (get) Token: 0x060010DF RID: 4319 RVA: 0x00030B0B File Offset: 0x0002ED0B
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700096F RID: 2415
	// (get) Token: 0x060010E0 RID: 4320 RVA: 0x00030B12 File Offset: 0x0002ED12
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000970 RID: 2416
	// (get) Token: 0x060010E1 RID: 4321 RVA: 0x00030B19 File Offset: 0x0002ED19
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000971 RID: 2417
	// (get) Token: 0x060010E2 RID: 4322 RVA: 0x00030B20 File Offset: 0x0002ED20
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000972 RID: 2418
	// (get) Token: 0x060010E3 RID: 4323 RVA: 0x00030B27 File Offset: 0x0002ED27
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000973 RID: 2419
	// (get) Token: 0x060010E4 RID: 4324 RVA: 0x00030B2E File Offset: 0x0002ED2E
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000974 RID: 2420
	// (get) Token: 0x060010E5 RID: 4325 RVA: 0x00030B35 File Offset: 0x0002ED35
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x00030B3C File Offset: 0x0002ED3C
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			"SwordWeaponBreakablesProjectile"
		};
	}

	// Token: 0x060010E7 RID: 4327 RVA: 0x00030B5C File Offset: 0x0002ED5C
	protected override void FireProjectile()
	{
		base.FireProjectile();
		if (this.m_spawnBreakablesProjectile)
		{
			this.m_breakablesProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, "SwordWeaponBreakablesProjectile", this.ProjectileOffset, true, 0f, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_breakablesProjectile);
		}
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x00030BB8 File Offset: 0x0002EDB8
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_firedProjectile != null)
		{
			this.m_firedProjectile.FlagForDestruction(null);
			this.m_firedProjectile = null;
		}
		if (this.m_breakablesProjectile != null)
		{
			this.m_breakablesProjectile.FlagForDestruction(null);
			this.m_breakablesProjectile = null;
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x040011ED RID: 4589
	protected const string BREAKABLES_PROJECTILE_NAME = "SwordWeaponBreakablesProjectile";

	// Token: 0x040011EE RID: 4590
	[SerializeField]
	private bool m_spawnBreakablesProjectile;

	// Token: 0x040011EF RID: 4591
	private Projectile_RL m_breakablesProjectile;
}
