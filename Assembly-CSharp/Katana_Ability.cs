using System;
using FMODUnity;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x0200019B RID: 411
public class Katana_Ability : Sword_Ability
{
	// Token: 0x06000F8A RID: 3978 RVA: 0x0002DA66 File Offset: 0x0002BC66
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_airborneProjectileName
		};
	}

	// Token: 0x17000891 RID: 2193
	// (get) Token: 0x06000F8B RID: 3979 RVA: 0x0002DA86 File Offset: 0x0002BC86
	protected float TellIntroAnimSpeedGround
	{
		get
		{
			return 1.4f;
		}
	}

	// Token: 0x17000892 RID: 2194
	// (get) Token: 0x06000F8C RID: 3980 RVA: 0x0002DA8D File Offset: 0x0002BC8D
	protected float TellIntroAnimSpeedAir
	{
		get
		{
			return 1.4f;
		}
	}

	// Token: 0x17000893 RID: 2195
	// (get) Token: 0x06000F8D RID: 3981 RVA: 0x0002DA94 File Offset: 0x0002BC94
	protected float TellIntroAnimExitDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000894 RID: 2196
	// (get) Token: 0x06000F8E RID: 3982 RVA: 0x0002DA9B File Offset: 0x0002BC9B
	protected float TellIntroAnimExitDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000895 RID: 2197
	// (get) Token: 0x06000F8F RID: 3983 RVA: 0x0002DAA2 File Offset: 0x0002BCA2
	protected float TellHoldAnimSpeedGround
	{
		get
		{
			return 2.35f;
		}
	}

	// Token: 0x17000896 RID: 2198
	// (get) Token: 0x06000F90 RID: 3984 RVA: 0x0002DAA9 File Offset: 0x0002BCA9
	protected float TellHoldAnimSpeedAir
	{
		get
		{
			return 2.35f;
		}
	}

	// Token: 0x17000897 RID: 2199
	// (get) Token: 0x06000F91 RID: 3985 RVA: 0x0002DAB0 File Offset: 0x0002BCB0
	protected float TellHoldAnimDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000898 RID: 2200
	// (get) Token: 0x06000F92 RID: 3986 RVA: 0x0002DAB7 File Offset: 0x0002BCB7
	protected float TellHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000899 RID: 2201
	// (get) Token: 0x06000F93 RID: 3987 RVA: 0x0002DABE File Offset: 0x0002BCBE
	protected float AttackIntroAnimSpeedGround
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700089A RID: 2202
	// (get) Token: 0x06000F94 RID: 3988 RVA: 0x0002DAC5 File Offset: 0x0002BCC5
	protected float AttackIntroAnimSpeedAir
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700089B RID: 2203
	// (get) Token: 0x06000F95 RID: 3989 RVA: 0x0002DACC File Offset: 0x0002BCCC
	protected float AttackIntroAnimExitDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700089C RID: 2204
	// (get) Token: 0x06000F96 RID: 3990 RVA: 0x0002DAD3 File Offset: 0x0002BCD3
	protected float AttackIntroAnimExitDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700089D RID: 2205
	// (get) Token: 0x06000F97 RID: 3991 RVA: 0x0002DADA File Offset: 0x0002BCDA
	protected float AttackHoldAnimSpeedGround
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700089E RID: 2206
	// (get) Token: 0x06000F98 RID: 3992 RVA: 0x0002DAE1 File Offset: 0x0002BCE1
	protected float AttackHoldAnimSpeedAir
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700089F RID: 2207
	// (get) Token: 0x06000F99 RID: 3993 RVA: 0x0002DAE8 File Offset: 0x0002BCE8
	protected float AttackHoldAnimDelayGround
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170008A0 RID: 2208
	// (get) Token: 0x06000F9A RID: 3994 RVA: 0x0002DAEF File Offset: 0x0002BCEF
	protected float AttackHoldAnimDelayAir
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170008A1 RID: 2209
	// (get) Token: 0x06000F9B RID: 3995 RVA: 0x0002DAF6 File Offset: 0x0002BCF6
	protected float ExitIntroAnimSpeedGround
	{
		get
		{
			return 2.2f;
		}
	}

	// Token: 0x170008A2 RID: 2210
	// (get) Token: 0x06000F9C RID: 3996 RVA: 0x0002DAFD File Offset: 0x0002BCFD
	protected float ExitIntroAnimSpeedAir
	{
		get
		{
			return 1.4f;
		}
	}

	// Token: 0x170008A3 RID: 2211
	// (get) Token: 0x06000F9D RID: 3997 RVA: 0x0002DB04 File Offset: 0x0002BD04
	protected float ExitHoldAnimDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008A4 RID: 2212
	// (get) Token: 0x06000F9E RID: 3998 RVA: 0x0002DB0B File Offset: 0x0002BD0B
	protected float ExitHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008A5 RID: 2213
	// (get) Token: 0x06000F9F RID: 3999 RVA: 0x0002DB12 File Offset: 0x0002BD12
	public override Vector2 ProjectileOffset
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return base.ProjectileOffset;
			}
			return this.m_airBorneProjectileOffset;
		}
	}

	// Token: 0x170008A6 RID: 2214
	// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x0002DB29 File Offset: 0x0002BD29
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

	// Token: 0x170008A7 RID: 2215
	// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x0002DB40 File Offset: 0x0002BD40
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

	// Token: 0x170008A8 RID: 2216
	// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x0002DB57 File Offset: 0x0002BD57
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

	// Token: 0x170008A9 RID: 2217
	// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x0002DB6E File Offset: 0x0002BD6E
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

	// Token: 0x170008AA RID: 2218
	// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x0002DB85 File Offset: 0x0002BD85
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

	// Token: 0x170008AB RID: 2219
	// (get) Token: 0x06000FA5 RID: 4005 RVA: 0x0002DB9C File Offset: 0x0002BD9C
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

	// Token: 0x170008AC RID: 2220
	// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x0002DBB3 File Offset: 0x0002BDB3
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

	// Token: 0x170008AD RID: 2221
	// (get) Token: 0x06000FA7 RID: 4007 RVA: 0x0002DBCA File Offset: 0x0002BDCA
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

	// Token: 0x170008AE RID: 2222
	// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x0002DBE1 File Offset: 0x0002BDE1
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

	// Token: 0x170008AF RID: 2223
	// (get) Token: 0x06000FA9 RID: 4009 RVA: 0x0002DBF8 File Offset: 0x0002BDF8
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

	// Token: 0x170008B0 RID: 2224
	// (get) Token: 0x06000FAA RID: 4010 RVA: 0x0002DC0F File Offset: 0x0002BE0F
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

	// Token: 0x170008B1 RID: 2225
	// (get) Token: 0x06000FAB RID: 4011 RVA: 0x0002DC26 File Offset: 0x0002BE26
	public override string ProjectileName
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return base.ProjectileName;
			}
			return this.m_airborneProjectileName;
		}
	}

	// Token: 0x170008B2 RID: 2226
	// (get) Token: 0x06000FAC RID: 4012 RVA: 0x0002DC3D File Offset: 0x0002BE3D
	public override AbilityData AbilityData
	{
		get
		{
			if (this.PerformGroundAttack)
			{
				return base.AbilityData;
			}
			return this.m_airborneAbilityData;
		}
	}

	// Token: 0x170008B3 RID: 2227
	// (get) Token: 0x06000FAD RID: 4013 RVA: 0x0002DC54 File Offset: 0x0002BE54
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

	// Token: 0x170008B4 RID: 2228
	// (get) Token: 0x06000FAE RID: 4014 RVA: 0x0002DC6B File Offset: 0x0002BE6B
	protected bool PerformGroundAttack
	{
		get
		{
			return (this.IsGrounded && !this.m_isAirAttacking) || this.m_isGroundAttacking;
		}
	}

	// Token: 0x170008B5 RID: 2229
	// (get) Token: 0x06000FAF RID: 4015 RVA: 0x0002DC85 File Offset: 0x0002BE85
	protected bool IsGrounded
	{
		get
		{
			return !this.m_abilityController || this.m_abilityController.PlayerController.IsGrounded;
		}
	}

	// Token: 0x06000FB0 RID: 4016 RVA: 0x0002DCAC File Offset: 0x0002BEAC
	public override void PreCastAbility()
	{
		this.m_isGroundAttacking = false;
		this.m_isAirAttacking = false;
		if (this.IsGrounded)
		{
			this.m_isGroundAttacking = true;
		}
		else
		{
			this.m_isAirAttacking = true;
		}
		if (Rewired_RL.Player.GetButton("MoveVertical"))
		{
			this.m_attackDirection = Katana_Ability.KatanaAttackDirection.Up;
		}
		else if (Rewired_RL.Player.GetNegativeButton("MoveVertical"))
		{
			this.m_attackDirection = Katana_Ability.KatanaAttackDirection.Down;
		}
		else
		{
			this.m_attackDirection = Katana_Ability.KatanaAttackDirection.None;
		}
		this.m_animator.SetFloat("Attack_Direction", (float)this.m_attackDirection);
		this.m_prepEventEmitter.Play();
		base.PreCastAbility();
	}

	// Token: 0x06000FB1 RID: 4017 RVA: 0x0002DD44 File Offset: 0x0002BF44
	protected override void FireProjectile()
	{
		base.FireProjectile();
		if (this.m_isGroundAttacking || !this.m_isGroundAttacking)
		{
			if (this.m_attackDirection == Katana_Ability.KatanaAttackDirection.Up)
			{
				Vector3 localEulerAngles = this.m_firedProjectile.transform.localEulerAngles;
				localEulerAngles.z = 35f;
				this.m_firedProjectile.transform.localEulerAngles = localEulerAngles;
				return;
			}
			if (this.m_attackDirection == Katana_Ability.KatanaAttackDirection.Down)
			{
				Vector3 localEulerAngles2 = this.m_firedProjectile.transform.localEulerAngles;
				localEulerAngles2.z = -35f;
				this.m_firedProjectile.transform.localEulerAngles = localEulerAngles2;
			}
		}
	}

	// Token: 0x06000FB2 RID: 4018 RVA: 0x0002DDD6 File Offset: 0x0002BFD6
	protected override void OnEnterExitLogic()
	{
		this.m_retractBladeEventEmitter.Play();
		base.OnEnterExitLogic();
	}

	// Token: 0x06000FB3 RID: 4019 RVA: 0x0002DDEC File Offset: 0x0002BFEC
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_isAirAttacking = false;
		this.m_isGroundAttacking = false;
		this.m_abilityController.PlayerController.DisableFriction = false;
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.gameObject.SetActive(false);
			this.m_firedProjectile = null;
		}
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(true);
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x04001178 RID: 4472
	[Header("Airborne Values")]
	[SerializeField]
	protected string m_abilityAirTellIntroName;

	// Token: 0x04001179 RID: 4473
	[SerializeField]
	protected AbilityData m_airborneAbilityData;

	// Token: 0x0400117A RID: 4474
	[SerializeField]
	protected string m_airborneProjectileName;

	// Token: 0x0400117B RID: 4475
	[SerializeField]
	protected Vector2 m_airBorneProjectileOffset;

	// Token: 0x0400117C RID: 4476
	[SerializeField]
	protected bool m_hasAirborneAttackFlipCheck;

	// Token: 0x0400117D RID: 4477
	[Header("Audio Event Emitters")]
	[SerializeField]
	private StudioEventEmitter m_prepEventEmitter;

	// Token: 0x0400117E RID: 4478
	[SerializeField]
	private StudioEventEmitter m_retractBladeEventEmitter;

	// Token: 0x0400117F RID: 4479
	private bool m_isAirAttacking;

	// Token: 0x04001180 RID: 4480
	private bool m_isGroundAttacking;

	// Token: 0x04001181 RID: 4481
	private float TellIntroAnim = 1f;

	// Token: 0x04001182 RID: 4482
	private float TellHoldAnim = 2f;

	// Token: 0x04001183 RID: 4483
	private float AttackIntroAnim = 1f;

	// Token: 0x04001184 RID: 4484
	private float AttackHoldAnim = 1f;

	// Token: 0x04001185 RID: 4485
	private float ExitIntroAnim = 1f;

	// Token: 0x04001186 RID: 4486
	private float Attack_Ground_HoldDelay = 0.1f;

	// Token: 0x04001187 RID: 4487
	private float Attack_Air_HoldDelay = 0.1f;

	// Token: 0x04001188 RID: 4488
	private Katana_Ability.KatanaAttackDirection m_attackDirection;

	// Token: 0x02000AD2 RID: 2770
	private enum KatanaAttackDirection
	{
		// Token: 0x04004A46 RID: 19014
		Up = 1,
		// Token: 0x04004A47 RID: 19015
		None,
		// Token: 0x04004A48 RID: 19016
		Down
	}
}
