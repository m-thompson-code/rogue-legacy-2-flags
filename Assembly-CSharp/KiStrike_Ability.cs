using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x02000182 RID: 386
public class KiStrike_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x1700075E RID: 1886
	// (get) Token: 0x06000DBE RID: 3518 RVA: 0x0002A90B File Offset: 0x00028B0B
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700075F RID: 1887
	// (get) Token: 0x06000DBF RID: 3519 RVA: 0x0002A912 File Offset: 0x00028B12
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000760 RID: 1888
	// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x0002A919 File Offset: 0x00028B19
	protected override float TellAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000761 RID: 1889
	// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x0002A920 File Offset: 0x00028B20
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000762 RID: 1890
	// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x0002A927 File Offset: 0x00028B27
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000763 RID: 1891
	// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x0002A92E File Offset: 0x00028B2E
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000764 RID: 1892
	// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x0002A935 File Offset: 0x00028B35
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000765 RID: 1893
	// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x0002A93C File Offset: 0x00028B3C
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000766 RID: 1894
	// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x0002A943 File Offset: 0x00028B43
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000767 RID: 1895
	// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x0002A94A File Offset: 0x00028B4A
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x0002A951 File Offset: 0x00028B51
	public override IEnumerator CastAbility()
	{
		if (this.m_disableGravity && this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.DisableHorizontalMovement)
		{
			this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
			this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
			this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(false);
		}
		yield return base.CastAbility();
		yield break;
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x0002A960 File Offset: 0x00028B60
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.FlagForDestruction(null);
			this.m_firedProjectile = null;
		}
		if (this.m_disableGravity)
		{
			this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(true);
			if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
			{
				this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
			}
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x040010FF RID: 4351
	[SerializeField]
	private bool m_disableGravity;
}
