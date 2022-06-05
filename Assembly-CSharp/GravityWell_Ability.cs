using System;
using System.Collections;
using FMODUnity;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020002B2 RID: 690
public class GravityWell_Ability : BaseAbility_RL, ISpell, IAbility
{
	// Token: 0x1700096D RID: 2413
	// (get) Token: 0x06001442 RID: 5186 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700096E RID: 2414
	// (get) Token: 0x06001443 RID: 5187 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700096F RID: 2415
	// (get) Token: 0x06001444 RID: 5188 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000970 RID: 2416
	// (get) Token: 0x06001445 RID: 5189 RVA: 0x000050CB File Offset: 0x000032CB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x17000971 RID: 2417
	// (get) Token: 0x06001446 RID: 5190 RVA: 0x00004FFB File Offset: 0x000031FB
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000972 RID: 2418
	// (get) Token: 0x06001447 RID: 5191 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000973 RID: 2419
	// (get) Token: 0x06001448 RID: 5192 RVA: 0x00004FFB File Offset: 0x000031FB
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000974 RID: 2420
	// (get) Token: 0x06001449 RID: 5193 RVA: 0x0000456C File Offset: 0x0000276C
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x17000975 RID: 2421
	// (get) Token: 0x0600144A RID: 5194 RVA: 0x00004536 File Offset: 0x00002736
	protected override float ExitAnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000976 RID: 2422
	// (get) Token: 0x0600144B RID: 5195 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600144C RID: 5196 RVA: 0x0000A431 File Offset: 0x00008631
	public override void PreCastAbility()
	{
		base.PreCastAbility();
		this.m_chargeEmitter.Play();
	}

	// Token: 0x0600144D RID: 5197 RVA: 0x0000A444 File Offset: 0x00008644
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

	// Token: 0x0600144E RID: 5198 RVA: 0x0000A453 File Offset: 0x00008653
	protected override void FireProjectile()
	{
		this.m_chargeEmitter.Stop();
		base.FireProjectile();
		this.m_castStartTime = Time.time;
	}

	// Token: 0x0600144F RID: 5199 RVA: 0x0000A471 File Offset: 0x00008671
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

	// Token: 0x06001450 RID: 5200 RVA: 0x00086C2C File Offset: 0x00084E2C
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

	// Token: 0x04001607 RID: 5639
	[SerializeField]
	private StudioEventEmitter m_chargeEmitter;

	// Token: 0x04001608 RID: 5640
	private float m_castDuration = 0.75f;

	// Token: 0x04001609 RID: 5641
	private float m_castStartTime;
}
