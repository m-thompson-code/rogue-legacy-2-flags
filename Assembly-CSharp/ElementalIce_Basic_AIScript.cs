using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000108 RID: 264
public class ElementalIce_Basic_AIScript : BaseAIScript
{
	// Token: 0x0600062B RID: 1579 RVA: 0x0000532F File Offset: 0x0000352F
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"ElementalIceBoltProjectile",
			"ElementalIceBoltExplosionProjectile"
		};
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x0600062C RID: 1580 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x0600062D RID: 1581 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x0600062E RID: 1582 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x0600062F RID: 1583 RVA: 0x00005065 File Offset: 0x00003265
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-5f, 5f);
		}
	}

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06000630 RID: 1584 RVA: 0x00005076 File Offset: 0x00003276
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(2f, 5f);
		}
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x0000534D File Offset: 0x0000354D
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		enemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x0000536C File Offset: 0x0000356C
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			base.EnemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
		}
	}

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06000633 RID: 1587 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x06000634 RID: 1588 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x06000635 RID: 1589 RVA: 0x00004FFB File Offset: 0x000031FB
	protected virtual float m_shoot_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06000636 RID: 1590 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06000637 RID: 1591 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06000638 RID: 1592 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_shoot_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06000639 RID: 1593 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_shoot_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x0600063A RID: 1594 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x0600063B RID: 1595 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x0600063C RID: 1596 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_shoot_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700028F RID: 655
	// (get) Token: 0x0600063D RID: 1597 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x0600063E RID: 1598 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_shoot_CentreShot
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x0600063F RID: 1599 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float m_shoot_TimesShotDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06000640 RID: 1600 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_shoot_TotalSideShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06000641 RID: 1601 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_shoot_ShotPatternLoops
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x06000642 RID: 1602 RVA: 0x00005391 File Offset: 0x00003591
	protected virtual float m_shoot_Projectile_Spread
	{
		get
		{
			return 30f;
		}
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x00005398 File Offset: 0x00003598
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootIceball()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		bool alwaysFacing = true;
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert || base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			alwaysFacing = false;
		}
		base.EnemyController.AlwaysFacing = alwaysFacing;
		yield return this.Default_TellIntroAndLoop("Shoot_Tell_Intro", this.m_shoot_TellIntro_AnimSpeed, "Shoot_Tell_Hold", this.m_shoot_TellHold_AnimSpeed, this.m_shoot_Tell_Delay);
		yield return this.Default_Animation("Shoot_Attack_Intro", this.m_shoot_AttackIntro_AnimSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("Shoot_Attack_Hold", this.m_shoot_AttackHold_AnimSpeed, 0f, false);
		Vector2 pt = base.EnemyController.TargetController.Midpoint - base.EnemyController.transform.position;
		float initialFireAngle = CDGHelper.VectorToAngle(pt);
		float angleDelta = 0f;
		angleDelta = this.m_shoot_Projectile_Spread;
		if (this.m_shoot_CentreShot)
		{
			this.FireProjectile("ElementalIceBoltProjectile", 0, false, initialFireAngle, 1f, true, true, true);
		}
		int num2;
		for (int i = 0; i < this.m_shoot_ShotPatternLoops; i = num2 + 1)
		{
			for (int j = 1; j < this.m_shoot_TotalSideShots + 1; j++)
			{
				float num = initialFireAngle;
				num += angleDelta * (float)j;
				this.FireProjectile("ElementalIceBoltProjectile", 0, false, num, 1f, true, true, true);
				num = initialFireAngle;
				num -= angleDelta * (float)j;
				this.FireProjectile("ElementalIceBoltProjectile", 0, false, num, 1f, true, true, true);
			}
			if (i < this.m_shoot_ShotPatternLoops - 1 && this.m_shoot_TimesShotDelay > 0f)
			{
				yield return base.Wait(this.m_shoot_TimesShotDelay, false);
			}
			num2 = i;
		}
		yield return this.Default_Animation("Shoot_Exit", this.m_shoot_Exit_AnimSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06000644 RID: 1604 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinAttack_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06000645 RID: 1605 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinAttack_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06000646 RID: 1606 RVA: 0x00004FFB File Offset: 0x000031FB
	protected virtual float m_spinAttack_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06000647 RID: 1607 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinAttack_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06000648 RID: 1608 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spinAttack_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06000649 RID: 1609 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_spinAttack_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x0600064A RID: 1610 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_spinAttack_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x0600064B RID: 1611 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinAttack_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x0600064C RID: 1612 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spinAttack_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x0600064D RID: 1613 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_spinAttack_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x0600064E RID: 1614 RVA: 0x00003C70 File Offset: 0x00001E70
	protected virtual float m_spinAttack_Exit_AttackCD
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x0600064F RID: 1615 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float m_spinAttack_TimesShotDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06000650 RID: 1616 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_spinAttack_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06000651 RID: 1617 RVA: 0x00004762 File Offset: 0x00002962
	protected virtual int m_spinAttack_ShotPatternLoops
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x06000652 RID: 1618 RVA: 0x000053A7 File Offset: 0x000035A7
	protected virtual float m_spinAttack_projectile_RandomSpread
	{
		get
		{
			return 180f;
		}
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x000053AE File Offset: 0x000035AE
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
		yield return this.Default_TellIntroAndLoop("Spin_Tell_Intro", this.m_spinAttack_TellIntro_AnimSpeed, "Spin_Tell_Hold", this.m_spinAttack_TellHold_AnimSpeed, this.m_spinAttack_Tell_Delay);
		yield return this.Default_Animation("Spin_Attack_Intro", this.m_spinAttack_AttackIntro_AnimSpeed, this.m_spinAttack_AttackIntro_Delay, true);
		yield return this.Default_Animation("Spin_Attack_Hold", this.m_spinAttack_AttackHold_AnimSpeed, 0f, false);
		Vector2 pt = base.EnemyController.TargetController.Midpoint - base.EnemyController.transform.position;
		float initialFireAngle = CDGHelper.VectorToAngle(pt);
		if (this.m_spinAttack_TotalShots > 0)
		{
			int num = 360 / this.m_spinAttack_TotalShots;
		}
		int num2;
		for (int i = 0; i < this.m_spinAttack_ShotPatternLoops; i = num2 + 1)
		{
			for (int j = 0; j < this.m_spinAttack_TotalShots; j++)
			{
				float angle = initialFireAngle + UnityEngine.Random.Range(-this.m_spinAttack_projectile_RandomSpread, this.m_spinAttack_projectile_RandomSpread);
				this.FireProjectile("ElementalIceBoltProjectile", 0, false, angle, 1f, true, true, true);
			}
			if (i < this.m_spinAttack_ShotPatternLoops - 1 && this.m_spinAttack_TimesShotDelay > 0f)
			{
				yield return base.Wait(this.m_spinAttack_TimesShotDelay, false);
			}
			num2 = i;
		}
		yield return this.Default_Animation("Spin_Exit", this.m_spinAttack_Exit_AnimSpeed, this.m_spinAttack_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_spinAttack_Exit_ForceIdle, this.m_spinAttack_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x040009A2 RID: 2466
	[SerializeField]
	private int m_elementalType;

	// Token: 0x040009A3 RID: 2467
	protected const string ICEBALL_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x040009A4 RID: 2468
	protected const string ICEBALL_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x040009A5 RID: 2469
	protected const string ICEBALL_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x040009A6 RID: 2470
	protected const string ICEBALL_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x040009A7 RID: 2471
	protected const string ICEBALL_EXIT = "Shoot_Exit";

	// Token: 0x040009A8 RID: 2472
	protected const string ICEBALL_PROJECTILE = "ElementalIceBoltProjectile";

	// Token: 0x040009A9 RID: 2473
	protected const string SPINATTACK_TELL_INTRO = "Spin_Tell_Intro";

	// Token: 0x040009AA RID: 2474
	protected const string SPINATTACK_TELL_HOLD = "Spin_Tell_Hold";

	// Token: 0x040009AB RID: 2475
	protected const string SPINATTACK_ATTACK_INTRO = "Spin_Attack_Intro";

	// Token: 0x040009AC RID: 2476
	protected const string SPINATTACK_ATTACK_HOLD = "Spin_Attack_Hold";

	// Token: 0x040009AD RID: 2477
	protected const string SPINATTACK_EXIT = "Spin_Exit";

	// Token: 0x040009AE RID: 2478
	protected const string SPINATTACK_PROJECTILE = "ElementalIceBoltProjectile";
}
