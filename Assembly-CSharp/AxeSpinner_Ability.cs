using System;
using System.Collections;

// Token: 0x020002E2 RID: 738
public class AxeSpinner_Ability : Sword_Ability
{
	// Token: 0x17000A63 RID: 2659
	// (get) Token: 0x0600163C RID: 5692 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000A64 RID: 2660
	// (get) Token: 0x0600163D RID: 5693 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A65 RID: 2661
	// (get) Token: 0x0600163E RID: 5694 RVA: 0x00004FDE File Offset: 0x000031DE
	protected override float TellAnimSpeed
	{
		get
		{
			return 1.15f;
		}
	}

	// Token: 0x17000A66 RID: 2662
	// (get) Token: 0x0600163F RID: 5695 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A67 RID: 2663
	// (get) Token: 0x06001640 RID: 5696 RVA: 0x00004FDE File Offset: 0x000031DE
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.15f;
		}
	}

	// Token: 0x17000A68 RID: 2664
	// (get) Token: 0x06001641 RID: 5697 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A69 RID: 2665
	// (get) Token: 0x06001642 RID: 5698 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000A6A RID: 2666
	// (get) Token: 0x06001643 RID: 5699 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A6B RID: 2667
	// (get) Token: 0x06001644 RID: 5700 RVA: 0x00003DE8 File Offset: 0x00001FE8
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1.4f;
		}
	}

	// Token: 0x17000A6C RID: 2668
	// (get) Token: 0x06001645 RID: 5701 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001646 RID: 5702 RVA: 0x0000B168 File Offset: 0x00009368
	protected override void OnEnterExitLogic()
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.FlagForDestruction(null);
		}
		base.OnEnterExitLogic();
	}

	// Token: 0x06001647 RID: 5703 RVA: 0x0000B189 File Offset: 0x00009389
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

	// Token: 0x06001648 RID: 5704 RVA: 0x0008B20C File Offset: 0x0008940C
	protected override void OnEnterAttackLogic()
	{
		this.m_isSpinning = true;
		this.m_abilityController.BroadcastAbilityCastEvents(base.CastAbilityType);
		int level = SaveManager.PlayerSaveData.GetRelic(RelicType.AttackCooldown).Level;
		this.m_abilityController.PlayerController.MovementSpeedMod += -0.3f * (float)level;
		this.m_attackCooldownSpeedModApplied = true;
		this.FireProjectile();
	}

	// Token: 0x06001649 RID: 5705 RVA: 0x0008B274 File Offset: 0x00089474
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

	// Token: 0x0600164A RID: 5706 RVA: 0x0008B2D8 File Offset: 0x000894D8
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

	// Token: 0x040016BF RID: 5823
	private bool m_isSpinning;

	// Token: 0x040016C0 RID: 5824
	private bool m_attackCooldownSpeedModApplied;
}
