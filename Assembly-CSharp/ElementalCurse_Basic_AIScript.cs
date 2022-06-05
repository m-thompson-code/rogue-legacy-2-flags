using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class ElementalCurse_Basic_AIScript : BaseAIScript
{
	// Token: 0x060003CC RID: 972 RVA: 0x00015510 File Offset: 0x00013710
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"ElementalCurseHomingProjectile",
			"ElementalCurseCurseBlueProjectile"
		};
	}

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x060003CD RID: 973 RVA: 0x0001552E File Offset: 0x0001372E
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x060003CE RID: 974 RVA: 0x0001553F File Offset: 0x0001373F
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x060003CF RID: 975 RVA: 0x00015550 File Offset: 0x00013750
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x060003D0 RID: 976 RVA: 0x00015561 File Offset: 0x00013761
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-5f, 5f);
		}
	}

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x060003D1 RID: 977 RVA: 0x00015572 File Offset: 0x00013772
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(2f, 5f);
		}
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x00015583 File Offset: 0x00013783
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		enemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x000155A2 File Offset: 0x000137A2
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			base.EnemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
		}
	}

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x060003D4 RID: 980 RVA: 0x000155C7 File Offset: 0x000137C7
	protected virtual float m_shoot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x060003D5 RID: 981 RVA: 0x000155CE File Offset: 0x000137CE
	protected virtual float m_shoot_TellHold_AnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x060003D6 RID: 982 RVA: 0x000155D5 File Offset: 0x000137D5
	protected virtual float m_shoot_Tell_Delay
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x060003D7 RID: 983 RVA: 0x000155DC File Offset: 0x000137DC
	protected virtual float m_shoot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x060003D8 RID: 984 RVA: 0x000155E3 File Offset: 0x000137E3
	protected virtual float m_shoot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x060003D9 RID: 985 RVA: 0x000155EA File Offset: 0x000137EA
	protected virtual float m_shoot_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x060003DA RID: 986 RVA: 0x000155F1 File Offset: 0x000137F1
	protected virtual float m_shoot_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x060003DB RID: 987 RVA: 0x000155F8 File Offset: 0x000137F8
	protected virtual float m_shoot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x060003DC RID: 988 RVA: 0x000155FF File Offset: 0x000137FF
	protected virtual float m_shoot_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x060003DD RID: 989 RVA: 0x00015606 File Offset: 0x00013806
	protected virtual float m_shoot_Exit_ForceIdle
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x060003DE RID: 990 RVA: 0x0001560D File Offset: 0x0001380D
	protected virtual float m_shoot_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x060003DF RID: 991 RVA: 0x00015614 File Offset: 0x00013814
	protected virtual int m_shoot_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x060003E0 RID: 992 RVA: 0x00015617 File Offset: 0x00013817
	protected virtual float m_shoot_TotalShotDuration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x0001561E File Offset: 0x0001381E
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootCurseball()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_TellIntroAndLoop("Shoot_Tell_Intro", this.m_shoot_TellIntro_AnimSpeed, "Shoot_Tell_Hold", this.m_shoot_TellHold_AnimSpeed, this.m_shoot_Tell_Delay);
		yield return this.Default_Animation("Shoot_Attack_Intro", this.m_shoot_AttackIntro_AnimSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("Shoot_Attack_Hold", this.m_shoot_AttackHold_AnimSpeed, 0f, false);
		float shotInterval = this.m_shoot_TotalShotDuration / (float)this.m_shoot_TotalShots;
		int num;
		for (int i = 0; i < this.m_shoot_TotalShots; i = num + 1)
		{
			float angle = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
			this.FireProjectile("ElementalCurseHomingProjectile", 0, false, angle, 1f, true, true, true);
			if (shotInterval > 0f && i < this.m_shoot_TotalShots - 1)
			{
				yield return base.Wait(shotInterval, false);
			}
			num = i;
		}
		yield return this.Default_Animation("Shoot_Exit", this.m_shoot_Exit_AnimSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x060003E2 RID: 994 RVA: 0x0001562D File Offset: 0x0001382D
	protected virtual float m_spinAttack_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x060003E3 RID: 995 RVA: 0x00015634 File Offset: 0x00013834
	protected virtual float m_spinAttack_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x060003E4 RID: 996 RVA: 0x0001563B File Offset: 0x0001383B
	protected virtual float m_spinAttack_Tell_Delay
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x060003E5 RID: 997 RVA: 0x00015642 File Offset: 0x00013842
	protected virtual float m_spinAttack_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x060003E6 RID: 998 RVA: 0x00015649 File Offset: 0x00013849
	protected virtual float m_spinAttack_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x060003E7 RID: 999 RVA: 0x00015650 File Offset: 0x00013850
	protected virtual float m_spinAttack_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00015657 File Offset: 0x00013857
	protected virtual float m_spinAttack_AttackHold_PreDelay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0001565E File Offset: 0x0001385E
	protected virtual float m_spinAttack_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x060003EA RID: 1002 RVA: 0x00015665 File Offset: 0x00013865
	protected virtual float m_spinAttack_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x060003EB RID: 1003 RVA: 0x0001566C File Offset: 0x0001386C
	protected virtual float m_spinAttack_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x060003EC RID: 1004 RVA: 0x00015673 File Offset: 0x00013873
	protected virtual float m_spinAttack_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x060003ED RID: 1005 RVA: 0x0001567A File Offset: 0x0001387A
	protected virtual float m_spinAttack_Exit_AttackCD
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x060003EE RID: 1006 RVA: 0x00015681 File Offset: 0x00013881
	protected virtual float m_spinAttack_TimesShotDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x060003EF RID: 1007 RVA: 0x00015688 File Offset: 0x00013888
	protected virtual int m_spinAttack_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x060003F0 RID: 1008 RVA: 0x0001568B File Offset: 0x0001388B
	protected virtual int m_spinAttack_ShotPatternLoops
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x060003F1 RID: 1009 RVA: 0x0001568E File Offset: 0x0001388E
	protected virtual float m_spinAttack_projectile_RandomSpread
	{
		get
		{
			return 179f;
		}
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x00015695 File Offset: 0x00013895
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
				this.FireProjectile("ElementalCurseCurseBlueProjectile", 0, false, angle, 1f, true, true, true);
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

	// Token: 0x04000815 RID: 2069
	[SerializeField]
	private int m_elementalType;

	// Token: 0x04000816 RID: 2070
	protected const string CURSEBALL_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x04000817 RID: 2071
	protected const string CURSEBALL_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x04000818 RID: 2072
	protected const string CURSEBALL_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x04000819 RID: 2073
	protected const string CURSEBALL_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x0400081A RID: 2074
	protected const string CURSEBALL_EXIT = "Shoot_Exit";

	// Token: 0x0400081B RID: 2075
	protected const string CURSEBALL_PROJECTILE = "ElementalCurseHomingProjectile";

	// Token: 0x0400081C RID: 2076
	protected const string SPINATTACK_TELL_INTRO = "Spin_Tell_Intro";

	// Token: 0x0400081D RID: 2077
	protected const string SPINATTACK_TELL_HOLD = "Spin_Tell_Hold";

	// Token: 0x0400081E RID: 2078
	protected const string SPINATTACK_ATTACK_INTRO = "Spin_Attack_Intro";

	// Token: 0x0400081F RID: 2079
	protected const string SPINATTACK_ATTACK_HOLD = "Spin_Attack_Hold";

	// Token: 0x04000820 RID: 2080
	protected const string SPINATTACK_EXIT = "Spin_Exit";

	// Token: 0x04000821 RID: 2081
	protected const string SPINATTACK_PROJECTILE = "ElementalCurseCurseBlueProjectile";
}
