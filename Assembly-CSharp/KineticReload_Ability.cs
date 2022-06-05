using System;

// Token: 0x020002CC RID: 716
public class KineticReload_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x170009E6 RID: 2534
	// (get) Token: 0x0600154D RID: 5453 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170009E7 RID: 2535
	// (get) Token: 0x0600154E RID: 5454 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170009E8 RID: 2536
	// (get) Token: 0x0600154F RID: 5455 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170009E9 RID: 2537
	// (get) Token: 0x06001550 RID: 5456 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009EA RID: 2538
	// (get) Token: 0x06001551 RID: 5457 RVA: 0x00004FFB File Offset: 0x000031FB
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x170009EB RID: 2539
	// (get) Token: 0x06001552 RID: 5458 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009EC RID: 2540
	// (get) Token: 0x06001553 RID: 5459 RVA: 0x00004FFB File Offset: 0x000031FB
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x170009ED RID: 2541
	// (get) Token: 0x06001554 RID: 5460 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009EE RID: 2542
	// (get) Token: 0x06001555 RID: 5461 RVA: 0x00004536 File Offset: 0x00002736
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170009EF RID: 2543
	// (get) Token: 0x06001556 RID: 5462 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001557 RID: 5463 RVA: 0x00089C18 File Offset: 0x00087E18
	protected override void OnEnterTellIntroLogic()
	{
		base.OnEnterTellIntroLogic();
		if (!string.IsNullOrEmpty(this.ProjectileName))
		{
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, true, 0f, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			this.m_firedProjectile.transform.SetParent(this.m_abilityController.PlayerController.transform, true);
			this.ApplyAbilityCosts();
		}
	}

	// Token: 0x06001558 RID: 5464 RVA: 0x0000A9C3 File Offset: 0x00008BC3
	protected override void FireProjectile()
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.FlagForDestruction(null);
		}
		this.m_abilityController.ResetAbilityAmmo(CastAbilityType.Weapon, false);
	}
}
