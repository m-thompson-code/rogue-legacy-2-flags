using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000FC RID: 252
public class ElementalDash_Basic_AIScript : BaseAIScript
{
	// Token: 0x060005A6 RID: 1446 RVA: 0x00005134 File Offset: 0x00003334
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"ElementalDashVoidProjectile",
			"ElementalDashVoidZoneProjectile",
			"ElementalDashVoidZoneLargeProjectile",
			"ElementalDashVoidZoneWarningProjectile"
		};
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x060005A8 RID: 1448 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x060005AA RID: 1450 RVA: 0x00005065 File Offset: 0x00003265
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-5f, 5f);
		}
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x060005AB RID: 1451 RVA: 0x00005076 File Offset: 0x00003276
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(2f, 5f);
		}
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x00005162 File Offset: 0x00003362
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		enemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x00005181 File Offset: 0x00003381
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			base.EnemyController.Animator.SetInteger("ElementalType", this.m_elementalType);
		}
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x060005AE RID: 1454 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x060005AF RID: 1455 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00004FFB File Offset: 0x000031FB
	protected virtual float m_shoot_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x060005B1 RID: 1457 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x060005B2 RID: 1458 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_shoot_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x060005B4 RID: 1460 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_shoot_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_shoot_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x060005B6 RID: 1462 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x060005B7 RID: 1463 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_shoot_Exit_ForceIdle
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x060005B8 RID: 1464 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x060005B9 RID: 1465 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_shoot_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x060005BA RID: 1466 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_shoot_TotalShotDuration
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x060005BB RID: 1467 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_RandAngleOffset
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x060005BC RID: 1468 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_shoot_EnableSpreadShot
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x060005BD RID: 1469 RVA: 0x000046FA File Offset: 0x000028FA
	protected virtual int m_shoot_SpreadShot_Angle
	{
		get
		{
			return 10;
		}
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x000051A6 File Offset: 0x000033A6
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootVoidball()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_TellIntroAndLoop("Shoot_Tell_Intro", this.m_shoot_TellIntro_AnimSpeed, "Shoot_Tell_Hold", this.m_shoot_TellHold_AnimSpeed, this.m_shoot_Tell_Delay);
		yield return this.Default_Animation("Shoot_Attack_Intro", this.m_shoot_AttackIntro_AnimSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("Shoot_Attack_Hold", this.m_shoot_AttackHold_AnimSpeed, 0f, false);
		float fireAngle = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
		float shotInterval = this.m_shoot_TotalShotDuration / (float)this.m_shoot_TotalShots;
		int num2;
		for (int i = 0; i < this.m_shoot_TotalShots; i = num2 + 1)
		{
			float num = UnityEngine.Random.Range(-this.m_shoot_RandAngleOffset, this.m_shoot_RandAngleOffset);
			fireAngle += num;
			if (this.m_shoot_EnableSpreadShot)
			{
				this.FireProjectile("ElementalDashVoidProjectile", 0, false, fireAngle, 1f, true, true, true);
				this.FireProjectile("ElementalDashVoidProjectile", 0, false, fireAngle + (float)this.m_shoot_SpreadShot_Angle, 1f, true, true, true);
				this.FireProjectile("ElementalDashVoidProjectile", 0, false, fireAngle - (float)this.m_shoot_SpreadShot_Angle, 1f, true, true, true);
			}
			else
			{
				this.FireProjectile("ElementalDashVoidProjectile", 0, false, fireAngle, 1f, true, true, true);
				if (shotInterval > 0f && i < this.m_shoot_TotalShots - 1)
				{
					yield return base.Wait(shotInterval, false);
				}
			}
			num2 = i;
		}
		yield return this.Default_Animation("Shoot_Exit", this.m_shoot_Exit_AnimSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x060005BF RID: 1471 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_voidwall_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x060005C0 RID: 1472 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_voidWall_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x060005C1 RID: 1473 RVA: 0x00004FFB File Offset: 0x000031FB
	protected virtual float m_voidWall_Tell_Delay
	{
		get
		{
			return 1.1f;
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x060005C2 RID: 1474 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_voidWall_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x060005C3 RID: 1475 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_voidWall_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x060005C4 RID: 1476 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_voidWall_AttackHold_AnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x060005C5 RID: 1477 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_voidWall_AttackHold_Delay
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x060005C6 RID: 1478 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_voidWall_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x060005C7 RID: 1479 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_voidWall_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x060005C8 RID: 1480 RVA: 0x000050CB File Offset: 0x000032CB
	protected virtual float m_voidWall_Exit_ForceIdle
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x1700023E RID: 574
	// (get) Token: 0x060005C9 RID: 1481 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_voidWall_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x060005CA RID: 1482 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_voidWall_CreateCenterWall
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x060005CB RID: 1483 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_voidWall_CreateSideWalls
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x060005CC RID: 1484 RVA: 0x00004536 File Offset: 0x00002736
	protected virtual float m_voidWall_SideWallsOffset
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x060005CD RID: 1485 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_voidWall_LargeWalls
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x060005CE RID: 1486 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_voidWall_Appear_Delay
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x000051B5 File Offset: 0x000033B5
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator VoidWall()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_TellIntroAndLoop("Spin_Tell_Intro", this.m_voidwall_TellIntro_AnimSpeed, "Spin_Tell_Hold", this.m_voidWall_TellHold_AnimSpeed, this.m_voidWall_Tell_Delay);
		yield return this.Default_Animation("Spin_Attack_Intro", this.m_voidWall_AttackIntro_AnimSpeed, this.m_voidWall_AttackIntro_Delay, true);
		yield return this.Default_Animation("Spin_Attack_Hold", this.m_voidWall_AttackHold_AnimSpeed, 0f, false);
		Vector3 projectilePos = base.EnemyController.TargetController.transform.position;
		this.m_voidWallTellProjectile = this.FireProjectile("ElementalDashVoidZoneWarningProjectile", 1, true, 90f, 1f, true, true, true);
		projectilePos.z = this.m_voidWallTellProjectile.transform.position.z;
		this.m_voidWallTellProjectile.transform.position = projectilePos;
		if (this.m_voidWall_CreateSideWalls)
		{
			Vector3 vector = projectilePos;
			vector += new Vector3(this.m_voidWall_SideWallsOffset, 0f, 0f);
			this.m_voidWallTellProjectile2 = this.FireProjectile("ElementalDashVoidZoneWarningProjectile", 1, true, 90f, 1f, true, true, true);
			vector.z = this.m_voidWallTellProjectile2.transform.position.z;
			this.m_voidWallTellProjectile2.transform.position = vector;
			vector = projectilePos;
			vector += new Vector3(-this.m_voidWall_SideWallsOffset, 0f, 0f);
			this.m_voidWallTellProjectile3 = this.FireProjectile("ElementalDashVoidZoneWarningProjectile", 1, true, 90f, 1f, true, true, true);
			vector.z = this.m_voidWallTellProjectile3.transform.position.z;
			this.m_voidWallTellProjectile3.transform.position = vector;
		}
		if (this.m_voidWall_Appear_Delay > 0f)
		{
			yield return base.Wait(this.m_voidWall_Appear_Delay, false);
		}
		base.StopProjectile(ref this.m_voidWallTellProjectile);
		if (this.m_voidWall_CreateSideWalls)
		{
			base.StopProjectile(ref this.m_voidWallTellProjectile2);
			base.StopProjectile(ref this.m_voidWallTellProjectile3);
		}
		if (this.m_voidWall_CreateCenterWall)
		{
			if (this.m_voidWall_LargeWalls)
			{
				Projectile_RL projectile_RL = this.FireProjectile("ElementalDashVoidZoneLargeProjectile", 0, false, 0f, 0f, true, true, true);
				projectilePos.z = projectile_RL.transform.position.z;
				projectile_RL.transform.position = projectilePos;
			}
			else if (!this.m_voidWall_LargeWalls)
			{
				Projectile_RL projectile_RL2 = this.FireProjectile("ElementalDashVoidZoneProjectile", 0, false, 0f, 0f, true, true, true);
				projectilePos.z = projectile_RL2.transform.position.z;
				projectile_RL2.transform.position = projectilePos;
			}
		}
		if (this.m_voidWall_CreateSideWalls)
		{
			Vector3 position = projectilePos;
			position.x += this.m_voidWall_SideWallsOffset;
			Projectile_RL projectile_RL3 = this.FireProjectile("ElementalDashVoidZoneProjectile", 0, false, 0f, 1f, true, true, true);
			position.z = projectile_RL3.transform.position.z;
			projectile_RL3.transform.position = position;
			position = projectilePos;
			position.x -= this.m_voidWall_SideWallsOffset;
			Projectile_RL projectile_RL4 = this.FireProjectile("ElementalDashVoidZoneProjectile", 0, false, 0f, 1f, true, true, true);
			position.z = projectile_RL4.transform.position.z;
			projectile_RL4.transform.position = position;
		}
		yield return this.Default_Animation("Spin_Exit", this.m_voidWall_Exit_AnimSpeed, this.m_voidWall_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_voidWall_Exit_ForceIdle, this.m_voidWall_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x000051C4 File Offset: 0x000033C4
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.StopProjectile(ref this.m_voidWallTellProjectile);
		base.StopProjectile(ref this.m_voidWallTellProjectile2);
		base.StopProjectile(ref this.m_voidWallTellProjectile3);
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x00057D70 File Offset: 0x00055F70
	private float GetTargetGroundYPos()
	{
		LayerMask mask = 256;
		mask |= 2048;
		RaycastHit2D hit = Physics2D.Raycast(base.EnemyController.TargetController.Midpoint, Vector2.down, 100f, mask);
		if (hit)
		{
			return hit.point.y;
		}
		return base.EnemyController.TargetController.transform.position.y;
	}

	// Token: 0x04000970 RID: 2416
	[SerializeField]
	private int m_elementalType;

	// Token: 0x04000971 RID: 2417
	protected const string VOID_WALL_TELL_PROJECTILE = "ElementalDashVoidZoneWarningProjectile";

	// Token: 0x04000972 RID: 2418
	protected Projectile_RL m_voidWallTellProjectile;

	// Token: 0x04000973 RID: 2419
	protected Projectile_RL m_voidWallTellProjectile2;

	// Token: 0x04000974 RID: 2420
	protected Projectile_RL m_voidWallTellProjectile3;

	// Token: 0x04000975 RID: 2421
	protected const string VOIDBALL_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x04000976 RID: 2422
	protected const string VOIDBALL_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x04000977 RID: 2423
	protected const string VOIDBALL_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x04000978 RID: 2424
	protected const string VOIDBALL_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x04000979 RID: 2425
	protected const string VOIDBALL_EXIT = "Shoot_Exit";

	// Token: 0x0400097A RID: 2426
	protected const string VOIDBALL_PROJECTILE = "ElementalDashVoidProjectile";

	// Token: 0x0400097B RID: 2427
	protected const string VOIDWALL_TELL_INTRO = "Spin_Tell_Intro";

	// Token: 0x0400097C RID: 2428
	protected const string VOIDWALL_TELL_HOLD = "Spin_Tell_Hold";

	// Token: 0x0400097D RID: 2429
	protected const string VOIDWALL_ATTACK_INTRO = "Spin_Attack_Intro";

	// Token: 0x0400097E RID: 2430
	protected const string VOIDWALL_ATTACK_HOLD = "Spin_Attack_Hold";

	// Token: 0x0400097F RID: 2431
	protected const string VOIDWALL_EXIT = "Spin_Exit";

	// Token: 0x04000980 RID: 2432
	protected const string VOIDWALL_PROJECTILE = "ElementalDashVoidZoneProjectile";

	// Token: 0x04000981 RID: 2433
	protected const string VOIDWALL_PROJECTILE_LARGE = "ElementalDashVoidZoneLargeProjectile";
}
