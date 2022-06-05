using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class Flamer_Miniboss_AIScript : Flamer_Basic_AIScript
{
	// Token: 0x060005EC RID: 1516 RVA: 0x00018AFD File Offset: 0x00016CFD
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlamerLineBoltProjectile",
			"FlamerLineBoltMinibossProjectile",
			"FlamerLineBoltMinibossProjectile",
			"FlamerWarningProjectile"
		};
	}

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x060005ED RID: 1517 RVA: 0x00018B2B File Offset: 0x00016D2B
	protected override float m_flameWalk_Exit_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x060005EE RID: 1518 RVA: 0x00018B32 File Offset: 0x00016D32
	protected override float m_flameWalk_AttackCD
	{
		get
		{
			return 11f;
		}
	}

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x060005EF RID: 1519 RVA: 0x00018B39 File Offset: 0x00016D39
	protected override float m_flameWalk_AttackMoveSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x060005F0 RID: 1520 RVA: 0x00018B40 File Offset: 0x00016D40
	protected virtual float m_jumpAttack_Exit_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x060005F1 RID: 1521 RVA: 0x00018B47 File Offset: 0x00016D47
	protected virtual float m_jumpAttack_AttackCD
	{
		get
		{
			return 11f;
		}
	}

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x060005F2 RID: 1522 RVA: 0x00018B4E File Offset: 0x00016D4E
	protected virtual Vector2 m_jumpAttack_Power
	{
		get
		{
			return new Vector2(-14f, 34f);
		}
	}

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x060005F3 RID: 1523 RVA: 0x00018B5F File Offset: 0x00016D5F
	protected virtual bool m_jumpAttack_hasMultipleFlames
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x060005F4 RID: 1524 RVA: 0x00018B62 File Offset: 0x00016D62
	protected virtual float m_jumpAttack_AttackDuration
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x060005F5 RID: 1525 RVA: 0x00018B69 File Offset: 0x00016D69
	protected virtual float m_jumpAttack_AttackMoveSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x00018B70 File Offset: 0x00016D70
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator JumpAttack()
	{
		this.StopAndFaceTarget();
		yield return base.Wait(this.m_jumpAttack_Tell_Delay, false);
		if (!base.EnemyController.IsFacingRight)
		{
			base.SetVelocityX(this.m_jumpAttack_Power.x, true);
		}
		else
		{
			base.SetVelocityX(-this.m_jumpAttack_Power.x, true);
		}
		base.SetVelocityY(this.m_jumpAttack_Power.y, false);
		yield return base.Wait(0.05f, false);
		yield return this.m_waitUntilFallingYield;
		base.SetVelocity(0f, 0f, false);
		float storedKnockbackDefense = base.EnemyController.BaseKnockbackDefense;
		base.EnemyController.BaseKnockbackDefense = 99f;
		base.EnemyController.ControllerCorgi.GravityActive(false);
		this.StopAndFaceTarget();
		bool isFacingRight = base.EnemyController.IsFacingRight;
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("BigFlame_Tell_Intro", this.m_jumpAttack_TellIntro_AnimationSpeed, "BigFlame_Tell_Intro", this.m_jumpAttack_TellHold_AnimationSpeed, this.m_jumpAttack_Tell_Delay);
		yield return this.Default_Animation("BigFlame_Attack_Intro", this.m_jumpAttack_AttackIntro_AnimationSpeed, this.m_jumpAttack_AttackIntro_Delay, true);
		yield return this.Default_Animation("BigFlame_Attack_Hold", this.m_jumpAttack_AttackHold_AnimationSpeed, this.m_jumpAttack_AttackHold_Delay, false);
		this.m_flameWalkProjectile = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, true, -90f, 1f, true, true, true);
		if (this.m_jumpAttack_hasMultipleFlames)
		{
			this.m_flameWalkProjectile2 = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, true, -50f, 1f, true, true, true);
			this.m_flameWalkProjectile3 = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, true, -130f, 1f, true, true, true);
		}
		if (isFacingRight)
		{
			base.SetVelocityX(this.m_jumpAttack_AttackMoveSpeed, false);
		}
		else
		{
			base.SetVelocityX(-this.m_jumpAttack_AttackMoveSpeed, false);
		}
		if (this.m_jumpAttack_AttackDuration > 0f)
		{
			yield return base.Wait(this.m_jumpAttack_AttackDuration, false);
		}
		base.SetVelocityX(0f, false);
		if (this.m_flameWalkProjectile && !this.m_flameWalkProjectile.IsFreePoolObj && this.m_flameWalkProjectile.Owner == base.EnemyController.gameObject)
		{
			this.m_flameWalkProjectile.FlagForDestruction(null);
		}
		if (this.m_flameWalkProjectile2 && !this.m_flameWalkProjectile2.IsFreePoolObj && this.m_flameWalkProjectile2.Owner == base.EnemyController.gameObject)
		{
			this.m_flameWalkProjectile2.FlagForDestruction(null);
		}
		if (this.m_flameWalkProjectile3 && !this.m_flameWalkProjectile3.IsFreePoolObj && this.m_flameWalkProjectile3.Owner == base.EnemyController.gameObject)
		{
			this.m_flameWalkProjectile3.FlagForDestruction(null);
		}
		yield return this.m_loseGravityDurationYield;
		base.EnemyController.BaseKnockbackDefense = storedKnockbackDefense;
		base.EnemyController.ControllerCorgi.GravityActive(true);
		yield return base.WaitUntilIsGrounded();
		yield return base.Wait(this.m_jumpAttack_Exit_Delay, false);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_jumpAttack_Exit_ForceIdle, this.m_jumpAttack_AttackCD);
		yield break;
	}

	// Token: 0x04000A37 RID: 2615
	protected const string FLAMEWALK_LINEBOLT_MINIBOSS_PROJECTILE = "FlamerLineBoltMinibossProjectile";

	// Token: 0x04000A38 RID: 2616
	protected float m_jump_Tell_Delay = 0.75f;

	// Token: 0x04000A39 RID: 2617
	protected float m_jump_Tell_AnimationSpeed = 0.75f;

	// Token: 0x04000A3A RID: 2618
	protected float m_jumpAttack_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000A3B RID: 2619
	protected float m_jumpAttack_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000A3C RID: 2620
	protected float m_jumpAttack_Tell_Delay = 0.5f;

	// Token: 0x04000A3D RID: 2621
	protected float m_jumpAttack_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000A3E RID: 2622
	protected float m_jumpAttack_AttackIntro_Delay;

	// Token: 0x04000A3F RID: 2623
	protected float m_jumpAttack_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000A40 RID: 2624
	protected float m_jumpAttack_AttackHold_Delay;

	// Token: 0x04000A41 RID: 2625
	protected float m_jumpAttack_Exit_AnimationSpeed = 1f;

	// Token: 0x04000A42 RID: 2626
	protected float m_jumpAttack_Exit_Delay;
}
