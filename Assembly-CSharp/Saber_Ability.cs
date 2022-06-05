using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using RLAudio;
using UnityEngine;

// Token: 0x02000309 RID: 777
public class Saber_Ability : Sword_Ability
{
	// Token: 0x06001865 RID: 6245 RVA: 0x0000C4A2 File Offset: 0x0000A6A2
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_airborneProjectileName,
			this.m_upAttackProjectileName
		};
	}

	// Token: 0x17000BBB RID: 3003
	// (get) Token: 0x06001866 RID: 6246 RVA: 0x0000C4CB File Offset: 0x0000A6CB
	protected float TellIntroAnimSpeedGround
	{
		get
		{
			return this.TellIntroAnim;
		}
	}

	// Token: 0x17000BBC RID: 3004
	// (get) Token: 0x06001867 RID: 6247 RVA: 0x0000C4CB File Offset: 0x0000A6CB
	protected float TellIntroAnimSpeedAir
	{
		get
		{
			return this.TellIntroAnim;
		}
	}

	// Token: 0x17000BBD RID: 3005
	// (get) Token: 0x06001868 RID: 6248 RVA: 0x0000C4CB File Offset: 0x0000A6CB
	protected float TellIntroAnimSpeedUpAttack
	{
		get
		{
			return this.TellIntroAnim;
		}
	}

	// Token: 0x17000BBE RID: 3006
	// (get) Token: 0x06001869 RID: 6249 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellIntroAnimExitDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BBF RID: 3007
	// (get) Token: 0x0600186A RID: 6250 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellIntroAnimExitDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BC0 RID: 3008
	// (get) Token: 0x0600186B RID: 6251 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellIntroAnimExitDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BC1 RID: 3009
	// (get) Token: 0x0600186C RID: 6252 RVA: 0x0000C4D3 File Offset: 0x0000A6D3
	protected float TellHoldAnimSpeedGround
	{
		get
		{
			return this.TellHoldAnim;
		}
	}

	// Token: 0x17000BC2 RID: 3010
	// (get) Token: 0x0600186D RID: 6253 RVA: 0x0000C4D3 File Offset: 0x0000A6D3
	protected float TellHoldAnimSpeedAir
	{
		get
		{
			return this.TellHoldAnim;
		}
	}

	// Token: 0x17000BC3 RID: 3011
	// (get) Token: 0x0600186E RID: 6254 RVA: 0x0000C4D3 File Offset: 0x0000A6D3
	protected float TellHoldAnimSpeedUpAttack
	{
		get
		{
			return this.TellHoldAnim;
		}
	}

	// Token: 0x17000BC4 RID: 3012
	// (get) Token: 0x0600186F RID: 6255 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellHoldAnimDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BC5 RID: 3013
	// (get) Token: 0x06001870 RID: 6256 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BC6 RID: 3014
	// (get) Token: 0x06001871 RID: 6257 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float TellHoldAnimDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BC7 RID: 3015
	// (get) Token: 0x06001872 RID: 6258 RVA: 0x0000C4DB File Offset: 0x0000A6DB
	protected float AttackIntroAnimSpeedGround
	{
		get
		{
			return this.AttackIntroAnim;
		}
	}

	// Token: 0x17000BC8 RID: 3016
	// (get) Token: 0x06001873 RID: 6259 RVA: 0x0000C4DB File Offset: 0x0000A6DB
	protected float AttackIntroAnimSpeedAir
	{
		get
		{
			return this.AttackIntroAnim;
		}
	}

	// Token: 0x17000BC9 RID: 3017
	// (get) Token: 0x06001874 RID: 6260 RVA: 0x0000C4DB File Offset: 0x0000A6DB
	protected float AttackIntroAnimSpeedUpAttack
	{
		get
		{
			return this.AttackIntroAnim;
		}
	}

	// Token: 0x17000BCA RID: 3018
	// (get) Token: 0x06001875 RID: 6261 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackIntroAnimExitDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BCB RID: 3019
	// (get) Token: 0x06001876 RID: 6262 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackIntroAnimExitDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BCC RID: 3020
	// (get) Token: 0x06001877 RID: 6263 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackIntroAnimExitDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BCD RID: 3021
	// (get) Token: 0x06001878 RID: 6264 RVA: 0x0000C4E3 File Offset: 0x0000A6E3
	protected float AttackHoldAnimSpeedGround
	{
		get
		{
			return this.AttackHoldAnim;
		}
	}

	// Token: 0x17000BCE RID: 3022
	// (get) Token: 0x06001879 RID: 6265 RVA: 0x0000C4E3 File Offset: 0x0000A6E3
	protected float AttackHoldAnimSpeedAir
	{
		get
		{
			return this.AttackHoldAnim;
		}
	}

	// Token: 0x17000BCF RID: 3023
	// (get) Token: 0x0600187A RID: 6266 RVA: 0x0000C4E3 File Offset: 0x0000A6E3
	protected float AttackHoldAnimSpeedUpAttack
	{
		get
		{
			return this.AttackHoldAnim;
		}
	}

	// Token: 0x17000BD0 RID: 3024
	// (get) Token: 0x0600187B RID: 6267 RVA: 0x0000C4EB File Offset: 0x0000A6EB
	protected float AttackHoldAnimDelayGround
	{
		get
		{
			return this.AttackHoldDelay;
		}
	}

	// Token: 0x17000BD1 RID: 3025
	// (get) Token: 0x0600187C RID: 6268 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BD2 RID: 3026
	// (get) Token: 0x0600187D RID: 6269 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float AttackHoldAnimDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BD3 RID: 3027
	// (get) Token: 0x0600187E RID: 6270 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
	protected float ExitIntroAnimSpeedGround
	{
		get
		{
			return this.ExitIntroAnim;
		}
	}

	// Token: 0x17000BD4 RID: 3028
	// (get) Token: 0x0600187F RID: 6271 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
	protected float ExitIntroAnimSpeedAir
	{
		get
		{
			return this.ExitIntroAnim;
		}
	}

	// Token: 0x17000BD5 RID: 3029
	// (get) Token: 0x06001880 RID: 6272 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
	protected float ExitIntroAnimSpeedUpAttack
	{
		get
		{
			return this.ExitIntroAnim;
		}
	}

	// Token: 0x17000BD6 RID: 3030
	// (get) Token: 0x06001881 RID: 6273 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float ExitHoldAnimDelayGround
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BD7 RID: 3031
	// (get) Token: 0x06001882 RID: 6274 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float ExitHoldAnimDelayAir
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BD8 RID: 3032
	// (get) Token: 0x06001883 RID: 6275 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected float ExitHoldAnimDelayUpAttack
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BD9 RID: 3033
	// (get) Token: 0x06001884 RID: 6276 RVA: 0x0000C4FB File Offset: 0x0000A6FB
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

	// Token: 0x17000BDA RID: 3034
	// (get) Token: 0x06001885 RID: 6277 RVA: 0x0000C521 File Offset: 0x0000A721
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

	// Token: 0x17000BDB RID: 3035
	// (get) Token: 0x06001886 RID: 6278 RVA: 0x0000C547 File Offset: 0x0000A747
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

	// Token: 0x17000BDC RID: 3036
	// (get) Token: 0x06001887 RID: 6279 RVA: 0x0000C56D File Offset: 0x0000A76D
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

	// Token: 0x17000BDD RID: 3037
	// (get) Token: 0x06001888 RID: 6280 RVA: 0x0000C593 File Offset: 0x0000A793
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

	// Token: 0x17000BDE RID: 3038
	// (get) Token: 0x06001889 RID: 6281 RVA: 0x0000C5B9 File Offset: 0x0000A7B9
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

	// Token: 0x17000BDF RID: 3039
	// (get) Token: 0x0600188A RID: 6282 RVA: 0x0000C5DF File Offset: 0x0000A7DF
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

	// Token: 0x17000BE0 RID: 3040
	// (get) Token: 0x0600188B RID: 6283 RVA: 0x0000C605 File Offset: 0x0000A805
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

	// Token: 0x17000BE1 RID: 3041
	// (get) Token: 0x0600188C RID: 6284 RVA: 0x0000C62B File Offset: 0x0000A82B
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

	// Token: 0x17000BE2 RID: 3042
	// (get) Token: 0x0600188D RID: 6285 RVA: 0x0000C651 File Offset: 0x0000A851
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

	// Token: 0x17000BE3 RID: 3043
	// (get) Token: 0x0600188E RID: 6286 RVA: 0x0000C677 File Offset: 0x0000A877
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

	// Token: 0x17000BE4 RID: 3044
	// (get) Token: 0x0600188F RID: 6287 RVA: 0x0000C69D File Offset: 0x0000A89D
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

	// Token: 0x17000BE5 RID: 3045
	// (get) Token: 0x06001890 RID: 6288 RVA: 0x0000C6C3 File Offset: 0x0000A8C3
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

	// Token: 0x17000BE6 RID: 3046
	// (get) Token: 0x06001891 RID: 6289 RVA: 0x0000C6E9 File Offset: 0x0000A8E9
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

	// Token: 0x17000BE7 RID: 3047
	// (get) Token: 0x06001892 RID: 6290 RVA: 0x0000C70F File Offset: 0x0000A90F
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

	// Token: 0x17000BE8 RID: 3048
	// (get) Token: 0x06001893 RID: 6291 RVA: 0x0000C735 File Offset: 0x0000A935
	protected bool PerformUpAttack
	{
		get
		{
			return this.m_isUpAttacking;
		}
	}

	// Token: 0x17000BE9 RID: 3049
	// (get) Token: 0x06001894 RID: 6292 RVA: 0x0000C73D File Offset: 0x0000A93D
	protected bool PerformGroundAttack
	{
		get
		{
			return (this.IsGrounded && !this.m_isAirAttacking) || this.m_isGroundAttacking;
		}
	}

	// Token: 0x17000BEA RID: 3050
	// (get) Token: 0x06001895 RID: 6293 RVA: 0x0000B815 File Offset: 0x00009A15
	protected bool IsGrounded
	{
		get
		{
			return !this.m_abilityController || this.m_abilityController.PlayerController.IsGrounded;
		}
	}

	// Token: 0x06001896 RID: 6294 RVA: 0x0000C757 File Offset: 0x0000A957
	protected override void Awake()
	{
		this.m_waitFixedUpdateYield = new WaitForFixedUpdate();
		base.Awake();
	}

	// Token: 0x06001897 RID: 6295 RVA: 0x0008DF84 File Offset: 0x0008C184
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

	// Token: 0x06001898 RID: 6296 RVA: 0x0000C76A File Offset: 0x0000A96A
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

	// Token: 0x06001899 RID: 6297 RVA: 0x0000C780 File Offset: 0x0000A980
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

	// Token: 0x0600189A RID: 6298 RVA: 0x0008E00C File Offset: 0x0008C20C
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

	// Token: 0x0600189B RID: 6299 RVA: 0x0008E134 File Offset: 0x0008C334
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

	// Token: 0x0600189C RID: 6300 RVA: 0x0008E1D8 File Offset: 0x0008C3D8
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

	// Token: 0x0600189D RID: 6301 RVA: 0x0000C78F File Offset: 0x0000A98F
	protected override void OnExitExitLogic()
	{
		if (!this.m_ignoreAnimationComplete)
		{
			base.OnExitExitLogic();
		}
		this.m_ignoreAnimationComplete = false;
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x0000C7A6 File Offset: 0x0000A9A6
	protected override void PlayDashCritAudio()
	{
		AudioManager.PlayOneShotAttached(this, "event:/SFX/Weapons/sfx_weapon_crit_sabre_swing", base.gameObject);
	}

	// Token: 0x04001798 RID: 6040
	[Header("Airborne Values")]
	[SerializeField]
	protected string m_abilityAirTellIntroName;

	// Token: 0x04001799 RID: 6041
	[SerializeField]
	protected AbilityData m_airborneAbilityData;

	// Token: 0x0400179A RID: 6042
	[SerializeField]
	protected string m_airborneProjectileName;

	// Token: 0x0400179B RID: 6043
	[SerializeField]
	protected Vector2 m_airBorneProjectileOffset;

	// Token: 0x0400179C RID: 6044
	[SerializeField]
	protected bool m_hasAirborneAttackFlipCheck;

	// Token: 0x0400179D RID: 6045
	[Header("Upward Attack Values")]
	[SerializeField]
	protected string m_upAttackTellIntroName;

	// Token: 0x0400179E RID: 6046
	[SerializeField]
	protected AbilityData m_upAttackAbilityData;

	// Token: 0x0400179F RID: 6047
	[SerializeField]
	protected string m_upAttackProjectileName;

	// Token: 0x040017A0 RID: 6048
	[SerializeField]
	protected Vector2 m_upAttackProjectileOffset;

	// Token: 0x040017A1 RID: 6049
	[SerializeField]
	protected bool m_hasUpAttackFlipCheck;

	// Token: 0x040017A2 RID: 6050
	private bool m_isAirAttacking;

	// Token: 0x040017A3 RID: 6051
	private bool m_isGroundAttacking;

	// Token: 0x040017A4 RID: 6052
	private bool m_isUpAttacking;

	// Token: 0x040017A5 RID: 6053
	private bool m_performedAttackCancel;

	// Token: 0x040017A6 RID: 6054
	private bool m_lastAttackWasAirAttack;

	// Token: 0x040017A7 RID: 6055
	private WaitForFixedUpdate m_waitFixedUpdateYield;

	// Token: 0x040017A8 RID: 6056
	private bool m_ignoreAnimationComplete;

	// Token: 0x040017A9 RID: 6057
	private float TellIntroAnim = 1.6f;

	// Token: 0x040017AA RID: 6058
	private float TellHoldAnim = 2f;

	// Token: 0x040017AB RID: 6059
	private float AttackIntroAnim = 1f;

	// Token: 0x040017AC RID: 6060
	private float AttackHoldAnim = 1.5f;

	// Token: 0x040017AD RID: 6061
	private float ExitIntroAnim = 1f;

	// Token: 0x040017AE RID: 6062
	private float ExitHoldAnim = 1f;

	// Token: 0x040017AF RID: 6063
	private float AttackHoldDelay = 0.1f;
}
