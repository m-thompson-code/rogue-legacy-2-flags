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
	// Token: 0x02000975 RID: 2421
	[AddComponentMenu("Corgi Engine/Character/Abilities/Character Downstrike RL")]
	public class CharacterDownStrike_RL : CharacterAbility, IHasProjectileNameArray, IAudioEventEmitter
	{
		// Token: 0x0600522A RID: 21034 RVA: 0x00123731 File Offset: 0x00121931
		public override string HelpBoxText()
		{
			return "This component allows your character to dash. Here you can define the distance the dash should cover, how much force to apply, and the cooldown between the end of a dash and the start of the next one.";
		}

		// Token: 0x17001B2C RID: 6956
		// (get) Token: 0x0600522B RID: 21035 RVA: 0x00123738 File Offset: 0x00121938
		public bool IsTriggeringBounce
		{
			get
			{
				return this.m_triggerBounce;
			}
		}

		// Token: 0x17001B2D RID: 6957
		// (get) Token: 0x0600522C RID: 21036 RVA: 0x00123740 File Offset: 0x00121940
		public bool AbilityInProgress
		{
			get
			{
				return this.m_attackInProgress;
			}
		}

		// Token: 0x17001B2E RID: 6958
		// (get) Token: 0x0600522D RID: 21037 RVA: 0x00123748 File Offset: 0x00121948
		public string Description
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x17001B2F RID: 6959
		// (get) Token: 0x0600522E RID: 21038 RVA: 0x00123750 File Offset: 0x00121950
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

		// Token: 0x17001B30 RID: 6960
		// (get) Token: 0x0600522F RID: 21039 RVA: 0x001237AD File Offset: 0x001219AD
		// (set) Token: 0x06005230 RID: 21040 RVA: 0x001237B5 File Offset: 0x001219B5
		public OnDownStrikeDelegate OnDownStrikeEvent { get; set; }

		// Token: 0x17001B31 RID: 6961
		// (get) Token: 0x06005231 RID: 21041 RVA: 0x001237BE File Offset: 0x001219BE
		public IRelayLink<Projectile_RL, GameObject> OnSuccessfulDownstrikeRelay
		{
			get
			{
				return this.m_onSuccessfulDownstrikeRelay;
			}
		}

		// Token: 0x17001B32 RID: 6962
		// (get) Token: 0x06005232 RID: 21042 RVA: 0x001237C8 File Offset: 0x001219C8
		public bool IsHoldingDownStrikeAngle
		{
			get
			{
				float num = Mathf.Atan2(this._verticalInput, this._horizontalInput) * 57.29578f;
				return num > (float)this.ForwardKickMinMaxAngle.x && num < (float)this.ForwardKickMinMaxAngle.y;
			}
		}

		// Token: 0x06005233 RID: 21043 RVA: 0x00123810 File Offset: 0x00121A10
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

		// Token: 0x06005234 RID: 21044 RVA: 0x001238BC File Offset: 0x00121ABC
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

		// Token: 0x06005235 RID: 21045 RVA: 0x00123948 File Offset: 0x00121B48
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

		// Token: 0x06005236 RID: 21046 RVA: 0x00123AA9 File Offset: 0x00121CA9
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

		// Token: 0x06005237 RID: 21047 RVA: 0x00123AB8 File Offset: 0x00121CB8
		private bool EnteredSpinAttackState()
		{
			return this._animator.GetCurrentAnimatorStateInfo(0).IsName("SpinKick_Attack") || this._movement.CurrentState != CharacterStates.MovementStates.DownStriking;
		}

		// Token: 0x06005238 RID: 21048 RVA: 0x00123AF3 File Offset: 0x00121CF3
		public override void ProcessAbility()
		{
			base.ProcessAbility();
		}

		// Token: 0x06005239 RID: 21049 RVA: 0x00123AFC File Offset: 0x00121CFC
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

		// Token: 0x0600523A RID: 21050 RVA: 0x00123BCB File Offset: 0x00121DCB
		protected void Update()
		{
			this.UpdateBounceLockTimer();
		}

		// Token: 0x0600523B RID: 21051 RVA: 0x00123BD3 File Offset: 0x00121DD3
		private void LateUpdate()
		{
			if (this.m_triggerBounce)
			{
				this.TriggerBounce(true);
			}
		}

		// Token: 0x0600523C RID: 21052 RVA: 0x00123BE4 File Offset: 0x00121DE4
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

		// Token: 0x0600523D RID: 21053 RVA: 0x00123E01 File Offset: 0x00122001
		public void ForceTriggerBounce(float bounceHeight)
		{
			this.m_highestBounceAmount = bounceHeight;
			this.TriggerBounce(false);
			DownstrikeProjectile_RL.ConsecutiveStrikes++;
		}

		// Token: 0x0600523E RID: 21054 RVA: 0x00123E20 File Offset: 0x00122020
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

		// Token: 0x0600523F RID: 21055 RVA: 0x001241F2 File Offset: 0x001223F2
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

		// Token: 0x06005240 RID: 21056 RVA: 0x00124204 File Offset: 0x00122404
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

		// Token: 0x06005241 RID: 21057 RVA: 0x0012426C File Offset: 0x0012246C
		protected override void InitializeAnimatorParameters()
		{
			this.RegisterAnimatorParameter("Kick_Down", AnimatorControllerParameterType.Bool);
			this.RegisterAnimatorParameter("Kick_DownForward", AnimatorControllerParameterType.Bool);
			this.RegisterAnimatorParameter("360Kick", AnimatorControllerParameterType.Trigger);
			this.RegisterAnimatorParameter("SpinKickTell_Anim_Speed", AnimatorControllerParameterType.Float);
			this.RegisterAnimatorParameter("SpinKickAttack_Anim_Speed", AnimatorControllerParameterType.Float);
			this.RegisterAnimatorParameter("SpinKickExit_Anim_Speed", AnimatorControllerParameterType.Float);
		}

		// Token: 0x06005242 RID: 21058 RVA: 0x001242C4 File Offset: 0x001224C4
		public override void UpdateAnimator()
		{
			if (!this.SpinKickInstead)
			{
				bool flag = (this.m_inputAngle > (float)this.ForwardKickMinMaxAngle.x && this.m_inputAngle < (float)this.DownKickMinMaxAngle.x) || (this.m_inputAngle > (float)this.DownKickMinMaxAngle.y && this.m_inputAngle < (float)this.ForwardKickMinMaxAngle.y);
				MMAnimator.UpdateAnimatorBool(this._animator, "Kick_Down", this._movement.CurrentState == CharacterStates.MovementStates.DownStriking && !flag, this._character._animatorParameters);
				MMAnimator.UpdateAnimatorBool(this._animator, "Kick_DownForward", this._movement.CurrentState == CharacterStates.MovementStates.DownStriking && flag, this._character._animatorParameters);
			}
		}

		// Token: 0x06005243 RID: 21059 RVA: 0x00124390 File Offset: 0x00122590
		protected void OnDestroy()
		{
			if (this.m_playerController && this.m_playerController.ControllerCorgi)
			{
				this.m_playerController.ControllerCorgi.OnCorgiLandedEnterRelay.RemoveListener(this.m_resetConsecutiveStrikes);
			}
		}

		// Token: 0x06005244 RID: 21060 RVA: 0x001243CD File Offset: 0x001225CD
		private void ResetConsecutiveStrikes(CorgiController corgiController)
		{
			if (this.m_firedProjectile != null)
			{
				DownstrikeProjectile_RL.ConsecutiveStrikes = 0;
			}
		}

		// Token: 0x04004428 RID: 17448
		[Space(10f)]
		[SerializeField]
		protected string m_projectileName;

		// Token: 0x04004429 RID: 17449
		[SerializeField]
		protected string m_spinKickProjectileName;

		// Token: 0x0400442A RID: 17450
		[SerializeField]
		protected string m_resonantSpinKickProjectileName;

		// Token: 0x0400442B RID: 17451
		[SerializeField]
		private string m_resonantDownStrikeProjectileName;

		// Token: 0x0400442C RID: 17452
		[SerializeField]
		private string m_relicCaltropDropProjectileName;

		// Token: 0x0400442D RID: 17453
		[SerializeField]
		private string m_relicCaltropProjectileName;

		// Token: 0x0400442E RID: 17454
		[Space(10f)]
		public float AttackDuration = 3f;

		// Token: 0x0400442F RID: 17455
		public float AttackSpeed = 40f;

		// Token: 0x04004430 RID: 17456
		public float AttackCooldown;

		// Token: 0x04004431 RID: 17457
		public float ForwardKickAngle = -70f;

		// Token: 0x04004432 RID: 17458
		public Vector2Int ForwardKickMinMaxAngle = new Vector2Int(-120, -70);

		// Token: 0x04004433 RID: 17459
		public Vector2Int DownKickMinMaxAngle = new Vector2Int(-100, -90);

		// Token: 0x04004434 RID: 17460
		public float AttackBounceHeight = 2f;

		// Token: 0x04004435 RID: 17461
		public float BounceInputLockDuration = 1f;

		// Token: 0x04004436 RID: 17462
		public bool AttackButtonTriggersSpinKick;

		// Token: 0x04004437 RID: 17463
		public bool SpinKickInstead;

		// Token: 0x04004438 RID: 17464
		[SerializeField]
		[ConditionalHide("SpinKickInstead", true)]
		private float m_spinKickTellAnimMultiplier = 1f;

		// Token: 0x04004439 RID: 17465
		[SerializeField]
		[ConditionalHide("SpinKickInstead", true)]
		private float m_spinKickAttackAnimMultiplier = 1f;

		// Token: 0x0400443A RID: 17466
		[SerializeField]
		[ConditionalHide("SpinKickInstead", true)]
		private float m_spinKickExitAnimMultiplier = 1f;

		// Token: 0x0400443B RID: 17467
		[Header("Bounce Mods")]
		public bool ResetsDoubleJump;

		// Token: 0x0400443C RID: 17468
		public bool ResetsDash;

		// Token: 0x0400443D RID: 17469
		[SerializeField]
		[FormerlySerializedAs("m_downStrikeBounceUnityEvent")]
		private UnityEvent m_bounceEffectTriggeredUnityEvent;

		// Token: 0x0400443E RID: 17470
		protected Vector2 m_computedDashForce;

		// Token: 0x0400443F RID: 17471
		protected float _cooldownTimeStamp;

		// Token: 0x04004440 RID: 17472
		protected float _startTime;

		// Token: 0x04004441 RID: 17473
		protected Vector3 _initialPosition;

		// Token: 0x04004442 RID: 17474
		protected float _dashDirection;

		// Token: 0x04004443 RID: 17475
		protected float _distanceTraveled;

		// Token: 0x04004444 RID: 17476
		protected float _slopeAngleSave;

		// Token: 0x04004445 RID: 17477
		protected bool _downstrikeEndedNaturally = true;

		// Token: 0x04004446 RID: 17478
		protected IEnumerator _downstrikeCoroutine;

		// Token: 0x04004447 RID: 17479
		protected float m_totalDashTime;

		// Token: 0x04004448 RID: 17480
		protected bool m_attackInProgress;

		// Token: 0x04004449 RID: 17481
		protected IEnumerator m_attackCoroutine;

		// Token: 0x0400444A RID: 17482
		protected DownstrikeProjectile_RL m_firedProjectile;

		// Token: 0x0400444B RID: 17483
		protected float m_inputAngle;

		// Token: 0x0400444C RID: 17484
		protected Vector2 m_forwardKickRightAngle;

		// Token: 0x0400444D RID: 17485
		protected Vector2 m_forwardKickLeftAngle;

		// Token: 0x0400444E RID: 17486
		protected Vector3 m_originalRotation;

		// Token: 0x0400444F RID: 17487
		protected float m_bounceLockTimer;

		// Token: 0x04004450 RID: 17488
		private int m_consecutiveStrikes;

		// Token: 0x04004451 RID: 17489
		private WaitUntil m_waitUntilSpinKickAttackYield;

		// Token: 0x04004452 RID: 17490
		private Action<CorgiController_RL> m_resetConsecutiveStrikes;

		// Token: 0x04004453 RID: 17491
		private Action<Projectile_RL, GameObject> m_bounce;

		// Token: 0x04004454 RID: 17492
		[NonSerialized]
		private string[] m_projectileNameArray;

		// Token: 0x04004456 RID: 17494
		private Relay<Projectile_RL, GameObject> m_onSuccessfulDownstrikeRelay = new Relay<Projectile_RL, GameObject>();

		// Token: 0x04004457 RID: 17495
		private float m_highestBounceAmount = float.MinValue;

		// Token: 0x04004458 RID: 17496
		private bool m_triggerBounce;

		// Token: 0x04004459 RID: 17497
		private bool m_triggerBounceInvincibility;

		// Token: 0x0400445A RID: 17498
		private ForceManaRegenEventArgs m_regenEventArgs = new ForceManaRegenEventArgs(0f, false);

		// Token: 0x0400445B RID: 17499
		private PlayerDownstrikeEventArgs m_downstrikeEventArgs = new PlayerDownstrikeEventArgs(null, null);
	}
}
