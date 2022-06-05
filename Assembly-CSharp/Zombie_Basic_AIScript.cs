using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x02000154 RID: 340
public class Zombie_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000B75 RID: 2933 RVA: 0x00022C24 File Offset: 0x00020E24
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"ZombiePassBoltMinibossProjectile",
			"ZombieLungeProjectile"
		};
	}

	// Token: 0x17000655 RID: 1621
	// (get) Token: 0x06000B76 RID: 2934 RVA: 0x00022C42 File Offset: 0x00020E42
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.35f, 0.55f);
		}
	}

	// Token: 0x17000656 RID: 1622
	// (get) Token: 0x06000B77 RID: 2935 RVA: 0x00022C53 File Offset: 0x00020E53
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.5f, 1.05f);
		}
	}

	// Token: 0x17000657 RID: 1623
	// (get) Token: 0x06000B78 RID: 2936 RVA: 0x00022C64 File Offset: 0x00020E64
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.5f, 0.5f);
		}
	}

	// Token: 0x17000658 RID: 1624
	// (get) Token: 0x06000B79 RID: 2937 RVA: 0x00022C75 File Offset: 0x00020E75
	protected virtual float DigDownAnimSpeed
	{
		get
		{
			return 1.7f;
		}
	}

	// Token: 0x17000659 RID: 1625
	// (get) Token: 0x06000B7A RID: 2938 RVA: 0x00022C7C File Offset: 0x00020E7C
	protected virtual float DigUpAnimSpeed
	{
		get
		{
			return 1.7f;
		}
	}

	// Token: 0x1700065A RID: 1626
	// (get) Token: 0x06000B7B RID: 2939 RVA: 0x00022C83 File Offset: 0x00020E83
	protected virtual float DigDown_IdleDelay
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x1700065B RID: 1627
	// (get) Token: 0x06000B7C RID: 2940 RVA: 0x00022C8A File Offset: 0x00020E8A
	protected virtual float DigUp_IdleDelay
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x1700065C RID: 1628
	// (get) Token: 0x06000B7D RID: 2941 RVA: 0x00022C91 File Offset: 0x00020E91
	protected virtual float m_tunnel_moveSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700065D RID: 1629
	// (get) Token: 0x06000B7E RID: 2942 RVA: 0x00022C98 File Offset: 0x00020E98
	protected virtual Vector2 swing_Dash_AttackSpeed
	{
		get
		{
			return new Vector2(14f, 0f);
		}
	}

	// Token: 0x1700065E RID: 1630
	// (get) Token: 0x06000B7F RID: 2943 RVA: 0x00022CA9 File Offset: 0x00020EA9
	protected virtual float m_swing_Dash_AttackTime
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x1700065F RID: 1631
	// (get) Token: 0x06000B80 RID: 2944 RVA: 0x00022CB0 File Offset: 0x00020EB0
	protected virtual float m_swing_AttackCD
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x17000660 RID: 1632
	// (get) Token: 0x06000B81 RID: 2945 RVA: 0x00022CB7 File Offset: 0x00020EB7
	// (set) Token: 0x06000B82 RID: 2946 RVA: 0x00022CBF File Offset: 0x00020EBF
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

	// Token: 0x06000B83 RID: 2947 RVA: 0x00022CC8 File Offset: 0x00020EC8
	private void Awake()
	{
		this.m_onZombieHit = new Action<object, EventArgs>(this.OnZombieHit);
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x00022CDC File Offset: 0x00020EDC
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

	// Token: 0x06000B85 RID: 2949 RVA: 0x00022D6C File Offset: 0x00020F6C
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

	// Token: 0x06000B86 RID: 2950 RVA: 0x00022E02 File Offset: 0x00021002
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			this.ResetZombie();
		}
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x00022E14 File Offset: 0x00021014
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

	// Token: 0x06000B88 RID: 2952 RVA: 0x00022E6A File Offset: 0x0002106A
	private void OnZombieHit(object sender, EventArgs args)
	{
		this.m_digDownDelay = Time.time + 3f;
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x00022E80 File Offset: 0x00021080
	private void StartUndergroundEffect()
	{
		if (this.m_undergroundEffect)
		{
			this.m_undergroundEffect.Stop(EffectStopType.Immediate);
		}
		this.m_undergroundEffect = EffectManager.PlayEffect(base.gameObject, base.Animator, "EnemyHiddenUnderground_Effect", base.EnemyController.transform.position, -1f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		this.m_undergroundEffect.transform.SetParent(base.EnemyController.transform);
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x00022EF4 File Offset: 0x000210F4
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

	// Token: 0x06000B8B RID: 2955 RVA: 0x00022FE2 File Offset: 0x000211E2
	public override IEnumerator Idle()
	{
		if (!this.IsHidden && Time.time >= this.m_digDownDelay)
		{
			yield return this.DigDown();
		}
		yield return base.Idle();
		yield break;
	}

	// Token: 0x06000B8C RID: 2956 RVA: 0x00022FF1 File Offset: 0x000211F1
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

	// Token: 0x06000B8D RID: 2957 RVA: 0x00023000 File Offset: 0x00021200
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

	// Token: 0x06000B8E RID: 2958 RVA: 0x0002300F File Offset: 0x0002120F
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

	// Token: 0x06000B8F RID: 2959 RVA: 0x0002301E File Offset: 0x0002121E
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

	// Token: 0x06000B90 RID: 2960 RVA: 0x00023030 File Offset: 0x00021230
	public override void OnLBCompleteOrCancelled()
	{
		base.EnemyController.BaseKnockbackDefense = (float)base.EnemyController.EnemyData.KnockbackDefence;
		base.EnemyController.LockFlip = false;
		base.EnemyController.DisableFriction = false;
		this.StopPersistentCoroutine(this.m_digMoveEffectCoroutine);
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x00023083 File Offset: 0x00021283
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

	// Token: 0x06000B92 RID: 2962 RVA: 0x00023092 File Offset: 0x00021292
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

	// Token: 0x06000B93 RID: 2963 RVA: 0x000230AF File Offset: 0x000212AF
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

	// Token: 0x06000B94 RID: 2964 RVA: 0x000230BE File Offset: 0x000212BE
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

	// Token: 0x06000B95 RID: 2965 RVA: 0x000230CD File Offset: 0x000212CD
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

	// Token: 0x06000B96 RID: 2966 RVA: 0x000230DC File Offset: 0x000212DC
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

	// Token: 0x17000661 RID: 1633
	// (get) Token: 0x06000B97 RID: 2967 RVA: 0x000230EB File Offset: 0x000212EB
	protected virtual float Lunge_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000662 RID: 1634
	// (get) Token: 0x06000B98 RID: 2968 RVA: 0x000230F2 File Offset: 0x000212F2
	protected virtual float Lunge_TellHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000663 RID: 1635
	// (get) Token: 0x06000B99 RID: 2969 RVA: 0x000230F9 File Offset: 0x000212F9
	protected virtual float Lunge_TellHold_Duration
	{
		get
		{
			return 0.7f;
		}
	}

	// Token: 0x17000664 RID: 1636
	// (get) Token: 0x06000B9A RID: 2970 RVA: 0x00023100 File Offset: 0x00021300
	protected virtual float Lunge_TellHold_DurationShort
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x17000665 RID: 1637
	// (get) Token: 0x06000B9B RID: 2971 RVA: 0x00023107 File Offset: 0x00021307
	protected virtual float Lunge_AttackIntro_AnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000666 RID: 1638
	// (get) Token: 0x06000B9C RID: 2972 RVA: 0x0002310E File Offset: 0x0002130E
	protected virtual float Lunge_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000667 RID: 1639
	// (get) Token: 0x06000B9D RID: 2973 RVA: 0x00023115 File Offset: 0x00021315
	protected virtual float Lunge_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000668 RID: 1640
	// (get) Token: 0x06000B9E RID: 2974 RVA: 0x0002311C File Offset: 0x0002131C
	protected virtual float Lunge_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000669 RID: 1641
	// (get) Token: 0x06000B9F RID: 2975 RVA: 0x00023123 File Offset: 0x00021323
	protected virtual float Lunge_Exit_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700066A RID: 1642
	// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x0002312A File Offset: 0x0002132A
	protected virtual float Lunge_Exit_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700066B RID: 1643
	// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x00023131 File Offset: 0x00021331
	protected virtual float Lunge_Exit_ForceIdle
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700066C RID: 1644
	// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x00023138 File Offset: 0x00021338
	protected virtual float Lunge_Exit_AttackCD
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700066D RID: 1645
	// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x0002313F File Offset: 0x0002133F
	protected virtual float Lunge_Count
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700066E RID: 1646
	// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x00023146 File Offset: 0x00021346
	protected virtual float Delay_Between_Lunges
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700066F RID: 1647
	// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x0002314D File Offset: 0x0002134D
	protected virtual Vector2 Lunge_Power
	{
		get
		{
			return new Vector2(20f, 9f);
		}
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x0002315E File Offset: 0x0002135E
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

	// Token: 0x06000BA7 RID: 2983 RVA: 0x0002316D File Offset: 0x0002136D
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

	// Token: 0x06000BA8 RID: 2984 RVA: 0x00023198 File Offset: 0x00021398
	private void FadeOutColorTrail()
	{
		if (this.m_colorTrailEffect && this.m_colorTrailEffect.isActiveAndEnabled && this.m_colorTrailEffect.Source == base.EnemyController.gameObject)
		{
			this.m_colorTrailEffect.transform.SetParent(null, true);
			base.StartCoroutine(this.FadeOutColorTrailCoroutine(0.1f));
		}
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x00023200 File Offset: 0x00021400
	private void DisableEnemiesCensoredEffect()
	{
		if (this.m_enemiesCensoredEffect && this.m_enemiesCensoredEffect.isActiveAndEnabled && this.m_enemiesCensoredEffect.Source == base.EnemyController.gameObject)
		{
			this.m_enemiesCensoredEffect.Stop(EffectStopType.Immediate);
		}
		this.m_enemiesCensoredEffect = null;
	}

	// Token: 0x06000BAA RID: 2986 RVA: 0x00023257 File Offset: 0x00021457
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

	// Token: 0x06000BAB RID: 2987 RVA: 0x0002326D File Offset: 0x0002146D
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

	// Token: 0x06000BAC RID: 2988 RVA: 0x0002327C File Offset: 0x0002147C
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

	// Token: 0x04001004 RID: 4100
	private const float HIDE_DELAY = 3f;

	// Token: 0x04001005 RID: 4101
	private const float RISE_DELAY = 0.5f;

	// Token: 0x04001006 RID: 4102
	private float m_digDownDelay;

	// Token: 0x04001007 RID: 4103
	private float m_digUpDelay;

	// Token: 0x04001008 RID: 4104
	private Coroutine m_digMoveEffectCoroutine;

	// Token: 0x04001009 RID: 4105
	private EventInstance m_tunnelAudioEventInstance;

	// Token: 0x0400100A RID: 4106
	private BaseEffect m_colorTrailEffect;

	// Token: 0x0400100B RID: 4107
	private BaseEffect m_enemiesCensoredEffect;

	// Token: 0x0400100C RID: 4108
	private BaseEffect m_undergroundEffect;

	// Token: 0x0400100D RID: 4109
	private Action<object, EventArgs> m_onZombieHit;

	// Token: 0x0400100E RID: 4110
	protected float m_swing_TellIntro_AnimationSpeed = 1f;

	// Token: 0x0400100F RID: 4111
	protected float m_swing_TellHold_AnimationSpeed = 1f;

	// Token: 0x04001010 RID: 4112
	protected float m_swing_TellIntroAndHold_Delay;

	// Token: 0x04001011 RID: 4113
	protected float m_swing_AttackIntro_AnimationSpeed = 0.5f;

	// Token: 0x04001012 RID: 4114
	protected float m_swing_AttackIntro_Delay = 0.35f;

	// Token: 0x04001013 RID: 4115
	protected float m_swing_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x04001014 RID: 4116
	protected float m_swing_AttackHold_Delay;

	// Token: 0x04001015 RID: 4117
	protected float m_swing_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04001016 RID: 4118
	protected float m_swing_Exit_Delay;

	// Token: 0x04001017 RID: 4119
	protected float m_swing_Exit_ForceIdle = 0.15f;

	// Token: 0x04001018 RID: 4120
	private bool m_isHidden;

	// Token: 0x04001019 RID: 4121
	protected float m_digDash_TellIntro_AnimationSpeed = 1f;

	// Token: 0x0400101A RID: 4122
	protected float m_digDash_TellHold_AnimationSpeed = 1f;

	// Token: 0x0400101B RID: 4123
	protected float m_digDash_TellIntroAndHold_Delay;

	// Token: 0x0400101C RID: 4124
	protected float m_digDash_AttackIntro_AnimationSpeed = 0.5f;

	// Token: 0x0400101D RID: 4125
	protected float m_digDash_AttackIntro_Delay = 0.35f;

	// Token: 0x0400101E RID: 4126
	protected float m_digDash_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x0400101F RID: 4127
	protected float m_digDash_AttackHold_Delay;

	// Token: 0x04001020 RID: 4128
	protected float m_digDash_Attack_RevealDelay = 0.65f;

	// Token: 0x04001021 RID: 4129
	protected float m_digDash_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04001022 RID: 4130
	protected float m_digDash_Exit_Delay = 0.65f;

	// Token: 0x04001023 RID: 4131
	protected float m_digDash_Attack_Duration = 1.25f;

	// Token: 0x04001024 RID: 4132
	protected float m_digDash_Attack_MoveSpeedOverride = 12f;

	// Token: 0x04001025 RID: 4133
	protected float m_digDash_Exit_ForceIdle = 0.15f;

	// Token: 0x04001026 RID: 4134
	protected float m_digDash_AttackCD = 1f;

	// Token: 0x04001027 RID: 4135
	protected const string LUNGE_TELL_INTRO = "Lunge_Tell_Intro";

	// Token: 0x04001028 RID: 4136
	protected const string LUNGE_TELL_HOLD = "Lunge_Tell_Hold";

	// Token: 0x04001029 RID: 4137
	protected const string LUNGE_ATTACK_INTRO = "Lunge_Attack_Intro";

	// Token: 0x0400102A RID: 4138
	protected const string LUNGE_ATTACK_HOLD = "Lunge_Attack_Hold";

	// Token: 0x0400102B RID: 4139
	protected const string LUNGE_EXIT = "Lunge_Exit";

	// Token: 0x0400102C RID: 4140
	protected const string LUNGE_PROJECTILE = "ZombieLungeProjectile";
}
