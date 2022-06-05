using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200018D RID: 397
public class TimeSlow_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x17000795 RID: 1941
	// (get) Token: 0x06000E2D RID: 3629 RVA: 0x0002B8B1 File Offset: 0x00029AB1
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000796 RID: 1942
	// (get) Token: 0x06000E2E RID: 3630 RVA: 0x0002B8B8 File Offset: 0x00029AB8
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000797 RID: 1943
	// (get) Token: 0x06000E2F RID: 3631 RVA: 0x0002B8BF File Offset: 0x00029ABF
	protected override float TellAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000798 RID: 1944
	// (get) Token: 0x06000E30 RID: 3632 RVA: 0x0002B8C6 File Offset: 0x00029AC6
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000799 RID: 1945
	// (get) Token: 0x06000E31 RID: 3633 RVA: 0x0002B8CD File Offset: 0x00029ACD
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x1700079A RID: 1946
	// (get) Token: 0x06000E32 RID: 3634 RVA: 0x0002B8D4 File Offset: 0x00029AD4
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700079B RID: 1947
	// (get) Token: 0x06000E33 RID: 3635 RVA: 0x0002B8DB File Offset: 0x00029ADB
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x1700079C RID: 1948
	// (get) Token: 0x06000E34 RID: 3636 RVA: 0x0002B8E2 File Offset: 0x00029AE2
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x1700079D RID: 1949
	// (get) Token: 0x06000E35 RID: 3637 RVA: 0x0002B8E9 File Offset: 0x00029AE9
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700079E RID: 1950
	// (get) Token: 0x06000E36 RID: 3638 RVA: 0x0002B8F0 File Offset: 0x00029AF0
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x0002B8F7 File Offset: 0x00029AF7
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

	// Token: 0x06000E38 RID: 3640 RVA: 0x0002B923 File Offset: 0x00029B23
	public override IEnumerator CastAbility()
	{
		if (!this.m_spellActive)
		{
			yield break;
		}
		yield return base.CastAbility();
		yield break;
	}

	// Token: 0x06000E39 RID: 3641 RVA: 0x0002B934 File Offset: 0x00029B34
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

	// Token: 0x06000E3A RID: 3642 RVA: 0x0002B9E0 File Offset: 0x00029BE0
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

	// Token: 0x06000E3B RID: 3643 RVA: 0x0002BA44 File Offset: 0x00029C44
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

	// Token: 0x06000E3C RID: 3644 RVA: 0x0002BAF1 File Offset: 0x00029CF1
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

	// Token: 0x04001119 RID: 4377
	private bool m_spellActive;

	// Token: 0x0400111A RID: 4378
	private float m_ticRateStartTime;
}
