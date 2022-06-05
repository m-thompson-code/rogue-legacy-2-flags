using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000191 RID: 401
public class Axe_Ability : Sword_Ability
{
	// Token: 0x06000E96 RID: 3734 RVA: 0x0002C283 File Offset: 0x0002A483
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_airborneProjectileName,
			this.m_landingProjectileName
		};
	}

	// Token: 0x170007DD RID: 2013
	// (get) Token: 0x06000E97 RID: 3735 RVA: 0x0002C2AC File Offset: 0x0002A4AC
	protected float TellIntroAnimSpeedGround
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170007DE RID: 2014
	// (get) Token: 0x06000E98 RID: 3736 RVA: 0x0002C2B3 File Offset: 0x0002A4B3
	protected float TellIntroAnimSpeedAir
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170007DF RID: 2015
	// (get) Token: 0x06000E99 RID: 3737 RVA: 0x0002C2BA File Offset: 0x0002A4BA
	protected float TellIntroAnimExitDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007E0 RID: 2016
	// (get) Token: 0x06000E9A RID: 3738 RVA: 0x0002C2C1 File Offset: 0x0002A4C1
	protected float TellIntroAnimExitDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007E1 RID: 2017
	// (get) Token: 0x06000E9B RID: 3739 RVA: 0x0002C2C8 File Offset: 0x0002A4C8
	protected float TellHoldAnimSpeedGround
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007E2 RID: 2018
	// (get) Token: 0x06000E9C RID: 3740 RVA: 0x0002C2CF File Offset: 0x0002A4CF
	protected float TellHoldAnimSpeedAir
	{
		get
		{
			return 1.15f;
		}
	}

	// Token: 0x170007E3 RID: 2019
	// (get) Token: 0x06000E9D RID: 3741 RVA: 0x0002C2D6 File Offset: 0x0002A4D6
	protected float TellHoldAnimDelayGround
	{
		get
		{
			return 0.025f;
		}
	}

	// Token: 0x170007E4 RID: 2020
	// (get) Token: 0x06000E9E RID: 3742 RVA: 0x0002C2DD File Offset: 0x0002A4DD
	protected float TellHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007E5 RID: 2021
	// (get) Token: 0x06000E9F RID: 3743 RVA: 0x0002C2E4 File Offset: 0x0002A4E4
	protected float AttackIntroAnimSpeedGround
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007E6 RID: 2022
	// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x0002C2EB File Offset: 0x0002A4EB
	protected float AttackIntroAnimSpeedAir
	{
		get
		{
			return 1.15f;
		}
	}

	// Token: 0x170007E7 RID: 2023
	// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x0002C2F2 File Offset: 0x0002A4F2
	protected float AttackIntroAnimExitDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007E8 RID: 2024
	// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x0002C2F9 File Offset: 0x0002A4F9
	protected float AttackIntroAnimExitDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007E9 RID: 2025
	// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x0002C300 File Offset: 0x0002A500
	protected float AttackHoldAnimSpeedGround
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x170007EA RID: 2026
	// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x0002C307 File Offset: 0x0002A507
	protected float AttackHoldAnimSpeedAir
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x170007EB RID: 2027
	// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x0002C30E File Offset: 0x0002A50E
	protected float AttackHoldAnimDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007EC RID: 2028
	// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x0002C315 File Offset: 0x0002A515
	protected float AttackHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007ED RID: 2029
	// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x0002C31C File Offset: 0x0002A51C
	protected float ExitIntroAnimSpeedGround
	{
		get
		{
			return 1.6f;
		}
	}

	// Token: 0x170007EE RID: 2030
	// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x0002C323 File Offset: 0x0002A523
	protected float ExitIntroAnimSpeedAir
	{
		get
		{
			return 1.4f;
		}
	}

	// Token: 0x170007EF RID: 2031
	// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x0002C32A File Offset: 0x0002A52A
	protected float ExitHoldAnimDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007F0 RID: 2032
	// (get) Token: 0x06000EAA RID: 3754 RVA: 0x0002C331 File Offset: 0x0002A531
	protected float ExitHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007F1 RID: 2033
	// (get) Token: 0x06000EAB RID: 3755 RVA: 0x0002C338 File Offset: 0x0002A538
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

	// Token: 0x170007F2 RID: 2034
	// (get) Token: 0x06000EAC RID: 3756 RVA: 0x0002C35E File Offset: 0x0002A55E
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

	// Token: 0x170007F3 RID: 2035
	// (get) Token: 0x06000EAD RID: 3757 RVA: 0x0002C375 File Offset: 0x0002A575
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

	// Token: 0x170007F4 RID: 2036
	// (get) Token: 0x06000EAE RID: 3758 RVA: 0x0002C38C File Offset: 0x0002A58C
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

	// Token: 0x170007F5 RID: 2037
	// (get) Token: 0x06000EAF RID: 3759 RVA: 0x0002C3A3 File Offset: 0x0002A5A3
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

	// Token: 0x170007F6 RID: 2038
	// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x0002C3BA File Offset: 0x0002A5BA
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

	// Token: 0x170007F7 RID: 2039
	// (get) Token: 0x06000EB1 RID: 3761 RVA: 0x0002C3D1 File Offset: 0x0002A5D1
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

	// Token: 0x170007F8 RID: 2040
	// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x0002C3E8 File Offset: 0x0002A5E8
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

	// Token: 0x170007F9 RID: 2041
	// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x0002C3FF File Offset: 0x0002A5FF
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

	// Token: 0x170007FA RID: 2042
	// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x0002C416 File Offset: 0x0002A616
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

	// Token: 0x170007FB RID: 2043
	// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x0002C42D File Offset: 0x0002A62D
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

	// Token: 0x170007FC RID: 2044
	// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x0002C444 File Offset: 0x0002A644
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

	// Token: 0x170007FD RID: 2045
	// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x0002C45B File Offset: 0x0002A65B
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

	// Token: 0x170007FE RID: 2046
	// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x0002C484 File Offset: 0x0002A684
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

	// Token: 0x170007FF RID: 2047
	// (get) Token: 0x06000EB9 RID: 3769 RVA: 0x0002C4D3 File Offset: 0x0002A6D3
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

	// Token: 0x17000800 RID: 2048
	// (get) Token: 0x06000EBA RID: 3770 RVA: 0x0002C4EA File Offset: 0x0002A6EA
	// (set) Token: 0x06000EBB RID: 3771 RVA: 0x0002C4F2 File Offset: 0x0002A6F2
	private protected bool PerformGroundAttack { protected get; private set; }

	// Token: 0x17000801 RID: 2049
	// (get) Token: 0x06000EBC RID: 3772 RVA: 0x0002C4FB File Offset: 0x0002A6FB
	protected bool PerformLandingAttack
	{
		get
		{
			return !this.PerformGroundAttack && this.IsGrounded && this.m_isAirAttacking && base.CurrentAbilityAnimState == AbilityAnimState.Exit;
		}
	}

	// Token: 0x17000802 RID: 2050
	// (get) Token: 0x06000EBD RID: 3773 RVA: 0x0002C520 File Offset: 0x0002A720
	protected bool IsGrounded
	{
		get
		{
			return !this.m_abilityController || (!this.m_abilityController.PlayerController.CharacterDash.IsDashing && this.m_abilityController.PlayerController.IsGrounded && (!this.m_abilityController.PlayerController.IsGrounded || !this.m_abilityController.PlayerController.CharacterJump.JumpHappenedThisFrame));
		}
	}

	// Token: 0x17000803 RID: 2051
	// (get) Token: 0x06000EBE RID: 3774 RVA: 0x0002C58F File Offset: 0x0002A78F
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

	// Token: 0x17000804 RID: 2052
	// (get) Token: 0x06000EBF RID: 3775 RVA: 0x0002C5AE File Offset: 0x0002A7AE
	public override bool JumpInterruptable
	{
		get
		{
			return (this.PerformGroundAttack || !this.IsGrounded) && !this.m_abilityController.PlayerController.CharacterJump.IsJumpWithinLeeway() && base.JumpInterruptable;
		}
	}

	// Token: 0x17000805 RID: 2053
	// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x0002C5DF File Offset: 0x0002A7DF
	protected override bool IsInCritWindow
	{
		get
		{
			return this.PerformGroundAttack && base.IsInCritWindow;
		}
	}

	// Token: 0x06000EC1 RID: 3777 RVA: 0x0002C5F1 File Offset: 0x0002A7F1
	protected override void Awake()
	{
		base.Awake();
		this.m_waitUntilLandedYield = new WaitUntil(() => this.IsGrounded);
	}

	// Token: 0x06000EC2 RID: 3778 RVA: 0x0002C610 File Offset: 0x0002A810
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

	// Token: 0x06000EC3 RID: 3779 RVA: 0x0002C64A File Offset: 0x0002A84A
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

	// Token: 0x06000EC4 RID: 3780 RVA: 0x0002C687 File Offset: 0x0002A887
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

	// Token: 0x06000EC5 RID: 3781 RVA: 0x0002C6BE File Offset: 0x0002A8BE
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

	// Token: 0x06000EC6 RID: 3782 RVA: 0x0002C6D4 File Offset: 0x0002A8D4
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_isAirAttacking = false;
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x0400112C RID: 4396
	[Header("Airborne Values")]
	[SerializeField]
	protected string m_abilityAirTellIntroName;

	// Token: 0x0400112D RID: 4397
	[SerializeField]
	protected AbilityData m_airborneAbilityData;

	// Token: 0x0400112E RID: 4398
	[SerializeField]
	protected string m_airborneProjectileName;

	// Token: 0x0400112F RID: 4399
	[SerializeField]
	protected Vector2 m_airBorneProjectileOffset;

	// Token: 0x04001130 RID: 4400
	[SerializeField]
	protected bool m_hasAirborneAttackFlipCheck;

	// Token: 0x04001131 RID: 4401
	[Header("Attack Landing Values")]
	[SerializeField]
	protected AbilityData m_landingAbilityData;

	// Token: 0x04001132 RID: 4402
	[SerializeField]
	protected string m_landingProjectileName;

	// Token: 0x04001133 RID: 4403
	[SerializeField]
	protected Vector2 m_landingProjectileOffset;

	// Token: 0x04001134 RID: 4404
	private bool m_isAirAttacking;

	// Token: 0x04001135 RID: 4405
	private WaitUntil m_waitUntilLandedYield;

	// Token: 0x04001136 RID: 4406
	protected float AttackMovementModAfterSpinLand;
}
