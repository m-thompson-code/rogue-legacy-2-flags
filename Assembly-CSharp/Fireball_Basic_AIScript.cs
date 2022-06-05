using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000C9 RID: 201
public class Fireball_Basic_AIScript : BaseAIScript, IBodyOnEnterHitResponse, IHitResponse
{
	// Token: 0x0600059D RID: 1437 RVA: 0x00018272 File Offset: 0x00016472
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.FIREBALL_PROJECTILE,
			this.FIREBALL_HOMING_PROJECTILE
		};
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x00018292 File Offset: 0x00016492
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.LogicController.OverrideLogicDelay(0.7f);
		this.m_trailRenderer = base.EnemyController.GetComponentInChildren<TrailRenderer>();
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x000182C1 File Offset: 0x000164C1
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_trailRenderer)
		{
			this.m_trailRenderer.Clear();
		}
	}

	// Token: 0x170002AF RID: 687
	// (get) Token: 0x060005A0 RID: 1440 RVA: 0x000182E1 File Offset: 0x000164E1
	protected virtual string FIREBALL_PROJECTILE
	{
		get
		{
			return "FireballEnemyProjectile";
		}
	}

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x060005A1 RID: 1441 RVA: 0x000182E8 File Offset: 0x000164E8
	protected virtual string FIREBALL_HOMING_PROJECTILE
	{
		get
		{
			return "FireballMinibossEnemyProjectile";
		}
	}

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x060005A2 RID: 1442 RVA: 0x000182EF File Offset: 0x000164EF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00018300 File Offset: 0x00016500
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.5f, 1.5f);
		}
	}

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x060005A4 RID: 1444 RVA: 0x00018311 File Offset: 0x00016511
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x060005A5 RID: 1445 RVA: 0x00018322 File Offset: 0x00016522
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x060005A6 RID: 1446 RVA: 0x00018329 File Offset: 0x00016529
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00018330 File Offset: 0x00016530
	protected virtual float m_fireballSpeedMultiplier
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x060005A8 RID: 1448 RVA: 0x00018337 File Offset: 0x00016537
	protected virtual bool m_dropsFireballsWhileWalking
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x060005A9 RID: 1449 RVA: 0x0001833A File Offset: 0x0001653A
	protected virtual float m_timeBetweenWalkTowardFireballDrops
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x00018344 File Offset: 0x00016544
	private void FixedUpdate()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (this.m_dropsFireballsWhileWalking && base.LogicController && base.LogicController.CurrentLogicBlockName != null)
		{
			if (base.LogicController.CurrentLogicBlockName.StartsWith("Walk"))
			{
				if (this.m_timeSinceLastWalkTowardsFireball >= this.m_timeBetweenWalkTowardFireballDrops)
				{
					this.DropFireball(false);
					this.m_timeSinceLastWalkTowardsFireball = 0f;
					return;
				}
				this.m_timeSinceLastWalkTowardsFireball += Time.deltaTime;
				return;
			}
			else
			{
				this.m_timeSinceLastWalkTowardsFireball = 0f;
			}
		}
	}

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x060005AB RID: 1451 RVA: 0x000183D2 File Offset: 0x000165D2
	protected virtual float m_dash_TellIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x060005AC RID: 1452 RVA: 0x000183D9 File Offset: 0x000165D9
	protected virtual float m_dash_TellIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x060005AD RID: 1453 RVA: 0x000183E0 File Offset: 0x000165E0
	protected virtual float m_dash_TellHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x060005AE RID: 1454 RVA: 0x000183E7 File Offset: 0x000165E7
	protected virtual float m_dash_TellHold_Delay
	{
		get
		{
			return 0.85f;
		}
	}

	// Token: 0x170002BD RID: 701
	// (get) Token: 0x060005AF RID: 1455 RVA: 0x000183EE File Offset: 0x000165EE
	protected virtual float m_dash_AttackIntro_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x060005B0 RID: 1456 RVA: 0x000183F5 File Offset: 0x000165F5
	protected virtual float m_dash_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x060005B1 RID: 1457 RVA: 0x000183FC File Offset: 0x000165FC
	protected virtual float m_dash_AttackHold_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x060005B2 RID: 1458 RVA: 0x00018403 File Offset: 0x00016603
	protected virtual float m_dash_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x060005B3 RID: 1459 RVA: 0x0001840A File Offset: 0x0001660A
	protected virtual float m_dash_Exit_AnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x060005B4 RID: 1460 RVA: 0x00018411 File Offset: 0x00016611
	protected virtual float m_dash_Exit_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00018418 File Offset: 0x00016618
	protected virtual float m_dash_Exit_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x060005B6 RID: 1462 RVA: 0x0001841F File Offset: 0x0001661F
	protected virtual float m_dash_Exit_AttackCD
	{
		get
		{
			return 5.5f;
		}
	}

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x060005B7 RID: 1463 RVA: 0x00018426 File Offset: 0x00016626
	protected virtual float m_dash_Attack_ForwardSpeedOverride
	{
		get
		{
			return 27.5f;
		}
	}

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x060005B8 RID: 1464 RVA: 0x0001842D File Offset: 0x0001662D
	protected virtual float m_dash_Attack_Duration
	{
		get
		{
			return 0.6f;
		}
	}

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x060005B9 RID: 1465 RVA: 0x00018434 File Offset: 0x00016634
	protected virtual bool m_dropsFireballsDuringDashAttack
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x060005BA RID: 1466 RVA: 0x00018437 File Offset: 0x00016637
	protected virtual float m_fireballDropDuringDashInterval
	{
		get
		{
			return 0.075f;
		}
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x0001843E File Offset: 0x0001663E
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator DashAttack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.DisableOffscreenWarnings = false;
		ProjectileManager.AttachOffscreenIcon(base.EnemyController, true);
		yield return this.Default_Animation("Dash_Tell_Intro", this.m_dash_TellIntro_AnimSpeed, this.m_dash_TellIntro_Delay, false);
		yield return this.Default_Animation("Dash_Tell_Hold", this.m_dash_TellHold_AnimSpeed, this.m_dash_TellHold_Delay, false);
		base.EnemyController.AttackingWithContactDamage = true;
		base.EnemyController.Heading = base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint;
		base.EnemyController.LockFlip = true;
		base.EnemyController.FollowTarget = false;
		yield return this.Default_Animation("Dash_Attack_Intro", this.m_dash_AttackIntro_AnimSpeed, this.m_dash_AttackIntro_Delay, true);
		yield return this.Default_Animation("Dash_Attack_Hold", this.m_dash_AttackHold_AnimSpeed, this.m_dash_AttackHold_Delay, false);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_dash_Attack_ForwardSpeedOverride;
		if (this.m_dropsFireballsDuringDashAttack)
		{
			yield return this.DropFireballDuringDash();
		}
		else if (this.m_dash_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_dash_Attack_Duration, false);
		}
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.AttackingWithContactDamage = false;
		yield return this.Default_Animation("Dash_Exit", this.m_dash_Exit_AnimSpeed, this.m_dash_Exit_Delay, true);
		base.EnemyController.DisableOffscreenWarnings = true;
		yield return this.Default_Attack_Cooldown(this.m_dash_Exit_ForceIdle, this.m_dash_Exit_AttackCD);
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		yield break;
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x0001844D File Offset: 0x0001664D
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator StunAttack()
	{
		this.StopAndFaceTarget();
		yield return this.Default_Animation("Stunned", this.m_dash_TellIntro_AnimSpeed, 5f, true);
		base.EnemyController.AttackingWithContactDamage = true;
		Vector2 pt = PlayerManager.GetPlayer().transform.position - base.EnemyController.transform.position;
		float angle = CDGHelper.VectorToAngle(pt);
		this.FireProjectile(this.FIREBALL_HOMING_PROJECTILE, 0, false, angle, this.m_fireballSpeedMultiplier, true, true, true);
		yield return base.Wait(0.15f, false);
		this.FireProjectile(this.FIREBALL_HOMING_PROJECTILE, 0, false, angle, this.m_fireballSpeedMultiplier, true, true, true);
		yield return base.Wait(0.15f, false);
		this.FireProjectile(this.FIREBALL_HOMING_PROJECTILE, 0, false, angle, this.m_fireballSpeedMultiplier, true, true, true);
		yield return base.Wait(0.15f, false);
		this.FireProjectile(this.FIREBALL_HOMING_PROJECTILE, 0, false, angle, this.m_fireballSpeedMultiplier, true, true, true);
		yield return base.Wait(0.15f, false);
		this.FireProjectile(this.FIREBALL_HOMING_PROJECTILE, 0, false, angle, this.m_fireballSpeedMultiplier, true, true, true);
		yield return base.Wait(0.15f, false);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.AttackingWithContactDamage = false;
		yield return this.Default_Attack_Cooldown(this.m_dash_Exit_ForceIdle, this.m_dash_Exit_AttackCD);
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		yield break;
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x0001845C File Offset: 0x0001665C
	private IEnumerator DropFireballDuringDash()
	{
		int numFireballs = (int)(this.m_dash_Attack_Duration / this.m_fireballDropDuringDashInterval);
		float remainingTime = this.m_dash_Attack_Duration - this.m_fireballDropDuringDashInterval * (float)numFireballs;
		int num;
		for (int i = 0; i < numFireballs; i = num + 1)
		{
			if (base.EnemyController.EnemyRank == EnemyRank.Miniboss)
			{
				this.DropFireball(false);
			}
			else
			{
				this.DropFireball(false);
			}
			if (this.m_fireballDropDuringDashInterval > 0f)
			{
				yield return base.Wait(this.m_fireballDropDuringDashInterval, false);
			}
			num = i;
		}
		if (remainingTime > 0f)
		{
			yield return base.Wait(remainingTime, false);
		}
		yield break;
	}

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x060005BE RID: 1470 RVA: 0x0001846B File Offset: 0x0001666B
	protected virtual int m_spawnFireballOnHitCount
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x060005BF RID: 1471 RVA: 0x0001846E File Offset: 0x0001666E
	protected virtual Vector2 m_spawnFireballOnHitSpeedModRange
	{
		get
		{
			return new Vector2(1f, 1f);
		}
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x00018480 File Offset: 0x00016680
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (this.m_spawnFireballOnHitCount > 0)
		{
			int num = 360 / (this.m_spawnFireballOnHitCount + 1);
			for (int i = 0; i < this.m_spawnFireballOnHitCount; i++)
			{
				float angle = (float)UnityEngine.Random.Range(num * i, num * (i + 1));
				float speedMod = this.m_spawnFireballOnHitSpeedModRange.x;
				if (this.m_spawnFireballOnHitSpeedModRange.x != this.m_spawnFireballOnHitSpeedModRange.y)
				{
					speedMod = UnityEngine.Random.Range(this.m_spawnFireballOnHitSpeedModRange.x, this.m_spawnFireballOnHitSpeedModRange.y);
				}
				this.FireProjectile(this.FIREBALL_PROJECTILE, new Vector2(0f, 0f), false, angle, speedMod, true, true, true);
			}
		}
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x0001852C File Offset: 0x0001672C
	private void DropFireball(bool homing = false)
	{
		float num = 270f;
		if (homing)
		{
			num = CDGHelper.VectorToAngle(PlayerManager.GetPlayer().transform.position - base.EnemyController.transform.position);
			this.FireProjectile(this.FIREBALL_HOMING_PROJECTILE, 0, false, num, this.m_fireballSpeedMultiplier, true, true, true);
			return;
		}
		if (base.EnemyController.EnemyRank == EnemyRank.Miniboss)
		{
			this.FireProjectile(this.FIREBALL_PROJECTILE, 0, false, num, this.m_fireballSpeedMultiplier, true, true, true);
			this.FireProjectile(this.FIREBALL_PROJECTILE, 0, false, num - 180f, this.m_fireballSpeedMultiplier, true, true, true);
			return;
		}
		this.FireProjectile(this.FIREBALL_PROJECTILE, 0, false, num, this.m_fireballSpeedMultiplier, true, true, true);
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x000185EB File Offset: 0x000167EB
	public override void OnLBCompleteOrCancelled()
	{
		base.EnemyController.FollowTarget = true;
		base.EnemyController.LockFlip = false;
		base.EnemyController.DisableOffscreenWarnings = true;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x040009F9 RID: 2553
	private TrailRenderer m_trailRenderer;

	// Token: 0x040009FA RID: 2554
	private float m_timeSinceLastWalkTowardsFireball;

	// Token: 0x040009FB RID: 2555
	protected const string DASH_TELL_INTRO = "Dash_Tell_Intro";

	// Token: 0x040009FC RID: 2556
	protected const string DASH_TELL_HOLD = "Dash_Tell_Hold";

	// Token: 0x040009FD RID: 2557
	protected const string DASH_ATTACK_INTRO = "Dash_Attack_Intro";

	// Token: 0x040009FE RID: 2558
	protected const string DASH_ATTACK_HOLD = "Dash_Attack_Hold";

	// Token: 0x040009FF RID: 2559
	protected const string DASH_EXIT = "Dash_Exit";
}
