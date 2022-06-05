using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x02000317 RID: 791
public class Tonfa_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x17000C31 RID: 3121
	// (get) Token: 0x06001925 RID: 6437 RVA: 0x0000CA3F File Offset: 0x0000AC3F
	// (set) Token: 0x06001926 RID: 6438 RVA: 0x0000CA47 File Offset: 0x0000AC47
	public bool InCombo { get; set; }

	// Token: 0x17000C32 RID: 3122
	// (get) Token: 0x06001927 RID: 6439 RVA: 0x00008A92 File Offset: 0x00006C92
	protected virtual int NumComboAttacks
	{
		get
		{
			return 99;
		}
	}

	// Token: 0x17000C33 RID: 3123
	// (get) Token: 0x06001928 RID: 6440 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x17000C34 RID: 3124
	// (get) Token: 0x06001929 RID: 6441 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C35 RID: 3125
	// (get) Token: 0x0600192A RID: 6442 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float TellAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000C36 RID: 3126
	// (get) Token: 0x0600192B RID: 6443 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C37 RID: 3127
	// (get) Token: 0x0600192C RID: 6444 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000C38 RID: 3128
	// (get) Token: 0x0600192D RID: 6445 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C39 RID: 3129
	// (get) Token: 0x0600192E RID: 6446 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000C3A RID: 3130
	// (get) Token: 0x0600192F RID: 6447 RVA: 0x0000CA50 File Offset: 0x0000AC50
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.145f;
		}
	}

	// Token: 0x17000C3B RID: 3131
	// (get) Token: 0x06001930 RID: 6448 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000C3C RID: 3132
	// (get) Token: 0x06001931 RID: 6449 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C3D RID: 3133
	// (get) Token: 0x06001932 RID: 6450 RVA: 0x0000B839 File Offset: 0x00009A39
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

	// Token: 0x17000C3E RID: 3134
	// (get) Token: 0x06001933 RID: 6451 RVA: 0x0000CA57 File Offset: 0x0000AC57
	public string AbilityComboName
	{
		get
		{
			return this.m_abilityTellIntroName.Replace("1Left", this.m_animComboNumber.ToString());
		}
	}

	// Token: 0x17000C3F RID: 3135
	// (get) Token: 0x06001934 RID: 6452 RVA: 0x0000CA74 File Offset: 0x0000AC74
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

	// Token: 0x06001935 RID: 6453 RVA: 0x0000CA8B File Offset: 0x0000AC8B
	protected override void Awake()
	{
		base.Awake();
		if (this.m_abilityTellIntroName != null)
		{
			this.m_abilityTellIntroRight = this.m_abilityTellIntroName.Replace("Left", "Right");
		}
	}

	// Token: 0x06001936 RID: 6454 RVA: 0x0000CAB6 File Offset: 0x0000ACB6
	public override void PreCastAbility()
	{
		this.m_attackStartLeft = !this.m_attackStartLeft;
		this.m_comboNumber = 0;
		this.m_animComboNumber = 1;
		this.InCombo = false;
		base.PreCastAbility();
	}

	// Token: 0x06001937 RID: 6455 RVA: 0x0000CAE2 File Offset: 0x0000ACE2
	protected override void OnEnterTellLogic()
	{
		this.m_hasAttacked = false;
		this.m_comboNumber++;
		base.OnEnterTellLogic();
	}

	// Token: 0x06001938 RID: 6456 RVA: 0x0000CAFF File Offset: 0x0000ACFF
	protected override void OnEnterAttackLogic()
	{
		base.OnEnterAttackLogic();
		this.m_canAttackAgainCounter = 0f;
	}

	// Token: 0x06001939 RID: 6457 RVA: 0x0008F174 File Offset: 0x0008D374
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

	// Token: 0x0600193A RID: 6458 RVA: 0x0000CB12 File Offset: 0x0000AD12
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

	// Token: 0x0600193B RID: 6459 RVA: 0x0000CB21 File Offset: 0x0000AD21
	private void StopPushForwardCoroutine()
	{
		if (this.m_pushForwardCoroutine != null)
		{
			base.StopCoroutine(this.m_pushForwardCoroutine);
		}
		this.m_abilityController.PlayerController.DisableFriction = false;
	}

	// Token: 0x0600193C RID: 6460 RVA: 0x0008F1E8 File Offset: 0x0008D3E8
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

	// Token: 0x0600193D RID: 6461 RVA: 0x0008F2A4 File Offset: 0x0008D4A4
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

	// Token: 0x0600193E RID: 6462 RVA: 0x0000CB48 File Offset: 0x0000AD48
	protected override void OnExitExitLogic()
	{
		if (this.m_hasAttacked)
		{
			return;
		}
		base.OnExitExitLogic();
	}

	// Token: 0x0600193F RID: 6463 RVA: 0x0000CB59 File Offset: 0x0000AD59
	private void RecastAbility()
	{
		this.m_abilityController.StartAbility(base.CastAbilityType, false, false);
	}

	// Token: 0x06001940 RID: 6464 RVA: 0x0008F38C File Offset: 0x0008D58C
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

	// Token: 0x06001941 RID: 6465 RVA: 0x0008E00C File Offset: 0x0008C20C
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

	// Token: 0x040017F1 RID: 6129
	private int m_comboNumber = 1;

	// Token: 0x040017F2 RID: 6130
	private int m_animComboNumber = 1;

	// Token: 0x040017F3 RID: 6131
	private bool m_attackStartLeft;

	// Token: 0x040017F4 RID: 6132
	private string m_abilityTellIntroRight;

	// Token: 0x040017F5 RID: 6133
	private float m_canAttackAgainCounter;

	// Token: 0x040017F6 RID: 6134
	private Coroutine m_pushForwardCoroutine;

	// Token: 0x040017F7 RID: 6135
	private bool m_hasAttacked;

	// Token: 0x040017F9 RID: 6137
	private const float CanAttackAgainDelay = 0.035f;

	// Token: 0x040017FA RID: 6138
	private const float TonfaDashDistance = 1.5f;

	// Token: 0x040017FB RID: 6139
	private const float TonfaDashDuration = 0.15f;
}
