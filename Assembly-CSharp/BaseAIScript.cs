using System;
using System.Collections;
using System.Linq;
using RLAudio;
using UnityEngine;

// Token: 0x02000093 RID: 147
public abstract class BaseAIScript : MonoBehaviour, IHasProjectileNameArray, IAudioEventEmitter
{
	// Token: 0x17000068 RID: 104
	// (get) Token: 0x0600021F RID: 543 RVA: 0x00012535 File Offset: 0x00010735
	protected virtual Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.35f, 0.75f);
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000220 RID: 544 RVA: 0x00012546 File Offset: 0x00010746
	protected virtual Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.4f, 0.9f);
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06000221 RID: 545 RVA: 0x00012557 File Offset: 0x00010757
	protected virtual Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.4f, 0.9f);
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06000222 RID: 546 RVA: 0x00012568 File Offset: 0x00010768
	protected virtual Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(3f, 8.5f);
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x06000223 RID: 547 RVA: 0x00012579 File Offset: 0x00010779
	protected virtual Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(1.5f, 6f);
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x06000224 RID: 548 RVA: 0x0001258A File Offset: 0x0001078A
	protected virtual float IdleAnimSpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x06000225 RID: 549 RVA: 0x00012591 File Offset: 0x00010791
	protected virtual float WalkAnimSpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06000226 RID: 550 RVA: 0x00012598 File Offset: 0x00010798
	protected virtual float ForcedIdleDuration_IfReversingDirection
	{
		get
		{
			return 0.175f;
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06000227 RID: 551 RVA: 0x0001259F File Offset: 0x0001079F
	public string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				this.InitializeProjectileNameArray();
				if (this.m_projectileNameArray == null)
				{
					this.m_projectileNameArray = EnemySpawnController.EmptyProjectileNameArray_STATIC;
				}
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06000228 RID: 552 RVA: 0x000125C8 File Offset: 0x000107C8
	public string Description
	{
		get
		{
			if (this.m_description == string.Empty)
			{
				this.m_description = this.ToString();
			}
			return this.m_description;
		}
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x06000229 RID: 553 RVA: 0x000125EE File Offset: 0x000107EE
	// (set) Token: 0x0600022A RID: 554 RVA: 0x000125F6 File Offset: 0x000107F6
	public bool IsPaused { get; private set; }

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x0600022B RID: 555 RVA: 0x000125FF File Offset: 0x000107FF
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x0600022C RID: 556 RVA: 0x00012607 File Offset: 0x00010807
	public LogicController LogicController
	{
		get
		{
			return this.m_logicController;
		}
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x0600022D RID: 557 RVA: 0x0001260F File Offset: 0x0001080F
	public EnemyController EnemyController
	{
		get
		{
			return this.m_enemyController;
		}
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x0600022E RID: 558 RVA: 0x00012617 File Offset: 0x00010817
	public GameObject Target
	{
		get
		{
			return this.m_enemyController.Target;
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x0600022F RID: 559 RVA: 0x00012624 File Offset: 0x00010824
	public BaseCharacterController TargetController
	{
		get
		{
			return this.m_enemyController.TargetController;
		}
	}

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x06000230 RID: 560 RVA: 0x00012631 File Offset: 0x00010831
	public bool IsInitialized
	{
		get
		{
			return this.m_isInitialized;
		}
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x06000231 RID: 561 RVA: 0x00012639 File Offset: 0x00010839
	public virtual bool ForceDeathAnimation
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000232 RID: 562
	protected abstract void InitializeProjectileNameArray();

	// Token: 0x06000233 RID: 563 RVA: 0x0001263C File Offset: 0x0001083C
	public virtual void Initialize(EnemyController enemyController)
	{
		this.m_enemyController = enemyController;
		this.m_animator = this.m_enemyController.Animator;
		this.m_logicController = this.m_enemyController.LogicController;
		base.transform.localPosition = Vector3.zero;
		this.m_isInitialized = true;
	}

	// Token: 0x06000234 RID: 564 RVA: 0x00012689 File Offset: 0x00010889
	protected virtual void OnDisable()
	{
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0001268B File Offset: 0x0001088B
	public virtual void OnEnemyActivated()
	{
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0001268D File Offset: 0x0001088D
	public virtual void Pause()
	{
		this.IsPaused = true;
		if (this.m_waitYield != null)
		{
			this.m_waitYield.Pause();
		}
	}

	// Token: 0x06000237 RID: 567 RVA: 0x000126A9 File Offset: 0x000108A9
	public virtual void Unpause()
	{
		this.IsPaused = false;
		if (this.m_waitYield != null)
		{
			this.m_waitYield.Unpause();
		}
	}

	// Token: 0x06000238 RID: 568 RVA: 0x000126C8 File Offset: 0x000108C8
	public virtual void OnLBCompleteOrCancelled()
	{
		this.EnemyController.ResetBaseValues();
		this.SetAttackingWithContactDamage(false, 0f);
		if (global::AnimatorUtility.HasParameter(this.m_animator, "Anim_Speed"))
		{
			this.m_animator.SetFloat("Anim_Speed", 1f);
		}
		else if (global::AnimatorUtility.HasParameter(this.m_animator, "Ability_Anim_Speed"))
		{
			this.m_animator.SetFloat("Ability_Anim_Speed", 1f);
		}
		this.EnemyController.ResetTurnTrigger();
		this.Unpause();
	}

	// Token: 0x06000239 RID: 569 RVA: 0x0001274D File Offset: 0x0001094D
	public virtual void ResetScript()
	{
		this.LogicController.ForceExecuteLogicBlockName_OnceOnly = null;
		this.OnLBCompleteOrCancelled();
	}

	// Token: 0x0600023A RID: 570 RVA: 0x00012761 File Offset: 0x00010961
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[WanderLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public virtual IEnumerator Idle()
	{
		yield return this.Idle(UnityEngine.Random.Range(this.IdleDuration.x, this.IdleDuration.y));
		yield break;
	}

	// Token: 0x0600023B RID: 571 RVA: 0x00012770 File Offset: 0x00010970
	public virtual IEnumerator Idle(float idleDuration)
	{
		if (this.EnemyController.IsFlying || (!this.EnemyController.IsFlying && this.EnemyController.IsGrounded))
		{
			this.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
			this.EnemyController.FollowTarget = true;
			if (!this.EnemyController.KnockedIntoAir)
			{
				this.SetVelocity(Vector2.zero, false);
			}
		}
		this.SetAnimationSpeedMultiplier(this.IdleAnimSpeedMod);
		yield return this.Wait(idleDuration, false);
		this.SetAnimationSpeedMultiplier(1f);
		yield return null;
		yield break;
	}

	// Token: 0x0600023C RID: 572 RVA: 0x00012786 File Offset: 0x00010986
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public virtual IEnumerator WalkTowards()
	{
		if (this.LogicController.PreviousLogicBlockName == "WalkAway")
		{
			yield return this.Idle(this.ForcedIdleDuration_IfReversingDirection);
		}
		this.SetAnimationSpeedMultiplier(this.WalkAnimSpeedMod);
		if (this.EnemyController.IsFlying || (!this.EnemyController.IsFlying && this.EnemyController.IsGrounded))
		{
			if (this.EnemyController.IsFlying)
			{
				this.EnemyController.FollowTarget = true;
				this.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
				this.EnemyController.GenerateRandomFollowOffset(this.RandomFollowOffsetX, this.RandomFollowOffsetY);
			}
			else
			{
				bool flag = true;
				if ((this.EnemyController.IsTargetToMyRight && this.EnemyController.RightSidePlatformDropPrevented) || (!this.EnemyController.IsTargetToMyRight && this.EnemyController.LeftSidePlatformDropPrevented))
				{
					flag = false;
				}
				if (flag)
				{
					if (this.EnemyController.IsTargetToMyRight)
					{
						this.SetVelocity(this.EnemyController.ActualSpeed, 0f, false);
					}
					else
					{
						this.SetVelocity(-this.EnemyController.ActualSpeed, 0f, false);
					}
				}
				if (!this.EnemyController.AlwaysFacing)
				{
					this.FaceTarget();
				}
				if ((this.EnemyController.IsFacingRight && this.EnemyController.ControllerCorgi.State.IsCollidingRight && this.EnemyController.Velocity.x > 0f) || (!this.EnemyController.IsFacingRight && this.EnemyController.ControllerCorgi.State.IsCollidingLeft && this.EnemyController.Velocity.x < 0f))
				{
					this.EnemyController.SetVelocityX(0f, false);
				}
			}
		}
		yield return this.Wait(UnityEngine.Random.Range(this.WalkTowardsDuration.x, this.WalkTowardsDuration.y), false);
		this.SetAnimationSpeedMultiplier(1f);
		yield return null;
		yield break;
	}

	// Token: 0x0600023D RID: 573 RVA: 0x00012795 File Offset: 0x00010995
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public virtual IEnumerator WalkAway()
	{
		if (this.LogicController.PreviousLogicBlockName == "WalkTowards")
		{
			yield return this.Idle(this.ForcedIdleDuration_IfReversingDirection);
		}
		this.SetAnimationSpeedMultiplier(this.WalkAnimSpeedMod);
		if (this.EnemyController.IsFlying || (!this.EnemyController.IsFlying && this.EnemyController.IsGrounded))
		{
			if (this.EnemyController.IsFlying)
			{
				this.EnemyController.FollowTarget = true;
				this.EnemyController.FlyingMovementType = FlyingMovementType.Away;
			}
			else
			{
				bool flag = true;
				if ((!this.EnemyController.IsTargetToMyRight && this.EnemyController.RightSidePlatformDropPrevented) || (this.EnemyController.IsTargetToMyRight && this.EnemyController.LeftSidePlatformDropPrevented))
				{
					flag = false;
				}
				if (flag)
				{
					if (this.EnemyController.IsTargetToMyRight)
					{
						this.SetVelocity(-this.EnemyController.ActualSpeed, 0f, false);
					}
					else
					{
						this.SetVelocity(this.EnemyController.ActualSpeed, 0f, false);
					}
				}
				if (!this.EnemyController.AlwaysFacing)
				{
					this.FaceAwayFromTarget();
				}
			}
		}
		yield return this.Wait(UnityEngine.Random.Range(this.WalkAwayDuration.x, this.WalkAwayDuration.y), false);
		this.SetAnimationSpeedMultiplier(1f);
		yield return null;
		yield break;
	}

	// Token: 0x0600023E RID: 574 RVA: 0x000127A4 File Offset: 0x000109A4
	public virtual IEnumerator DeathAnim()
	{
		if (this.EnemyController.IsBoss)
		{
			this.EnemyController.BaseKnockbackDefense = 999999f;
			RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 0f);
			yield return this.Wait(0.25f, true);
			this.PlayDeathAnimAudio();
			BossRoomController component = this.EnemyController.Room.gameObject.GetComponent<BossRoomController>();
			bool flag;
			if (component)
			{
				flag = component.AllBossesDeadOrDying;
			}
			else
			{
				flag = (EnemyManager.NumActiveEnemies == 0 && EnemyManager.NumActiveSummonedEnemies == 0);
			}
			if (component && this.EnemyController.KilledByKnockout && this.EnemyController.IsDead && flag)
			{
				StoreAPIManager.GiveAchievement(AchievementType.BoxerBoss, StoreType.All);
				CameraController.PlayKOEffect(7f);
				yield return this.Wait(0.5f, true);
			}
			else
			{
				EffectManager.PlayEffect(base.gameObject, this.m_animator, "ScreenFlashEffect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
				yield return this.Wait(0.2f, true);
				EffectManager.PlayEffect(base.gameObject, this.m_animator, "ScreenFlashEffect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
				yield return this.Wait(0.2f, true);
			}
			yield return this.Wait(0.5f, true);
			RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
			this.EnemyController.BaseKnockbackDefense = (float)this.EnemyController.EnemyData.KnockbackDefence;
		}
		yield break;
	}

	// Token: 0x0600023F RID: 575 RVA: 0x000127B3 File Offset: 0x000109B3
	protected virtual void PlayDeathAnimAudio()
	{
		AudioManager.PlayOneShot(this, "event:/SFX/Enemies/sfx_spellswordBoss_death_flash", base.gameObject.transform.position);
	}

	// Token: 0x06000240 RID: 576 RVA: 0x000127D0 File Offset: 0x000109D0
	public virtual IEnumerator SpawnAnim()
	{
		yield break;
	}

	// Token: 0x06000241 RID: 577 RVA: 0x000127D8 File Offset: 0x000109D8
	public virtual IEnumerator Default_TellIntroAndLoop(string tellIntroName, float tellIntroAnimSpeed, string tellHoldName, float tellHoldAnimSpeed, float totalDuration)
	{
		if (tellHoldAnimSpeed == 0f)
		{
			tellHoldAnimSpeed = 1f;
		}
		float startTime = Time.time;
		this.SetAnimationSpeedMultiplier(tellIntroAnimSpeed);
		yield return this.ChangeAnimationState(tellIntroName);
		yield return this.WaitUntilAnimComplete(this.DefaultAnimationLayer);
		float midElapsedTime = Time.time - startTime;
		float duration = totalDuration - midElapsedTime;
		this.SetAnimationSpeedMultiplier(tellHoldAnimSpeed);
		yield return this.ChangeAnimationState(tellHoldName);
		if (midElapsedTime < totalDuration && duration > 0f)
		{
			yield return this.Wait(duration, false);
		}
		yield break;
	}

	// Token: 0x06000242 RID: 578 RVA: 0x0001280C File Offset: 0x00010A0C
	public virtual IEnumerator Default_Animation(string animName, float animSpeed, float delay, bool waitUntilAnimationComplete = true)
	{
		this.SetAnimationSpeedMultiplier(animSpeed);
		yield return this.ChangeAnimationState(animName);
		if (waitUntilAnimationComplete)
		{
			yield return this.WaitUntilAnimComplete(this.DefaultAnimationLayer);
		}
		if (delay > 0f)
		{
			yield return this.Wait(delay, false);
		}
		yield break;
	}

	// Token: 0x06000243 RID: 579 RVA: 0x00012838 File Offset: 0x00010A38
	public virtual IEnumerator Default_Animation(int animID, float animSpeed, float delay, bool waitUntilAnimationComplete = true)
	{
		this.SetAnimationSpeedMultiplier(animSpeed);
		yield return this.ChangeAnimationState(animID);
		if (waitUntilAnimationComplete)
		{
			yield return this.WaitUntilAnimComplete(this.DefaultAnimationLayer);
		}
		if (delay > 0f)
		{
			yield return this.Wait(delay, false);
		}
		yield break;
	}

	// Token: 0x06000244 RID: 580 RVA: 0x00012864 File Offset: 0x00010A64
	public virtual IEnumerator Default_Attack_Cooldown(float idleDuration, float attackCD)
	{
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		float num = BurdenManager.GetBurdenStatGain(BurdenType.EnemyAggression);
		if (this.EnemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Speed) || this.EnemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_ArmorShred))
		{
			num += 0.25f;
		}
		num = Mathf.Max(1f - num, 0.25f);
		idleDuration *= num;
		if (idleDuration > 0f)
		{
			yield return this.Idle(idleDuration);
		}
		if (attackCD > 0f)
		{
			this.TriggerAttackCooldown(attackCD, false);
		}
		this.TriggerRestState(false);
		yield break;
	}

	// Token: 0x06000245 RID: 581 RVA: 0x00012884 File Offset: 0x00010A84
	public virtual void StopAndFaceTarget()
	{
		this.FaceTarget();
		if (this.EnemyController.IsFlying)
		{
			this.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
			this.SetVelocity(Vector2.zero, false);
			return;
		}
		if (this.EnemyController.IsGrounded)
		{
			this.SetVelocityX(0f, false);
		}
	}

	// Token: 0x06000246 RID: 582 RVA: 0x000128D6 File Offset: 0x00010AD6
	public void SetVelocity(Vector2 velocity, bool additive)
	{
		this.m_enemyController.SetVelocity(velocity.x, velocity.y, additive);
	}

	// Token: 0x06000247 RID: 583 RVA: 0x000128F0 File Offset: 0x00010AF0
	public void SetVelocity(float x, float y, bool additive)
	{
		this.m_enemyController.SetVelocity(x, y, additive);
	}

	// Token: 0x06000248 RID: 584 RVA: 0x00012900 File Offset: 0x00010B00
	public void SetVelocityX(float velocity, bool additive)
	{
		this.m_enemyController.SetVelocityX(velocity, additive);
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0001290F File Offset: 0x00010B0F
	public void SetVelocityY(float velocity, bool additive)
	{
		this.m_enemyController.SetVelocityY(velocity, additive);
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0001291E File Offset: 0x00010B1E
	public void Flip()
	{
		this.m_enemyController.CharacterCorgi.Flip(false, false);
	}

	// Token: 0x0600024B RID: 587 RVA: 0x00012934 File Offset: 0x00010B34
	public Vector3 GetRelativeSpawnPositionAtIndex(int spawnPosIndex, bool matchFacing)
	{
		Vector3 vector = this.EnemyController.SpawnPositionController.GetSpawnPosition(spawnPosIndex);
		vector = this.EnemyController.transform.InverseTransformPoint(vector);
		vector *= this.EnemyController.BaseScaleToOffsetWith;
		if (!this.EnemyController.IsFacingRight && matchFacing)
		{
			vector.x = -vector.x;
		}
		return vector;
	}

	// Token: 0x0600024C RID: 588 RVA: 0x00012998 File Offset: 0x00010B98
	public Vector3 GetAbsoluteSpawnPositionAtIndex(int spawnPosIndex, bool matchFacing)
	{
		Vector3 spawnPosition = this.EnemyController.SpawnPositionController.GetSpawnPosition(spawnPosIndex);
		if (!this.EnemyController.IsFacingRight && matchFacing)
		{
			spawnPosition.x = -spawnPosition.x;
		}
		return spawnPosition;
	}

	// Token: 0x0600024D RID: 589 RVA: 0x000129D8 File Offset: 0x00010BD8
	public void RemoveStatusEffects(bool includeCommanderBuffs)
	{
		if (includeCommanderBuffs)
		{
			this.EnemyController.StatusEffectController.StopAllStatusEffects(false);
			return;
		}
		foreach (StatusEffectType statusEffectType in StatusEffectType_RL.TypeArray)
		{
			if (!StatusEffect_EV.COMMANDER_STATUS_EFFECT_ARRAY.Contains(statusEffectType))
			{
				this.EnemyController.StatusEffectController.StopStatusEffect(statusEffectType, false);
			}
		}
	}

	// Token: 0x0600024E RID: 590 RVA: 0x00012A31 File Offset: 0x00010C31
	protected void SetAttackingWithContactDamage(bool enable, float delay = 0.1f)
	{
		if (this.m_setContactDamageCoroutine != null)
		{
			base.StopCoroutine(this.m_setContactDamageCoroutine);
			this.m_setContactDamageCoroutine = null;
		}
		if (!enable)
		{
			this.EnemyController.AttackingWithContactDamage = enable;
			return;
		}
		this.m_setContactDamageCoroutine = base.StartCoroutine(this.AttackingWithContactDamageCoroutine(delay));
	}

	// Token: 0x0600024F RID: 591 RVA: 0x00012A71 File Offset: 0x00010C71
	protected IEnumerator AttackingWithContactDamageCoroutine(float delay)
	{
		delay += Time.time;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.EnemyController.AttackingWithContactDamage = true;
		yield break;
	}

	// Token: 0x06000250 RID: 592 RVA: 0x00012A87 File Offset: 0x00010C87
	protected void StopProjectile(ref Projectile_RL projectile)
	{
		if (projectile && projectile.isActiveAndEnabled && projectile.OwnerController == this.m_enemyController)
		{
			projectile.FlagForDestruction(null);
		}
		projectile = null;
	}

	// Token: 0x06000251 RID: 593 RVA: 0x00012ABA File Offset: 0x00010CBA
	public IEnumerator WaitUntilAnimChanged(string animatorBoolName, int animLayer)
	{
		if (this.m_waitAnimChangedYield == null)
		{
			this.m_waitAnimChangedYield = new WaitUntilAnimChanged_Yield(this.Animator, animatorBoolName, (AnimationLayer)animLayer);
		}
		else
		{
			this.m_waitAnimChangedYield.CreateNew(this.Animator, animatorBoolName, (AnimationLayer)animLayer);
		}
		yield return this.m_waitAnimChangedYield;
		yield break;
	}

	// Token: 0x06000252 RID: 594 RVA: 0x00012AD7 File Offset: 0x00010CD7
	public IEnumerator WaitUntilAnimComplete(int animLayer)
	{
		if (this.m_waitAnimCompleteYield == null)
		{
			this.m_waitAnimCompleteYield = new WaitUntilAnimComplete_Yield(this.Animator, animLayer);
		}
		else
		{
			this.m_waitAnimCompleteYield.CreateNew(this.Animator, animLayer);
		}
		yield return this.m_waitAnimCompleteYield;
		yield break;
	}

	// Token: 0x06000253 RID: 595 RVA: 0x00012AED File Offset: 0x00010CED
	public IEnumerator WaitUntilIsGrounded()
	{
		while (!this.EnemyController.IsGrounded)
		{
			yield return null;
		}
		this.EnemyController.ResetTurnTrigger();
		yield break;
	}

	// Token: 0x06000254 RID: 596 RVA: 0x00012AFC File Offset: 0x00010CFC
	protected IEnumerator Wait(float seconds, bool unscaledTime = false)
	{
		if (this.EnemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Speed))
		{
			seconds /= 1.25f;
		}
		if (this.m_waitYield == null)
		{
			this.m_waitYield = new WaitRL_Yield(seconds, unscaledTime);
		}
		else
		{
			this.m_waitYield.CreateNew(seconds, unscaledTime);
		}
		if (this.IsPaused)
		{
			this.m_waitYield.Pause();
		}
		yield return this.m_waitYield;
		yield break;
	}

	// Token: 0x06000255 RID: 597 RVA: 0x00012B19 File Offset: 0x00010D19
	protected IEnumerator Wait(float minSeconds, float maxSeconds, bool unscaledTime = false)
	{
		float waitTime = UnityEngine.Random.Range(minSeconds, maxSeconds);
		if (this.m_waitYield == null)
		{
			this.m_waitYield = new WaitRL_Yield(waitTime, unscaledTime);
		}
		else
		{
			this.m_waitYield.CreateNew(waitTime, unscaledTime);
		}
		yield return this.m_waitYield;
		yield break;
	}

	// Token: 0x0400065C RID: 1628
	private const string ANIM_SPEED_STRING = "Anim_Speed";

	// Token: 0x0400065D RID: 1629
	private const string ABILITY_ANIM_SPEED_STRING = "Ability_Anim_Speed";

	// Token: 0x0400065E RID: 1630
	private const string DEATH_SCREEN_FLASH_AUDIO_PATH = "event:/SFX/Enemies/sfx_spellswordBoss_death_flash";

	// Token: 0x0400065F RID: 1631
	protected static readonly int NEUTRAL_STATE = Animator.StringToHash("Neutral");

	// Token: 0x04000660 RID: 1632
	private Animator m_animator;

	// Token: 0x04000661 RID: 1633
	private LogicController m_logicController;

	// Token: 0x04000662 RID: 1634
	private EnemyController m_enemyController;

	// Token: 0x04000663 RID: 1635
	private Coroutine m_setContactDamageCoroutine;

	// Token: 0x04000664 RID: 1636
	private bool m_isInitialized;

	// Token: 0x04000665 RID: 1637
	private string m_description = string.Empty;

	// Token: 0x04000666 RID: 1638
	[NonSerialized]
	protected string[] m_projectileNameArray;

	// Token: 0x04000668 RID: 1640
	[NonSerialized]
	public int DefaultAnimationLayer;

	// Token: 0x04000669 RID: 1641
	private WaitUntilAnimChanged_Yield m_waitAnimChangedYield;

	// Token: 0x0400066A RID: 1642
	private WaitUntilAnimComplete_Yield m_waitAnimCompleteYield;

	// Token: 0x0400066B RID: 1643
	private WaitRL_Yield m_waitYield;
}
