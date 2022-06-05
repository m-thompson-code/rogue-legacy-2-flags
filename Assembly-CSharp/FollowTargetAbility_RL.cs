using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using Rewired;
using UnityEngine;

// Token: 0x02000168 RID: 360
public class FollowTargetAbility_RL : BaseAbility_RL
{
	// Token: 0x170006C5 RID: 1733
	// (get) Token: 0x06000C79 RID: 3193 RVA: 0x000265D4 File Offset: 0x000247D4
	protected virtual AbilityAnimState StateToHoldAttackOn
	{
		get
		{
			return AbilityAnimState.Attack_Intro;
		}
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x000265D7 File Offset: 0x000247D7
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

	// Token: 0x06000C7B RID: 3195 RVA: 0x00026604 File Offset: 0x00024804
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

	// Token: 0x06000C7C RID: 3196 RVA: 0x0002673C File Offset: 0x0002493C
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

	// Token: 0x06000C7D RID: 3197 RVA: 0x00026752 File Offset: 0x00024952
	protected override void FireProjectile()
	{
		this.m_abilityController.PlayerController.FallMultiplierOverride = this.m_storedFallMultiplier;
		this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		base.FireProjectile();
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x00026784 File Offset: 0x00024984
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

	// Token: 0x06000C7F RID: 3199 RVA: 0x000268D4 File Offset: 0x00024AD4
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

	// Token: 0x06000C80 RID: 3200 RVA: 0x00026A50 File Offset: 0x00024C50
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

	// Token: 0x06000C81 RID: 3201 RVA: 0x00026B1C File Offset: 0x00024D1C
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

	// Token: 0x04001081 RID: 4225
	[SerializeField]
	protected float m_gravityReductionModWhenAiming = 1f;

	// Token: 0x04001082 RID: 4226
	[SerializeField]
	protected GameObject m_targetGO;

	// Token: 0x04001083 RID: 4227
	[SerializeField]
	protected bool m_targetAlwaysVisible;

	// Token: 0x04001084 RID: 4228
	protected float m_targetMoveSpeed = 20f;

	// Token: 0x04001085 RID: 4229
	protected float m_targetTurnSpeed = 9999f;

	// Token: 0x04001086 RID: 4230
	protected bool m_isFollowing;

	// Token: 0x04001087 RID: 4231
	protected bool m_isHovering;

	// Token: 0x04001088 RID: 4232
	protected float m_storedFallMultiplier;

	// Token: 0x04001089 RID: 4233
	protected float m_orientation;
}
