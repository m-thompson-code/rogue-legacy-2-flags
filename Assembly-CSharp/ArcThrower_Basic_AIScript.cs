using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200008C RID: 140
public class ArcThrower_Basic_AIScript : BaseAIScript
{
	// Token: 0x060001E2 RID: 482 RVA: 0x00011E49 File Offset: 0x00010049
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"ArcThrowerCurseBoltProjectile",
			"ArcThrowerBoltProjectile",
			"ArcThrowerPotionProjectile",
			"ArcThrowerPotionExplosionProjectile",
			"ArcThrowerTimeBombProjectile",
			"ArcThrowerTimeBombExplosionProjectile"
		};
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x060001E3 RID: 483 RVA: 0x00011E87 File Offset: 0x00010087
	protected virtual float m_shoot_ProjectileDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x060001E4 RID: 484 RVA: 0x00011E8E File Offset: 0x0001008E
	protected virtual float m_shoot_ProjectileAmount
	{
		get
		{
			return 12f;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x060001E5 RID: 485 RVA: 0x00011E95 File Offset: 0x00010095
	protected virtual float m_shoot_NumberMediumShots
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x060001E6 RID: 486 RVA: 0x00011E9C File Offset: 0x0001009C
	protected virtual float m_shoot_NumberHighShots
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x060001E7 RID: 487 RVA: 0x00011EA3 File Offset: 0x000100A3
	protected virtual bool m_shoot_Explosive_Bullets
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x00011EA6 File Offset: 0x000100A6
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Shoot()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		base.Animator.SetBool("Shoot_Explosive_Bullets", this.m_shoot_Explosive_Bullets);
		yield return this.Default_TellIntroAndLoop("ChestBone_Tell_Intro", this.m_shoot_TellIntro_AnimationSpeed, "ChestBone_Tell_Hold", this.m_shoot_TellHold_AnimationSpeed, this.m_shoot_TellIntroAndHold_Delay);
		yield return this.Default_Animation("ChestBone_Attack_Intro", this.m_shoot_AttackIntro_AnimationSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("ChestBone_Attack_Hold", this.m_shoot_AttackHold_AnimationSpeed, 0f, false);
		if (this.m_shoot_Explosive_Bullets)
		{
			this.FireProjectile("ArcThrowerTimeBombProjectile", 0, true, this.m_LowShotAngle, 1f, true, true, true);
			this.FireProjectile("ArcThrowerTimeBombProjectile", 0, true, this.m_HighShotAngle, 1f, true, true, true);
		}
		else
		{
			this.FireProjectile("ArcThrowerCurseBoltProjectile", 0, true, this.m_LowShotAngle, 1f, true, true, true);
			if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
			{
				this.FireProjectile("ArcThrowerCurseBoltProjectile", 0, true, this.m_HighShotAngle, 1f, true, true, true);
			}
		}
		if (this.m_shoot_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.m_shoot_AttackHold_Delay, false);
		}
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("ChestBone_Exit", this.m_shoot_Exit_AnimationSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_AttackCD);
		yield break;
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x060001E9 RID: 489 RVA: 0x00011EB5 File Offset: 0x000100B5
	protected virtual float m_spray_ProjectileDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x060001EA RID: 490 RVA: 0x00011EBC File Offset: 0x000100BC
	protected virtual float m_spray_ProjectileAmount
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x060001EB RID: 491 RVA: 0x00011EC3 File Offset: 0x000100C3
	protected virtual float m_sprayProjectilesPerLoop
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x060001EC RID: 492 RVA: 0x00011ECA File Offset: 0x000100CA
	protected virtual float m_spray_LowAngle
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x060001ED RID: 493 RVA: 0x00011ED1 File Offset: 0x000100D1
	protected virtual float m_spray_SpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060001EE RID: 494 RVA: 0x00011ED8 File Offset: 0x000100D8
	protected virtual float m_spray_TiltSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060001EF RID: 495 RVA: 0x00011EDF File Offset: 0x000100DF
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Spray()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_TellIntroAndLoop("Fire_Tell_Intro", this.m_spray_TellIntro_AnimationSpeed, "Fire_Tell_Hold", this.m_spray_TellHold_AnimationSpeed, this.m_spray_TellIntroAndHold_Delay);
		yield return this.Default_Animation("Fire_Attack_Intro", this.m_spray_AttackIntro_AnimationSpeed, this.m_spray_AttackIntro_Delay, true);
		yield return this.Default_Animation("Fire_Attack_Hold", this.m_spray_AttackHold_AnimationSpeed, 0f, false);
		int i = 0;
		while ((float)i < this.m_spray_ProjectileAmount)
		{
			if (this.m_sprayProjectilesPerLoop >= 1f)
			{
				this.FireProjectile("ArcThrowerBoltProjectile", 1, true, this.m_spray_LowAngle + (float)i * this.m_spray_TiltSpeed, 1.1f * this.m_spray_SpeedMod, true, true, true);
			}
			if (this.m_sprayProjectilesPerLoop >= 2f)
			{
				this.FireProjectile("ArcThrowerBoltProjectile", 1, true, this.m_spray_MediumAngle + (float)i * this.m_spray_TiltSpeed, 0.7f * this.m_spray_SpeedMod, true, true, true);
			}
			if (this.m_sprayProjectilesPerLoop >= 3f)
			{
				this.FireProjectile("ArcThrowerBoltProjectile", 1, true, this.m_spray_HighAngle + (float)i * this.m_spray_TiltSpeed, 0.7f * this.m_spray_SpeedMod, true, true, true);
			}
			yield return base.Wait(this.m_spray_ProjectileDelay, false);
			int num = i;
			i = num + 1;
		}
		if (this.m_spray_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.m_spray_AttackHold_Delay, false);
		}
		base.EnemyController.AlwaysFacing = true;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("Fire_Exit", this.m_spray_Exit_AnimationSpeed, this.m_spray_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_spray_Exit_ForceIdle, this.m_spray_AttackCD);
		yield break;
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x060001F0 RID: 496 RVA: 0x00011EEE File Offset: 0x000100EE
	protected virtual Vector2 m_jumpPower
	{
		get
		{
			return new Vector2(12f, 28f);
		}
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x00011EFF File Offset: 0x000100FF
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Jump_Attack()
	{
		this.StopAndFaceTarget();
		yield return base.Wait(0.1f, false);
		yield return this.Default_Animation("Jump_Tell", this.m_jump_Tell_AnimationSpeed, this.m_jump_Tell_Delay, true);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.ChangeAnimationState("JumpUp");
		if (base.EnemyController.IsFacingRight)
		{
			base.SetVelocityX(this.m_jumpPower.x, false);
		}
		else
		{
			base.SetVelocityX(-this.m_jumpPower.x, false);
		}
		base.SetVelocityY(this.m_jumpPower.y, false);
		yield return base.Wait(0.05f, false);
		if (base.EnemyController.IsFacingRight)
		{
			base.EnemyController.JumpHorizontalVelocity = this.m_jumpPower.x;
		}
		else
		{
			base.EnemyController.JumpHorizontalVelocity = -this.m_jumpPower.x;
		}
		yield return base.WaitUntilIsGrounded();
		base.SetVelocityX(0f, false);
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_jump_Exit_ForceIdle, this.m_jump_Exit_AttackCD);
		yield break;
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x00011F0E File Offset: 0x0001010E
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Throw_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_TellIntroAndLoop("ThrowBone_Tell_Intro", this.m_throwBone_TellIntro_AnimationSpeed, "ThrowBone_Tell_Hold", this.m_throwBone_TellHold_AnimationSpeed, 1.2f);
		yield return this.Default_Animation("ThrowBone_Attack_Intro", this.m_throwBone_AttackIntro_AnimationSpeed, this.m_throwBone_AttackIntro_Delay, true);
		this.FireProjectile("ArcThrowerPotionProjectile", 0, true, (float)this.m_BoneNear_Angle, 1f, true, true, true);
		this.FireProjectile("ArcThrowerPotionProjectile", 0, true, (float)this.m_FarBoneAngle, 1f, true, true, true);
		yield return this.Default_Animation("ThrowBone_Attack_Hold", this.m_throwBone_AttackHold_AnimationSpeed, 0f, false);
		yield return base.Wait(0.6f, false);
		yield return base.WaitUntilIsGrounded();
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("ThrowBone_Exit", this.m_throwBone_Exit_AnimationSpeed, this.m_throwBone_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_throwBone_Exit_ForceIdle, this.m_throwBone_AttackCD);
		yield break;
	}

	// Token: 0x040005EB RID: 1515
	protected float m_shoot_TellIntro_AnimationSpeed = 1f;

	// Token: 0x040005EC RID: 1516
	protected float m_shoot_TellHold_AnimationSpeed = 1f;

	// Token: 0x040005ED RID: 1517
	protected float m_shoot_TellIntroAndHold_Delay = 1.4f;

	// Token: 0x040005EE RID: 1518
	protected float m_shoot_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x040005EF RID: 1519
	protected float m_shoot_AttackIntro_Delay;

	// Token: 0x040005F0 RID: 1520
	protected float m_shoot_AttackHold_AnimationSpeed = 1f;

	// Token: 0x040005F1 RID: 1521
	protected float m_shoot_AttackHold_Delay = 1f;

	// Token: 0x040005F2 RID: 1522
	protected float m_shoot_Exit_AnimationSpeed = 1f;

	// Token: 0x040005F3 RID: 1523
	protected float m_shoot_Exit_Delay;

	// Token: 0x040005F4 RID: 1524
	protected float m_shoot_Exit_ForceIdle = 0.15f;

	// Token: 0x040005F5 RID: 1525
	protected float m_shoot_AttackCD = 2.5f;

	// Token: 0x040005F6 RID: 1526
	protected float m_LowShotAngle = 22f;

	// Token: 0x040005F7 RID: 1527
	protected float m_MediumShotAngle = 60f;

	// Token: 0x040005F8 RID: 1528
	protected float m_HighShotAngle = 80f;

	// Token: 0x040005F9 RID: 1529
	protected float m_spray_TellIntro_AnimationSpeed = 1.1f;

	// Token: 0x040005FA RID: 1530
	protected float m_spray_TellHold_AnimationSpeed = 1.1f;

	// Token: 0x040005FB RID: 1531
	protected float m_spray_TellIntroAndHold_Delay = 0.675f;

	// Token: 0x040005FC RID: 1532
	protected float m_spray_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x040005FD RID: 1533
	protected float m_spray_AttackIntro_Delay;

	// Token: 0x040005FE RID: 1534
	protected float m_spray_AttackHold_AnimationSpeed = 1f;

	// Token: 0x040005FF RID: 1535
	protected float m_spray_AttackHold_Delay;

	// Token: 0x04000600 RID: 1536
	protected float m_spray_Exit_AnimationSpeed = 1f;

	// Token: 0x04000601 RID: 1537
	protected float m_spray_Exit_Delay;

	// Token: 0x04000602 RID: 1538
	protected float m_spray_Exit_ForceIdle = 0.15f;

	// Token: 0x04000603 RID: 1539
	protected float m_spray_AttackCD;

	// Token: 0x04000604 RID: 1540
	protected float m_spray_MediumAngle = 50f;

	// Token: 0x04000605 RID: 1541
	protected float m_spray_HighAngle = 78f;

	// Token: 0x04000606 RID: 1542
	protected float m_jump_Tell_AnimationSpeed = 1f;

	// Token: 0x04000607 RID: 1543
	protected float m_jump_Tell_Delay = 0.55f;

	// Token: 0x04000608 RID: 1544
	protected float m_jump_AttackIntro_AnimationSpeed = 1.25f;

	// Token: 0x04000609 RID: 1545
	protected float m_jump_AttackIntro_Delay;

	// Token: 0x0400060A RID: 1546
	protected float m_jump_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x0400060B RID: 1547
	protected float m_jump_AttackHold_Delay;

	// Token: 0x0400060C RID: 1548
	protected float m_jump_Exit_AnimationSpeed = 1.25f;

	// Token: 0x0400060D RID: 1549
	protected float m_jump_Exit_Delay;

	// Token: 0x0400060E RID: 1550
	protected float m_jump_Exit_ForceIdle;

	// Token: 0x0400060F RID: 1551
	protected float m_jump_Exit_AttackCD = 10f;

	// Token: 0x04000610 RID: 1552
	protected float m_throwBone_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000611 RID: 1553
	protected float m_throwBone_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000612 RID: 1554
	protected const float m_throwBone_TellIntroAndHold_Delay = 1.2f;

	// Token: 0x04000613 RID: 1555
	protected float m_throwBone_AttackIntro_AnimationSpeed = 0.8f;

	// Token: 0x04000614 RID: 1556
	protected float m_throwBone_AttackIntro_Delay;

	// Token: 0x04000615 RID: 1557
	protected float m_throwBone_AttackHold_AnimationSpeed = 0.8f;

	// Token: 0x04000616 RID: 1558
	protected const float m_throwBone_AttackHold_Delay = 0.6f;

	// Token: 0x04000617 RID: 1559
	protected float m_throwBone_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000618 RID: 1560
	protected float m_throwBone_Exit_Delay = 0.15f;

	// Token: 0x04000619 RID: 1561
	protected int m_BoneNear_Angle = 83;

	// Token: 0x0400061A RID: 1562
	protected int m_FarBoneAngle = 70;

	// Token: 0x0400061B RID: 1563
	protected float m_throwBone_Exit_ForceIdle = 0.15f;

	// Token: 0x0400061C RID: 1564
	protected float m_throwBone_AttackCD;
}
