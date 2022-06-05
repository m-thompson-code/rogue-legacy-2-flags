using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020002D0 RID: 720
public class Roll_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x170009FD RID: 2557
	// (get) Token: 0x06001572 RID: 5490 RVA: 0x00004536 File Offset: 0x00002736
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170009FE RID: 2558
	// (get) Token: 0x06001573 RID: 5491 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009FF RID: 2559
	// (get) Token: 0x06001574 RID: 5492 RVA: 0x00004536 File Offset: 0x00002736
	protected override float TellAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000A00 RID: 2560
	// (get) Token: 0x06001575 RID: 5493 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A01 RID: 2561
	// (get) Token: 0x06001576 RID: 5494 RVA: 0x00004FFB File Offset: 0x000031FB
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000A02 RID: 2562
	// (get) Token: 0x06001577 RID: 5495 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A03 RID: 2563
	// (get) Token: 0x06001578 RID: 5496 RVA: 0x00004FFB File Offset: 0x000031FB
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000A04 RID: 2564
	// (get) Token: 0x06001579 RID: 5497 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A05 RID: 2565
	// (get) Token: 0x0600157A RID: 5498 RVA: 0x00004536 File Offset: 0x00002736
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000A06 RID: 2566
	// (get) Token: 0x0600157B RID: 5499 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600157C RID: 5500 RVA: 0x0000AA78 File Offset: 0x00008C78
	public override void PreCastAbility()
	{
		base.PreCastAbility();
		this.ApplyAbilityCosts();
		this.StartCooldownTimer();
	}

	// Token: 0x0600157D RID: 5501 RVA: 0x0000AA8C File Offset: 0x00008C8C
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

	// Token: 0x0600157E RID: 5502 RVA: 0x0000AA9B File Offset: 0x00008C9B
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

	// Token: 0x0600157F RID: 5503 RVA: 0x0000AAB1 File Offset: 0x00008CB1
	protected override void OnExitExitLogic()
	{
		base.OnExitExitLogic();
		this.m_abilityController.PlayerController.JustRolled = true;
	}

	// Token: 0x06001580 RID: 5504 RVA: 0x00089F98 File Offset: 0x00088198
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

	// Token: 0x04001679 RID: 5753
	private float m_rollStartTime;

	// Token: 0x0400167A RID: 5754
	private float m_prevInvincTime;
}
