using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001F3 RID: 499
public class SpearKnight_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000DAC RID: 3500 RVA: 0x00007C7A File Offset: 0x00005E7A
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"SpearKnightBoltMinibossProjectile",
			"SpearKnightCurseProjectile",
			"SpearKnightCurseProjectile",
			"SpearKnightDaggerBoltRedProjectile",
			"SpearKnightDaggerBoltRedExpertProjectile",
			"SpearKnightDaggerBoltRedMinibossProjectile"
		};
	}

	// Token: 0x1700065F RID: 1631
	// (get) Token: 0x06000DAD RID: 3501 RVA: 0x00003F6C File Offset: 0x0000216C
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.35f, 0.75f);
		}
	}

	// Token: 0x17000660 RID: 1632
	// (get) Token: 0x06000DAE RID: 3502 RVA: 0x0000745A File Offset: 0x0000565A
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.5f, 0.85f);
		}
	}

	// Token: 0x17000661 RID: 1633
	// (get) Token: 0x06000DAF RID: 3503 RVA: 0x0000746B File Offset: 0x0000566B
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.5f, 0.5f);
		}
	}

	// Token: 0x17000662 RID: 1634
	// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_raiseKnockbackDefenseWhileAttacking
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000663 RID: 1635
	// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x00003E42 File Offset: 0x00002042
	protected virtual int m_knockbackDefenseBoostOverride
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x17000664 RID: 1636
	// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x0000530E File Offset: 0x0000350E
	protected virtual float Dash_AttackSpeed
	{
		get
		{
			return 28f;
		}
	}

	// Token: 0x17000665 RID: 1637
	// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float Dash_AttackDuration
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000666 RID: 1638
	// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_dashAttack_DashOffLedges
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000667 RID: 1639
	// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x00007CB8 File Offset: 0x00005EB8
	protected virtual float DashUppercut_AttackSpeed
	{
		get
		{
			return 19f;
		}
	}

	// Token: 0x17000668 RID: 1640
	// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x0000452F File Offset: 0x0000272F
	protected virtual float DashUppercut_AttackDuration
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000669 RID: 1641
	// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float DashUppercut_JumpSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700066A RID: 1642
	// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x000068DA File Offset: 0x00004ADA
	protected virtual float Uppercut_JumpPower
	{
		get
		{
			return 27f;
		}
	}

	// Token: 0x1700066B RID: 1643
	// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x0000530E File Offset: 0x0000350E
	protected virtual float m_thrust_AttackSpeed
	{
		get
		{
			return 28f;
		}
	}

	// Token: 0x1700066C RID: 1644
	// (get) Token: 0x06000DBA RID: 3514 RVA: 0x0000747C File Offset: 0x0000567C
	protected virtual float m_thrust_AttackDuration
	{
		get
		{
			return 0.325f;
		}
	}

	// Token: 0x1700066D RID: 1645
	// (get) Token: 0x06000DBB RID: 3515 RVA: 0x00004536 File Offset: 0x00002736
	protected virtual float m_thrust_AttackAmount
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700066E RID: 1646
	// (get) Token: 0x06000DBC RID: 3516 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float m_thrust_AttackLoopDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700066F RID: 1647
	// (get) Token: 0x06000DBD RID: 3517 RVA: 0x00004FE5 File Offset: 0x000031E5
	protected virtual float m_throw_Exit_ForceIdle
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x17000670 RID: 1648
	// (get) Token: 0x06000DBE RID: 3518 RVA: 0x00007483 File Offset: 0x00005683
	protected virtual float m_throw_AttackCD
	{
		get
		{
			return 3.5f;
		}
	}

	// Token: 0x17000671 RID: 1649
	// (get) Token: 0x06000DBF RID: 3519 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_throw_Attack_TargetPlayer
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000672 RID: 1650
	// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_throw_Attack_ProjectileAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000673 RID: 1651
	// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x0000457A File Offset: 0x0000277A
	protected virtual float m_throw_Attack_ProjectileDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x06000DC2 RID: 3522 RVA: 0x00007CBF File Offset: 0x00005EBF
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
		float dashSpeed = this.Dash_AttackSpeed;
		if (!base.EnemyController.IsTargetToMyRight)
		{
			dashSpeed = -dashSpeed;
		}
		base.EnemyController.LockFlip = true;
		if (this.m_dashAttack_DashOffLedges)
		{
			base.EnemyController.FallLedge = true;
		}
		yield return this.Default_TellIntroAndLoop("Dash_Tell_Intro", this.m_dash_TellIntro_AnimationSpeed, "Dash_Tell_Hold", this.m_dash_TellHold_AnimationSpeed, this.m_dash_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Dash_Attack_Intro", this.m_dash_AttackIntro_AnimationSpeed, this.m_dash_AttackIntro_Delay, true);
		yield return this.Default_Animation("Dash_Attack_Hold", this.m_dash_AttackHold_AnimationSpeed, this.m_dash_AttackHold_Delay, false);
		if (this.m_raiseKnockbackDefenseWhileAttacking && base.EnemyController.BaseKnockbackDefense < (float)this.m_knockbackDefenseBoostOverride)
		{
			base.EnemyController.BaseKnockbackDefense = (float)this.m_knockbackDefenseBoostOverride;
		}
		base.SetVelocityX(dashSpeed, false);
		base.EnemyController.GroundHorizontalVelocity = dashSpeed;
		base.EnemyController.DisableFriction = true;
		if (this.Dash_AttackDuration > 0f)
		{
			yield return base.Wait(this.Dash_AttackDuration, false);
		}
		yield return base.WaitUntilIsGrounded();
		base.SetVelocityX(0f, false);
		base.EnemyController.GroundHorizontalVelocity = 0f;
		base.EnemyController.DisableFriction = false;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("Dash_Exit", this.m_dash_Exit_AnimationSpeed, this.m_dash_Exit_Delay, true);
		base.EnemyController.FallLedge = false;
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_dash_Exit_ForceIdle, this.m_dash_AttackCD);
		yield break;
	}

	// Token: 0x06000DC3 RID: 3523 RVA: 0x00007CCE File Offset: 0x00005ECE
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator DashAndUppercutAttack()
	{
		this.StopAndFaceTarget();
		float dashSpeed = this.DashUppercut_AttackSpeed;
		float jumpSpeed = this.DashUppercut_JumpSpeed;
		if (!base.EnemyController.IsTargetToMyRight)
		{
			dashSpeed = -dashSpeed;
			jumpSpeed = -jumpSpeed;
		}
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("Combo_Tell_Intro", this.m_dashUppercut_TellIntro_AnimationSpeed, "Combo_Tell_Hold", this.m_dashUppercut_TellHold_AnimationSpeed, this.m_dashUppercut_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Combo_Dash_Attack_Intro", this.m_dashUppercut_DashOnly_AttackIntro_AnimationSpeed, this.m_dashUppercut_DashOnly_AttackIntro_Delay, true);
		yield return this.Default_Animation("Combo_Dash_Attack_Hold", this.m_dashUppercut_DashOnly_AttackHold_AnimationSpeed, this.m_dashUppercut_DashOnly_AttackHold_Delay, false);
		base.SetVelocityX(dashSpeed, false);
		base.EnemyController.GroundHorizontalVelocity = dashSpeed;
		if (this.DashUppercut_AttackDuration > 0f)
		{
			yield return base.Wait(this.DashUppercut_AttackDuration, false);
		}
		yield return this.Default_Animation("Combo_Uppercut_Tell", this.m_dashUppercut_UppercutOnly_AttackIntro_AnimationSpeed, this.m_dashUppercut_UppercutOnly_AttackIntro_Delay, true);
		yield return this.Default_Animation("Combo_Uppercut_Hold", this.m_dashUppercut_UppercutOnly_AttackHold_AnimationSpeed, this.m_dashUppercut_UppercutOnly_AttackHold_Delay, false);
		base.SetVelocity(jumpSpeed, this.Uppercut_JumpPower, false);
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			yield return base.Wait(0.45f, false);
			this.FireProjectile("SpearKnightBoltMinibossProjectile", 0, true, 55f, 1f, true, true, true);
			this.FireProjectile("SpearKnightBoltMinibossProjectile", 0, true, 125f, 1f, true, true, true);
			this.FireProjectile("SpearKnightBoltMinibossProjectile", 0, true, 75f, 1f, true, true, true);
			this.FireProjectile("SpearKnightBoltMinibossProjectile", 0, true, 105f, 1f, true, true, true);
			this.FireProjectile("SpearKnightBoltMinibossProjectile", 0, true, 95f, 1f, true, true, true);
			this.FireProjectile("SpearKnightBoltMinibossProjectile", 0, true, 85f, 1f, true, true, true);
		}
		yield return base.WaitUntilIsGrounded();
		yield return this.Default_Animation("Combo_Exit", this.m_dashUppercut_Exit_AnimationSpeed, this.m_dashUppercut_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Attack_Cooldown(this.m_dashUppercut_Exit_ForceIdle, this.m_dashUppercut_AttackCD);
		yield break;
	}

	// Token: 0x06000DC4 RID: 3524 RVA: 0x00007CDD File Offset: 0x00005EDD
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ThrustAttack()
	{
		int i = 0;
		while ((float)i < this.m_thrust_AttackAmount)
		{
			float thrustSpeed = this.m_thrust_AttackSpeed;
			if (i == 0)
			{
				yield return this.Default_TellIntroAndLoop("Thrust_Tell_Intro", this.m_thrust_TellIntro_AnimationSpeed, "Thrust_Tell_Hold", this.m_thrust_TellHold_AnimationSpeed, this.m_thrust_TellIntroAndHold_Delay);
			}
			else
			{
				yield return this.Default_TellIntroAndLoop("Thrust_Tell_Intro", this.m_thrust_TellIntro_AnimationSpeed, "Thrust_Tell_Hold", this.m_thrust_TellHold_AnimationSpeed, this.m_thrust_TellIntroAndHoldRepeat_Delay);
			}
			this.StopAndFaceTarget();
			if (!base.EnemyController.IsTargetToMyRight)
			{
				thrustSpeed = -thrustSpeed;
			}
			base.EnemyController.LockFlip = true;
			yield return this.Default_Animation("Thrust_Attack_Intro", this.m_thrust_AttackIntro_AnimationSpeed, this.m_thrust_AttackIntro_Delay, true);
			yield return this.Default_Animation("Thrust_Attack_Hold", this.m_thrust_AttackHold_AnimationSpeed, this.m_thrust_AttackHold_Delay, false);
			if (this.m_raiseKnockbackDefenseWhileAttacking && base.EnemyController.BaseKnockbackDefense < (float)this.m_knockbackDefenseBoostOverride)
			{
				base.EnemyController.BaseKnockbackDefense = (float)this.m_knockbackDefenseBoostOverride;
			}
			base.SetVelocityX(thrustSpeed, false);
			base.EnemyController.GroundHorizontalVelocity = thrustSpeed;
			base.EnemyController.DisableFriction = true;
			if (this.m_thrust_AttackDuration > 0f)
			{
				yield return base.Wait(this.m_thrust_AttackDuration, false);
			}
			base.SetVelocityX(0f, false);
			base.EnemyController.GroundHorizontalVelocity = 0f;
			base.EnemyController.DisableFriction = false;
			if (this.m_thrust_AttackLoopDelay > 0f)
			{
				yield return base.Wait(this.m_thrust_AttackLoopDelay, false);
			}
			yield return this.Default_Animation("Thrust_Exit", this.m_thrust_Exit_AnimationSpeed, this.m_thrust_Exit_Delay, true);
			base.EnemyController.LockFlip = false;
			int num = i;
			i = num + 1;
		}
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_thrust_Exit_ForceIdle, this.m_thrust_AttackCD);
		yield break;
	}

	// Token: 0x06000DC5 RID: 3525 RVA: 0x00007CEC File Offset: 0x00005EEC
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ThrowAttack()
	{
		this.StopAndFaceTarget();
		if (!this.m_throw_Attack_TargetPlayer)
		{
			base.EnemyController.LockFlip = true;
		}
		yield return this.Default_TellIntroAndLoop("SpearKnight_Throw_Tell_Intro", this.m_throw_TellIntro_AnimationSpeed, "SpearKnight_Throw_Tell_Hold", this.m_throw_TellHold_AnimationSpeed, this.m_throw_TellIntroAndHold_Delay);
		int num2;
		for (int i = 0; i < this.m_throw_Attack_ProjectileAmount; i = num2 + 1)
		{
			yield return this.Default_Animation("SpearKnight_Throw_Attack_Intro", this.m_throw_AttackIntro_AnimationSpeed, this.m_throw_AttackIntro_Delay, true);
			yield return this.Default_Animation("SpearKnight_Throw_Attack_Hold", this.m_throw_AttackHold_AnimationSpeed, 0f, false);
			string projectileName = "SpearKnightDaggerBoltRedProjectile";
			if (base.EnemyController.EnemyRank == EnemyRank.Expert)
			{
				projectileName = "SpearKnightDaggerBoltRedExpertProjectile";
			}
			else if (base.EnemyController.EnemyRank == EnemyRank.Miniboss)
			{
				projectileName = "SpearKnightDaggerBoltRedMinibossProjectile";
			}
			if (this.m_throw_Attack_TargetPlayer)
			{
				float num = Mathf.Atan2(base.EnemyController.TargetController.Midpoint.y - base.EnemyController.Midpoint.y, base.EnemyController.TargetController.Midpoint.x - base.EnemyController.Midpoint.x);
				num = CDGHelper.WrapAngleDegrees(CDGHelper.ToDegrees(num), false);
				this.FireProjectile(projectileName, 1, false, num, 1f, true, true, true);
			}
			else
			{
				this.FireProjectile(projectileName, 1, true, 0f, 1f, true, true, true);
			}
			if (this.m_throw_Attack_ProjectileAmount > 1 && this.m_throw_Attack_ProjectileDelay > 0f)
			{
				yield return base.Wait(this.m_throw_Attack_ProjectileDelay, false);
			}
			num2 = i;
		}
		yield return base.Wait(this.m_throw_AttackHold_Delay, false);
		yield return this.Default_Animation("SpearKnight_Throw_Exit", this.m_throw_Exit_AnimationSpeed, this.m_throw_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_throw_Exit_ForceIdle, this.m_throw_AttackCD);
		yield break;
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x00007CFB File Offset: 0x00005EFB
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public virtual IEnumerator UpperCutBullets()
	{
		yield break;
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x00007D03 File Offset: 0x00005F03
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator HeadBobbleAttack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("SpearKnight_HeadShake_Tell_Intro", this.m_headBobble_TellIntro_AnimSpeed, "SpearKnight_HeadShake_Tell_Hold", this.m_headBobble_TellHold_AnimSpeed, this.m_headBobble_TellIntroAndHold_Delay);
		yield return this.Default_Animation("SpearKnight_HeadShake_Attack_Intro", this.m_headBobble_AttackIntro_AnimSpeed, this.m_headBobble_AttackIntro_Delay, false);
		yield return this.ChangeAnimationState("SpearKnight_HeadShake_Attack_Hold");
		float projFireInterval = this.m_headBobble_AttackHold_Delay / (float)this.m_numHeadBobbleProjectiles;
		int num;
		for (int i = 0; i < this.m_numHeadBobbleProjectiles; i = num + 1)
		{
			this.FireProjectile("SpearKnightCurseProjectile", 2, false, 90f, 1f, true, true, true);
			yield return base.Wait(projFireInterval, false);
			num = i;
		}
		yield return base.Wait(this.m_headBobble_AttackHold_Exit_Delay, false);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("SpearKnight_HeadShake_Exit", this.m_headBobble_Exit_AnimSpeed, this.m_headBobble_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_headBobble_Exit_IdleDuration, this.m_headBobble_AttackCD);
		yield break;
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x00007D12 File Offset: 0x00005F12
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.EnemyController.FallLedge = false;
		base.EnemyController.DisableFriction = false;
		base.EnemyController.GroundHorizontalVelocity = 0f;
		base.EnemyController.LockFlip = false;
	}

	// Token: 0x04000FDD RID: 4061
	private const string MINIBOSS_BOLT_PROJECTILE = "SpearKnightBoltMinibossProjectile";

	// Token: 0x04000FDE RID: 4062
	private const string HEAD_BOBBLE_PROJECTILE = "SpearKnightCurseProjectile";

	// Token: 0x04000FDF RID: 4063
	private const string CURSE_PROJECTILE = "SpearKnightCurseProjectile";

	// Token: 0x04000FE0 RID: 4064
	private const string DAGGER_BOLT_PROJECTILE = "SpearKnightDaggerBoltRedProjectile";

	// Token: 0x04000FE1 RID: 4065
	private const string EXPERT_DAGGER_BOLT_PROJECTILE = "SpearKnightDaggerBoltRedExpertProjectile";

	// Token: 0x04000FE2 RID: 4066
	private const string MINIBOSS_DAGGER_BOLT_PROJECTILE = "SpearKnightDaggerBoltRedMinibossProjectile";

	// Token: 0x04000FE3 RID: 4067
	protected float m_dash_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000FE4 RID: 4068
	protected float m_dash_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000FE5 RID: 4069
	protected float m_dash_TellIntroAndHold_Delay = 0.75f;

	// Token: 0x04000FE6 RID: 4070
	protected float m_dash_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000FE7 RID: 4071
	protected float m_dash_AttackIntro_Delay;

	// Token: 0x04000FE8 RID: 4072
	protected float m_dash_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000FE9 RID: 4073
	protected float m_dash_AttackHold_Delay;

	// Token: 0x04000FEA RID: 4074
	protected float m_dash_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000FEB RID: 4075
	protected float m_dash_Exit_Delay;

	// Token: 0x04000FEC RID: 4076
	protected float m_dash_Exit_ForceIdle = 0.15f;

	// Token: 0x04000FED RID: 4077
	protected float m_dash_AttackCD = 2.5f;

	// Token: 0x04000FEE RID: 4078
	protected float m_dashUppercut_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000FEF RID: 4079
	protected float m_dashUppercut_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000FF0 RID: 4080
	protected float m_dashUppercut_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x04000FF1 RID: 4081
	protected float m_dashUppercut_DashOnly_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000FF2 RID: 4082
	protected float m_dashUppercut_DashOnly_AttackIntro_Delay;

	// Token: 0x04000FF3 RID: 4083
	protected float m_dashUppercut_DashOnly_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000FF4 RID: 4084
	protected float m_dashUppercut_DashOnly_AttackHold_Delay;

	// Token: 0x04000FF5 RID: 4085
	protected float m_dashUppercut_UppercutOnly_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000FF6 RID: 4086
	protected float m_dashUppercut_UppercutOnly_AttackIntro_Delay;

	// Token: 0x04000FF7 RID: 4087
	protected float m_dashUppercut_UppercutOnly_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x04000FF8 RID: 4088
	protected float m_dashUppercut_UppercutOnly_AttackHold_Delay;

	// Token: 0x04000FF9 RID: 4089
	protected float m_dashUppercut_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000FFA RID: 4090
	protected float m_dashUppercut_Exit_Delay;

	// Token: 0x04000FFB RID: 4091
	protected float m_dashUppercut_Exit_ForceIdle = 0.15f;

	// Token: 0x04000FFC RID: 4092
	protected float m_dashUppercut_AttackCD;

	// Token: 0x04000FFD RID: 4093
	protected float m_thrust_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000FFE RID: 4094
	protected float m_thrust_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000FFF RID: 4095
	protected float m_thrust_TellIntroAndHold_Delay = 0.75f;

	// Token: 0x04001000 RID: 4096
	protected float m_thrust_TellIntroAndHoldRepeat_Delay = 0.15f;

	// Token: 0x04001001 RID: 4097
	protected float m_thrust_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04001002 RID: 4098
	protected float m_thrust_AttackIntro_Delay;

	// Token: 0x04001003 RID: 4099
	protected float m_thrust_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04001004 RID: 4100
	protected float m_thrust_AttackHold_Delay;

	// Token: 0x04001005 RID: 4101
	protected float m_thrust_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04001006 RID: 4102
	protected float m_thrust_Exit_Delay;

	// Token: 0x04001007 RID: 4103
	protected float m_thrust_Exit_ForceIdle = 0.15f;

	// Token: 0x04001008 RID: 4104
	protected float m_thrust_AttackCD = 2.5f;

	// Token: 0x04001009 RID: 4105
	protected float m_throw_TellIntro_AnimationSpeed = 0.75f;

	// Token: 0x0400100A RID: 4106
	protected float m_throw_TellHold_AnimationSpeed = 0.65f;

	// Token: 0x0400100B RID: 4107
	protected float m_throw_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x0400100C RID: 4108
	protected float m_throw_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x0400100D RID: 4109
	protected float m_throw_AttackIntro_Delay;

	// Token: 0x0400100E RID: 4110
	protected float m_throw_AttackHold_AnimationSpeed = 1f;

	// Token: 0x0400100F RID: 4111
	protected float m_throw_AttackHold_Delay = 0.25f;

	// Token: 0x04001010 RID: 4112
	protected float m_throw_Exit_AnimationSpeed = 0.45f;

	// Token: 0x04001011 RID: 4113
	protected float m_throw_Exit_Delay = 0.15f;

	// Token: 0x04001012 RID: 4114
	protected const string HEAD_BOBBLE_TELL_INTRO = "SpearKnight_HeadShake_Tell_Intro";

	// Token: 0x04001013 RID: 4115
	protected const string HEAD_BOBBLE_TELL_HOLD = "SpearKnight_HeadShake_Tell_Hold";

	// Token: 0x04001014 RID: 4116
	protected const string HEAD_BOBBLE_ATTACK_INTRO = "SpearKnight_HeadShake_Attack_Intro";

	// Token: 0x04001015 RID: 4117
	protected const string HEAD_BOBBLE_ATTACK_HOLD = "SpearKnight_HeadShake_Attack_Hold";

	// Token: 0x04001016 RID: 4118
	protected const string HEAD_BOBBLE_EXIT = "SpearKnight_HeadShake_Exit";

	// Token: 0x04001017 RID: 4119
	protected float m_headBobble_TellIntro_AnimSpeed = 1f;

	// Token: 0x04001018 RID: 4120
	protected float m_headBobble_TellHold_AnimSpeed = 1f;

	// Token: 0x04001019 RID: 4121
	protected float m_headBobble_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x0400101A RID: 4122
	protected float m_headBobble_AttackIntro_AnimSpeed = 1f;

	// Token: 0x0400101B RID: 4123
	protected float m_headBobble_AttackIntro_Delay;

	// Token: 0x0400101C RID: 4124
	protected float m_headBobble_AttackHold_AnimSpeed = 1f;

	// Token: 0x0400101D RID: 4125
	protected float m_headBobble_AttackHold_Delay = 1f;

	// Token: 0x0400101E RID: 4126
	protected float m_headBobble_AttackHold_Exit_Delay = 0.5f;

	// Token: 0x0400101F RID: 4127
	protected float m_headBobble_Exit_AnimSpeed = 1f;

	// Token: 0x04001020 RID: 4128
	protected float m_headBobble_Exit_Delay = 0.45f;

	// Token: 0x04001021 RID: 4129
	protected float m_headBobble_Exit_IdleDuration = 0.15f;

	// Token: 0x04001022 RID: 4130
	protected float m_headBobble_AttackCD = 15f;

	// Token: 0x04001023 RID: 4131
	protected int m_numHeadBobbleProjectiles = 3;
}
