using System;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x0200018A RID: 394
public class SpearSpin_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x1700078B RID: 1931
	// (get) Token: 0x06000E14 RID: 3604 RVA: 0x0002B55F File Offset: 0x0002975F
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700078C RID: 1932
	// (get) Token: 0x06000E15 RID: 3605 RVA: 0x0002B566 File Offset: 0x00029766
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700078D RID: 1933
	// (get) Token: 0x06000E16 RID: 3606 RVA: 0x0002B56D File Offset: 0x0002976D
	protected override float TellAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700078E RID: 1934
	// (get) Token: 0x06000E17 RID: 3607 RVA: 0x0002B574 File Offset: 0x00029774
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700078F RID: 1935
	// (get) Token: 0x06000E18 RID: 3608 RVA: 0x0002B57B File Offset: 0x0002977B
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000790 RID: 1936
	// (get) Token: 0x06000E19 RID: 3609 RVA: 0x0002B582 File Offset: 0x00029782
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000791 RID: 1937
	// (get) Token: 0x06000E1A RID: 3610 RVA: 0x0002B589 File Offset: 0x00029789
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000792 RID: 1938
	// (get) Token: 0x06000E1B RID: 3611 RVA: 0x0002B590 File Offset: 0x00029790
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000793 RID: 1939
	// (get) Token: 0x06000E1C RID: 3612 RVA: 0x0002B597 File Offset: 0x00029797
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000794 RID: 1940
	// (get) Token: 0x06000E1D RID: 3613 RVA: 0x0002B59E File Offset: 0x0002979E
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x0002B5A5 File Offset: 0x000297A5
	protected override void Awake()
	{
		base.Awake();
		this.m_onCollision = new Action<Projectile_RL, GameObject>(this.OnCollision);
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x0002B5BF File Offset: 0x000297BF
	protected override void FireProjectile()
	{
	}

	// Token: 0x06000E20 RID: 3616 RVA: 0x0002B5C4 File Offset: 0x000297C4
	public override void PreCastAbility()
	{
		this.m_abilityController.PlayerController.PauseGravity(true, true);
		this.m_abilityController.PlayerController.IsSpearSpinning = true;
		base.PreCastAbility();
		this.m_hitProjectilesWhileActive = false;
		this.m_spearSpinStartTime = Time.time;
		base.FireProjectile();
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.OnCollisionRelay.AddListener(this.m_onCollision, false);
		}
	}

	// Token: 0x06000E21 RID: 3617 RVA: 0x0002B638 File Offset: 0x00029838
	protected void OnCollision(Projectile_RL projectile, GameObject colliderObj)
	{
		if (colliderObj.CompareTag("EnemyProjectile"))
		{
			this.m_hitProjectilesWhileActive = true;
			this.HitProjectileRelay.Dispatch();
			EffectManager.SetEffectParams("SlowTime_Effect", new object[]
			{
				"m_timeScaleValue",
				0.05f
			});
			EffectManager.PlayEffect(this.m_abilityController.PlayerController.gameObject, this.m_abilityController.PlayerController.Animator, "SlowTime_Effect", Vector3.zero, 0.175f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			this.RegenMana(colliderObj);
			base.EndCooldownTimer(false);
			this.m_spearSpinSuccessStartTime = Time.time;
		}
	}

	// Token: 0x06000E22 RID: 3618 RVA: 0x0002B6DC File Offset: 0x000298DC
	public override void StartCooldownTimer()
	{
		if (this.m_spearSpinStartTime < this.m_spearSpinSuccessStartTime + 0.05f)
		{
			return;
		}
		base.StartCooldownTimer();
	}

	// Token: 0x06000E23 RID: 3619 RVA: 0x0002B6F9 File Offset: 0x000298F9
	private void RegenMana(GameObject colliderObj)
	{
		this.m_regenEventArgs.Initialise(15f, false);
		EffectManager.PlayEffect(colliderObj, null, "ManaRegenBurst_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerForceManaRegen, this, this.m_regenEventArgs);
	}

	// Token: 0x06000E24 RID: 3620 RVA: 0x0002B734 File Offset: 0x00029934
	public override void StopAbility(bool abilityInterrupted)
	{
		this.SpinCompleteRelay.Dispatch(this.m_hitProjectilesWhileActive);
		this.m_abilityController.PlayerController.ResumeGravity();
		this.m_abilityController.PlayerController.IsSpearSpinning = false;
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.OnCollisionRelay.RemoveListener(this.m_onCollision);
			this.m_firedProjectile.FlagForDestruction(null);
			this.m_firedProjectile = null;
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x04001110 RID: 4368
	private bool m_hitProjectilesWhileActive;

	// Token: 0x04001111 RID: 4369
	private float m_spearSpinSuccessStartTime;

	// Token: 0x04001112 RID: 4370
	private float m_spearSpinStartTime;

	// Token: 0x04001113 RID: 4371
	private Action<Projectile_RL, GameObject> m_onCollision;

	// Token: 0x04001114 RID: 4372
	public Relay<bool> SpinCompleteRelay = new Relay<bool>();

	// Token: 0x04001115 RID: 4373
	public Relay HitProjectileRelay = new Relay();

	// Token: 0x04001116 RID: 4374
	private ForceManaRegenEventArgs m_regenEventArgs = new ForceManaRegenEventArgs(0f, false);
}
