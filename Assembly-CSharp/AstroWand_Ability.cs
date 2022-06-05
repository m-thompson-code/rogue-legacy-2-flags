using System;
using System.Collections;
using MoreMountains.CorgiEngine;

// Token: 0x020002DD RID: 733
public class AstroWand_Ability : BaseAbility_RL
{
	// Token: 0x17000A29 RID: 2601
	// (get) Token: 0x060015E4 RID: 5604 RVA: 0x00003C62 File Offset: 0x00001E62
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000A2A RID: 2602
	// (get) Token: 0x060015E5 RID: 5605 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A2B RID: 2603
	// (get) Token: 0x060015E6 RID: 5606 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000A2C RID: 2604
	// (get) Token: 0x060015E7 RID: 5607 RVA: 0x0000AD83 File Offset: 0x00008F83
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0.155f;
		}
	}

	// Token: 0x17000A2D RID: 2605
	// (get) Token: 0x060015E8 RID: 5608 RVA: 0x00003C62 File Offset: 0x00001E62
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000A2E RID: 2606
	// (get) Token: 0x060015E9 RID: 5609 RVA: 0x0000452F File Offset: 0x0000272F
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000A2F RID: 2607
	// (get) Token: 0x060015EA RID: 5610 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000A30 RID: 2608
	// (get) Token: 0x060015EB RID: 5611 RVA: 0x00003FB0 File Offset: 0x000021B0
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.175f;
		}
	}

	// Token: 0x17000A31 RID: 2609
	// (get) Token: 0x060015EC RID: 5612 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000A32 RID: 2610
	// (get) Token: 0x060015ED RID: 5613 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A33 RID: 2611
	// (get) Token: 0x060015EE RID: 5614 RVA: 0x00003CC4 File Offset: 0x00001EC4
	private float PushbackAmountY
	{
		get
		{
			return 12f;
		}
	}

	// Token: 0x060015EF RID: 5615 RVA: 0x0000AD8A File Offset: 0x00008F8A
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

	// Token: 0x060015F0 RID: 5616 RVA: 0x0000AD99 File Offset: 0x00008F99
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

	// Token: 0x060015F1 RID: 5617 RVA: 0x0008ACDC File Offset: 0x00088EDC
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

	// Token: 0x060015F2 RID: 5618 RVA: 0x0000ADAF File Offset: 0x00008FAF
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_gravityReductionModWhenAiming < 1f)
		{
			this.m_abilityController.PlayerController.FallMultiplierOverride = this.m_storedFallMultiplier;
		}
		this.m_animator.SetBool("Hover", false);
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x040016A5 RID: 5797
	private bool m_stopVelocityWhenAiming = true;

	// Token: 0x040016A6 RID: 5798
	private float m_gravityReductionModWhenAiming = 0.25f;

	// Token: 0x040016A7 RID: 5799
	private float m_storedFallMultiplier;
}
