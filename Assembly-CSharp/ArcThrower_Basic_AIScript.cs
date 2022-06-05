using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000094 RID: 148
public class ArcThrower_Basic_AIScript : BaseAIScript
{
	// Token: 0x060001F6 RID: 502 RVA: 0x00003C7F File Offset: 0x00001E7F
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

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x060001F7 RID: 503 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float m_shoot_ProjectileDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x060001F8 RID: 504 RVA: 0x00003CC4 File Offset: 0x00001EC4
	protected virtual float m_shoot_ProjectileAmount
	{
		get
		{
			return 12f;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x060001F9 RID: 505 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_NumberMediumShots
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x060001FA RID: 506 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_NumberHighShots
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060001FB RID: 507 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_shoot_Explosive_Bullets
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060001FC RID: 508 RVA: 0x00003CD5 File Offset: 0x00001ED5
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

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x060001FD RID: 509 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float m_spray_ProjectileDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x060001FE RID: 510 RVA: 0x00003C62 File Offset: 0x00001E62
	protected virtual float m_spray_ProjectileAmount
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x060001FF RID: 511 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_sprayProjectilesPerLoop
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x06000200 RID: 512 RVA: 0x00003CEB File Offset: 0x00001EEB
	protected virtual float m_spray_LowAngle
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x06000201 RID: 513 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_spray_SpeedMod
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x06000202 RID: 514 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_spray_TiltSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000203 RID: 515 RVA: 0x00003CF2 File Offset: 0x00001EF2
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

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x06000204 RID: 516 RVA: 0x00003D01 File Offset: 0x00001F01
	protected virtual Vector2 m_jumpPower
	{
		get
		{
			return new Vector2(12f, 28f);
		}
	}

	// Token: 0x06000205 RID: 517 RVA: 0x00003D12 File Offset: 0x00001F12
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

	// Token: 0x06000206 RID: 518 RVA: 0x00003D21 File Offset: 0x00001F21
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

	// Token: 0x0400060C RID: 1548
	protected float m_shoot_TellIntro_AnimationSpeed = 1f;

	// Token: 0x0400060D RID: 1549
	protected float m_shoot_TellHold_AnimationSpeed = 1f;

	// Token: 0x0400060E RID: 1550
	protected float m_shoot_TellIntroAndHold_Delay = 1.4f;

	// Token: 0x0400060F RID: 1551
	protected float m_shoot_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000610 RID: 1552
	protected float m_shoot_AttackIntro_Delay;

	// Token: 0x04000611 RID: 1553
	protected float m_shoot_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000612 RID: 1554
	protected float m_shoot_AttackHold_Delay = 1f;

	// Token: 0x04000613 RID: 1555
	protected float m_shoot_Exit_AnimationSpeed = 1f;

	// Token: 0x04000614 RID: 1556
	protected float m_shoot_Exit_Delay;

	// Token: 0x04000615 RID: 1557
	protected float m_shoot_Exit_ForceIdle = 0.15f;

	// Token: 0x04000616 RID: 1558
	protected float m_shoot_AttackCD = 2.5f;

	// Token: 0x04000617 RID: 1559
	protected float m_LowShotAngle = 22f;

	// Token: 0x04000618 RID: 1560
	protected float m_MediumShotAngle = 60f;

	// Token: 0x04000619 RID: 1561
	protected float m_HighShotAngle = 80f;

	// Token: 0x0400061A RID: 1562
	protected float m_spray_TellIntro_AnimationSpeed = 1.1f;

	// Token: 0x0400061B RID: 1563
	protected float m_spray_TellHold_AnimationSpeed = 1.1f;

	// Token: 0x0400061C RID: 1564
	protected float m_spray_TellIntroAndHold_Delay = 0.675f;

	// Token: 0x0400061D RID: 1565
	protected float m_spray_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x0400061E RID: 1566
	protected float m_spray_AttackIntro_Delay;

	// Token: 0x0400061F RID: 1567
	protected float m_spray_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000620 RID: 1568
	protected float m_spray_AttackHold_Delay;

	// Token: 0x04000621 RID: 1569
	protected float m_spray_Exit_AnimationSpeed = 1f;

	// Token: 0x04000622 RID: 1570
	protected float m_spray_Exit_Delay;

	// Token: 0x04000623 RID: 1571
	protected float m_spray_Exit_ForceIdle = 0.15f;

	// Token: 0x04000624 RID: 1572
	protected float m_spray_AttackCD;

	// Token: 0x04000625 RID: 1573
	protected float m_spray_MediumAngle = 50f;

	// Token: 0x04000626 RID: 1574
	protected float m_spray_HighAngle = 78f;

	// Token: 0x04000627 RID: 1575
	protected float m_jump_Tell_AnimationSpeed = 1f;

	// Token: 0x04000628 RID: 1576
	protected float m_jump_Tell_Delay = 0.55f;

	// Token: 0x04000629 RID: 1577
	protected float m_jump_AttackIntro_AnimationSpeed = 1.25f;

	// Token: 0x0400062A RID: 1578
	protected float m_jump_AttackIntro_Delay;

	// Token: 0x0400062B RID: 1579
	protected float m_jump_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x0400062C RID: 1580
	protected float m_jump_AttackHold_Delay;

	// Token: 0x0400062D RID: 1581
	protected float m_jump_Exit_AnimationSpeed = 1.25f;

	// Token: 0x0400062E RID: 1582
	protected float m_jump_Exit_Delay;

	// Token: 0x0400062F RID: 1583
	protected float m_jump_Exit_ForceIdle;

	// Token: 0x04000630 RID: 1584
	protected float m_jump_Exit_AttackCD = 10f;

	// Token: 0x04000631 RID: 1585
	protected float m_throwBone_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000632 RID: 1586
	protected float m_throwBone_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000633 RID: 1587
	protected const float m_throwBone_TellIntroAndHold_Delay = 1.2f;

	// Token: 0x04000634 RID: 1588
	protected float m_throwBone_AttackIntro_AnimationSpeed = 0.8f;

	// Token: 0x04000635 RID: 1589
	protected float m_throwBone_AttackIntro_Delay;

	// Token: 0x04000636 RID: 1590
	protected float m_throwBone_AttackHold_AnimationSpeed = 0.8f;

	// Token: 0x04000637 RID: 1591
	protected const float m_throwBone_AttackHold_Delay = 0.6f;

	// Token: 0x04000638 RID: 1592
	protected float m_throwBone_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000639 RID: 1593
	protected float m_throwBone_Exit_Delay = 0.15f;

	// Token: 0x0400063A RID: 1594
	protected int m_BoneNear_Angle = 83;

	// Token: 0x0400063B RID: 1595
	protected int m_FarBoneAngle = 70;

	// Token: 0x0400063C RID: 1596
	protected float m_throwBone_Exit_ForceIdle = 0.15f;

	// Token: 0x0400063D RID: 1597
	protected float m_throwBone_AttackCD;
}
