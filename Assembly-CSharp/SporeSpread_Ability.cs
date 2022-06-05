using System;

// Token: 0x020002B7 RID: 695
public class SporeSpread_Ability : AimedAbilityFast_RL, ISpell, IAbility
{
	// Token: 0x17000985 RID: 2437
	// (get) Token: 0x0600146D RID: 5229 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000986 RID: 2438
	// (get) Token: 0x0600146E RID: 5230 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000987 RID: 2439
	// (get) Token: 0x0600146F RID: 5231 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float TellAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000988 RID: 2440
	// (get) Token: 0x06001470 RID: 5232 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000989 RID: 2441
	// (get) Token: 0x06001471 RID: 5233 RVA: 0x00004536 File Offset: 0x00002736
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700098A RID: 2442
	// (get) Token: 0x06001472 RID: 5234 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700098B RID: 2443
	// (get) Token: 0x06001473 RID: 5235 RVA: 0x00004536 File Offset: 0x00002736
	protected override float AttackAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700098C RID: 2444
	// (get) Token: 0x06001474 RID: 5236 RVA: 0x00004A00 File Offset: 0x00002C00
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x1700098D RID: 2445
	// (get) Token: 0x06001475 RID: 5237 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700098E RID: 2446
	// (get) Token: 0x06001476 RID: 5238 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001477 RID: 5239 RVA: 0x00086E9C File Offset: 0x0008509C
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

	// Token: 0x04001613 RID: 5651
	private float m_fireAngle;

	// Token: 0x04001614 RID: 5652
	private float m_fireAngle2 = 7.5f;

	// Token: 0x04001615 RID: 5653
	private float m_fireAngle3 = 15f;
}
