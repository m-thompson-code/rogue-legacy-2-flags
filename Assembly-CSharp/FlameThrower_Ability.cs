using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002AD RID: 685
public class FlameThrower_Ability : BaseAbility_RL, ISpell, IAbility
{
	// Token: 0x17000955 RID: 2389
	// (get) Token: 0x06001415 RID: 5141 RVA: 0x0000A305 File Offset: 0x00008505
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1.45f;
		}
	}

	// Token: 0x17000956 RID: 2390
	// (get) Token: 0x06001416 RID: 5142 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000957 RID: 2391
	// (get) Token: 0x06001417 RID: 5143 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float TellAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000958 RID: 2392
	// (get) Token: 0x06001418 RID: 5144 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000959 RID: 2393
	// (get) Token: 0x06001419 RID: 5145 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x1700095A RID: 2394
	// (get) Token: 0x0600141A RID: 5146 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700095B RID: 2395
	// (get) Token: 0x0600141B RID: 5147 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700095C RID: 2396
	// (get) Token: 0x0600141C RID: 5148 RVA: 0x00003D8C File Offset: 0x00001F8C
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.075f;
		}
	}

	// Token: 0x1700095D RID: 2397
	// (get) Token: 0x0600141D RID: 5149 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700095E RID: 2398
	// (get) Token: 0x0600141E RID: 5150 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600141F RID: 5151 RVA: 0x0000A30C File Offset: 0x0000850C
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

	// Token: 0x06001420 RID: 5152 RVA: 0x0000A343 File Offset: 0x00008543
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

	// Token: 0x06001421 RID: 5153 RVA: 0x0000A359 File Offset: 0x00008559
	protected override void OnEnterAttackLogic()
	{
		this.FireProjectile();
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x0000A361 File Offset: 0x00008561
	protected override void ApplyAbilityCosts()
	{
		this.m_abilityController.BroadcastAbilityCastEvents(base.CastAbilityType);
		base.ApplyAbilityCosts();
	}

	// Token: 0x06001423 RID: 5155 RVA: 0x0000A37A File Offset: 0x0000857A
	protected override void OnExitAttackLogic()
	{
		this.m_firedProjectile.transform.SetParent(null, true);
		this.StartCooldownTimer();
		base.OnExitAttackLogic();
	}

	// Token: 0x06001424 RID: 5156 RVA: 0x0000A39A File Offset: 0x0000859A
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

	// Token: 0x06001425 RID: 5157 RVA: 0x00086940 File Offset: 0x00084B40
	protected override void Update()
	{
		if (this.m_isCasting && this.m_frameStarted != Time.frameCount && Rewired_RL.Player.GetButtonDown(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
		{
			this.m_isCasting = false;
		}
		base.Update();
	}

	// Token: 0x06001426 RID: 5158 RVA: 0x0000A3D5 File Offset: 0x000085D5
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

	// Token: 0x040015F9 RID: 5625
	private bool m_isCasting;

	// Token: 0x040015FA RID: 5626
	private int m_frameStopped;

	// Token: 0x040015FB RID: 5627
	private int m_frameStarted;
}
