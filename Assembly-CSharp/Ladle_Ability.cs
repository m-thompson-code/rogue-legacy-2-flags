using System;
using UnityEngine;

// Token: 0x0200019E RID: 414
public class Ladle_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x06000FCC RID: 4044 RVA: 0x0002DFF4 File Offset: 0x0002C1F4
	protected override void Awake()
	{
		base.Awake();
		this.m_onProjectileCollision = new Action<Projectile_RL, GameObject>(this.OnProjectileCollision);
	}

	// Token: 0x170008C4 RID: 2244
	// (get) Token: 0x06000FCD RID: 4045 RVA: 0x0002E00E File Offset: 0x0002C20E
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170008C5 RID: 2245
	// (get) Token: 0x06000FCE RID: 4046 RVA: 0x0002E015 File Offset: 0x0002C215
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0.01f;
		}
	}

	// Token: 0x170008C6 RID: 2246
	// (get) Token: 0x06000FCF RID: 4047 RVA: 0x0002E01C File Offset: 0x0002C21C
	protected override float TellAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170008C7 RID: 2247
	// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x0002E023 File Offset: 0x0002C223
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008C8 RID: 2248
	// (get) Token: 0x06000FD1 RID: 4049 RVA: 0x0002E02A File Offset: 0x0002C22A
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170008C9 RID: 2249
	// (get) Token: 0x06000FD2 RID: 4050 RVA: 0x0002E031 File Offset: 0x0002C231
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008CA RID: 2250
	// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x0002E038 File Offset: 0x0002C238
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170008CB RID: 2251
	// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x0002E03F File Offset: 0x0002C23F
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.075f;
		}
	}

	// Token: 0x170008CC RID: 2252
	// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x0002E046 File Offset: 0x0002C246
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170008CD RID: 2253
	// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x0002E04D File Offset: 0x0002C24D
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000FD7 RID: 4055 RVA: 0x0002E054 File Offset: 0x0002C254
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		abilityController.Animator.SetBool("FryingPan_UsePirateAudio", false);
	}

	// Token: 0x06000FD8 RID: 4056 RVA: 0x0002E06F File Offset: 0x0002C26F
	protected override void FireProjectile()
	{
		base.FireProjectile();
		this.m_firedProjectile.OnCollisionRelay.AddListener(this.m_onProjectileCollision, false);
	}

	// Token: 0x06000FD9 RID: 4057 RVA: 0x0002E090 File Offset: 0x0002C290
	private void OnProjectileCollision(Projectile_RL proj, GameObject colliderObj)
	{
		if (CollisionType_RL.IsProjectile(colliderObj))
		{
			this.m_abilityController.PlayerController.StatusEffectController.StartStatusEffect(StatusEffectType.Player_FreeCrit, 3f, null);
			if (this.m_firedProjectile)
			{
				this.m_firedProjectile.OnCollisionRelay.RemoveListener(this.m_onProjectileCollision);
			}
		}
	}

	// Token: 0x06000FDA RID: 4058 RVA: 0x0002E0E9 File Offset: 0x0002C2E9
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.OnCollisionRelay.RemoveListener(this.m_onProjectileCollision);
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x0400118C RID: 4492
	private Action<Projectile_RL, GameObject> m_onProjectileCollision;
}
