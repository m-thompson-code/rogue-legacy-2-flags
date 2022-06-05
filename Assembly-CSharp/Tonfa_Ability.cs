using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020001AB RID: 427
public class Tonfa_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x17000975 RID: 2421
	// (get) Token: 0x060010EA RID: 4330 RVA: 0x00030C16 File Offset: 0x0002EE16
	// (set) Token: 0x060010EB RID: 4331 RVA: 0x00030C1E File Offset: 0x0002EE1E
	public bool InCombo { get; set; }

	// Token: 0x17000976 RID: 2422
	// (get) Token: 0x060010EC RID: 4332 RVA: 0x00030C27 File Offset: 0x0002EE27
	protected virtual int NumComboAttacks
	{
		get
		{
			return 99;
		}
	}

	// Token: 0x17000977 RID: 2423
	// (get) Token: 0x060010ED RID: 4333 RVA: 0x00030C2B File Offset: 0x0002EE2B
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x17000978 RID: 2424
	// (get) Token: 0x060010EE RID: 4334 RVA: 0x00030C32 File Offset: 0x0002EE32
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000979 RID: 2425
	// (get) Token: 0x060010EF RID: 4335 RVA: 0x00030C39 File Offset: 0x0002EE39
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700097A RID: 2426
	// (get) Token: 0x060010F0 RID: 4336 RVA: 0x00030C40 File Offset: 0x0002EE40
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700097B RID: 2427
	// (get) Token: 0x060010F1 RID: 4337 RVA: 0x00030C47 File Offset: 0x0002EE47
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700097C RID: 2428
	// (get) Token: 0x060010F2 RID: 4338 RVA: 0x00030C4E File Offset: 0x0002EE4E
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700097D RID: 2429
	// (get) Token: 0x060010F3 RID: 4339 RVA: 0x00030C55 File Offset: 0x0002EE55
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700097E RID: 2430
	// (get) Token: 0x060010F4 RID: 4340 RVA: 0x00030C5C File Offset: 0x0002EE5C
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.145f;
		}
	}

	// Token: 0x1700097F RID: 2431
	// (get) Token: 0x060010F5 RID: 4341 RVA: 0x00030C63 File Offset: 0x0002EE63
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000980 RID: 2432
	// (get) Token: 0x060010F6 RID: 4342 RVA: 0x00030C6A File Offset: 0x0002EE6A
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000981 RID: 2433
	// (get) Token: 0x060010F7 RID: 4343 RVA: 0x00030C71 File Offset: 0x0002EE71
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

	// Token: 0x17000982 RID: 2434
	// (get) Token: 0x060010F8 RID: 4344 RVA: 0x00030C91 File Offset: 0x0002EE91
	public string AbilityComboName
	{
		get
		{
			return this.m_abilityTellIntroName.Replace("1Left", this.m_animComboNumber.ToString());
		}
	}

	// Token: 0x17000983 RID: 2435
	// (get) Token: 0x060010F9 RID: 4345 RVA: 0x00030CAE File Offset: 0x0002EEAE
	public override string AbilityTellIntroName
	{
		get
		{
			if (this.m_attackStartLeft)
			{
				return base.AbilityTellIntroName;
			}
			return this.m_abilityTellIntroRight;
		}
	}

	// Token: 0x060010FA RID: 4346 RVA: 0x00030CC5 File Offset: 0x0002EEC5
	protected override void Awake()
	{
		base.Awake();
		if (this.m_abilityTellIntroName != null)
		{
			this.m_abilityTellIntroRight = this.m_abilityTellIntroName.Replace("Left", "Right");
		}
	}

	// Token: 0x060010FB RID: 4347 RVA: 0x00030CF0 File Offset: 0x0002EEF0
	public override void PreCastAbility()
	{
		this.m_attackStartLeft = !this.m_attackStartLeft;
		this.m_comboNumber = 0;
		this.m_animComboNumber = 1;
		this.InCombo = false;
		base.PreCastAbility();
	}

	// Token: 0x060010FC RID: 4348 RVA: 0x00030D1C File Offset: 0x0002EF1C
	protected override void OnEnterTellLogic()
	{
		this.m_hasAttacked = false;
		this.m_comboNumber++;
		base.OnEnterTellLogic();
	}

	// Token: 0x060010FD RID: 4349 RVA: 0x00030D39 File Offset: 0x0002EF39
	protected override void OnEnterAttackLogic()
	{
		base.OnEnterAttackLogic();
		this.m_canAttackAgainCounter = 0f;
	}

	// Token: 0x060010FE RID: 4350 RVA: 0x00030D4C File Offset: 0x0002EF4C
	protected override void FireProjectile()
	{
		base.FireProjectile();
		if (this.m_firedProjectile != null)
		{
			this.m_firedProjectile.OnCollisionRelay.AddListener(new Action<Projectile_RL, GameObject>(this.ContinueComboEvent), false);
		}
		if (this.m_comboNumber <= 1 && this.m_abilityController.PlayerController.IsGrounded)
		{
			this.StopPushForwardCoroutine();
			this.m_pushForwardCoroutine = base.StartCoroutine(this.PushForwardCoroutine());
		}
	}

	// Token: 0x060010FF RID: 4351 RVA: 0x00030DBE File Offset: 0x0002EFBE
	private IEnumerator PushForwardCoroutine()
	{
		float num = 0.15f;
		float num2 = 1.5f / num;
		float endingTime = Time.time + num;
		if (this.m_abilityController.PlayerController.CharacterCorgi.REPlayer.GetAxis("MoveHorizontal") > 0f && !this.m_abilityController.PlayerController.IsFacingRight)
		{
			this.m_abilityController.PlayerController.CharacterCorgi.Flip(false, false);
		}
		else if (this.m_abilityController.PlayerController.CharacterCorgi.REPlayer.GetAxis("MoveHorizontal") < 0f && this.m_abilityController.PlayerController.IsFacingRight)
		{
			this.m_abilityController.PlayerController.CharacterCorgi.Flip(false, false);
		}
		this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
		if (this.m_abilityController.PlayerController.IsFacingRight)
		{
			this.m_abilityController.PlayerController.SetVelocityX(num2, false);
		}
		else
		{
			this.m_abilityController.PlayerController.SetVelocityX(-num2, false);
		}
		if (this.m_abilityController.PlayerController.IsGrounded)
		{
			this.m_abilityController.PlayerController.MovementState = CharacterStates.MovementStates.Idle;
		}
		while (Time.time < endingTime)
		{
			this.m_abilityController.PlayerController.DisableFriction = true;
			this.PreventPlatformDrop();
			yield return null;
		}
		this.m_abilityController.PlayerController.DisableFriction = false;
		yield break;
	}

	// Token: 0x06001100 RID: 4352 RVA: 0x00030DCD File Offset: 0x0002EFCD
	private void StopPushForwardCoroutine()
	{
		if (this.m_pushForwardCoroutine != null)
		{
			base.StopCoroutine(this.m_pushForwardCoroutine);
		}
		this.m_abilityController.PlayerController.DisableFriction = false;
	}

	// Token: 0x06001101 RID: 4353 RVA: 0x00030DF4 File Offset: 0x0002EFF4
	private void ContinueComboEvent(Projectile_RL projectile, GameObject colliderObj)
	{
		if (projectile != null)
		{
			projectile.OnCollisionRelay.RemoveListener(new Action<Projectile_RL, GameObject>(this.ContinueComboEvent));
		}
		if (this.m_abilityController.PlayerController.ConditionState == CharacterStates.CharacterConditions.Stunned)
		{
			return;
		}
		this.InCombo = true;
		this.StopPushForwardCoroutine();
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.DisableHorizontalMovement)
		{
			this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
			this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
			this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(false);
		}
	}

	// Token: 0x06001102 RID: 4354 RVA: 0x00030EB0 File Offset: 0x0002F0B0
	protected void LateUpdate()
	{
		if (!base.AbilityActive)
		{
			return;
		}
		if (this.m_canAttackAgainCounter >= 0.035f)
		{
			if (this.m_comboNumber < this.NumComboAttacks && base.CurrentAbilityAnimState >= AbilityAnimState.Attack && Rewired_RL.Player.GetButtonDown(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
			{
				if (this.InCombo)
				{
					this.m_animComboNumber++;
					if (this.m_animComboNumber > 5)
					{
						this.m_animComboNumber = 2;
					}
					this.m_hasAttacked = true;
					this.InCombo = false;
					base.CancelChangeAnimCoroutine();
					this.m_animator.Play(this.AbilityComboName);
					return;
				}
				this.m_abilityController.StopAbility(base.CastAbilityType, true);
				this.m_abilityController.StartAbility(base.CastAbilityType, false, false);
				return;
			}
		}
		else
		{
			this.m_canAttackAgainCounter += Time.deltaTime;
		}
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x00030F97 File Offset: 0x0002F197
	protected override void OnExitExitLogic()
	{
		if (this.m_hasAttacked)
		{
			return;
		}
		base.OnExitExitLogic();
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x00030FA8 File Offset: 0x0002F1A8
	private void RecastAbility()
	{
		this.m_abilityController.StartAbility(base.CastAbilityType, false, false);
	}

	// Token: 0x06001105 RID: 4357 RVA: 0x00030FC0 File Offset: 0x0002F1C0
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_firedProjectile != null)
		{
			this.m_firedProjectile.OnCollisionRelay.RemoveListener(new Action<Projectile_RL, GameObject>(this.ContinueComboEvent));
		}
		this.m_animator.ResetTrigger("ContinueCombo");
		this.StopPushForwardCoroutine();
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(true);
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x06001106 RID: 4358 RVA: 0x00031050 File Offset: 0x0002F250
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

	// Token: 0x040011F0 RID: 4592
	private int m_comboNumber = 1;

	// Token: 0x040011F1 RID: 4593
	private int m_animComboNumber = 1;

	// Token: 0x040011F2 RID: 4594
	private bool m_attackStartLeft;

	// Token: 0x040011F3 RID: 4595
	private string m_abilityTellIntroRight;

	// Token: 0x040011F4 RID: 4596
	private float m_canAttackAgainCounter;

	// Token: 0x040011F5 RID: 4597
	private Coroutine m_pushForwardCoroutine;

	// Token: 0x040011F6 RID: 4598
	private bool m_hasAttacked;

	// Token: 0x040011F8 RID: 4600
	private const float CanAttackAgainDelay = 0.035f;

	// Token: 0x040011F9 RID: 4601
	private const float TonfaDashDistance = 1.5f;

	// Token: 0x040011FA RID: 4602
	private const float TonfaDashDuration = 0.15f;
}
