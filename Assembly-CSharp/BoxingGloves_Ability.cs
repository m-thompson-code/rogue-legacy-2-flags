using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class BoxingGloves_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x06000EDF RID: 3807 RVA: 0x0002C843 File Offset: 0x0002AA43
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_projectileName2,
			this.m_projectileName3,
			this.m_downAttackProjectileName,
			this.m_upAttackProjectileName
		};
	}

	// Token: 0x17000813 RID: 2067
	// (get) Token: 0x06000EE0 RID: 3808 RVA: 0x0002C87E File Offset: 0x0002AA7E
	protected virtual int NumComboAttacks
	{
		get
		{
			return 999999;
		}
	}

	// Token: 0x17000814 RID: 2068
	// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x0002C885 File Offset: 0x0002AA85
	protected float TellIntroAnimSpeedNormalAttack
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000815 RID: 2069
	// (get) Token: 0x06000EE2 RID: 3810 RVA: 0x0002C88C File Offset: 0x0002AA8C
	protected float TellIntroAnimSpeedDownAttack
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x17000816 RID: 2070
	// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x0002C893 File Offset: 0x0002AA93
	protected float TellIntroAnimSpeedUpAttack
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x17000817 RID: 2071
	// (get) Token: 0x06000EE4 RID: 3812 RVA: 0x0002C89A File Offset: 0x0002AA9A
	protected float TellIntroAnimExitDelayNormalAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000818 RID: 2072
	// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x0002C8A1 File Offset: 0x0002AAA1
	protected float TellIntroAnimExitDelayDownAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000819 RID: 2073
	// (get) Token: 0x06000EE6 RID: 3814 RVA: 0x0002C8A8 File Offset: 0x0002AAA8
	protected float TellIntroAnimExitDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700081A RID: 2074
	// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x0002C8AF File Offset: 0x0002AAAF
	protected float TellAnimSpeedNormalAttack
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700081B RID: 2075
	// (get) Token: 0x06000EE8 RID: 3816 RVA: 0x0002C8B6 File Offset: 0x0002AAB6
	protected float TellAnimSpeedDownAttack
	{
		get
		{
			return this.TellAnimSpeedNormalAttack;
		}
	}

	// Token: 0x1700081C RID: 2076
	// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x0002C8BE File Offset: 0x0002AABE
	protected float TellAnimSpeedUpAttack
	{
		get
		{
			return this.TellAnimSpeedNormalAttack;
		}
	}

	// Token: 0x1700081D RID: 2077
	// (get) Token: 0x06000EEA RID: 3818 RVA: 0x0002C8C6 File Offset: 0x0002AAC6
	protected float TellAnimExitDelayNormalAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700081E RID: 2078
	// (get) Token: 0x06000EEB RID: 3819 RVA: 0x0002C8CD File Offset: 0x0002AACD
	protected float TellAnimExitDelayDownAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700081F RID: 2079
	// (get) Token: 0x06000EEC RID: 3820 RVA: 0x0002C8D4 File Offset: 0x0002AAD4
	protected float TellAnimExitDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000820 RID: 2080
	// (get) Token: 0x06000EED RID: 3821 RVA: 0x0002C8DB File Offset: 0x0002AADB
	protected float AttackIntroAnimSpeedNormalAttack
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000821 RID: 2081
	// (get) Token: 0x06000EEE RID: 3822 RVA: 0x0002C8E2 File Offset: 0x0002AAE2
	protected float AttackIntroAnimSpeedDownAttack
	{
		get
		{
			return this.AttackIntroAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000822 RID: 2082
	// (get) Token: 0x06000EEF RID: 3823 RVA: 0x0002C8EA File Offset: 0x0002AAEA
	protected float AttackIntroAnimSpeedUpAttack
	{
		get
		{
			return this.AttackIntroAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000823 RID: 2083
	// (get) Token: 0x06000EF0 RID: 3824 RVA: 0x0002C8F2 File Offset: 0x0002AAF2
	protected float AttackIntroAnimExitDelayNormalAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000824 RID: 2084
	// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x0002C8F9 File Offset: 0x0002AAF9
	protected float AttackIntroAnimExitDelayDownAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000825 RID: 2085
	// (get) Token: 0x06000EF2 RID: 3826 RVA: 0x0002C900 File Offset: 0x0002AB00
	protected float AttackIntroAnimExitDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000826 RID: 2086
	// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x0002C907 File Offset: 0x0002AB07
	protected float AttackAnimSpeedNormalAttack
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000827 RID: 2087
	// (get) Token: 0x06000EF4 RID: 3828 RVA: 0x0002C90E File Offset: 0x0002AB0E
	protected float AttackAnimSpeedDownAttack
	{
		get
		{
			return this.AttackAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000828 RID: 2088
	// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x0002C916 File Offset: 0x0002AB16
	protected float AttackAnimSpeedUpAttack
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000829 RID: 2089
	// (get) Token: 0x06000EF6 RID: 3830 RVA: 0x0002C91D File Offset: 0x0002AB1D
	protected float AttackAnimExitDelayNormalAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700082A RID: 2090
	// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x0002C924 File Offset: 0x0002AB24
	protected float AttackAnimExitDelayDownAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700082B RID: 2091
	// (get) Token: 0x06000EF8 RID: 3832 RVA: 0x0002C92B File Offset: 0x0002AB2B
	protected float AttackAnimExitDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700082C RID: 2092
	// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x0002C932 File Offset: 0x0002AB32
	protected float ExitAnimSpeedNormalAttack
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700082D RID: 2093
	// (get) Token: 0x06000EFA RID: 3834 RVA: 0x0002C939 File Offset: 0x0002AB39
	protected float ExitAnimSpeedDownAttack
	{
		get
		{
			return this.ExitAnimSpeedNormalAttack;
		}
	}

	// Token: 0x1700082E RID: 2094
	// (get) Token: 0x06000EFB RID: 3835 RVA: 0x0002C941 File Offset: 0x0002AB41
	protected float ExitAnimSpeedUpAttack
	{
		get
		{
			return 1.2f;
		}
	}

	// Token: 0x1700082F RID: 2095
	// (get) Token: 0x06000EFC RID: 3836 RVA: 0x0002C948 File Offset: 0x0002AB48
	protected float ExitAnimExitDelayNormalAttack
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x17000830 RID: 2096
	// (get) Token: 0x06000EFD RID: 3837 RVA: 0x0002C94F File Offset: 0x0002AB4F
	protected float ExitAnimExitDelayDownAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000831 RID: 2097
	// (get) Token: 0x06000EFE RID: 3838 RVA: 0x0002C956 File Offset: 0x0002AB56
	protected float ExitAnimExitDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000832 RID: 2098
	// (get) Token: 0x06000EFF RID: 3839 RVA: 0x0002C95D File Offset: 0x0002AB5D
	public override Vector2 ProjectileOffset
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.m_upAttackProjectileOffset;
			}
			if (this.PerformDownAttack)
			{
				return this.m_downAttackProjectileOffset;
			}
			return base.ProjectileOffset;
		}
	}

	// Token: 0x17000833 RID: 2099
	// (get) Token: 0x06000F00 RID: 3840 RVA: 0x0002C983 File Offset: 0x0002AB83
	protected override float TellIntroAnimSpeed
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.TellIntroAnimSpeedUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.TellIntroAnimSpeedDownAttack;
			}
			return this.TellIntroAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000834 RID: 2100
	// (get) Token: 0x06000F01 RID: 3841 RVA: 0x0002C9A9 File Offset: 0x0002ABA9
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.TellIntroAnimExitDelayUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.TellIntroAnimExitDelayDownAttack;
			}
			return this.TellIntroAnimExitDelayNormalAttack;
		}
	}

	// Token: 0x17000835 RID: 2101
	// (get) Token: 0x06000F02 RID: 3842 RVA: 0x0002C9CF File Offset: 0x0002ABCF
	protected override float TellAnimSpeed
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.TellAnimSpeedUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.TellAnimSpeedDownAttack;
			}
			return this.TellAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000836 RID: 2102
	// (get) Token: 0x06000F03 RID: 3843 RVA: 0x0002C9F5 File Offset: 0x0002ABF5
	protected override float TellAnimExitDelay
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.TellAnimExitDelayUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.TellAnimExitDelayDownAttack;
			}
			return this.TellAnimExitDelayNormalAttack;
		}
	}

	// Token: 0x17000837 RID: 2103
	// (get) Token: 0x06000F04 RID: 3844 RVA: 0x0002CA1B File Offset: 0x0002AC1B
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.AttackIntroAnimSpeedUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.AttackIntroAnimSpeedDownAttack;
			}
			return this.AttackIntroAnimSpeedNormalAttack;
		}
	}

	// Token: 0x17000838 RID: 2104
	// (get) Token: 0x06000F05 RID: 3845 RVA: 0x0002CA41 File Offset: 0x0002AC41
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.AttackIntroAnimExitDelayUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.AttackIntroAnimExitDelayDownAttack;
			}
			return this.AttackIntroAnimExitDelayNormalAttack;
		}
	}

	// Token: 0x17000839 RID: 2105
	// (get) Token: 0x06000F06 RID: 3846 RVA: 0x0002CA67 File Offset: 0x0002AC67
	protected override float AttackAnimSpeed
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.AttackAnimSpeedUpAttack;
			}
			if (this.IsGrounded)
			{
				return this.AttackAnimSpeedDownAttack;
			}
			return this.AttackAnimSpeedNormalAttack;
		}
	}

	// Token: 0x1700083A RID: 2106
	// (get) Token: 0x06000F07 RID: 3847 RVA: 0x0002CA8D File Offset: 0x0002AC8D
	protected override float AttackAnimExitDelay
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.AttackAnimExitDelayUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.AttackAnimExitDelayDownAttack;
			}
			return this.AttackAnimExitDelayNormalAttack;
		}
	}

	// Token: 0x1700083B RID: 2107
	// (get) Token: 0x06000F08 RID: 3848 RVA: 0x0002CAB3 File Offset: 0x0002ACB3
	protected override float ExitAnimSpeed
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.ExitAnimSpeedUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.ExitAnimSpeedDownAttack;
			}
			return this.ExitAnimSpeedNormalAttack;
		}
	}

	// Token: 0x1700083C RID: 2108
	// (get) Token: 0x06000F09 RID: 3849 RVA: 0x0002CAD9 File Offset: 0x0002ACD9
	protected override float ExitAnimExitDelay
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.ExitAnimExitDelayUpAttack;
			}
			if (this.PerformDownAttack)
			{
				return this.ExitAnimExitDelayDownAttack;
			}
			return this.ExitAnimExitDelayNormalAttack;
		}
	}

	// Token: 0x1700083D RID: 2109
	// (get) Token: 0x06000F0A RID: 3850 RVA: 0x0002CB00 File Offset: 0x0002AD00
	public override string ProjectileName
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.m_upAttackProjectileName;
			}
			if (this.PerformDownAttack)
			{
				return this.m_downAttackProjectileName;
			}
			switch (this.m_punchIndex)
			{
			case 1:
				return base.ProjectileName;
			case 2:
				return this.m_projectileName2;
			case 3:
				return this.m_projectileName3;
			default:
				return base.ProjectileName;
			}
		}
	}

	// Token: 0x1700083E RID: 2110
	// (get) Token: 0x06000F0B RID: 3851 RVA: 0x0002CB63 File Offset: 0x0002AD63
	public override AbilityData AbilityData
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.m_upAttackAbilityData;
			}
			if (this.PerformDownAttack)
			{
				return this.m_downAttackAbilityData;
			}
			return base.AbilityData;
		}
	}

	// Token: 0x1700083F RID: 2111
	// (get) Token: 0x06000F0C RID: 3852 RVA: 0x0002CB89 File Offset: 0x0002AD89
	public override string AbilityTellIntroName
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.m_upAttackTellIntroName;
			}
			if (this.PerformDownAttack)
			{
				return this.m_downAttackTellIntroName;
			}
			return base.AbilityTellIntroName.Replace('1', this.m_punchIndex.ToString()[0]);
		}
	}

	// Token: 0x17000840 RID: 2112
	// (get) Token: 0x06000F0D RID: 3853 RVA: 0x0002CBC7 File Offset: 0x0002ADC7
	protected bool PerformUpAttack
	{
		get
		{
			return this.m_isUpAttacking;
		}
	}

	// Token: 0x17000841 RID: 2113
	// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0002CBCF File Offset: 0x0002ADCF
	protected bool PerformDownAttack
	{
		get
		{
			return this.m_isDownAttacking;
		}
	}

	// Token: 0x17000842 RID: 2114
	// (get) Token: 0x06000F0F RID: 3855 RVA: 0x0002CBD7 File Offset: 0x0002ADD7
	protected bool IsGrounded
	{
		get
		{
			return !this.m_abilityController || this.m_abilityController.PlayerController.IsGrounded;
		}
	}

	// Token: 0x17000843 RID: 2115
	// (get) Token: 0x06000F10 RID: 3856 RVA: 0x0002CBFB File Offset: 0x0002ADFB
	public override float MovementMod
	{
		get
		{
			if (!this.m_abilityController.PlayerController.IsGrounded)
			{
				return 1f;
			}
			return base.MovementMod;
		}
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x0002CC1B File Offset: 0x0002AE1B
	protected override void Awake()
	{
		base.Awake();
		this.m_continueComboEvent = new Action<Projectile_RL, GameObject>(this.ContinueComboEvent);
	}

	// Token: 0x06000F12 RID: 3858 RVA: 0x0002CC38 File Offset: 0x0002AE38
	public override void PreCastAbility()
	{
		this.m_isUpAttacking = false;
		this.m_isDownAttacking = false;
		if (Rewired_RL.Player.GetButton("MoveVertical"))
		{
			this.m_isUpAttacking = true;
		}
		this.m_punchIndex = 1;
		this.m_canAttackAgainCounter = 0f;
		this.m_comboNumber = 0;
		base.PreCastAbility();
	}

	// Token: 0x06000F13 RID: 3859 RVA: 0x0002CC8A File Offset: 0x0002AE8A
	protected override void OnEnterTellLogic()
	{
		this.m_hasAttacked = false;
		this.m_comboNumber++;
		base.OnEnterTellLogic();
	}

	// Token: 0x06000F14 RID: 3860 RVA: 0x0002CCA7 File Offset: 0x0002AEA7
	protected override void OnEnterAttackLogic()
	{
		base.OnEnterAttackLogic();
		this.m_canAttackAgainCounter = 0f;
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x0002CCBA File Offset: 0x0002AEBA
	protected override void FireProjectile()
	{
		base.FireProjectile();
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.OnCollisionRelay.AddListener(this.m_continueComboEvent, false);
		}
	}

	// Token: 0x06000F16 RID: 3862 RVA: 0x0002CCE8 File Offset: 0x0002AEE8
	private void ContinueComboEvent(Projectile_RL projectile, GameObject colliderObj)
	{
		if (projectile)
		{
			projectile.OnCollisionRelay.RemoveListener(this.m_continueComboEvent);
		}
		if (!this)
		{
			return;
		}
		if (this.m_abilityController.PlayerController.ConditionState == CharacterStates.CharacterConditions.Stunned)
		{
			return;
		}
		if (this.PerformDownAttack || this.PerformUpAttack)
		{
			return;
		}
		if (this.m_abilityController.PlayerController.CharacterDash.IsDashing)
		{
			this.m_abilityController.PlayerController.CharacterDash.StopDash();
		}
		this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
		this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
		this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(false);
		if (this.m_reenableGravityCoroutine != null)
		{
			base.StopCoroutine(this.m_reenableGravityCoroutine);
		}
		this.m_reenableGravityCoroutine = base.StartCoroutine(this.ReenableGravityCoroutine());
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x0002CDE6 File Offset: 0x0002AFE6
	private IEnumerator ReenableGravityCoroutine()
	{
		float delay = Time.time + 0.25f;
		while (Time.time < delay)
		{
			yield return null;
		}
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(true);
		yield break;
	}

	// Token: 0x06000F18 RID: 3864 RVA: 0x0002CDF8 File Offset: 0x0002AFF8
	protected void LateUpdate()
	{
		if (!base.AbilityActive)
		{
			return;
		}
		if (base.CurrentAbilityAnimState >= AbilityAnimState.Attack && !this.m_isUpAttacking)
		{
			if (this.m_canAttackAgainCounter >= 0.085f)
			{
				if (this.m_comboNumber < this.NumComboAttacks && Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
				{
					this.m_hasAttacked = true;
					base.CancelChangeAnimCoroutine();
					this.m_isUpAttacking = false;
					this.m_isDownAttacking = false;
					base.IsAnimationComplete = false;
					this.m_punchIndex++;
					if (this.m_punchIndex > 3)
					{
						this.m_punchIndex = 1;
					}
					if (Rewired_RL.Player.GetButton("MoveVertical"))
					{
						this.m_isUpAttacking = true;
						if (base.AbilityActive)
						{
							this.m_animator.Play(this.AbilityTellIntroName);
							return;
						}
						this.m_abilityController.StartAbility(base.CastAbilityType, false, false);
						return;
					}
					else
					{
						if (base.AbilityActive)
						{
							this.m_animator.Play(this.AbilityTellIntroName);
							return;
						}
						this.m_abilityController.StartAbility(base.CastAbilityType, false, false);
						return;
					}
				}
			}
			else
			{
				this.m_canAttackAgainCounter += Time.deltaTime;
			}
		}
	}

	// Token: 0x06000F19 RID: 3865 RVA: 0x0002CF2C File Offset: 0x0002B12C
	protected override void OnExitExitLogic()
	{
		if (this.m_hasAttacked)
		{
			return;
		}
		base.OnExitExitLogic();
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x0002CF40 File Offset: 0x0002B140
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.OnCollisionRelay.RemoveListener(this.m_continueComboEvent);
		}
		if (this.m_reenableGravityCoroutine != null)
		{
			base.StopCoroutine(this.m_reenableGravityCoroutine);
		}
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(true);
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x04001138 RID: 4408
	[SerializeField]
	protected string m_projectileName2;

	// Token: 0x04001139 RID: 4409
	[SerializeField]
	protected string m_projectileName3;

	// Token: 0x0400113A RID: 4410
	[Header("Downward Attack Values")]
	[SerializeField]
	protected string m_downAttackTellIntroName;

	// Token: 0x0400113B RID: 4411
	[SerializeField]
	protected AbilityData m_downAttackAbilityData;

	// Token: 0x0400113C RID: 4412
	[SerializeField]
	protected string m_downAttackProjectileName;

	// Token: 0x0400113D RID: 4413
	[SerializeField]
	protected Vector2 m_downAttackProjectileOffset;

	// Token: 0x0400113E RID: 4414
	[Header("Upward Attack Values")]
	[SerializeField]
	protected string m_upAttackTellIntroName;

	// Token: 0x0400113F RID: 4415
	[SerializeField]
	protected AbilityData m_upAttackAbilityData;

	// Token: 0x04001140 RID: 4416
	[SerializeField]
	protected string m_upAttackProjectileName;

	// Token: 0x04001141 RID: 4417
	[SerializeField]
	protected Vector2 m_upAttackProjectileOffset;

	// Token: 0x04001142 RID: 4418
	private bool m_isUpAttacking;

	// Token: 0x04001143 RID: 4419
	private bool m_isDownAttacking;

	// Token: 0x04001144 RID: 4420
	private int m_comboNumber = 1;

	// Token: 0x04001145 RID: 4421
	private float m_canAttackAgainCounter;

	// Token: 0x04001146 RID: 4422
	private Coroutine m_pushForwardCoroutine;

	// Token: 0x04001147 RID: 4423
	private int m_punchIndex;

	// Token: 0x04001148 RID: 4424
	private Coroutine m_reenableGravityCoroutine;

	// Token: 0x04001149 RID: 4425
	private bool m_hasAttacked;

	// Token: 0x0400114A RID: 4426
	private Action<Projectile_RL, GameObject> m_continueComboEvent;

	// Token: 0x0400114B RID: 4427
	private const float CanAttackAgainDelay = 0.085f;

	// Token: 0x0400114C RID: 4428
	private const float GRAVITY_DISABLE_DURATION = 0.25f;
}
