using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200016C RID: 364
public class FlyingShield_Basic_AIScript : BaseAIScript
{
	// Token: 0x060009BB RID: 2491 RVA: 0x0000673E File Offset: 0x0000493E
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlyingShieldWaterProjectile",
			"FlyingShieldWaterExpertProjectile",
			"FlyingShieldWaterMinibossProjectile"
		};
	}

	// Token: 0x1700046C RID: 1132
	// (get) Token: 0x060009BC RID: 2492 RVA: 0x0000645C File Offset: 0x0000465C
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.7f, 1f);
		}
	}

	// Token: 0x1700046D RID: 1133
	// (get) Token: 0x060009BD RID: 2493 RVA: 0x00006275 File Offset: 0x00004475
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.8f, 1.1f);
		}
	}

	// Token: 0x1700046E RID: 1134
	// (get) Token: 0x060009BE RID: 2494 RVA: 0x00006275 File Offset: 0x00004475
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.8f, 1.1f);
		}
	}

	// Token: 0x1700046F RID: 1135
	// (get) Token: 0x060009BF RID: 2495 RVA: 0x00005FAA File Offset: 0x000041AA
	protected virtual float m_spinMove_TellIntro_AnimationSpeed
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x17000470 RID: 1136
	// (get) Token: 0x060009C0 RID: 2496 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float m_spinMove_TellIntro_EndDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000471 RID: 1137
	// (get) Token: 0x060009C1 RID: 2497 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinMove_TellHold_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000472 RID: 1138
	// (get) Token: 0x060009C2 RID: 2498 RVA: 0x00006764 File Offset: 0x00004964
	protected virtual float m_spinMove_TellHold_Duration
	{
		get
		{
			return 0.425f;
		}
	}

	// Token: 0x17000473 RID: 1139
	// (get) Token: 0x060009C3 RID: 2499 RVA: 0x00005FAA File Offset: 0x000041AA
	protected virtual float m_spinMove_AttackIntro_AnimationSpeed
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x060009C4 RID: 2500 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spinMove_AttackIntro_EndDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x060009C5 RID: 2501 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spinMove_Attack_FlameDrops
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x060009C6 RID: 2502 RVA: 0x0000676B File Offset: 0x0000496B
	protected virtual float m_spinMove_Attack_Duration
	{
		get
		{
			return 3.25f;
		}
	}

	// Token: 0x17000477 RID: 1143
	// (get) Token: 0x060009C7 RID: 2503 RVA: 0x0000521E File Offset: 0x0000341E
	protected virtual float m_spinMove_Attack_MoveSpeed
	{
		get
		{
			return 22f;
		}
	}

	// Token: 0x17000478 RID: 1144
	// (get) Token: 0x060009C8 RID: 2504 RVA: 0x00005FAA File Offset: 0x000041AA
	protected virtual float m_spinMove_ExitIntro_AnimationSpeed
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x17000479 RID: 1145
	// (get) Token: 0x060009C9 RID: 2505 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float m_spinMove_ExitIntro_EndDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x1700047A RID: 1146
	// (get) Token: 0x060009CA RID: 2506 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_hasTailSpin
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700047B RID: 1147
	// (get) Token: 0x060009CB RID: 2507 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_hasTailRam
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700047C RID: 1148
	// (get) Token: 0x060009CC RID: 2508 RVA: 0x00006772 File Offset: 0x00004972
	protected virtual float m_fireball_TailSpawnDelay
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x1700047D RID: 1149
	// (get) Token: 0x060009CD RID: 2509 RVA: 0x00006779 File Offset: 0x00004979
	protected virtual float m_fireball_TailRamSpawnDelay
	{
		get
		{
			return 0.015f;
		}
	}

	// Token: 0x1700047E RID: 1150
	// (get) Token: 0x060009CE RID: 2510 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float m_spinMove_Exit_ForceIdle
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x1700047F RID: 1151
	// (get) Token: 0x060009CF RID: 2511 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spinMove_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x060009D0 RID: 2512 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_raiseKnockbackDefenseWhileAttacking
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x060009D1 RID: 2513 RVA: 0x00004762 File Offset: 0x00002962
	protected virtual int m_knockbackDefenseBoostOverride
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x060009D2 RID: 2514 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_ramMove_ChaseDuration
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x060009D3 RID: 2515 RVA: 0x000052A9 File Offset: 0x000034A9
	protected virtual float m_ramMove_TellIntro_AnimationSpeed
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x060009D4 RID: 2516 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_ramMove_TellHold_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x060009D5 RID: 2517 RVA: 0x00006780 File Offset: 0x00004980
	protected virtual float m_ramMove_TellIntroAndHold_Delay
	{
		get
		{
			return 0.65f;
		}
	}

	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x060009D6 RID: 2518 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_ramMove_AttackIntro_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x060009D7 RID: 2519 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_ramMove_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x060009D8 RID: 2520 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_ramMove_AttackHold_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x060009D9 RID: 2521 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_ramMove_AttackHold_Duration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x060009DA RID: 2522 RVA: 0x00004520 File Offset: 0x00002720
	protected virtual float m_ramMove_Attack_Duration
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x060009DB RID: 2523 RVA: 0x00006787 File Offset: 0x00004987
	protected virtual float m_ramMove_Attack_Speed
	{
		get
		{
			return 41f;
		}
	}

	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x060009DC RID: 2524 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_ramMove_ExitIntro_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x060009DD RID: 2525 RVA: 0x000050CB File Offset: 0x000032CB
	protected virtual float m_ramMove_ExitIntro_EndDelay
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x060009DE RID: 2526 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float m_ramMove_ExitIntro_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x060009DF RID: 2527 RVA: 0x00003D93 File Offset: 0x00001F93
	protected virtual float m_ramMove_Attack_CD
	{
		get
		{
			return 6f;
		}
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x0000678E File Offset: 0x0000498E
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.DisableFriction = true;
		this.m_fireball_TailCounter = this.m_fireball_TailSpawnDelay;
		this.m_bounceLogic = base.EnemyController.GetComponent<BounceCollision>();
		this.m_bounceLogic.enabled = false;
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x000067CC File Offset: 0x000049CC
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Ram_Attack()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.GenerateRandomFollowOffset(new Vector2(1f, 1f), Vector2.zero);
		if (this.m_ramMove_ChaseDuration > 0f)
		{
			yield return base.Wait(this.m_ramMove_ChaseDuration, false);
		}
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_TellIntroAndLoop("Ram_Tell_Intro", this.m_ramMove_TellIntro_AnimationSpeed, "Ram_Tell_Hold", this.m_ramMove_TellHold_AnimationSpeed, this.m_ramMove_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Override;
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_Animation("Ram_Attack_Intro", this.m_ramMove_AttackIntro_AnimationSpeed, this.m_ramMove_AttackIntro_Delay, true);
		yield return this.Default_Animation("Ram_Attack_Hold", this.m_ramMove_AttackHold_AnimationSpeed, this.m_ramMove_AttackHold_Duration, false);
		this.m_enableTail = this.m_hasTailRam;
		this.m_isRamAttack = true;
		if (base.EnemyController.IsFacingRight)
		{
			base.SetVelocityX(this.m_ramMove_Attack_Speed, false);
		}
		else
		{
			base.SetVelocityX(-this.m_ramMove_Attack_Speed, false);
		}
		if (this.m_ramMove_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_ramMove_Attack_Duration, false);
		}
		base.EnemyController.ResetBaseValues();
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		this.m_enableTail = false;
		this.m_isRamAttack = false;
		yield return this.Default_Animation("Ram_Exit", this.m_ramMove_ExitIntro_AnimationSpeed, this.m_ramMove_ExitIntro_EndDelay, true);
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_ramMove_ExitIntro_ForceIdle, this.m_ramMove_Attack_CD);
		yield break;
	}

	// Token: 0x060009E2 RID: 2530 RVA: 0x000067DB File Offset: 0x000049DB
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Spin_Attack()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.FollowTarget = true;
		base.EnemyController.FollowOffset = Vector3.zero;
		base.EnemyController.ForceFaceTarget();
		base.EnemyController.BaseTurnSpeed = this.m_spinAttackTurnSpeedOverride;
		this.m_aim = true;
		yield return this.Default_Animation("Spin_Tell_Intro", this.m_spinMove_TellIntro_AnimationSpeed, this.m_spinMove_TellIntro_EndDelay, true);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Spin_Tell_Hold", this.m_spinMove_TellIntro_AnimationSpeed, 0f, true);
		yield return this.Default_Animation("Spin_Attack_Intro", this.m_spinMove_TellIntro_AnimationSpeed, 0f, true);
		yield return this.Default_Animation("Spin_Attack_Hold", this.m_spinMove_TellHold_AnimationSpeed, this.m_spinMove_TellHold_Duration, true);
		base.EnemyController.FollowTarget = false;
		if (this.m_raiseKnockbackDefenseWhileAttacking)
		{
			base.EnemyController.BaseKnockbackDefense = (float)this.m_knockbackDefenseBoostOverride;
		}
		base.EnemyController.FlyingMovementType = FlyingMovementType.Override;
		base.SetVelocity(base.EnemyController.HeadingX * this.m_spinMove_Attack_MoveSpeed, base.EnemyController.HeadingY * this.m_spinMove_Attack_MoveSpeed, false);
		this.m_bounceLogic.enabled = true;
		this.m_aim = false;
		base.EnemyController.HitboxController.CollisionType = CollisionType.EnemyProjectile;
		this.m_enableTail = this.m_hasTailSpin;
		if (this.m_spinMove_Attack_FlameDrops > 0f)
		{
			int i = 0;
			while ((float)i < this.m_spinMove_Attack_FlameDrops)
			{
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
				{
					this.FireProjectile("FlyingShieldWaterMinibossProjectile", 0, false, 0f, 0f, true, true, true);
				}
				else
				{
					this.FireProjectile("FlyingShieldWaterProjectile", 0, false, 0f, 0f, true, true, true);
				}
				if (this.m_spinMove_Attack_Duration / this.m_spinMove_Attack_FlameDrops > 0f)
				{
					yield return base.Wait(this.m_spinMove_Attack_Duration / this.m_spinMove_Attack_FlameDrops, false);
				}
				int num = i;
				i = num + 1;
			}
		}
		else if (this.m_spinMove_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_spinMove_Attack_Duration, false);
		}
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		this.m_bounceLogic.enabled = false;
		base.EnemyController.HitboxController.CollisionType = CollisionType.Enemy;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.ChangeAnimationState("Spin_Exit");
		yield return base.WaitUntilAnimComplete(this.DefaultAnimationLayer);
		base.Animator.ResetTrigger("Turn");
		base.EnemyController.ForceFaceTarget();
		base.EnemyController.FollowTarget = true;
		this.m_enableTail = false;
		base.EnemyController.ResetBaseValues();
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		this.TriggerAttackCooldown(this.m_spinMove_Exit_AttackCD, false);
		this.TriggerRestState(false);
		yield break;
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x000067EA File Offset: 0x000049EA
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		this.m_aim = false;
		this.m_aimSprite.SetActive(false);
		this.m_enableTail = false;
		base.EnemyController.HitboxController.CollisionType = CollisionType.Enemy;
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x0000681D File Offset: 0x00004A1D
	public override void ResetScript()
	{
		base.ResetScript();
		this.m_bounceLogic.enabled = false;
		this.m_enableTail = false;
		this.m_fireball_TailCounter = this.m_fireball_TailSpawnDelay;
		this.m_aim = false;
		this.m_aimSprite.SetActive(false);
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x00067210 File Offset: 0x00065410
	protected void Update()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (this.m_enableTail && this.m_fireball_TailCounter > 0f)
		{
			this.m_fireball_TailCounter -= Time.deltaTime;
			if (this.m_fireball_TailCounter <= 0f)
			{
				if (this.m_isRamAttack)
				{
					this.m_fireball_TailCounter = this.m_fireball_TailRamSpawnDelay;
				}
				else
				{
					this.m_fireball_TailCounter = this.m_fireball_TailSpawnDelay;
				}
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Advanced)
				{
					this.FireProjectile("FlyingShieldWaterProjectile", 0, false, 0f, 0f, true, true, true);
				}
				else if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
				{
					this.FireProjectile("FlyingShieldWaterExpertProjectile", 0, false, 0f, 0f, true, true, true);
				}
				else
				{
					this.FireProjectile("FlyingShieldWaterMinibossProjectile", 0, false, 0f, 0f, true, true, true);
				}
			}
		}
		if (this.m_aim)
		{
			if (!this.m_aimSprite.activeSelf)
			{
				this.m_aimSprite.SetActive(true);
			}
			this.m_aimSprite.transform.SetLocalEulerZ(CDGHelper.VectorToAngle(base.EnemyController.Heading));
			return;
		}
		if (this.m_aimSprite.activeSelf)
		{
			this.m_aimSprite.SetActive(false);
		}
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x00006857 File Offset: 0x00004A57
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_aimSprite.activeSelf)
		{
			this.m_aimSprite.SetActive(false);
		}
	}

	// Token: 0x04000D4A RID: 3402
	[SerializeField]
	private GameObject m_aimSprite;

	// Token: 0x04000D4B RID: 3403
	private float m_fireball_TailCounter;

	// Token: 0x04000D4C RID: 3404
	private bool m_enableTail;

	// Token: 0x04000D4D RID: 3405
	private bool m_isRamAttack;

	// Token: 0x04000D4E RID: 3406
	private bool m_aim;

	// Token: 0x04000D4F RID: 3407
	private BounceCollision m_bounceLogic;

	// Token: 0x04000D50 RID: 3408
	protected Vector3 m_ramMove_SetRamPosition = new Vector3(-1f, 0f, 0f);

	// Token: 0x04000D51 RID: 3409
	private float m_spinAttackTurnSpeedOverride = 175f;
}
