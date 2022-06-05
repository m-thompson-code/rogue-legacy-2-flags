using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class Template_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000A07 RID: 2567 RVA: 0x0001FF62 File Offset: 0x0001E162
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"ElementalFireBoltProjectile",
			"ElementalFireBoltLargeProjectile"
		};
	}

	// Token: 0x17000561 RID: 1377
	// (get) Token: 0x06000A08 RID: 2568 RVA: 0x0001FF80 File Offset: 0x0001E180
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x17000562 RID: 1378
	// (get) Token: 0x06000A09 RID: 2569 RVA: 0x0001FF91 File Offset: 0x0001E191
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(1.25f, 2f);
		}
	}

	// Token: 0x17000563 RID: 1379
	// (get) Token: 0x06000A0A RID: 2570 RVA: 0x0001FFA2 File Offset: 0x0001E1A2
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(1.25f, 2f);
		}
	}

	// Token: 0x17000564 RID: 1380
	// (get) Token: 0x06000A0B RID: 2571 RVA: 0x0001FFB3 File Offset: 0x0001E1B3
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(3.5f, 10f);
		}
	}

	// Token: 0x17000565 RID: 1381
	// (get) Token: 0x06000A0C RID: 2572 RVA: 0x0001FFC4 File Offset: 0x0001E1C4
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(1f, 6f);
		}
	}

	// Token: 0x17000566 RID: 1382
	// (get) Token: 0x06000A0D RID: 2573 RVA: 0x0001FFD5 File Offset: 0x0001E1D5
	protected virtual float m_shoot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000567 RID: 1383
	// (get) Token: 0x06000A0E RID: 2574 RVA: 0x0001FFDC File Offset: 0x0001E1DC
	protected virtual float m_shoot_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000568 RID: 1384
	// (get) Token: 0x06000A0F RID: 2575 RVA: 0x0001FFE3 File Offset: 0x0001E1E3
	protected virtual float m_shoot_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000569 RID: 1385
	// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0001FFEA File Offset: 0x0001E1EA
	protected virtual float m_shoot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700056A RID: 1386
	// (get) Token: 0x06000A11 RID: 2577 RVA: 0x0001FFF1 File Offset: 0x0001E1F1
	protected virtual float m_shoot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700056B RID: 1387
	// (get) Token: 0x06000A12 RID: 2578 RVA: 0x0001FFF8 File Offset: 0x0001E1F8
	protected virtual float m_shoot_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700056C RID: 1388
	// (get) Token: 0x06000A13 RID: 2579 RVA: 0x0001FFFF File Offset: 0x0001E1FF
	protected virtual float m_shoot_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x1700056D RID: 1389
	// (get) Token: 0x06000A14 RID: 2580 RVA: 0x00020006 File Offset: 0x0001E206
	protected virtual float m_shoot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700056E RID: 1390
	// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0002000D File Offset: 0x0001E20D
	protected virtual float m_shoot_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700056F RID: 1391
	// (get) Token: 0x06000A16 RID: 2582 RVA: 0x00020014 File Offset: 0x0001E214
	protected virtual float m_shoot_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x06000A17 RID: 2583 RVA: 0x0002001B File Offset: 0x0001E21B
	protected virtual float m_shoot_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000571 RID: 1393
	// (get) Token: 0x06000A18 RID: 2584 RVA: 0x00020022 File Offset: 0x0001E222
	protected virtual int m_shoot_TotalShots
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x06000A19 RID: 2585 RVA: 0x00020025 File Offset: 0x0001E225
	protected virtual float m_shoot_TotalShotDuration
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x06000A1A RID: 2586 RVA: 0x0002002C File Offset: 0x0001E22C
	protected virtual float m_shoot_RandAngleOffset
	{
		get
		{
			return 18f;
		}
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x00020033 File Offset: 0x0001E233
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Move01_RANDOM_SHOOT()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_TellIntroAndLoop("Shoot_Tell_Intro", this.m_shoot_TellIntro_AnimSpeed, "Shoot_Tell_Hold", this.m_shoot_TellHold_AnimSpeed, this.m_shoot_Tell_Delay);
		yield return this.Default_Animation("Shoot_Attack_Intro", this.m_shoot_AttackIntro_AnimSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("Shoot_Attack_Hold", this.m_shoot_AttackHold_AnimSpeed, 0f, false);
		float shotInterval = this.m_shoot_TotalShotDuration / (float)this.m_shoot_TotalShots;
		int num3;
		for (int i = 0; i < this.m_shoot_TotalShots; i = num3 + 1)
		{
			float num = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
			float num2 = UnityEngine.Random.Range(-this.m_shoot_RandAngleOffset, this.m_shoot_RandAngleOffset);
			num += num2;
			this.FireProjectile("ElementalFireBoltProjectile", 0, false, num, 1f, true, true, true);
			if (shotInterval > 0f && i < this.m_shoot_TotalShots - 1)
			{
				yield return base.Wait(shotInterval, false);
			}
			num3 = i;
		}
		yield return this.Default_Animation("Shoot_Exit", this.m_shoot_Exit_AnimSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x06000A1C RID: 2588 RVA: 0x00020042 File Offset: 0x0001E242
	protected virtual float m_spinAttack_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000575 RID: 1397
	// (get) Token: 0x06000A1D RID: 2589 RVA: 0x00020049 File Offset: 0x0001E249
	protected virtual float m_spinAttack_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x06000A1E RID: 2590 RVA: 0x00020050 File Offset: 0x0001E250
	protected virtual float m_spinAttack_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000577 RID: 1399
	// (get) Token: 0x06000A1F RID: 2591 RVA: 0x00020057 File Offset: 0x0001E257
	protected virtual float m_spinAttack_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000578 RID: 1400
	// (get) Token: 0x06000A20 RID: 2592 RVA: 0x0002005E File Offset: 0x0001E25E
	protected virtual float m_spinAttack_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x06000A21 RID: 2593 RVA: 0x00020065 File Offset: 0x0001E265
	protected virtual float m_spinAttack_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x06000A22 RID: 2594 RVA: 0x0002006C File Offset: 0x0001E26C
	protected virtual float m_spinAttack_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x06000A23 RID: 2595 RVA: 0x00020073 File Offset: 0x0001E273
	protected virtual float m_spinAttack_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700057C RID: 1404
	// (get) Token: 0x06000A24 RID: 2596 RVA: 0x0002007A File Offset: 0x0001E27A
	protected virtual float m_spinAttack_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x06000A25 RID: 2597 RVA: 0x00020081 File Offset: 0x0001E281
	protected virtual float m_spinAttack_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x06000A26 RID: 2598 RVA: 0x00020088 File Offset: 0x0001E288
	protected virtual float m_spinAttack_Exit_AttackCD
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x1700057F RID: 1407
	// (get) Token: 0x06000A27 RID: 2599 RVA: 0x0002008F File Offset: 0x0001E28F
	protected virtual float m_spinAttack_TimesShotDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000580 RID: 1408
	// (get) Token: 0x06000A28 RID: 2600 RVA: 0x00020096 File Offset: 0x0001E296
	protected virtual int m_spinAttack_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000581 RID: 1409
	// (get) Token: 0x06000A29 RID: 2601 RVA: 0x00020099 File Offset: 0x0001E299
	protected virtual int m_spinAttack_ShotPatternLoops
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x17000582 RID: 1410
	// (get) Token: 0x06000A2A RID: 2602 RVA: 0x0002009C File Offset: 0x0001E29C
	protected virtual float m_spinAttack_projectile_RandomSpread
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x000200A3 File Offset: 0x0001E2A3
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator SpinAttack()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		bool alwaysFacing = true;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert || base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			alwaysFacing = false;
		}
		base.EnemyController.AlwaysFacing = alwaysFacing;
		yield return this.Default_TellIntroAndLoop("Shoot_Tell_Intro", this.m_spinAttack_TellIntro_AnimSpeed, "Shoot_Tell_Hold", this.m_spinAttack_TellHold_AnimSpeed, this.m_spinAttack_Tell_Delay);
		yield return this.Default_Animation("Shoot_Attack_Intro", this.m_spinAttack_AttackIntro_AnimSpeed, this.m_spinAttack_AttackIntro_Delay, true);
		yield return this.Default_Animation("Shoot_Attack_Hold", this.m_spinAttack_AttackHold_AnimSpeed, 0f, false);
		float num = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.transform.position);
		int num2;
		for (int i = 0; i < this.m_spinAttack_ShotPatternLoops; i = num2 + 1)
		{
			num = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.transform.position);
			for (int j = 0; j < this.m_spinAttack_TotalShots; j++)
			{
				float angle = num + UnityEngine.Random.Range(-this.m_spinAttack_projectile_RandomSpread, this.m_spinAttack_projectile_RandomSpread);
				this.FireProjectile("ElementalFireBoltLargeProjectile", 0, false, angle, 1f, true, true, true);
			}
			if (i < this.m_spinAttack_ShotPatternLoops - 1 && this.m_spinAttack_TimesShotDelay > 0f)
			{
				yield return base.Wait(this.m_spinAttack_TimesShotDelay, false);
			}
			num2 = i;
		}
		yield return this.Default_Animation("Shoot_Exit", this.m_spinAttack_Exit_AnimSpeed, this.m_spinAttack_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_spinAttack_Exit_ForceIdle, this.m_spinAttack_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x04000EA0 RID: 3744
	protected const string MOVE01_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x04000EA1 RID: 3745
	protected const string MOVE01_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x04000EA2 RID: 3746
	protected const string MOVE01_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x04000EA3 RID: 3747
	protected const string MOVE01_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x04000EA4 RID: 3748
	protected const string MOVE01_EXIT = "Shoot_Exit";

	// Token: 0x04000EA5 RID: 3749
	protected const string MOVE01_PROJECTILE = "ElementalFireBoltProjectile";

	// Token: 0x04000EA6 RID: 3750
	protected const string SPINATTACK_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x04000EA7 RID: 3751
	protected const string SPINATTACK_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x04000EA8 RID: 3752
	protected const string SPINATTACK_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x04000EA9 RID: 3753
	protected const string SPINATTACK_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x04000EAA RID: 3754
	protected const string SPINATTACK_EXIT = "Shoot_Exit";

	// Token: 0x04000EAB RID: 3755
	protected const string SPINATTACK_PROJECTILE = "ElementalFireBoltLargeProjectile";
}
