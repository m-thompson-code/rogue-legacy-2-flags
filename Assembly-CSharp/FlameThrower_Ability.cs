using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000173 RID: 371
public class FlameThrower_Ability : BaseAbility_RL, ISpell, IAbility
{
	// Token: 0x170006F3 RID: 1779
	// (get) Token: 0x06000CEA RID: 3306 RVA: 0x00027984 File Offset: 0x00025B84
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1.45f;
		}
	}

	// Token: 0x170006F4 RID: 1780
	// (get) Token: 0x06000CEB RID: 3307 RVA: 0x0002798B File Offset: 0x00025B8B
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006F5 RID: 1781
	// (get) Token: 0x06000CEC RID: 3308 RVA: 0x00027992 File Offset: 0x00025B92
	protected override float TellAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170006F6 RID: 1782
	// (get) Token: 0x06000CED RID: 3309 RVA: 0x00027999 File Offset: 0x00025B99
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006F7 RID: 1783
	// (get) Token: 0x06000CEE RID: 3310 RVA: 0x000279A0 File Offset: 0x00025BA0
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170006F8 RID: 1784
	// (get) Token: 0x06000CEF RID: 3311 RVA: 0x000279A7 File Offset: 0x00025BA7
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006F9 RID: 1785
	// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x000279AE File Offset: 0x00025BAE
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170006FA RID: 1786
	// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x000279B5 File Offset: 0x00025BB5
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.075f;
		}
	}

	// Token: 0x170006FB RID: 1787
	// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x000279BC File Offset: 0x00025BBC
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170006FC RID: 1788
	// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x000279C3 File Offset: 0x00025BC3
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000CF4 RID: 3316 RVA: 0x000279CA File Offset: 0x00025BCA
	public override void PreCastAbility()
	{
		if (this.m_isCasting && this.m_frameStopped != Time.frameCount)
		{
			this.StopAbility(false);
			return;
		}
		this.m_frameStarted = Time.frameCount;
		this.m_isCasting = true;
		base.PreCastAbility();
	}

	// Token: 0x06000CF5 RID: 3317 RVA: 0x00027A01 File Offset: 0x00025C01
	protected override IEnumerator ChangeAnim(float duration)
	{
		while (duration > 0f)
		{
			duration -= Time.deltaTime;
			yield return null;
		}
		if (base.CurrentAbilityAnimState == AbilityAnimState.Attack)
		{
			float ticRate = this.m_firedProjectile.RepeatHitDuration;
			while (this.m_abilityController.PlayerController.CurrentMana >= (float)base.ActualCost && this.m_isCasting)
			{
				ticRate -= Time.deltaTime;
				if (ticRate <= 0f)
				{
					this.ApplyAbilityCosts();
					ticRate = this.m_firedProjectile.RepeatHitDuration;
				}
				yield return null;
			}
		}
		this.m_animator.SetTrigger("Change_Ability_Anim");
		base.PerformTurnAnimCheck();
		yield break;
	}

	// Token: 0x06000CF6 RID: 3318 RVA: 0x00027A17 File Offset: 0x00025C17
	protected override void OnEnterAttackLogic()
	{
		this.FireProjectile();
	}

	// Token: 0x06000CF7 RID: 3319 RVA: 0x00027A1F File Offset: 0x00025C1F
	protected override void ApplyAbilityCosts()
	{
		this.m_abilityController.BroadcastAbilityCastEvents(base.CastAbilityType);
		base.ApplyAbilityCosts();
	}

	// Token: 0x06000CF8 RID: 3320 RVA: 0x00027A38 File Offset: 0x00025C38
	protected override void OnExitAttackLogic()
	{
		this.m_firedProjectile.transform.SetParent(null, true);
		this.StartCooldownTimer();
		base.OnExitAttackLogic();
	}

	// Token: 0x06000CF9 RID: 3321 RVA: 0x00027A58 File Offset: 0x00025C58
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.FlagForDestruction(null);
			this.m_firedProjectile = null;
		}
		this.m_isCasting = false;
		this.m_frameStopped = Time.frameCount;
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x06000CFA RID: 3322 RVA: 0x00027A94 File Offset: 0x00025C94
	protected override void Update()
	{
		if (this.m_isCasting && this.m_frameStarted != Time.frameCount && Rewired_RL.Player.GetButtonDown(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
		{
			this.m_isCasting = false;
		}
		base.Update();
	}

	// Token: 0x06000CFB RID: 3323 RVA: 0x00027AE0 File Offset: 0x00025CE0
	private IEnumerator StopProjectileParticleSystem(Projectile_RL projectile)
	{
		ParticleSystem partSys = projectile.GetComponentInChildren<ParticleSystem>();
		if (partSys != null)
		{
			projectile.transform.SetParent(null, true);
			projectile.HitboxController.DisableAllCollisions = true;
			partSys.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			yield return new WaitUntil(() => !partSys.IsAlive());
			projectile.HitboxController.DisableAllCollisions = false;
			projectile.FlagForDestruction(null);
		}
		else
		{
			projectile.FlagForDestruction(null);
		}
		yield break;
	}

	// Token: 0x040010B7 RID: 4279
	private bool m_isCasting;

	// Token: 0x040010B8 RID: 4280
	private int m_frameStopped;

	// Token: 0x040010B9 RID: 4281
	private int m_frameStarted;
}
