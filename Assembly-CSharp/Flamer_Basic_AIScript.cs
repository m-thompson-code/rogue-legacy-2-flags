using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000CD RID: 205
public class Flamer_Basic_AIScript : BaseAIScript
{
	// Token: 0x060005CF RID: 1487 RVA: 0x0001865F File Offset: 0x0001685F
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlamerLineBoltProjectile",
			"FlamerLineBoltMinibossProjectile",
			"FlamerWarningProjectile",
			"FlamerBombProjectile",
			"FlamerBombExplosionProjectile"
		};
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x060005D0 RID: 1488 RVA: 0x00018695 File Offset: 0x00016895
	protected virtual float m_jumpAttack_loseGravityDuration
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x0001869C File Offset: 0x0001689C
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.m_waitUntilFallingYield = new WaitUntil(() => base.EnemyController.Velocity.y < 0f);
		this.m_loseGravityDurationYield = new WaitRL_Yield(this.m_jumpAttack_loseGravityDuration, false);
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x060005D2 RID: 1490 RVA: 0x000186CE File Offset: 0x000168CE
	protected virtual float m_flameWalk_Exit_ForceIdle
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x060005D3 RID: 1491 RVA: 0x000186D5 File Offset: 0x000168D5
	protected virtual float m_flameWalk_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x060005D4 RID: 1492 RVA: 0x000186DC File Offset: 0x000168DC
	protected virtual float m_flameWalk_AttackDuration
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x060005D5 RID: 1493 RVA: 0x000186E3 File Offset: 0x000168E3
	protected virtual float m_flameWalk_AttackMoveSpeed
	{
		get
		{
			return 6.5f;
		}
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x000186EA File Offset: 0x000168EA
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator CastFlameWalk()
	{
		this.StopAndFaceTarget();
		bool isFacingRight = base.EnemyController.IsFacingRight;
		base.EnemyController.LockFlip = true;
		this.m_flameTellProjectile = this.FireProjectile("FlamerWarningProjectile", 2, true, 0f, 1f, true, true, true);
		yield return this.Default_TellIntroAndLoop("Flame_Tell_Intro", this.m_flameWalk_TellIntro_AnimationSpeed, "Flame_Tell_Hold", this.m_flameWalk_TellHold_AnimationSpeed, this.m_flameWalk_Tell_Delay);
		base.StopProjectile(ref this.m_flameTellProjectile);
		yield return this.Default_Animation("Flame_Attack_Intro", this.m_flameWalk_AttackIntro_AnimationSpeed, this.m_flameWalk_AttackIntro_Delay, true);
		yield return this.Default_Animation("Flame_Attack_Hold", this.m_flameWalk_AttackHold_AnimationSpeed, this.m_flameWalk_AttackHold_Delay, false);
		this.m_firingState = Flamer_Basic_AIScript.FlameFiringState.Regular;
		this.m_flameWalkProjectile = this.FireProjectile("FlamerLineBoltProjectile", 2, true, 0f, 1f, true, true, true);
		if (isFacingRight)
		{
			base.SetVelocityX(this.m_flameWalk_AttackMoveSpeed, false);
		}
		else
		{
			base.SetVelocityX(-this.m_flameWalk_AttackMoveSpeed, false);
		}
		base.EnemyController.GroundHorizontalVelocity = base.EnemyController.Velocity.x;
		if (this.m_flameWalk_AttackDuration > 0f)
		{
			yield return base.Wait(this.m_flameWalk_AttackDuration, false);
		}
		base.SetVelocityX(0f, false);
		base.EnemyController.GroundHorizontalVelocity = 0f;
		base.StopProjectile(ref this.m_flameWalkProjectile);
		yield return this.Default_Animation("Flame_Exit", this.m_flameWalk_Exit_AnimationSpeed, this.m_flameWalk_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_flameWalk_Exit_ForceIdle, this.m_flameWalk_AttackCD);
		yield break;
	}

	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x060005D7 RID: 1495 RVA: 0x000186F9 File Offset: 0x000168F9
	protected virtual float m_flameBolt_Exit_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x060005D8 RID: 1496 RVA: 0x00018700 File Offset: 0x00016900
	protected virtual float m_flameBolt_AttackCD
	{
		get
		{
			return 11f;
		}
	}

	// Token: 0x170002DA RID: 730
	// (get) Token: 0x060005D9 RID: 1497 RVA: 0x00018707 File Offset: 0x00016907
	protected virtual Vector2 m_flameBolt_Jump
	{
		get
		{
			return new Vector2(-12f, 13f);
		}
	}

	// Token: 0x170002DB RID: 731
	// (get) Token: 0x060005DA RID: 1498 RVA: 0x00018718 File Offset: 0x00016918
	protected virtual float m_flameBolt_SecondShotDelay
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0001871F File Offset: 0x0001691F
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator FlameBolt()
	{
		this.StopAndFaceTarget();
		bool isFacingRight = base.EnemyController.IsFacingRight;
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("BigFlame_Tell_Intro", this.m_flameBolt_TellIntro_AnimationSpeed, "BigFlame_Tell_Hold", this.m_flameBolt_TellHold_AnimationSpeed, this.m_flameBolt_Tell_Delay);
		yield return this.Default_Animation("BigFlame_Attack_Intro", this.m_flameBolt_AttackIntro_AnimationSpeed, this.m_flameBolt_AttackIntro_Delay, true);
		yield return this.Default_Animation("BigFlame_Attack_Hold", this.m_flameBolt_AttackHold_AnimationSpeed, this.m_flameBolt_AttackHold_Delay, false);
		if (base.EnemyController.IsFacingRight)
		{
			this.FireProjectile("FlamerBombProjectile", 3, false, 0f, 1f, true, true, true);
			base.SetVelocityX(this.m_flameBolt_Jump.x, false);
		}
		else
		{
			this.FireProjectile("FlamerBombProjectile", 3, false, 180f, 1f, true, true, true);
			base.SetVelocityX(-this.m_flameBolt_Jump.x, false);
		}
		base.SetVelocityY(this.m_flameBolt_Jump.y, false);
		yield return base.Wait(0.05f, false);
		if (base.EnemyController.IsFacingRight)
		{
			base.EnemyController.JumpHorizontalVelocity = this.m_flameBolt_Jump.x;
		}
		else
		{
			base.EnemyController.JumpHorizontalVelocity = -this.m_flameBolt_Jump.x;
		}
		if (base.EnemyController.EnemyRank == EnemyRank.Expert)
		{
			yield return base.Wait(this.m_flameBolt_SecondShotDelay, false);
			if (base.EnemyController.IsFacingRight)
			{
				this.FireProjectile("FlamerBombProjectile", 3, false, 0f, 1f, true, true, true);
				base.SetVelocityX(this.m_flameBolt_Jump.x, false);
			}
			else
			{
				this.FireProjectile("FlamerBombProjectile", 3, false, 180f, 1f, true, true, true);
				base.SetVelocityX(-this.m_flameBolt_Jump.x, false);
			}
			base.SetVelocityY(this.m_flameBolt_Jump.y, false);
			yield return base.Wait(0.05f, false);
			if (base.EnemyController.IsFacingRight)
			{
				base.EnemyController.JumpHorizontalVelocity = this.m_flameBolt_Jump.x;
			}
			else
			{
				base.EnemyController.JumpHorizontalVelocity = -this.m_flameBolt_Jump.x;
			}
		}
		yield return base.WaitUntilIsGrounded();
		base.SetVelocityX(0f, false);
		yield return this.Default_Animation("BigFlame_Exit", this.m_flameBolt_Exit_AnimationSpeed, this.m_flameBolt_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_flameBolt_Exit_ForceIdle, this.m_flameBolt_AttackCD);
		yield break;
	}

	// Token: 0x170002DC RID: 732
	// (get) Token: 0x060005DC RID: 1500 RVA: 0x0001872E File Offset: 0x0001692E
	protected virtual float m_megaFlame_Exit_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170002DD RID: 733
	// (get) Token: 0x060005DD RID: 1501 RVA: 0x00018735 File Offset: 0x00016935
	protected virtual float m_megaFlame_AttackCD
	{
		get
		{
			return 11f;
		}
	}

	// Token: 0x170002DE RID: 734
	// (get) Token: 0x060005DE RID: 1502 RVA: 0x0001873C File Offset: 0x0001693C
	protected virtual bool m_megaFlame_hasVerticalDashFlame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x060005DF RID: 1503 RVA: 0x0001873F File Offset: 0x0001693F
	protected virtual float m_megaFlame_AttackDuration
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x060005E0 RID: 1504 RVA: 0x00018746 File Offset: 0x00016946
	protected virtual float m_megaFlame_AttackMidPauseDuration
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0001874D File Offset: 0x0001694D
	protected virtual float m_megaFlame_AttackVertDuration
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x060005E2 RID: 1506 RVA: 0x00018754 File Offset: 0x00016954
	protected virtual float m_megaFlame_AttackMoveSpeed
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0001875B File Offset: 0x0001695B
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public IEnumerator CastMegaFlame()
	{
		this.StopAndFaceTarget();
		bool isFacingRight = base.EnemyController.IsFacingRight;
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("BigFlame_Tell_Intro", this.m_megaFlame_TellIntro_AnimationSpeed, "BigFlame_Tell_Hold", this.m_megaFlame_TellHold_AnimationSpeed, this.m_megaFlame_Tell_Delay);
		yield return this.Default_Animation("BigFlame_Attack_Intro", this.m_megaFlame_AttackIntro_AnimationSpeed, this.m_megaFlame_AttackIntro_Delay, true);
		yield return this.Default_Animation("BigFlame_Attack_Hold", this.m_megaFlame_AttackHold_AnimationSpeed, this.m_megaFlame_AttackHold_Delay, false);
		this.m_firingState = Flamer_Basic_AIScript.FlameFiringState.Mega1;
		this.m_flameWalkProjectile = this.FireProjectile("FlamerLineBoltMinibossProjectile", 2, false, 0f, 1f, true, true, true);
		this.m_flameWalkProjectile2 = this.FireProjectile("FlamerLineBoltMinibossProjectile", 0, false, 180f, 1f, true, true, true);
		if (isFacingRight)
		{
			base.SetVelocityX(this.m_megaFlame_AttackMoveSpeed, false);
		}
		else
		{
			base.SetVelocityX(-this.m_megaFlame_AttackMoveSpeed, false);
		}
		if (this.m_megaFlame_AttackDuration > 0f)
		{
			yield return base.Wait(this.m_megaFlame_AttackDuration, false);
		}
		base.SetVelocityX(0f, false);
		base.StopProjectile(ref this.m_flameWalkProjectile);
		base.StopProjectile(ref this.m_flameWalkProjectile2);
		if (this.m_megaFlame_AttackMidPauseDuration > 0f)
		{
			yield return base.Wait(this.m_megaFlame_AttackMidPauseDuration, false);
		}
		this.m_firingState = Flamer_Basic_AIScript.FlameFiringState.Mega2;
		this.m_flameWalkProjectile = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, false, 70f, 1f, true, true, true);
		this.m_flameWalkProjectile2 = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, false, 110f, 1f, true, true, true);
		this.m_flameWalkProjectile3 = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, false, 90f, 1f, true, true, true);
		if (this.m_megaFlame_AttackVertDuration > 0f)
		{
			yield return base.Wait(this.m_megaFlame_AttackVertDuration, false);
		}
		yield return this.Default_Animation("BigFlame_Exit", this.m_megaFlame_Exit_AnimationSpeed, this.m_megaFlame_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.LockFlip = false;
		base.StopProjectile(ref this.m_flameWalkProjectile);
		base.StopProjectile(ref this.m_flameWalkProjectile2);
		base.StopProjectile(ref this.m_flameWalkProjectile3);
		yield return this.Default_Attack_Cooldown(this.m_megaFlame_Exit_ForceIdle, this.m_megaFlame_AttackCD);
		yield break;
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0001876C File Offset: 0x0001696C
	public override void Pause()
	{
		base.Pause();
		if (this.m_flameWalkProjectile && !this.m_flameWalkProjectile.IsFreePoolObj && this.m_flameWalkProjectile.Owner == base.EnemyController.gameObject)
		{
			this.m_wasFrozenWhileFiring = true;
			base.StopProjectile(ref this.m_flameWalkProjectile);
		}
		if (this.m_flameWalkProjectile2 && !this.m_flameWalkProjectile2.IsFreePoolObj && this.m_flameWalkProjectile2.Owner == base.EnemyController.gameObject)
		{
			this.m_wasFrozenWhileFiring = true;
			base.StopProjectile(ref this.m_flameWalkProjectile2);
		}
		if (this.m_flameWalkProjectile3 && !this.m_flameWalkProjectile3.IsFreePoolObj && this.m_flameWalkProjectile3.Owner == base.EnemyController.gameObject)
		{
			this.m_wasFrozenWhileFiring = true;
			base.StopProjectile(ref this.m_flameWalkProjectile3);
		}
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x00018860 File Offset: 0x00016A60
	public override void Unpause()
	{
		base.Unpause();
		if (this.m_wasFrozenWhileFiring)
		{
			switch (this.m_firingState)
			{
			case Flamer_Basic_AIScript.FlameFiringState.Regular:
				this.m_flameWalkProjectile = this.FireProjectile("FlamerLineBoltProjectile", 2, true, 0f, 1f, true, true, true);
				break;
			case Flamer_Basic_AIScript.FlameFiringState.Mega1:
				this.m_flameWalkProjectile = this.FireProjectile("FlamerLineBoltMinibossProjectile", 2, false, 0f, 1f, true, true, true);
				this.m_flameWalkProjectile2 = this.FireProjectile("FlamerLineBoltMinibossProjectile", 0, false, 180f, 1f, true, true, true);
				break;
			case Flamer_Basic_AIScript.FlameFiringState.Mega2:
				this.m_flameWalkProjectile = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, false, 70f, 1f, true, true, true);
				this.m_flameWalkProjectile2 = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, false, 110f, 1f, true, true, true);
				this.m_flameWalkProjectile3 = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, false, 90f, 1f, true, true, true);
				break;
			}
			this.m_wasFrozenWhileFiring = false;
		}
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x0001896C File Offset: 0x00016B6C
	public override void OnLBCompleteOrCancelled()
	{
		base.StopProjectile(ref this.m_flameTellProjectile);
		base.StopProjectile(ref this.m_flameWalkProjectile);
		base.StopProjectile(ref this.m_flameWalkProjectile2);
		base.StopProjectile(ref this.m_flameWalkProjectile3);
		this.m_wasFrozenWhileFiring = false;
		base.EnemyController.GroundHorizontalVelocity = 0f;
		base.OnLBCompleteOrCancelled();
		this.m_firingState = Flamer_Basic_AIScript.FlameFiringState.None;
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x000189CD File Offset: 0x00016BCD
	public override void ResetScript()
	{
		base.EnemyController.ControllerCorgi.GravityActive(true);
		base.EnemyController.DisableFriction = false;
		this.m_firingState = Flamer_Basic_AIScript.FlameFiringState.None;
		base.ResetScript();
	}

	// Token: 0x04000A00 RID: 2560
	protected const string FLAME_TELL_PROJECTILE = "FlamerWarningProjectile";

	// Token: 0x04000A01 RID: 2561
	protected Projectile_RL m_flameTellProjectile;

	// Token: 0x04000A02 RID: 2562
	protected Projectile_RL m_flameWalkProjectile;

	// Token: 0x04000A03 RID: 2563
	protected Projectile_RL m_flameWalkProjectile2;

	// Token: 0x04000A04 RID: 2564
	protected Projectile_RL m_flameWalkProjectile3;

	// Token: 0x04000A05 RID: 2565
	protected Flamer_Basic_AIScript.FlameFiringState m_firingState;

	// Token: 0x04000A06 RID: 2566
	protected WaitUntil m_waitUntilFallingYield;

	// Token: 0x04000A07 RID: 2567
	protected WaitRL_Yield m_loseGravityDurationYield;

	// Token: 0x04000A08 RID: 2568
	protected const string FLAMEWALK_TELL_INTRO = "Flame_Tell_Intro";

	// Token: 0x04000A09 RID: 2569
	protected const string FLAMEWALK_TELL_HOLD = "Flame_Tell_Hold";

	// Token: 0x04000A0A RID: 2570
	protected const string FLAMEWALK_ATTACK_INTRO = "Flame_Attack_Intro";

	// Token: 0x04000A0B RID: 2571
	protected const string FLAMEWALK_ATTACK_HOLD = "Flame_Attack_Hold";

	// Token: 0x04000A0C RID: 2572
	protected const string FLAMEWALK_EXIT = "Flame_Exit";

	// Token: 0x04000A0D RID: 2573
	protected const string FLAMEWALK_LINEBOLT_PROJECTILE = "FlamerLineBoltProjectile";

	// Token: 0x04000A0E RID: 2574
	protected float m_flameWalk_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000A0F RID: 2575
	protected float m_flameWalk_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000A10 RID: 2576
	protected float m_flameWalk_Tell_Delay = 1.15f;

	// Token: 0x04000A11 RID: 2577
	protected float m_flameWalk_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000A12 RID: 2578
	protected float m_flameWalk_AttackIntro_Delay;

	// Token: 0x04000A13 RID: 2579
	protected float m_flameWalk_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000A14 RID: 2580
	protected float m_flameWalk_AttackHold_Delay;

	// Token: 0x04000A15 RID: 2581
	protected float m_flameWalk_Exit_AnimationSpeed = 1f;

	// Token: 0x04000A16 RID: 2582
	protected float m_flameWalk_Exit_Delay;

	// Token: 0x04000A17 RID: 2583
	protected const string FLAME_BOLT_TELL_INTRO = "BigFlame_Tell_Intro";

	// Token: 0x04000A18 RID: 2584
	protected const string FLAME_BOLT_TELL_HOLD = "BigFlame_Tell_Hold";

	// Token: 0x04000A19 RID: 2585
	protected const string FLAME_BOLT_ATTACK_INTRO = "BigFlame_Attack_Intro";

	// Token: 0x04000A1A RID: 2586
	protected const string FLAME_BOLT_ATTACK_HOLD = "BigFlame_Attack_Hold";

	// Token: 0x04000A1B RID: 2587
	protected const string FLAME_BOLT_EXIT = "BigFlame_Exit";

	// Token: 0x04000A1C RID: 2588
	protected const string FLAME_BOLT_PROJECTILE = "FlamerBombProjectile";

	// Token: 0x04000A1D RID: 2589
	protected const string FLAME_BOLT_EXPLOSION_PROJECTILE = "FlamerBombExplosionProjectile";

	// Token: 0x04000A1E RID: 2590
	protected float m_flameBolt_TellIntro_AnimationSpeed = 1.15f;

	// Token: 0x04000A1F RID: 2591
	protected float m_flameBolt_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000A20 RID: 2592
	protected float m_flameBolt_Tell_Delay = 1.3f;

	// Token: 0x04000A21 RID: 2593
	protected float m_flameBolt_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000A22 RID: 2594
	protected float m_flameBolt_AttackIntro_Delay;

	// Token: 0x04000A23 RID: 2595
	protected float m_flameBolt_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000A24 RID: 2596
	protected float m_flameBolt_AttackHold_Delay;

	// Token: 0x04000A25 RID: 2597
	protected float m_flameBolt_Exit_AnimationSpeed = 1f;

	// Token: 0x04000A26 RID: 2598
	protected float m_flameBolt_Exit_Delay;

	// Token: 0x04000A27 RID: 2599
	protected const string BIGFLAME_TELL_INTRO = "BigFlame_Tell_Intro";

	// Token: 0x04000A28 RID: 2600
	protected const string BIGFLAME_TELL_HOLD = "BigFlame_Tell_Hold";

	// Token: 0x04000A29 RID: 2601
	protected const string BIGFLAME_ATTACK_INTRO = "BigFlame_Attack_Intro";

	// Token: 0x04000A2A RID: 2602
	protected const string BIGFLAME_ATTACK_HOLD = "BigFlame_Attack_Hold";

	// Token: 0x04000A2B RID: 2603
	protected const string BIGFLAME_EXIT = "BigFlame_Exit";

	// Token: 0x04000A2C RID: 2604
	protected const string BIGFLAME_PROJECTILE = "FlamerLineBoltMinibossProjectile";

	// Token: 0x04000A2D RID: 2605
	protected float m_megaFlame_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000A2E RID: 2606
	protected float m_megaFlame_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000A2F RID: 2607
	protected float m_megaFlame_Tell_Delay = 1.75f;

	// Token: 0x04000A30 RID: 2608
	protected float m_megaFlame_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000A31 RID: 2609
	protected float m_megaFlame_AttackIntro_Delay;

	// Token: 0x04000A32 RID: 2610
	protected float m_megaFlame_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000A33 RID: 2611
	protected float m_megaFlame_AttackHold_Delay;

	// Token: 0x04000A34 RID: 2612
	protected float m_megaFlame_Exit_AnimationSpeed = 1f;

	// Token: 0x04000A35 RID: 2613
	protected float m_megaFlame_Exit_Delay;

	// Token: 0x04000A36 RID: 2614
	private bool m_wasFrozenWhileFiring;

	// Token: 0x020009F2 RID: 2546
	protected enum FlameFiringState
	{
		// Token: 0x040046C7 RID: 18119
		None,
		// Token: 0x040046C8 RID: 18120
		Regular,
		// Token: 0x040046C9 RID: 18121
		Mega1,
		// Token: 0x040046CA RID: 18122
		Mega2
	}
}
