using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x0200009C RID: 156
public class AxeKnight_Basic_AIScript : BaseAIScript
{
	// Token: 0x0600022F RID: 559 RVA: 0x00003DBA File Offset: 0x00001FBA
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

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000230 RID: 560 RVA: 0x00003DE8 File Offset: 0x00001FE8
	protected override float WalkAnimSpeedMod
	{
		get
		{
			return 1.4f;
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06000231 RID: 561 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06000232 RID: 562 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x06000233 RID: 563 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x06000234 RID: 564 RVA: 0x00003E11 File Offset: 0x00002011
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		if (!this.m_axeSpinAudioEvent.isValid())
		{
			this.m_axeSpinAudioEvent = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_axeKnight_spin_loop", base.EnemyController.transform);
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x06000235 RID: 565 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_jump_spawnAxeOnLand
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x06000236 RID: 566 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_raiseKnockbackDefenseWhileAttacking
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06000237 RID: 567 RVA: 0x00003E42 File Offset: 0x00002042
	protected virtual int m_knockbackDefenseBoostOverride
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x06000238 RID: 568 RVA: 0x00003E45 File Offset: 0x00002045
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

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06000239 RID: 569 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_throwSecondAxe
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600023A RID: 570 RVA: 0x00003E54 File Offset: 0x00002054
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

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x0600023B RID: 571 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_throwSecondSpinAxe
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x0600023C RID: 572 RVA: 0x00003E63 File Offset: 0x00002063
	protected virtual float m_spinAttack_ChaseSpeed
	{
		get
		{
			return 11f;
		}
	}

	// Token: 0x0600023D RID: 573 RVA: 0x00003E6A File Offset: 0x0000206A
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

	// Token: 0x0600023E RID: 574 RVA: 0x00003E79 File Offset: 0x00002079
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

	// Token: 0x0600023F RID: 575 RVA: 0x00003E8F File Offset: 0x0000208F
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

	// Token: 0x06000240 RID: 576 RVA: 0x0004EC0C File Offset: 0x0004CE0C
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

	// Token: 0x06000241 RID: 577 RVA: 0x0004EC78 File Offset: 0x0004CE78
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

	// Token: 0x06000242 RID: 578 RVA: 0x0004ED18 File Offset: 0x0004CF18
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

	// Token: 0x06000243 RID: 579 RVA: 0x00003EA5 File Offset: 0x000020A5
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

	// Token: 0x06000244 RID: 580 RVA: 0x00003EDE File Offset: 0x000020DE
	private void OnDestroy()
	{
		if (this.m_axeSpinAudioEvent.isValid())
		{
			this.m_axeSpinAudioEvent.release();
		}
	}

	// Token: 0x0400064B RID: 1611
	private EventInstance m_axeSpinAudioEvent;

	// Token: 0x0400064C RID: 1612
	private const string JUMP_TELL_INTRO = "JumpSpin_Tell_Intro";

	// Token: 0x0400064D RID: 1613
	private const string JUMP_TELL_HOLD = "JumpSpin_Tell_Hold";

	// Token: 0x0400064E RID: 1614
	private const string JUMP_ATTACK_INTRO = "JumpSpin_Attack_Intro";

	// Token: 0x0400064F RID: 1615
	private const string JUMP_ATTACK_HOLD = "JumpSpin_Attack_Hold";

	// Token: 0x04000650 RID: 1616
	private const string JUMP_EXIT = "JumpSpin_Exit";

	// Token: 0x04000651 RID: 1617
	private const string JUMP_PROJECTILE_NAME = "AxeKnightSpinProjectile";

	// Token: 0x04000652 RID: 1618
	protected float m_jump_Tell_Delay = 0.4f;

	// Token: 0x04000653 RID: 1619
	protected float m_jump_Tell_AnimationSpeed = 0.75f;

	// Token: 0x04000654 RID: 1620
	protected float m_jump_AttackIntro_Delay;

	// Token: 0x04000655 RID: 1621
	protected float m_jump_AttackIntro_AnimationSpeed = 0.75f;

	// Token: 0x04000656 RID: 1622
	protected float m_jump_AttackHold_Delay;

	// Token: 0x04000657 RID: 1623
	protected float m_jump_AttackHold_AnimationSpeed = 0.75f;

	// Token: 0x04000658 RID: 1624
	protected float m_jump_Exit_Delay;

	// Token: 0x04000659 RID: 1625
	protected float m_jump_Exit_ForceIdle = 0.15f;

	// Token: 0x0400065A RID: 1626
	protected float m_jump_Exit_AttackCD;

	// Token: 0x0400065B RID: 1627
	protected Vector2 m_jump_Power = new Vector2(13f, 26f);

	// Token: 0x0400065C RID: 1628
	private Projectile_RL m_axeJumpProjectile;

	// Token: 0x0400065D RID: 1629
	private Projectile_RL m_axeSpinAttackProjectile;

	// Token: 0x0400065E RID: 1630
	private const string AXE_TELL_INTRO = "AxeThrow_Tell_Intro";

	// Token: 0x0400065F RID: 1631
	private const string AXE_TELL_HOLD = "AxeThrow_Tell_Hold";

	// Token: 0x04000660 RID: 1632
	private const string AXE_ATTACK_INTRO = "AxeThrow_Attack_Intro";

	// Token: 0x04000661 RID: 1633
	private const string AXE_ATTACK_HOLD = "AxeThrow_Attack_Hold";

	// Token: 0x04000662 RID: 1634
	private const string AXE_EXIT = "AxeThrow_Exit";

	// Token: 0x04000663 RID: 1635
	private const string AXE_PROJECTILE_NAME = "AxeKnightBoltProjectile";

	// Token: 0x04000664 RID: 1636
	protected float m_throwAxe_TellIntro_AnimationSpeed = 0.75f;

	// Token: 0x04000665 RID: 1637
	protected float m_throwAxe_TellHold_AnimationSpeed = 1.35f;

	// Token: 0x04000666 RID: 1638
	protected const float m_throwAxe_TellIntroAndHold_Delay = 0.85f;

	// Token: 0x04000667 RID: 1639
	protected float m_throwAxe_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x04000668 RID: 1640
	protected float m_throwAxe_AttackIntro_Delay;

	// Token: 0x04000669 RID: 1641
	protected float m_throwAxe_AttackHold_AnimationSpeed = 1.1f;

	// Token: 0x0400066A RID: 1642
	protected float m_throwAxe_AttackHold_Delay = 1.25f;

	// Token: 0x0400066B RID: 1643
	protected float m_throwAxe_Exit_AnimationSpeed = 1f;

	// Token: 0x0400066C RID: 1644
	protected float m_throwAxe_Exit_Delay;

	// Token: 0x0400066D RID: 1645
	protected float m_throwAxe_Exit_ForceIdle = 0.15f;

	// Token: 0x0400066E RID: 1646
	protected float m_throwAxe_Exit_AttackCD;

	// Token: 0x0400066F RID: 1647
	protected int m_throwAxe_Angle;

	// Token: 0x04000670 RID: 1648
	protected int m_throwAxe_Angle2 = 10;

	// Token: 0x04000671 RID: 1649
	private const string SPIN_ATTACK_TELL_INTRO = "BodySpin_Tell_Intro";

	// Token: 0x04000672 RID: 1650
	private const string SPIN_ATTACK_TELL_HOLD = "BodySpin_Tell_Hold";

	// Token: 0x04000673 RID: 1651
	private const string SPIN_ATTACK_ATTACK_INTRO = "BodySpin_Attack_Intro";

	// Token: 0x04000674 RID: 1652
	private const string SPIN_ATTACK_ATTACK_HOLD = "BodySpin_Attack_Hold";

	// Token: 0x04000675 RID: 1653
	private const string SPIN_ATTACK_EXIT = "BodySpin_Exit";

	// Token: 0x04000676 RID: 1654
	private const string SPIN_ATTACK_PROJECTILE_NAME = "AxeKnightWhirlingProjectile";

	// Token: 0x04000677 RID: 1655
	private const string SPIN_ATTACK_LAUNCHER_PROJECTILE_NAME = "AxeKnightWhirlingLauncherProjectile";

	// Token: 0x04000678 RID: 1656
	protected float m_spinAttack_TellIntro_AnimationSpeed = 0.75f;

	// Token: 0x04000679 RID: 1657
	protected float m_spinAttack_TellHold_AnimationSpeed = 1.35f;

	// Token: 0x0400067A RID: 1658
	protected const float m_spinAttack_TellIntroAndHold_Delay = 1.1f;

	// Token: 0x0400067B RID: 1659
	protected float m_spinAttack_AttackIntro_AnimationSpeed = 1.1f;

	// Token: 0x0400067C RID: 1660
	protected float m_spinAttack_AttackIntro_Delay;

	// Token: 0x0400067D RID: 1661
	protected float m_spinAttack_AttackHold_AnimationSpeed = 2f;

	// Token: 0x0400067E RID: 1662
	protected float m_spinAttack_AttackHold_Delay = 3f;

	// Token: 0x0400067F RID: 1663
	protected float m_spinAttack_Exit_AnimationSpeed = 1f;

	// Token: 0x04000680 RID: 1664
	protected float m_spinAttack_Exit_Delay;

	// Token: 0x04000681 RID: 1665
	protected float m_spinAttack_Exit_ForceIdle = 0.15f;

	// Token: 0x04000682 RID: 1666
	protected float m_spinAttack_Exit_AttackCD;

	// Token: 0x04000683 RID: 1667
	protected int m_spinAttack_Angle = 90;

	// Token: 0x04000684 RID: 1668
	protected int m_spinAttack_Angle2 = 10;

	// Token: 0x04000685 RID: 1669
	protected float m_spinAttack_Acceleration = 0.08f;

	// Token: 0x04000686 RID: 1670
	private Coroutine m_chasePlayerPersistentCoroutine;

	// Token: 0x04000687 RID: 1671
	private Coroutine m_spinAttackBlinkCoroutine;

	// Token: 0x04000688 RID: 1672
	private bool m_isJumpAttacking;

	// Token: 0x04000689 RID: 1673
	private bool m_isSpinAttacking;
}
