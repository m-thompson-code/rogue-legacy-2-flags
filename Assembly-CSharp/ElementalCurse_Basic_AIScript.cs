using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public class ElementalCurse_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000569 RID: 1385 RVA: 0x00005047 File Offset: 0x00003247
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"ElementalCurseHomingProjectile",
			"ElementalCurseCurseBlueProjectile"
		};
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x0600056A RID: 1386 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x0600056B RID: 1387 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x0600056C RID: 1388 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x0600056D RID: 1389 RVA: 0x00005065 File Offset: 0x00003265
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-5f, 5f);
		}
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x0600056E RID: 1390 RVA: 0x00005076 File Offset: 0x00003276
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(2f, 5f);
		}
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x00005087 File Offset: 0x00003287
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		enemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x000050A6 File Offset: 0x000032A6
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			base.EnemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
		}
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x06000571 RID: 1393 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x06000572 RID: 1394 RVA: 0x00004536 File Offset: 0x00002736
	protected virtual float m_shoot_TellHold_AnimSpeed
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x06000573 RID: 1395 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_shoot_Tell_Delay
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x06000574 RID: 1396 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x06000575 RID: 1397 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001FD RID: 509
	// (get) Token: 0x06000576 RID: 1398 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_shoot_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x06000577 RID: 1399 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_shoot_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x06000578 RID: 1400 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x06000579 RID: 1401 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x0600057A RID: 1402 RVA: 0x000050CB File Offset: 0x000032CB
	protected virtual float m_shoot_Exit_ForceIdle
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x0600057B RID: 1403 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x0600057C RID: 1404 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_shoot_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x0600057D RID: 1405 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_TotalShotDuration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x000050D2 File Offset: 0x000032D2
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

	// Token: 0x17000205 RID: 517
	// (get) Token: 0x0600057F RID: 1407 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinAttack_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000206 RID: 518
	// (get) Token: 0x06000580 RID: 1408 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinAttack_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x06000581 RID: 1409 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected virtual float m_spinAttack_Tell_Delay
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x06000582 RID: 1410 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinAttack_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x06000583 RID: 1411 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spinAttack_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06000584 RID: 1412 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_spinAttack_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06000585 RID: 1413 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_spinAttack_AttackHold_PreDelay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06000586 RID: 1414 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_spinAttack_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06000587 RID: 1415 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spinAttack_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06000588 RID: 1416 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spinAttack_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06000589 RID: 1417 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_spinAttack_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x0600058A RID: 1418 RVA: 0x00003C62 File Offset: 0x00001E62
	protected virtual float m_spinAttack_Exit_AttackCD
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x0600058B RID: 1419 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float m_spinAttack_TimesShotDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x0600058C RID: 1420 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_spinAttack_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x0600058D RID: 1421 RVA: 0x000047A4 File Offset: 0x000029A4
	protected virtual int m_spinAttack_ShotPatternLoops
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x0600058E RID: 1422 RVA: 0x000050E1 File Offset: 0x000032E1
	protected virtual float m_spinAttack_projectile_RandomSpread
	{
		get
		{
			return 179f;
		}
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x000050E8 File Offset: 0x000032E8
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

	// Token: 0x0400095A RID: 2394
	[SerializeField]
	private int m_elementalType;

	// Token: 0x0400095B RID: 2395
	protected const string CURSEBALL_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x0400095C RID: 2396
	protected const string CURSEBALL_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x0400095D RID: 2397
	protected const string CURSEBALL_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x0400095E RID: 2398
	protected const string CURSEBALL_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x0400095F RID: 2399
	protected const string CURSEBALL_EXIT = "Shoot_Exit";

	// Token: 0x04000960 RID: 2400
	protected const string CURSEBALL_PROJECTILE = "ElementalCurseHomingProjectile";

	// Token: 0x04000961 RID: 2401
	protected const string SPINATTACK_TELL_INTRO = "Spin_Tell_Intro";

	// Token: 0x04000962 RID: 2402
	protected const string SPINATTACK_TELL_HOLD = "Spin_Tell_Hold";

	// Token: 0x04000963 RID: 2403
	protected const string SPINATTACK_ATTACK_INTRO = "Spin_Attack_Intro";

	// Token: 0x04000964 RID: 2404
	protected const string SPINATTACK_ATTACK_HOLD = "Spin_Attack_Hold";

	// Token: 0x04000965 RID: 2405
	protected const string SPINATTACK_EXIT = "Spin_Exit";

	// Token: 0x04000966 RID: 2406
	protected const string SPINATTACK_PROJECTILE = "ElementalCurseCurseBlueProjectile";
}
