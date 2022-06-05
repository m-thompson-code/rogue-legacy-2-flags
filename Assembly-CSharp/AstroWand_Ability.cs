using System;
using System.Collections;
using MoreMountains.CorgiEngine;

// Token: 0x0200018E RID: 398
public class AstroWand_Ability : BaseAbility_RL
{
	// Token: 0x1700079F RID: 1951
	// (get) Token: 0x06000E3F RID: 3647 RVA: 0x0002BB25 File Offset: 0x00029D25
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170007A0 RID: 1952
	// (get) Token: 0x06000E40 RID: 3648 RVA: 0x0002BB2C File Offset: 0x00029D2C
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007A1 RID: 1953
	// (get) Token: 0x06000E41 RID: 3649 RVA: 0x0002BB33 File Offset: 0x00029D33
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007A2 RID: 1954
	// (get) Token: 0x06000E42 RID: 3650 RVA: 0x0002BB3A File Offset: 0x00029D3A
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0.155f;
		}
	}

	// Token: 0x170007A3 RID: 1955
	// (get) Token: 0x06000E43 RID: 3651 RVA: 0x0002BB41 File Offset: 0x00029D41
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170007A4 RID: 1956
	// (get) Token: 0x06000E44 RID: 3652 RVA: 0x0002BB48 File Offset: 0x00029D48
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x170007A5 RID: 1957
	// (get) Token: 0x06000E45 RID: 3653 RVA: 0x0002BB4F File Offset: 0x00029D4F
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007A6 RID: 1958
	// (get) Token: 0x06000E46 RID: 3654 RVA: 0x0002BB56 File Offset: 0x00029D56
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.175f;
		}
	}

	// Token: 0x170007A7 RID: 1959
	// (get) Token: 0x06000E47 RID: 3655 RVA: 0x0002BB5D File Offset: 0x00029D5D
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007A8 RID: 1960
	// (get) Token: 0x06000E48 RID: 3656 RVA: 0x0002BB64 File Offset: 0x00029D64
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007A9 RID: 1961
	// (get) Token: 0x06000E49 RID: 3657 RVA: 0x0002BB6B File Offset: 0x00029D6B
	private float PushbackAmountY
	{
		get
		{
			return 12f;
		}
	}

	// Token: 0x06000E4A RID: 3658 RVA: 0x0002BB72 File Offset: 0x00029D72
	public override IEnumerator CastAbility()
	{
		if (this.m_stopVelocityWhenAiming)
		{
			if (this.m_abilityController.PlayerController.CharacterJump.JumpHappenedThisFrame)
			{
				this.m_abilityController.PlayerController.MovementState = CharacterStates.MovementStates.Idle;
			}
			this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
			this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
		}
		yield return base.CastAbility();
		yield break;
	}

	// Token: 0x06000E4B RID: 3659 RVA: 0x0002BB81 File Offset: 0x00029D81
	protected override IEnumerator ChangeAnim(float duration)
	{
		yield return base.ChangeAnim(duration);
		if (base.CurrentAbilityAnimState == AbilityAnimState.TellIntro)
		{
			this.m_storedFallMultiplier = this.m_abilityController.PlayerController.FallMultiplierOverride;
			if (this.m_gravityReductionModWhenAiming < 1f)
			{
				this.m_abilityController.PlayerController.FallMultiplierOverride = this.m_gravityReductionModWhenAiming;
			}
			if (!this.m_abilityController.PlayerController.IsGrounded)
			{
				this.m_animator.SetBool("Hover", true);
			}
		}
		else if (base.CurrentAbilityAnimState == AbilityAnimState.Tell)
		{
			this.m_abilityController.PlayerController.FallMultiplierOverride = this.m_storedFallMultiplier;
			if (!this.m_abilityController.PlayerController.IsGrounded)
			{
				this.m_animator.SetBool("Hover", false);
			}
		}
		yield break;
	}

	// Token: 0x06000E4C RID: 3660 RVA: 0x0002BB98 File Offset: 0x00029D98
	protected override void FireProjectile()
	{
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerCastingAstroWand, this, null);
		base.FireProjectile();
		if (!this.m_abilityController.PlayerController.IsGrounded)
		{
			float num = this.PushbackAmountY;
			if (num < this.m_abilityController.PlayerController.Velocity.y)
			{
				num = this.m_abilityController.PlayerController.Velocity.y;
			}
			this.m_abilityController.PlayerController.SetVelocityY(num, false);
		}
	}

	// Token: 0x06000E4D RID: 3661 RVA: 0x0002BC0D File Offset: 0x00029E0D
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_gravityReductionModWhenAiming < 1f)
		{
			this.m_abilityController.PlayerController.FallMultiplierOverride = this.m_storedFallMultiplier;
		}
		this.m_animator.SetBool("Hover", false);
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x0400111B RID: 4379
	private bool m_stopVelocityWhenAiming = true;

	// Token: 0x0400111C RID: 4380
	private float m_gravityReductionModWhenAiming = 0.25f;

	// Token: 0x0400111D RID: 4381
	private float m_storedFallMultiplier;
}
