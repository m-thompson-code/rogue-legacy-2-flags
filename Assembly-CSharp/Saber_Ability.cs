using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using RLAudio;
using UnityEngine;

// Token: 0x020001A5 RID: 421
public class Saber_Ability : Sword_Ability
{
	// Token: 0x06001054 RID: 4180 RVA: 0x0002F822 File Offset: 0x0002DA22
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_airborneProjectileName,
			this.m_upAttackProjectileName
		};
	}

	// Token: 0x1700090D RID: 2317
	// (get) Token: 0x06001055 RID: 4181 RVA: 0x0002F84B File Offset: 0x0002DA4B
	protected float TellIntroAnimSpeedGround
	{
		get
		{
			return this.TellIntroAnim;
		}
	}

	// Token: 0x1700090E RID: 2318
	// (get) Token: 0x06001056 RID: 4182 RVA: 0x0002F853 File Offset: 0x0002DA53
	protected float TellIntroAnimSpeedAir
	{
		get
		{
			return this.TellIntroAnim;
		}
	}

	// Token: 0x1700090F RID: 2319
	// (get) Token: 0x06001057 RID: 4183 RVA: 0x0002F85B File Offset: 0x0002DA5B
	protected float TellIntroAnimSpeedUpAttack
	{
		get
		{
			return this.TellIntroAnim;
		}
	}

	// Token: 0x17000910 RID: 2320
	// (get) Token: 0x06001058 RID: 4184 RVA: 0x0002F863 File Offset: 0x0002DA63
	protected float TellIntroAnimExitDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000911 RID: 2321
	// (get) Token: 0x06001059 RID: 4185 RVA: 0x0002F86A File Offset: 0x0002DA6A
	protected float TellIntroAnimExitDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000912 RID: 2322
	// (get) Token: 0x0600105A RID: 4186 RVA: 0x0002F871 File Offset: 0x0002DA71
	protected float TellIntroAnimExitDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000913 RID: 2323
	// (get) Token: 0x0600105B RID: 4187 RVA: 0x0002F878 File Offset: 0x0002DA78
	protected float TellHoldAnimSpeedGround
	{
		get
		{
			return this.TellHoldAnim;
		}
	}

	// Token: 0x17000914 RID: 2324
	// (get) Token: 0x0600105C RID: 4188 RVA: 0x0002F880 File Offset: 0x0002DA80
	protected float TellHoldAnimSpeedAir
	{
		get
		{
			return this.TellHoldAnim;
		}
	}

	// Token: 0x17000915 RID: 2325
	// (get) Token: 0x0600105D RID: 4189 RVA: 0x0002F888 File Offset: 0x0002DA88
	protected float TellHoldAnimSpeedUpAttack
	{
		get
		{
			return this.TellHoldAnim;
		}
	}

	// Token: 0x17000916 RID: 2326
	// (get) Token: 0x0600105E RID: 4190 RVA: 0x0002F890 File Offset: 0x0002DA90
	protected float TellHoldAnimDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000917 RID: 2327
	// (get) Token: 0x0600105F RID: 4191 RVA: 0x0002F897 File Offset: 0x0002DA97
	protected float TellHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000918 RID: 2328
	// (get) Token: 0x06001060 RID: 4192 RVA: 0x0002F89E File Offset: 0x0002DA9E
	protected float TellHoldAnimDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000919 RID: 2329
	// (get) Token: 0x06001061 RID: 4193 RVA: 0x0002F8A5 File Offset: 0x0002DAA5
	protected float AttackIntroAnimSpeedGround
	{
		get
		{
			return this.AttackIntroAnim;
		}
	}

	// Token: 0x1700091A RID: 2330
	// (get) Token: 0x06001062 RID: 4194 RVA: 0x0002F8AD File Offset: 0x0002DAAD
	protected float AttackIntroAnimSpeedAir
	{
		get
		{
			return this.AttackIntroAnim;
		}
	}

	// Token: 0x1700091B RID: 2331
	// (get) Token: 0x06001063 RID: 4195 RVA: 0x0002F8B5 File Offset: 0x0002DAB5
	protected float AttackIntroAnimSpeedUpAttack
	{
		get
		{
			return this.AttackIntroAnim;
		}
	}

	// Token: 0x1700091C RID: 2332
	// (get) Token: 0x06001064 RID: 4196 RVA: 0x0002F8BD File Offset: 0x0002DABD
	protected float AttackIntroAnimExitDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700091D RID: 2333
	// (get) Token: 0x06001065 RID: 4197 RVA: 0x0002F8C4 File Offset: 0x0002DAC4
	protected float AttackIntroAnimExitDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700091E RID: 2334
	// (get) Token: 0x06001066 RID: 4198 RVA: 0x0002F8CB File Offset: 0x0002DACB
	protected float AttackIntroAnimExitDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700091F RID: 2335
	// (get) Token: 0x06001067 RID: 4199 RVA: 0x0002F8D2 File Offset: 0x0002DAD2
	protected float AttackHoldAnimSpeedGround
	{
		get
		{
			return this.AttackHoldAnim;
		}
	}

	// Token: 0x17000920 RID: 2336
	// (get) Token: 0x06001068 RID: 4200 RVA: 0x0002F8DA File Offset: 0x0002DADA
	protected float AttackHoldAnimSpeedAir
	{
		get
		{
			return this.AttackHoldAnim;
		}
	}

	// Token: 0x17000921 RID: 2337
	// (get) Token: 0x06001069 RID: 4201 RVA: 0x0002F8E2 File Offset: 0x0002DAE2
	protected float AttackHoldAnimSpeedUpAttack
	{
		get
		{
			return this.AttackHoldAnim;
		}
	}

	// Token: 0x17000922 RID: 2338
	// (get) Token: 0x0600106A RID: 4202 RVA: 0x0002F8EA File Offset: 0x0002DAEA
	protected float AttackHoldAnimDelayGround
	{
		get
		{
			return this.AttackHoldDelay;
		}
	}

	// Token: 0x17000923 RID: 2339
	// (get) Token: 0x0600106B RID: 4203 RVA: 0x0002F8F2 File Offset: 0x0002DAF2
	protected float AttackHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000924 RID: 2340
	// (get) Token: 0x0600106C RID: 4204 RVA: 0x0002F8F9 File Offset: 0x0002DAF9
	protected float AttackHoldAnimDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000925 RID: 2341
	// (get) Token: 0x0600106D RID: 4205 RVA: 0x0002F900 File Offset: 0x0002DB00
	protected float ExitIntroAnimSpeedGround
	{
		get
		{
			return this.ExitIntroAnim;
		}
	}

	// Token: 0x17000926 RID: 2342
	// (get) Token: 0x0600106E RID: 4206 RVA: 0x0002F908 File Offset: 0x0002DB08
	protected float ExitIntroAnimSpeedAir
	{
		get
		{
			return this.ExitIntroAnim;
		}
	}

	// Token: 0x17000927 RID: 2343
	// (get) Token: 0x0600106F RID: 4207 RVA: 0x0002F910 File Offset: 0x0002DB10
	protected float ExitIntroAnimSpeedUpAttack
	{
		get
		{
			return this.ExitIntroAnim;
		}
	}

	// Token: 0x17000928 RID: 2344
	// (get) Token: 0x06001070 RID: 4208 RVA: 0x0002F918 File Offset: 0x0002DB18
	protected float ExitHoldAnimDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000929 RID: 2345
	// (get) Token: 0x06001071 RID: 4209 RVA: 0x0002F91F File Offset: 0x0002DB1F
	protected float ExitHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700092A RID: 2346
	// (get) Token: 0x06001072 RID: 4210 RVA: 0x0002F926 File Offset: 0x0002DB26
	protected float ExitHoldAnimDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700092B RID: 2347
	// (get) Token: 0x06001073 RID: 4211 RVA: 0x0002F92D File Offset: 0x0002DB2D
	public override Vector2 ProjectileOffset
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.m_upAttackProjectileOffset;
			}
			if (this.PerformGroundAttack)
			{
				return base.ProjectileOffset;
			}
			return this.m_airBorneProjectileOffset;
		}
	}

	// Token: 0x1700092C RID: 2348
	// (get) Token: 0x06001074 RID: 4212 RVA: 0x0002F953 File Offset: 0x0002DB53
	protected override float TellIntroAnimSpeed
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.TellIntroAnimSpeedUpAttack;
			}
			if (this.PerformGroundAttack)
			{
				return this.TellIntroAnimSpeedGround;
			}
			return this.TellIntroAnimSpeedAir;
		}
	}

	// Token: 0x1700092D RID: 2349
	// (get) Token: 0x06001075 RID: 4213 RVA: 0x0002F979 File Offset: 0x0002DB79
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.TellIntroAnimExitDelayUpAttack;
			}
			if (this.PerformGroundAttack)
			{
				return this.TellIntroAnimExitDelayGround;
			}
			return this.TellIntroAnimExitDelayAir;
		}
	}

	// Token: 0x1700092E RID: 2350
	// (get) Token: 0x06001076 RID: 4214 RVA: 0x0002F99F File Offset: 0x0002DB9F
	protected override float TellAnimSpeed
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.TellHoldAnimSpeedUpAttack;
			}
			if (this.PerformGroundAttack)
			{
				return this.TellHoldAnimSpeedGround;
			}
			return this.TellHoldAnimSpeedAir;
		}
	}

	// Token: 0x1700092F RID: 2351
	// (get) Token: 0x06001077 RID: 4215 RVA: 0x0002F9C5 File Offset: 0x0002DBC5
	protected override float TellAnimExitDelay
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.TellHoldAnimDelayUpAttack;
			}
			if (this.PerformGroundAttack)
			{
				return this.TellHoldAnimDelayGround;
			}
			return this.TellHoldAnimDelayAir;
		}
	}

	// Token: 0x17000930 RID: 2352
	// (get) Token: 0x06001078 RID: 4216 RVA: 0x0002F9EB File Offset: 0x0002DBEB
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.AttackIntroAnimSpeedUpAttack;
			}
			if (this.PerformGroundAttack)
			{
				return this.AttackIntroAnimSpeedGround;
			}
			return this.AttackIntroAnimSpeedAir;
		}
	}

	// Token: 0x17000931 RID: 2353
	// (get) Token: 0x06001079 RID: 4217 RVA: 0x0002FA11 File Offset: 0x0002DC11
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.AttackIntroAnimExitDelayUpAttack;
			}
			if (this.PerformGroundAttack)
			{
				return this.AttackIntroAnimExitDelayGround;
			}
			return this.AttackIntroAnimExitDelayAir;
		}
	}

	// Token: 0x17000932 RID: 2354
	// (get) Token: 0x0600107A RID: 4218 RVA: 0x0002FA37 File Offset: 0x0002DC37
	protected override float AttackAnimSpeed
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.AttackHoldAnimSpeedUpAttack;
			}
			if (this.IsGrounded)
			{
				return this.AttackHoldAnimSpeedGround;
			}
			return this.AttackHoldAnimSpeedAir;
		}
	}

	// Token: 0x17000933 RID: 2355
	// (get) Token: 0x0600107B RID: 4219 RVA: 0x0002FA5D File Offset: 0x0002DC5D
	protected override float AttackAnimExitDelay
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.AttackHoldAnimDelayUpAttack;
			}
			if (this.PerformGroundAttack)
			{
				return this.AttackHoldAnimDelayGround;
			}
			return this.AttackHoldAnimDelayAir;
		}
	}

	// Token: 0x17000934 RID: 2356
	// (get) Token: 0x0600107C RID: 4220 RVA: 0x0002FA83 File Offset: 0x0002DC83
	protected override float ExitAnimSpeed
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.ExitIntroAnimSpeedUpAttack;
			}
			if (this.PerformGroundAttack)
			{
				return this.ExitIntroAnimSpeedGround;
			}
			return this.ExitIntroAnimSpeedAir;
		}
	}

	// Token: 0x17000935 RID: 2357
	// (get) Token: 0x0600107D RID: 4221 RVA: 0x0002FAA9 File Offset: 0x0002DCA9
	protected override float ExitAnimExitDelay
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.ExitHoldAnimDelayUpAttack;
			}
			if (this.PerformGroundAttack)
			{
				return this.ExitHoldAnimDelayGround;
			}
			return this.ExitHoldAnimDelayAir;
		}
	}

	// Token: 0x17000936 RID: 2358
	// (get) Token: 0x0600107E RID: 4222 RVA: 0x0002FACF File Offset: 0x0002DCCF
	public override bool HasAttackFlipCheck
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.m_hasUpAttackFlipCheck;
			}
			if (this.PerformGroundAttack)
			{
				return base.HasAttackFlipCheck;
			}
			return this.m_hasAirborneAttackFlipCheck;
		}
	}

	// Token: 0x17000937 RID: 2359
	// (get) Token: 0x0600107F RID: 4223 RVA: 0x0002FAF5 File Offset: 0x0002DCF5
	public override string ProjectileName
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.m_upAttackProjectileName;
			}
			if (this.PerformGroundAttack)
			{
				return base.ProjectileName;
			}
			return this.m_airborneProjectileName;
		}
	}

	// Token: 0x17000938 RID: 2360
	// (get) Token: 0x06001080 RID: 4224 RVA: 0x0002FB1B File Offset: 0x0002DD1B
	public override AbilityData AbilityData
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.m_upAttackAbilityData;
			}
			if (this.PerformGroundAttack)
			{
				return base.AbilityData;
			}
			return this.m_airborneAbilityData;
		}
	}

	// Token: 0x17000939 RID: 2361
	// (get) Token: 0x06001081 RID: 4225 RVA: 0x0002FB41 File Offset: 0x0002DD41
	public override string AbilityTellIntroName
	{
		get
		{
			if (this.PerformUpAttack)
			{
				return this.m_upAttackTellIntroName;
			}
			if (this.PerformGroundAttack)
			{
				return base.AbilityTellIntroName;
			}
			return this.m_abilityAirTellIntroName;
		}
	}

	// Token: 0x1700093A RID: 2362
	// (get) Token: 0x06001082 RID: 4226 RVA: 0x0002FB67 File Offset: 0x0002DD67
	protected bool PerformUpAttack
	{
		get
		{
			return this.m_isUpAttacking;
		}
	}

	// Token: 0x1700093B RID: 2363
	// (get) Token: 0x06001083 RID: 4227 RVA: 0x0002FB6F File Offset: 0x0002DD6F
	protected bool PerformGroundAttack
	{
		get
		{
			return (this.IsGrounded && !this.m_isAirAttacking) || this.m_isGroundAttacking;
		}
	}

	// Token: 0x1700093C RID: 2364
	// (get) Token: 0x06001084 RID: 4228 RVA: 0x0002FB89 File Offset: 0x0002DD89
	protected bool IsGrounded
	{
		get
		{
			return !this.m_abilityController || this.m_abilityController.PlayerController.IsGrounded;
		}
	}

	// Token: 0x06001085 RID: 4229 RVA: 0x0002FBAD File Offset: 0x0002DDAD
	protected override void Awake()
	{
		this.m_waitFixedUpdateYield = new WaitForFixedUpdate();
		base.Awake();
	}

	// Token: 0x06001086 RID: 4230 RVA: 0x0002FBC0 File Offset: 0x0002DDC0
	public override void PreCastAbility()
	{
		this.m_isGroundAttacking = false;
		this.m_isAirAttacking = false;
		this.m_isUpAttacking = false;
		this.m_ignoreAnimationComplete = false;
		if (this.IsGrounded)
		{
			this.m_isGroundAttacking = true;
			this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
		}
		else
		{
			this.m_isAirAttacking = true;
		}
		this.m_lastAttackWasAirAttack = this.m_isAirAttacking;
		this.m_performedAttackCancel = false;
		base.PreCastAbility();
	}

	// Token: 0x06001087 RID: 4231 RVA: 0x0002FC46 File Offset: 0x0002DE46
	protected override IEnumerator ChangeAnim(float duration)
	{
		if (base.CurrentAbilityAnimState == AbilityAnimState.TellIntro && this.IsGrounded)
		{
			yield return base.ChangeAnim(0f);
		}
		else
		{
			yield return base.ChangeAnim(duration);
		}
		yield break;
	}

	// Token: 0x06001088 RID: 4232 RVA: 0x0002FC5C File Offset: 0x0002DE5C
	protected IEnumerator PushForward()
	{
		float speed = 0f;
		float num = 0f / speed;
		float endingTime = Time.fixedTime + num;
		if (this.m_abilityController.PlayerController.IsFacingRight)
		{
			this.m_abilityController.PlayerController.SetVelocityX(speed, false);
		}
		else
		{
			this.m_abilityController.PlayerController.SetVelocityX(-speed, false);
		}
		if (this.IsGrounded)
		{
			this.m_abilityController.PlayerController.MovementState = CharacterStates.MovementStates.Idle;
		}
		while (Time.fixedTime < endingTime)
		{
			float num2 = speed * 1f;
			this.m_abilityController.PlayerController.DisableFriction = true;
			if (this.m_abilityController.PlayerController.IsFacingRight)
			{
				this.m_abilityController.PlayerController.SetVelocityX(num2, false);
			}
			else
			{
				this.m_abilityController.PlayerController.SetVelocityX(-num2, false);
			}
			this.PreventPlatformDrop();
			yield return this.m_waitFixedUpdateYield;
		}
		this.m_abilityController.PlayerController.SetVelocityX(0f, false);
		this.m_abilityController.PlayerController.DisableFriction = false;
		yield break;
	}

	// Token: 0x06001089 RID: 4233 RVA: 0x0002FC6C File Offset: 0x0002DE6C
	private void PreventPlatformDrop()
	{
		CorgiController_RL controllerCorgi = this.m_abilityController.PlayerController.ControllerCorgi;
		Vector2 origin = controllerCorgi.BoundsBottomLeftCorner;
		Vector2 origin2 = controllerCorgi.BoundsBottomRightCorner;
		float distance = Mathf.Clamp(controllerCorgi.BoundsHeight / 2f + controllerCorgi.BoundsWidth / 2f, controllerCorgi.StickyRaycastLength + 0.5f, float.MaxValue);
		if (this.m_abilityController.PlayerController.Velocity.x > 0f)
		{
			if (!Physics2D.Raycast(origin2, Vector2.down, distance, this.m_abilityController.PlayerController.ControllerCorgi.SavedPlatformMask))
			{
				this.m_abilityController.PlayerController.SetVelocityX(0f, false);
				return;
			}
		}
		else if (this.m_abilityController.PlayerController.Velocity.x < 0f && !Physics2D.Raycast(origin, Vector2.down, distance, this.m_abilityController.PlayerController.ControllerCorgi.SavedPlatformMask))
		{
			this.m_abilityController.PlayerController.SetVelocityX(0f, false);
		}
	}

	// Token: 0x0600108A RID: 4234 RVA: 0x0002FD94 File Offset: 0x0002DF94
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_isAirAttacking = false;
		this.m_isGroundAttacking = false;
		this.m_isUpAttacking = false;
		this.m_ignoreAnimationComplete = false;
		this.m_abilityController.PlayerController.DisableFriction = false;
		if (this.m_firedProjectile != null)
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

	// Token: 0x0600108B RID: 4235 RVA: 0x0002FE38 File Offset: 0x0002E038
	protected override void Update()
	{
		base.Update();
		if (Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)) && this.m_lastAttackWasAirAttack && this.IsGrounded)
		{
			float axis = this.m_abilityController.PlayerController.CharacterCorgi.REPlayer.GetAxis("MoveHorizontal");
			if ((axis > 0f && !this.m_abilityController.PlayerController.IsFacingRight) || (axis < 0f && this.m_abilityController.PlayerController.IsFacingRight))
			{
				this.m_abilityController.PlayerController.CharacterCorgi.Flip(false, false);
			}
			this.m_abilityController.StopAbility(base.CastAbilityType, true);
			if (base.CurrentAbilityAnimState == AbilityAnimState.Exit)
			{
				this.m_ignoreAnimationComplete = true;
			}
			this.m_performedAttackCancel = true;
			this.m_abilityController.StartAbility(base.CastAbilityType, false, false);
		}
	}

	// Token: 0x0600108C RID: 4236 RVA: 0x0002FF28 File Offset: 0x0002E128
	protected override void OnExitExitLogic()
	{
		if (!this.m_ignoreAnimationComplete)
		{
			base.OnExitExitLogic();
		}
		this.m_ignoreAnimationComplete = false;
	}

	// Token: 0x0600108D RID: 4237 RVA: 0x0002FF3F File Offset: 0x0002E13F
	protected override void PlayDashCritAudio()
	{
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Weapons/sfx_weapon_crit_sabre_swing", base.gameObject);
	}

	// Token: 0x040011BB RID: 4539
	[Header("Airborne Values")]
	[SerializeField]
	protected string m_abilityAirTellIntroName;

	// Token: 0x040011BC RID: 4540
	[SerializeField]
	protected AbilityData m_airborneAbilityData;

	// Token: 0x040011BD RID: 4541
	[SerializeField]
	protected string m_airborneProjectileName;

	// Token: 0x040011BE RID: 4542
	[SerializeField]
	protected Vector2 m_airBorneProjectileOffset;

	// Token: 0x040011BF RID: 4543
	[SerializeField]
	protected bool m_hasAirborneAttackFlipCheck;

	// Token: 0x040011C0 RID: 4544
	[Header("Upward Attack Values")]
	[SerializeField]
	protected string m_upAttackTellIntroName;

	// Token: 0x040011C1 RID: 4545
	[SerializeField]
	protected AbilityData m_upAttackAbilityData;

	// Token: 0x040011C2 RID: 4546
	[SerializeField]
	protected string m_upAttackProjectileName;

	// Token: 0x040011C3 RID: 4547
	[SerializeField]
	protected Vector2 m_upAttackProjectileOffset;

	// Token: 0x040011C4 RID: 4548
	[SerializeField]
	protected bool m_hasUpAttackFlipCheck;

	// Token: 0x040011C5 RID: 4549
	private bool m_isAirAttacking;

	// Token: 0x040011C6 RID: 4550
	private bool m_isGroundAttacking;

	// Token: 0x040011C7 RID: 4551
	private bool m_isUpAttacking;

	// Token: 0x040011C8 RID: 4552
	private bool m_performedAttackCancel;

	// Token: 0x040011C9 RID: 4553
	private bool m_lastAttackWasAirAttack;

	// Token: 0x040011CA RID: 4554
	private WaitForFixedUpdate m_waitFixedUpdateYield;

	// Token: 0x040011CB RID: 4555
	private bool m_ignoreAnimationComplete;

	// Token: 0x040011CC RID: 4556
	private float TellIntroAnim = 1.6f;

	// Token: 0x040011CD RID: 4557
	private float TellHoldAnim = 2f;

	// Token: 0x040011CE RID: 4558
	private float AttackIntroAnim = 1f;

	// Token: 0x040011CF RID: 4559
	private float AttackHoldAnim = 1.5f;

	// Token: 0x040011D0 RID: 4560
	private float ExitIntroAnim = 1f;

	// Token: 0x040011D1 RID: 4561
	private float ExitHoldAnim = 1f;

	// Token: 0x040011D2 RID: 4562
	private float AttackHoldDelay = 0.1f;
}
