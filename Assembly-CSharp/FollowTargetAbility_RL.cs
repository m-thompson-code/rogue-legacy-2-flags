using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using Rewired;
using UnityEngine;

// Token: 0x0200029D RID: 669
public class FollowTargetAbility_RL : BaseAbility_RL
{
	// Token: 0x1700091D RID: 2333
	// (get) Token: 0x06001386 RID: 4998 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected virtual AbilityAnimState StateToHoldAttackOn
	{
		get
		{
			return AbilityAnimState.Attack_Intro;
		}
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x00009EB6 File Offset: 0x000080B6
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		if (this.m_targetAlwaysVisible)
		{
			this.m_targetGO.SetActive(true);
			return;
		}
		this.m_targetGO.SetActive(false);
	}

	// Token: 0x06001388 RID: 5000 RVA: 0x000857C0 File Offset: 0x000839C0
	public override void PreCastAbility()
	{
		base.PreCastAbility();
		if (!this.m_targetGO.activeSelf)
		{
			this.m_targetGO.SetActive(true);
		}
		this.m_isHovering = false;
		this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
		if (this.m_abilityController.PlayerController.IsFacingRight)
		{
			this.m_orientation = 1f;
		}
		else
		{
			this.m_orientation = -1f;
		}
		this.m_storedFallMultiplier = this.m_abilityController.PlayerController.FallMultiplierOverride;
		Vector2 v = this.m_abilityController.PlayerController.Midpoint + this.m_projectileOffset;
		this.m_targetGO.transform.position = v;
		if (this.m_gravityReductionModWhenAiming < 1f)
		{
			this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
			this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
			this.m_abilityController.PlayerController.FallMultiplierOverride = this.m_gravityReductionModWhenAiming;
			if (!this.m_abilityController.PlayerController.IsGrounded)
			{
				this.m_animator.SetBool("Hover", true);
				this.m_isHovering = true;
			}
		}
	}

	// Token: 0x06001389 RID: 5001 RVA: 0x00009EE1 File Offset: 0x000080E1
	protected override IEnumerator ChangeAnim(float duration)
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
		while (base.CurrentAbilityAnimState == this.StateToHoldAttackOn && Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
		{
			yield return null;
		}
		this.m_animator.SetTrigger("Change_Ability_Anim");
		base.PerformTurnAnimCheck();
		yield break;
	}

	// Token: 0x0600138A RID: 5002 RVA: 0x00009EF7 File Offset: 0x000080F7
	protected override void FireProjectile()
	{
		this.m_abilityController.PlayerController.FallMultiplierOverride = this.m_storedFallMultiplier;
		this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		base.FireProjectile();
	}

