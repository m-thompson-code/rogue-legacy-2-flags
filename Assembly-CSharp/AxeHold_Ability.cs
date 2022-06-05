using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200018F RID: 399
public class AxeHold_Ability : Sword_Ability
{
	// Token: 0x06000E51 RID: 3665 RVA: 0x0002BC75 File Offset: 0x00029E75
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_airborneProjectileName,
			this.m_landingProjectileName
		};
	}

	// Token: 0x170007AA RID: 1962
	// (get) Token: 0x06000E52 RID: 3666 RVA: 0x0002BC9E File Offset: 0x00029E9E
	protected float TellIntroAnimSpeedGround
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170007AB RID: 1963
	// (get) Token: 0x06000E53 RID: 3667 RVA: 0x0002BCA5 File Offset: 0x00029EA5
	protected float TellIntroAnimSpeedAir
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170007AC RID: 1964
	// (get) Token: 0x06000E54 RID: 3668 RVA: 0x0002BCAC File Offset: 0x00029EAC
	protected float TellIntroAnimExitDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007AD RID: 1965
	// (get) Token: 0x06000E55 RID: 3669 RVA: 0x0002BCB3 File Offset: 0x00029EB3
	protected float TellIntroAnimExitDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007AE RID: 1966
	// (get) Token: 0x06000E56 RID: 3670 RVA: 0x0002BCBA File Offset: 0x00029EBA
	protected float TellHoldAnimSpeedGround
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007AF RID: 1967
	// (get) Token: 0x06000E57 RID: 3671 RVA: 0x0002BCC1 File Offset: 0x00029EC1
	protected float TellHoldAnimSpeedAir
	{
		get
		{
			return 1.15f;
		}
	}

	// Token: 0x170007B0 RID: 1968
	// (get) Token: 0x06000E58 RID: 3672 RVA: 0x0002BCC8 File Offset: 0x00029EC8
	protected float TellHoldAnimDelayGround
	{
		get
		{
			return 0.025f;
		}
	}

	// Token: 0x170007B1 RID: 1969
	// (get) Token: 0x06000E59 RID: 3673 RVA: 0x0002BCCF File Offset: 0x00029ECF
	protected float TellHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007B2 RID: 1970
	// (get) Token: 0x06000E5A RID: 3674 RVA: 0x0002BCD6 File Offset: 0x00029ED6
	protected float AttackIntroAnimSpeedGround
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170007B3 RID: 1971
	// (get) Token: 0x06000E5B RID: 3675 RVA: 0x0002BCDD File Offset: 0x00029EDD
	protected float AttackIntroAnimSpeedAir
	{
		get
		{
			return 1.15f;
		}
	}

	// Token: 0x170007B4 RID: 1972
	// (get) Token: 0x06000E5C RID: 3676 RVA: 0x0002BCE4 File Offset: 0x00029EE4
	protected float AttackIntroAnimExitDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007B5 RID: 1973
	// (get) Token: 0x06000E5D RID: 3677 RVA: 0x0002BCEB File Offset: 0x00029EEB
	protected float AttackIntroAnimExitDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007B6 RID: 1974
	// (get) Token: 0x06000E5E RID: 3678 RVA: 0x0002BCF2 File Offset: 0x00029EF2
	protected float AttackHoldAnimSpeedGround
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x170007B7 RID: 1975
	// (get) Token: 0x06000E5F RID: 3679 RVA: 0x0002BCF9 File Offset: 0x00029EF9
	protected float AttackHoldAnimSpeedAir
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x170007B8 RID: 1976
	// (get) Token: 0x06000E60 RID: 3680 RVA: 0x0002BD00 File Offset: 0x00029F00
	protected float AttackHoldAnimDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007B9 RID: 1977
	// (get) Token: 0x06000E61 RID: 3681 RVA: 0x0002BD07 File Offset: 0x00029F07
	protected float AttackHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007BA RID: 1978
	// (get) Token: 0x06000E62 RID: 3682 RVA: 0x0002BD0E File Offset: 0x00029F0E
	protected float ExitIntroAnimSpeedGround
	{
		get
		{
			return 1.6f;
		}
	}

	// Token: 0x170007BB RID: 1979
	// (get) Token: 0x06000E63 RID: 3683 RVA: 0x0002BD15 File Offset: 0x00029F15
	protected float ExitIntroAnimSpeedAir
	{
		get
		{
			return 1.4f;
		}
	}

	// Token: 0x170007BC RID: 1980
	// (get) Token: 0x06000E64 RID: 3684 RVA: 0x0002BD1C File Offset: 0x00029F1C
	protected float ExitHoldAnimDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007BD RID: 1981
	// (get) Token: 0x06000E65 RID: 3685 RVA: 0x0002BD23 File Offset: 0x00029F23
	protected float ExitHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170007BE RID: 1982
	// (get) Token: 0x06000E66 RID: 3686 RVA: 0x0002BD2A File Offset: 0x00029F2A
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

	// Token: 0x170007BF RID: 1983
	// (get) Token: 0x06000E67 RID: 3687 RVA: 0x0002BD50 File Offset: 0x00029F50
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

	// Token: 0x170007C0 RID: 1984
	// (get) Token: 0x06000E68 RID: 3688 RVA: 0x0002BD67 File Offset: 0x00029F67
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

	// Token: 0x170007C1 RID: 1985
	// (get) Token: 0x06000E69 RID: 3689 RVA: 0x0002BD7E File Offset: 0x00029F7E
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

	// Token: 0x170007C2 RID: 1986
	// (get) Token: 0x06000E6A RID: 3690 RVA: 0x0002BD95 File Offset: 0x00029F95
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

	// Token: 0x170007C3 RID: 1987
	// (get) Token: 0x06000E6B RID: 3691 RVA: 0x0002BDAC File Offset: 0x00029FAC
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

	// Token: 0x170007C4 RID: 1988
	// (get) Token: 0x06000E6C RID: 3692 RVA: 0x0002BDC3 File Offset: 0x00029FC3
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

	// Token: 0x170007C5 RID: 1989
	// (get) Token: 0x06000E6D RID: 3693 RVA: 0x0002BDDA File Offset: 0x00029FDA
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

	// Token: 0x170007C6 RID: 1990
	// (get) Token: 0x06000E6E RID: 3694 RVA: 0x0002BDF1 File Offset: 0x00029FF1
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

	// Token: 0x170007C7 RID: 1991
	// (get) Token: 0x06000E6F RID: 3695 RVA: 0x0002BE08 File Offset: 0x0002A008
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

	// Token: 0x170007C8 RID: 1992
	// (get) Token: 0x06000E70 RID: 3696 RVA: 0x0002BE1F File Offset: 0x0002A01F
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

	// Token: 0x170007C9 RID: 1993
	// (get) Token: 0x06000E71 RID: 3697 RVA: 0x0002BE36 File Offset: 0x0002A036
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

	// Token: 0x170007CA RID: 1994
	// (get) Token: 0x06000E72 RID: 3698 RVA: 0x0002BE4D File Offset: 0x0002A04D
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

	// Token: 0x170007CB RID: 1995
	// (get) Token: 0x06000E73 RID: 3699 RVA: 0x0002BE74 File Offset: 0x0002A074
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

	// Token: 0x170007CC RID: 1996
	// (get) Token: 0x06000E74 RID: 3700 RVA: 0x0002BEC3 File Offset: 0x0002A0C3
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

	// Token: 0x170007CD RID: 1997
	// (get) Token: 0x06000E75 RID: 3701 RVA: 0x0002BEDA File Offset: 0x0002A0DA
	// (set) Token: 0x06000E76 RID: 3702 RVA: 0x0002BEE2 File Offset: 0x0002A0E2
	private protected bool PerformGroundAttack { protected get; private set; }

	// Token: 0x170007CE RID: 1998
	// (get) Token: 0x06000E77 RID: 3703 RVA: 0x0002BEEB File Offset: 0x0002A0EB
	protected bool PerformLandingAttack
	{
		get
		{
			return !this.PerformGroundAttack && this.IsGrounded && this.m_isAirAttacking && base.CurrentAbilityAnimState == AbilityAnimState.Exit;
		}
	}

	// Token: 0x170007CF RID: 1999
	// (get) Token: 0x06000E78 RID: 3704 RVA: 0x0002BF10 File Offset: 0x0002A110
	protected bool IsGrounded
	{
		get
		{
			return !(this.m_abilityController != null) || (this.m_abilityController.PlayerController.IsGrounded && (!this.m_abilityController.PlayerController.IsGrounded || !this.m_abilityController.PlayerController.CharacterJump.JumpHappenedThisFrame));
		}
	}

	// Token: 0x170007D0 RID: 2000
	// (get) Token: 0x06000E79 RID: 3705 RVA: 0x0002BF69 File Offset: 0x0002A169
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

	// Token: 0x170007D1 RID: 2001
	// (get) Token: 0x06000E7A RID: 3706 RVA: 0x0002BF88 File Offset: 0x0002A188
	public override bool JumpInterruptable
	{
		get
		{
			return (this.PerformGroundAttack || !this.IsGrounded) && !this.m_abilityController.PlayerController.CharacterJump.IsJumpWithinLeeway() && base.JumpInterruptable;
		}
	}

	// Token: 0x170007D2 RID: 2002
	// (get) Token: 0x06000E7B RID: 3707 RVA: 0x0002BFB9 File Offset: 0x0002A1B9
	protected override bool IsInCritWindow
	{
		get
		{
			return this.PerformGroundAttack && base.IsInCritWindow;
		}
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x0002BFCB File Offset: 0x0002A1CB
	protected override void Awake()
	{
		base.Awake();
		this.m_waitUntilLandedYield = new WaitUntil(() => this.IsGrounded);
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x0002BFEA File Offset: 0x0002A1EA
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

	// Token: 0x06000E7E RID: 3710 RVA: 0x0002C024 File Offset: 0x0002A224
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

	// Token: 0x06000E7F RID: 3711 RVA: 0x0002C062 File Offset: 0x0002A262
	protected override void FireProjectile()
	{
		if ((!this.PerformGroundAttack && !this.IsGrounded) || this.PerformGroundAttack || this.PerformLandingAttack)
		{
			base.FireProjectile();
		}
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x0002C08A File Offset: 0x0002A28A
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

	// Token: 0x06000E81 RID: 3713 RVA: 0x0002C0A0 File Offset: 0x0002A2A0
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_isAirAttacking = false;
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x0400111E RID: 4382
	[Header("Airborne Values")]
	[SerializeField]
	protected string m_abilityAirTellIntroName;

	// Token: 0x0400111F RID: 4383
	[SerializeField]
	protected AbilityData m_airborneAbilityData;

	// Token: 0x04001120 RID: 4384
	[SerializeField]
	protected string m_airborneProjectileName;

	// Token: 0x04001121 RID: 4385
	[SerializeField]
	protected Vector2 m_airBorneProjectileOffset;

	// Token: 0x04001122 RID: 4386
	[SerializeField]
	protected bool m_hasAirborneAttackFlipCheck;

	// Token: 0x04001123 RID: 4387
	[Header("Attack Landing Values")]
	[SerializeField]
	protected AbilityData m_landingAbilityData;

	// Token: 0x04001124 RID: 4388
	[SerializeField]
	protected string m_landingProjectileName;

	// Token: 0x04001125 RID: 4389
	[SerializeField]
	protected Vector2 m_landingProjectileOffset;

	// Token: 0x04001126 RID: 4390
	private bool m_isAirAttacking;

	// Token: 0x04001127 RID: 4391
	private WaitUntil m_waitUntilLandedYield;

	// Token: 0x04001128 RID: 4392
	protected float AttackMovementModAfterSpinLand;
}
