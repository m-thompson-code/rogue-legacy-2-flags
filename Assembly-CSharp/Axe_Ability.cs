using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002E4 RID: 740
public class Axe_Ability : Sword_Ability
{
	// Token: 0x06001653 RID: 5715 RVA: 0x0000B1B6 File Offset: 0x000093B6
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_airborneProjectileName,
			this.m_landingProjectileName
		};
	}

	// Token: 0x17000A6F RID: 2671
	// (get) Token: 0x06001654 RID: 5716 RVA: 0x00003C70 File Offset: 0x00001E70
	protected float TellIntroAnimSpeedGround
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000A70 RID: 2672
	// (get) Token: 0x06001655 RID: 5717 RVA: 0x00003C70 File Offset: 0x00001E70
	protected float TellIntroAnimSpeedAir
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000A71 RID: 2673
	// (get) Token: 0x06001656 RID: 5718 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellIntroAnimExitDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A72 RID: 2674
	// (get) Token: 0x06001657 RID: 5719 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellIntroAnimExitDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A73 RID: 2675
	// (get) Token: 0x06001658 RID: 5720 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected float TellHoldAnimSpeedGround
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000A74 RID: 2676
	// (get) Token: 0x06001659 RID: 5721 RVA: 0x00004FDE File Offset: 0x000031DE
	protected float TellHoldAnimSpeedAir
	{
		get
		{
			return 1.15f;
		}
	}

	// Token: 0x17000A75 RID: 2677
	// (get) Token: 0x0600165A RID: 5722 RVA: 0x0000AE5D File Offset: 0x0000905D
	protected float TellHoldAnimDelayGround
	{
		get
		{
			return 0.025f;
		}
	}

	// Token: 0x17000A76 RID: 2678
	// (get) Token: 0x0600165B RID: 5723 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A77 RID: 2679
	// (get) Token: 0x0600165C RID: 5724 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected float AttackIntroAnimSpeedGround
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000A78 RID: 2680
	// (get) Token: 0x0600165D RID: 5725 RVA: 0x00004FDE File Offset: 0x000031DE
	protected float AttackIntroAnimSpeedAir
	{
		get
		{
			return 1.15f;
		}
	}

	// Token: 0x17000A79 RID: 2681
	// (get) Token: 0x0600165E RID: 5726 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackIntroAnimExitDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A7A RID: 2682
	// (get) Token: 0x0600165F RID: 5727 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackIntroAnimExitDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A7B RID: 2683
	// (get) Token: 0x06001660 RID: 5728 RVA: 0x00005FAA File Offset: 0x000041AA
	protected float AttackHoldAnimSpeedGround
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x17000A7C RID: 2684
	// (get) Token: 0x06001661 RID: 5729 RVA: 0x00005FAA File Offset: 0x000041AA
	protected float AttackHoldAnimSpeedAir
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x17000A7D RID: 2685
	// (get) Token: 0x06001662 RID: 5730 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackHoldAnimDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A7E RID: 2686
	// (get) Token: 0x06001663 RID: 5731 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A7F RID: 2687
	// (get) Token: 0x06001664 RID: 5732 RVA: 0x00005307 File Offset: 0x00003507
	protected float ExitIntroAnimSpeedGround
	{
		get
		{
			return 1.6f;
		}
	}

	// Token: 0x17000A80 RID: 2688
	// (get) Token: 0x06001665 RID: 5733 RVA: 0x00003DE8 File Offset: 0x00001FE8
	protected float ExitIntroAnimSpeedAir
	{
		get
		{
			return 1.4f;
		}
	}

	// Token: 0x17000A81 RID: 2689
	// (get) Token: 0x06001666 RID: 5734 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float ExitHoldAnimDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A82 RID: 2690
	// (get) Token: 0x06001667 RID: 5735 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float ExitHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000A83 RID: 2691
	// (get) Token: 0x06001668 RID: 5736 RVA: 0x0000B1DF File Offset: 0x000093DF
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

	// Token: 0x17000A84 RID: 2692
	// (get) Token: 0x06001669 RID: 5737 RVA: 0x0000B205 File Offset: 0x00009405
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

	// Token: 0x17000A85 RID: 2693
	// (get) Token: 0x0600166A RID: 5738 RVA: 0x0000B21C File Offset: 0x0000941C
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

	// Token: 0x17000A86 RID: 2694
	// (get) Token: 0x0600166B RID: 5739 RVA: 0x0000B233 File Offset: 0x00009433
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

	// Token: 0x17000A87 RID: 2695
	// (get) Token: 0x0600166C RID: 5740 RVA: 0x0000B24A File Offset: 0x0000944A
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

	// Token: 0x17000A88 RID: 2696
	// (get) Token: 0x0600166D RID: 5741 RVA: 0x0000B261 File Offset: 0x00009461
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

	// Token: 0x17000A89 RID: 2697
	// (get) Token: 0x0600166E RID: 5742 RVA: 0x0000B278 File Offset: 0x00009478
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

	// Token: 0x17000A8A RID: 2698
	// (get) Token: 0x0600166F RID: 5743 RVA: 0x0000B28F File Offset: 0x0000948F
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

	// Token: 0x17000A8B RID: 2699
	// (get) Token: 0x06001670 RID: 5744 RVA: 0x0000B2A6 File Offset: 0x000094A6
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

	// Token: 0x17000A8C RID: 2700
	// (get) Token: 0x06001671 RID: 5745 RVA: 0x0000B2BD File Offset: 0x000094BD
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

	// Token: 0x17000A8D RID: 2701
	// (get) Token: 0x06001672 RID: 5746 RVA: 0x0000B2D4 File Offset: 0x000094D4
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

	// Token: 0x17000A8E RID: 2702
	// (get) Token: 0x06001673 RID: 5747 RVA: 0x0000B2EB File Offset: 0x000094EB
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

	// Token: 0x17000A8F RID: 2703
	// (get) Token: 0x06001674 RID: 5748 RVA: 0x0000B302 File Offset: 0x00009502
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

	// Token: 0x17000A90 RID: 2704
	// (get) Token: 0x06001675 RID: 5749 RVA: 0x0008B3BC File Offset: 0x000895BC
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

	// Token: 0x17000A91 RID: 2705
	// (get) Token: 0x06001676 RID: 5750 RVA: 0x0000B328 File Offset: 0x00009528
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

	// Token: 0x17000A92 RID: 2706
	// (get) Token: 0x06001677 RID: 5751 RVA: 0x0000B33F File Offset: 0x0000953F
	// (set) Token: 0x06001678 RID: 5752 RVA: 0x0000B347 File Offset: 0x00009547
	private protected bool PerformGroundAttack { protected get; private set; }

	// Token: 0x17000A93 RID: 2707
	// (get) Token: 0x06001679 RID: 5753 RVA: 0x0000B350 File Offset: 0x00009550
	protected bool PerformLandingAttack
	{
		get
		{
			return !this.PerformGroundAttack && this.IsGrounded && this.m_isAirAttacking && base.CurrentAbilityAnimState == AbilityAnimState.Exit;
		}
	}

	// Token: 0x17000A94 RID: 2708
	// (get) Token: 0x0600167A RID: 5754 RVA: 0x0008B40C File Offset: 0x0008960C
	protected bool IsGrounded
	{
		get
		{
			return !this.m_abilityController || (!this.m_abilityController.PlayerController.CharacterDash.IsDashing && this.m_abilityController.PlayerController.IsGrounded && (!this.m_abilityController.PlayerController.IsGrounded || !this.m_abilityController.PlayerController.CharacterJump.JumpHappenedThisFrame));
		}
	}

	// Token: 0x17000A95 RID: 2709
	// (get) Token: 0x0600167B RID: 5755 RVA: 0x0000B375 File Offset: 0x00009575
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

	// Token: 0x17000A96 RID: 2710
	// (get) Token: 0x0600167C RID: 5756 RVA: 0x0000B394 File Offset: 0x00009594
	public override bool JumpInterruptable
	{
		get
		{
			return (this.PerformGroundAttack || !this.IsGrounded) && !this.m_abilityController.PlayerController.CharacterJump.IsJumpWithinLeeway() && base.JumpInterruptable;
		}
	}

	// Token: 0x17000A97 RID: 2711
	// (get) Token: 0x0600167D RID: 5757 RVA: 0x0000B3C5 File Offset: 0x000095C5
	protected override bool IsInCritWindow
	{
		get
		{
			return this.PerformGroundAttack && base.IsInCritWindow;
		}
	}

	// Token: 0x0600167E RID: 5758 RVA: 0x0000B3D7 File Offset: 0x000095D7
	protected override void Awake()
	{
		base.Awake();
		this.m_waitUntilLandedYield = new WaitUntil(() => this.IsGrounded);
	}

	// Token: 0x0600167F RID: 5759 RVA: 0x0000B3F6 File Offset: 0x000095F6
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

	// Token: 0x06001680 RID: 5760 RVA: 0x0000B430 File Offset: 0x00009630
	protected override void OnEnterExitLogic()
	{
		if (!this.PerformGroundAttack && this.m_firedProjectile)
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

	// Token: 0x06001681 RID: 5761 RVA: 0x0000B46D File Offset: 0x0000966D
	protected override void FireProjectile()
	{
		if ((!this.PerformGroundAttack && !this.IsGrounded) || this.PerformGroundAttack || this.PerformLandingAttack)
		{
			if (this.PerformGroundAttack)
			{
				base.ForceTriggerCrit = true;
			}
			base.FireProjectile();
		}
	}

	// Token: 0x06001682 RID: 5762 RVA: 0x0000B4A4 File Offset: 0x000096A4
	protected override IEnumerator ChangeAnim(float duration)
	{
		if (base.CurrentAbilityAnimState != AbilityAnimState.Exit && !this.PerformGroundAttack && this.IsGrounded)
		{
			if (base.CurrentAbilityAnimState == AbilityAnimState.Attack_Intro)
			{
				this.m_animator.Play("AxeAirborne_Attack_Hold");
			}
			yield return base.ChangeAnim(0f);
		}
		else if (base.CurrentAbilityAnimState == AbilityAnimState.Attack && !this.IsGrounded)
		{
			yield return this.m_waitUntilLandedYield;
			yield return base.ChangeAnim(0f);
		}
		else
		{
			yield return base.ChangeAnim(duration);
		}
		yield break;
	}

	// Token: 0x06001683 RID: 5763 RVA: 0x0000B4BA File Offset: 0x000096BA
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_isAirAttacking = false;
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x040016C5 RID: 5829
	[Header("Airborne Values")]
	[SerializeField]
	protected string m_abilityAirTellIntroName;

	// Token: 0x040016C6 RID: 5830
	[SerializeField]
	protected AbilityData m_airborneAbilityData;

	// Token: 0x040016C7 RID: 5831
	[SerializeField]
	protected string m_airborneProjectileName;

	// Token: 0x040016C8 RID: 5832
	[SerializeField]
	protected Vector2 m_airBorneProjectileOffset;

	// Token: 0x040016C9 RID: 5833
	[SerializeField]
	protected bool m_hasAirborneAttackFlipCheck;

	// Token: 0x040016CA RID: 5834
	[Header("Attack Landing Values")]
	[SerializeField]
	protected AbilityData m_landingAbilityData;

	// Token: 0x040016CB RID: 5835
	[SerializeField]
	protected string m_landingProjectileName;

	// Token: 0x040016CC RID: 5836
	[SerializeField]
	protected Vector2 m_landingProjectileOffset;

	// Token: 0x040016CD RID: 5837
	private bool m_isAirAttacking;

	// Token: 0x040016CE RID: 5838
	private WaitUntil m_waitUntilLandedYield;

	// Token: 0x040016CF RID: 5839
	protected float AttackMovementModAfterSpinLand;
}
