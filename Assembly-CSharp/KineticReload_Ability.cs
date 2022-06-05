using System;

// Token: 0x02000183 RID: 387
public class KineticReload_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x17000768 RID: 1896
	// (get) Token: 0x06000DCC RID: 3532 RVA: 0x0002A9E6 File Offset: 0x00028BE6
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000769 RID: 1897
	// (get) Token: 0x06000DCD RID: 3533 RVA: 0x0002A9ED File Offset: 0x00028BED
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700076A RID: 1898
	// (get) Token: 0x06000DCE RID: 3534 RVA: 0x0002A9F4 File Offset: 0x00028BF4
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700076B RID: 1899
	// (get) Token: 0x06000DCF RID: 3535 RVA: 0x0002A9FB File Offset: 0x00028BFB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700076C RID: 1900
	// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x0002AA02 File Offset: 0x00028C02
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x1700076D RID: 1901
	// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x0002AA09 File Offset: 0x00028C09
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700076E RID: 1902
	// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x0002AA10 File Offset: 0x00028C10
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x1700076F RID: 1903
	// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x0002AA17 File Offset: 0x00028C17
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000770 RID: 1904
	// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x0002AA1E File Offset: 0x00028C1E
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000771 RID: 1905
	// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x0002AA25 File Offset: 0x00028C25
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x0002AA2C File Offset: 0x00028C2C
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

	// Token: 0x06000DD7 RID: 3543 RVA: 0x0002AAB5 File Offset: 0x00028CB5
	protected override void FireProjectile()
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.FlagForDestruction(null);
		}
		this.m_abilityController.ResetAbilityAmmo(CastAbilityType.Weapon, false);
	}
}
