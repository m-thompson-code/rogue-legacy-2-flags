using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200015E RID: 350
public class FlyingHammer_Basic_AIScript : BaseAIScript
{
	// Token: 0x0600093E RID: 2366 RVA: 0x000657BC File Offset: 0x000639BC
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlyingHammerMagmaProjectile",
			"FlyingHammerWarningMinibossProjectile",
			"FlyingHammerWarningLargeProjectile",
			"FlyingHammerWarningProjectile",
			"FlyingHammerQuakeMinibossProjectile",
			"FlyingHammerQuakeLargeProjectile",
			"FlyingHammerQuakeProjectile",
			"FlyingHammerFireballMinibossProjectile",
			"FlyingHammerCurseBoltProjectile",
			"FlyingHammerWhirlwindProjectile",
			"FlyingHammerWarningWhirlwindProjectile"
		};
	}

	// Token: 0x17000427 RID: 1063
	// (get) Token: 0x0600093F RID: 2367 RVA: 0x0000645C File Offset: 0x0000465C
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.7f, 1f);
		}
	}

	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x06000940 RID: 2368 RVA: 0x00006275 File Offset: 0x00004475
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.8f, 1.1f);
		}
	}

	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x06000941 RID: 2369 RVA: 0x00006275 File Offset: 0x00004475
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.8f, 1.1f);
		}
	}

	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x06000942 RID: 2370 RVA: 0x00004A48 File Offset: 0x00002C48
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(0f, 0f);
		}
	}

	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x06000943 RID: 2371 RVA: 0x00004A48 File Offset: 0x00002C48
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(0f, 0f);
		}
	}

	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x06000944 RID: 2372 RVA: 0x00003DA4 File Offset: 0x00001FA4
	protected virtual float m_vertSpin_Attack_MovementSpeed
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x06000945 RID: 2373 RVA: 0x00004A26 File Offset: 0x00002C26
	protected virtual Vector2 m_fireballSpeedMod
	{
		get
		{
			return new Vector2(0.5f, 1f);
		}
	}

	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x06000946 RID: 2374 RVA: 0x0000646D File Offset: 0x0000466D
	protected virtual Vector2 m_fireballAngle
	{
		get
		{
			return new Vector2(180f, 0f);
		}
	}

	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x06000947 RID: 2375 RVA: 0x000050CB File Offset: 0x000032CB
	protected virtual float m_initialProjectileDelay
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x06000948 RID: 2376 RVA: 0x0000647E File Offset: 0x0000467E
	protected virtual float m_exitProjectileDelay
	{
		get
		{
			return 0.55f;
		}
	}

	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x06000949 RID: 2377 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_projectileSpawnAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000432 RID: 1074
	// (get) Token: 0x0600094A RID: 2378 RVA: 0x00005303 File Offset: 0x00003503
	protected virtual int m_projectileSpawnLoopCount
	{
		get
		{
			return 12;
		}
	}

	// Token: 0x17000433 RID: 1075
	// (get) Token: 0x0600094B RID: 2379 RVA: 0x00003C5B File Offset: 0x00001E5B
	protected virtual float m_projectileRateOfFire
	{
		get
		{
			return 0.125f;
		}
	}

	// Token: 0x17000434 RID: 1076
	// (get) Token: 0x0600094C RID: 2380 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_shockwave_IsLarge
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x0600094D RID: 2381 RVA: 0x00006220 File Offset: 0x00004420
	protected virtual float m_shockwave_Attack_TurnSpeed
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x0600094E RID: 2382 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shockwave_Attack_MovementSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x0600094F RID: 2383 RVA: 0x00006485 File Offset: 0x00004685
	protected virtual Vector2 m_shockwave_fireballSpeedMod
	{
		get
		{
			return new Vector2(0.5f, 1.25f);
		}
	}

	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x06000950 RID: 2384 RVA: 0x00006496 File Offset: 0x00004696
	protected virtual Vector2 m_shockwave_fireballAngle
	{
		get
		{
			return new Vector2(360f, 0f);
		}
	}

	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x06000951 RID: 2385 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shockwave_initialProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x06000952 RID: 2386 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shockwave_exitProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700043B RID: 1083
	// (get) Token: 0x06000953 RID: 2387 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual int m_shockwave_projectileSpawnAmount
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x06000954 RID: 2388 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual int m_shockwave_projectileSpawnLoopCount
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x06000955 RID: 2389 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_causesRicochet
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x06000956 RID: 2390 RVA: 0x000064A7 File Offset: 0x000046A7
	protected virtual Vector2 m_ricochetKnockbackMod
	{
		get
		{
			return new Vector2(10.5f, 13.5f);
		}
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x000064B8 File Offset: 0x000046B8
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Vertical_Spin_Attack()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.FollowOffset = Vector3.zero;
		yield return this.Default_TellIntroAndLoop("SideSpin_Tell_Intro", this.m_vertSpin_TellIntro_AnimationSpeed, "SideSpin_Tell_Hold", this.m_vertSpin_TellHold_AnimationSpeed, this.m_vertSpin_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("SideSpin_Attack_Intro", this.m_vertSpin_AttackIntro_AnimationSpeed, this.m_vertSpin_AttackIntro_Delay, true);
		yield return this.Default_Animation("SideSpin_Attack_Hold", 1f, 0f, false);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_vertSpin_Attack_MovementSpeed;
		base.EnemyController.BaseTurnSpeed = this.m_vertSpin_Attack_TurnSpeed;
		if (this.m_initialProjectileDelay > 0f)
		{
			yield return base.Wait(this.m_initialProjectileDelay, false);
		}
		int num2;
		for (int i = 0; i < this.m_projectileSpawnLoopCount; i = num2 + 1)
		{
			for (int j = 0; j < this.m_projectileSpawnAmount; j++)
			{
				int num = (int)UnityEngine.Random.Range(this.m_fireballAngle.x, this.m_fireballAngle.y);
				float speedMod = UnityEngine.Random.Range(this.m_fireballSpeedMod.x, this.m_fireballSpeedMod.y);
				this.FireProjectile("FlyingHammerMagmaProjectile", new Vector2(0f, 0f), true, (float)num, speedMod, true, true, true);
			}
			if (this.m_projectileRateOfFire > 0f)
			{
				yield return base.Wait(this.m_projectileRateOfFire, false);
			}
			num2 = i;
		}
		base.EnemyController.ResetBaseValues();
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("SideSpin_Exit", this.m_vertSpin_ExitIntro_AnimationSpeed, 0.15f, true);
		base.EnemyController.ResetTurnTrigger();
		yield return this.Default_Attack_Cooldown(0.5f, this.m_vertSpin_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x000064C7 File Offset: 0x000046C7
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Shockwave_Attack()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.FollowOffset = Vector3.zero;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			this.m_warningProjectile = this.FireProjectile("FlyingHammerWarningMinibossProjectile", 0, true, 0f, 1f, true, true, true);
		}
		else if (this.m_shockwave_IsLarge)
		{
			this.m_warningProjectile = this.FireProjectile("FlyingHammerWarningLargeProjectile", 0, true, 0f, 1f, true, true, true);
		}
		else
		{
			this.m_warningProjectile = this.FireProjectile("FlyingHammerWarningProjectile", 0, true, 0f, 1f, true, true, true);
		}
		yield return this.Default_TellIntroAndLoop("WallHit_Tell_Intro", this.m_shockwave_TellIntro_AnimationSpeed, "WallHit_Tell_Hold", this.m_shockwave_TellHold_AnimationSpeed, this.m_shockwave_TellIntroAndHold_Delay);
		base.StopProjectile(ref this.m_warningProjectile);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("WallHit_Attack_Intro", this.m_shockwave_AttackIntro_AnimationSpeed, this.m_shockwave_AttackIntro_Delay, true);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_shockwave_Attack_MovementSpeed;
		base.EnemyController.BaseTurnSpeed = this.m_shockwave_Attack_TurnSpeed;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			this.m_shockwaveProjectile = this.FireProjectile("FlyingHammerQuakeMinibossProjectile", 0, true, 0f, 1f, true, true, true);
		}
		else if (this.m_shockwave_IsLarge)
		{
			this.m_shockwaveProjectile = this.FireProjectile("FlyingHammerQuakeLargeProjectile", 0, true, 0f, 1f, true, true, true);
		}
		else
		{
			this.m_shockwaveProjectile = this.FireProjectile("FlyingHammerQuakeProjectile", 0, true, 0f, 1f, true, true, true);
		}
		this.m_shockwaveProjectileSpawnPos = this.m_shockwaveProjectile.transform.position;
		yield return this.Default_Animation("WallHit_Attack_Hold", 1f, 0f, true);
		if (this.m_initialProjectileDelay > 0f)
		{
			yield return base.Wait(this.m_shockwave_initialProjectileDelay, false);
		}
		int num2;
		for (int i = 0; i < this.m_shockwave_projectileSpawnLoopCount + 1; i = num2 + 1)
		{
			for (int j = 0; j < this.m_shockwave_projectileSpawnAmount; j++)
			{
				int num = (int)UnityEngine.Random.Range(this.m_shockwave_fireballAngle.x, this.m_shockwave_fireballAngle.y);
				float speedMod = UnityEngine.Random.Range(this.m_shockwave_fireballSpeedMod.x, this.m_shockwave_fireballSpeedMod.y);
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
				{
					this.FireProjectile("FlyingHammerFireballMinibossProjectile", new Vector2(0f, 0f), false, (float)num, speedMod, true, true, true);
				}
				else
				{
					this.FireProjectile("FlyingHammerCurseBoltProjectile", new Vector2(0f, 0f), false, (float)num, speedMod, true, true, true);
				}
			}
			if (1f / (float)this.m_shockwave_projectileSpawnLoopCount > 0f)
			{
				yield return base.Wait(1f / (float)(this.m_shockwave_projectileSpawnLoopCount + 1), false);
			}
			num2 = i;
		}
		base.StopProjectile(ref this.m_shockwaveProjectile);
		base.EnemyController.ResetBaseValues();
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("WallHit_Exit", this.m_shockwave_ExitIntro_AnimationSpeed, 0.15f, true);
		base.EnemyController.ResetTurnTrigger();
		yield return this.Default_Attack_Cooldown(1.25f, this.m_shockwave_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x000064D6 File Offset: 0x000046D6
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShockwaveAtPlayer_Attack()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.FollowOffset = Vector3.zero;
		base.EnemyController.LockFlip = true;
		Vector3 shockwaveSpawnPos = base.EnemyController.TargetController.Midpoint;
		this.m_isHammeringAtPlayer = true;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningWhirlwindProjectile", shockwaveSpawnPos, false, 0f, 1f, true, true, true);
		}
		else if (this.m_shockwave_IsLarge)
		{
			this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningWhirlwindProjectile", shockwaveSpawnPos, false, 0f, 1f, true, true, true);
		}
		else
		{
			this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningWhirlwindProjectile", shockwaveSpawnPos, false, 0f, 1f, true, true, true);
		}
		yield return this.Default_TellIntroAndLoop("VerticalSpin_Tell_Intro", this.m_shockwaveAtPlayer_TellIntro_AnimationSpeed, "VerticalSpin_Tell_Hold", this.m_shockwaveAtPlayer_TellHold_AnimationSpeed, this.m_shockwaveAtPlayer_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		base.StopProjectile(ref this.m_warningProjectile);
		yield return this.Default_Animation("VerticalSpin_Attack_Intro", this.m_shockwaveAtPlayer_AttackIntro_AnimationSpeed, this.m_shockwaveAtPlayer_AttackIntro_Delay, true);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_shockwave_Attack_MovementSpeed;
		base.EnemyController.BaseTurnSpeed = this.m_shockwave_Attack_TurnSpeed;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerWhirlwindProjectile", shockwaveSpawnPos, false, 0f, 1f, true, true, true);
		}
		else if (this.m_shockwave_IsLarge)
		{
			this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerWhirlwindProjectile", shockwaveSpawnPos, false, 0f, 1f, true, true, true);
		}
		else
		{
			this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerWhirlwindProjectile", shockwaveSpawnPos, false, 0f, 1f, true, true, true);
		}
		this.m_shockwaveProjectileSpawnPos = this.m_shockwaveProjectile.transform.position;
		if (this.m_shockwave_IsLarge)
		{
			yield return this.Default_Animation("VerticalSpin_Attack_Hold", 1f, 0f, true);
		}
		else
		{
			yield return this.Default_Animation("VerticalSpin_Attack_Hold", 1f, 1.55f, true);
		}
		if (this.m_initialProjectileDelay > 0f)
		{
			yield return base.Wait(this.m_shockwave_initialProjectileDelay, false);
		}
		if (this.m_shockwave_IsLarge)
		{
			int num2;
			for (int i = 0; i < this.m_shockwave_projectileSpawnLoopCount + 1; i = num2 + 1)
			{
				for (int j = 0; j < this.m_shockwave_projectileSpawnAmount; j++)
				{
					int num = (int)UnityEngine.Random.Range(this.m_shockwave_fireballAngle.x, this.m_shockwave_fireballAngle.y);
					float speedMod = UnityEngine.Random.Range(this.m_shockwave_fireballSpeedMod.x, this.m_shockwave_fireballSpeedMod.y);
					if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
					{
						this.FireProjectileAbsPos("FlyingHammerFireballMinibossProjectile", shockwaveSpawnPos, false, (float)num, speedMod, true, true, true);
					}
					else
					{
						this.FireProjectileAbsPos("FlyingHammerCurseBoltProjectile", shockwaveSpawnPos, false, (float)num, speedMod, true, true, true);
					}
				}
				if (1f / (float)this.m_shockwave_projectileSpawnLoopCount > 0f)
				{
					yield return base.Wait(1f / (float)(this.m_shockwave_projectileSpawnLoopCount + 1), false);
				}
				num2 = i;
			}
		}
		base.StopProjectile(ref this.m_shockwaveProjectile);
		this.m_isHammeringAtPlayer = false;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("VerticalSpin_Exit", this.m_shockwaveAtPlayer_ExitIntro_AnimationSpeed, 0.15f, true);
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		base.EnemyController.LockFlip = false;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_Attack_Cooldown(1.25f, this.m_shockwaveAtPlayer_Exit_AttackCD);
		yield break;
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x00065830 File Offset: 0x00063A30
	public override void Pause()
	{
		base.Pause();
		if (this.m_shockwaveProjectile && !this.m_shockwaveProjectile.IsFreePoolObj && this.m_shockwaveProjectile.Owner == base.EnemyController.gameObject)
		{
			this.m_frozenWhileHammering = true;
			base.StopProjectile(ref this.m_shockwaveProjectile);
			if (!this.m_isHammeringAtPlayer)
			{
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
				{
					this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningMinibossProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
					return;
				}
				if (this.m_shockwave_IsLarge)
				{
					this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningLargeProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
					return;
				}
				this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
				return;
			}
			else
			{
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
				{
					this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningWhirlwindProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
					return;
				}
				if (this.m_shockwave_IsLarge)
				{
					this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningWhirlwindProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
					return;
				}
				this.m_warningProjectile = this.FireProjectileAbsPos("FlyingHammerWarningWhirlwindProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
			}
		}
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x000659D0 File Offset: 0x00063BD0
	public override void Unpause()
	{
		base.Unpause();
		if (this.m_frozenWhileHammering)
		{
			if (!this.m_isHammeringAtPlayer)
			{
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
				{
					this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerQuakeMinibossProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
				}
				else if (this.m_shockwave_IsLarge)
				{
					this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerQuakeLargeProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
				}
				else
				{
					this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerQuakeProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
				}
			}
			else if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
			{
				this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerWhirlwindProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
			}
			else if (this.m_shockwave_IsLarge)
			{
				this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerWhirlwindProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
			}
			else
			{
				this.m_shockwaveProjectile = this.FireProjectileAbsPos("FlyingHammerWhirlwindProjectile", this.m_shockwaveProjectileSpawnPos, false, 0f, 1f, true, true, true);
			}
			base.StopProjectile(ref this.m_warningProjectile);
			this.m_frozenWhileHammering = false;
		}
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x000064E5 File Offset: 0x000046E5
	public override void OnLBCompleteOrCancelled()
	{
		base.StopProjectile(ref this.m_warningProjectile);
		base.StopProjectile(ref this.m_shockwaveProjectile);
		this.m_frozenWhileHammering = false;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0000650C File Offset: 0x0000470C
	public override void ResetScript()
	{
		this.m_frozenWhileHammering = false;
		this.m_isHammeringAtPlayer = false;
		base.ResetScript();
	}

	// Token: 0x04000CE8 RID: 3304
	private const string GENERIC_PROJECTILE_NAME = "FlyingHammerMagmaProjectile";

	// Token: 0x04000CE9 RID: 3305
	private const string WARNING_MINIBOSS_PROJECTILE_NAME = "FlyingHammerWarningMinibossProjectile";

	// Token: 0x04000CEA RID: 3306
	private const string WARNING_LARGE_PROJECTILE_NAME = "FlyingHammerWarningLargeProjectile";

	// Token: 0x04000CEB RID: 3307
	private const string WARNING_PROJECTILE_NAME = "FlyingHammerWarningProjectile";

	// Token: 0x04000CEC RID: 3308
	private const string QUAKE_MINIBOSS_PROJECTILE_NAME = "FlyingHammerQuakeMinibossProjectile";

	// Token: 0x04000CED RID: 3309
	private const string QUAKE_LARGE_PROJECTILE_NAME = "FlyingHammerQuakeLargeProjectile";

	// Token: 0x04000CEE RID: 3310
	private const string QUAKE_PROJECTILE_NAME = "FlyingHammerQuakeProjectile";

	// Token: 0x04000CEF RID: 3311
	private const string FIREBALL_MINIBOSS_PROJECTILE_NAME = "FlyingHammerFireballMinibossProjectile";

	// Token: 0x04000CF0 RID: 3312
	private const string CURSEBOLT_PROJECTILE_NAME = "FlyingHammerCurseBoltProjectile";

	// Token: 0x04000CF1 RID: 3313
	private bool m_frozenWhileHammering;

	// Token: 0x04000CF2 RID: 3314
	private bool m_isHammeringAtPlayer;

	// Token: 0x04000CF3 RID: 3315
	protected Projectile_RL m_warningProjectile;

	// Token: 0x04000CF4 RID: 3316
	protected Projectile_RL m_shockwaveProjectile;

	// Token: 0x04000CF5 RID: 3317
	protected Vector3 m_shockwaveProjectileSpawnPos;

	// Token: 0x04000CF6 RID: 3318
	protected float m_vertSpin_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000CF7 RID: 3319
	protected float m_vertSpin_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000CF8 RID: 3320
	protected float m_vertSpin_TellIntroAndHold_Delay = 0.75f;

	// Token: 0x04000CF9 RID: 3321
	protected float m_vertSpin_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000CFA RID: 3322
	protected float m_vertSpin_AttackIntro_Delay;

	// Token: 0x04000CFB RID: 3323
	protected const float m_vertSpin_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000CFC RID: 3324
	protected const float m_vertSpin_AttackHold_Delay = 0f;

	// Token: 0x04000CFD RID: 3325
	protected float m_vertSpin_ExitIntro_AnimationSpeed = 1f;

	// Token: 0x04000CFE RID: 3326
	protected const float m_vertSpin_ExitIntro_EndDelay = 0.15f;

	// Token: 0x04000CFF RID: 3327
	protected const float m_vertSpin_Exit_ForceIdle = 0.5f;

	// Token: 0x04000D00 RID: 3328
	protected float m_vertSpin_Exit_AttackCD = 3f;

	// Token: 0x04000D01 RID: 3329
	protected float m_vertSpin_Attack_TurnSpeed;

	// Token: 0x04000D02 RID: 3330
	protected float m_shockwave_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000D03 RID: 3331
	protected float m_shockwave_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000D04 RID: 3332
	protected float m_shockwave_TellIntroAndHold_Delay = 1f;

	// Token: 0x04000D05 RID: 3333
	protected float m_shockwave_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000D06 RID: 3334
	protected float m_shockwave_AttackIntro_Delay;

	// Token: 0x04000D07 RID: 3335
	protected const float m_shockwave_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000D08 RID: 3336
	protected const float m_shockwave_AttackHold_Delay = 1f;

	// Token: 0x04000D09 RID: 3337
	protected float m_shockwave_ExitIntro_AnimationSpeed = 1f;

	// Token: 0x04000D0A RID: 3338
	protected const float m_shockwave_ExitIntro_EndDelay = 0.15f;

	// Token: 0x04000D0B RID: 3339
	protected const float m_shockwave_Exit_ForceIdle = 1.25f;

	// Token: 0x04000D0C RID: 3340
	protected float m_shockwave_Exit_AttackCD;

	// Token: 0x04000D0D RID: 3341
	protected float m_shockwaveAtPlayer_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000D0E RID: 3342
	protected float m_shockwaveAtPlayer_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000D0F RID: 3343
	protected float m_shockwaveAtPlayer_TellIntroAndHold_Delay = 1.45f;

	// Token: 0x04000D10 RID: 3344
	protected float m_shockwaveAtPlayer_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000D11 RID: 3345
	protected float m_shockwaveAtPlayer_AttackIntro_Delay;

	// Token: 0x04000D12 RID: 3346
	protected const float m_shockwaveAtPlayer_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000D13 RID: 3347
	protected const float m_shockwaveAtPlayer_AttackHold_Delay = 1.55f;

	// Token: 0x04000D14 RID: 3348
	protected float m_shockwaveAtPlayer_ExitIntro_AnimationSpeed = 1f;

	// Token: 0x04000D15 RID: 3349
	protected const float m_shockwaveAtPlayer_ExitIntro_EndDelay = 0.15f;

	// Token: 0x04000D16 RID: 3350
	protected const float m_shockwaveAtPlayer_Exit_ForceIdle = 1.25f;

	// Token: 0x04000D17 RID: 3351
	protected float m_shockwaveAtPlayer_Exit_AttackCD = 4f;

	// Token: 0x04000D18 RID: 3352
	protected const string SHOCKWAVE_AT_PLAYER_PROJECTILE = "FlyingHammerWhirlwindProjectile";

	// Token: 0x04000D19 RID: 3353
	protected const string SHOCKWAVE_WARNING_AT_PLAYER_PROJECTILE = "FlyingHammerWarningWhirlwindProjectile";
}
