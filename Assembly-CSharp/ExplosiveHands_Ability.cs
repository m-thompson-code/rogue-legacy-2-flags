using System;
using UnityEngine;

// Token: 0x020002F0 RID: 752
public class ExplosiveHands_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x17000B0A RID: 2826
	// (get) Token: 0x0600173A RID: 5946 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000B0B RID: 2827
	// (get) Token: 0x0600173B RID: 5947 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B0C RID: 2828
	// (get) Token: 0x0600173C RID: 5948 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float TellAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000B0D RID: 2829
	// (get) Token: 0x0600173D RID: 5949 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B0E RID: 2830
	// (get) Token: 0x0600173E RID: 5950 RVA: 0x00004536 File Offset: 0x00002736
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000B0F RID: 2831
	// (get) Token: 0x0600173F RID: 5951 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B10 RID: 2832
	// (get) Token: 0x06001740 RID: 5952 RVA: 0x00004536 File Offset: 0x00002736
	protected override float AttackAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000B11 RID: 2833
	// (get) Token: 0x06001741 RID: 5953 RVA: 0x00004A00 File Offset: 0x00002C00
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000B12 RID: 2834
	// (get) Token: 0x06001742 RID: 5954 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B13 RID: 2835
	// (get) Token: 0x06001743 RID: 5955 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B14 RID: 2836
	// (get) Token: 0x06001744 RID: 5956 RVA: 0x0000BC8B File Offset: 0x00009E8B
	public override string AbilityTellIntroName
	{
		get
		{
			if (this.m_useAltCast)
			{
				return this.m_altSpellCast;
			}
			return base.AbilityTellIntroName;
		}
	}

	// Token: 0x06001745 RID: 5957 RVA: 0x0000BCA2 File Offset: 0x00009EA2
	public override void PreCastAbility()
	{
		if (Time.time - this.m_timeSinceLastCast < 1f)
		{
			this.m_useAltCast = !this.m_useAltCast;
		}
		else
		{
			this.m_useAltCast = false;
		}
		this.m_timeSinceLastCast = Time.time;
		base.PreCastAbility();
	}

	// Token: 0x06001746 RID: 5958 RVA: 0x0008C05C File Offset: 0x0008A25C
	protected override void FireProjectile()
	{
		if (!string.IsNullOrEmpty(this.ProjectileName))
		{
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, this.m_projectileMatchFacing, this.m_fireAngle, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			this.ApplyAbilityCosts();
		}
	}

	// Token: 0x0400171F RID: 5919
	[SerializeField]
	private string m_altSpellCast;

	// Token: 0x04001720 RID: 5920
	[SerializeField]
	private bool m_projectileMatchFacing = true;

	// Token: 0x04001721 RID: 5921
	[SerializeField]
	private float m_fireAngle;

	// Token: 0x04001722 RID: 5922
	private bool m_useAltCast;

	// Token: 0x04001723 RID: 5923
	private float m_timeSinceLastCast;
}
