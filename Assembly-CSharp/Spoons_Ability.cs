using System;

// Token: 0x02000314 RID: 788
public class Spoons_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x17000C12 RID: 3090
	// (get) Token: 0x060018FD RID: 6397 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000C13 RID: 3091
	// (get) Token: 0x060018FE RID: 6398 RVA: 0x00004565 File Offset: 0x00002765
	protected virtual float Spoon_Anim_Setter
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x17000C14 RID: 3092
	// (get) Token: 0x060018FF RID: 6399 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C15 RID: 3093
	// (get) Token: 0x06001900 RID: 6400 RVA: 0x0000C9DE File Offset: 0x0000ABDE
	protected override float TellAnimSpeed
	{
		get
		{
			return this.Spoon_Anim_Setter;
		}
	}

	// Token: 0x17000C16 RID: 3094
	// (get) Token: 0x06001901 RID: 6401 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C17 RID: 3095
	// (get) Token: 0x06001902 RID: 6402 RVA: 0x0000C9DE File Offset: 0x0000ABDE
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return this.Spoon_Anim_Setter;
		}
	}

	// Token: 0x17000C18 RID: 3096
	// (get) Token: 0x06001903 RID: 6403 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C19 RID: 3097
	// (get) Token: 0x06001904 RID: 6404 RVA: 0x0000C9DE File Offset: 0x0000ABDE
	protected override float AttackAnimSpeed
	{
		get
		{
			return this.Spoon_Anim_Setter;
		}
	}

	// Token: 0x17000C1A RID: 3098
	// (get) Token: 0x06001905 RID: 6405 RVA: 0x00008A75 File Offset: 0x00006C75
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.45f;
		}
	}

	// Token: 0x17000C1B RID: 3099
	// (get) Token: 0x06001906 RID: 6406 RVA: 0x0000C9DE File Offset: 0x0000ABDE
	protected override float ExitAnimSpeed
	{
		get
		{
			return this.Spoon_Anim_Setter;
		}
	}

	// Token: 0x17000C1C RID: 3100
	// (get) Token: 0x06001907 RID: 6407 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001908 RID: 6408 RVA: 0x0008F018 File Offset: 0x0008D218
	protected override void FireProjectile()
	{
		if (!string.IsNullOrEmpty(this.ProjectileName))
		{
			Projectile_RL projectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, true, this.m_fireAngle, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(projectile);
			this.ApplyAbilityCosts();
		}
	}

	// Token: 0x040017EC RID: 6124
	private float m_fireAngle = 48f;
}
