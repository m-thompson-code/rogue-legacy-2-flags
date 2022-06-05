using System;
using System.Collections;
using MoreMountains.Tools;
using Rewired;
using Sigtrap.Relays;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000973 RID: 2419
	[AddComponentMenu("Corgi Engine/Character/Abilities/Character Dash RL")]
	public class CharacterDash_RL : CharacterDash, IHasProjectileNameArray
	{
		// Token: 0x17001B24 RID: 6948
		// (get) Token: 0x06005212 RID: 21010 RVA: 0x00122EB6 File Offset: 0x001210B6
		public string[] ProjectileNameArray
		{
			get
			{
				if (this.m_projectileNameArray == null)
				{
					this.m_projectileNameArray = new string[]
					{
						"RelicDashStrikeExplosionProjectile",
						"RelicProjectileDashStartProjectile"
					};
				}
				return this.m_projectileNameArray;
			}
		}

		// Token: 0x17001B25 RID: 6949
		// (get) Token: 0x06005213 RID: 21011 RVA: 0x00122EE2 File Offset: 0x001210E2
		public IRelayLink DashRelay
		{
			get
			{
				return this.m_dashRelay.link;
			}
		}

		// Token: 0x17001B26 RID: 6950
		// (get) Token: 0x06005214 RID: 21012 RVA: 0x00122EEF File Offset: 0x001210EF
		public IRelayLink DashCompleteRelay
		{
			get
			{
				return this.m_dashCompleteRelay.link;
			}
		}

		// Token: 0x17001B27 RID: 6951
		// (get) Token: 0x06005215 RID: 21013 RVA: 0x00122EFC File Offset: 0x001210FC
		public bool IsVoidDashing
		{
			get
			{
				return this.m_dashInProgress && this._distanceTraveled > this.DashDistance;
			}
		}

		// Token: 0x17001B28 RID: 6952
		// (get) Token: 0x06005216 RID: 21014 RVA: 0x00122F16 File Offset: 0x00121116
		public bool IsDashing
		{
			get
			{
				return this.m_dashInProgress;
			}
		}

		// Token: 0x17001B29 RID: 6953
		// (get) Token: 0x06005217 RID: 21015 RVA: 0x00122F1E File Offset: 0x0012111E
		// (set) Token: 0x06005218 RID: 21016 RVA: 0x00122F3B File Offset: 0x0012113B
		public override float DashDistance
		{
			get
			{
				if (this.EnableOmnidash)
				{
					return this.m_dashDistance * 1f;
				}
				return this.m_dashDistance;
			}
			set
			{
				this.m_dashDistance = value;
			}
		}

		// Token: 0x17001B2A RID: 6954
		// (get) Token: 0x06005219 RID: 21017 RVA: 0x00122F44 File Offset: 0x00121144
		// (set) Token: 0x0600521A RID: 21018 RVA: 0x00122F4C File Offset: 0x0012114C
		public int TotalDashesAllowed
		{
			get
			{
				return this.m_totalDashesAllowed;
			}
			set
			{
				this.m_totalDashesAllowed = value;
			}
		}

		// Token: 0x17001B2B RID: 6955
		// (get) Token: 0x0600521B RID: 21019 RVA: 0x00122F55 File Offset: 0x00121155
		public int NumDashesAvailable
		{
			get
			{
				return this.m_numDashesAvailable;
			}
		}

		// Token: 0x0600521C RID: 21020 RVA: 0x00122F60 File Offset: 0x00121160
		protected override void HandleInput()
		{
			if (this.m_playerController.ConditionState == CharacterStates.CharacterConditions.ControlledMovement)
			{
				return;
			}
			if (this.IsDashing)
			{
				return;
			}
			if (this._character.REPlayer.GetButtonDown("DashLeft") || this._character.REPlayer.GetButtonDown("DashRight"))
			{
				this.m_dashRightPressed = this._character.REPlayer.GetButtonDown("DashRight");
				this.StartDash();
			}
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x00122FD4 File Offset: 0x001211D4
		public override void ProcessAbility()
		{
			if (this._controller.Velocity.y <= 0.1f && this._controller.State.IsGrounded && this._movement.CurrentState != CharacterStates.MovementStates.Dashing)
			{
				this.ResetNumberOfDashes();
			}
			if (this._movement.CurrentState == CharacterStates.MovementStates.Dashing)
			{
				this._controller.GravityActive(false);
				this._controller.SetVerticalForce(this.m_computedDashForce.y);
				this._controller.SetHorizontalForce(this.m_computedDashForce.x);
			}
			if (!this._dashEndedNaturally && this._movement.CurrentState != CharacterStates.MovementStates.Dashing)
			{
				this.StopDash();
			}
		}

		// Token: 0x0600521E RID: 21022 RVA: 0x00123080 File Offset: 0x00121280
		public override void StartDash()
		{
			if (!this.IsAbilityPermitted())
			{
				return;
			}
			this.m_dashRelay.Dispatch();
			this.InitiateDash();
		}

		// Token: 0x0600521F RID: 21023 RVA: 0x0012309C File Offset: 0x0012129C
		public virtual bool IsAbilityPermitted()
		{
			bool flag = this.m_playerController.MovementState == CharacterStates.MovementStates.DownStriking && false;
			if (!this.AbilityPermitted || (this._condition.CurrentState != CharacterStates.CharacterConditions.Normal && this._condition.CurrentState != CharacterStates.CharacterConditions.DisableHorizontalMovement && this._condition.CurrentState != CharacterStates.CharacterConditions.Stunned) || flag || this.m_dashInProgress)
			{
				return false;
			}
			this.m_isRecoveryDash = (this._condition.CurrentState == CharacterStates.CharacterConditions.Stunned && (this.m_playerController.CharacterHitResponse as PlayerHitResponse).CanRecoverFromStun && !this.m_playerController.IsGrounded && SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockAirDash) > 0);
			if (this._condition.CurrentState == CharacterStates.CharacterConditions.Stunned && !this.m_isRecoveryDash)
			{
				return false;
			}
			if (this.m_numDashesAvailable <= 0 && !this.m_playerController.IsGrounded && !this.m_isRecoveryDash)
			{
				return false;
			}
			if (this._cooldownTimeStamp > Time.time)
			{
				return false;
			}
			if (this.m_playerController && this.m_playerController.IsInitialized)
			{
				if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Weapon) && !this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false).DashInterruptable)
				{
					return false;
				}
				if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Spell) && !this.m_playerController.CastAbility.GetAbility(CastAbilityType.Spell, false).DashInterruptable)
				{
					return false;
				}
				if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Talent) && !this.m_playerController.CastAbility.GetAbility(CastAbilityType.Talent, false).DashInterruptable)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005220 RID: 21024 RVA: 0x00123238 File Offset: 0x00121438
		public override void InitiateDash()
		{
			if (this.m_playerController && this.m_playerController.IsInitialized)
			{
				if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Weapon) && !this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false).CanCastWhileDashing)
				{
					this.m_playerController.CastAbility.StopAbility(CastAbilityType.Weapon, true);
				}
				if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Spell) && !this.m_playerController.CastAbility.GetAbility(CastAbilityType.Spell, false).CanCastWhileDashing)
				{
					this.m_playerController.CastAbility.StopAbility(CastAbilityType.Spell, true);
				}
				if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Talent) && !this.m_playerController.CastAbility.GetAbility(CastAbilityType.Talent, false).CanCastWhileDashing)
				{
					this.m_playerController.CastAbility.StopAbility(CastAbilityType.Talent, true);
				}
				int level = SaveManager.PlayerSaveData.GetRelic(RelicType.DashStrikeDamageUp).Level;
				if (level > 0)
				{
					Vector2 offset = this.m_playerController.Midpoint - this.m_playerController.transform.localPosition;
					Projectile_RL projectile_RL = ProjectileManager.FireProjectile(this.m_playerController.gameObject, "RelicDashStrikeExplosionProjectile", offset, true, 0f, 1f, false, true, true, true);
					projectile_RL.CastAbilityType = CastAbilityType.Talent;
					projectile_RL.LifespanTimer += 0.2f * (float)(level - 1);
				}
				this.m_playerController.Animator.SetBool("Bounce", false);
				this.m_playerController.Animator.SetBool("DanceBounce", false);
				this.m_playerController.CharacterFlight.StopFlight();
				BaseAbility_RL ability = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false);
				if (ability && ability.CanCastWhileDashing && ability.CritsWhenDashing)
				{
					this.m_playerController.LookController.SetCritBlinkEffectEnabled(true, CritBlinkEffectTriggerType.DashAttack);
				}
			}
			this._movement.ChangeState(CharacterStates.MovementStates.Dashing);
			RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.ProjectileDashStart);
			if (relic.Level > 0)
			{
				if (relic.IntValue >= 0)
				{
					Vector2 offset2 = new Vector2(0f, 1f);
					Projectile_RL projectile_RL2 = ProjectileManager.FireProjectile(base.gameObject, "RelicProjectileDashStartProjectile", offset2, true, 0f, 1f, false, true, true, true);
					projectile_RL2.CastAbilityType = CastAbilityType.Talent;
					projectile_RL2.LifespanTimer += 1f * (float)(relic.Level - 1);
					relic.SetIntValue(0, false, true);
				}
				else
				{
					relic.SetIntValue(1, true, true);
				}
			}
			if (this.m_isRecoveryDash)
			{
				this.m_playerController.CharacterHitResponse.StopCharacterStunned();
				this.m_playerController.GetComponent<BlinkPulseEffect>().StartSingleBlinkEffect(Color.white);
				EffectManager.SetEffectParams("SlowTime_Effect", new object[]
				{
					"m_timeScaleValue",
					0.1f,
					"m_slowDuration",
					0.1f
				});
				EffectManager.PlayEffect(base.gameObject, this._animator, "SlowTime_Effect", Vector3.zero, 0.2f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			}
			if (TraitManager.IsTraitActive(TraitType.InvulnDash))
			{
				this.m_playerController.HitboxController.ChangeCollisionType(HitboxType.Body, CollisionType.None);
			}
			else
			{
				this.m_playerController.HitboxController.ChangeCollisionType(HitboxType.Body, CollisionType.Player_Dodging);
			}
			this._cooldownTimeStamp = Time.time + this.DashCooldown + this.DashDistance / this.DashForce;
			this._dashCoroutine = this.Dash();
			base.StartCoroutine(this._dashCoroutine);
		}

		// Token: 0x06005221 RID: 21025 RVA: 0x001235AB File Offset: 0x001217AB
		protected override IEnumerator Dash()
		{
			bool isVoidDash = SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockVoidDash) > 0;
			if (isVoidDash)
			{
				this._animator.SetBool("VoidDash", true);
			}
			this._animator.SetTrigger("Dashing_Effect");
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerDash, this, null);
			this.m_numDashesAvailable--;
			this.m_numDashesAvailable = Mathf.Clamp(this.m_numDashesAvailable, 0, 9999);
			this.m_dashInProgress = true;
			this._startTime = Time.time;
			this._dashEndedNaturally = false;
			this._initialPosition = base.transform.position;
			this._distanceTraveled = 0f;
			this._shouldKeepDashing = true;
			this._dashDirection = (float)Mathf.RoundToInt(this._horizontalInput);
			if (this._dashDirection == 0f)
			{
				this._dashDirection = (this._character.IsFacingRight ? 1f : -1f);
			}
			if (!this.EnableOmnidash && SaveManager.ConfigData.EnableDualButtonDash)
			{
				if (this.m_dashRightPressed)
				{
					this._dashDirection = 1f;
				}
				else
				{
					this._dashDirection = -1f;
				}
			}
			this._computedDashForce = this.DashForce * this._dashDirection;
			float initialDashDuration = this.DashDistance / this.DashForce;
			float dashStartTime = Time.time;
			float totalDashTime = Time.time + initialDashDuration;
			if (this.EnableOmnidash)
			{
				ControllerType lastActiveControllerType = ReInput.controllers.GetLastActiveControllerType();
				Vector2 vector2;
				if (SaveManager.ConfigData.ToggleMouseAiming && (lastActiveControllerType == ControllerType.Mouse || lastActiveControllerType == ControllerType.Keyboard))
				{
					Vector2 vector = this.m_playerController.Midpoint;
					Vector2 pt = CameraController.GameCamera.ScreenToWorldPoint(ReInput.controllers.Mouse.screenPosition);
					if (TraitManager.IsTraitActive(TraitType.UpsideDown))
					{
						float num = CameraController.GameCamera.transform.position.y - vector.y;
						vector.y += num * 2f;
					}
					vector2 = CDGHelper.VectorBetweenPts(vector, pt);
				}
				else
				{
					vector2 = new Vector2(this._horizontalInput, this._verticalInput);
				}
				vector2.Normalize();
				if (vector2 == Vector2.zero)
				{
					if (this._character.IsFacingRight)
					{
						vector2.x = 1f;
					}
					else
					{
						vector2.x = -1f;
					}
				}
				if ((vector2.x < 0f && this.m_playerController.IsFacingRight) || (vector2.x > 0f && !this.m_playerController.IsFacingRight))
				{
					this.m_playerController.CharacterCorgi.Flip(false, false);
				}
				this.m_computedDashForce.x = vector2.x * this.DashForce;
				this.m_computedDashForce.y = vector2.y * this.DashForce;
			}
			else
			{
				this.m_computedDashForce.x = this._computedDashForce;
				this.m_computedDashForce.y = 0f;
			}
			if (this.m_isRecoveryDash && ((!this._character.IsFacingRight && this._dashDirection > 0f) || (this._character.IsFacingRight && this._dashDirection < 0f)))
			{
				this._character.Flip(false, false);
			}
			float maxDistance = this.DashDistance;
			if (isVoidDash)
			{
				maxDistance *= 2f;
			}
			bool dashButtonReleased = false;
			while (this._distanceTraveled < maxDistance && this._shouldKeepDashing && this._movement.CurrentState == CharacterStates.MovementStates.Dashing && totalDashTime > Time.time)
			{
				this._distanceTraveled = Vector3.Distance(this._initialPosition, base.transform.position);
				this._controller.GravityActive(false);
				this._controller.SetVerticalForce(this.m_computedDashForce.y);
				this._controller.SetHorizontalForce(this.m_computedDashForce.x);
				if (!dashButtonReleased)
				{
					if (this.m_dashRightPressed)
					{
						dashButtonReleased = this._character.REPlayer.GetButtonUp("DashRight");
					}
					else
					{
						dashButtonReleased = this._character.REPlayer.GetButtonUp("DashLeft");
					}
				}
				if (isVoidDash)
				{
					if (!dashButtonReleased && (this._character.REPlayer.GetButton("DashLeft") || this._character.REPlayer.GetButton("DashRight")))
					{
						totalDashTime = dashStartTime + initialDashDuration * 2f;
					}
					else
					{
						totalDashTime = dashStartTime + initialDashDuration;
					}
				}
				yield return null;
			}
			this.StopDash();
			yield break;
		}

		// Token: 0x06005222 RID: 21026 RVA: 0x001235BC File Offset: 0x001217BC
		public override void StopDash()
		{
			if (this._dashCoroutine != null)
			{
				base.StopCoroutine(this._dashCoroutine);
			}
			this._controller.GravityActive(true);
			this._controller.SetForce(Vector2.zero);
			this._dashEndedNaturally = true;
			this.m_dashInProgress = false;
			this._cooldownTimeStamp = 0f;
			if (this._movement.CurrentState == CharacterStates.MovementStates.Dashing)
			{
				this._movement.ChangeState(CharacterStates.MovementStates.Idle);
			}
			this._animator.SetBool("VoidDash", false);
			this._animator.ResetTrigger("Dashing_Effect");
			this.m_playerController.HitboxController.ChangeCollisionType(HitboxType.Body, CollisionType.Player);
			BaseAbility_RL ability = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false);
			if (ability && ability.CritsWhenDashing)
			{
				this.m_playerController.LookController.SetCritBlinkEffectEnabled(false, CritBlinkEffectTriggerType.DashAttack);
			}
			this.m_dashCompleteRelay.Dispatch();
		}

		// Token: 0x06005223 RID: 21027 RVA: 0x0012369F File Offset: 0x0012189F
		public void ResetNumberOfDashes()
		{
			this.m_numDashesAvailable = this.m_totalDashesAllowed;
		}

		// Token: 0x06005224 RID: 21028 RVA: 0x001236B0 File Offset: 0x001218B0
		public override void UpdateAnimator()
		{
			bool abilityInProgress = this.m_playerController.CharacterDownStrike.AbilityInProgress;
			MMAnimator.UpdateAnimatorBool(this._animator, "Dashing", this._movement.CurrentState == CharacterStates.MovementStates.Dashing && !abilityInProgress, this._character._animatorParameters);
		}

		// Token: 0x0400441E RID: 17438
		[NonSerialized]
		private string[] m_projectileNameArray;

		// Token: 0x0400441F RID: 17439
		protected Vector2 m_computedDashForce;

		// Token: 0x04004420 RID: 17440
		public bool EnableOmnidash = true;

		// Token: 0x04004421 RID: 17441
		[SerializeField]
		private int m_totalDashesAllowed = 1;

		// Token: 0x04004422 RID: 17442
		[SerializeField]
		private int m_numDashesAvailable = 1;

		// Token: 0x04004423 RID: 17443
		private bool m_dashInProgress;

		// Token: 0x04004424 RID: 17444
		private bool m_dashRightPressed;

		// Token: 0x04004425 RID: 17445
		private bool m_isRecoveryDash;

		// Token: 0x04004426 RID: 17446
		private Relay m_dashRelay = new Relay();

		// Token: 0x04004427 RID: 17447
		private Relay m_dashCompleteRelay = new Relay();
	}
}