	// Token: 0x0600138B RID: 5003 RVA: 0x000858F8 File Offset: 0x00083AF8
	protected override void Update()
	{
		base.Update();
		if (!base.AbilityActive)
		{
			if (!this.m_abilityController.OnPauseResetInput && this.m_abilityController.PlayerController.CurrentMana >= (float)base.ActualCost && (base.CurrentAmmo > 0 || base.MaxAmmo == 0) && base.CooldownTimer <= 0f && this.m_abilityController.PlayerController.CharacterCorgi.REPlayer.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
			{
				this.m_abilityController.StartAbility(base.CastAbilityType, false, false);
			}
			return;
		}
		this.m_isFollowing = false;
		if (base.CurrentAbilityAnimState == this.StateToHoldAttackOn && Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
		{
			this.UpdateFollow();
			this.ConstrainTargetToRoom();
			this.m_isFollowing = true;
		}
		if (this.m_isHovering && this.m_abilityController.PlayerController.IsGrounded)
		{
			this.m_isHovering = false;
			this.m_animator.SetBool("Hover", false);
			return;
		}
		if (!this.m_isHovering && !this.m_abilityController.PlayerController.IsGrounded)
		{
			this.m_isHovering = true;
			this.m_animator.SetBool("Hover", true);
		}
	}

	// Token: 0x0600138C RID: 5004 RVA: 0x00085A48 File Offset: 0x00083C48
	protected virtual void UpdateFollow()
	{
		ControllerType lastActiveControllerType = ReInput.controllers.GetLastActiveControllerType();
		float num = 0f;
		float num2 = 0f;
		if (SaveManager.ConfigData.ToggleMouseAiming && (lastActiveControllerType == ControllerType.Mouse || lastActiveControllerType == ControllerType.Keyboard))
		{
			Vector3 position = this.m_targetGO.transform.position;
			Vector2 vector = CameraController.GameCamera.ScreenToWorldPoint(ReInput.controllers.Mouse.screenPosition);
			if (CDGHelper.DistanceBetweenPts(vector, position) > 0.15f)
			{
				this.m_orientation = CDGHelper.TurnToFaceRadians(position, vector, CDGHelper.ToRadians(this.m_targetTurnSpeed), this.m_orientation, Time.deltaTime, false);
				num = Mathf.Sin(this.m_orientation);
				num2 = Mathf.Cos(this.m_orientation);
			}
			else
			{
				this.m_targetGO.transform.position = vector;
			}
		}
		else if (lastActiveControllerType == ControllerType.Joystick || !SaveManager.ConfigData.ToggleMouseAiming)
		{
			num = Rewired_RL.Player.GetAxis("MoveVertical");
			num2 = Rewired_RL.Player.GetAxis("MoveHorizontal");
		}
		if (num != 0f || num2 != 0f)
		{
			Vector3 position2 = this.m_targetGO.transform.position;
			position2.x += num2 * this.m_targetMoveSpeed * Time.deltaTime;
			position2.y += num * this.m_targetMoveSpeed * Time.deltaTime;
			this.m_targetGO.transform.position = position2;
		}
	}

	// Token: 0x0600138D RID: 5005 RVA: 0x00085BC4 File Offset: 0x00083DC4
	public void ConstrainTargetToRoom()
	{
		BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
		if (!currentPlayerRoom.IsNativeNull())
		{
			Rect boundsRect = currentPlayerRoom.BoundsRect;
			Vector2 vector = this.m_targetGO.transform.position;
			if (vector.x < boundsRect.xMin)
			{
				vector.x = boundsRect.xMin;
			}
			else if (vector.x > boundsRect.xMax)
			{
				vector.x = boundsRect.xMax;
			}
			if (vector.y < boundsRect.yMin)
			{
				vector.y = boundsRect.yMin;
			}
			else if (vector.y > boundsRect.yMax)
			{
				vector.y = boundsRect.yMax;
			}
			this.m_targetGO.transform.position = vector;
		}
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x00085C90 File Offset: 0x00083E90
	public override void StopAbility(bool abilityInterrupted)
	{
		base.StopAbility(abilityInterrupted);
		if (!this.m_targetAlwaysVisible && this.m_targetGO.activeSelf)
		{
			this.m_targetGO.SetActive(false);
		}
		if (this.m_gravityReductionModWhenAiming < 1f)
		{
			this.m_abilityController.PlayerController.FallMultiplierOverride = this.m_storedFallMultiplier;
		}
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		this.m_animator.SetBool("Hover", false);
	}

	// Token: 0x040015B1 RID: 5553
	[SerializeField]
	protected float m_gravityReductionModWhenAiming = 1f;

	// Token: 0x040015B2 RID: 5554
	[SerializeField]
	protected GameObject m_targetGO;

	// Token: 0x040015B3 RID: 5555
	[SerializeField]
	protected bool m_targetAlwaysVisible;

	// Token: 0x040015B4 RID: 5556
	protected float m_targetMoveSpeed = 20f;

	// Token: 0x040015B5 RID: 5557
	protected float m_targetTurnSpeed = 9999f;

	// Token: 0x040015B6 RID: 5558
	protected bool m_isFollowing;

	// Token: 0x040015B7 RID: 5559
	protected bool m_isHovering;

	// Token: 0x040015B8 RID: 5560
	protected float m_storedFallMultiplier;

	// Token: 0x040015B9 RID: 5561
	protected float m_orientation;
}
