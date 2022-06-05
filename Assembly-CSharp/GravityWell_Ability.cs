using System;
using System.Collections;
using FMODUnity;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x02000175 RID: 373
public class GravityWell_Ability : BaseAbility_RL, ISpell, IAbility
{
	// Token: 0x17000707 RID: 1799
	// (get) Token: 0x06000D09 RID: 3337 RVA: 0x00027C0F File Offset: 0x00025E0F
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000708 RID: 1800
	// (get) Token: 0x06000D0A RID: 3338 RVA: 0x00027C16 File Offset: 0x00025E16
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000709 RID: 1801
	// (get) Token: 0x06000D0B RID: 3339 RVA: 0x00027C1D File Offset: 0x00025E1D
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700070A RID: 1802
	// (get) Token: 0x06000D0C RID: 3340 RVA: 0x00027C24 File Offset: 0x00025E24
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x1700070B RID: 1803
	// (get) Token: 0x06000D0D RID: 3341 RVA: 0x00027C2B File Offset: 0x00025E2B
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x1700070C RID: 1804
	// (get) Token: 0x06000D0E RID: 3342 RVA: 0x00027C32 File Offset: 0x00025E32
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700070D RID: 1805
	// (get) Token: 0x06000D0F RID: 3343 RVA: 0x00027C39 File Offset: 0x00025E39
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x1700070E RID: 1806
	// (get) Token: 0x06000D10 RID: 3344 RVA: 0x00027C40 File Offset: 0x00025E40
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x1700070F RID: 1807
	// (get) Token: 0x06000D11 RID: 3345 RVA: 0x00027C47 File Offset: 0x00025E47
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000710 RID: 1808
	// (get) Token: 0x06000D12 RID: 3346 RVA: 0x00027C4E File Offset: 0x00025E4E
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000D13 RID: 3347 RVA: 0x00027C55 File Offset: 0x00025E55
	public override void PreCastAbility()
	{
		base.PreCastAbility();
		this.m_chargeEmitter.Play();
	}

	// Token: 0x06000D14 RID: 3348 RVA: 0x00027C68 File Offset: 0x00025E68
	public override IEnumerator CastAbility()
	{
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.DisableHorizontalMovement)
		{
			this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
			this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
			this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(false);
		}
		this.m_castStartTime = 0f;
		yield return base.CastAbility();
		yield break;
	}

	// Token: 0x06000D15 RID: 3349 RVA: 0x00027C77 File Offset: 0x00025E77
	protected override void FireProjectile()
	{
		this.m_chargeEmitter.Stop();
		base.FireProjectile();
		this.m_castStartTime = Time.time;
	}

	// Token: 0x06000D16 RID: 3350 RVA: 0x00027C95 File Offset: 0x00025E95
	protected override IEnumerator ChangeAnim(float duration)
	{
		if (base.CurrentAbilityAnimState == AbilityAnimState.Attack)
		{
			while (Time.time < this.m_castStartTime + this.m_castDuration)
			{
				yield return null;
			}
		}
		yield return base.ChangeAnim(duration);
		yield break;
	}

	// Token: 0x06000D17 RID: 3351 RVA: 0x00027CAC File Offset: 0x00025EAC
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.FlagForDestruction(null);
			this.m_firedProjectile = null;
		}
		this.m_chargeEmitter.Stop();
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(true);
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x040010BC RID: 4284
	[SerializeField]
	private StudioEventEmitter m_chargeEmitter;

	// Token: 0x040010BD RID: 4285
	private float m_castDuration = 0.75f;

	// Token: 0x040010BE RID: 4286
	private float m_castStartTime;
}
