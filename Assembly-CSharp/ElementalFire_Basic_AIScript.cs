using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class ElementalFire_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000433 RID: 1075 RVA: 0x0001597A File Offset: 0x00013B7A
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"ElementalFireBoltProjectile",
			"ElementalFireBoltLargePassProjectile"
		};
	}

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06000434 RID: 1076 RVA: 0x00015998 File Offset: 0x00013B98
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x06000435 RID: 1077 RVA: 0x000159A9 File Offset: 0x00013BA9
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(1.25f, 2f);
		}
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x06000436 RID: 1078 RVA: 0x000159BA File Offset: 0x00013BBA
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(1.25f, 2f);
		}
	}

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x06000437 RID: 1079 RVA: 0x000159CB File Offset: 0x00013BCB
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(3.5f, 10f);
		}
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06000438 RID: 1080 RVA: 0x000159DC File Offset: 0x00013BDC
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(1f, 6f);
		}
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x000159ED File Offset: 0x00013BED
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		enemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
		if (enemyController.EnemyRank == EnemyRank.Expert)
		{
			enemyController.Animator.SetBool("ThrowGiantBall", true);
		}
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x00015A26 File Offset: 0x00013C26
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			base.EnemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
		}
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x0600043B RID: 1083 RVA: 0x00015A4B File Offset: 0x00013C4B
	protected virtual float m_shoot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x0600043C RID: 1084 RVA: 0x00015A52 File Offset: 0x00013C52
	protected virtual float m_shoot_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x0600043D RID: 1085 RVA: 0x00015A59 File Offset: 0x00013C59
	protected virtual float m_shoot_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x0600043E RID: 1086 RVA: 0x00015A60 File Offset: 0x00013C60
	protected virtual float m_shoot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x0600043F RID: 1087 RVA: 0x00015A67 File Offset: 0x00013C67
	protected virtual float m_shoot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06000440 RID: 1088 RVA: 0x00015A6E File Offset: 0x00013C6E
	protected virtual float m_shoot_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x06000441 RID: 1089 RVA: 0x00015A75 File Offset: 0x00013C75
	protected virtual float m_shoot_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x06000442 RID: 1090 RVA: 0x00015A7C File Offset: 0x00013C7C
	protected virtual float m_shoot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x06000443 RID: 1091 RVA: 0x00015A83 File Offset: 0x00013C83
	protected virtual float m_shoot_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x06000444 RID: 1092 RVA: 0x00015A8A File Offset: 0x00013C8A
	protected virtual float m_shoot_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x06000445 RID: 1093 RVA: 0x00015A91 File Offset: 0x00013C91
	protected virtual float m_shoot_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x06000446 RID: 1094 RVA: 0x00015A98 File Offset: 0x00013C98
	protected virtual int m_shoot_TotalShots
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x06000447 RID: 1095 RVA: 0x00015A9B File Offset: 0x00013C9B
	protected virtual float m_shoot_TotalShotDuration
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x06000448 RID: 1096 RVA: 0x00015AA2 File Offset: 0x00013CA2
	protected virtual float m_shoot_RandAngleOffset
	{
		get
		{
			return 18f;
		}
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x00015AA9 File Offset: 0x00013CA9
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

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x0600044A RID: 1098 RVA: 0x00015AB8 File Offset: 0x00013CB8
	protected virtual float m_spinAttack_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x0600044B RID: 1099 RVA: 0x00015ABF File Offset: 0x00013CBF
	protected virtual float m_spinAttack_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x0600044C RID: 1100 RVA: 0x00015AC6 File Offset: 0x00013CC6
	protected virtual float m_spinAttack_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x0600044D RID: 1101 RVA: 0x00015ACD File Offset: 0x00013CCD
	protected virtual float m_spinAttack_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x0600044E RID: 1102 RVA: 0x00015AD4 File Offset: 0x00013CD4
	protected virtual float m_spinAttack_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x0600044F RID: 1103 RVA: 0x00015ADB File Offset: 0x00013CDB
	protected virtual float m_spinAttack_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x06000450 RID: 1104 RVA: 0x00015AE2 File Offset: 0x00013CE2
	protected virtual float m_spinAttack_AttackHold_PreDelay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x06000451 RID: 1105 RVA: 0x00015AE9 File Offset: 0x00013CE9
	protected virtual float m_spinAttack_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x06000452 RID: 1106 RVA: 0x00015AF0 File Offset: 0x00013CF0
	protected virtual float m_spinAttack_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x06000453 RID: 1107 RVA: 0x00015AF7 File Offset: 0x00013CF7
	protected virtual float m_spinAttack_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x06000454 RID: 1108 RVA: 0x00015AFE File Offset: 0x00013CFE
	protected virtual float m_spinAttack_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x06000455 RID: 1109 RVA: 0x00015B05 File Offset: 0x00013D05
	protected virtual float m_spinAttack_Exit_AttackCD
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x06000456 RID: 1110 RVA: 0x00015B0C File Offset: 0x00013D0C
	protected virtual float m_spinAttack_TimesShotDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x06000457 RID: 1111 RVA: 0x00015B13 File Offset: 0x00013D13
	protected virtual int m_spinAttack_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x06000458 RID: 1112 RVA: 0x00015B16 File Offset: 0x00013D16
	protected virtual int m_spinAttack_ShotPatternLoops
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x06000459 RID: 1113 RVA: 0x00015B19 File Offset: 0x00013D19
	protected virtual float m_spinAttack_projectile_RandomSpread
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x00015B20 File Offset: 0x00013D20
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

	// Token: 0x04000834 RID: 2100
	[SerializeField]
	private int m_elementalType;

	// Token: 0x04000835 RID: 2101
	protected const string ELEMENTAL_FIRE_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x04000836 RID: 2102
	protected const string ELEMENTAL_FIRE_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x04000837 RID: 2103
	protected const string ELEMENTAL_FIRE_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x04000838 RID: 2104
	protected const string ELEMENTAL_FIRE_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x04000839 RID: 2105
	protected const string ELEMENTAL_FIRE_EXIT = "Shoot_Exit";

	// Token: 0x0400083A RID: 2106
	protected const string ELEMENTAL_FIRE_PROJECTILE = "ElementalFireBoltProjectile";

	// Token: 0x0400083B RID: 2107
	protected const string SPINATTACK_TELL_INTRO = "Spin_Tell_Intro";

	// Token: 0x0400083C RID: 2108
	protected const string SPINATTACK_TELL_HOLD = "Spin_Tell_Hold";

	// Token: 0x0400083D RID: 2109
	protected const string SPINATTACK_ATTACK_INTRO = "Spin_Attack_Intro";

	// Token: 0x0400083E RID: 2110
	protected const string SPINATTACK_ATTACK_HOLD = "Spin_Attack_Hold";

	// Token: 0x0400083F RID: 2111
	protected const string SPINATTACK_EXIT = "Spin_Exit";

	// Token: 0x04000840 RID: 2112
	protected const string SPINATTACK_PROJECTILE = "ElementalFireBoltLargePassProjectile";
}
