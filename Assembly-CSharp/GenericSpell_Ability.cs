using System;
using UnityEngine;

// Token: 0x02000174 RID: 372
public class GenericSpell_Ability : BaseAbility_RL, ISpell, IAbility
{
	// Token: 0x170006FD RID: 1789
	// (get) Token: 0x06000CFD RID: 3325 RVA: 0x00027AF7 File Offset: 0x00025CF7
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170006FE RID: 1790
	// (get) Token: 0x06000CFE RID: 3326 RVA: 0x00027AFE File Offset: 0x00025CFE
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006FF RID: 1791
	// (get) Token: 0x06000CFF RID: 3327 RVA: 0x00027B05 File Offset: 0x00025D05
	protected override float TellAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x17000700 RID: 1792
	// (get) Token: 0x06000D00 RID: 3328 RVA: 0x00027B0C File Offset: 0x00025D0C
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000701 RID: 1793
	// (get) Token: 0x06000D01 RID: 3329 RVA: 0x00027B13 File Offset: 0x00025D13
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000702 RID: 1794
	// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00027B1A File Offset: 0x00025D1A
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000703 RID: 1795
	// (get) Token: 0x06000D03 RID: 3331 RVA: 0x00027B21 File Offset: 0x00025D21
	protected override float AttackAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000704 RID: 1796
	// (get) Token: 0x06000D04 RID: 3332 RVA: 0x00027B28 File Offset: 0x00025D28
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000705 RID: 1797
	// (get) Token: 0x06000D05 RID: 3333 RVA: 0x00027B2F File Offset: 0x00025D2F
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000706 RID: 1798
	// (get) Token: 0x06000D06 RID: 3334 RVA: 0x00027B36 File Offset: 0x00025D36
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000D07 RID: 3335 RVA: 0x00027B40 File Offset: 0x00025D40
	protected override void FireProjectile()
	{
		if (!string.IsNullOrEmpty(this.ProjectileName))
		{
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, this.ProjectileOffset, this.m_projectileMatchFacing, this.m_fireAngle, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			if (!this.m_projectileMatchFacing && this.m_firedProjectile.transform.parent == this.m_abilityController.PlayerController.Pivot.transform)
			{
				this.m_firedProjectile.transform.SetParent(this.m_abilityController.PlayerController.transform, false);
			}
			this.ApplyAbilityCosts();
		}
	}

	// Token: 0x040010BA RID: 4282
	[SerializeField]
	private bool m_projectileMatchFacing = true;

	// Token: 0x040010BB RID: 4283
	[SerializeField]
	private float m_fireAngle;
}
