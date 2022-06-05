using System;
using System.Collections;
using Rewired;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000977 RID: 2423
	[AddComponentMenu("Corgi Engine/Character/Abilities/Character Jump RL")]
	public class CharacterJump_RL : CharacterJump
	{
		// Token: 0x17001B35 RID: 6965
		// (get) Token: 0x06005253 RID: 21075 RVA: 0x00124E21 File Offset: 0x00123021
		public bool IsJumpDash
		{
			get
			{
				return this.m_isJumpDash;
			}
		}

		// Token: 0x17001B36 RID: 6966
		// (get) Token: 0x06005254 RID: 21076 RVA: 0x00124E29 File Offset: 0x00123029
		public override bool JumpAuthorized
		{
			get
			{
				if (this.EvaluateJumpTimeWindow())
				{
					return true;
				}
				if (this.JumpRestrictions == CharacterJump.JumpBehavior.CanJumpAnywhere || this.JumpRestrictions == CharacterJump.JumpBehavior.CanJumpAnywhereAnyNumberOfTimes)
				{
					return true;
				}
				if (this.JumpRestrictions == CharacterJump.JumpBehavior.CanJumpOnGround)
				{
					if (this.IsJumpWithinLeeway())
					{
						return true;
					}
					if (base.NumberOfJumpsLeft > 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06005255 RID: 21077 RVA: 0x00124E67 File Offset: 0x00123067
		public void StartJumpTime()
		{
			this.m_jumpStartTime = Time.time;
		}

		// Token: 0x06005256 RID: 21078 RVA: 0x00124E74 File Offset: 0x00123074
		public void ResetBrakeForce()
		{
			this.m_useBrakeForce = false;
		}

		// Token: 0x06005257 RID: 21079 RVA: 0x00124E7D File Offset: 0x0012307D
		private void Awake()
		{
			this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		}

		// Token: 0x06005258 RID: 21080 RVA: 0x00124E91 File Offset: 0x00123091
		protected override void OnEnable()
		{
			base.OnEnable();
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		}

		// Token: 0x06005259 RID: 21081 RVA: 0x00124EA5 File Offset: 0x001230A5
		protected override void OnDisable()
		{
			base.OnDisable();
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		}

		// Token: 0x0600525A RID: 21082 RVA: 0x00124EBC File Offset: 0x001230BC
		private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs eventArgs)
		{
			RoomViaDoorEventArgs roomViaDoorEventArgs = eventArgs as RoomViaDoorEventArgs;
			if (roomViaDoorEventArgs.ViaDoor != null && roomViaDoorEventArgs.ViaDoor.Side != RoomSide.Bottom)
			{
				return;
			}
			if (this.m_preventBrakingCoroutine != null)
			{
				base.StopCoroutine(this.m_preventBrakingCoroutine);
			}
			this.m_preventBrakingCoroutine = base.StartCoroutine(this.PreventBraking());
			global::PlayerController playerController = PlayerManager.GetPlayerController();
			float num = 4.375f;
			float b = Mathf.Sqrt(2f * Mathf.Abs(this._controller.Parameters.Gravity) * num);
			playerController.SetVelocityY(Mathf.Min(playerController.Velocity.y, b), false);
		}

		// Token: 0x0600525B RID: 21083 RVA: 0x00124F58 File Offset: 0x00123158
		private IEnumerator PreventBraking()
		{
			if (this.m_disableBrakeWaitYield == null)
			{
				this.m_disableBrakeWaitYield = new WaitRL_Yield(Room_EV.DISABLE_BRAKING_FORCE_TIME, false);
			}
			bool disableBrakeForceValueOnStart = this.DisableBrakeForce;
			this.DisableBrakeForce = true;
			yield return this.m_disableBrakeWaitYield;
			this.DisableBrakeForce = disableBrakeForceValueOnStart;
			yield break;
		}

		// Token: 0x0600525C RID: 21084 RVA: 0x00124F68 File Offset: 0x00123168
		protected override void HandleInput()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (this.m_playerController.ConditionState == CharacterStates.CharacterConditions.ControlledMovement)
			{
				return;
			}
			if (SaveManager.ConfigData.EnableQuickDrop && this._movement.CurrentState != CharacterStates.MovementStates.Jumping && !Rewired_RL.Player.GetButton("FreeLook") && this._character.REPlayer.GetNegativeButton("MoveVertical") && this._controller.State.IsGrounded && !this.m_playerController.CastAbility.IsAiming)
			{
				float axis = Rewired_RL.Player.GetAxis("MoveVertical");
				float axis2 = Rewired_RL.Player.GetAxis("MoveHorizontal");
				bool flag = RewiredOnStartupController.CurrentActiveControllerType == ControllerType.Keyboard && axis == -1f;
				Vector2 pt = new Vector2(axis2, axis);
				pt.Normalize();
				float num = CDGHelper.VectorToAngle(pt);
				if (((num > 255f && num < 285f) || flag) && this.JumpDownFromOneWayPlatform())
				{
					return;
				}
			}
			if (this._character.REPlayer.GetButtonDown("Jump"))
			{
				this.JumpStart();
			}
			if (this._jumpButtonPressed && !this._character.REPlayer.GetButton("Jump"))
			{
				this.JumpStop();
			}
		}

		// Token: 0x0600525D RID: 21085 RVA: 0x001250AD File Offset: 0x001232AD
		public bool IsAbilityPermitted()
		{
			return true;
		}

		// Token: 0x0600525E RID: 21086 RVA: 0x001250B0 File Offset: 0x001232B0
		public override void ProcessAbility()
		{
			base.ProcessAbility();
			base.JumpHappenedThisFrame = false;
			if (!this.AbilityPermitted || this._condition.CurrentState == CharacterStates.CharacterConditions.Stunned)
			{
				return;
			}
			if (this._controller.Velocity.y <= 0.1f && this._controller.State.IsGrounded)
			{
				this.ResetDoubleJump();
			}
			if (!this._controller.State.IsGrounded && !this._prevIsFalling && !this.EvaluateJumpTimeWindow())
			{
				this._prevIsFalling = true;
				if (base.NumberOfJumpsLeft >= this.NumberOfJumps)
				{
					base.NumberOfJumpsLeft--;
				}
			}
			if (this._controller.State.IsGrounded)
			{
				this._lastTimeGrounded = Time.time;
				return;
			}
			if (this.DisableBrakeForce)
			{
				return;
			}
			if (this._controller.Velocity.y <= 0f)
			{
				this.m_useBrakeForce = false;
			}
			if (this.m_useBrakeForce && this._jumpButtonReleased && this._prevJumpButtonReleased != this._jumpButtonReleased && this._controller.Velocity.y > 0f)
			{
				this._brakeForce = -this.JumpReleaseForce;
			}
			else
			{
				this._brakeForce = 0f;
			}
			if (this._controller.Velocity.y > 0f)
			{
				if (this._brakeForce != 0f)
				{
					this._controller.AddVerticalForce(this._brakeForce * 40f * Time.deltaTime);
					return;
				}
			}
			else if (this._brakeForce != 0f)
			{
				this._brakeForce = 0f;
			}
		}

		// Token: 0x0600525F RID: 21087 RVA: 0x00125244 File Offset: 0x00123444
		private bool OtherAbilitiesCanBeInterruptedByJump()
		{
			if (this.m_playerController && this.m_playerController.IsInitialized)
			{
				if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Weapon) && !this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false).JumpInterruptable)
				{
					return false;
				}
				if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Spell) && !this.m_playerController.CastAbility.GetAbility(CastAbilityType.Spell, false).JumpInterruptable)
				{
					return false;
				}
				if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Talent) && !this.m_playerController.CastAbility.GetAbility(CastAbilityType.Talent, false).JumpInterruptable)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005260 RID: 21088 RVA: 0x001252FC File Offset: 0x001234FC
		protected override bool EvaluateJumpConditions()
		{
			if (this.m_playerController.IsGrounded && this._condition.CurrentState == CharacterStates.CharacterConditions.Normal && !this.m_playerController.CharacterDownStrike.IsHoldingDownStrikeAngle)
			{
				this.m_queueJump = true;
			}
			if (!this.m_playerController.IsGrounded && this.m_jumpStartTime + 0.1f > Time.time)
			{
				return false;
			}
			if (this._condition.CurrentState == CharacterStates.CharacterConditions.Stunned && (this.m_playerController.CharacterHitResponse as PlayerHitResponse).CanRecoverFromStun && !this.m_playerController.IsGrounded)
			{
				return true;
			}
			if (!this._controller.State.IsGrounded && this.m_playerController.CharacterDownStrike.IsHoldingDownStrikeAngle && !SaveManager.ConfigData.DisablePressDownSpinKick)
			{
				return false;
			}
			if (SaveManager.PlayerSaveData.GetRelic(RelicType.FlightBonusCurse).Level > 0 && !this._controller.State.IsGrounded)
			{
				this.m_queueJump = false;
				if (!this.OtherAbilitiesCanBeInterruptedByJump())
				{
					return false;
				}
				if (!this.m_playerController.CharacterFlight.IsFlying && !GlobalTeleporterController.IsTeleporting)
				{
					this.m_playerController.CharacterFlight.StartFlight(2.1474836E+09f, 0.1f);
				}
				else
				{
					this.m_playerController.CharacterFlight.StopFlight();
				}
				return false;
			}
			else
			{
				if (this.m_playerController.CastAbility.TalentAbilityType == AbilityType.CometTalent && this.m_playerController.CastAbility.GetAbility(CastAbilityType.Talent, false).AbilityActive)
				{
					return true;
				}
				CharacterDash_RL characterDash = this.m_playerController.CharacterDash;
				if (!this.AbilityPermitted || !this.JumpAuthorized || (this._condition.CurrentState != CharacterStates.CharacterConditions.Normal && this._condition.CurrentState != CharacterStates.CharacterConditions.DisableHorizontalMovement) || (this._movement.CurrentState == CharacterStates.MovementStates.Dashing && !this.CanJumpWhileDashing && !characterDash.IsVoidDashing) || this._movement.CurrentState == CharacterStates.MovementStates.DownStriking || (this._controller.State.IsCollidingAbove && (!this._character.REPlayer.GetNegativeButton("MoveVertical") || !this._controller.State.IsGrounded)))
				{
					return false;
				}
				if (!this._controller.State.IsGrounded && !this.IsJumpWithinLeeway() && !this.EvaluateJumpTimeWindow() && this.JumpRestrictions != CharacterJump.JumpBehavior.CanJumpAnywhereAnyNumberOfTimes && base.NumberOfJumpsLeft <= 0)
				{
					return false;
				}
				if (ReInput.isReady && this._character.REPlayer.GetNegativeButton("MoveVertical") && this._controller.State.IsGrounded)
				{
					bool flag = this.m_playerController.CastAbility.IsAiming;
					if (flag && this.m_playerController.CastAbility.WeaponAbilityType == AbilityType.CannonWeapon && this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false).IsAiming)
					{
						flag = false;
					}
					if (!flag && this.JumpDownFromOneWayPlatform())
					{
						return false;
					}
				}
				return this.OtherAbilitiesCanBeInterruptedByJump();
			}
		}

		// Token: 0x06005261 RID: 21089 RVA: 0x001255E4 File Offset: 0x001237E4
		public bool IsJumpWithinLeeway()
		{
			return this._controller.State.IsGrounded || (this._controller.Velocity.y <= 0f && this._controller.IsWithinJumpLeeway);
		}

		// Token: 0x06005262 RID: 21090 RVA: 0x00125623 File Offset: 0x00123823
		private IEnumerator QueueJumpCoroutine()
		{
			float startTime = Time.time;
			while (Time.time < startTime + 0.2f)
			{
				if (this.EvaluateJumpConditions())
				{
					this.JumpStart();
					if (!this._character.REPlayer.GetButton("Jump"))
					{
						this.JumpStop();
					}
					this.m_queueJumpCoroutine = null;
					yield break;
				}
				yield return null;
			}
			this.m_queueJumpCoroutine = null;
			yield break;
		}

		// Token: 0x06005263 RID: 21091 RVA: 0x00125634 File Offset: 0x00123834
		public override void JumpStart()
		{
			this.m_queueJump = false;
			if (!this.EvaluateJumpConditions())
			{
				if (this.AbilityPermitted && this.m_queueJump && this.m_queueJumpCoroutine == null)
				{
					this.m_queueJumpCoroutine = base.StartCoroutine(this.QueueJumpCoroutine());
				}
				return;
			}
			if (this.m_queueJumpCoroutine != null)
			{
				base.StopCoroutine(this.m_queueJumpCoroutine);
				this.m_queueJumpCoroutine = null;
			}
			bool flag = this._condition.CurrentState == CharacterStates.CharacterConditions.Stunned && (this.m_playerController.CharacterHitResponse as PlayerHitResponse).CanRecoverFromStun && !this.m_playerController.IsGrounded;
			this._controller.ResetColliderSize();
			this.m_isJumpDash = false;
			if ((this.CanJumpWhileDashing || this.m_playerController.CharacterDash.IsVoidDashing) && this._movement.CurrentState == CharacterStates.MovementStates.Dashing)
			{
				this.m_playerController.CharacterDash.StopDash();
			}
			this.m_playerController.CharacterFlight.StopFlight();
			this._controller.State.JustStartedJump = true;
			this.m_jumpStartTime = Time.time;
			this._movement.ChangeState(CharacterStates.MovementStates.Jumping);
			if (this.IsJumpWithinLeeway() && !flag)
			{
				this.ResetDoubleJump();
				if (TraitManager.IsTraitActive(TraitType.HighJump))
				{
					base.StartCoroutine(this.HighJumpEffectCoroutine());
				}
			}
			bool flag2 = this.m_playerController.CastAbility.TalentAbilityType == AbilityType.CometTalent && this.m_playerController.CastAbility.GetAbility(CastAbilityType.Talent, false).AbilityActive;
			if (base.NumberOfJumpsLeft != this.NumberOfJumps || flag || flag2)
			{
				this._doubleJumping = true;
			}
			if (!this._doubleJumping && !flag)
			{
				this.m_playerController.CharacterDash.ResetNumberOfDashes();
			}
			if (!flag)
			{
				base.NumberOfJumpsLeft--;
				if (base.NumberOfJumpsLeft < 0)
				{
					base.NumberOfJumpsLeft = 0;
				}
			}
			this._condition.ChangeState(CharacterStates.CharacterConditions.Normal);
			this._controller.GravityActive(true);
			this._controller.CollisionsOn();
			this.SetJumpFlags();
			this.m_useBrakeForce = true;
			float num = this.JumpHeight * this.JumpHeightMultiplier;
			if (base.NumberOfJumpsLeft < this.NumberOfJumps - 1)
			{
				num = this.DoubleJumpHeight * this.JumpHeightMultiplier;
			}
			if (flag)
			{
				num /= 2f;
				this._controller.SetHorizontalForce(0f);
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
				if (SaveManager.PlayerSaveData.GetRelic(RelicType.PlatformOnAerial).Level > 0)
				{
					ProjectileManager.FireProjectile(base.gameObject, "CreatePlatformTalentProjectile", new Vector2(0f, -0.25f), true, 0f, 1f, false, true, true, true);
				}
			}
			float verticalForce = Mathf.Sqrt(2f * Mathf.Abs(this._controller.Parameters.Gravity) * num);
			this._controller.SetVerticalForce(verticalForce);
			base.JumpHappenedThisFrame = true;
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerJump, this, null);
			if (this._doubleJumping)
			{
				this._animator.SetTrigger("AirJump");
				Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerDoubleJump, this, null);
			}
		}

		// Token: 0x06005264 RID: 21092 RVA: 0x001259A1 File Offset: 0x00123BA1
		private IEnumerator HighJumpEffectCoroutine()
		{
			this.m_playerController.Animator.SetBool("Trait_HighJump", true);
			yield return null;
			yield return null;
			this.m_playerController.Animator.SetBool("Trait_HighJump", false);
			yield break;
		}

		// Token: 0x06005265 RID: 21093 RVA: 0x001259B0 File Offset: 0x00123BB0
		private IEnumerator DebugJump(float jumpHeight)
		{
			this._playerYPositionAtStart_DEBUG = base.transform.position.y;
			this._heightAtPeak_DEBUG = 0f;
			this._peakYPosition_DEBUG = this._playerYPositionAtStart_DEBUG + jumpHeight;
			while (Mathf.Abs(this._controller.Velocity.y) > 0.01f)
			{
				if (this._controller.Velocity.y <= 0f && this._heightAtPeak_DEBUG == 0f)
				{
					this._heightAtPeak_DEBUG = base.transform.position.y - this._playerYPositionAtStart_DEBUG;
				}
				yield return null;
			}
			this._debugJumpCoroutine = null;
			yield break;
		}

		// Token: 0x06005266 RID: 21094 RVA: 0x001259C6 File Offset: 0x00123BC6
		private void ResetDoubleJump()
		{
			base.NumberOfJumpsLeft = this.NumberOfJumps;
			this._prevIsFalling = false;
			this._doubleJumping = false;
			this._animator.ResetTrigger("AirJump");
		}

		// Token: 0x06005267 RID: 21095 RVA: 0x001259F2 File Offset: 0x00123BF2
		public override void UpdateAnimator()
		{
			this._animator.SetBool("IsDoubleJumping", this._doubleJumping);
			base.UpdateAnimator();
		}

		// Token: 0x06005268 RID: 21096 RVA: 0x00125A10 File Offset: 0x00123C10
		private void OnDrawGizmos()
		{
		}

		// Token: 0x06005269 RID: 21097 RVA: 0x00125A12 File Offset: 0x00123C12
		public override void SetJumpFlags()
		{
			this._jumpButtonPressTime = Time.time;
			this._jumpButtonPressed = true;
			this._jumpButtonReleased = false;
			this._prevJumpButtonReleased = this._jumpButtonReleased;
			this._brakeForce = 0f;
			this.DisableBrakeForce = false;
			this.m_useBrakeForce = false;
		}

		// Token: 0x0600526A RID: 21098 RVA: 0x00125A52 File Offset: 0x00123C52
		public override void ResetJumpButtonReleased()
		{
			this._jumpButtonReleased = false;
			this._prevJumpButtonReleased = this._jumpButtonReleased;
		}

		// Token: 0x04004460 RID: 17504
		[Tooltip("Deceleration Force applied if Jump Button is released when Character's Jump Height is above the Minimum Jump Height")]
		public float JumpReleaseForce = 2f;

		// Token: 0x04004461 RID: 17505
		[Tooltip("Deceleration Force applied if Jump Button is released when Character's Jump Height is below the Minimum Jump Height")]
		public float MinimumJumpReleaseForce = 2f;

		// Token: 0x04004462 RID: 17506
		public float FallGravityMultiplier = -1f;

		// Token: 0x04004463 RID: 17507
		public float MinimumJumpHeight = 1f;

		// Token: 0x04004464 RID: 17508
		public float DoubleJumpHeight = 3f;

		// Token: 0x04004465 RID: 17509
		public float JumpHeightMultiplier = 1f;

		// Token: 0x04004466 RID: 17510
		public bool DisableBrakeForce;

		// Token: 0x04004467 RID: 17511
		[Space(10f)]
		public bool CanJumpWhileDashing = true;

		// Token: 0x04004468 RID: 17512
		private bool _prevJumpButtonReleased;

		// Token: 0x04004469 RID: 17513
		private bool _prevIsFalling;

		// Token: 0x0400446A RID: 17514
		private bool m_isJumpDash;

		// Token: 0x0400446B RID: 17515
		private bool m_useBrakeForce;

		// Token: 0x0400446C RID: 17516
		private float m_jumpStartTime;

		// Token: 0x0400446D RID: 17517
		private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

		// Token: 0x0400446E RID: 17518
		private Coroutine m_preventBrakingCoroutine;

		// Token: 0x0400446F RID: 17519
		private WaitRL_Yield m_disableBrakeWaitYield;

		// Token: 0x04004470 RID: 17520
		private bool m_queueJump;

		// Token: 0x04004471 RID: 17521
		private Coroutine m_queueJumpCoroutine;

		// Token: 0x04004472 RID: 17522
		private Coroutine _debugJumpCoroutine;

		// Token: 0x04004473 RID: 17523
		private float _playerYPositionAtStart_DEBUG;

		// Token: 0x04004474 RID: 17524
		private float _heightAtPeak_DEBUG;

		// Token: 0x04004475 RID: 17525
		private float _peakYPosition_DEBUG;

		// Token: 0x04004476 RID: 17526
		private float _brakeForce;
	}
}
