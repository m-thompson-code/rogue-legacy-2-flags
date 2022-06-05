using System;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020002D7 RID: 727
public class SpearSpin_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x17000A11 RID: 2577
	// (get) Token: 0x060015AD RID: 5549 RVA: 0x00004536 File Offset: 0x00002736
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000A12 RID: 2578
	// (get) Token: 0x060015AE RID: 5550 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A13 RID: 2579
	// (get) Token: 0x060015AF RID: 5551 RVA: 0x00004536 File Offset: 0x00002736
	protected override float TellAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000A14 RID: 2580
	// (get) Token: 0x060015B0 RID: 5552 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A15 RID: 2581
	// (get) Token: 0x060015B1 RID: 5553 RVA: 0x00004FFB File Offset: 0x000031FB
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000A16 RID: 2582
	// (get) Token: 0x060015B2 RID: 5554 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A17 RID: 2583
	// (get) Token: 0x060015B3 RID: 5555 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000A18 RID: 2584
	// (get) Token: 0x060015B4 RID: 5556 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A19 RID: 2585
	// (get) Token: 0x060015B5 RID: 5557 RVA: 0x00004536 File Offset: 0x00002736
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000A1A RID: 2586
	// (get) Token: 0x060015B6 RID: 5558 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060015B7 RID: 5559 RVA: 0x0000ABEA File Offset: 0x00008DEA
	protected override void Awake()
	{
		base.Awake();
		this.m_onCollision = new Action<Projectile_RL, GameObject>(this.OnCollision);
	}

	// Token: 0x060015B8 RID: 5560 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void FireProjectile()
	{
	}

	// Token: 0x060015B9 RID: 5561 RVA: 0x0008A824 File Offset: 0x00088A24
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

	// Token: 0x060015BA RID: 5562 RVA: 0x0008A898 File Offset: 0x00088A98
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

	// Token: 0x060015BB RID: 5563 RVA: 0x0000AC04 File Offset: 0x00008E04
	public override void StartCooldownTimer()
	{
		if (this.m_spearSpinStartTime < this.m_spearSpinSuccessStartTime + 0.05f)
		{
			return;
		}
		base.StartCooldownTimer();
	}

	// Token: 0x060015BC RID: 5564 RVA: 0x0000AC21 File Offset: 0x00008E21
	private void RegenMana(GameObject colliderObj)
	{
		this.m_regenEventArgs.Initialise(15f, false);
		EffectManager.PlayEffect(colliderObj, null, "ManaRegenBurst_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerForceManaRegen, this, this.m_regenEventArgs);
	}

	// Token: 0x060015BD RID: 5565 RVA: 0x0008A93C File Offset: 0x00088B3C
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

	// Token: 0x04001693 RID: 5779
	private bool m_hitProjectilesWhileActive;

	// Token: 0x04001694 RID: 5780
	private float m_spearSpinSuccessStartTime;

	// Token: 0x04001695 RID: 5781
	private float m_spearSpinStartTime;

	// Token: 0x04001696 RID: 5782
	private Action<Projectile_RL, GameObject> m_onCollision;

	// Token: 0x04001697 RID: 5783
	public Relay<bool> SpinCompleteRelay = new Relay<bool>();

	// Token: 0x04001698 RID: 5784
	public Relay HitProjectileRelay = new Relay();

	// Token: 0x04001699 RID: 5785
	private ForceManaRegenEventArgs m_regenEventArgs = new ForceManaRegenEventArgs(0f, false);
}
