using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x02000090 RID: 144
public class AxeKnight_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000203 RID: 515 RVA: 0x00012121 File Offset: 0x00010321
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"AxeKnightSpinProjectile",
			"AxeKnightBoltProjectile",
			"AxeKnightWhirlingProjectile",
			"AxeKnightWhirlingLauncherProjectile"
		};
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000204 RID: 516 RVA: 0x0001214F File Offset: 0x0001034F
	protected override float WalkAnimSpeedMod
	{
		get
		{
			return 1.4f;
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06000205 RID: 517 RVA: 0x00012156 File Offset: 0x00010356
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000206 RID: 518 RVA: 0x00012167 File Offset: 0x00010367
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000207 RID: 519 RVA: 0x00012178 File Offset: 0x00010378
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x06000208 RID: 520 RVA: 0x00012189 File Offset: 0x00010389
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		if (!this.m_axeSpinAudioEvent.isValid())
		{
			this.m_axeSpinAudioEvent = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_axeKnight_spin_loop", base.EnemyController.transform);
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000209 RID: 521 RVA: 0x000121BA File Offset: 0x000103BA
	protected virtual bool m_jump_spawnAxeOnLand
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x0600020A RID: 522 RVA: 0x000121BD File Offset: 0x000103BD
	protected virtual bool m_raiseKnockbackDefenseWhileAttacking
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x0600020B RID: 523 RVA: 0x000121C0 File Offset: 0x000103C0
	protected virtual int m_knockbackDefenseBoostOverride
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x0600020C RID: 524 RVA: 0x000121C3 File Offset: 0x000103C3
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Jump_Attack()
	{
		if (this.m_raiseKnockbackDefenseWhileAttacking)
		{
			base.EnemyController.BaseKnockbackDefense = (float)this.m_knockbackDefenseBoostOverride;
		}
		yield return base.WaitUntilIsGrounded();
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_Animation("JumpSpin_Tell_Intro", this.m_jump_Tell_AnimationSpeed, this.m_jump_Tell_Delay, true);
		base.SetAttackingWithContactDamage(true, 0.1f);
		if (base.EnemyController.IsFacingRight)
		{
			base.SetVelocityX(this.m_jump_Power.x, true);
		}
		else
		{
			base.SetVelocityX(-this.m_jump_Power.x, true);
		}
		base.SetVelocityY(this.m_jump_Power.y, false);
		yield return this.Default_Animation("JumpSpin_Attack_Intro", this.m_jump_AttackIntro_AnimationSpeed, this.m_jump_AttackIntro_Delay, true);
		yield return this.ChangeAnimationState("JumpSpin_Attack_Hold");
		yield return base.Wait(0.05f, false);
		this.m_axeJumpProjectile = this.FireProjectile("AxeKnightSpinProjectile", 1, false, 0f, 1f, true, true, true);
		yield return base.WaitUntilIsGrounded();
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.SetVelocity(0f, 0f, false);
		if (this.m_jump_spawnAxeOnLand)
		{
			this.FireProjectile("AxeKnightBoltProjectile", 1, false, (float)this.m_throwAxe_Angle, 1f, true, true, true);
			this.FireProjectile("AxeKnightBoltProjectile", 1, false, 180f, 1f, true, true, true);
		}
		base.StopProjectile(ref this.m_axeJumpProjectile);
		yield return this.Default_Animation("JumpSpin_Exit", this.m_jump_Tell_AnimationSpeed, this.m_jump_Tell_Delay, true);
		if (this.m_jump_Exit_Delay > 0f)
		{
			base.Wait(this.m_jump_Exit_Delay, false);
		}
		base.EnemyController.AlwaysFacing = true;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_jump_Exit_ForceIdle, this.m_jump_Exit_AttackCD);
		yield break;
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x0600020D RID: 525 RVA: 0x000121D2 File Offset: 0x000103D2
	protected virtual bool m_throwSecondAxe
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600020E RID: 526 RVA: 0x000121D5 File Offset: 0x000103D5
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Throw_Axe_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_TellIntroAndLoop("AxeThrow_Tell_Intro", this.m_throwAxe_TellIntro_AnimationSpeed, "AxeThrow_Tell_Hold", this.m_throwAxe_TellHold_AnimationSpeed, 0.85f);
		yield return this.Default_Animation("AxeThrow_Attack_Intro", this.m_throwAxe_AttackIntro_AnimationSpeed, this.m_throwAxe_AttackIntro_Delay, true);
		yield return this.Default_Animation("AxeThrow_Attack_Hold", this.m_throwAxe_AttackHold_AnimationSpeed, 0f, false);
		float num = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
		if (!base.EnemyController.IsFacingRight)
		{
			num = CDGHelper.WrapAngleDegrees(-180f - num, true);
		}
		this.FireProjectile("AxeKnightBoltProjectile", 1, true, num + (float)this.m_throwAxe_Angle, 1f, true, true, true);
		if (this.m_throwSecondAxe)
		{
			this.FireProjectile("AxeKnightBoltProjectile", 1, true, num + (float)this.m_throwAxe_Angle2, 1.25f, true, true, true);
		}
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return base.Wait(this.m_throwAxe_AttackHold_Delay, false);
		yield return this.Default_Animation("AxeThrow_Exit", this.m_throwAxe_Exit_AnimationSpeed, this.m_throwAxe_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_throwAxe_Exit_ForceIdle, this.m_throwAxe_Exit_AttackCD);
		yield break;
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x0600020F RID: 527 RVA: 0x000121E4 File Offset: 0x000103E4
	protected virtual bool m_throwSecondSpinAxe
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x06000210 RID: 528 RVA: 0x000121E7 File Offset: 0x000103E7
	protected virtual float m_spinAttack_ChaseSpeed
	{
		get
		{
			return 11f;
		}
	}

	// Token: 0x06000211 RID: 529 RVA: 0x000121EE File Offset: 0x000103EE
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator SpinAttack_Attack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_TellIntroAndLoop("BodySpin_Tell_Intro", this.m_spinAttack_TellIntro_AnimationSpeed, "BodySpin_Tell_Hold", this.m_spinAttack_TellHold_AnimationSpeed, 1.1f);
		base.SetAttackingWithContactDamage(true, 0.1f);
		if (this.m_axeSpinAudioEvent.isValid())
		{
			AudioManager.PlayAttached(null, this.m_axeSpinAudioEvent, base.EnemyController.gameObject);
		}
		yield return this.Default_Animation("BodySpin_Attack_Intro", this.m_spinAttack_AttackIntro_AnimationSpeed, this.m_spinAttack_AttackIntro_Delay, true);
		yield return this.Default_Animation("BodySpin_Attack_Hold", this.m_spinAttack_AttackHold_AnimationSpeed, 0f, false);
		this.StopPersistentCoroutine(this.m_chasePlayerPersistentCoroutine);
		this.m_chasePlayerPersistentCoroutine = this.RunPersistentCoroutine(this.ChasePlayerPersistentCoroutine(this.m_spinAttack_ChaseSpeed));
		this.m_axeSpinAttackProjectile = this.FireProjectile("AxeKnightWhirlingProjectile", 3, false, 0f, 1f, true, true, true);
		yield return base.Wait(this.m_spinAttack_AttackHold_Delay - 1f, false);
		Color red = Color.red;
		red.a = 0.5f;
		this.m_spinAttackBlinkCoroutine = this.RunPersistentCoroutine(this.SpinAttackBlinkCoroutine(1f, red));
		yield return base.Wait(1f, false);
		this.StopPersistentCoroutine(this.m_spinAttackBlinkCoroutine);
		base.SetAttackingWithContactDamage(false, 0.1f);
		base.StopProjectile(ref this.m_axeSpinAttackProjectile);
		this.FireProjectile("AxeKnightWhirlingLauncherProjectile", 1, true, (float)this.m_spinAttack_Angle, 1f, true, true, true);
		if (this.m_throwSecondSpinAxe)
		{
			this.FireProjectile("AxeKnightWhirlingLauncherProjectile", 1, true, (float)this.m_spinAttack_Angle2, 1.25f, true, true, true);
		}
		base.EnemyController.ResetTurnTrigger();
		base.EnemyController.ResetBaseValues();
		base.EnemyController.AlwaysFacing = true;
		this.StopPersistentCoroutine(this.m_chasePlayerPersistentCoroutine);
		if (this.m_axeSpinAudioEvent.isValid())
		{
			AudioManager.Stop(this.m_axeSpinAudioEvent, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		yield return this.Default_Animation("BodySpin_Exit", this.m_spinAttack_Exit_AnimationSpeed, this.m_spinAttack_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_spinAttack_Exit_ForceIdle, this.m_spinAttack_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000212 RID: 530 RVA: 0x000121FD File Offset: 0x000103FD
	private IEnumerator ChasePlayerPersistentCoroutine(float speed)
	{
		float currentSpeed = 0f;
		if (base.EnemyController.IsTargetToMyRight)
		{
			currentSpeed = speed;
		}
		else
		{
			currentSpeed = -speed;
		}
		for (;;)
		{
			if (!base.IsPaused)
			{
				if (base.EnemyController.IsTargetToMyRight)
				{
					currentSpeed = Mathf.Min(speed, currentSpeed + this.m_spinAttack_Acceleration);
				}
				else
				{
					currentSpeed = Mathf.Max(-speed, currentSpeed - this.m_spinAttack_Acceleration);
				}
			}
			else
			{
				currentSpeed = 0f;
			}
			base.EnemyController.SetVelocityX(currentSpeed, false);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000213 RID: 531 RVA: 0x00012213 File Offset: 0x00010413
	private IEnumerator SpinAttackBlinkCoroutine(float duration, Color color)
	{
		for (;;)
		{
			float blinkDuration = Time.time + base.EnemyController.BlinkPulseEffect.SingleBlinkDuration * 2f;
			base.EnemyController.BlinkPulseEffect.StartSingleBlinkEffect(color);
			while (Time.time < blinkDuration)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06000214 RID: 532 RVA: 0x0001222C File Offset: 0x0001042C
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		this.StopPersistentCoroutine(this.m_chasePlayerPersistentCoroutine);
		this.StopPersistentCoroutine(this.m_spinAttackBlinkCoroutine);
		base.StopProjectile(ref this.m_axeJumpProjectile);
		base.StopProjectile(ref this.m_axeSpinAttackProjectile);
		this.m_isJumpAttacking = false;
		this.m_isSpinAttacking = false;
		if (this.m_axeSpinAudioEvent.isValid())
		{
			AudioManager.Stop(this.m_axeSpinAudioEvent, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06000215 RID: 533 RVA: 0x00012298 File Offset: 0x00010498
	public override void Pause()
	{
		base.Pause();
		if (this.m_axeJumpProjectile && this.m_axeJumpProjectile.isActiveAndEnabled && this.m_axeJumpProjectile.OwnerController == base.EnemyController)
		{
			this.m_isJumpAttacking = true;
			base.StopProjectile(ref this.m_axeJumpProjectile);
		}
		if (this.m_axeSpinAttackProjectile && this.m_axeSpinAttackProjectile.isActiveAndEnabled && this.m_axeSpinAttackProjectile.OwnerController == base.EnemyController)
		{
			this.m_isSpinAttacking = true;
			base.StopProjectile(ref this.m_axeSpinAttackProjectile);
		}
	}

	// Token: 0x06000216 RID: 534 RVA: 0x00012338 File Offset: 0x00010538
	public override void Unpause()
	{
		base.Unpause();
		if (this.m_isSpinAttacking)
		{
			this.m_axeSpinAttackProjectile = this.FireProjectile("AxeKnightWhirlingProjectile", 3, false, 0f, 1f, true, true, true);
			this.m_isSpinAttacking = false;
		}
		if (this.m_isJumpAttacking)
		{
			this.m_axeJumpProjectile = this.FireProjectile("AxeKnightSpinProjectile", 1, false, 0f, 1f, true, true, true);
			this.m_isJumpAttacking = false;
		}
	}

	// Token: 0x06000217 RID: 535 RVA: 0x000123A9 File Offset: 0x000105A9
	protected override void OnDisable()
	{
		base.OnDisable();
		base.StopProjectile(ref this.m_axeJumpProjectile);
		base.StopProjectile(ref this.m_axeSpinAttackProjectile);
		if (this.m_axeSpinAudioEvent.isValid())
		{
			AudioManager.Stop(this.m_axeSpinAudioEvent, FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
	}

	// Token: 0x06000218 RID: 536 RVA: 0x000123E2 File Offset: 0x000105E2
	private void OnDestroy()
	{
		if (this.m_axeSpinAudioEvent.isValid())
		{
			this.m_axeSpinAudioEvent.release();
		}
	}

	// Token: 0x0400061D RID: 1565
	private EventInstance m_axeSpinAudioEvent;

	// Token: 0x0400061E RID: 1566
	private const string JUMP_TELL_INTRO = "JumpSpin_Tell_Intro";

	// Token: 0x0400061F RID: 1567
	private const string JUMP_TELL_HOLD = "JumpSpin_Tell_Hold";

	// Token: 0x04000620 RID: 1568
	private const string JUMP_ATTACK_INTRO = "JumpSpin_Attack_Intro";

	// Token: 0x04000621 RID: 1569
	private const string JUMP_ATTACK_HOLD = "JumpSpin_Attack_Hold";

	// Token: 0x04000622 RID: 1570
	private const string JUMP_EXIT = "JumpSpin_Exit";

	// Token: 0x04000623 RID: 1571
	private const string JUMP_PROJECTILE_NAME = "AxeKnightSpinProjectile";

	// Token: 0x04000624 RID: 1572
	protected float m_jump_Tell_Delay = 0.4f;

	// Token: 0x04000625 RID: 1573
	protected float m_jump_Tell_AnimationSpeed = 0.75f;

	// Token: 0x04000626 RID: 1574
	protected float m_jump_AttackIntro_Delay;

	// Token: 0x04000627 RID: 1575
	protected float m_jump_AttackIntro_AnimationSpeed = 0.75f;

	// Token: 0x04000628 RID: 1576
	protected float m_jump_AttackHold_Delay;

	// Token: 0x04000629 RID: 1577
	protected float m_jump_AttackHold_AnimationSpeed = 0.75f;

	// Token: 0x0400062A RID: 1578
	protected float m_jump_Exit_Delay;

	// Token: 0x0400062B RID: 1579
	protected float m_jump_Exit_ForceIdle = 0.15f;

	// Token: 0x0400062C RID: 1580
	protected float m_jump_Exit_AttackCD;

	// Token: 0x0400062D RID: 1581
	protected Vector2 m_jump_Power = new Vector2(13f, 26f);

	// Token: 0x0400062E RID: 1582
	private Projectile_RL m_axeJumpProjectile;

	// Token: 0x0400062F RID: 1583
	private Projectile_RL m_axeSpinAttackProjectile;

	// Token: 0x04000630 RID: 1584
	private const string AXE_TELL_INTRO = "AxeThrow_Tell_Intro";

	// Token: 0x04000631 RID: 1585
	private const string AXE_TELL_HOLD = "AxeThrow_Tell_Hold";

	// Token: 0x04000632 RID: 1586
	private const string AXE_ATTACK_INTRO = "AxeThrow_Attack_Intro";

	// Token: 0x04000633 RID: 1587
	private const string AXE_ATTACK_HOLD = "AxeThrow_Attack_Hold";

	// Token: 0x04000634 RID: 1588
	private const string AXE_EXIT = "AxeThrow_Exit";

	// Token: 0x04000635 RID: 1589
	private const string AXE_PROJECTILE_NAME = "AxeKnightBoltProjectile";

	// Token: 0x04000636 RID: 1590
	protected float m_throwAxe_TellIntro_AnimationSpeed = 0.75f;

	// Token: 0x04000637 RID: 1591
	protected float m_throwAxe_TellHold_AnimationSpeed = 1.35f;

	// Token: 0x04000638 RID: 1592
	protected const float m_throwAxe_TellIntroAndHold_Delay = 0.85f;

	// Token: 0x04000639 RID: 1593
	protected float m_throwAxe_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x0400063A RID: 1594
	protected float m_throwAxe_AttackIntro_Delay;

	// Token: 0x0400063B RID: 1595
	protected float m_throwAxe_AttackHold_AnimationSpeed = 1.1f;

	// Token: 0x0400063C RID: 1596
	protected float m_throwAxe_AttackHold_Delay = 1.25f;

	// Token: 0x0400063D RID: 1597
	protected float m_throwAxe_Exit_AnimationSpeed = 1f;

	// Token: 0x0400063E RID: 1598
	protected float m_throwAxe_Exit_Delay;

	// Token: 0x0400063F RID: 1599
	protected float m_throwAxe_Exit_ForceIdle = 0.15f;

	// Token: 0x04000640 RID: 1600
	protected float m_throwAxe_Exit_AttackCD;

	// Token: 0x04000641 RID: 1601
	protected int m_throwAxe_Angle;

	// Token: 0x04000642 RID: 1602
	protected int m_throwAxe_Angle2 = 10;

	// Token: 0x04000643 RID: 1603
	private const string SPIN_ATTACK_TELL_INTRO = "BodySpin_Tell_Intro";

	// Token: 0x04000644 RID: 1604
	private const string SPIN_ATTACK_TELL_HOLD = "BodySpin_Tell_Hold";

	// Token: 0x04000645 RID: 1605
	private const string SPIN_ATTACK_ATTACK_INTRO = "BodySpin_Attack_Intro";

	// Token: 0x04000646 RID: 1606
	private const string SPIN_ATTACK_ATTACK_HOLD = "BodySpin_Attack_Hold";

	// Token: 0x04000647 RID: 1607
	private const string SPIN_ATTACK_EXIT = "BodySpin_Exit";

	// Token: 0x04000648 RID: 1608
	private const string SPIN_ATTACK_PROJECTILE_NAME = "AxeKnightWhirlingProjectile";

	// Token: 0x04000649 RID: 1609
	private const string SPIN_ATTACK_LAUNCHER_PROJECTILE_NAME = "AxeKnightWhirlingLauncherProjectile";

	// Token: 0x0400064A RID: 1610
	protected float m_spinAttack_TellIntro_AnimationSpeed = 0.75f;

	// Token: 0x0400064B RID: 1611
	protected float m_spinAttack_TellHold_AnimationSpeed = 1.35f;

	// Token: 0x0400064C RID: 1612
	protected const float m_spinAttack_TellIntroAndHold_Delay = 1.1f;

	// Token: 0x0400064D RID: 1613
	protected float m_spinAttack_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x0400064E RID: 1614
	protected float m_spinAttack_AttackIntro_Delay;

	// Token: 0x0400064F RID: 1615
	protected float m_spinAttack_AttackHold_AnimationSpeed = 2f;

	// Token: 0x04000650 RID: 1616
	protected float m_spinAttack_AttackHold_Delay = 3f;

	// Token: 0x04000651 RID: 1617
	protected float m_spinAttack_Exit_AnimationSpeed = 1f;

	// Token: 0x04000652 RID: 1618
	protected float m_spinAttack_Exit_Delay;

	// Token: 0x04000653 RID: 1619
	protected float m_spinAttack_Exit_ForceIdle = 0.15f;

	// Token: 0x04000654 RID: 1620
	protected float m_spinAttack_Exit_AttackCD;

	// Token: 0x04000655 RID: 1621
	protected int m_spinAttack_Angle = 90;

	// Token: 0x04000656 RID: 1622
	protected int m_spinAttack_Angle2 = 10;

	// Token: 0x04000657 RID: 1623
	protected float m_spinAttack_Acceleration = 0.08f;

	// Token: 0x04000658 RID: 1624
	private Coroutine m_chasePlayerPersistentCoroutine;

	// Token: 0x04000659 RID: 1625
	private Coroutine m_spinAttackBlinkCoroutine;

	// Token: 0x0400065A RID: 1626
	private bool m_isJumpAttacking;

	// Token: 0x0400065B RID: 1627
	private bool m_isSpinAttacking;
}
