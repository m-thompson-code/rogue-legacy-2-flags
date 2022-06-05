using System;
using System.Collections;
using System.Linq;
using RLAudio;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public abstract class BaseAIScript : MonoBehaviour, IHasProjectileNameArray, IAudioEventEmitter
{
	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000269 RID: 617 RVA: 0x00003F6C File Offset: 0x0000216C
	protected virtual Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.35f, 0.75f);
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x0600026A RID: 618 RVA: 0x00003F7D File Offset: 0x0000217D
	protected virtual Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.4f, 0.9f);
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x0600026B RID: 619 RVA: 0x00003F7D File Offset: 0x0000217D
	protected virtual Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.4f, 0.9f);
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x0600026C RID: 620 RVA: 0x00003F8E File Offset: 0x0000218E
	protected virtual Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(3f, 8.5f);
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x0600026D RID: 621 RVA: 0x00003F9F File Offset: 0x0000219F
	protected virtual Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(1.5f, 6f);
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x0600026E RID: 622 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float IdleAnimSpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x0600026F RID: 623 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float WalkAnimSpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x06000270 RID: 624 RVA: 0x00003FB0 File Offset: 0x000021B0
	protected virtual float ForcedIdleDuration_IfReversingDirection
	{
		get
		{
			return 0.175f;
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x06000271 RID: 625 RVA: 0x00003FB7 File Offset: 0x000021B7
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

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06000272 RID: 626 RVA: 0x00003FE0 File Offset: 0x000021E0
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

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x06000273 RID: 627 RVA: 0x00004006 File Offset: 0x00002206
	// (set) Token: 0x06000274 RID: 628 RVA: 0x0000400E File Offset: 0x0000220E
	public bool IsPaused { get; private set; }

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x06000275 RID: 629 RVA: 0x00004017 File Offset: 0x00002217
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x06000276 RID: 630 RVA: 0x0000401F File Offset: 0x0000221F
	public LogicController LogicController
	{
		get
		{
			return this.m_logicController;
		}
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x06000277 RID: 631 RVA: 0x00004027 File Offset: 0x00002227
	public EnemyController EnemyController
	{
		get
		{
			return this.m_enemyController;
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x06000278 RID: 632 RVA: 0x0000402F File Offset: 0x0000222F
	public GameObject Target
	{
		get
		{
			return this.m_enemyController.Target;
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x06000279 RID: 633 RVA: 0x0000403C File Offset: 0x0000223C
	public BaseCharacterController TargetController
	{
		get
		{
			return this.m_enemyController.TargetController;
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x0600027A RID: 634 RVA: 0x00004049 File Offset: 0x00002249
	public bool IsInitialized
	{
		get
		{
			return this.m_isInitialized;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x0600027B RID: 635 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public virtual bool ForceDeathAnimation
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600027C RID: 636
	protected abstract void InitializeProjectileNameArray();

	// Token: 0x0600027D RID: 637 RVA: 0x0004F80C File Offset: 0x0004DA0C
	public virtual void Initialize(EnemyController enemyController)
	{
		this.m_enemyController = enemyController;
		this.m_animator = this.m_enemyController.Animator;
		this.m_logicController = this.m_enemyController.LogicController;
		base.transform.localPosition = Vector3.zero;
		this.m_isInitialized = true;
	}

	// Token: 0x0600027E RID: 638 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void OnDisable()
	{
	}

	// Token: 0x0600027F RID: 639 RVA: 0x00002FCA File Offset: 0x000011CA
	public virtual void OnEnemyActivated()
	{
	}

	// Token: 0x06000280 RID: 640 RVA: 0x00004051 File Offset: 0x00002251
	public virtual void Pause()
	{
		this.IsPaused = true;
		if (this.m_waitYield != null)
		{
			this.m_waitYield.Pause();
		}
	}

	// Token: 0x06000281 RID: 641 RVA: 0x0000406D File Offset: 0x0000226D
	public virtual void Unpause()
	{
		this.IsPaused = false;
		if (this.m_waitYield != null)
		{
			this.m_waitYield.Unpause();
		}
	}

	// Token: 0x06000282 RID: 642 RVA: 0x0004F85C File Offset: 0x0004DA5C
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

	// Token: 0x06000283 RID: 643 RVA: 0x00004089 File Offset: 0x00002289
	public virtual void ResetScript()
	{
		this.LogicController.ForceExecuteLogicBlockName_OnceOnly = null;
		this.OnLBCompleteOrCancelled();
	}

	// Token: 0x06000284 RID: 644 RVA: 0x0000409D File Offset: 0x0000229D
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

	// Token: 0x06000285 RID: 645 RVA: 0x000040AC File Offset: 0x000022AC
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

	// Token: 0x06000286 RID: 646 RVA: 0x000040C2 File Offset: 0x000022C2
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

	// Token: 0x06000287 RID: 647 RVA: 0x000040D1 File Offset: 0x000022D1
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

	// Token: 0x06000288 RID: 648 RVA: 0x000040E0 File Offset: 0x000022E0
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

	// Token: 0x06000289 RID: 649 RVA: 0x000040EF File Offset: 0x000022EF
	protected virtual void PlayDeathAnimAudio()
	{
		AudioManager.PlayOneShot(this, "event:/SFX/Enemies/sfx_spellswordBoss_death_flash", base.gameObject.transform.position);
	}

	// Token: 0x0600028A RID: 650 RVA: 0x0000410C File Offset: 0x0000230C
	public virtual IEnumerator SpawnAnim()
	{
		yield break;
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00004114 File Offset: 0x00002314
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

	// Token: 0x0600028C RID: 652 RVA: 0x00004148 File Offset: 0x00002348
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

	// Token: 0x0600028D RID: 653 RVA: 0x00004174 File Offset: 0x00002374
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

	// Token: 0x0600028E RID: 654 RVA: 0x000041A0 File Offset: 0x000023A0
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

	// Token: 0x0600028F RID: 655 RVA: 0x0004F8E4 File Offset: 0x0004DAE4
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

	// Token: 0x06000290 RID: 656 RVA: 0x000041BD File Offset: 0x000023BD
	public void SetVelocity(Vector2 velocity, bool additive)
	{
		this.m_enemyController.SetVelocity(velocity.x, velocity.y, additive);
	}

	// Token: 0x06000291 RID: 657 RVA: 0x000041D7 File Offset: 0x000023D7
	public void SetVelocity(float x, float y, bool additive)
	{
		this.m_enemyController.SetVelocity(x, y, additive);
	}

	// Token: 0x06000292 RID: 658 RVA: 0x000041E7 File Offset: 0x000023E7
	public void SetVelocityX(float velocity, bool additive)
	{
		this.m_enemyController.SetVelocityX(velocity, additive);
	}

	// Token: 0x06000293 RID: 659 RVA: 0x000041F6 File Offset: 0x000023F6
	public void SetVelocityY(float velocity, bool additive)
	{
		this.m_enemyController.SetVelocityY(velocity, additive);
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00004205 File Offset: 0x00002405
	public void Flip()
	{
		this.m_enemyController.CharacterCorgi.Flip(false, false);
	}

	// Token: 0x06000295 RID: 661 RVA: 0x0004F938 File Offset: 0x0004DB38
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

	// Token: 0x06000296 RID: 662 RVA: 0x0004F99C File Offset: 0x0004DB9C
	public Vector3 GetAbsoluteSpawnPositionAtIndex(int spawnPosIndex, bool matchFacing)
	{
		Vector3 spawnPosition = this.EnemyController.SpawnPositionController.GetSpawnPosition(spawnPosIndex);
		if (!this.EnemyController.IsFacingRight && matchFacing)
		{
			spawnPosition.x = -spawnPosition.x;
		}
		return spawnPosition;
	}

	// Token: 0x06000297 RID: 663 RVA: 0x0004F9DC File Offset: 0x0004DBDC
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

	// Token: 0x06000298 RID: 664 RVA: 0x00004219 File Offset: 0x00002419
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

	// Token: 0x06000299 RID: 665 RVA: 0x00004259 File Offset: 0x00002459
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

	// Token: 0x0600029A RID: 666 RVA: 0x0000426F File Offset: 0x0000246F
	protected void StopProjectile(ref Projectile_RL projectile)
	{
		if (projectile && projectile.isActiveAndEnabled && projectile.OwnerController == this.m_enemyController)
		{
			projectile.FlagForDestruction(null);
		}
		projectile = null;
	}

	// Token: 0x0600029B RID: 667 RVA: 0x000042A2 File Offset: 0x000024A2
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

	// Token: 0x0600029C RID: 668 RVA: 0x000042BF File Offset: 0x000024BF
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

	// Token: 0x0600029D RID: 669 RVA: 0x000042D5 File Offset: 0x000024D5
	public IEnumerator WaitUntilIsGrounded()
	{
		while (!this.EnemyController.IsGrounded)
		{
			yield return null;
		}
		this.EnemyController.ResetTurnTrigger();
		yield break;
	}

	// Token: 0x0600029E RID: 670 RVA: 0x000042E4 File Offset: 0x000024E4
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

	// Token: 0x0600029F RID: 671 RVA: 0x00004301 File Offset: 0x00002501
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

	// Token: 0x0400069D RID: 1693
	private const string ANIM_SPEED_STRING = "Anim_Speed";

	// Token: 0x0400069E RID: 1694
	private const string ABILITY_ANIM_SPEED_STRING = "Ability_Anim_Speed";

	// Token: 0x0400069F RID: 1695
	private const string DEATH_SCREEN_FLASH_AUDIO_PATH = "event:/SFX/Enemies/sfx_spellswordBoss_death_flash";

	// Token: 0x040006A0 RID: 1696
	protected static readonly int NEUTRAL_STATE = Animator.StringToHash("Neutral");

	// Token: 0x040006A1 RID: 1697
	private Animator m_animator;

	// Token: 0x040006A2 RID: 1698
	private LogicController m_logicController;

	// Token: 0x040006A3 RID: 1699
	private EnemyController m_enemyController;

	// Token: 0x040006A4 RID: 1700
	private Coroutine m_setContactDamageCoroutine;

	// Token: 0x040006A5 RID: 1701
	private bool m_isInitialized;

	// Token: 0x040006A6 RID: 1702
	private string m_description = string.Empty;

	// Token: 0x040006A7 RID: 1703
	[NonSerialized]
	protected string[] m_projectileNameArray;

	// Token: 0x040006A9 RID: 1705
	[NonSerialized]
	public int DefaultAnimationLayer;

	// Token: 0x040006AA RID: 1706
	private WaitUntilAnimChanged_Yield m_waitAnimChangedYield;

	// Token: 0x040006AB RID: 1707
	private WaitUntilAnimComplete_Yield m_waitAnimCompleteYield;

	// Token: 0x040006AC RID: 1708
	private WaitRL_Yield m_waitYield;
}
