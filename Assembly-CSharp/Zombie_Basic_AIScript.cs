using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x0200026E RID: 622
public class Zombie_Basic_AIScript : BaseAIScript
{
	// Token: 0x060011E0 RID: 4576 RVA: 0x000093A1 File Offset: 0x000075A1
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"ZombiePassBoltMinibossProjectile",
			"ZombieLungeProjectile"
		};
	}

	// Token: 0x17000877 RID: 2167
	// (get) Token: 0x060011E1 RID: 4577 RVA: 0x000093BF File Offset: 0x000075BF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.35f, 0.55f);
		}
	}

	// Token: 0x17000878 RID: 2168
	// (get) Token: 0x060011E2 RID: 4578 RVA: 0x000093D0 File Offset: 0x000075D0
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.5f, 1.05f);
		}
	}

	// Token: 0x17000879 RID: 2169
	// (get) Token: 0x060011E3 RID: 4579 RVA: 0x0000746B File Offset: 0x0000566B
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.5f, 0.5f);
		}
	}

	// Token: 0x1700087A RID: 2170
	// (get) Token: 0x060011E4 RID: 4580 RVA: 0x000093E1 File Offset: 0x000075E1
	protected virtual float DigDownAnimSpeed
	{
		get
		{
			return 1.7f;
		}
	}

	// Token: 0x1700087B RID: 2171
	// (get) Token: 0x060011E5 RID: 4581 RVA: 0x000093E1 File Offset: 0x000075E1
	protected virtual float DigUpAnimSpeed
	{
		get
		{
			return 1.7f;
		}
	}

	// Token: 0x1700087C RID: 2172
	// (get) Token: 0x060011E6 RID: 4582 RVA: 0x0000456C File Offset: 0x0000276C
	protected virtual float DigDown_IdleDelay
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x1700087D RID: 2173
	// (get) Token: 0x060011E7 RID: 4583 RVA: 0x0000456C File Offset: 0x0000276C
	protected virtual float DigUp_IdleDelay
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x1700087E RID: 2174
	// (get) Token: 0x060011E8 RID: 4584 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_tunnel_moveSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700087F RID: 2175
	// (get) Token: 0x060011E9 RID: 4585 RVA: 0x000093E8 File Offset: 0x000075E8
	protected virtual Vector2 swing_Dash_AttackSpeed
	{
		get
		{
			return new Vector2(14f, 0f);
		}
	}

	// Token: 0x17000880 RID: 2176
	// (get) Token: 0x060011EA RID: 4586 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float m_swing_Dash_AttackTime
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000881 RID: 2177
	// (get) Token: 0x060011EB RID: 4587 RVA: 0x00005319 File Offset: 0x00003519
	protected virtual float m_swing_AttackCD
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x17000882 RID: 2178
	// (get) Token: 0x060011EC RID: 4588 RVA: 0x000093F9 File Offset: 0x000075F9
	// (set) Token: 0x060011ED RID: 4589 RVA: 0x00009401 File Offset: 0x00007601
	protected bool IsHidden
	{
		get
		{
			return this.m_isHidden;
		}
		set
		{
			this.m_isHidden = value;
		}
	}

	// Token: 0x060011EE RID: 4590 RVA: 0x0000940A File Offset: 0x0000760A
	private void Awake()
	{
		this.m_onZombieHit = new Action<object, EventArgs>(this.OnZombieHit);
	}

	// Token: 0x060011EF RID: 4591 RVA: 0x000807D8 File Offset: 0x0007E9D8
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.LogicController.DisableDamageDuringInitialDelay = false;
		this.ResetZombie();
		base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.AddListener(this.m_onZombieHit, false);
		base.EnemyController.StatusEffectController.SetAllStatusEffectsHidden(true);
		base.EnemyController.DisableOffscreenWarnings = true;
		base.EnemyController.InvisibleDuringSummonAnim = true;
		if (!this.m_tunnelAudioEventInstance.isValid())
		{
			this.m_tunnelAudioEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_zombie_burrow_move_loop", base.transform);
		}
	}

	// Token: 0x060011F0 RID: 4592 RVA: 0x00080868 File Offset: 0x0007EA68
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_tunnelAudioEventInstance.isValid())
		{
			this.m_tunnelAudioEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
		if (this.m_undergroundEffect)
		{
			EffectManager.StopEffect(this.m_undergroundEffect, EffectStopType.Immediate);
			this.m_undergroundEffect = null;
		}
		if (this.m_colorTrailEffect && this.m_colorTrailEffect.isActiveAndEnabled && this.m_colorTrailEffect.Source == base.EnemyController.gameObject)
		{
			this.m_colorTrailEffect.Stop(EffectStopType.Immediate);
		}
		this.DisableEnemiesCensoredEffect();
	}

	// Token: 0x060011F1 RID: 4593 RVA: 0x0000941E File Offset: 0x0000761E
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			this.ResetZombie();
		}
	}

	// Token: 0x060011F2 RID: 4594 RVA: 0x00080900 File Offset: 0x0007EB00
	private void OnDestroy()
	{
		if (!GameManager.IsApplicationClosing && base.EnemyController)
		{
			base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onZombieHit);
		}
		if (this.m_tunnelAudioEventInstance.isValid())
		{
			this.m_tunnelAudioEventInstance.release();
		}
	}

	// Token: 0x060011F3 RID: 4595 RVA: 0x0000942E File Offset: 0x0000762E
	private void OnZombieHit(object sender, EventArgs args)
	{
		this.m_digDownDelay = Time.time + 3f;
	}

	// Token: 0x060011F4 RID: 4596 RVA: 0x00080958 File Offset: 0x0007EB58
	private void StartUndergroundEffect()
	{
		if (this.m_undergroundEffect)
		{
			this.m_undergroundEffect.Stop(EffectStopType.Immediate);
		}
		this.m_undergroundEffect = EffectManager.PlayEffect(base.gameObject, base.Animator, "EnemyHiddenUnderground_Effect", base.EnemyController.transform.position, -1f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		this.m_undergroundEffect.transform.SetParent(base.EnemyController.transform);
	}

	// Token: 0x060011F5 RID: 4597 RVA: 0x000809CC File Offset: 0x0007EBCC
	private void ResetZombie()
	{
		if (!GameManager.IsApplicationClosing && base.IsInitialized)
		{
			bool flag = EffectManager.AnimatorEffectsDisabled(base.Animator);
			EffectManager.RemoveAnimatorFromDisableList(base.Animator);
			base.EnemyController.Animator.Play("Hidden", 0, 1f);
			base.EnemyController.Animator.Update(0f);
			this.StartUndergroundEffect();
			base.EnemyController.HitboxController.SetHitboxActiveState(HitboxType.Weapon, false);
			base.EnemyController.HitboxController.SetHitboxActiveState(HitboxType.Body, false);
			base.EnemyController.StatusEffectController.SetAllStatusEffectsHidden(true);
			base.EnemyController.DisableOffscreenWarnings = true;
			base.EnemyController.Visuals.SetActive(false);
			this.IsHidden = true;
			if (flag)
			{
				EffectManager.AddAnimatorToDisableList(base.Animator);
			}
			if (this.m_tunnelAudioEventInstance.isValid())
			{
				this.m_tunnelAudioEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
		}
	}

	// Token: 0x060011F6 RID: 4598 RVA: 0x00009441 File Offset: 0x00007641
	public override IEnumerator Idle()
	{
		if (!this.IsHidden && Time.time >= this.m_digDownDelay)
		{
			yield return this.DigDown();
		}
		yield return base.Idle();
		yield break;
	}

	// Token: 0x060011F7 RID: 4599 RVA: 0x00009450 File Offset: 0x00007650
	public override IEnumerator WalkTowards()
	{
		if (this.IsHidden)
		{
			if (Time.time >= this.m_digUpDelay && base.LogicController.CurrentRangeState == LogicState.Close)
			{
				yield return this.DigUp();
			}
			else if (this.m_tunnel_moveSpeed > 0f)
			{
				AudioManager.PlayAttached(this, this.m_tunnelAudioEventInstance, base.gameObject);
				this.m_digMoveEffectCoroutine = this.RunPersistentCoroutine(this.DigMoveEffectCoroutine());
				yield return base.WalkTowards();
				this.StopPersistentCoroutine(this.m_digMoveEffectCoroutine);
				AudioManager.Stop(this.m_tunnelAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			else
			{
				yield return base.Idle();
			}
		}
		else
		{
			yield return base.WalkTowards();
		}
		yield break;
	}

	// Token: 0x060011F8 RID: 4600 RVA: 0x0000945F File Offset: 0x0000765F
	public override IEnumerator WalkAway()
	{
		if (this.IsHidden && this.m_tunnel_moveSpeed > 0f)
		{
			AudioManager.PlayAttached(this, this.m_tunnelAudioEventInstance, base.gameObject);
			this.m_digMoveEffectCoroutine = this.RunPersistentCoroutine(this.DigMoveEffectCoroutine());
			yield return base.WalkTowards();
			this.StopPersistentCoroutine(this.m_digMoveEffectCoroutine);
			AudioManager.Stop(this.m_tunnelAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		else
		{
			yield return base.WalkAway();
		}
		yield break;
	}

	// Token: 0x060011F9 RID: 4601 RVA: 0x0000946E File Offset: 0x0000766E
	protected virtual IEnumerator DigDown()
	{
		base.SetVelocity(Vector2.zero, false);
		if (!this.IsHidden)
		{
			base.EnemyController.BaseKnockbackDefense = 99999f;
			base.EnemyController.HitboxController.SetHitboxActiveState(HitboxType.Weapon, false);
			this.IsHidden = true;
			base.SetVelocity(Vector2.zero, false);
			this.SetAnimationSpeedMultiplier(this.DigDownAnimSpeed);
			yield return this.ChangeAnimationState("DigDown");
			yield return base.WaitUntilAnimComplete(this.DefaultAnimationLayer);
			this.StartUndergroundEffect();
			base.EnemyController.StatusEffectController.SetAllStatusEffectsHidden(true);
			base.EnemyController.DisableOffscreenWarnings = true;
			base.EnemyController.BaseKnockbackDefense = (float)base.EnemyController.EnemyData.KnockbackDefence;
			base.EnemyController.HitboxController.SetHitboxActiveState(HitboxType.Body, false);
			base.EnemyController.Visuals.SetActive(false);
			if (this.DigDown_IdleDelay > 0f)
			{
				yield return base.Wait(this.DigDown_IdleDelay, false);
			}
			this.m_digUpDelay = Time.time + 0.5f;
			this.FadeOutColorTrail();
			this.DisableEnemiesCensoredEffect();
		}
		yield return this.Default_Attack_Cooldown(this.m_digDash_Exit_ForceIdle, this.m_digDash_AttackCD);
		yield break;
	}

	// Token: 0x060011FA RID: 4602 RVA: 0x0000947D File Offset: 0x0000767D
	protected virtual IEnumerator DigUp()
	{
		if (this.IsHidden)
		{
			if (TraitManager.IsTraitActive(TraitType.EnemiesCensored))
			{
				this.DisableEnemiesCensoredEffect();
				this.m_enemiesCensoredEffect = EnemiesCensored_Trait.ApplyCensoredEffect(base.EnemyController);
			}
			if (this.m_undergroundEffect)
			{
				EffectManager.StopEffect(this.m_undergroundEffect, EffectStopType.Gracefully);
				this.m_undergroundEffect = null;
			}
			base.EnemyController.HitboxController.SetHitboxActiveState(HitboxType.Body, true);
			this.IsHidden = false;
			base.EnemyController.Visuals.SetActive(true);
			base.EnemyController.BaseKnockbackDefense = 99999f;
			base.SetVelocity(Vector2.zero, false);
			float multiplier = this.DigUpAnimSpeed;
			if (!base.EnemyController.IsGrounded)
			{
				multiplier = 5f;
			}
			this.SetAnimationSpeedMultiplier(multiplier);
			yield return this.ChangeAnimationState("DigUp");
			yield return base.WaitUntilAnimComplete(this.DefaultAnimationLayer);
			base.EnemyController.StatusBarController.ResetPositionAndScale(base.EnemyController);
			base.EnemyController.StatusEffectController.SetAllStatusEffectsHidden(false);
			base.EnemyController.DisableOffscreenWarnings = false;
			base.EnemyController.BaseKnockbackDefense = (float)base.EnemyController.EnemyData.KnockbackDefence;
			base.EnemyController.HitboxController.SetHitboxActiveState(HitboxType.Weapon, true);
			if (this.DigUp_IdleDelay > 0f)
			{
				yield return base.Wait(this.DigUp_IdleDelay, false);
			}
			this.m_digDownDelay = Time.time + 3f;
			if (TraitManager.IsTraitActive(TraitType.ColorTrails))
			{
				this.FadeOutColorTrail();
				this.m_colorTrailEffect = EffectManager.PlayEffect(base.EnemyController.gameObject, base.EnemyController.Animator, "ColorTrails_Trait_Effect", base.EnemyController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
				TrailRenderer componentInChildren = this.m_colorTrailEffect.GetComponentInChildren<TrailRenderer>();
				componentInChildren.startColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
				componentInChildren.endColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
				componentInChildren.widthMultiplier = 1.5f;
				componentInChildren.time = 18f;
				this.m_colorTrailEffect.transform.SetParent(base.EnemyController.gameObject.transform, true);
			}
		}
		yield break;
	}

	// Token: 0x060011FB RID: 4603 RVA: 0x00080ABC File Offset: 0x0007ECBC
	public override void OnLBCompleteOrCancelled()
	{
		base.EnemyController.BaseKnockbackDefense = (float)base.EnemyController.EnemyData.KnockbackDefence;
		base.EnemyController.LockFlip = false;
		base.EnemyController.DisableFriction = false;
		this.StopPersistentCoroutine(this.m_digMoveEffectCoroutine);
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x060011FC RID: 4604 RVA: 0x0000948C File Offset: 0x0000768C
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Swing_Attack()
	{
		if (this.IsHidden)
		{
			yield return this.DigUp();
		}
		this.StopAndFaceTarget();
		yield return this.Default_TellIntroAndLoop("Swing_Tell_Intro", this.m_swing_TellIntro_AnimationSpeed, "Swing_Tell_Hold", this.m_swing_TellHold_AnimationSpeed, this.m_swing_TellIntroAndHold_Delay);
		yield return this.Default_Animation("Swing_Attack_Intro", this.m_swing_AttackIntro_AnimationSpeed, this.m_swing_AttackIntro_Delay, true);
		yield return this.Default_Animation("Swing_Attack_Hold", this.m_swing_AttackHold_AnimationSpeed, this.m_swing_AttackHold_Delay, false);
		this.Single_Action_Dash(this.swing_Dash_AttackSpeed.x, this.swing_Dash_AttackSpeed.y);
		if (this.m_swing_Dash_AttackTime > 0f)
		{
			yield return base.Wait(this.m_swing_Dash_AttackTime, false);
		}
		base.SetVelocityX(0f, false);
		yield return this.Default_Animation("Swing_Exit", this.m_swing_Exit_AnimationSpeed, this.m_swing_Exit_Delay, true);
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_swing_Exit_ForceIdle, this.m_swing_AttackCD);
		yield break;
	}

	// Token: 0x060011FD RID: 4605 RVA: 0x0000949B File Offset: 0x0000769B
	public override IEnumerator Default_Attack_Cooldown(float idleDuration, float attackCD)
	{
		float num = BurdenManager.GetBurdenStatGain(BurdenType.EnemyAggression);
		if (base.EnemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Speed) || base.EnemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_ArmorShred))
		{
			num += 0.25f;
		}
		num = Mathf.Max(1f - num, 0f);
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

	// Token: 0x060011FE RID: 4606 RVA: 0x000094B8 File Offset: 0x000076B8
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Tunnel_WalkTowards()
	{
		if (!this.IsHidden)
		{
			yield return this.DigDown();
		}
		base.EnemyController.BaseSpeed = this.m_tunnel_moveSpeed;
		AudioManager.PlayAttached(this, this.m_tunnelAudioEventInstance, base.gameObject);
		this.m_digMoveEffectCoroutine = this.RunPersistentCoroutine(this.DigMoveEffectCoroutine());
		yield return base.WalkTowards();
		this.StopPersistentCoroutine(this.m_digMoveEffectCoroutine);
		AudioManager.Stop(this.m_tunnelAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		base.EnemyController.ResetBaseValues();
		yield break;
	}

	// Token: 0x060011FF RID: 4607 RVA: 0x000094C7 File Offset: 0x000076C7
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Tunnel_WalkAway()
	{
		if (!this.IsHidden)
		{
			yield return this.DigDown();
		}
		base.EnemyController.BaseSpeed = this.m_tunnel_moveSpeed;
		AudioManager.PlayAttached(this, this.m_tunnelAudioEventInstance, base.gameObject);
		this.m_digMoveEffectCoroutine = this.RunPersistentCoroutine(this.DigMoveEffectCoroutine());
		yield return base.WalkAway();
		this.StopPersistentCoroutine(this.m_digMoveEffectCoroutine);
		AudioManager.Stop(this.m_tunnelAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		base.EnemyController.ResetBaseValues();
		yield break;
	}

	// Token: 0x06001200 RID: 4608 RVA: 0x000094D6 File Offset: 0x000076D6
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[MinibossEnemy]
	public IEnumerator Dig_Dash()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.FollowOffset = Vector3.zero;
		base.EnemyController.BaseTurnSpeed = 900f;
		base.EnemyController.FollowTarget = true;
		base.EnemyController.LockFlip = false;
		if (this.IsHidden)
		{
			yield return this.DigUp();
		}
		if (this.m_digDash_Attack_RevealDelay > 0f)
		{
			yield return base.Wait(this.m_digDash_Attack_RevealDelay, false);
		}
		yield return this.Default_TellIntroAndLoop("Swing_Tell_Intro", this.m_digDash_TellIntro_AnimationSpeed, "Swing_Tell_Hold", this.m_digDash_TellHold_AnimationSpeed, this.m_digDash_TellIntroAndHold_Delay);
		yield return this.Default_Animation("Swing_Attack_Intro", this.m_digDash_AttackIntro_AnimationSpeed, this.m_digDash_AttackIntro_Delay, true);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_digDash_Attack_MoveSpeedOverride;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			this.FireProjectile("ZombiePassBoltMinibossProjectile", 0, true, 0f, 0f, true, true, true);
		}
		base.EnemyController.BaseTurnSpeed = 0f;
		base.EnemyController.FollowTarget = false;
		base.EnemyController.LockFlip = true;
		yield return this.Default_Animation("Swing_Attack_Hold", this.m_digDash_AttackHold_AnimationSpeed, this.m_digDash_AttackHold_Delay, false);
		if (this.m_digDash_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_digDash_Attack_Duration, false);
		}
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			this.FireProjectile("ZombiePassBoltMinibossProjectile", 0, true, 0f, 0f, true, true, true);
		}
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.BaseSpeed = base.EnemyController.EnemyData.Speed;
		yield return this.Default_Animation("Swing_Exit", this.m_digDash_Exit_AnimationSpeed, this.m_digDash_Exit_Delay, true);
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_digDash_Exit_ForceIdle, this.m_digDash_AttackCD);
		yield break;
	}

	// Token: 0x06001201 RID: 4609 RVA: 0x000094E5 File Offset: 0x000076E5
	private IEnumerator DigMoveEffectCoroutine()
	{
		float smokeDelayInterval = Time.time + 0.15f;
		for (;;)
		{
			if (Time.time >= smokeDelayInterval)
			{
				smokeDelayInterval = Time.time + 0.15f;
				EffectManager.PlayEffect(base.EnemyController.gameObject, null, "DustCloudTinyParticles_Effect", base.EnemyController.transform.localPosition, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x17000883 RID: 2179
	// (get) Token: 0x06001202 RID: 4610 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float Lunge_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000884 RID: 2180
	// (get) Token: 0x06001203 RID: 4611 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float Lunge_TellHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000885 RID: 2181
	// (get) Token: 0x06001204 RID: 4612 RVA: 0x00006CC8 File Offset: 0x00004EC8
	protected virtual float Lunge_TellHold_Duration
	{
		get
		{
			return 0.7f;
		}
	}

	// Token: 0x17000886 RID: 2182
	// (get) Token: 0x06001205 RID: 4613 RVA: 0x000050CB File Offset: 0x000032CB
	protected virtual float Lunge_TellHold_DurationShort
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x17000887 RID: 2183
	// (get) Token: 0x06001206 RID: 4614 RVA: 0x00004536 File Offset: 0x00002736
	protected virtual float Lunge_AttackIntro_AnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000888 RID: 2184
	// (get) Token: 0x06001207 RID: 4615 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float Lunge_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000889 RID: 2185
	// (get) Token: 0x06001208 RID: 4616 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float Lunge_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700088A RID: 2186
	// (get) Token: 0x06001209 RID: 4617 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float Lunge_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700088B RID: 2187
	// (get) Token: 0x0600120A RID: 4618 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float Lunge_Exit_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700088C RID: 2188
	// (get) Token: 0x0600120B RID: 4619 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float Lunge_Exit_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700088D RID: 2189
	// (get) Token: 0x0600120C RID: 4620 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float Lunge_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700088E RID: 2190
	// (get) Token: 0x0600120D RID: 4621 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float Lunge_Exit_AttackCD
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700088F RID: 2191
	// (get) Token: 0x0600120E RID: 4622 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float Lunge_Count
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000890 RID: 2192
	// (get) Token: 0x0600120F RID: 4623 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float Delay_Between_Lunges
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000891 RID: 2193
	// (get) Token: 0x06001210 RID: 4624 RVA: 0x000094F4 File Offset: 0x000076F4
	protected virtual Vector2 Lunge_Power
	{
		get
		{
			return new Vector2(20f, 9f);
		}
	}

	// Token: 0x06001211 RID: 4625 RVA: 0x00009505 File Offset: 0x00007705
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Lunge_Attack()
	{
		if (this.IsHidden)
		{
			yield return this.Idle();
			yield break;
		}
		int i = 0;
		while ((float)i < this.Lunge_Count)
		{
			base.EnemyController.LockFlip = false;
			this.StopAndFaceTarget();
			base.EnemyController.LockFlip = true;
			if (i == 0)
			{
				yield return this.Default_TellIntroAndLoop("Lunge_Tell_Intro", this.Lunge_TellIntro_AnimSpeed, "Lunge_Tell_Hold", this.Lunge_TellHold_AnimSpeed, this.Lunge_TellHold_Duration);
			}
			else
			{
				yield return this.Default_TellIntroAndLoop("Lunge_Tell_Intro", this.Lunge_TellIntro_AnimSpeed, "Lunge_Tell_Hold", this.Lunge_TellHold_AnimSpeed, this.Lunge_TellHold_DurationShort);
			}
			yield return this.Default_Animation("Lunge_Attack_Intro", this.Lunge_AttackIntro_AnimSpeed, this.Lunge_AttackIntro_Delay, true);
			base.EnemyController.DisableFriction = true;
			if (base.EnemyController.IsFacingRight)
			{
				base.SetVelocityX(this.Lunge_Power.x, false);
			}
			else
			{
				base.SetVelocityX(-this.Lunge_Power.x, false);
			}
			base.SetVelocityY(this.Lunge_Power.y, false);
			this.FireProjectile("ZombieLungeProjectile", 1, true, 0f, 1f, true, true, true);
			yield return this.Default_Animation("Lunge_Attack_Hold", this.Lunge_AttackHold_AnimSpeed, this.Lunge_AttackHold_Delay, true);
			base.EnemyController.DisableFriction = false;
			yield return base.WaitUntilIsGrounded();
			base.EnemyController.SetVelocityX(0f, false);
			if (this.Delay_Between_Lunges > 0f)
			{
				yield return base.Wait(this.Delay_Between_Lunges, false);
			}
			int num = i;
			i = num + 1;
		}
		yield return this.Default_Animation("Lunge_Exit", this.Lunge_Exit_AnimSpeed, this.Lunge_Exit_Duration, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.Lunge_Exit_ForceIdle, this.Lunge_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06001212 RID: 4626 RVA: 0x00009514 File Offset: 0x00007714
	public void Single_Action_Dash(float dashXVelocity, float dashYVelocity)
	{
		if (base.EnemyController.IsFacingRight)
		{
			base.SetVelocityX(dashXVelocity, false);
		}
		else
		{
			base.SetVelocityX(-dashXVelocity, false);
		}
		base.SetVelocityY(dashYVelocity, false);
	}

	// Token: 0x06001213 RID: 4627 RVA: 0x00080B10 File Offset: 0x0007ED10
	private void FadeOutColorTrail()
	{
		if (this.m_colorTrailEffect && this.m_colorTrailEffect.isActiveAndEnabled && this.m_colorTrailEffect.Source == base.EnemyController.gameObject)
		{
			this.m_colorTrailEffect.transform.SetParent(null, true);
			base.StartCoroutine(this.FadeOutColorTrailCoroutine(0.1f));
		}
	}

	// Token: 0x06001214 RID: 4628 RVA: 0x00080B78 File Offset: 0x0007ED78
	private void DisableEnemiesCensoredEffect()
	{
		if (this.m_enemiesCensoredEffect && this.m_enemiesCensoredEffect.isActiveAndEnabled && this.m_enemiesCensoredEffect.Source == base.EnemyController.gameObject)
		{
			this.m_enemiesCensoredEffect.Stop(EffectStopType.Immediate);
		}
		this.m_enemiesCensoredEffect = null;
	}

	// Token: 0x06001215 RID: 4629 RVA: 0x0000953E File Offset: 0x0000773E
	private IEnumerator FadeOutColorTrailCoroutine(float duration)
	{
		FadeOutTrailRenderer fadeOut = this.m_colorTrailEffect.GetComponent<FadeOutTrailRenderer>();
		float delay = Time.time + duration;
		while (Time.time < delay)
		{
			float opacity = (delay - Time.time) / duration;
			fadeOut.SetOpacity(opacity);
			yield return null;
		}
		fadeOut.SetOpacity(0f);
		this.m_colorTrailEffect.Stop(EffectStopType.Immediate);
		yield break;
	}

	// Token: 0x06001216 RID: 4630 RVA: 0x00009554 File Offset: 0x00007754
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ForceDigUpFast()
	{
		yield return this.DigUp();
		yield break;
	}

	// Token: 0x06001217 RID: 4631 RVA: 0x00080BD0 File Offset: 0x0007EDD0
	private void LateUpdate()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (base.EnemyController && this.IsHidden && !base.EnemyController.IsGrounded)
		{
			base.LogicController.StopAllLogic(false);
			base.LogicController.ForceExecuteLogicBlockName_OnceOnly = "ForceDigUpFast";
		}
	}

	// Token: 0x040014C0 RID: 5312
	private const float HIDE_DELAY = 3f;

	// Token: 0x040014C1 RID: 5313
	private const float RISE_DELAY = 0.5f;

	// Token: 0x040014C2 RID: 5314
	private float m_digDownDelay;

	// Token: 0x040014C3 RID: 5315
	private float m_digUpDelay;

	// Token: 0x040014C4 RID: 5316
	private Coroutine m_digMoveEffectCoroutine;

	// Token: 0x040014C5 RID: 5317
	private EventInstance m_tunnelAudioEventInstance;

	// Token: 0x040014C6 RID: 5318
	private BaseEffect m_colorTrailEffect;

	// Token: 0x040014C7 RID: 5319
	private BaseEffect m_enemiesCensoredEffect;

	// Token: 0x040014C8 RID: 5320
	private BaseEffect m_undergroundEffect;

	// Token: 0x040014C9 RID: 5321
	private Action<object, EventArgs> m_onZombieHit;

	// Token: 0x040014CA RID: 5322
	protected float m_swing_TellIntro_AnimationSpeed = 1f;

	// Token: 0x040014CB RID: 5323
	protected float m_swing_TellHold_AnimationSpeed = 1f;

	// Token: 0x040014CC RID: 5324
	protected float m_swing_TellIntroAndHold_Delay;

	// Token: 0x040014CD RID: 5325
	protected float m_swing_AttackIntro_AnimationSpeed = 0.5f;

	// Token: 0x040014CE RID: 5326
	protected float m_swing_AttackIntro_Delay = 0.35f;

	// Token: 0x040014CF RID: 5327
	protected float m_swing_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x040014D0 RID: 5328
	protected float m_swing_AttackHold_Delay;

	// Token: 0x040014D1 RID: 5329
	protected float m_swing_Exit_AnimationSpeed = 0.65f;

	// Token: 0x040014D2 RID: 5330
	protected float m_swing_Exit_Delay;

	// Token: 0x040014D3 RID: 5331
	protected float m_swing_Exit_ForceIdle = 0.15f;

	// Token: 0x040014D4 RID: 5332
	private bool m_isHidden;

	// Token: 0x040014D5 RID: 5333
	protected float m_digDash_TellIntro_AnimationSpeed = 1f;

	// Token: 0x040014D6 RID: 5334
	protected float m_digDash_TellHold_AnimationSpeed = 1f;

	// Token: 0x040014D7 RID: 5335
	protected float m_digDash_TellIntroAndHold_Delay;

	// Token: 0x040014D8 RID: 5336
	protected float m_digDash_AttackIntro_AnimationSpeed = 0.5f;

	// Token: 0x040014D9 RID: 5337
	protected float m_digDash_AttackIntro_Delay = 0.35f;

	// Token: 0x040014DA RID: 5338
	protected float m_digDash_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x040014DB RID: 5339
	protected float m_digDash_AttackHold_Delay;

	// Token: 0x040014DC RID: 5340
	protected float m_digDash_Attack_RevealDelay = 0.65f;

	// Token: 0x040014DD RID: 5341
	protected float m_digDash_Exit_AnimationSpeed = 0.65f;

	// Token: 0x040014DE RID: 5342
	protected float m_digDash_Exit_Delay = 0.65f;

	// Token: 0x040014DF RID: 5343
	protected float m_digDash_Attack_Duration = 1.25f;

	// Token: 0x040014E0 RID: 5344
	protected float m_digDash_Attack_MoveSpeedOverride = 12f;

	// Token: 0x040014E1 RID: 5345
	protected float m_digDash_Exit_ForceIdle = 0.15f;

	// Token: 0x040014E2 RID: 5346
	protected float m_digDash_AttackCD = 1f;

	// Token: 0x040014E3 RID: 5347
	protected const string LUNGE_TELL_INTRO = "Lunge_Tell_Intro";

	// Token: 0x040014E4 RID: 5348
	protected const string LUNGE_TELL_HOLD = "Lunge_Tell_Hold";

	// Token: 0x040014E5 RID: 5349
	protected const string LUNGE_ATTACK_INTRO = "Lunge_Attack_Intro";

	// Token: 0x040014E6 RID: 5350
	protected const string LUNGE_ATTACK_HOLD = "Lunge_Attack_Hold";

	// Token: 0x040014E7 RID: 5351
	protected const string LUNGE_EXIT = "Lunge_Exit";

	// Token: 0x040014E8 RID: 5352
	protected const string LUNGE_PROJECTILE = "ZombieLungeProjectile";
}
