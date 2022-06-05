using System;
using System.Collections;

// Token: 0x02000190 RID: 400
public class AxeSpinner_Ability : Sword_Ability
{
	// Token: 0x170007D3 RID: 2003
	// (get) Token: 0x06000E85 RID: 3717 RVA: 0x0002C0C9 File Offset: 0x0002A2C9
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170007D4 RID: 2004
	// (get) Token: 0x06000E86 RID: 3718 RVA: 0x0002C0D0 File Offset: 0x0002A2D0
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007D5 RID: 2005
	// (get) Token: 0x06000E87 RID: 3719 RVA: 0x0002C0D7 File Offset: 0x0002A2D7
	protected override float TellAnimSpeed
	{
		get
		{
			return 1.15f;
		}
	}

	// Token: 0x170007D6 RID: 2006
	// (get) Token: 0x06000E88 RID: 3720 RVA: 0x0002C0DE File Offset: 0x0002A2DE
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007D7 RID: 2007
	// (get) Token: 0x06000E89 RID: 3721 RVA: 0x0002C0E5 File Offset: 0x0002A2E5
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.15f;
		}
	}

	// Token: 0x170007D8 RID: 2008
	// (get) Token: 0x06000E8A RID: 3722 RVA: 0x0002C0EC File Offset: 0x0002A2EC
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007D9 RID: 2009
	// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0002C0F3 File Offset: 0x0002A2F3
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170007DA RID: 2010
	// (get) Token: 0x06000E8C RID: 3724 RVA: 0x0002C0FA File Offset: 0x0002A2FA
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007DB RID: 2011
	// (get) Token: 0x06000E8D RID: 3725 RVA: 0x0002C101 File Offset: 0x0002A301
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1.4f;
		}
	}

	// Token: 0x170007DC RID: 2012
	// (get) Token: 0x06000E8E RID: 3726 RVA: 0x0002C108 File Offset: 0x0002A308
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000E8F RID: 3727 RVA: 0x0002C10F File Offset: 0x0002A30F
	protected override void OnEnterExitLogic()
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.FlagForDestruction(null);
		}
		base.OnEnterExitLogic();
	}

	// Token: 0x06000E90 RID: 3728 RVA: 0x0002C130 File Offset: 0x0002A330
	protected override IEnumerator ChangeAnim(float duration)
	{
		if (base.CurrentAbilityAnimState == AbilityAnimState.Attack)
		{
			while (this.m_isSpinning)
			{
				yield return null;
			}
		}
		yield return base.ChangeAnim(duration);
		yield break;
	}

	// Token: 0x06000E91 RID: 3729 RVA: 0x0002C148 File Offset: 0x0002A348
	protected override void OnEnterAttackLogic()
	{
		this.m_isSpinning = true;
		this.m_abilityController.BroadcastAbilityCastEvents(base.CastAbilityType);
		int level = SaveManager.PlayerSaveData.GetRelic(RelicType.AttackCooldown).Level;
		this.m_abilityController.PlayerController.MovementSpeedMod += -0.3f * (float)level;
		this.m_attackCooldownSpeedModApplied = true;
		this.FireProjectile();
	}

	// Token: 0x06000E92 RID: 3730 RVA: 0x0002C1B0 File Offset: 0x0002A3B0
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_isSpinning = false;
		this.StartCooldownTimer();
		if (this.m_attackCooldownSpeedModApplied)
		{
			int level = SaveManager.PlayerSaveData.GetRelic(RelicType.AttackCooldown).Level;
			this.m_abilityController.PlayerController.MovementSpeedMod -= -0.3f * (float)level;
			this.m_attackCooldownSpeedModApplied = false;
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x0002C214 File Offset: 0x0002A414
	protected override void Update()
	{
		base.Update();
		if (!base.AbilityActive)
		{
			return;
		}
		if (Rewired_RL.Player.GetButtonDown(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)) && this.m_isSpinning)
		{
			this.m_isSpinning = false;
			base.CancelChangeAnimCoroutine();
			this.m_animator.Play("AxeGrounded_Exit");
		}
	}

	// Token: 0x0400112A RID: 4394
	private bool m_isSpinning;

	// Token: 0x0400112B RID: 4395
	private bool m_attackCooldownSpeedModApplied;
}
