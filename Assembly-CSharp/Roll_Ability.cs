using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x02000187 RID: 391
public class Roll_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x1700077F RID: 1919
	// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x0002AECF File Offset: 0x000290CF
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000780 RID: 1920
	// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x0002AED6 File Offset: 0x000290D6
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000781 RID: 1921
	// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x0002AEDD File Offset: 0x000290DD
	protected override float TellAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000782 RID: 1922
	// (get) Token: 0x06000DF4 RID: 3572 RVA: 0x0002AEE4 File Offset: 0x000290E4
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000783 RID: 1923
	// (get) Token: 0x06000DF5 RID: 3573 RVA: 0x0002AEEB File Offset: 0x000290EB
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000784 RID: 1924
	// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x0002AEF2 File Offset: 0x000290F2
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000785 RID: 1925
	// (get) Token: 0x06000DF7 RID: 3575 RVA: 0x0002AEF9 File Offset: 0x000290F9
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000786 RID: 1926
	// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x0002AF00 File Offset: 0x00029100
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000787 RID: 1927
	// (get) Token: 0x06000DF9 RID: 3577 RVA: 0x0002AF07 File Offset: 0x00029107
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000788 RID: 1928
	// (get) Token: 0x06000DFA RID: 3578 RVA: 0x0002AF0E File Offset: 0x0002910E
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000DFB RID: 3579 RVA: 0x0002AF15 File Offset: 0x00029115
	public override void PreCastAbility()
	{
		base.PreCastAbility();
		this.ApplyAbilityCosts();
		this.StartCooldownTimer();
	}

	// Token: 0x06000DFC RID: 3580 RVA: 0x0002AF29 File Offset: 0x00029129
	public override IEnumerator CastAbility()
	{
		PlayerController playerController = this.m_abilityController.PlayerController;
		if (playerController.CharacterDash.IsDashing)
		{
			playerController.CharacterDash.StopDash();
		}
		this.m_prevInvincTime = playerController.InvincibilityTimer;
		if (this.m_prevInvincTime > 5f)
		{
			this.m_prevInvincTime = 0f;
		}
		playerController.CharacterHitResponse.SetInvincibleTime(9999f, false, false);
		playerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
		playerController.CharacterJump.ResetBrakeForce();
		playerController.DisableFriction = true;
		playerController.DisableDoorBlock = true;
		float num = Rewired_RL.Player.GetAxis("MoveHorizontal");
		if (num == 0f)
		{
			if (playerController.IsFacingRight)
			{
				num = 1f;
			}
			else
			{
				num = -1f;
			}
		}
		if ((num > 0f && !playerController.IsFacingRight) || (num < 0f && playerController.IsFacingRight))
		{
			playerController.CharacterCorgi.Flip(false, false);
		}
		if (num > 0f)
		{
			playerController.SetVelocityX(22f, false);
		}
		else
		{
			playerController.SetVelocityX(-22f, false);
		}
		playerController.SetVelocityY(0f, false);
		this.m_rollStartTime = Time.time;
		yield return base.CastAbility();
		yield break;
	}

	// Token: 0x06000DFD RID: 3581 RVA: 0x0002AF38 File Offset: 0x00029138
	protected override IEnumerator ChangeAnim(float duration)
	{
		if (base.CurrentAbilityAnimState == AbilityAnimState.Attack)
		{
			while (Time.time - this.m_rollStartTime < 0.3f)
			{
				yield return null;
			}
			this.m_animator.SetTrigger("Change_Ability_Anim");
		}
		else
		{
			yield return base.ChangeAnim(duration);
		}
		yield break;
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x0002AF4E File Offset: 0x0002914E
	protected override void OnExitExitLogic()
	{
		base.OnExitExitLogic();
		this.m_abilityController.PlayerController.JustRolled = true;
	}

	// Token: 0x06000DFF RID: 3583 RVA: 0x0002AF68 File Offset: 0x00029168
	public override void StopAbility(bool abilityInterrupted)
	{
		PlayerController playerController = this.m_abilityController.PlayerController;
		playerController.DisableFriction = false;
		playerController.DisableDoorBlock = false;
		float num = this.m_prevInvincTime - (Time.time - this.m_rollStartTime);
		num = Mathf.Min(1f, num);
		if (num > 0.1f)
		{
			playerController.CharacterHitResponse.SetInvincibleTime(num, false, playerController.CharacterHitResponse.InvincibilityEffectPlaying);
		}
		else
		{
			playerController.CharacterHitResponse.SetInvincibleTime(0.1f, false, false);
		}
		playerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.FlagForDestruction(null);
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x04001104 RID: 4356
	private float m_rollStartTime;

	// Token: 0x04001105 RID: 4357
	private float m_prevInvincTime;
}
