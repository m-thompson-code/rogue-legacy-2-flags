using System;

// Token: 0x02000315 RID: 789
public class SwordBeam_Ability : Sword_Ability
{
	// Token: 0x17000C1D RID: 3101
	// (get) Token: 0x0600190A RID: 6410 RVA: 0x00004A6C File Offset: 0x00002C6C
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2.25f;
		}
	}

	// Token: 0x17000C1E RID: 3102
	// (get) Token: 0x0600190B RID: 6411 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C1F RID: 3103
	// (get) Token: 0x0600190C RID: 6412 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float TellAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000C20 RID: 3104
	// (get) Token: 0x0600190D RID: 6413 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C21 RID: 3105
	// (get) Token: 0x0600190E RID: 6414 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000C22 RID: 3106
	// (get) Token: 0x0600190F RID: 6415 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C23 RID: 3107
	// (get) Token: 0x06001910 RID: 6416 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000C24 RID: 3108
	// (get) Token: 0x06001911 RID: 6417 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000C25 RID: 3109
	// (get) Token: 0x06001912 RID: 6418 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000C26 RID: 3110
	// (get) Token: 0x06001913 RID: 6419 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001914 RID: 6420 RVA: 0x0000C9F9 File Offset: 0x0000ABF9
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			"SwordWeaponBreakablesProjectile",
			"SwordBeamWeaponProjectile"
		};
	}

	// Token: 0x06001915 RID: 6421 RVA: 0x0008F074 File Offset: 0x0008D274
	protected override void FireProjectile()
	{
		base.FireProjectile();
		Projectile_RL projectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, "SwordBeamWeaponProjectile", this.ProjectileOffset, true, 0f, 1f, false, true, true, true);
		this.m_abilityController.InitializeProjectile(projectile);
	}

	// Token: 0x040017ED RID: 6125
	private const string BEAM_PROJECTILE_NAME = "SwordBeamWeaponProjectile";
}
