using System;
using System.Collections;
using MoreMountains.Tools;
using RLAudio;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000F2B RID: 3883
	[AddComponentMenu("Corgi Engine/Character/Abilities/Character Downstrike RL")]
	public class CharacterDownStrike_RL : CharacterAbility, IHasProjectileNameArray, IAudioEventEmitter
	{
		// Token: 0x06007043 RID: 28739 RVA: 0x0003D390 File Offset: 0x0003B590
		public override string HelpBoxText()
		{
			return "This component allows your character to dash. Here you can define the distance the dash should cover, how much force to apply, and the cooldown between the end of a dash and the start of the next one.";
		}

		// Token: 0x1700247B RID: 9339
		// (get) Token: 0x06007044 RID: 28740 RVA: 0x0003DF2D File Offset: 0x0003C12D
		public bool IsTriggeringBounce
		{
			get
			{
				return this.m_triggerBounce;
			}
		}

		// Token: 0x1700247C RID: 9340
		// (get) Token: 0x06007045 RID: 28741 RVA: 0x0003DF35 File Offset: 0x0003C135
		public bool AbilityInProgress
		{
			get
			{
				return this.m_attackInProgress;
			}
		}

		// Token: 0x1700247D RID: 9341
		// (get) Token: 0x06007046 RID: 28742 RVA: 0x00009A7B File Offset: 0x00007C7B
		public string Description
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x1700247E RID: 9342
		// (get) Token: 0x06007047 RID: 28743 RVA: 0x0019179C File Offset: 0x0018F99C
		public string[] ProjectileNameArray
		{
			get
			{
				if (this.m_projectileNameArray == null)
				{
					this.m_projectileNameArray = new string[]
					{
						this.m_projectileName,
						this.m_spinKickProjectileName,
						this.m_resonantSpinKickProjectileName,
						this.m_resonantDownStrikeProjectileName,
						this.m_relicCaltropDropProjectileName,
						this.m_relicCaltropProjectileName
					};
				}
				return this.m_projectileNameArray;
			}
		}

		// Token: 0x1700247F RID: 9343
		// (get) Token: 0x06007048 RID: 28744 RVA: 0x0003DF3D File Offset: 0x0003C13D
		// (set) Token: 0x06007049 RID: 28745 RVA: 0x0003DF45 File Offset: 0x0003C145
		public OnDownStrikeDelegate OnDownStrikeEvent { get; set; }

		// Token: 0x17002480 RID: 9344
		// (get) Token: 0x0600704A RID: 28746 RVA: 0x0003DF4E File Offset: 0x0003C14E
		public IRelayLink<Projectile_RL, GameObject> OnSuccessfulDownstrikeRelay
		{
			get
			{
				return this.m_onSuccessfulDownstrikeRelay;
			}
		}

		// Token: 0x17002481 RID: 9345
		// (get) Token: 0x0600704B RID: 28747 RVA: 0x001917FC File Offset: 0x0018F9FC
		public bool IsHoldingDownStrikeAngle
		{
			get
			{
				float num = Mathf.Atan2(this._verticalInput, this._horizontalInput) * 57.29578f;
				return num > (float)this.ForwardKickMinMaxAngle.x && num < (float)this.ForwardKickMinMaxAngle.y;
			}
		}

		// Token: 0x0600704C RID: 28748 RVA: 0x00191844 File Offset: 0x0018FA44
		protected override void Initialization()
		{
			base.Initialization();
			this.m_forwardKickRightAngle = CDGHelper.AngleToVector(this.ForwardKickAngle);
			this.m_forwardKickRightAngle.Normalize();
			this.m_forwardKickLeftAngle = CDGHelper.AngleToVector(-180f - this.ForwardKickAngle);
			this.m_forwardKickLeftAngle.Normalize();
			this.m_waitUntilSpinKickAttackYield = new WaitUntil(() => this.EnteredSpinAttackState());
			this.m_resetConsecutiveStrikes = new Action<CorgiController_RL>(this.ResetConsecutiveStrikes);
			this.m_playerController.ControllerCorgi.OnCorgiLandedEnterRelay.AddListener(this.m_resetConsecutiveStrikes, false);
			this.m_bounce = new Action<Projectile_RL, GameObject>(this.Bounce);
		}

		// Token: 0x0600704D RID: 28749 RVA: 0x001918F0 File Offset: 0x0018FAF0
		protected override void HandleInput()
		{
			if (this.m_playerController.ConditionState == CharacterStates.CharacterConditions.ControlledMovement)
			{
				return;
			}
			if (this.AttackButtonTriggersSpinKick && this._character.REPlayer.GetButtonDown("Attack"))
			{
				this.StartDownStrike(false);
				return;
			}
			if (!SaveManager.ConfigData.DisablePressDownSpinKick && this._character.REPlayer.GetButtonDown("Jump"))
			{
				this.StartDownStrike(false);
				return;
			}
			if (this._character.REPlayer.GetButtonDown("Downstrike"))
			{
				this.StartDownStrike(true);
			}
		}

		// Token: 0x0600704E RID: 28750 RVA: 0x0019197C File Offset: 0x0018FB7C
		public virtual void StartDownStrike(bool ignoreDownstrikeAngle)
		{
			bool flag = this._movement.CurrentState == CharacterStates.MovementStates.Dashing && false;
			if (!this.AbilityPermitted || this._controller.State.IsGrounded || this._condition.CurrentState == CharacterStates.CharacterConditions.Stunned || (this._condition.CurrentState != CharacterStates.CharacterConditions.Normal && this._condition.CurrentState != CharacterStates.CharacterConditions.DisableHorizontalMovement) || flag || this.m_attackInProgress || this.m_bounceLockTimer > 0f)
			{
				return;
			}
			if (!this.IsHoldingDownStrikeAngle && !ignoreDownstrikeAngle)
			{
				return;
			}
			if (this.IsHoldingDownStrikeAngle && !ignoreDownstrikeAngle && this._controller.State.JustStartedJump)
			{
				return;
			}
			if (this._cooldownTimeStamp <= Time.time)
			{
				if ((this._movement.CurrentState != CharacterStates.MovementStates.Flying && this._movement.CurrentState != CharacterStates.MovementStates.Dashing) || !this.SpinKickInstead)
				{
					this._movement.ChangeState(CharacterStates.MovementStates.DownStriking);
				}
				this._controller.State.IsCollidingBelow = false;
				this._controller.State.IsFalling = true;
				this._cooldownTimeStamp = Time.time + this.AttackCooldown + this.AttackDuration / this.AttackSpeed;
				float inputAngle = Mathf.Atan2(this._verticalInput, this._horizontalInput) * 57.29578f;
				this.m_inputAngle = inputAngle;
				this._downstrikeCoroutine = this.DownStrike();
				base.StartCoroutine(this._downstrikeCoroutine);
			}
		}

		// Token: 0x0600704F RID: 28751 RVA: 0x0003DF56 File Offset: 0x0003C156
		protected virtual IEnumerator DownStrike()
		{
			if (!this.AbilityPermitted || (this._condition.CurrentState != CharacterStates.CharacterConditions.Normal && this._condition.CurrentState != CharacterStates.CharacterConditions.DisableHorizontalMovement))
			{
				yield break;
			}
			this.m_playerController.CastAbility.StopAllAbilities(false);
			this._startTime = Time.time;
			this._downstrikeEndedNaturally = false;
			this._initialPosition = base.transform.position;
			this._distanceTraveled = 0f;
			this._dashDirection = (this._character.IsFacingRight ? 1f : -1f);
			this.m_totalDashTime = Time.time + this.AttackDuration / this.AttackSpeed;
			this.m_attackInProgress = true;
			if (this.m_inputAngle > (float)this.ForwardKickMinMaxAngle.x && this.m_inputAngle < (float)this.DownKickMinMaxAngle.x && !this.SpinKickInstead)
			{
				this.m_computedDashForce.x = this.m_forwardKickLeftAngle.x * this.AttackSpeed;
				this.m_computedDashForce.y = this.m_forwardKickLeftAngle.y * this.AttackSpeed;
			}
			else if (this.m_inputAngle > (float)this.DownKickMinMaxAngle.y && this.m_inputAngle < (float)this.ForwardKickMinMaxAngle.y && !this.SpinKickInstead)
			{
				this.m_computedDashForce.x = this.m_forwardKickRightAngle.x * this.AttackSpeed;
				this.m_computedDashForce.y = this.m_forwardKickRightAngle.y * this.AttackSpeed;
			}
			else
			{
				this.m_computedDashForce.x = 0f;
				this.m_computedDashForce.y = -1f * this.AttackSpeed;
			}
			if (!this.SpinKickInstead)
			{
				if (SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockBouncableDownstrike) > 0)
				{
					this.m_firedProjectile = (DownstrikeProjectile_RL)ProjectileManager.FireProjectile(base.gameObject, this.m_resonantDownStrikeProjectileName, Vector2.zero, false, 0f, 1f, false, true, true, true);
				}
				else
				{
					this.m_firedProjectile = (DownstrikeProjectile_RL)ProjectileManager.FireProjectile(base.gameObject, this.m_projectileName, Vector2.zero, false, 0f, 1f, false, true, true, true);
				}
				if (TraitManager.IsTraitActive(TraitType.BounceTerrain))
				{
					this.m_firedProjectile.CanHitWall = true;
				}
				else
				{
					this.m_firedProjectile.CanHitWall = false;
				}
			}
			else
			{
				MMAnimator.UpdateAnimatorTrigger(this._animator, "360Kick");
				MMAnimator.UpdateAnimatorFloat(this._animator, "Ability_Anim_Speed", this.m_spinKickTellAnimMultiplier);
				if (SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockBouncableDownstrike) > 0)
				{
					this.m_firedProjectile = (DownstrikeProjectile_RL)ProjectileManager.FireProjectile(base.gameObject, this.m_resonantSpinKickProjectileName, Vector2.zero, false, 0f, 1f, false, true, true, true);
				}
				else
				{
					this.m_firedProjectile = (DownstrikeProjectile_RL)ProjectileManager.FireProjectile(base.gameObject, this.m_spinKickProjectileName, Vector2.zero, false, 0f, 1f, false, true, true, true);
				}
				this.m_firedProjectile.transform.position = this.m_playerController.Midpoint;
				if (TraitManager.IsTraitActive(TraitType.BounceTerrain))
				{
					this.m_firedProjectile.CanHitWall = true;
				}
				else
				{
					this.m_firedProjectile.CanHitWall = false;
				}
			}
			int level = SaveManager.PlayerSaveData.GetRelic(RelicType.SpinKickArmorBreak).Level;
			if (level > 0)
			{
				this.m_firedProjectile.AttachStatusEffect(StatusEffectType.Enemy_ArmorBreak, 3.5f * (float)level);
			}
			this.m_firedProjectile.OnCollisionRelay.AddListener(this.m_bounce, false);
			if (this.OnDownStrikeEvent != null)
			{
				this.OnDownStrikeEvent();
			}
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerDownstrikeCast, this, null);
			if (this.SpinKickInstead)
			{
				yield return this.m_waitUntilSpinKickAttackYield;
				MMAnimator.UpdateAnimatorFloat(this._animator, "Ability_Anim_Speed", this.m_spinKickAttackAnimMultiplier);
			}
			bool isSpinKicking = this.SpinKickInstead;
			while (this._movement.CurrentState == CharacterStates.MovementStates.DownStriking || isSpinKicking)
			{
				this._distanceTraveled = Vector3.Distance(this._initialPosition, base.transform.position);
				if (this._controller.State.IsCollidingBelow)
				{
					this._character.MovementState.ChangeState(CharacterStates.MovementStates.Idle);
					this.StopDownStrike();
					yield break;
				}
				if (!this.SpinKickInstead)
				{
					this._controller.GravityActive(false);
					this._controller.SetVerticalForce(this.m_computedDashForce.y);
					this._controller.SetHorizontalForce(this.m_computedDashForce.x);
				}
				else
				{
					AnimatorStateInfo currentAnimatorStateInfo = this._animator.GetCurrentAnimatorStateInfo(0);
					if (!this._animator.GetBool("360Kick") && !currentAnimatorStateInfo.IsName("SpinKick_Tell") && !currentAnimatorStateInfo.IsName("SpinKick_Attack"))
					{
						isSpinKicking = false;
						this.StopDownStrike();
						yield break;
					}
				}
				yield return null;
			}
			this.StopDownStrike();
			yield break;
		}

		// Token: 0x06007050 RID: 28752 RVA: 0x00191AE0 File Offset: 0x0018FCE0
		private bool EnteredSpinAttackState()
		{
			return this._animator.GetCurrentAnimatorStateInfo(0).IsName("SpinKick_Attack") || this._movement.CurrentState != CharacterStates.MovementStates.DownStriking;
		}

		// Token: 0x06007051 RID: 28753 RVA: 0x0003DF65 File Offset: 0x0003C165
		public override void ProcessAbility()
		{
			base.ProcessAbility();
		}

		// Token: 0x06007052 RID: 28754 RVA: 0x00191B1C File Offset: 0x0018FD1C
		public virtual void StopDownStrike()
		{
			if (this._downstrikeCoroutine != null)
			{
				base.StopCoroutine(this._downstrikeCoroutine);
			}
			if (!this.SpinKickInstead && !this.m_playerController.CharacterDash.IsDashing)
			{
				this._controller.GravityActive(true);
				this._controller.SetForce(Vector2.zero);
			}
			this._downstrikeEndedNaturally = true;
			this.m_attackInProgress = false;
			if (this._movement.CurrentState == CharacterStates.MovementStates.DownStriking)
			{
				this._movement.ChangeState(CharacterStates.MovementStates.Idle);
			}
			if (this.m_firedProjectile)
			{
				this.m_firedProjectile.OnCollisionRelay.RemoveListener(this.m_bounce);
				this.m_firedProjectile.FlagForDestruction(null);
			}
			MMAnimator.UpdateAnimatorFloat(this._animator, "Ability_Anim_Speed", 1f);
			this.m_inputAngle = 0f;
		}

		// Token: 0x06007053 RID: 28755 RVA: 0x0003DF6D File Offset: 0x0003C16D
		protected void Update()
		{
			this.UpdateBounceLockTimer();
		}

		// Token: 0x06007054 RID: 28756 RVA: 0x0003DF75 File Offset: 0x0003C175
		private void LateUpdate()
		{
			if (this.m_triggerBounce)
			{
				this.TriggerBounce(true);
			}
		}

		// Token: 0x06007055 RID: 28757 RVA: 0x00191BEC File Offset: 0x0018FDEC
		public void Bounce(Projectile_RL projectile, GameObject colliderObj)
		{
			this.m_downstrikeEventArgs.Initialise(projectile, colliderObj);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerDownstrikeBounce, this, this.m_downstrikeEventArgs);
			Collider2D lastCollidedWith = projectile.HitboxController.LastCollidedWith;
			if (lastCollidedWith.CompareTag("Enemy"))
			{
				int manaRegenOnSpinKick = RuneLogicHelper.GetManaRegenOnSpinKick();
				if (manaRegenOnSpinKick > 0)
				{
					this.m_regenEventArgs.Initialise((float)manaRegenOnSpinKick, false);
					EffectManager.PlayEffect(this.m_playerController.gameObject, this._animator, "ManaRegenBurst_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
					Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerForceManaRegen, this, this.m_regenEventArgs);
				}
				if (this.m_playerController.CharacterClass.ClassType == ClassType.LuteClass)
				{
					this.m_playerController.StatusEffectController.StartStatusEffect(StatusEffectType.Player_Dance, 0f, this.m_playerController);
				}
			}
			if (this._condition.CurrentState == CharacterStates.CharacterConditions.Stunned || this._condition.CurrentState == CharacterStates.CharacterConditions.Dead)
			{
				return;
			}
			if (lastCollidedWith.CompareTag("EnemyProjectile") || lastCollidedWith.CompareTag("Generic_Bounceable"))
			{
				Vector3 effectPosition = projectile.HitboxController.LastCollidedWith.ClosestPoint(base.transform.position);
				EffectManager.PlayEffect(projectile.gameObject, this._animator, "BounceProjectileIndicatorBounced_Effect", effectPosition, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
				if (this.m_bounceEffectTriggeredUnityEvent != null)
				{
					this.m_bounceEffectTriggeredUnityEvent.Invoke();
				}
			}
			DownstrikeKnockbackOverride component = colliderObj.GetComponent<DownstrikeKnockbackOverride>();
			float num = this.AttackBounceHeight;
			if (component != null)
			{
				num = component.Value;
			}
			BoundsObj component2 = colliderObj.GetComponent<BoundsObj>();
			if (component2)
			{
				float num2 = num * num / (2f * Mathf.Abs(-50f));
				float num3 = component2.Top - projectile.Owner.transform.position.y;
				if (TraitManager.IsTraitActive(TraitType.GainDownStrike))
				{
					num3 = 0f;
				}
				num = Mathf.Sqrt(2f * Mathf.Abs(-50f) * (num2 + num3));
			}
			this.m_triggerBounceInvincibility = true;
			if (num > this.m_highestBounceAmount)
			{
				this.m_highestBounceAmount = num;
				this.m_triggerBounce = true;
				this.m_onSuccessfulDownstrikeRelay.Dispatch(projectile, colliderObj);
			}
		}

		// Token: 0x06007056 RID: 28758 RVA: 0x0003DF86 File Offset: 0x0003C186
		public void ForceTriggerBounce(float bounceHeight)
		{
			this.m_highestBounceAmount = bounceHeight;
			this.TriggerBounce(false);
			DownstrikeProjectile_RL.ConsecutiveStrikes++;
		}

		// Token: 0x06007057 RID: 28759 RVA: 0x00191E0C File Offset: 0x0019000C
		private void TriggerBounce(bool isDownstriking)
		{
			this.m_triggerBounce = false;
			float highestBounceAmount = this.m_highestBounceAmount;
			this.m_highestBounceAmount = float.MinValue;
			if (this._condition.CurrentState == CharacterStates.CharacterConditions.Stunned || this._condition.CurrentState == CharacterStates.CharacterConditions.Dead)
			{
				return;
			}
			this.m_playerController.ControllerCorgi.GravityActive(true);
			if (this.m_playerController.CharacterClass.ClassType == ClassType.LuteClass)
			{
				this.m_playerController.Animator.SetBool("DanceBounce", true);
			}
			else
			{
				this.m_playerController.Animator.SetBool("Bounce", true);
			}
			if (this.m_firedProjectile)
			{
				this.m_firedProjectile.OnCollisionRelay.RemoveListener(this.m_bounce);
			}
			this.StopDownStrike();
			this.m_playerController.CharacterDash.StopDash();
			this.m_playerController.CharacterFlight.StopFlight();
			if (TraitManager.IsTraitActive(TraitType.GainDownStrike) && isDownstriking && this.m_firedProjectile && this.m_firedProjectile.HitboxController.LastCollidedWith && this.m_firedProjectile.HitboxController.LastCollidedWith.isActiveAndEnabled)
			{
				Vector3 vector = this.m_firedProjectile.HitboxController.LastCollidedWith.ClosestPoint(this.m_firedProjectile.Midpoint);
				float num = 0.5f;
				RaycastHit2D hit = Physics2D.BoxCast(new Vector2(this.m_playerController.CollisionBounds.position.x, vector.y), this.m_playerController.CollisionBounds.size, 0f, Vector2.up, num, this.m_playerController.ControllerCorgi.PlatformMask);
				if (hit)
				{
					num = hit.distance;
				}
				if (num > 0f)
				{
					Vector3 localPosition = this.m_playerController.transform.localPosition;
					localPosition.y = vector.y + num;
					this.m_playerController.transform.localPosition = localPosition;
					this.m_playerController.ControllerCorgi.SetRaysParameters();
				}
			}
			this.m_playerController.SetVelocityY(highestBounceAmount, false);
			this._movement.ChangeState(CharacterStates.MovementStates.Jumping);
			if (this.m_playerController.CharacterJump)
			{
				this.m_playerController.CharacterJump.ResetBrakeForce();
				this.m_playerController.CharacterJump.StartJumpTime();
			}
			this.m_bounceLockTimer = this.BounceInputLockDuration;
			if ((this.ResetsDash || SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockBouncableDownstrike) > 0) && this.m_playerController.CharacterDash != null)
			{
				this.m_playerController.CharacterDash.ResetNumberOfDashes();
			}
			if (this.ResetsDoubleJump && this.m_playerController.CharacterJump != null)
			{
				this.m_playerController.CharacterJump.ResetNumberOfJumps();
			}
			this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false).LockoutTimer += 0.175f;
			if (this.m_triggerBounceInvincibility)
			{
				this.m_triggerBounceInvincibility = false;
				base.StartCoroutine(this.DisableBodyHitboxCoroutine());
			}
			AudioManager.PlayOneShotAttached(this, "event:/SFX/Weapons/sfx_player_spinkick_bounce", base.gameObject);
			DownstrikeProjectile_RL.ConsecutiveStrikes++;
			RelicObj relic = SaveManager.PlayerSaveData.GetRelic(RelicType.SpinKickLeavesCaltrops);
			if (relic.Level > 0)
			{
				float num2 = (float)Relic_EV.GetRelicMaxStack(relic.RelicType, relic.Level);
				if ((float)relic.IntValue >= num2)
				{
					ProjectileManager.FireProjectile(this.m_playerController.gameObject, this.m_relicCaltropDropProjectileName, new Vector2(0f, this.m_playerController.ControllerCorgi.BoundsHeight / 2f), true, 90f, 1f, false, true, true, true);
					relic.SetIntValue(0, false, true);
					this.m_playerController.StartSpinKicksDropCaltropsTimer();
				}
			}
		}

		// Token: 0x06007058 RID: 28760 RVA: 0x0003DFA2 File Offset: 0x0003C1A2
		private IEnumerator DisableBodyHitboxCoroutine()
		{
			bool storedBodyHitboxState = this.m_playerController.HitboxController.GetCollider(HitboxType.Body);
			this.m_playerController.HitboxController.SetHitboxActiveState(HitboxType.Body, false);
			float delay = Time.time + 0.015f;
			while (Time.time < delay)
			{
				yield return null;
			}
			if (!LocalTeleporterController.IsTeleporting)
			{
				this.m_playerController.HitboxController.SetHitboxActiveState(HitboxType.Body, storedBodyHitboxState);
			}
			yield break;
		}

		// Token: 0x06007059 RID: 28761 RVA: 0x001921E0 File Offset: 0x001903E0
		protected void UpdateBounceLockTimer()
		{
			if (this.m_bounceLockTimer > 0f)
			{
				this.m_bounceLockTimer -= Time.deltaTime;
				if (this.m_bounceLockTimer <= 0f)
				{
					this.m_playerController.Animator.SetBool("Bounce", false);
					this.m_playerController.Animator.SetBool("DanceBounce", false);
				}
			}
		}

		// Token: 0x0600705A RID: 28762 RVA: 0x00192248 File Offset: 0x00190448
		protected override void InitializeAnimatorParameters()
		{
			this.RegisterAnimatorParameter("Kick_Down", AnimatorControllerParameterType.Bool);
			this.RegisterAnimatorParameter("Kick_DownForward", AnimatorControllerParameterType.Bool);
			this.RegisterAnimatorParameter("360Kick", AnimatorControllerParameterType.Trigger);
			this.RegisterAnimatorParameter("SpinKickTell_Anim_Speed", AnimatorControllerParameterType.Float);
			this.RegisterAnimatorParameter("SpinKickAttack_Anim_Speed", AnimatorControllerParameterType.Float);
			this.RegisterAnimatorParameter("SpinKickExit_Anim_Speed", AnimatorControllerParameterType.Float);
		}

		// Token: 0x0600705B RID: 28763 RVA: 0x001922A0 File Offset: 0x001904A0
		public override void UpdateAnimator()
		{
			if (!this.SpinKickInstead)
			{
				bool flag = (this.m_inputAngle > (float)this.ForwardKickMinMaxAngle.x && this.m_inputAngle < (float)this.DownKickMinMaxAngle.x) || (this.m_inputAngle > (float)this.DownKickMinMaxAngle.y && this.m_inputAngle < (float)this.ForwardKickMinMaxAngle.y);
				MMAnimator.UpdateAnimatorBool(this._animator, "Kick_Down", this._movement.CurrentState == CharacterStates.MovementStates.DownStriking && !flag, this._character._animatorParameters);
				MMAnimator.UpdateAnimatorBool(this._animator, "Kick_DownForward", this._movement.CurrentState == CharacterStates.MovementStates.DownStriking && flag, this._character._animatorParameters);
			}
		}

		// Token: 0x0600705C RID: 28764 RVA: 0x0003DFB1 File Offset: 0x0003C1B1
		protected void OnDestroy()
		{
			if (this.m_playerController && this.m_playerController.ControllerCorgi)
			{
				this.m_playerController.ControllerCorgi.OnCorgiLandedEnterRelay.RemoveListener(this.m_resetConsecutiveStrikes);
			}
		}

		// Token: 0x0600705D RID: 28765 RVA: 0x0003DFEE File Offset: 0x0003C1EE
		private void ResetConsecutiveStrikes(CorgiController corgiController)
		{
			if (this.m_firedProjectile != null)
			{
				DownstrikeProjectile_RL.ConsecutiveStrikes = 0;
			}
		}

		// Token: 0x04005A77 RID: 23159
		[Space(10f)]
		[SerializeField]
		protected string m_projectileName;

		// Token: 0x04005A78 RID: 23160
		[SerializeField]
		protected string m_spinKickProjectileName;

		// Token: 0x04005A79 RID: 23161
		[SerializeField]
		protected string m_resonantSpinKickProjectileName;

		// Token: 0x04005A7A RID: 23162
		[SerializeField]
		private string m_resonantDownStrikeProjectileName;

		// Token: 0x04005A7B RID: 23163
		[SerializeField]
		private string m_relicCaltropDropProjectileName;

		// Token: 0x04005A7C RID: 23164
		[SerializeField]
		private string m_relicCaltropProjectileName;

		// Token: 0x04005A7D RID: 23165
		[Space(10f)]
		public float AttackDuration = 3f;

		// Token: 0x04005A7E RID: 23166
		public float AttackSpeed = 40f;

		// Token: 0x04005A7F RID: 23167
		public float AttackCooldown;

		// Token: 0x04005A80 RID: 23168
		public float ForwardKickAngle = -70f;

		// Token: 0x04005A81 RID: 23169
		public Vector2Int ForwardKickMinMaxAngle = new Vector2Int(-120, -70);

		// Token: 0x04005A82 RID: 23170
		public Vector2Int DownKickMinMaxAngle = new Vector2Int(-100, -90);

		// Token: 0x04005A83 RID: 23171
		public float AttackBounceHeight = 2f;

		// Token: 0x04005A84 RID: 23172
		public float BounceInputLockDuration = 1f;

		// Token: 0x04005A85 RID: 23173
		public bool AttackButtonTriggersSpinKick;

		// Token: 0x04005A86 RID: 23174
		public bool SpinKickInstead;

		// Token: 0x04005A87 RID: 23175
		[SerializeField]
		[ConditionalHide("SpinKickInstead", true)]
		private float m_spinKickTellAnimMultiplier = 1f;

		// Token: 0x04005A88 RID: 23176
		[SerializeField]
		[ConditionalHide("SpinKickInstead", true)]
		private float m_spinKickAttackAnimMultiplier = 1f;

		// Token: 0x04005A89 RID: 23177
		[SerializeField]
		[ConditionalHide("SpinKickInstead", true)]
		private float m_spinKickExitAnimMultiplier = 1f;

		// Token: 0x04005A8A RID: 23178
		[Header("Bounce Mods")]
		public bool ResetsDoubleJump;

		// Token: 0x04005A8B RID: 23179
		public bool ResetsDash;

		// Token: 0x04005A8C RID: 23180
		[SerializeField]
		[FormerlySerializedAs("m_downStrikeBounceUnityEvent")]
		private UnityEvent m_bounceEffectTriggeredUnityEvent;

		// Token: 0x04005A8D RID: 23181
		protected Vector2 m_computedDashForce;

		// Token: 0x04005A8E RID: 23182
		protected float _cooldownTimeStamp;

		// Token: 0x04005A8F RID: 23183
		protected float _startTime;

		// Token: 0x04005A90 RID: 23184
		protected Vector3 _initialPosition;

		// Token: 0x04005A91 RID: 23185
		protected float _dashDirection;

		// Token: 0x04005A92 RID: 23186
		protected float _distanceTraveled;

		// Token: 0x04005A93 RID: 23187
		protected float _slopeAngleSave;

		// Token: 0x04005A94 RID: 23188
		protected bool _downstrikeEndedNaturally = true;

		// Token: 0x04005A95 RID: 23189
		protected IEnumerator _downstrikeCoroutine;

		// Token: 0x04005A96 RID: 23190
		protected float m_totalDashTime;

		// Token: 0x04005A97 RID: 23191
		protected bool m_attackInProgress;

		// Token: 0x04005A98 RID: 23192
		protected IEnumerator m_attackCoroutine;

		// Token: 0x04005A99 RID: 23193
		protected DownstrikeProjectile_RL m_firedProjectile;

		// Token: 0x04005A9A RID: 23194
		protected float m_inputAngle;

		// Token: 0x04005A9B RID: 23195
		protected Vector2 m_forwardKickRightAngle;

		// Token: 0x04005A9C RID: 23196
		protected Vector2 m_forwardKickLeftAngle;

		// Token: 0x04005A9D RID: 23197
		protected Vector3 m_originalRotation;

		// Token: 0x04005A9E RID: 23198
		protected float m_bounceLockTimer;

		// Token: 0x04005A9F RID: 23199
		private int m_consecutiveStrikes;

		// Token: 0x04005AA0 RID: 23200
		private WaitUntil m_waitUntilSpinKickAttackYield;

		// Token: 0x04005AA1 RID: 23201
		private Action<CorgiController_RL> m_resetConsecutiveStrikes;

		// Token: 0x04005AA2 RID: 23202
		private Action<Projectile_RL, GameObject> m_bounce;

		// Token: 0x04005AA3 RID: 23203
		[NonSerialized]
		private string[] m_projectileNameArray;

		// Token: 0x04005AA5 RID: 23205
		private Relay<Projectile_RL, GameObject> m_onSuccessfulDownstrikeRelay = new Relay<Projectile_RL, GameObject>();

		// Token: 0x04005AA6 RID: 23206
		private float m_highestBounceAmount = float.MinValue;

		// Token: 0x04005AA7 RID: 23207
		private bool m_triggerBounce;

		// Token: 0x04005AA8 RID: 23208
		private bool m_triggerBounceInvincibility;

		// Token: 0x04005AA9 RID: 23209
		private ForceManaRegenEventArgs m_regenEventArgs = new ForceManaRegenEventArgs(0f, false);

		// Token: 0x04005AAA RID: 23210
		private PlayerDownstrikeEventArgs m_downstrikeEventArgs = new PlayerDownstrikeEventArgs(null, null);
	}
}
