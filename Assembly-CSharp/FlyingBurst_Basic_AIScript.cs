using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000155 RID: 341
public class FlyingBurst_Basic_AIScript : BaseAIScript
{
	// Token: 0x060008F9 RID: 2297 RVA: 0x00006336 File Offset: 0x00004536
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlyingBurstBounceBoltMinibossProjectile",
			"FlyingBurstSlashBoltProjectile",
			"FlyingBurstSuperBounceBoltMinibossProjectile",
			"FlyingBurstBlueBoltProjectile",
			"FlyingBurstBlueBoltMinibossProjectile",
			"FlyingBurstSlashBoltMinibossProjectile"
		};
	}

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x060008FA RID: 2298 RVA: 0x00006264 File Offset: 0x00004464
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.3f, 0.7f);
		}
	}

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x060008FB RID: 2299 RVA: 0x00006374 File Offset: 0x00004574
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.55f, 1.1f);
		}
	}

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x060008FC RID: 2300 RVA: 0x00006374 File Offset: 0x00004574
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.55f, 1.1f);
		}
	}

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x060008FD RID: 2301 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int ShootPatternTotalLoops
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x060008FE RID: 2302 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float ShootPatternTotalLoopsDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x060008FF RID: 2303 RVA: 0x000047A4 File Offset: 0x000029A4
	protected virtual int NumFireballsMelee
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x06000900 RID: 2304 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual int NumFireballsBasic
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x06000901 RID: 2305 RVA: 0x00006385 File Offset: 0x00004585
	protected virtual float MeleeFireSpread
	{
		get
		{
			return 120f;
		}
	}

	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x06000902 RID: 2306 RVA: 0x00006385 File Offset: 0x00004585
	protected virtual float BasicFireSpread
	{
		get
		{
			return 120f;
		}
	}

	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x06000903 RID: 2307 RVA: 0x0000638C File Offset: 0x0000458C
	protected virtual Vector2 FireballIntervalDelay
	{
		get
		{
			return new Vector2(0.3f, 0.3f);
		}
	}

	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x06000904 RID: 2308 RVA: 0x00005FEC File Offset: 0x000041EC
	protected virtual Vector2 FireballRandSpeedMod
	{
		get
		{
			return new Vector2(1f, 1f);
		}
	}

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x06000905 RID: 2309 RVA: 0x0000452F File Offset: 0x0000272F
	protected virtual float FireballDelayBetweenBasicAndMelee
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x0000639D File Offset: 0x0000459D
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootFireball()
	{
		if (base.LogicController.EnemyLogicType != EnemyLogicType.Miniboss)
		{
			this.StopAndFaceTarget();
		}
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_TellIntroAndLoop("Shake_Tell_Intro", this.m_shoot_TellIntro_AnimationSpeed, "Shake_Tell_Hold", this.m_shoot_TellHold_AnimationSpeed, this.m_shoot_Tell_Delay);
		yield return this.Default_Animation("Shake_Attack_Intro", this.m_shoot_AttackIntro_AnimationSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("Shake_Attack_Hold", this.m_shoot_AttackHold_AnimationSpeed, 0f, false);
		int num3;
		for (int i = 0; i < this.ShootPatternTotalLoops; i = num3 + 1)
		{
			for (int j = 0; j < this.NumFireballsMelee; j = num3 + 1)
			{
				float num = CDGHelper.WrapAngleDegrees(CDGHelper.ToDegrees(Mathf.Atan2(base.EnemyController.TargetController.Midpoint.y - base.EnemyController.Midpoint.y, base.EnemyController.TargetController.Midpoint.x - base.EnemyController.Midpoint.x)), false) - this.MeleeFireSpread / 2f;
				float speedMod = UnityEngine.Random.Range(this.FireballRandSpeedMod.x, this.FireballRandSpeedMod.y);
				float angle = UnityEngine.Random.Range(num, num + this.MeleeFireSpread);
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
				{
					this.FireProjectile("FlyingBurstBounceBoltMinibossProjectile", 1, false, angle, speedMod, true, true, true);
				}
				else
				{
					this.FireProjectile("FlyingBurstSlashBoltProjectile", 1, false, angle, speedMod, true, true, true);
				}
				float num2 = UnityEngine.Random.Range(this.FireballIntervalDelay.x, this.FireballIntervalDelay.y);
				if (num2 > 0f)
				{
					yield return base.Wait(num2, false);
				}
				num3 = j;
			}
			if (this.FireballDelayBetweenBasicAndMelee > 0f)
			{
				yield return base.Wait(this.FireballDelayBetweenBasicAndMelee, false);
			}
			for (int j = 0; j < this.NumFireballsBasic; j = num3 + 1)
			{
				float num4 = CDGHelper.WrapAngleDegrees(CDGHelper.ToDegrees(Mathf.Atan2(base.EnemyController.TargetController.Midpoint.y - base.EnemyController.Midpoint.y, base.EnemyController.TargetController.Midpoint.x - base.EnemyController.Midpoint.x)), false) - this.BasicFireSpread / 2f;
				float speedMod2 = UnityEngine.Random.Range(this.FireballRandSpeedMod.x, this.FireballRandSpeedMod.y);
				float angle2 = UnityEngine.Random.Range(num4, num4 + this.BasicFireSpread);
				if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
				{
					this.FireProjectile("FlyingBurstSuperBounceBoltMinibossProjectile", 1, false, angle2, speedMod2, true, true, true);
				}
				else
				{
					this.FireProjectile("FlyingBurstBlueBoltProjectile", 1, false, angle2, speedMod2, true, true, true);
				}
				float num5 = UnityEngine.Random.Range(this.FireballIntervalDelay.x, this.FireballIntervalDelay.y);
				if (num5 > 0f)
				{
					yield return base.Wait(num5, false);
				}
				num3 = j;
			}
			if (this.ShootPatternTotalLoopsDelay > 0f)
			{
				yield return base.Wait(this.ShootPatternTotalLoopsDelay, false);
			}
			num3 = i;
		}
		if (this.m_shoot_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.m_shoot_AttackHold_Delay, false);
		}
		yield return this.Default_Animation("Shake_Exit", this.m_shoot_Exit_AnimationSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x06000907 RID: 2311 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_bigShake_NumFireballsBasic
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x06000908 RID: 2312 RVA: 0x000063AC File Offset: 0x000045AC
	protected virtual float m_bigShake_BasicFireSpread
	{
		get
		{
			return 50f;
		}
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x000063B3 File Offset: 0x000045B3
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator BigShakeAttack()
	{
		this.StopAndFaceTarget();
		yield return this.Default_TellIntroAndLoop("BigShake_Tell_Intro", this.m_bigShake_TellIntro_AnimationSpeed, "BigShake_Tell_Hold", this.m_bigShake_TellHold_AnimationSpeed, this.m_bigShake_Tell_Delay);
		yield return this.Default_Animation("BigShake_Attack_Intro", this.m_bigShake_AttackIntro_AnimationSpeed, this.m_bigShake_AttackIntro_Delay, true);
		yield return this.Default_Animation("BigShake_Attack_Hold", this.m_bigShake_AttackHold_AnimationSpeed, 0f, false);
		float num = Mathf.Atan2(base.EnemyController.TargetController.Midpoint.y - base.EnemyController.Midpoint.y, base.EnemyController.TargetController.Midpoint.x - base.EnemyController.Midpoint.x);
		num = CDGHelper.WrapAngleDegrees(CDGHelper.ToDegrees(num), false);
		this.FireProjectile("FlyingBurstBlueBoltProjectile", 1, false, num, 1f, true, true, true);
		this.FireProjectile("FlyingBurstBlueBoltProjectile", 1, false, num + 10f, 1f, true, true, true);
		this.FireProjectile("FlyingBurstBlueBoltProjectile", 1, false, num - 10f, 1f, true, true, true);
		for (int i = 0; i < this.m_bigShake_NumFireballsBasic; i++)
		{
			this.FireProjectile("FlyingBurstBlueBoltProjectile", 1, false, num + this.m_bigShake_BasicFireSpread * (float)(i + 1), 1f, true, true, true);
			this.FireProjectile("FlyingBurstBlueBoltProjectile", 1, false, num - this.m_bigShake_BasicFireSpread * (float)(i + 1), 1f, true, true, true);
		}
		if (this.m_bigShake_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.m_bigShake_AttackHold_Delay, false);
		}
		yield return this.Default_Animation("BigShake_Exit", this.m_bigShake_Exit_AnimationSpeed, this.m_bigShake_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_bigShake_Exit_ForceIdle, this.m_bigShake_Exit_AttackCD);
		yield break;
	}

	// Token: 0x04000CB0 RID: 3248
	protected float m_shoot_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000CB1 RID: 3249
	protected float m_shoot_TellHold_AnimationSpeed = 2f;

	// Token: 0x04000CB2 RID: 3250
	protected float m_shoot_Tell_Delay = 1.15f;

	// Token: 0x04000CB3 RID: 3251
	protected float m_shoot_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000CB4 RID: 3252
	protected float m_shoot_AttackIntro_Delay;

	// Token: 0x04000CB5 RID: 3253
	protected float m_shoot_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x04000CB6 RID: 3254
	protected float m_shoot_AttackHold_Delay = 0.4f;

	// Token: 0x04000CB7 RID: 3255
	protected float m_shoot_Exit_AnimationSpeed = 1f;

	// Token: 0x04000CB8 RID: 3256
	protected float m_shoot_Exit_Delay;

	// Token: 0x04000CB9 RID: 3257
	protected float m_shoot_Exit_ForceIdle = 0.15f;

	// Token: 0x04000CBA RID: 3258
	protected float m_shoot_Exit_AttackCD;

	// Token: 0x04000CBB RID: 3259
	protected float m_bigShake_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000CBC RID: 3260
	protected float m_bigShake_TellHold_AnimationSpeed = 2f;

	// Token: 0x04000CBD RID: 3261
	protected float m_bigShake_Tell_Delay = 1.15f;

	// Token: 0x04000CBE RID: 3262
	protected float m_bigShake_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000CBF RID: 3263
	protected float m_bigShake_AttackIntro_Delay;

	// Token: 0x04000CC0 RID: 3264
	protected float m_bigShake_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x04000CC1 RID: 3265
	protected float m_bigShake_AttackHold_Delay = 0.4f;

	// Token: 0x04000CC2 RID: 3266
	protected float m_bigShake_Exit_AnimationSpeed = 1f;

	// Token: 0x04000CC3 RID: 3267
	protected float m_bigShake_Exit_Delay;

	// Token: 0x04000CC4 RID: 3268
	protected float m_bigShake_Exit_ForceIdle = 0.15f;

	// Token: 0x04000CC5 RID: 3269
	protected float m_bigShake_Exit_AttackCD;
}
