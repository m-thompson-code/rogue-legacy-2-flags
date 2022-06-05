using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x0200030F RID: 783
public class Spear_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x17000C00 RID: 3072
	// (get) Token: 0x060018D4 RID: 6356 RVA: 0x00005319 File Offset: 0x00003519
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x17000C01 RID: 3073
	// (get) Token: 0x060018D5 RID: 6357 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C02 RID: 3074
	// (get) Token: 0x060018D6 RID: 6358 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float TellAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000C03 RID: 3075
	// (get) Token: 0x060018D7 RID: 6359 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C04 RID: 3076
	// (get) Token: 0x060018D8 RID: 6360 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000C05 RID: 3077
	// (get) Token: 0x060018D9 RID: 6361 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C06 RID: 3078
	// (get) Token: 0x060018DA RID: 6362 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000C07 RID: 3079
	// (get) Token: 0x060018DB RID: 6363 RVA: 0x0000B4E9 File Offset: 0x000096E9
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.08f;
		}
	}

	// Token: 0x17000C08 RID: 3080
	// (get) Token: 0x060018DC RID: 6364 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000C09 RID: 3081
	// (get) Token: 0x060018DD RID: 6365 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000C0A RID: 3082
	// (get) Token: 0x060018DE RID: 6366 RVA: 0x000052B0 File Offset: 0x000034B0
	protected override float AttackBounceHeight
	{
		get
		{
			return 18f;
		}
	}

	// Token: 0x17000C0B RID: 3083
	// (get) Token: 0x060018DF RID: 6367 RVA: 0x0000C926 File Offset: 0x0000AB26
	protected bool IsGrounded
	{
		get
		{
			return !(this.m_abilityController != null) || this.m_abilityController.PlayerController.IsGrounded;
		}
	}

	// Token: 0x060018E0 RID: 6368 RVA: 0x0000C94B File Offset: 0x0000AB4B
	protected override void Awake()
	{
		this.m_waitFixedUpdateYield = new WaitForFixedUpdate();
		base.Awake();
	}

	// Token: 0x060018E1 RID: 6369 RVA: 0x0008EAF0 File Offset: 0x0008CCF0
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

	// Token: 0x060018E2 RID: 6370 RVA: 0x0000C95E File Offset: 0x0000AB5E
	protected override IEnumerator ChangeAnim(float duration)
	{
		yield return base.ChangeAnim(duration);
		yield break;
	}

	// Token: 0x060018E3 RID: 6371 RVA: 0x0008EBBC File Offset: 0x0008CDBC
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

	// Token: 0x060018E4 RID: 6372 RVA: 0x0000C974 File Offset: 0x0000AB74
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

	// Token: 0x060018E5 RID: 6373 RVA: 0x0000C98A File Offset: 0x0000AB8A
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

	// Token: 0x060018E6 RID: 6374 RVA: 0x0008E00C File Offset: 0x0008C20C
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

	// Token: 0x060018E7 RID: 6375 RVA: 0x0008EC88 File Offset: 0x0008CE88
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

	// Token: 0x060018E8 RID: 6376 RVA: 0x0008ECE8 File Offset: 0x0008CEE8
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

	// Token: 0x040017CB RID: 6091
	[SerializeField]
	protected bool m_allowOmniAttack;

	// Token: 0x040017CC RID: 6092
	protected Vector2 m_upAttackOffset = new Vector2(0f, 6f);

	// Token: 0x040017CD RID: 6093
	protected Vector2 m_rightAttackOffset = new Vector2(4f, 2.25f);

	// Token: 0x040017CE RID: 6094
	protected Vector2 m_downAttackOffset = new Vector2(0.25f, -1.75f);

	// Token: 0x040017CF RID: 6095
	protected Vector2 m_upRightAttackOffset = new Vector2(3.7f, 4.55f);

	// Token: 0x040017D0 RID: 6096
	protected Vector2 m_downRightAttackOffset = new Vector2(3.2f, -2.55f);

	// Token: 0x040017D1 RID: 6097
	private float m_spearAttackAngle;

	// Token: 0x040017D2 RID: 6098
	private Vector2 m_spearAttackOffset;

	// Token: 0x040017D3 RID: 6099
	private WaitForFixedUpdate m_waitFixedUpdateYield;

	// Token: 0x040017D4 RID: 6100
	public const float SpearDashDistance = 0f;

	// Token: 0x040017D5 RID: 6101
	public const float SpearDashSpeed = 0f;

	// Token: 0x040017D6 RID: 6102
	public const bool SpearAirStop = false;

	// Token: 0x040017D7 RID: 6103
	protected WaitRL_Yield m_gravityYield;

	// Token: 0x040017D8 RID: 6104
	protected Coroutine m_gravityYieldCoroutine;

	// Token: 0x02000310 RID: 784
	protected enum DirectionalAttackAnimLayer
	{
		// Token: 0x040017DA RID: 6106
		Up,
		// Token: 0x040017DB RID: 6107
		UpRight,
		// Token: 0x040017DC RID: 6108
		Right,
		// Token: 0x040017DD RID: 6109
		DownRight,
		// Token: 0x040017DE RID: 6110
		Down
	}
}
