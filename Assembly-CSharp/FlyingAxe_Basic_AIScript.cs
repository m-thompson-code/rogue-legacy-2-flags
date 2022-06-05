using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000D1 RID: 209
public class FlyingAxe_Basic_AIScript : BaseAIScript, IBodyOnEnterHitResponse, IHitResponse, IBodyOnStayHitResponse
{
	// Token: 0x060005FB RID: 1531 RVA: 0x00018C01 File Offset: 0x00016E01
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlyingAxeBounceBoltProjectile",
			"FlyingAxeFlameThrowerProjectile",
			"WallTurretFlameThrowerWarningProjectile_Template",
			"FlyingAxeFireballProjectile"
		};
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x060005FC RID: 1532 RVA: 0x00018C2F File Offset: 0x00016E2F
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.3f, 0.7f);
		}
	}

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x060005FD RID: 1533 RVA: 0x00018C40 File Offset: 0x00016E40
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.8f, 1.1f);
		}
	}

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x060005FE RID: 1534 RVA: 0x00018C51 File Offset: 0x00016E51
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.8f, 1.1f);
		}
	}

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x060005FF RID: 1535 RVA: 0x00018C62 File Offset: 0x00016E62
	protected virtual float m_sideSpin_Attack_Duration
	{
		get
		{
			return 5.5f;
		}
	}

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06000600 RID: 1536 RVA: 0x00018C69 File Offset: 0x00016E69
	protected virtual float m_sideSpin_Attack_TurnSpeed
	{
		get
		{
			return 85f;
		}
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06000601 RID: 1537 RVA: 0x00018C70 File Offset: 0x00016E70
	protected virtual float m_sideSpin_Attack_MovementSpeed
	{
		get
		{
			return 9f;
		}
	}

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06000602 RID: 1538 RVA: 0x00018C77 File Offset: 0x00016E77
	protected virtual float m_sideSpin_Exit_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06000603 RID: 1539 RVA: 0x00018C7E File Offset: 0x00016E7E
	protected virtual Vector2 m_sideSpin_InternalKnockBackMod
	{
		get
		{
			return new Vector2(1.75f, 1.75f);
		}
	}

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06000604 RID: 1540 RVA: 0x00018C8F File Offset: 0x00016E8F
	protected virtual float m_sideSpin_FlightRecoveryMod
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x00018C96 File Offset: 0x00016E96
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Side_Spin_Attack()
	{
		base.EnemyController.FollowTarget = true;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.FollowOffset = Vector3.zero;
		yield return this.Default_TellIntroAndLoop("SideSpin_Tell_Intro", this.m_sideSpin_TellIntro_AnimationSpeed, "SideSpin_Tell_Hold", this.m_sideSpin_TellHold_AnimationSpeed, this.m_sideSpin_TellIntroAndHold_Duration);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("SideSpin_Attack_Intro", this.m_sideSpin_AttackIntro_AnimationSpeed, this.m_sideSpin_AttackIntro_Delay, true);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_sideSpin_Attack_MovementSpeed;
		base.EnemyController.BaseTurnSpeed = this.m_sideSpin_Attack_TurnSpeed;
		base.EnemyController.InternalKnockbackMod = this.m_sideSpin_InternalKnockBackMod;
		base.EnemyController.FlightKnockbackDecelerationMod = this.m_sideSpin_FlightRecoveryMod;
		yield return this.Default_Animation("SideSpin_Attack_Hold", 1f, 0f, true);
		if (this.m_sideSpin_Attack_Duration > 0f)
		{
			yield return base.Wait(this.m_sideSpin_Attack_Duration, false);
		}
		base.EnemyController.ResetBaseValues();
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("SideSpin_Exit", this.m_sideSpin_ExitIntro_AnimationSpeed, 0.15f, true);
		base.EnemyController.ResetTurnTrigger();
		yield return this.Default_Attack_Cooldown(0.15f, this.m_sideSpin_Exit_AttackCD);
		yield break;
	}

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x06000606 RID: 1542 RVA: 0x00018CA5 File Offset: 0x00016EA5
	protected virtual float m_vertSpin_Attack_ChaseDuration
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x06000607 RID: 1543 RVA: 0x00018CAC File Offset: 0x00016EAC
	protected virtual float m_vertSpin_Attack_TurnSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x06000608 RID: 1544 RVA: 0x00018CB3 File Offset: 0x00016EB3
	protected virtual float m_vertSpin_Attack_MovementSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x06000609 RID: 1545 RVA: 0x00018CBA File Offset: 0x00016EBA
	protected virtual bool m_vertSpin_Attack_FireBulletsWhileSpinning
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x0600060A RID: 1546 RVA: 0x00018CBD File Offset: 0x00016EBD
	protected virtual int m_vertSpin_Attack_BulletShotPerLoop
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x170002FD RID: 765
	// (get) Token: 0x0600060B RID: 1547 RVA: 0x00018CC0 File Offset: 0x00016EC0
	protected virtual int m_vertSpin_Attack_TotalLoops
	{
		get
		{
			return 10;
		}
	}

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x0600060C RID: 1548 RVA: 0x00018CC4 File Offset: 0x00016EC4
	protected virtual Vector2 m_vertSpin_Attack_FireballSpeedMod
	{
		get
		{
			return new Vector2(0.5f, 1f);
		}
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x00018CD5 File Offset: 0x00016ED5
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Vertical_Spin_Attack()
	{
		base.EnemyController.AlwaysFacing = false;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.FollowOffset = Vector3.zero;
		this.m_flameTellProjectile1 = this.FireProjectile("WallTurretFlameThrowerWarningProjectile_Template", new Vector2(1f, 0f), false, 0f, 1f, true, true, true);
		this.m_flameTellProjectile2 = this.FireProjectile("WallTurretFlameThrowerWarningProjectile_Template", new Vector2(0f, 1f), false, 90f, 1f, true, true, true);
		this.m_flameTellProjectile3 = this.FireProjectile("WallTurretFlameThrowerWarningProjectile_Template", new Vector2(-1f, 0f), false, 180f, 1f, true, true, true);
		this.m_flameTellProjectile4 = this.FireProjectile("WallTurretFlameThrowerWarningProjectile_Template", new Vector2(0f, -1f), false, 270f, 1f, true, true, true);
		yield return this.Default_TellIntroAndLoop("VerticalSpin_Tell_Intro", this.m_vertSpin_TellIntro_AnimationSpeed, "VerticalSpin_Tell_Hold", this.m_vertSpin_TellHold_AnimationSpeed, this.m_vertSpin_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		base.StopProjectile(ref this.m_flameTellProjectile1);
		base.StopProjectile(ref this.m_flameTellProjectile2);
		base.StopProjectile(ref this.m_flameTellProjectile3);
		base.StopProjectile(ref this.m_flameTellProjectile4);
		yield return this.Default_Animation("VerticalSpin_Attack_Intro", this.m_vertSpin_AttackIntro_AnimationSpeed, this.m_vertSpin_AttackIntro_Delay, true);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_vertSpin_Attack_MovementSpeed;
		base.EnemyController.BaseTurnSpeed = this.m_vertSpin_Attack_TurnSpeed;
		yield return this.Default_Animation("VerticalSpin_Attack_Hold", 1f, 0f, false);
		this.m_flameThrowerProjectile1 = this.FireProjectile("FlyingAxeFlameThrowerProjectile", new Vector2(1f, 0f), false, 0f, 1f, true, true, true);
		this.m_flameThrowerProjectile2 = this.FireProjectile("FlyingAxeFlameThrowerProjectile", new Vector2(0f, 1f), false, 90f, 1f, true, true, true);
		this.m_flameThrowerProjectile3 = this.FireProjectile("FlyingAxeFlameThrowerProjectile", new Vector2(-1f, 0f), false, 180f, 1f, true, true, true);
		this.m_flameThrowerProjectile4 = this.FireProjectile("FlyingAxeFlameThrowerProjectile", new Vector2(0f, -1f), false, 270f, 1f, true, true, true);
		if (this.m_vertSpin_Attack_ChaseDuration > 0f)
		{
			if (this.m_vertSpin_Attack_FireBulletsWhileSpinning)
			{
				int angleDivider = 360 / this.m_vertSpin_Attack_BulletShotPerLoop;
				int num2;
				for (int i = 0; i < this.m_vertSpin_Attack_TotalLoops; i = num2 + 1)
				{
					for (int j = 0; j < this.m_vertSpin_Attack_BulletShotPerLoop; j++)
					{
						int num = UnityEngine.Random.Range(angleDivider * j, angleDivider * (j + 1));
						float speedMod = UnityEngine.Random.Range(this.m_vertSpin_Attack_FireballSpeedMod.x, this.m_vertSpin_Attack_FireballSpeedMod.y);
						this.FireProjectile("FlyingAxeFireballProjectile", new Vector2(0f, 0f), false, (float)num, speedMod, true, true, true);
					}
					yield return base.Wait(this.m_vertSpin_Attack_ChaseDuration / (float)this.m_vertSpin_Attack_TotalLoops, false);
					num2 = i;
				}
			}
			else
			{
				yield return base.Wait(this.m_vertSpin_Attack_ChaseDuration, false);
			}
		}
		base.EnemyController.ResetBaseValues();
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.StopProjectile(ref this.m_flameThrowerProjectile1);
		base.StopProjectile(ref this.m_flameThrowerProjectile2);
		base.StopProjectile(ref this.m_flameThrowerProjectile3);
		base.StopProjectile(ref this.m_flameThrowerProjectile4);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("VerticalSpin_Exit", this.m_vertSpin_ExitIntro_AnimationSpeed, 0.15f, true);
		base.EnemyController.ResetTurnTrigger();
		yield return this.Default_Attack_Cooldown(0.25f, this.m_vertSpin_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x00018CE4 File Offset: 0x00016EE4
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x00018CE6 File Offset: 0x00016EE6
	public void BodyOnStayHitResponse(IHitboxController otherHBController)
	{
		this.BodyOnEnterHitResponse(otherHBController);
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x00018CEF File Offset: 0x00016EEF
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.DisableDistanceThresholdCheck = true;
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x00018D04 File Offset: 0x00016F04
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.StopProjectile(ref this.m_flameTellProjectile1);
		base.StopProjectile(ref this.m_flameTellProjectile2);
		base.StopProjectile(ref this.m_flameTellProjectile3);
		base.StopProjectile(ref this.m_flameTellProjectile4);
		base.StopProjectile(ref this.m_flameThrowerProjectile1);
		base.StopProjectile(ref this.m_flameThrowerProjectile2);
		base.StopProjectile(ref this.m_flameThrowerProjectile3);
		base.StopProjectile(ref this.m_flameThrowerProjectile4);
	}

	// Token: 0x04000A43 RID: 2627
	protected float m_sideSpin_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000A44 RID: 2628
	protected float m_sideSpin_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000A45 RID: 2629
	protected float m_sideSpin_TellIntroAndHold_Duration = 1f;

	// Token: 0x04000A46 RID: 2630
	protected float m_sideSpin_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000A47 RID: 2631
	protected float m_sideSpin_AttackIntro_Delay;

	// Token: 0x04000A48 RID: 2632
	protected const float m_sideSpin_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000A49 RID: 2633
	protected const float m_sideSpin_AttackHold_Delay = 0f;

	// Token: 0x04000A4A RID: 2634
	protected float m_sideSpin_ExitIntro_AnimationSpeed = 1f;

	// Token: 0x04000A4B RID: 2635
	protected const float m_sideSpin_ExitIntro_EndDelay = 0.15f;

	// Token: 0x04000A4C RID: 2636
	protected const float m_sideSpin_Exit_ForceIdle = 0.15f;

	// Token: 0x04000A4D RID: 2637
	protected float m_vertSpin_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000A4E RID: 2638
	protected float m_vertSpin_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000A4F RID: 2639
	protected float m_vertSpin_TellIntroAndHold_Delay = 1.2f;

	// Token: 0x04000A50 RID: 2640
	protected float m_vertSpin_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000A51 RID: 2641
	protected float m_vertSpin_AttackIntro_Delay;

	// Token: 0x04000A52 RID: 2642
	protected const float m_vertSpin_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000A53 RID: 2643
	protected const float m_vertSpin_AttackHold_Delay = 0f;

	// Token: 0x04000A54 RID: 2644
	protected float m_vertSpin_ExitIntro_AnimationSpeed = 1f;

	// Token: 0x04000A55 RID: 2645
	protected const float m_vertSpin_ExitIntro_EndDelay = 0.15f;

	// Token: 0x04000A56 RID: 2646
	protected const float m_vertSpin_Exit_ForceIdle = 0.25f;

	// Token: 0x04000A57 RID: 2647
	protected float m_vertSpin_Exit_AttackCD = 5f;

	// Token: 0x04000A58 RID: 2648
	private Projectile_RL m_flameThrowerProjectile1;

	// Token: 0x04000A59 RID: 2649
	private Projectile_RL m_flameThrowerProjectile2;

	// Token: 0x04000A5A RID: 2650
	private Projectile_RL m_flameThrowerProjectile3;

	// Token: 0x04000A5B RID: 2651
	private Projectile_RL m_flameThrowerProjectile4;

	// Token: 0x04000A5C RID: 2652
	private const string FLAME_TELL_PROJECTILE = "WallTurretFlameThrowerWarningProjectile_Template";

	// Token: 0x04000A5D RID: 2653
	private const string FIREBALL_PROJECTILE = "FlyingAxeFireballProjectile";

	// Token: 0x04000A5E RID: 2654
	protected Projectile_RL m_flameTellProjectile1;

	// Token: 0x04000A5F RID: 2655
	protected Projectile_RL m_flameTellProjectile2;

	// Token: 0x04000A60 RID: 2656
	protected Projectile_RL m_flameTellProjectile3;

	// Token: 0x04000A61 RID: 2657
	protected Projectile_RL m_flameTellProjectile4;
}
