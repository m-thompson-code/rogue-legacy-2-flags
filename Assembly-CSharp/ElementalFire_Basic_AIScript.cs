using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class ElementalFire_Basic_AIScript : BaseAIScript
{
	// Token: 0x060005E8 RID: 1512 RVA: 0x0000522D File Offset: 0x0000342D
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"ElementalFireBoltProjectile",
			"ElementalFireBoltLargePassProjectile"
		};
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x060005E9 RID: 1513 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x060005EA RID: 1514 RVA: 0x00004F67 File Offset: 0x00003167
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(1.25f, 2f);
		}
	}

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x060005EB RID: 1515 RVA: 0x00004F67 File Offset: 0x00003167
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(1.25f, 2f);
		}
	}

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x060005EC RID: 1516 RVA: 0x00004F78 File Offset: 0x00003178
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(3.5f, 10f);
		}
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x060005ED RID: 1517 RVA: 0x00004F89 File Offset: 0x00003189
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(1f, 6f);
		}
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x0000524B File Offset: 0x0000344B
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		enemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
		if (enemyController.EnemyRank == EnemyRank.Expert)
		{
			enemyController.Animator.SetBool("ThrowGiantBall", true);
		}
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x00005284 File Offset: 0x00003484
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			base.EnemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
		}
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x060005F0 RID: 1520 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x060005F1 RID: 1521 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x060005F2 RID: 1522 RVA: 0x00004FFB File Offset: 0x000031FB
	protected virtual float m_shoot_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x060005F3 RID: 1523 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x060005F4 RID: 1524 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x060005F5 RID: 1525 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_shoot_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x060005F6 RID: 1526 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_shoot_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x060005F7 RID: 1527 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x060005F8 RID: 1528 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x060005F9 RID: 1529 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_shoot_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x060005FA RID: 1530 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x060005FB RID: 1531 RVA: 0x000047A7 File Offset: 0x000029A7
	protected virtual int m_shoot_TotalShots
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x060005FC RID: 1532 RVA: 0x000052A9 File Offset: 0x000034A9
	protected virtual float m_shoot_TotalShotDuration
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x060005FD RID: 1533 RVA: 0x000052B0 File Offset: 0x000034B0
	protected virtual float m_shoot_RandAngleOffset
	{
		get
		{
			return 18f;
		}
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x000052B7 File Offset: 0x000034B7
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootFireball()
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

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x060005FF RID: 1535 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinAttack_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06000600 RID: 1536 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinAttack_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06000601 RID: 1537 RVA: 0x00004FFB File Offset: 0x000031FB
	protected virtual float m_spinAttack_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06000602 RID: 1538 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinAttack_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06000603 RID: 1539 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spinAttack_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06000604 RID: 1540 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_spinAttack_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06000605 RID: 1541 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_spinAttack_AttackHold_PreDelay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06000606 RID: 1542 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_spinAttack_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06000607 RID: 1543 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinAttack_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06000608 RID: 1544 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spinAttack_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06000609 RID: 1545 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_spinAttack_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x0600060A RID: 1546 RVA: 0x00003C62 File Offset: 0x00001E62
	protected virtual float m_spinAttack_Exit_AttackCD
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x0600060B RID: 1547 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float m_spinAttack_TimesShotDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x0600060C RID: 1548 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_spinAttack_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x0600060D RID: 1549 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_spinAttack_ShotPatternLoops
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x0600060E RID: 1550 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spinAttack_projectile_RandomSpread
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x000052C6 File Offset: 0x000034C6
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
		yield return this.Default_Animation("Spin_Attack_Intro", this.m_spinAttack_AttackIntro_AnimSpeed, 0f, false);
		yield return this.Default_Animation("Spin_Attack_Hold", this.m_spinAttack_AttackHold_AnimSpeed, 0f, false);
		float num = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.transform.position);
		int num2;
		for (int i = 0; i < this.m_spinAttack_ShotPatternLoops; i = num2 + 1)
		{
			num = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.transform.position);
			for (int j = 0; j < this.m_spinAttack_TotalShots; j++)
			{
				float angle = num + UnityEngine.Random.Range(-this.m_spinAttack_projectile_RandomSpread, this.m_spinAttack_projectile_RandomSpread);
				this.FireProjectile("ElementalFireBoltLargePassProjectile", 0, false, angle, 1f, true, true, true);
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

	// Token: 0x0400098C RID: 2444
	[SerializeField]
	private int m_elementalType;

	// Token: 0x0400098D RID: 2445
	protected const string ELEMENTAL_FIRE_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x0400098E RID: 2446
	protected const string ELEMENTAL_FIRE_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x0400098F RID: 2447
	protected const string ELEMENTAL_FIRE_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x04000990 RID: 2448
	protected const string ELEMENTAL_FIRE_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x04000991 RID: 2449
	protected const string ELEMENTAL_FIRE_EXIT = "Shoot_Exit";

	// Token: 0x04000992 RID: 2450
	protected const string ELEMENTAL_FIRE_PROJECTILE = "ElementalFireBoltProjectile";

	// Token: 0x04000993 RID: 2451
	protected const string SPINATTACK_TELL_INTRO = "Spin_Tell_Intro";

	// Token: 0x04000994 RID: 2452
	protected const string SPINATTACK_TELL_HOLD = "Spin_Tell_Hold";

	// Token: 0x04000995 RID: 2453
	protected const string SPINATTACK_ATTACK_INTRO = "Spin_Attack_Intro";

	// Token: 0x04000996 RID: 2454
	protected const string SPINATTACK_ATTACK_HOLD = "Spin_Attack_Hold";

	// Token: 0x04000997 RID: 2455
	protected const string SPINATTACK_EXIT = "Spin_Exit";

	// Token: 0x04000998 RID: 2456
	protected const string SPINATTACK_PROJECTILE = "ElementalFireBoltLargePassProjectile";
}
