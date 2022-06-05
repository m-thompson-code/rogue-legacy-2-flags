using System;
using UnityEngine;

// Token: 0x02000316 RID: 790
public class Sword_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x17000C27 RID: 3111
	// (get) Token: 0x06001917 RID: 6423 RVA: 0x00004A6C File Offset: 0x00002C6C
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2.25f;
		}
	}

	// Token: 0x17000C28 RID: 3112
	// (get) Token: 0x06001918 RID: 6424 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C29 RID: 3113
	// (get) Token: 0x06001919 RID: 6425 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float TellAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000C2A RID: 3114
	// (get) Token: 0x0600191A RID: 6426 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C2B RID: 3115
	// (get) Token: 0x0600191B RID: 6427 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000C2C RID: 3116
	// (get) Token: 0x0600191C RID: 6428 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C2D RID: 3117
	// (get) Token: 0x0600191D RID: 6429 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000C2E RID: 3118
	// (get) Token: 0x0600191E RID: 6430 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000C2F RID: 3119
	// (get) Token: 0x0600191F RID: 6431 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000C30 RID: 3120
	// (get) Token: 0x06001920 RID: 6432 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001921 RID: 6433 RVA: 0x0000CA20 File Offset: 0x0000AC20
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			"SwordWeaponBreakablesProjectile"
		};
	}

	// Token: 0x06001922 RID: 6434 RVA: 0x0008F0C0 File Offset: 0x0008D2C0
	protected override void FireProjectile()
	{
		base.FireProjectile();
		if (this.m_spawnBreakablesProjectile)
		{
			this.m_breakablesProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, "SwordWeaponBreakablesProjectile", this.ProjectileOffset, true, 0f, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_breakablesProjectile);
		}
	}

	// Token: 0x06001923 RID: 6435 RVA: 0x0008F11C File Offset: 0x0008D31C
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

	// Token: 0x040017EE RID: 6126
	protected const string BREAKABLES_PROJECTILE_NAME = "SwordWeaponBreakablesProjectile";

	// Token: 0x040017EF RID: 6127
	[SerializeField]
	private bool m_spawnBreakablesProjectile;

	// Token: 0x040017F0 RID: 6128
	private Projectile_RL m_breakablesProjectile;
}
