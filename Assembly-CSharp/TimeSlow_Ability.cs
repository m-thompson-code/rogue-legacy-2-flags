using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002DB RID: 731
public class TimeSlow_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x17000A1D RID: 2589
	// (get) Token: 0x060015CC RID: 5580 RVA: 0x00004536 File Offset: 0x00002736
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000A1E RID: 2590
	// (get) Token: 0x060015CD RID: 5581 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A1F RID: 2591
	// (get) Token: 0x060015CE RID: 5582 RVA: 0x00004536 File Offset: 0x00002736
	protected override float TellAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000A20 RID: 2592
	// (get) Token: 0x060015CF RID: 5583 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A21 RID: 2593
	// (get) Token: 0x060015D0 RID: 5584 RVA: 0x00004FFB File Offset: 0x000031FB
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000A22 RID: 2594
	// (get) Token: 0x060015D1 RID: 5585 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A23 RID: 2595
	// (get) Token: 0x060015D2 RID: 5586 RVA: 0x00004FFB File Offset: 0x000031FB
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000A24 RID: 2596
	// (get) Token: 0x060015D3 RID: 5587 RVA: 0x000052A9 File Offset: 0x000034A9
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x17000A25 RID: 2597
	// (get) Token: 0x060015D4 RID: 5588 RVA: 0x00004536 File Offset: 0x00002736
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000A26 RID: 2598
	// (get) Token: 0x060015D5 RID: 5589 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060015D6 RID: 5590 RVA: 0x0000AD0D File Offset: 0x00008F0D
	public override void PreCastAbility()
	{
		if (this.m_spellActive)
		{
			this.m_spellActive = false;
			this.StopAbility(true);
			this.StopSlowdown();
			return;
		}
		this.m_spellActive = true;
		base.PreCastAbility();
	}

	// Token: 0x060015D7 RID: 5591 RVA: 0x0000AD39 File Offset: 0x00008F39
	public override IEnumerator CastAbility()
	{
		if (!this.m_spellActive)
		{
			yield break;
		}
		yield return base.CastAbility();
		yield break;
	}

	// Token: 0x060015D8 RID: 5592 RVA: 0x0008AAC8 File Offset: 0x00088CC8
	protected override void FireProjectile()
	{
		RLTimeScale.SetTimeScale(TimeScaleType.TimeSlowAbility, 0.2f);
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.CharacterMove.WalkSpeed *= 5f;
		playerController.CharacterMove.MovementSpeed = playerController.CharacterMove.WalkSpeed;
		playerController.AscentMultiplierOverride *= 5f;
		playerController.FallMultiplierOverride = 5f;
		playerController.CharacterJump.JumpHeightMultiplier = 5f;
		playerController.Animator.speed *= 5f;
		playerController.CharacterDash.DashForce *= 5f;
		this.ApplyAbilityCosts();
	}

	// Token: 0x060015D9 RID: 5593 RVA: 0x0008AB74 File Offset: 0x00088D74
	protected override void ApplyAbilityCosts()
	{
		if ((base.MaxAmmo > 0 && base.CurrentAmmo <= 0) || (base.ActualCost > 0 && this.m_abilityController.PlayerController.CurrentManaAsInt <= 0))
		{
			this.StopAbility(true);
			this.StopSlowdown();
			this.m_spellActive = false;
			return;
		}
		this.m_ticRateStartTime = Time.time;
		base.ApplyAbilityCosts();
	}

	// Token: 0x060015DA RID: 5594 RVA: 0x0008ABD8 File Offset: 0x00088DD8
	private void StopSlowdown()
	{
		RLTimeScale.SetTimeScale(TimeScaleType.TimeSlowAbility, 1f);
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.CharacterMove.WalkSpeed /= 5f;
		playerController.CharacterMove.MovementSpeed = playerController.CharacterMove.WalkSpeed;
		playerController.AscentMultiplierOverride /= 5f;
		playerController.FallMultiplierOverride /= 5f;
		playerController.Animator.speed /= 5f;
		playerController.CharacterDash.DashForce /= 5f;
		playerController.CharacterJump.JumpHeightMultiplier = 1f;
	}

	// Token: 0x060015DB RID: 5595 RVA: 0x0000AD48 File Offset: 0x00008F48
	protected void FixedUpdate()
	{
		if (!this.m_spellActive)
		{
			return;
		}
		if (Time.time >= this.m_ticRateStartTime + 0.5f)
		{
			this.ApplyAbilityCosts();
		}
	}

	// Token: 0x040016A0 RID: 5792
	private bool m_spellActive;

	// Token: 0x040016A1 RID: 5793
	private float m_ticRateStartTime;
}
