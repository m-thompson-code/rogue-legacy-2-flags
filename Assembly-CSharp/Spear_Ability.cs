using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020001A7 RID: 423
public class Spear_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x1700094A RID: 2378
	// (get) Token: 0x060010AB RID: 4267 RVA: 0x000304AC File Offset: 0x0002E6AC
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x1700094B RID: 2379
	// (get) Token: 0x060010AC RID: 4268 RVA: 0x000304B3 File Offset: 0x0002E6B3
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700094C RID: 2380
	// (get) Token: 0x060010AD RID: 4269 RVA: 0x000304BA File Offset: 0x0002E6BA
	protected override float TellAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700094D RID: 2381
	// (get) Token: 0x060010AE RID: 4270 RVA: 0x000304C1 File Offset: 0x0002E6C1
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700094E RID: 2382
	// (get) Token: 0x060010AF RID: 4271 RVA: 0x000304C8 File Offset: 0x0002E6C8
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700094F RID: 2383
	// (get) Token: 0x060010B0 RID: 4272 RVA: 0x000304CF File Offset: 0x0002E6CF
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000950 RID: 2384
	// (get) Token: 0x060010B1 RID: 4273 RVA: 0x000304D6 File Offset: 0x0002E6D6
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000951 RID: 2385
	// (get) Token: 0x060010B2 RID: 4274 RVA: 0x000304DD File Offset: 0x0002E6DD
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.08f;
		}
	}

	// Token: 0x17000952 RID: 2386
	// (get) Token: 0x060010B3 RID: 4275 RVA: 0x000304E4 File Offset: 0x0002E6E4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000953 RID: 2387
	// (get) Token: 0x060010B4 RID: 4276 RVA: 0x000304EB File Offset: 0x0002E6EB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000954 RID: 2388
	// (get) Token: 0x060010B5 RID: 4277 RVA: 0x000304F2 File Offset: 0x0002E6F2
	protected override float AttackBounceHeight
	{
		get
		{
			return 18f;
		}
	}

	// Token: 0x17000955 RID: 2389
	// (get) Token: 0x060010B6 RID: 4278 RVA: 0x000304F9 File Offset: 0x0002E6F9
	protected bool IsGrounded
	{
		get
		{
			return !(this.m_abilityController != null) || this.m_abilityController.PlayerController.IsGrounded;
		}
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x0003051E File Offset: 0x0002E71E
	protected override void Awake()
	{
		this.m_waitFixedUpdateYield = new WaitForFixedUpdate();
		base.Awake();
	}

	// Token: 0x060010B8 RID: 4280 RVA: 0x00030534 File Offset: 0x0002E734
	public override void PreCastAbility()
	{
		float num = Rewired_RL.Player.GetAxis("MoveHorizontal");
		float y = Rewired_RL.Player.GetAxis("MoveVertical");
		if (!this.m_allowOmniAttack)
		{
			y = 0f;
		}
		if (!PlayerManager.GetPlayerController().IsFacingRight && num == 0f)
		{
			num = -1f;
		}
		if (num == 1f)
		{
			num = 0.99f;
		}
		if (num == -1f)
		{
			num = -0.99f;
		}
		this.m_spearAttackAngle = CDGHelper.GetLockedAngle(CDGHelper.VectorToAngle(new Vector2(num, y)), 4);
		PlayerInputDirection inputDirection = PlayerInputDirection_RL.GetInputDirection(this.m_spearAttackAngle);
		this.m_spearAttackOffset = Vector2.zero;
		Spear_Ability.DirectionalAttackAnimLayer attackLayer = this.GetAttackLayer(inputDirection, out this.m_spearAttackOffset);
		this.m_abilityController.Animator.SetFloat("Attack_Direction", (float)attackLayer);
		base.PreCastAbility();
	}

	// Token: 0x060010B9 RID: 4281 RVA: 0x000305FF File Offset: 0x0002E7FF
	protected override IEnumerator ChangeAnim(float duration)
	{
		yield return base.ChangeAnim(duration);
		yield break;
	}

	// Token: 0x060010BA RID: 4282 RVA: 0x00030618 File Offset: 0x0002E818
	protected override void FireProjectile()
	{
		Vector2 b = this.m_abilityController.PlayerController.Midpoint - this.m_abilityController.PlayerController.transform.position;
		this.m_spearAttackOffset -= b;
		this.m_projectileOffset = this.m_spearAttackOffset;
		base.FireProjectile();
		if (this.m_spearAttackAngle != 180f)
		{
			Vector3 localEulerAngles = this.m_firedProjectile.Pivot.transform.localEulerAngles;
			localEulerAngles.z = this.m_spearAttackAngle;
			this.m_firedProjectile.Pivot.transform.localEulerAngles = localEulerAngles;
		}
		if (this.m_spearAttackAngle == 270f)
		{
			this.m_firedProjectile.OnCollisionRelay.AddListener(this.m_bounce, false);
		}
	}

	// Token: 0x060010BB RID: 4283 RVA: 0x000306E4 File Offset: 0x0002E8E4
	protected virtual IEnumerator GravityYield(float delay)
	{
		if (this.m_gravityYield == null)
		{
			this.m_gravityYield = new WaitRL_Yield(delay, false);
		}
		else
		{
			this.m_gravityYield.CreateNew(delay, false);
		}
		yield return this.m_gravityYield;
		this.m_abilityController.PlayerController.ControllerCorgi.GravityActive(true);
		this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		yield break;
	}

	// Token: 0x060010BC RID: 4284 RVA: 0x000306FA File Offset: 0x0002E8FA
	protected virtual IEnumerator PushForward()
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

	// Token: 0x060010BD RID: 4285 RVA: 0x0003070C File Offset: 0x0002E90C
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

	// Token: 0x060010BE RID: 4286 RVA: 0x00030834 File Offset: 0x0002EA34
	protected Spear_Ability.DirectionalAttackAnimLayer GetAttackLayer(PlayerInputDirection direction, out Vector2 attackOffset)
	{
		switch (direction)
		{
		case PlayerInputDirection.Down:
			attackOffset = this.m_downAttackOffset;
			return Spear_Ability.DirectionalAttackAnimLayer.Down;
		case PlayerInputDirection.UpLeft:
		case PlayerInputDirection.Up:
		case PlayerInputDirection.UpRight:
			attackOffset = this.m_upAttackOffset;
			return Spear_Ability.DirectionalAttackAnimLayer.Up;
		}
		attackOffset = this.m_rightAttackOffset;
		return Spear_Ability.DirectionalAttackAnimLayer.Right;
	}

	// Token: 0x060010BF RID: 4287 RVA: 0x00030894 File Offset: 0x0002EA94
	public override void StopAbility(bool abilityInterrupted)
	{
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.OnCollisionRelay.RemoveListener(this.m_bounce);
			this.m_firedProjectile.FlagForDestruction(null);
		}
		base.StopAllCoroutines();
		this.m_abilityController.PlayerController.DisableFriction = false;
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x040011DD RID: 4573
	[SerializeField]
	protected bool m_allowOmniAttack;

	// Token: 0x040011DE RID: 4574
	protected Vector2 m_upAttackOffset = new Vector2(0f, 6f);

	// Token: 0x040011DF RID: 4575
	protected Vector2 m_rightAttackOffset = new Vector2(4f, 2.25f);

	// Token: 0x040011E0 RID: 4576
	protected Vector2 m_downAttackOffset = new Vector2(0.25f, -1.75f);

	// Token: 0x040011E1 RID: 4577
	protected Vector2 m_upRightAttackOffset = new Vector2(3.7f, 4.55f);

	// Token: 0x040011E2 RID: 4578
	protected Vector2 m_downRightAttackOffset = new Vector2(3.2f, -2.55f);

	// Token: 0x040011E3 RID: 4579
	private float m_spearAttackAngle;

	// Token: 0x040011E4 RID: 4580
	private Vector2 m_spearAttackOffset;

	// Token: 0x040011E5 RID: 4581
	private WaitForFixedUpdate m_waitFixedUpdateYield;

	// Token: 0x040011E6 RID: 4582
	public const float SpearDashDistance = 0f;

	// Token: 0x040011E7 RID: 4583
	public const float SpearDashSpeed = 0f;

	// Token: 0x040011E8 RID: 4584
	public const bool SpearAirStop = false;

	// Token: 0x040011E9 RID: 4585
	protected WaitRL_Yield m_gravityYield;

	// Token: 0x040011EA RID: 4586
	protected Coroutine m_gravityYieldCoroutine;

	// Token: 0x02000AE1 RID: 2785
	protected enum DirectionalAttackAnimLayer
	{
		// Token: 0x04004A84 RID: 19076
		Up,
		// Token: 0x04004A85 RID: 19077
		UpRight,
		// Token: 0x04004A86 RID: 19078
		Right,
		// Token: 0x04004A87 RID: 19079
		DownRight,
		// Token: 0x04004A88 RID: 19080
		Down
	}
}
