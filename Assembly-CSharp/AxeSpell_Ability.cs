using System;

// Token: 0x020002AA RID: 682
public class AxeSpell_Ability : BaseAbility_RL, ISpell, IAbility
{
	// Token: 0x1700093E RID: 2366
	// (get) Token: 0x060013E9 RID: 5097 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x1700093F RID: 2367
	// (get) Token: 0x060013EA RID: 5098 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000940 RID: 2368
	// (get) Token: 0x060013EB RID: 5099 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float TellAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000941 RID: 2369
	// (get) Token: 0x060013EC RID: 5100 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000942 RID: 2370
	// (get) Token: 0x060013ED RID: 5101 RVA: 0x00004536 File Offset: 0x00002736
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000943 RID: 2371
	// (get) Token: 0x060013EE RID: 5102 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000944 RID: 2372
	// (get) Token: 0x060013EF RID: 5103 RVA: 0x00004536 File Offset: 0x00002736
	protected override float AttackAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000945 RID: 2373
	// (get) Token: 0x060013F0 RID: 5104 RVA: 0x00004A00 File Offset: 0x00002C00
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000946 RID: 2374
	// (get) Token: 0x060013F1 RID: 5105 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000947 RID: 2375
	// (get) Token: 0x060013F2 RID: 5106 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060013F3 RID: 5107 RVA: 0x00086320 File Offset: 0x00084520
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

	// Token: 0x040015E9 RID: 5609
	private float m_fireAngle = 85f;

	// Token: 0x040015EA RID: 5610
	private float m_fireAngle2 = 75f;

	// Token: 0x040015EB RID: 5611
	private float m_fireAngle3 = 65f;
}
