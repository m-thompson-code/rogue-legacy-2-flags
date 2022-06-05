using System;
using System.Collections;
using Rewired;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000F2F RID: 3887
	[AddComponentMenu("Corgi Engine/Character/Abilities/Character Jump RL")]
	public class CharacterJump_RL : CharacterJump
	{
		// Token: 0x17002488 RID: 9352
		// (get) Token: 0x06007078 RID: 28792 RVA: 0x0003E131 File Offset: 0x0003C331
		public bool IsJumpDash
		{
			get
			{
				return this.m_isJumpDash;
			}
		}

		// Token: 0x17002489 RID: 9353
		// (get) Token: 0x06007079 RID: 28793 RVA: 0x0003E139 File Offset: 0x0003C339
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

		// Token: 0x0600707A RID: 28794 RVA: 0x0003E177 File Offset: 0x0003C377
		public void StartJumpTime()
		{
			this.m_jumpStartTime = Time.time;
		}

		// Token: 0x0600707B RID: 28795 RVA: 0x0003E184 File Offset: 0x0003C384
		public void ResetBrakeForce()
		{
			this.m_useBrakeForce = false;
		}

		// Token: 0x0600707C RID: 28796 RVA: 0x0003E18D File Offset: 0x0003C38D
		private void Awake()
		{
			this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		}

		// Token: 0x0600707D RID: 28797 RVA: 0x0003E1A1 File Offset: 0x0003C3A1
		protected override void OnEnable()
		{
			base.OnEnable();
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		}

		// Token: 0x0600707E RID: 28798 RVA: 0x0003E1B5 File Offset: 0x0003C3B5
		protected override void OnDisable()
		{
			base.OnDisable();
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		}

		// Token: 0x0600707F RID: 28799 RVA: 0x00193274 File Offset: 0x00191474
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

		// Token: 0x06007080 RID: 28800 RVA: 0x0003E1C9 File Offset: 0x0003C3C9
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

		// Token: 0x06007081 RID: 28801 RVA: 0x00193310 File Offset: 0x00191510
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

		// Token: 0x06007082 RID: 28802 RVA: 0x00003DA1 File Offset: 0x00001FA1
		public bool IsAbilityPermitted()
		{
			return true;
		}

		// Token: 0x06007083 RID: 28803 RVA: 0x00193458 File Offset: 0x00191658
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

		// Token: 0x06007084 RID: 28804 RVA: 0x001935EC File Offset: 0x001917EC
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

		// Token: 0x06007085 RID: 28805 RVA: 0x001936A4 File Offset: 0x001918A4
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

		// Token: 0x06007086 RID: 28806 RVA: 0x0003E1D8 File Offset: 0x0003C3D8
		public bool IsJumpWithinLeeway()
		{
			return this._controller.State.IsGrounded || (this._controller.Velocity.y <= 0f && this._controller.IsWithinJumpLeeway);
		}

		// Token: 0x06007087 RID: 28807 RVA: 0x0003E217 File Offset: 0x0003C417
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

		// Token: 0x06007088 RID: 28808 RVA: 0x0019398C File Offset: 0x00191B8C
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

		// Token: 0x06007089 RID: 28809 RVA: 0x0003E226 File Offset: 0x0003C426
		private IEnumerator HighJumpEffectCoroutine()
		{
			this.m_playerController.Animator.SetBool("Trait_HighJump", true);
			yield return null;
			yield return null;
			this.m_playerController.Animator.SetBool("Trait_HighJump", false);
			yield break;
		}

		// Token: 0x0600708A RID: 28810 RVA: 0x0003E235 File Offset: 0x0003C435
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

		// Token: 0x0600708B RID: 28811 RVA: 0x0003E24B File Offset: 0x0003C44B
		private void ResetDoubleJump()
		{
			base.NumberOfJumpsLeft = this.NumberOfJumps;
			this._prevIsFalling = false;
			this._doubleJumping = false;
			this._animator.ResetTrigger("AirJump");
		}

		// Token: 0x0600708C RID: 28812 RVA: 0x0003E277 File Offset: 0x0003C477
		public override void UpdateAnimator()
		{
			this._animator.SetBool("IsDoubleJumping", this._doubleJumping);
			base.UpdateAnimator();
		}

		// Token: 0x0600708D RID: 28813 RVA: 0x00002FCA File Offset: 0x000011CA
		private void OnDrawGizmos()
		{
		}

		// Token: 0x0600708E RID: 28814 RVA: 0x0003E295 File Offset: 0x0003C495
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

		// Token: 0x0600708F RID: 28815 RVA: 0x0003E2D5 File Offset: 0x0003C4D5
		public override void ResetJumpButtonReleased()
		{
			this._jumpButtonReleased = false;
			this._prevJumpButtonReleased = this._jumpButtonReleased;
		}

		// Token: 0x04005AB8 RID: 23224
		[Tooltip("Deceleration Force applied if Jump Button is released when Character's Jump Height is above the Minimum Jump Height")]
		public float JumpReleaseForce = 2f;

		// Token: 0x04005AB9 RID: 23225
		[Tooltip("Deceleration Force applied if Jump Button is released when Character's Jump Height is below the Minimum Jump Height")]
		public float MinimumJumpReleaseForce = 2f;

		// Token: 0x04005ABA RID: 23226
		public float FallGravityMultiplier = -1f;

		// Token: 0x04005ABB RID: 23227
		public float MinimumJumpHeight = 1f;

		// Token: 0x04005ABC RID: 23228
		public float DoubleJumpHeight = 3f;

		// Token: 0x04005ABD RID: 23229
		public float JumpHeightMultiplier = 1f;

		// Token: 0x04005ABE RID: 23230
		public bool DisableBrakeForce;

		// Token: 0x04005ABF RID: 23231
		[Space(10f)]
		public bool CanJumpWhileDashing = true;

		// Token: 0x04005AC0 RID: 23232
		private bool _prevJumpButtonReleased;

		// Token: 0x04005AC1 RID: 23233
		private bool _prevIsFalling;

		// Token: 0x04005AC2 RID: 23234
		private bool m_isJumpDash;

		// Token: 0x04005AC3 RID: 23235
		private bool m_useBrakeForce;

		// Token: 0x04005AC4 RID: 23236
		private float m_jumpStartTime;

		// Token: 0x04005AC5 RID: 23237
		private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

		// Token: 0x04005AC6 RID: 23238
		private Coroutine m_preventBrakingCoroutine;

		// Token: 0x04005AC7 RID: 23239
		private WaitRL_Yield m_disableBrakeWaitYield;

		// Token: 0x04005AC8 RID: 23240
		private bool m_queueJump;

		// Token: 0x04005AC9 RID: 23241
		private Coroutine m_queueJumpCoroutine;

		// Token: 0x04005ACA RID: 23242
		private Coroutine _debugJumpCoroutine;

		// Token: 0x04005ACB RID: 23243
		private float _playerYPositionAtStart_DEBUG;

		// Token: 0x04005ACC RID: 23244
		private float _heightAtPeak_DEBUG;

		// Token: 0x04005ACD RID: 23245
		private float _peakYPosition_DEBUG;

		// Token: 0x04005ACE RID: 23246
		private float _brakeForce;
	}
}
