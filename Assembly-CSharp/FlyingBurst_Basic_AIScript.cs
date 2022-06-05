using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class FlyingBurst_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000624 RID: 1572 RVA: 0x00018E86 File Offset: 0x00017086
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

	// Token: 0x1700030D RID: 781
	// (get) Token: 0x06000625 RID: 1573 RVA: 0x00018EC4 File Offset: 0x000170C4
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.3f, 0.7f);
		}
	}

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x06000626 RID: 1574 RVA: 0x00018ED5 File Offset: 0x000170D5
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.55f, 1.1f);
		}
	}

	// Token: 0x1700030F RID: 783
	// (get) Token: 0x06000627 RID: 1575 RVA: 0x00018EE6 File Offset: 0x000170E6
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.55f, 1.1f);
		}
	}

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x06000628 RID: 1576 RVA: 0x00018EF7 File Offset: 0x000170F7
	protected virtual int ShootPatternTotalLoops
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000311 RID: 785
	// (get) Token: 0x06000629 RID: 1577 RVA: 0x00018EFA File Offset: 0x000170FA
	protected virtual float ShootPatternTotalLoopsDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000312 RID: 786
	// (get) Token: 0x0600062A RID: 1578 RVA: 0x00018F01 File Offset: 0x00017101
	protected virtual int NumFireballsMelee
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000313 RID: 787
	// (get) Token: 0x0600062B RID: 1579 RVA: 0x00018F04 File Offset: 0x00017104
	protected virtual int NumFireballsBasic
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x0600062C RID: 1580 RVA: 0x00018F07 File Offset: 0x00017107
	protected virtual float MeleeFireSpread
	{
		get
		{
			return 120f;
		}
	}

	// Token: 0x17000315 RID: 789
	// (get) Token: 0x0600062D RID: 1581 RVA: 0x00018F0E File Offset: 0x0001710E
	protected virtual float BasicFireSpread
	{
		get
		{
			return 120f;
		}
	}

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x0600062E RID: 1582 RVA: 0x00018F15 File Offset: 0x00017115
	protected virtual Vector2 FireballIntervalDelay
	{
		get
		{
			return new Vector2(0.3f, 0.3f);
		}
	}

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x0600062F RID: 1583 RVA: 0x00018F26 File Offset: 0x00017126
	protected virtual Vector2 FireballRandSpeedMod
	{
		get
		{
			return new Vector2(1f, 1f);
		}
	}

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x06000630 RID: 1584 RVA: 0x00018F37 File Offset: 0x00017137
	protected virtual float FireballDelayBetweenBasicAndMelee
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x00018F3E File Offset: 0x0001713E
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

	// Token: 0x17000319 RID: 793
	// (get) Token: 0x06000632 RID: 1586 RVA: 0x00018F4D File Offset: 0x0001714D
	protected virtual int m_bigShake_NumFireballsBasic
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x06000633 RID: 1587 RVA: 0x00018F50 File Offset: 0x00017150
	protected virtual float m_bigShake_BasicFireSpread
	{
		get
		{
			return 50f;
		}
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x00018F57 File Offset: 0x00017157
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

	// Token: 0x04000A62 RID: 2658
	protected float m_shoot_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000A63 RID: 2659
	protected float m_shoot_TellHold_AnimationSpeed = 2f;

	// Token: 0x04000A64 RID: 2660
	protected float m_shoot_Tell_Delay = 1.15f;

	// Token: 0x04000A65 RID: 2661
	protected float m_shoot_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000A66 RID: 2662
	protected float m_shoot_AttackIntro_Delay;

	// Token: 0x04000A67 RID: 2663
	protected float m_shoot_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x04000A68 RID: 2664
	protected float m_shoot_AttackHold_Delay = 0.4f;

	// Token: 0x04000A69 RID: 2665
	protected float m_shoot_Exit_AnimationSpeed = 1f;

	// Token: 0x04000A6A RID: 2666
	protected float m_shoot_Exit_Delay;

	// Token: 0x04000A6B RID: 2667
	protected float m_shoot_Exit_ForceIdle = 0.15f;

	// Token: 0x04000A6C RID: 2668
	protected float m_shoot_Exit_AttackCD;

	// Token: 0x04000A6D RID: 2669
	protected float m_bigShake_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000A6E RID: 2670
	protected float m_bigShake_TellHold_AnimationSpeed = 2f;

	// Token: 0x04000A6F RID: 2671
	protected float m_bigShake_Tell_Delay = 1.15f;

	// Token: 0x04000A70 RID: 2672
	protected float m_bigShake_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000A71 RID: 2673
	protected float m_bigShake_AttackIntro_Delay;

	// Token: 0x04000A72 RID: 2674
	protected float m_bigShake_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x04000A73 RID: 2675
	protected float m_bigShake_AttackHold_Delay = 0.4f;

	// Token: 0x04000A74 RID: 2676
	protected float m_bigShake_Exit_AnimationSpeed = 1f;

	// Token: 0x04000A75 RID: 2677
	protected float m_bigShake_Exit_Delay;

	// Token: 0x04000A76 RID: 2678
	protected float m_bigShake_Exit_ForceIdle = 0.15f;

	// Token: 0x04000A77 RID: 2679
	protected float m_bigShake_Exit_AttackCD;
}
