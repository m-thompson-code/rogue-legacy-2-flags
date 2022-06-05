using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002E0 RID: 736
public class AxeHold_Ability : Sword_Ability
{
	// Token: 0x06001602 RID: 5634 RVA: 0x0000AE34 File Offset: 0x00009034
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_airborneProjectileName,
			this.m_landingProjectileName
		};
	}

	// Token: 0x17000A38 RID: 2616
	// (get) Token: 0x06001603 RID: 5635 RVA: 0x00003C70 File Offset: 0x00001E70
	protected float TellIntroAnimSpeedGround
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000A39 RID: 2617
	// (get) Token: 0x06001604 RID: 5636 RVA: 0x00003C70 File Offset: 0x00001E70
	protected float TellIntroAnimSpeedAir
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000A3A RID: 2618
	// (get) Token: 0x06001605 RID: 5637 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellIntroAnimExitDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A3B RID: 2619
	// (get) Token: 0x06001606 RID: 5638 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellIntroAnimExitDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A3C RID: 2620
	// (get) Token: 0x06001607 RID: 5639 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected float TellHoldAnimSpeedGround
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000A3D RID: 2621
	// (get) Token: 0x06001608 RID: 5640 RVA: 0x00004FDE File Offset: 0x000031DE
	protected float TellHoldAnimSpeedAir
	{
		get
		{
			return 1.15f;
		}
	}

	// Token: 0x17000A3E RID: 2622
	// (get) Token: 0x06001609 RID: 5641 RVA: 0x0000AE5D File Offset: 0x0000905D
	protected float TellHoldAnimDelayGround
	{
		get
		{
			return 0.025f;
		}
	}

	// Token: 0x17000A3F RID: 2623
	// (get) Token: 0x0600160A RID: 5642 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A40 RID: 2624
	// (get) Token: 0x0600160B RID: 5643 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected float AttackIntroAnimSpeedGround
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000A41 RID: 2625
	// (get) Token: 0x0600160C RID: 5644 RVA: 0x00004FDE File Offset: 0x000031DE
	protected float AttackIntroAnimSpeedAir
	{
		get
		{
			return 1.15f;
		}
	}

	// Token: 0x17000A42 RID: 2626
	// (get) Token: 0x0600160D RID: 5645 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackIntroAnimExitDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A43 RID: 2627
	// (get) Token: 0x0600160E RID: 5646 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackIntroAnimExitDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A44 RID: 2628
	// (get) Token: 0x0600160F RID: 5647 RVA: 0x00005FAA File Offset: 0x000041AA
	protected float AttackHoldAnimSpeedGround
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x17000A45 RID: 2629
	// (get) Token: 0x06001610 RID: 5648 RVA: 0x00005FAA File Offset: 0x000041AA
	protected float AttackHoldAnimSpeedAir
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x17000A46 RID: 2630
	// (get) Token: 0x06001611 RID: 5649 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackHoldAnimDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A47 RID: 2631
	// (get) Token: 0x06001612 RID: 5650 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A48 RID: 2632
	// (get) Token: 0x06001613 RID: 5651 RVA: 0x00005307 File Offset: 0x00003507
	protected float ExitIntroAnimSpeedGround
	{
		get
		{
			return 1.6f;
		}
	}

	// Token: 0x17000A49 RID: 2633
	// (get) Token: 0x06001614 RID: 5652 RVA: 0x00003DE8 File Offset: 0x00001FE8
	protected float ExitIntroAnimSpeedAir
	{
		get
		{
			return 1.4f;
		}
	}

	// Token: 0x17000A4A RID: 2634
	// (get) Token: 0x06001615 RID: 5653 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float ExitHoldAnimDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A4B RID: 2635
	// (get) Token: 0x06001616 RID: 5654 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float ExitHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A4C RID: 2636
	// (get) Token: 0x06001617 RID: 5655 RVA: 0x0000AE64 File Offset: 0x00009064
	public override Vector2 ProjectileOffset
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return base.ProjectileOffset;
			}
			if (this.PerformLandingAttack)
			{
				return this.m_landingProjectileOffset;
			}
			return this.m_airBorneProjectileOffset;
		}
	}

	// Token: 0x17000A4D RID: 2637
	// (get) Token: 0x06001618 RID: 5656 RVA: 0x0000AE8A File Offset: 0x0000908A
	protected override float TellIntroAnimSpeed
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.TellIntroAnimSpeedGround;
			}
			return this.TellIntroAnimSpeedAir;
		}
	}

	// Token: 0x17000A4E RID: 2638
	// (get) Token: 0x06001619 RID: 5657 RVA: 0x0000AEA1 File Offset: 0x000090A1
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.TellIntroAnimExitDelayGround;
			}
			return this.TellIntroAnimExitDelayAir;
		}
	}

	// Token: 0x17000A4F RID: 2639
	// (get) Token: 0x0600161A RID: 5658 RVA: 0x0000AEB8 File Offset: 0x000090B8
	protected override float TellAnimSpeed
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.TellHoldAnimSpeedGround;
			}
			return this.TellHoldAnimSpeedAir;
		}
	}

	// Token: 0x17000A50 RID: 2640
	// (get) Token: 0x0600161B RID: 5659 RVA: 0x0000AECF File Offset: 0x000090CF
	protected override float TellAnimExitDelay
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.TellHoldAnimDelayGround;
			}
			return this.TellHoldAnimDelayAir;
		}
	}

	// Token: 0x17000A51 RID: 2641
	// (get) Token: 0x0600161C RID: 5660 RVA: 0x0000AEE6 File Offset: 0x000090E6
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.AttackIntroAnimSpeedGround;
			}
			return this.AttackIntroAnimSpeedAir;
		}
	}

	// Token: 0x17000A52 RID: 2642
	// (get) Token: 0x0600161D RID: 5661 RVA: 0x0000AEFD File Offset: 0x000090FD
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.AttackIntroAnimExitDelayGround;
			}
			return this.AttackIntroAnimExitDelayAir;
		}
	}

	// Token: 0x17000A53 RID: 2643
	// (get) Token: 0x0600161E RID: 5662 RVA: 0x0000AF14 File Offset: 0x00009114
	protected override float AttackAnimSpeed
	{
		get
		{
			if (this.IsGrounded)
			{
				return this.AttackHoldAnimSpeedGround;
			}
			return this.AttackHoldAnimSpeedAir;
		}
	}

	// Token: 0x17000A54 RID: 2644
	// (get) Token: 0x0600161F RID: 5663 RVA: 0x0000AF2B File Offset: 0x0000912B
	protected override float AttackAnimExitDelay
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.AttackHoldAnimDelayGround;
			}
			return this.AttackHoldAnimDelayAir;
		}
	}

	// Token: 0x17000A55 RID: 2645
	// (get) Token: 0x06001620 RID: 5664 RVA: 0x0000AF42 File Offset: 0x00009142
	protected override float ExitAnimSpeed
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.ExitIntroAnimSpeedGround;
			}
			return this.ExitIntroAnimSpeedAir;
		}
	}

	// Token: 0x17000A56 RID: 2646
	// (get) Token: 0x06001621 RID: 5665 RVA: 0x0000AF59 File Offset: 0x00009159
	protected override float ExitAnimExitDelay
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return this.ExitHoldAnimDelayGround;
			}
			return this.ExitHoldAnimDelayAir;
		}
	}

	// Token: 0x17000A57 RID: 2647
	// (get) Token: 0x06001622 RID: 5666 RVA: 0x0000AF70 File Offset: 0x00009170
	public override bool HasAttackFlipCheck
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return base.HasAttackFlipCheck;
			}
			return this.m_hasAirborneAttackFlipCheck;
		}
	}

	// Token: 0x17000A58 RID: 2648
	// (get) Token: 0x06001623 RID: 5667 RVA: 0x0000AF87 File Offset: 0x00009187
	public override string ProjectileName
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return base.ProjectileName;
			}
			if (this.PerformLandingAttack)
			{
				return this.m_landingProjectileName;
			}
			return this.m_airborneProjectileName;
		}
	}

	// Token: 0x17000A59 RID: 2649
	// (get) Token: 0x06001624 RID: 5668 RVA: 0x0008AEF4 File Offset: 0x000890F4
	public override AbilityData AbilityData
	{
		get
		{
			if (!base.AbilityActive)
			{
				if (this.IsGrounded)
				{
					return this.m_landingAbilityData;
				}
				return this.m_airborneAbilityData;
			}
			else
			{
				if (this.PerformGroundAttack)
				{
					return base.AbilityData;
				}
				if (this.PerformLandingAttack)
				{
					return this.m_landingAbilityData;
				}
				return this.m_airborneAbilityData;
			}
		}
	}

	// Token: 0x17000A5A RID: 2650
	// (get) Token: 0x06001625 RID: 5669 RVA: 0x0000AFAD File Offset: 0x000091AD
	public override string AbilityTellIntroName
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return base.AbilityTellIntroName;
			}
			return this.m_abilityAirTellIntroName;
		}
	}

	// Token: 0x17000A5B RID: 2651
	// (get) Token: 0x06001626 RID: 5670 RVA: 0x0000AFC4 File Offset: 0x000091C4
	// (set) Token: 0x06001627 RID: 5671 RVA: 0x0000AFCC File Offset: 0x000091CC
	private protected bool PerformGroundAttack { protected get; private set; }

	// Token: 0x17000A5C RID: 2652
	// (get) Token: 0x06001628 RID: 5672 RVA: 0x0000AFD5 File Offset: 0x000091D5
	protected bool PerformLandingAttack
	{
		get
		{
			return !this.PerformGroundAttack && this.IsGrounded && this.m_isAirAttacking && base.CurrentAbilityAnimState == AbilityAnimState.Exit;
		}
	}

	// Token: 0x17000A5D RID: 2653
	// (get) Token: 0x06001629 RID: 5673 RVA: 0x0008AF44 File Offset: 0x00089144
	protected bool IsGrounded
	{
		get
		{
			return !(this.m_abilityController != null) || (this.m_abilityController.PlayerController.IsGrounded && (!this.m_abilityController.PlayerController.IsGrounded || !this.m_abilityController.PlayerController.CharacterJump.JumpHappenedThisFrame));
		}
	}

	// Token: 0x17000A5E RID: 2654
	// (get) Token: 0x0600162A RID: 5674 RVA: 0x0000AFFA File Offset: 0x000091FA
	public override float MovementMod
	{
		get
		{
			if (!this.PerformGroundAttack && this.IsGrounded)
			{
				return this.AttackMovementModAfterSpinLand;
			}
			return base.MovementMod;
		}
	}

	// Token: 0x17000A5F RID: 2655
	// (get) Token: 0x0600162B RID: 5675 RVA: 0x0000B019 File Offset: 0x00009219
	public override bool JumpInterruptable
	{
		get
		{
			return (this.PerformGroundAttack || !this.IsGrounded) && !this.m_abilityController.PlayerController.CharacterJump.IsJumpWithinLeeway() && base.JumpInterruptable;
		}
	}

	// Token: 0x17000A60 RID: 2656
	// (get) Token: 0x0600162C RID: 5676 RVA: 0x0000B04A File Offset: 0x0000924A
	protected override bool IsInCritWindow
	{
		get
		{
			return this.PerformGroundAttack && base.IsInCritWindow;
		}
	}

	// Token: 0x0600162D RID: 5677 RVA: 0x0000B05C File Offset: 0x0000925C
	protected override void Awake()
	{
		base.Awake();
		this.m_waitUntilLandedYield = new WaitUntil(() => this.IsGrounded);
	}

	// Token: 0x0600162E RID: 5678 RVA: 0x0000B07B File Offset: 0x0000927B
	public override void PreCastAbility()
	{
		if (this.IsGrounded)
		{
			this.m_isAirAttacking = false;
		}
		else
		{
			this.m_isAirAttacking = true;
		}
		this.PerformGroundAttack = (this.IsGrounded && !this.m_isAirAttacking);
		base.PreCastAbility();
	}

	// Token: 0x0600162F RID: 5679 RVA: 0x0000B0B5 File Offset: 0x000092B5
	protected override void OnEnterExitLogic()
	{
		if (!this.PerformGroundAttack && this.m_firedProjectile != null)
		{
			this.m_firedProjectile.FlagForDestruction(null);
		}
		if (this.PerformLandingAttack)
		{
			base.PerformAttackFlipCheck();
			this.FireProjectile();
		}
		base.OnEnterExitLogic();
	}

	// Token: 0x06001630 RID: 5680 RVA: 0x0000B0F3 File Offset: 0x000092F3
	protected override void FireProjectile()
	{
		if ((!this.PerformGroundAttack && !this.IsGrounded) || this.PerformGroundAttack || this.PerformLandingAttack)
		{
			base.FireProjectile();
		}
	}

	// Token: 0x06001631 RID: 5681 RVA: 0x0000B11B File Offset: 0x0000931B
	protected override IEnumerator ChangeAnim(float duration)
	{
		if (base.CurrentAbilityAnimState != AbilityAnimState.Exit && !this.PerformGroundAttack && this.IsGrounded)
		{
			if (base.CurrentAbilityAnimState == AbilityAnimState.Attack_Intro)
			{
				this.PerformGroundAttack = true;
				this.m_isAirAttacking = false;
				this.m_animator.Play("AxeGrounded_Attack_Intro");
			}
			yield return base.ChangeAnim(0f);
		}
		else if (base.CurrentAbilityAnimState == AbilityAnimState.Attack && !this.IsGrounded)
		{
			yield return this.m_waitUntilLandedYield;
			yield return base.ChangeAnim(0f);
		}
		else if (base.CurrentAbilityAnimState == AbilityAnimState.Tell && this.IsGrounded)
		{
			if (duration <= 0f)
			{
				yield return null;
			}
			while (duration > 0f)
			{
				duration -= Time.deltaTime;
				yield return null;
			}
			while (Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
			{
				yield return null;
			}
			if (this.IsInCritWindow)
			{
				if (this.m_critEffectGO)
				{
					BaseEffect baseEffect = EffectManager.PlayEffect(this.m_abilityController.gameObject, null, "CriticalSuccess_Effect", Vector3.zero, 1f, EffectStopType.Gracefully, EffectTriggerDirection.None);
					baseEffect.transform.SetParent(this.m_critEffectGO.transform, false);
					baseEffect.transform.localPosition = new Vector3(this.m_critEffectOffset.x, 0f, 0f);
				}
				base.ForceTriggerCrit = true;
			}
			this.m_abilityCastStartTime = 0f;
			this.m_animator.SetTrigger("Change_Ability_Anim");
			base.PerformTurnAnimCheck();
		}
		else
		{
			yield return base.ChangeAnim(duration);
		}
		yield break;
	}

	// Token: 0x06001632 RID: 5682 RVA: 0x0000B131 File Offset: 0x00009331
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_isAirAttacking = false;
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x040016AF RID: 5807
	[Header("Airborne Values")]
	[SerializeField]
	protected string m_abilityAirTellIntroName;

	// Token: 0x040016B0 RID: 5808
	[SerializeField]
	protected AbilityData m_airborneAbilityData;

	// Token: 0x040016B1 RID: 5809
	[SerializeField]
	protected string m_airborneProjectileName;

	// Token: 0x040016B2 RID: 5810
	[SerializeField]
	protected Vector2 m_airBorneProjectileOffset;

	// Token: 0x040016B3 RID: 5811
	[SerializeField]
	protected bool m_hasAirborneAttackFlipCheck;

	// Token: 0x040016B4 RID: 5812
	[Header("Attack Landing Values")]
	[SerializeField]
	protected AbilityData m_landingAbilityData;

	// Token: 0x040016B5 RID: 5813
	[SerializeField]
	protected string m_landingProjectileName;

	// Token: 0x040016B6 RID: 5814
	[SerializeField]
	protected Vector2 m_landingProjectileOffset;

	// Token: 0x040016B7 RID: 5815
	private bool m_isAirAttacking;

	// Token: 0x040016B8 RID: 5816
	private WaitUntil m_waitUntilLandedYield;

	// Token: 0x040016B9 RID: 5817
	protected float AttackMovementModAfterSpinLand;
}
